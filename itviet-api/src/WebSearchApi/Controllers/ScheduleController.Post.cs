using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using WebApi.Helpers.Domains;
using WebApi.Models;
using WebApi.Models.Dtos;
using WebApi.Services;

namespace WebApi.Controllers
{

  public partial class ScheduleController
  {
    /// <summary>
    /// Create New Schedule
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [DisplayName("Create Schedule")]
    [HttpPost]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]

    public async Task<IActionResult> Create([FromBody] ScheduleForAddDto model)
    {
      var result = await _scheduleService.CreateScheduleAsync(model);
      if (result.IsSuccess) return Ok(new ApiOkResponse(result, StatusCodes.Status201Created));
      return BadRequest(result.GetErrorList());
    }

    /// <summary>
    /// Send SMS
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [DisplayName("Send SMS")]
    [HttpPost("sms")]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    //[Authorize(Roles = "SuperAdmin,Admin")]
    public async Task<IActionResult> SendSms([FromBody] SendSmsDto model)
    {
      if (!model.PhoneNumber.Any())
      {
        return BadRequest("Bạn phải gửi lên số điện thoại nhận tin nhắn!");
      }

      if (string.IsNullOrEmpty(model.Content))
      {
        return BadRequest("Bạn phải gửi lên nội dung tin nhắn!");
      }

      var result = await _emailAndSmsService.SendSMS(model);

      return Ok(new ApiOkResponse(result));
    }

    /// <summary>
    /// Release Schedule
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [DisplayName("Release Schedule")]
    [HttpPost("release")]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    //[Authorize(Roles = "SuperAdmin,Admin")]
    public async Task<IActionResult> ReleaseSchedule([FromBody] ScheduleReleasePayload model)
    {
      await _scheduleService.ReleaseSchedule(model);

      return Ok(new ApiOkResponse(null, StatusCodes.Status204NoContent));
    }

    [DisplayName("Release Schedule By Id")]
    [HttpPost("release-by-id/{id}")]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    //[Authorize(Roles = "SuperAdmin,Admin")]
    public async Task<IActionResult> ReleaseScheduleById(int id)
    {
      await _scheduleService.ReleaseScheduleById(id);

      return Ok(new ApiOkResponse(null, StatusCodes.Status204NoContent));
    }

    //////////////////////////////////////////-------------------CREATE-------------------//////////////////////////


    /// <summary>
    /// 
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [DisplayName("Create  Personal Notes")]
    [HttpPost]
    [Route("CreatePersonalNotes")]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    public async Task<IActionResult> CreatePersonalNotesAsync([FromBody] PersonalNotesDto model)
    {

      var result = await _scheduleService.CreatePersonalNotesAsync(model);

      return Ok(new ApiOkResponse(result, StatusCodes.Status201Created));
    }

    /// <summary>
    /// Tạo Tài liệu kết luận của cuộc họp
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [DisplayName("Create  Scheduled Result Document")]
    [HttpPost]
    [Route("CreateScheduledResultDocument")]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    public async Task<IActionResult> CreateScheduledResultDocumentAsync([FromBody] ScheduledResultDocumentDto model)
    {

      var result = await _scheduleService.CreateScheduledResultDocumentAsync(model);

      return Ok(new ApiOkResponse(result, StatusCodes.Status201Created));
    }

    /// <summary>
    /// Tạo báo cáo kết luận của cuộc họp
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [DisplayName("Create  Scheduled Result Report")]
    [HttpPost]
    [Route("CreateScheduledResultReport")]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    public async Task<IActionResult> CreateScheduledResultReportAsync([FromBody] ScheduledResultReportDto model)
    {

      var result = await _scheduleService.CreateScheduledResultReportAsync(model);
      if (result.IsSuccess==true) return Ok(new ApiOkResponse(result.GetData, StatusCodes.Status201Created));
      else return Ok(new ApiOkResponse(result.GetErrorList(), StatusCodes.Status400BadRequest, false));
    }

    /// <summary>
    /// Tạo Lich cá nhân
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [DisplayName("Create  Personal Schedule")]
    [HttpPost]
    [Route("CreatePersonalSchedule")]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    public async Task<IActionResult> CreatePersonalScheduleAsync([FromBody] PersonalScheduleDto model)
    {

      var result = await _scheduleService.CreatePersonalScheduleAsync(model);

      return Ok(new ApiOkResponse(result, StatusCodes.Status201Created));
    }
  }
}
