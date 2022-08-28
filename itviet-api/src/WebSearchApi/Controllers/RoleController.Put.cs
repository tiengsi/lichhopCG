using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApi.Helpers.Domains;
using WebApi.Models;
using WebApi.Models.Dtos;
using WebApi.Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{

  public partial class RoleController
  {
    /// <summary>
    /// Update Role
    /// </summary>
    /// <param name="index"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    [DisplayName("Update  Role")]
    [HttpPut]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    [Route("UpdateRoleById")]
    public async Task<IActionResult> UpdateRoleByIdAsync([FromBody] RoleDto model)
    {
      var result = await _roleService.UpdateRoleByIdAsync(model);
      if (result.IsSuccess) return Ok(new ApiOkResponse(result));
      return BadRequest(result.GetErrorList());
    }

  }
}
