using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data.Repositories;
using WebApi.Helpers;
using WebApi.Models;
using WebApi.Models.Dtos;

namespace WebApi.Services
{
  public interface IUploaderService
  {
    Task<UploadResultDto> UploadImage(List<IFormFile> files, bool isTransformation);
    Task<UploadResultDto> UploadFile(List<IFormFile> files, string type);
    Task<FunctionResult> UploadFileToServerAsync(UploadFileInfoDto uploadFileInfo);
    Task<string> DeleteImage(string publicId);
    Task<FunctionResult> UploadImageToServerAsync(UploadFileInfoDto uploadFileInfo, bool isTransformation);
  }

  public class UploaderService : IUploaderService
  {
    private readonly Cloudinary _cloudinary;
    IOptions<CloudinarySettings> _cloudinaryConfig;
    ILogger<UploaderService> _logger;
  
    private readonly Helpers.UploadSettings _uploadSettings;
    private readonly IOrganizeRepository _organzieRepository;

    public UploaderService(IOptions<CloudinarySettings> cloudinaryConfig, ILogger<UploaderService> logger, IOrganizeRepository organzieRepository, IOptions<Helpers.UploadSettings> uploadSettings)
    {
      _logger = logger;
     
      _cloudinaryConfig = cloudinaryConfig;
      _organzieRepository=organzieRepository;
      _uploadSettings= uploadSettings.Value;
      Account acc = new Account(
        _cloudinaryConfig.Value.CloudName,
        _cloudinaryConfig.Value.ApiKey,
        _cloudinaryConfig.Value.ApiSecret
        );
      _cloudinary = new Cloudinary(acc);
    }

    public async Task<string> DeleteImage(string publicId)
    {
      var result = await _cloudinary.DestroyAsync(new DeletionParams(publicId));
      _logger.LogInformation(result.Result.ToString());
      return "Xóa ảnh thành công";
    }

    public async Task<UploadResultDto> UploadImage(List<IFormFile> files, bool isTransformation)
    {
      var uploadResult = new ImageUploadResult();
      if (files.Any() && files[0].Length > 0)
      {
        var file = files[0];
        using (var stream = file.OpenReadStream())
        {
          var uploadParams = new ImageUploadParams()
          {
            File = new FileDescription(file.Name, stream),
            Folder = "project_schedule",
            UseFilename = true,
          };

          if (isTransformation)
          {
            uploadParams.Transformation = new Transformation().Width(500).Crop("fill").Gravity("face");
          }

          uploadResult = await _cloudinary.UploadAsync(uploadParams);
        }
      }

      return new UploadResultDto()
      {
        PublicId = uploadResult != null ? uploadResult.PublicId : string.Empty,
        Url = uploadResult != null ? uploadResult.Url.ToString() : string.Empty
      };
    }

    public async Task<UploadResultDto> UploadFile(List<IFormFile> files, string type)
    {
      var uploadResult = new RawUploadResult();
      if (files.Any() && files[0].Length > 0)
      {
        var file = files[0];
        using (var stream = file.OpenReadStream())
        {
          var uploadParams = new RawUploadParams()
          {
            File = new FileDescription(file.Name, stream),
            Folder = "project_schedule",
            UseFilename = true,
            PublicId = file.FileName
          };
          var fileType = string.IsNullOrEmpty(type) ? "auto" : type;
          uploadResult = await _cloudinary.UploadAsync(uploadParams, fileType);
        }
      }

      return new UploadResultDto()
      {
        PublicId = uploadResult != null ? uploadResult.PublicId : string.Empty,
        Url = uploadResult.Url != null ? uploadResult.Url.ToString() : string.Empty
      };
    }

    public async Task<FunctionResult> UploadFileToServerAsync(UploadFileInfoDto uploadFileInfo)
    {
      var uploadResult = new RawUploadResult();
      var uploadPath = $"{Directory.GetCurrentDirectory()}/Upload";
      var uploadAliasPath = "";
      var result = CheckValidationUploadFile(uploadFileInfo);
     
      if (result.IsSuccess==false) return result;

      var organizeInfo = await _organzieRepository.GetSingleById(uploadFileInfo.OrganizeId.Value);
      if (organizeInfo == null)
      {
        result.AddError("Đơn vị này không tồn tại");
        return result;
      }
      var file = uploadFileInfo.FileUpload[0];
    //  var fileNameKhongDau= Ultil.ChuyenCoDauThanhKhongDau(file.FileName);
      var fileNameKhongDau= file.FileName;
      using (var stream = file.OpenReadStream())
      {
        var uploadParams = new RawUploadParams()
        {
          File = new FileDescription(file.Name, stream),
          Folder = uploadPath,
          UseFilename = true,
          PublicId = fileNameKhongDau
        };
        var modeUpload = string.IsNullOrEmpty(uploadFileInfo.Mode) ? "auto" : uploadFileInfo.Mode;
        string datetimeNow = DateTime.Now.ToString("ddMMyyyyHHmmssfff");
        switch (modeUpload)
        {
          case "Schedule":
            uploadPath+=$"/{ nameof(_uploadSettings.Schedule)}/{organizeInfo.CodeName}/{DateTime.Now.Year}/{DateTime.Now.Month}/{DateTime.Now.Day}";
            uploadAliasPath+=$"{ _uploadSettings.Schedule}/{organizeInfo.CodeName}/{DateTime.Now.Year}/{DateTime.Now.Month}/{DateTime.Now.Day}";
          
            System.IO.Directory.CreateDirectory(uploadPath);
            uploadPath+=$"/{datetimeNow}_{fileNameKhongDau}";
            uploadAliasPath+=$"/{datetimeNow}_{fileNameKhongDau}";
            using (FileStream outputFileStream = new FileStream(uploadPath, FileMode.Create))
            {
              stream.CopyTo(outputFileStream);
            }
            result.SetData(uploadAliasPath);
            break;
          case "EmailTemplate":
            uploadPath+=$"/{nameof(_uploadSettings.EmailTemplate)}/{organizeInfo.CodeName}/{DateTime.Now.Year}/{DateTime.Now.Month}/{DateTime.Now.Day}";
            uploadAliasPath+=$"{_uploadSettings.EmailTemplate}/{organizeInfo.CodeName}/{DateTime.Now.Year}/{DateTime.Now.Month}/{DateTime.Now.Day}";
            System.IO.Directory.CreateDirectory(uploadPath);
            uploadPath+=$"/{datetimeNow}_{fileNameKhongDau}";
            uploadAliasPath+=$"/{datetimeNow}_{fileNameKhongDau}";
            using (FileStream outputFileStream = new FileStream(uploadPath, FileMode.Create))
            {
              stream.CopyTo(outputFileStream);
            }
            result.SetData(uploadAliasPath);
            break;
          case "PersonalSchedule":
            uploadPath+=$"/PersonalSchedule/{organizeInfo.CodeName}/{DateTime.Now.Year}/{DateTime.Now.Month}/{DateTime.Now.Day}";
            System.IO.Directory.CreateDirectory(uploadPath);
            uploadPath+=$"/{datetimeNow}_{fileNameKhongDau}";
            using (FileStream outputFileStream = new FileStream(uploadPath, FileMode.Create))
            {
              stream.CopyTo(outputFileStream);
            }
            result.SetData(uploadPath);
            break;
          default:
            break;
        }
      }
      return result;
    }

    public async Task<FunctionResult> UploadImageToServerAsync(UploadFileInfoDto uploadFileInfo, bool isTransformation)
    {
      var uploadResult = new RawUploadResult();
      var uploadPath = $"{Directory.GetCurrentDirectory()}/Upload";
      var uploadAliasPath = "Upload";
      var result = CheckValidationUploadFile(uploadFileInfo);

      if (result.IsSuccess==false) return await Task.Run(() => result);

      //var organizeInfo = await _organzieRepository.GetSingleById(uploadFileInfo.OrganizeId.Value);
      //if (organizeInfo == null)
      //{
      //  result.AddError("Đơn vị này không tồn tại");
      //  return result;
      //}
      var file = uploadFileInfo.FileUpload[0];
     // var fileNameKhongDau = Ultil.ChuyenCoDauThanhKhongDau(file.FileName);
      var fileNameKhongDau = file.FileName;
      using (var stream = file.OpenReadStream())
      {
        var uploadParams = new ImageUploadParams()
        {
          File = new FileDescription(file.Name, stream),
          Folder = uploadPath,
          UseFilename = true,
          PublicId = fileNameKhongDau
        };
        var modeUpload = string.IsNullOrEmpty(uploadFileInfo.Mode) ? "auto" : uploadFileInfo.Mode;
        try
        {

          string datetimeNow = DateTime.Now.ToString("ddMMyyyyHHmmssfff");
          switch (modeUpload)
          {
            case "Setting":
              // uploadPath+=$"/Setting/{organizeInfo.CodeName}/{DateTime.Now.Year}/{DateTime.Now.Month}/{DateTime.Now.Day}";
              uploadPath+=$"/Setting";
              uploadAliasPath+=$"/Setting";

              System.IO.Directory.CreateDirectory(uploadPath);
              uploadPath+=$"/{datetimeNow}_{fileNameKhongDau}";
              uploadAliasPath+=$"/{datetimeNow}_{fileNameKhongDau}";

              if (isTransformation)
              {
                uploadParams.Transformation = new Transformation().Width(500).Crop("fill").Gravity("face");
              }

              var imageFromStream = new Bitmap(stream);
              var ratio = (double)imageFromStream.Width/ (double)imageFromStream.Height;
              var formatRaw = imageFromStream.RawFormat;
              var newHeight = Convert.ToInt32(500/ratio);
              var resizeStream = new Bitmap(imageFromStream, new System.Drawing.Size(500, newHeight));
              resizeStream.Save(uploadPath, formatRaw);
              //using (FileStream outputFileStream = new FileStream(uploadPath, FileMode.Create))
              //{
              //  stream.CopyTo(outputFileStream);

              //}

              result.SetData(uploadAliasPath);
              break;
            default:
              break;
          }
        }
        catch (Exception ex)
        {
          result.AddError(ex.ToString());
        }
      }
      return await Task.Run(()=> result);
    }

    private FunctionResult CheckValidationUploadFile(UploadFileInfoDto uploadFileInfo)
    {
      var result = new FunctionResult();
      if (uploadFileInfo.FileUpload.Any() ==false || uploadFileInfo.FileUpload[0].Length <= 0) result.AddError("Không có file nào được tải lên");
      if (uploadFileInfo.OrganizeId <=0) result.AddError("Yêu cầu mã đơn vị cho việc upload file");
      return result;
    }
  }
}
