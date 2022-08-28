using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebApi.Helpers.Domains;

namespace WebApi.Controllers
{
  public partial class EmailTemplateController
  {
    [DisplayName("Get all emailtemplate")]
    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    //[Authorize(Roles = "SuperAdmin")]
    [Route("GetAllEmailTemplate")]
    public async Task<IActionResult> GetAllEmailTemplateAsync()
    {
      var result = await _emailTemplateService.GetAllEmailTemplateAsync();
      return Ok(new ApiOkResponse(result));
    }

    [DisplayName("Get list emailtemplate by organizeId")]
    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    [Route("GetListEmailTemplateByOrganizeId")]
    public async Task<IActionResult> GetListBrandNameByOrganizeIdAsync(int organizeId)
    {
      var result = await _emailTemplateService.GetListEmailTemplateByOrganizeIdAsync(organizeId);
      //if (result == null) return BadRequest("Mã đơn vị này không tồn tại");
      return Ok(new ApiOkResponse(result));
    }

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    [Route("GetEmailTemplateById")]
    public async Task<IActionResult> GetEmailTemplateById(int emailTemplateId)
    {
      var result = await _emailTemplateService.GetEmailTemplateByIdAsync(emailTemplateId);
      if (result == null) return BadRequest("Mã emailTeamplateId này không tồn tại");
      return Ok(new ApiOkResponse(result));
    }

  }
}
