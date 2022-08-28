using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApi.Helpers.Domains;
using WebApi.Models;
using WebApi.Models.Dtos;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Route("api/settings")]
    [ApiController]
    [Authorize]
    public class SettingController : ControllerBase
    {
        private readonly ISettingService _settingService;

        public SettingController(ISettingService settingService)
        {
            _settingService = settingService;
        }

        [DisplayName("Get All Of Setting")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _settingService.GetAll();

            return Ok(new ApiOkResponse(result));
        }

        [DisplayName("Get A Setting")]
        [HttpGet]
        [Route("{key}")]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
        
        public async Task<IActionResult> GetById(string key)
        {
            var result = await _settingService.GetByKey(key);

            return Ok(new ApiOkResponse(result));
        }

        [DisplayName("Update Settings")]
        [HttpPut]
        [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        public async Task<IActionResult> Update([FromBody]List<SettingDto> model)
        {
            if (!model.Any())
            {
                return BadRequest("Không có bản ghi nào được gửi lên để update!");
            }

            var result = await _settingService.Update(model);

            return Ok(new ApiOkResponse(result, StatusCodes.Status201Created, true, string.Empty));
        }
    }
}
