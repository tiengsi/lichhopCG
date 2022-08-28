using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApi.Helpers.Domains;
using WebApi.Models;
using WebApi.Models.Dtos;
using WebApi.Services;

namespace WebApi.Controllers
{
  [Route("api/department")]
  [ApiController]
  [Authorize]
  public class DepartmentController : ControllerBase
  {
    private readonly IDepartmentService _departmentService;
    private readonly IMapper _mapper;
    private readonly ILogger<DepartmentController> _logger;

    public DepartmentController(IDepartmentService departmentService, IMapper mapper, ILogger<DepartmentController> logger)
    {
      _departmentService = departmentService;
      _mapper = mapper;
      _logger = logger;
    }

    [DisplayName("Get All Of Department")]
    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    
    public async Task<IActionResult> GetAll(string name, string sortOrder, string sortField, int organizeId)
    {
      var result = await _departmentService.GetAll(name, sortOrder, sortField, organizeId);
      var mappResult = _mapper.Map<IEnumerable<DepartmentDto>>(result);
      return Ok(new ApiOkResponse(mappResult));
    }

    /// <summary>
    /// Get A List Department By OrganizeId
    /// </summary>
    /// <param name="organizeId"></param>
    /// <returns></returns>
    [DisplayName("Get A List Department By OrganizeId")]
    [HttpGet]
    [Route("GetListDepartmentByOrganizeId")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetListDepartmentByOrganizeIdAsync(int organizeId)
    {
      var result = await _departmentService.GetListDepartmentByOrganizeIdAsync(organizeId);
      return Ok(new ApiOkResponse(result));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [DisplayName("Get All Active")]
    [HttpGet("get-all-active")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
  
    public async Task<IActionResult> GetAllActive(int organizeId)
    {
      var result = await _departmentService.GetAll(string.Empty, "desc", "CreatedDate", organizeId, true);
      var mappResult = _mapper.Map<IEnumerable<DepartmentDto>>(result);

      return Ok(new ApiOkResponse(mappResult));
    }

    [DisplayName("Get Department officer")]
    [HttpPost("get-department-officer")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    
    public async Task<IActionResult> GetDepartmentOfficer(GetOfficerRequest request)
    {
      var departments = await _departmentService.GetDepartmentOfficer(request.Filter, request.SortField);

      var results = _mapper.Map<IEnumerable<TreeDepartmentOfficerDto>>(departments);

      return Ok(new ApiOkResponse(results));
    }

    [DisplayName("Get A Department")]
    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
   
    public async Task<IActionResult> GetById(int id)
    {
      var result = await _departmentService.GetById(id);
      if (result == null) return BadRequest("Mã phòng ban này không tồn tại");
      var resultMap = _mapper.Map<DepartmentDto>(result);
      return Ok(new ApiOkResponse(resultMap));
    }
    //===================================================================INSERT====================================================================

    [DisplayName("Create Department")]
    [HttpPost]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]

    public async Task<IActionResult> CreateDepartmentAsync([FromBody] DepartmentDto model)
    {
      var result = await _departmentService.CreateDepartmentAsync(model);
      if (result.IsSuccess)
        return Ok(new ApiOkResponse(result.GetData, StatusCodes.Status201Created));
      return BadRequest(result.GetErrorList());
    }

    //===================================================================DELETE====================================================================

    [DisplayName("Delete Department")]
    [HttpDelete]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    [Route("{id}")]

    public async Task<IActionResult> DeleteDepartmentByIdAsync(int id)
    {

      var result = await _departmentService.DeleteDepartmentByIdAsync(id);
      if (result.IsSuccess)
        return Ok(new ApiOkResponse(result.GetData, StatusCodes.Status200OK));
      return BadRequest(result.GetErrorList());
    }

    //===================================================================UPDATE====================================================================

    [DisplayName("Update Department")]
    [HttpPut]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]

    public async Task<IActionResult> Update([FromBody] DepartmentDto model)
    {
      _logger.LogInformation($"Update Department {@model}", model);

      var result = await _departmentService.UpdateDepartmentByIdAsync(model);
      if (result.IsSuccess) return Ok(new ApiOkResponse(result.GetData, StatusCodes.Status200OK));
      return BadRequest(result.GetErrorList());
    }

    [DisplayName("Patch Update")]
    [HttpPatch]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    [Route("{id}")]
    public async Task<IActionResult> PatchUpdate(int id, [FromBody] JsonPatchDocument<DepartmentModel> patchOrder)
    {
      if (id == 0)
      {
        return BadRequest("orderId is required");
      }

      if (patchOrder == null)
      {
        return BadRequest("patchOrder is required");
      }

      var result = await _departmentService.PathUpdate(id, patchOrder);
      return Ok(new ApiOkResponse(result, StatusCodes.Status204NoContent));
    }


    [DisplayName("Update Representative")]
    [HttpPut]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [Route("representative")]
    public async Task<IActionResult> UpdateRepresentative([FromBody] DepartmentRepresentativePayload model)
    {
      _logger.LogInformation($"Update Department {@model}", model);
      if (model.DepartmentId == 0)
      {
        return BadRequest("DepartmentId is required");
      }

      var result = await _departmentService.UpdateRepresentative(model);

      return Ok(new ApiOkResponse(result, StatusCodes.Status201Created));
    }
  }
}
