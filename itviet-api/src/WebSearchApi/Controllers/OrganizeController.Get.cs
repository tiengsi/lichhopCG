using System.ComponentModel;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using WebApi.Helpers.Domains;

namespace WebApi.Controllers
{

  public partial class OrganizeController
  {
    [DisplayName("Get All Of Organizes follow Tree")]
    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    //[Authorize(Roles = "SuperAdmin,Admin")]
    [Route("OrganizesTree")]
    public async Task<IActionResult> GetOrganizesTreeAsync()
    {
      var result = await _OrganizeService.GetOrganizesTreeAsync();

      return Ok(new ApiOkResponse(result));
    }

    [DisplayName("Get Organize By Id")]
    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    //[Authorize(Roles = "SuperAdmin,Admin")]
    [AllowAnonymous]
    [Route("GetOrganizeById/{organizeId}")]
    public async Task<IActionResult> GetOrganizesTreeAsync(int organizeId)
    {
      var result = await _OrganizeService.GetOrganizeByIdAsync(organizeId);

      return Ok(new ApiOkResponse(result));
    }



  }
}
