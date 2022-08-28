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
    /// Xóa 1 brandname
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [DisplayName("Delete brandname by id ")]
    [HttpDelete]
    [Route("DeleteBrandNameById/vnpt/{id}")]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    public async Task<IActionResult> DeleteVNPTBrandNameByIdAsync(int id)
    {

      var result = await _brandService.DeleteVNPT_BrandNameByIdAsync(id);
      if (result.IsSuccess) return Ok(new ApiOkResponse(result.GetData, StatusCodes.Status200OK));
      return BadRequest(result.GetErrorList());
    }

    /// <summary>
    /// Xóa 1 brandname
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [DisplayName("Delete brandname by id ")]
    [HttpDelete]
    [Route("DeleteBrandNameById/viettel/{id}")]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    public async Task<IActionResult> DeleteViettelBrandNameByIdAsync(int id)
    {

      var result = await _brandService.DeleteViettel_BrandNameByIdAsync(id);
      if (result.IsSuccess) return Ok(new ApiOkResponse(result.GetData, StatusCodes.Status200OK));
      return BadRequest(result.GetErrorList());
    }

  }
}
