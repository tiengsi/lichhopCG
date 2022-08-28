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
    [Route("api/posts")]
    [ApiController]
    [Authorize]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IMapper _mapper;

        public PostController(IPostService postService, IMapper mapper)
        {
            _postService = postService;
            _mapper = mapper;
        }

        [DisplayName("Get All Of Post")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
        
        public async Task<IActionResult> GetAll(
            string filter, 
            string categoryCode, 
            string sortOrder, 
            string sortField, 
            int index = 1, 
            int pageSize = 10)
        {
            var result = await _postService.GetAll(filter, categoryCode, sortOrder, sortField, index, pageSize);

            return Ok(new ApiOkResponse(result));
        }

        [DisplayName("Create Post")]
        [HttpPost]
        [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        public async Task<IActionResult> Create([FromBody]PostForCreateDto model)
        {
            if (string.IsNullOrEmpty(model.Title))
            {
                return BadRequest("Title is required");
            }

            if (string.IsNullOrEmpty(model.Body))
            {
                return BadRequest("Body is required");
            }

            if (string.IsNullOrEmpty(model.Description))
            {
                return BadRequest("Description is required");
            }

            if (model.CategoryId == 0)
            {
                return BadRequest("CategoryId is required");
            }

            var result = await _postService.Create(model);

            return Ok(new ApiOkResponse(result, StatusCodes.Status201Created));
        }

        [DisplayName("Delete Post")]
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
        [Route("{postId}")]
        public async Task<IActionResult> Delete(int postId)
        {

            var result = await _postService.Delete(postId);

            return Ok(new ApiOkResponse(result));
        }

        [DisplayName("Get A Post")]
        [HttpGet]
        [Route("{postId}")]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
       
        public async Task<IActionResult> GetById(int postId)
        {
            var result = await _postService.GetById(postId);
            var resultMap = _mapper.Map<PostDto>(result);

            return Ok(new ApiOkResponse(resultMap));
        }

        [DisplayName("Update Post")]
        [HttpPut]
        [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        public async Task<IActionResult> Update([FromBody]PostDto model)
        {
            if (model.PostId == 0)
            {
                return BadRequest("PostId is required");
            }

            if (string.IsNullOrEmpty(model.Title))
            {
                return BadRequest("Title is required");
            }

            if (model.CategoryId == 0)
            {
                return BadRequest("CategoryId is required");
            }

            var result = await _postService.Update(model);

            return Ok(new ApiOkResponse(result, StatusCodes.Status201Created));
        }

    }
}
