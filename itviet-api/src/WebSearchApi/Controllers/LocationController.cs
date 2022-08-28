using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApi.Helpers.Domains;
using WebApi.Models.Dtos;
using WebApi.Services;

namespace WebApi.Controllers
{
  [Route("api/location")]
  [ApiController]
  [Authorize]
  public class LocationController : ControllerBase
  {
    private readonly ILogger<LocationController> _logger;
    private readonly ILocationService _locationService;
    private readonly IMapper _mapper;

    public LocationController(ILogger<LocationController> logger, ILocationService postService, IMapper mapper)
    {
      _logger = logger;
      _locationService = postService;
      _mapper = mapper;
    }

    [DisplayName("Get All Of Location")]
    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    
    public async Task<IActionResult> GetAll(string filter, string sortOrder, string sortField, int organizeId, int index = 1, int pageSize = 10)
    {
      var result = await _locationService.GetAll(filter, sortOrder, sortField, organizeId, index, pageSize);
      var itemsMap = _mapper.Map<IEnumerable<LocationForListDto>>(result.Items);

      return Ok(new ApiOkResponse(new PaginationSet<LocationForListDto>()
      {
        Items = itemsMap,
        TotalCount = result.TotalCount,
        Page = result.Page
      }));
    }

    [DisplayName("Get All Of Location active for select")]
    [HttpGet("get-all-select-active")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    
    public async Task<IActionResult> GetAllActive(int organizeId)
    {
      try
      {
        var result = await _locationService.GetAllForSelect(organizeId);
        var itemsMap = _mapper.Map<IEnumerable<LocationDto>>(result);

        return Ok(new ApiOkResponse(itemsMap));
      }
      catch (Exception ex)
      {
        return Ok("");
      }
    }

    [DisplayName("Create Location")]
    [HttpPost]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    
    public async Task<IActionResult> Create([FromBody] LocationDto model)
    {
      var result = await _locationService.CreateLocationAsync(model);
      if(result.IsSuccess)
        return Ok(new ApiOkResponse(result.GetData, StatusCodes.Status201Created));
      return BadRequest(result.GetErrorList());
    }

    [DisplayName("Delete Location")]
    [HttpDelete]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    [Route("{postId}")]

    public async Task<IActionResult> DeleteLocationByIdAsync(int postId)
    {
      var result = await _locationService.DeleteLocationByIdAsync(postId);
      if (result.IsSuccess)
        return Ok(new ApiOkResponse(result.GetData, StatusCodes.Status201Created));
      return BadRequest(result.GetErrorList());
    }

    [DisplayName("Get A Location")]
    [HttpGet]
    [Route("{postId}")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    
    public async Task<IActionResult> GetLocationByIdAsync(int postId)
    {
      var result = await _locationService.GetLocationByIdAsync(postId);
      if (result == null) return BadRequest("Không tìm thấy mã địa điểm này");
      var resultMap = _mapper.Map<LocationDto>(result);

      return Ok(new ApiOkResponse(resultMap));
    }
    [DisplayName("Get list location by OrganizeId")]
    [HttpGet]
    [Route("GetListLocationByOrganizeId")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]

    public async Task<IActionResult> GetListLoacationByOrganizeIdAsync(int organizeId)
    {
      var result = await _locationService.GetListLocationByOrganizeIdAsync(organizeId);
      return Ok(new ApiOkResponse(result));
    }


    [DisplayName("Update Location")]
    [HttpPut]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
   
    public async Task<IActionResult> Update([FromBody] LocationDto model)
    {
      var result = await _locationService.UpdateLocationAsync(model);
      if(result.IsSuccess)
        return Ok(new ApiOkResponse(result.GetData, StatusCodes.Status201Created));
      return BadRequest(result.GetErrorList());
    }
  }
}
