using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebApi.Helpers.Domains;
using WebApi.Models.Dtos;
using WebApi.Services;

namespace WebApi.Controllers
{
  [Route("api/group-participants")]
  [ApiController]
  [Authorize]
  public class GroupParticipantController : ControllerBase
  {
    private readonly IGroupParticipantService _groupParticipantService;

    public GroupParticipantController(IGroupParticipantService groupParticipantService)
    {
      _groupParticipantService = groupParticipantService;
    }

    [DisplayName("Create Group Participant")]
    [HttpPost]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
   
    public async Task<IActionResult> Create([FromBody] GroupParticipantForCreateDto model)
    {
      var result = await _groupParticipantService.CreateGroupParticipantAsync(model);
      if(result.IsSuccess)
        return Ok(new ApiOkResponse(result.GetData, StatusCodes.Status201Created));
      return BadRequest(result.GetErrorList());
    }

    [DisplayName("Get All Of Group Participant")]
    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAll(string name, string sortOrder, string sortField, int organizeId, int index = 1, int pageSize = 10)
    {
      var result = await _groupParticipantService.GetAll(name, sortOrder, sortField, organizeId, index, pageSize);

      return Ok(new ApiOkResponse(result));
    }

    [DisplayName("Delete Group Participant")]
    [HttpDelete]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    [Route("{groupParticipantId}")]
   
    public async Task<IActionResult> Delete(int groupParticipantId)
    {
      var result = await _groupParticipantService.DeleteGroupParticipantByIdAsync(groupParticipantId);
      if (result.IsSuccess)
        return Ok(new ApiOkResponse(result.GetData, StatusCodes.Status200OK));
      return BadRequest(result.GetErrorList());
    }

    [DisplayName("Get A Group Participant")]
    [HttpGet]
    [Route("{groupParticipantId}")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]

    public async Task<IActionResult> GetGroupParticipantByIdAsync(int groupParticipantId)
    {
      var result = await _groupParticipantService.GetGroupParticipantByIdAsync(groupParticipantId);
      if (result == null) return BadRequest($"Không tìm thấy mã này!");
      return Ok(new ApiOkResponse(result));
    }
    [DisplayName("Get A List Group Participant By OrganizeId")]
    [HttpGet]
    [Route("GetListGroupParticipantByOrganizeId")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    
    public async Task<IActionResult> GetListGroupParticipantByOrganizeIdAsync(int organizeId)
    {
      var result = await _groupParticipantService.GetListGroupParticipantByOrganizeIdAsync(organizeId);

      return Ok(new ApiOkResponse(result));
    }

    [DisplayName("Update Group Participant")]
    [HttpPut]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
   
    public async Task<IActionResult> UpdateGroupParticipant([FromBody] GroupParticipantForCreateDto model)
    {
      var result = await _groupParticipantService.UpdateGroupParticipantAsync(model);
      if(result.IsSuccess)
         return Ok(new ApiOkResponse(result.GetData, StatusCodes.Status200OK));
      return BadRequest(result.GetErrorList());
    }
  }
}
