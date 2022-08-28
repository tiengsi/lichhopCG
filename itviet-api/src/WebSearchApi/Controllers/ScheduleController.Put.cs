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
    //////////////////////////////////////////-------------------UPDATE-------------------//////////////////////////
    /// <summary>
    /// 
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [DisplayName("Update  Personal Notes")]
    [HttpPut]
    [Route("UpdatePersonalNotes")]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    public async Task<IActionResult> UpdatePersonalNotesAsync([FromBody] PersonalNotesDto model)
    {

      var result = await _scheduleService.UpdatePersonalNotesAsync(model);
      if (result > 0)
        return Ok(new ApiOkResponse(result, StatusCodes.Status200OK));

      return BadRequest("Không tồn tại Id này");
    }


    /// <summary>
    /// cập nhật tài liệu kết luận theo Id
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [DisplayName("Update  Scheduled Result Document ")]
    [HttpPut]
    [Route("UpdateScheduledResultDocument")]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    public async Task<IActionResult> UpdateScheduledResultDocumentAsync([FromBody] ScheduledResultDocumentDto model)
    {

      var result = await _scheduleService.UpdateScheduledResultDocumentAsync(model);
      if (result > 0)
        return Ok(new ApiOkResponse(result, StatusCodes.Status200OK));

      return BadRequest("Không tồn tại Id này");
    }

    /// <summary>
    /// cập nhật báo cáo kết luận theo Id
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [DisplayName("Update  Scheduled Result Report ")]
    [HttpPut]
    [Route("UpdateScheduledResultReport")]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    public async Task<IActionResult> UpdateScheduledResultReportAsync([FromBody] ScheduledResultReportDto model)
    {

      var result = await _scheduleService.UpdateScheduledResultReportAsync(model);
      if (result > 0)
        return Ok(new ApiOkResponse(result, StatusCodes.Status200OK));

      return BadRequest("Không tồn tại Id này");
    }

    /// <summary>
    /// cập nhật lịch cá nhân theo Id
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [DisplayName("Update  Personal Schedule By Id")]
    [HttpPut]
    [Route("UpdatePersonalSchedule")]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    public async Task<IActionResult> UpdatePersonalScheduleAsync([FromBody] PersonalScheduleDto model)
    {

      var result = await _scheduleService.UpdatePersonalScheduleAsync(model);
      if (result > 0)
        return Ok(new ApiOkResponse(result, StatusCodes.Status200OK));

      return BadRequest("Không tồn tại Id này");
    }

    [DisplayName("Update Schedule")]
    [HttpPut]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    
    public async Task<IActionResult> UpdateScheduleByIdAsync([FromBody] ScheduleForAddDto model)
    {
      var result = await _scheduleService.UpdateScheduleByIdAsync(model);
      if (result.IsSuccess==true) return Ok(new ApiOkResponse(result, StatusCodes.Status200OK));
      return BadRequest(result.GetErrorList());
    }


    [DisplayName("Update Status")]
    [HttpPut("update-status/{id}")]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
   
    public async Task<IActionResult> UpdateStatusScheduleAsync(int id)
    {
      var resultId = await _scheduleService.UpdateStatusOfScheduleByIdAsync(id);

      return Ok(new ApiOkResponse(resultId, StatusCodes.Status200OK));
    }

    [DisplayName("Approve")]
    [HttpPut("{scheduleId}/approve")]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    //[Authorize(Roles = "SuperAdmin,Admin")]
    public async Task<IActionResult> ApproveScheduleAsync(int scheduleId)
    {
      var resultId = await _scheduleService.ApproveScheduleByIdAsync(scheduleId);

      return Ok(new ApiOkResponse(resultId, StatusCodes.Status200OK));
    }

    [DisplayName("Pause")]
    [HttpPut("pause")]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    //[Authorize(Roles = "SuperAdmin,Admin")]
    public async Task<IActionResult> PauseScheduleAsync([FromBody] ScheduleForDetailDto model)
    {
      var resultId = await _scheduleService.PauseScheduleByIdAsync(model);

      return Ok(new ApiOkResponse(resultId, StatusCodes.Status200OK));
    }

    [DisplayName("Change")]
    [HttpPut("change")]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    //[Authorize(Roles = "SuperAdmin,Admin")]
    public async Task<IActionResult> ChangeScheduleAsync([FromBody] ScheduleForDetailDto model)
    {
      var resultId = await _scheduleService.ChangeScheduleAsync(model);

      return Ok(new ApiOkResponse(resultId, StatusCodes.Status200OK));
    }



    [DisplayName("Update MessageContent")]
    [HttpPut("message-content")]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    //[Authorize(Roles = "SuperAdmin,Admin")]
    public async Task<IActionResult> UpdateMessageContent([FromBody] MessageContentPayload model)
    {
      await _scheduleService.UpdateMessageContent(model);

      return Ok(new ApiOkResponse(null, StatusCodes.Status204NoContent));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [DisplayName("Update  Schedule File Attachment List")]
    [HttpPut]
    [Route("UpdateScheduleFileAttachmentList")]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    public async Task<IActionResult> UpdateScheduleFileAttachmentListAsync([FromBody] List<ScheduleFileAttachmentShareDto> model)
    {

      var result = await _scheduleService.UpdateIsShareFieldScheduleFileAttachmentListAsync(model);
      if (result > 0)
        return Ok(new ApiOkResponse(result, StatusCodes.Status200OK));

      return BadRequest("danh sách rỗng");
    }
  }
}
