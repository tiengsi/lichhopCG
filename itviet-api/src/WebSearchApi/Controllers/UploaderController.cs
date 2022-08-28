using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Helpers.Domains;
using WebApi.Models.Dtos;
using WebApi.Services;

namespace WebApi.Controllers
{
  [Route("api/uploaders")]
  [ApiController]
  [Authorize]
  public class UploaderController : ControllerBase
  {
    private readonly IUploaderService _uploaderService;
    private readonly ISettingService _settingService;

    public UploaderController(IUploaderService uploaderService, ISettingService settingService)
    {
      _uploaderService = uploaderService;
      _settingService = settingService;
    }


    //[DisplayName("Upload Image")]
    //[HttpPost]
    //[ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.Created)]
    //[ProducesResponseType((int)HttpStatusCode.BadRequest)]
    //[ProducesResponseType((int)HttpStatusCode.Conflict)]
    //public async Task<IActionResult> Create([FromForm] List<IFormFile> file)
    //{
    //  if (!file.Any())
    //  {
    //    return Ok(new ApiOkResponse(null, 201, true, "Không có file nào được tải lên"));
    //  }

    //  var result = await _uploaderService.UploadImage(file, true);

    //  return Ok(new ApiOkResponse(result, StatusCodes.Status201Created));
    //}

    [DisplayName("Upload File Attachment")]
    [HttpPost]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    [Route("attachment")]
    public async Task<IActionResult> FileAttachment([FromForm] List<IFormFile> file)
    {
      if (!file.Any())
      {
        return Ok(new ApiOkResponse(null, 201, true, "Không có file nào được tải lên"));
      }

      var result = await _uploaderService.UploadFile(file, string.Empty);

      return Ok(new ApiOkResponse(result, StatusCodes.Status201Created));
    }

    [DisplayName("Update Image For Post")]
    [HttpPost]
    [Route("{postId}/post")]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    public async Task<IActionResult> UpdateImageForPost(int postId, [FromForm] List<IFormFile> file)
    {
      if (!file.Any())
      {
        return BadRequest("files is required");
      }

      var result = await _uploaderService.UploadImage(file, true);

      return Ok(new ApiOkResponse(result, StatusCodes.Status201Created));
    }

    [DisplayName("Update Image For Setting")]
    [HttpPost]
    [Route("{settingKey}/setting")]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    public async Task<IActionResult> UpdateImageForSetting(string settingKey, [FromForm] List<IFormFile> file)
    {
      if (string.IsNullOrEmpty(settingKey))
      {
        return BadRequest("settingKey is required");
      }
      if (!file.Any())
      {
        return BadRequest("files is required");
      }

      var result = await _uploaderService.UploadImage(file, false);
      await _settingService.Update(settingKey, result.Url, result.PublicId);

      return Ok(new ApiOkResponse(result, StatusCodes.Status201Created));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    [DisplayName("Upload File Attachment v2")]
    [HttpPost]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    [Route("attachmentFile/v2")]
    public async Task<IActionResult> UploadFileAttachmentV2Async([FromForm] UploadFileInfoDto fileInfo)
    {
      if (!fileInfo.FileUpload.Any())
      {
        return Ok(new ApiOkResponse(null, 201, true, "Không có file nào được tải lên"));
      }
      try
      {
        var result = await _uploaderService.UploadFileToServerAsync(fileInfo);
        if (result.IsSuccess) return Ok(new ApiOkResponse(result, StatusCodes.Status201Created));
        return Ok(new ApiOkResponse(result, StatusCodes.Status400BadRequest, false));

      }catch(Exception e)
      {
        return Ok(new ApiOkResponse(e, StatusCodes.Status400BadRequest, false));
      }
      ;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="settingKey"></param>
    /// <param name="file"></param>
    /// <returns></returns>
    [DisplayName("Update Image For Setting")]
    [HttpPost]
    [Route("{settingKey}/setting/v2")]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    public async Task<IActionResult> UpdateImageForSettingV2Async(string settingKey, [FromForm] UploadFileInfoDto fileInfo)
    {
      if (string.IsNullOrEmpty(settingKey))
      {
        return BadRequest("settingKey is required");
      }
      if (!fileInfo.FileUpload.Any())
      {
        return BadRequest("files is required");
      }

      var result = await _uploaderService.UploadImageToServerAsync(fileInfo, false);
      if (result.IsSuccess) await _settingService.Update(settingKey, result.GetData.ToString(), String.Empty);

      return Ok(new ApiOkResponse(result, StatusCodes.Status201Created));
    }
  }
}
