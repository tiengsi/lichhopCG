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
    /// 
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [DisplayName("Update  Organize")]
    [HttpPut]
    [Route("UpdateOrganize")]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    public async Task<IActionResult> UpdateOrganizeAsync([FromBody] OrganizeDto model)
    {
      var result = await _OrganizeService.UpdateOrganizeAsync(model);
      if (result.IsSuccess)
        return Ok(new ApiOkResponse(result.GetData, StatusCodes.Status200OK));
      else
       return BadRequest(result.GetErrorList());
    }
  }
}
