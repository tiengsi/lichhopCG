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
    [DisplayName("Delete Schedule")]
    [HttpDelete]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    [Route("{scheduleId}")]
    
    public async Task<IActionResult> DeleteScheduleByIdAsync(int scheduleId)
    {
      var result = await _scheduleService.DeleteScheduleByIdAsync(scheduleId);

      return Ok(new ApiOkResponse(result));
    }
    //==============================================================DELETE==================================================
    /// <summary>
    /// Xóa ghi chú cá nhân theo id
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [DisplayName("Delete  Personal Note by Id ")]
    [HttpDelete]
    [Route("DeletePersonalNoteById")]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    public async Task<IActionResult> DeletePersonalNotesAsync([FromBody] PersonalNotesDto model)
    {

      var result = await _scheduleService.DeletePersonalNotesAsync(model.PersonalNotesId);
      if (result > 0)
        return Ok(new ApiOkResponse(result, StatusCodes.Status200OK));

      return BadRequest("Không tồn tại Id này");
    }

    /// <summary>
    /// Xóa Lịch cá nhân theo  id
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [DisplayName("Delete  Personal Schedule by Id ")]
    [HttpDelete]
    [Route("DeletePersonalScheduleById")]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    public async Task<IActionResult> DeletePersonalScheduleByIdAsync([FromBody] PersonalScheduleDto model)
    {

      var result = await _scheduleService.DeletePersonalScheduleByIdAsync(model.PersonalScheduleId);
      if (result > 0)
        return Ok(new ApiOkResponse(result, StatusCodes.Status200OK));

      return BadRequest("Không tồn tại Id này");
    }
  }
}
