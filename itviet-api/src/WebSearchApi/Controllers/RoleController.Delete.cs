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
using WebApi.Models.Dtos;
using WebApi.Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{

  public partial class RoleController
  {
    /// <summary>
    /// Delete Role by Id
    /// </summary>
    /// <param name="index"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    [DisplayName(" Delete Role By Id")]
    [HttpDelete]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    [Route("DeleteRoleById/{roleId}")]
    public async Task<IActionResult> DeleteRoleByIdAsync(int roleId)
    {
      var result = await _roleService.DeleteRoleByIdAsync(roleId);

      if (result.IsSuccess) return Ok(new ApiOkResponse(result));
      return BadRequest(result.GetErrorList());
    }

  }
}
