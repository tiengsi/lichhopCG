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

namespace WebApi.Controllers
{
  [Route("api/Permissions")]
  [ApiController]
  [Authorize]
  public class PermissionController : ControllerBase
  {
    private readonly IPermissionService _PermissionService;

    public PermissionController(IPermissionService PermissionService)
    {
      _PermissionService = PermissionService;
    }

    /// <summary>
    /// Get Permissions Of FE
    /// </summary>
    /// <param name="host"></param>
    /// <param name="scheduleDate"></param>
    /// <param name="locationId"></param>
    /// <param name="active"></param>
    /// <param name="status"></param>
    /// <param name="sortOrder"></param>
    /// <param name="sortField"></param>
    /// <param name="index"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    [DisplayName("Get Permission List of UI")]
    [HttpGet]
    [Route("PermissionOfUIByUserId/{userId}")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
  

    public async Task<IActionResult> GetPermissionListOfUIByUserIdAsync(int userId)
    {
      var result = await _PermissionService.GetPermissionListOfUIByUserIdAsync(userId);

      return Ok(new ApiOkResponse(result));
    }


    /// <summary>
    /// Get Permissions Of FE
    /// </summary>
    /// <param name="host"></param>
    /// <param name="scheduleDate"></param>
    /// <param name="locationId"></param>
    /// <param name="active"></param>
    /// <param name="status"></param>
    /// <param name="sortOrder"></param>
    /// <param name="sortField"></param>
    /// <param name="index"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    [DisplayName("Get FE Permission List by Roles ")]
    [HttpGet]
    [Route("FEPermissionsByRoles")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetFEPermissionsByRolesAsync( )
    {
      var result = await _PermissionService.GetFEPermissionsByRolesAsync();

      return Ok(new ApiOkResponse(result));
    }

    /// <summary>
    /// Get Permissions Of FE
    /// </summary>
    /// <param name="host"></param>
    /// <param name="scheduleDate"></param>
    /// <param name="locationId"></param>
    /// <param name="active"></param>
    /// <param name="status"></param>
    /// <param name="sortOrder"></param>
    /// <param name="sortField"></param>
    /// <param name="index"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    [DisplayName("Update FE Permission List by Roles ")]
    [HttpPut]
    [Route("FEPermissionsByRoles/Update")]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    public async Task<IActionResult> UpdateFEPermissionsByRolesAsync([FromBody] IEnumerable<FEPermissionByRoleDto> model)
    {
      var result = await _PermissionService.UpdateFEPermissionsByRolesAsync(model);

      return Ok(new ApiOkResponse(result));
    }
  }
}
