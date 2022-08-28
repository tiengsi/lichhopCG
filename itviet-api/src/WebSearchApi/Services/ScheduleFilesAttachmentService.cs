using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data.Infrastructure;
using WebApi.Data.Repositories;
using WebApi.Helpers;
using WebApi.Models;
using WebApi.Models.Dtos;

namespace WebApi.Services
{
  public interface IScheduleFilesAttachmentService
  {
    Task<List<ScheduleFilesAttachmentDto>> getAllFilesAttachmentByScheduleId(int scheduleId);
    Task<List<ScheduleFilesAttachmentDto>> getAllFilesAttachmentByScheduleIdAsync_Ex(int scheduleId);
    Task<List<ScheduleFileAttachmentShareDto>> GetAllFilesAttachmentForShareByScheduleIdAsync(int scheduleId, string mode);

    void CreateFileAttachment(List<ScheduleFilesAttachmentDto> filesAttachment, int scheduleId);

    Task Update(List<ScheduleFilesAttachmentDto> filesAttachment, int scheduleId);
    Task<bool> DeleteFileAttachmentByIdAsync(int fileAttachmentId);
  }

  public class ScheduleFilesAttachmentService : IScheduleFilesAttachmentService
  {
    private readonly IScheduleFilesAttachmentRepository _attachmentRepository;
    private readonly ILogger<ScheduleFilesAttachmentService> _logger;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly AppSettings _appSettings;

    public ScheduleFilesAttachmentService(IScheduleFilesAttachmentRepository attachmentRepository, ILogger<ScheduleFilesAttachmentService> _logger,
            IMapper mapper, IUnitOfWork unitOfWork, IOptions<AppSettings> appSettings)
    {
      _attachmentRepository = attachmentRepository;
      this._logger = _logger;
      _mapper = mapper;
      _unitOfWork = unitOfWork;
      _appSettings=appSettings.Value;
    }

    public void CreateFileAttachment(List<ScheduleFilesAttachmentDto> filesAttachmentDto, int scheduleId)
    {
      try
      {
        var listAdd = new List<ScheduleFilesAttachmentModel>();
        foreach (var item in filesAttachmentDto)
        {
          var addItem = _mapper.Map<ScheduleFilesAttachmentModel>(item);
          addItem.ScheduleId = scheduleId;
          listAdd.Add(addItem);
        }

        if (listAdd.Any())
        {
          _attachmentRepository.AddMulti(listAdd);
        }
      }
      catch (System.Exception ex)
      {

        throw;
      }
    }

    public async Task<List<ScheduleFilesAttachmentDto>> getAllFilesAttachmentByScheduleId(int scheduleId)
    {
      var result = await _attachmentRepository.GetMulti(r => r.ScheduleId == scheduleId);
      var resultFinal = _mapper.Map<List<ScheduleFilesAttachmentDto>>(result);
      var hostURL = _appSettings.HostURL;
      foreach (var scheduleFileAttachment in resultFinal)
      {
        scheduleFileAttachment.FilePath=$"{hostURL}\\{scheduleFileAttachment.FilePath}";
      }
      return resultFinal;
    }

    public async Task<List<ScheduleFilesAttachmentDto>> getAllFilesAttachmentByScheduleIdAsync_Ex(int scheduleId)
    {
      var result = await _attachmentRepository.GetMulti(r => r.ScheduleId == scheduleId);
      var resultFinal = _mapper.Map<List<ScheduleFilesAttachmentDto>>(result);
      return resultFinal;
    }

    public async Task<List<ScheduleFileAttachmentShareDto>> GetAllFilesAttachmentForShareByScheduleIdAsync(int scheduleId, string mode)
    {
      var result = await _attachmentRepository.GetMulti(r => r.ScheduleId == scheduleId);
      var hostURL = _appSettings.HostURL;
      switch (mode)
      {
        case "NotShare":
        case "Share":
          var result2Share = new List<ScheduleFileAttachmentShareDto>();
          foreach (var item in result)
          {
            if (!item.IsShare) continue;
            var newObj = new ScheduleFileAttachmentShareDto()
            {
              Id = item.Id,
              FileName = item.FileName,
              FilePath =$"{hostURL}\\{item.FilePath}",
              IsShare = item.IsShare,
              Quote = item.Quote,
              ReleaseDate = item.ReleaseDate,
              ScheduleId = item.ScheduleId,
              CloudinaryPublicId = item.CloudinaryPublicId
            };
            result2Share.Add(newObj);
          }
          return result2Share;
        default:
          return null;
      }
    }

    public async Task Update(List<ScheduleFilesAttachmentDto> filesAttachment, int scheduleId)
    {
      try
      {
        var listFilesExist = await _attachmentRepository.GetMulti(r => r.ScheduleId == scheduleId);
        var toAdd = filesAttachment.Where(r => !listFilesExist.Exists(f => f.Id == r.Id)).ToList();
        var toUpdate = filesAttachment.Where(r => listFilesExist.Exists(f => f.Id == r.Id)).ToList();
        var toDelete = listFilesExist.Where(r => !filesAttachment.Exists(f => f.Id == r.Id)).ToList();

        var listAdd = new List<ScheduleFilesAttachmentModel>();
        foreach (var item in toAdd)
        {
          var addItem = _mapper.Map<ScheduleFilesAttachmentModel>(item);
          addItem.ScheduleId = scheduleId;
          listAdd.Add(addItem);
        }

        if (listAdd.Any())
        {
          _attachmentRepository.AddMulti(listAdd);
        }

        foreach (var item in toUpdate)
        {
          var updateItem = _mapper.Map<ScheduleFilesAttachmentModel>(item);
          _attachmentRepository.UpdateScheduleFileAttachment(updateItem);
        }

        foreach (var item in toDelete)
        {
          var deleteItem = _mapper.Map<ScheduleFilesAttachmentModel>(item);
          _attachmentRepository.Delete(deleteItem);
        }
      }
      catch (System.Exception ex)
      {
        _logger.LogError($"Files Upload fail: ", ex.Message);
      }
    }

    public async Task<bool> DeleteFileAttachmentByIdAsync(int fileAttachmentId)
    {
      await _attachmentRepository.DeleteFileAttachmentByIdAsync(fileAttachmentId);

      return true;
    }
  }
}
