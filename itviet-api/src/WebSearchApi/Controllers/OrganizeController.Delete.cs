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

  public partial class OrganizeController
  {
    /// <summary>
    /// Xóa Đơn vị
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [DisplayName("Delete  Organize by Id ")]
    [HttpDelete]
    [Route("DeleteOrganizeById/{organizeId}")]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    public async Task<IActionResult> DeleteOrganizeByIdAsync(int organizeId)
    {
      FunctionResult result = new FunctionResult();
      try
      {
        result = await _scheduleService.DeleteScheduleByOrganizeIdAsync(organizeId);        
      }
      catch(Exception e)
      {
        throw e;
      }
      try
      {        
        result = await _departmentService.DeleteDepartmentByOrganizeIdAsync(organizeId);        
      }
      catch (Exception e)
      {
        throw e;
      }
      try
      {        
        result = await _userService.DeleteListUserByOrganizeIdAsync(organizeId);
      }
      catch (Exception e)
      {
        throw e;
      }
      result = await _OrganizeService.DeleteOrganizeByIdAsync(organizeId);
      if (result.IsSuccess)
        return Ok(new ApiOkResponse(result.GetData, StatusCodes.Status200OK));
      else
        return BadRequest(result.GetErrorList());
    }
  }
}
