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
  public partial class BrandNameController
  {
    /// <summary>
    /// update 1 brandname
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [DisplayName("Update a brandname")]
    [HttpPut]
    [Route("UpdateBrandName/vnpt")]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    public async Task<IActionResult> UpdateVNPTBrandNameAsync([FromBody] VNPT_BrandNameDto model)
    {
      var branchName = model.ToModel();
      branchName.BrandNameId = model.BrandNameId;
      var result = await _brandService.UpdateVNPT_BrandNameAsync(branchName);
      if (result.IsSuccess) return Ok(new ApiOkResponse(result.GetData, StatusCodes.Status200OK));
      return BadRequest(result.GetErrorList());
    }

    /// <summary>
    /// update 1 brandname
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [DisplayName("Update a brandname")]
    [HttpPut]
    [Route("UpdateBrandName/viettel")]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    public async Task<IActionResult> UpdateViettelBrandNameAsync([FromBody] Viettel_BrandNameDto model)
    {
      var branchName = model.ToModel();
      branchName.BrandNameId = model.BrandNameId;
      var result = await _brandService.UpdateViettel_BrandNameAsync(branchName);
      if (result.IsSuccess) return Ok(new ApiOkResponse(result.GetData, StatusCodes.Status200OK));
      return BadRequest(result.GetErrorList());
    }
  }
}
