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
    /// Tạo vnpt brandname
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [DisplayName("Create a brandname")]
    [HttpPost]
    [Route("CreateBrandName/vnpt")]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    public async Task<IActionResult> CreateVNPT_BrandNameAsync([FromBody] VNPT_BrandNameDto model)
    {
      var branchName = model.ToModel();
      var result = await _brandService.CreateVNPT_BrandNameAsync(branchName);
      if (result.IsSuccess) return Ok(new ApiOkResponse(result.GetData, StatusCodes.Status201Created));
      return BadRequest(result.GetErrorList());
    }

    /// <summary>
    /// Tạo viettel brandname
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [DisplayName("Create a brandname")]
    [HttpPost]
    [Route("CreateBrandName/viettel")]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    public async Task<IActionResult> CreateViettelBrandNameAsync([FromBody] Viettel_BrandNameDto model)
    {
      var branchName = model.ToModel();
      var result = await _brandService.CreateViettel_BrandNameAsync(branchName);
      if (result.IsSuccess) return Ok(new ApiOkResponse(result.GetData, StatusCodes.Status201Created));
      return BadRequest(result.GetErrorList());
    }
  }
}
