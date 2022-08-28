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
    /// Xóa 1 brandname
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [DisplayName("Delete emailtemplate by id ")]
    [HttpDelete]
    [Route("DeleteEmailTemplate/{emailTemplateId}")]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    public async Task<IActionResult> DeleteEmailTemplateByIdAsync(int emailTemplateId)
    {

      var result = await _emailTemplateService.DeleteEmailTemplateByIdAsync(emailTemplateId);
      if (result.IsSuccess)
        return Ok(new ApiOkResponse(result.GetData, StatusCodes.Status200OK));
      else
        return BadRequest(result.GetErrorList());
    }
  }
}
