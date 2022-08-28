using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebApi.Helpers.Domains;
using WebApi.Models;

namespace WebApi.Controllers
{
  public partial class EmailTemplateController
  {
    /// <summary>
    /// Tạo brandname
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [DisplayName("Create a emailtemplate")]
    [HttpPost]
    [Route("CreateEmailTemplate")]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    public async Task<IActionResult> CreateEmailTemplateAsync([FromBody] EmailTemplateDto model)
    {

      var result = await _emailTemplateService.CreateEmailTemplateAsync(model);
      if (result.IsSuccess)
      {
        return Ok(new ApiOkResponse(result.GetData, StatusCodes.Status201Created));
      }
      else
      {
        return BadRequest(result.GetErrorList());
      }
    }
  }
}
