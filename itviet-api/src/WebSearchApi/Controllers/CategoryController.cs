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
using WebApi.Models.Enums;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Route("api/categories")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoryController(ILogger<CategoryController> logger, ICategoryService categoryService, IMapper mapper)
        {
            _logger = logger;
            _categoryService = categoryService;
            _mapper = mapper;
        }

        [DisplayName("Get All Of Category For Select")]
        [HttpGet]
        [Route("{type}/select")]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll(ECategoryType type)
        {
            var result = await _categoryService.GetByType(type);
            var mappResult = _mapper.Map<List<CategoryForSelectDto>>(result);

            return Ok(new ApiOkResponse(mappResult));
        }

        [DisplayName("Get All Of Category By Menu")]
        [HttpGet]
        [Route("{menu}/menu")]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCategoriesByMenu(string menu)
        {
            var result = await _categoryService.GetByMenu(menu);
            var mappResult = _mapper.Map<List<CategoryDto>>(result);

            return Ok(new ApiOkResponse(mappResult));
        }

        [DisplayName("Get Category Name By Code")]
        [HttpGet]
        [Route("{code}/code")]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetByCategoryCode(string code)
        {
            var result = await _categoryService.GetByCategoryCode(code);

            return Ok(new ApiOkResponse(result));
        }
    }
}
