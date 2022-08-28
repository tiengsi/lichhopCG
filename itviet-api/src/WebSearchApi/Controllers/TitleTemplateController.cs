using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Net;
using System.Threading.Tasks;
using WebApi.Helpers.Domains;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Controllers
{
  [Route("api/title-templates")]
  [ApiController]
  [Authorize]
  public class TitleTemplateController : ControllerBase
  {
    private readonly IScheduleTitleTemplateService _service;

    public TitleTemplateController(IScheduleTitleTemplateService _service)
    {
      this._service = _service;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="index"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    [DisplayName("Get All Of Schedule Title Template")]
    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAll(int index = 1, int pageSize = 10)
    {
      var result = await _service.GetAllScheduleTitleTemplateAsync(index, pageSize);

      return Ok(new ApiOkResponse(result));
    }

    /// <summary>
    /// Get All Schedule Title Template by OrganizeId
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [DisplayName("Get All Of Schedule Title Template by OrganizeId")]
    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    [Route("getAllScheduleTitleTemplateByOrganizeId")]
    public async Task<IActionResult> GetAllScheduleTitleTemplateByOrganizeIdAsync(int index = 1, int pageSize = 10, int organizeId=0)
    {
      var result = await _service.GetAllScheduleTitleTemplateByOrganizeIdAsync(index, pageSize, organizeId);

      return Ok(new ApiOkResponse(result));
    }


    [DisplayName("Get A Schedule Title Template")]
    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetById(int id)
    {
      var result = await _service.GetScheduleTitleTemplateByIdAsync(id);

      return Ok(new ApiOkResponse(result));
    }


    [DisplayName("Create Schedule Title Template")]
    [HttpPost]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    public async Task<IActionResult> Create([FromBody] ScheduleTitleTemplateDto model)
    {
      if (string.IsNullOrEmpty(model.Template))
      {
        return BadRequest("Template is required");
      }

      var result = await _service.CreateScheduleTitleTemplateAsync(model);

      return Ok(new ApiOkResponse(result, StatusCodes.Status201Created));
    }


    [DisplayName("Delete Schedule Title Template")]
    [HttpDelete]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    [Route("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
      var result = await _service.DeleteScheduleTitleTemplateAsync(id);
      if (result.IsSuccess)
        return Ok(new ApiOkResponse(result.GetData, StatusCodes.Status200OK));
      return BadRequest(result.GetErrorList());
    }


    [DisplayName("Update Schedule Title Template")]
    [HttpPut]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    public async Task<IActionResult> UpdateScheduleTitleTemplateAsync([FromBody] ScheduleTitleTemplateDto model)
    {
      if (model.Id == 0)
      {
        return BadRequest("Id is required");
      }

      if (string.IsNullOrEmpty(model.Template))
      {
        return BadRequest("Template is required");
      }

      var result = await _service.UpdateScheduleTitleTemplateAsync(model);

      return Ok(new ApiOkResponse(result, StatusCodes.Status201Created));
    }
  }
}
