using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using WebApi.Helpers.Domains;

namespace WebApi.Controllers
{
  public partial class BrandNameController
  {
    [DisplayName("Get All Of BrandName for superadmin")]
    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    //[Authorize(Roles = "SuperAdmin")]
    [Route("GetAllBrandName/vnpt")]
    public async Task<IActionResult> GetAllVNPTBrandNameAsync()
    {
      var result = await _brandService.GetAllVNPT_BrandNameAsync();

      return Ok(new ApiOkResponse(result));
    }

    [DisplayName("Get list brandname by organizeId")]
    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    [Route("GetListBrandNameByOrganizeId/vnpt")]
    public async Task<IActionResult> GetVNPTListBrandNameByOrganizeIdAsync(int organizeId)
    {
      var result = await _brandService.GetVNPT_BrandNameListByOrganizeIdAsync(organizeId);
      //if (result == null) return BadRequest("Mã đơn vị này không tồn tại");
      return Ok(new ApiOkResponse(result));
    }

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    [Route("GetBrandNameById/vnpt")]
    public async Task<IActionResult> GetVNPTBrandNameById(int brandNameId)
    {
      var result = await _brandService.GetVNPT_BrandNameById(brandNameId);
      if (result == null) return BadRequest("Mã id này không tồn tại");
      return Ok(new ApiOkResponse(result));
    }

    ////////////////////////////  VIETTEL
    ///
    [DisplayName("Get All Of BrandName for superadmin")]
    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    //[Authorize(Roles = "SuperAdmin")]
    [Route("GetAllBrandName/viettel")]
    public async Task<IActionResult> GetAllViettelBrandNameAsync()
    {
      var result = await _brandService.GetAllViettel_BrandNameAsync();

      return Ok(new ApiOkResponse(result));
    }
    [DisplayName("Get All Of BrandName by OrganizeId")]
    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    //[Authorize(Roles = "SuperAdmin")]
    [Route("GetAllBrandNameByOrganizeId")]
    public async Task<IActionResult> GetAllBrandNameByOrganizeId(int organizeId)
    {
      var result = await _brandService.GetAllBrandNameByOrganizeIdAsync(organizeId);

      return Ok(new ApiOkResponse(result));
    }


    [DisplayName("Get list brandname by organizeId")]
    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    [Route("GetListBrandNameByOrganizeId/viettel")]
    public async Task<IActionResult> GetViettelBrandNameListByOrganizeIdAsync(int organizeId)
    {
      var result = await _brandService.GetViettel_BrandNameListByOrganizeIdAsync(organizeId);
      //if (result == null) return BadRequest("Mã đơn vị này không tồn tại");
      return Ok(new ApiOkResponse(result));
    }

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    [Route("GetBrandNameById/viettel")]
    public async Task<IActionResult> GetViettelBrandNameById(int brandNameId)
    {
      var result = await _brandService.GetViettel_BrandNameById(brandNameId);
      if (result == null) return BadRequest("Mã id này không tồn tại");
      return Ok(new ApiOkResponse(result));
    }

  }
}
