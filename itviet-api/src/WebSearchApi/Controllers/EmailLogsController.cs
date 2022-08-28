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
    [Route("api/emailLogs")]
    [ApiController]
    [Authorize]
    public class EmailLogsController : ControllerBase
    {
        private readonly ILogger<EmailLogsController> _logger;
        private readonly IEmailLogsService _emailLogsService;
        private readonly IMapper _mapper;

        public EmailLogsController(ILogger<EmailLogsController> logger, IEmailLogsService emailLogsService, IMapper mapper)
        {
            _logger = logger;
            _emailLogsService = emailLogsService;
            _mapper = mapper;
        }

        [DisplayName("Get All Of Email Logs")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll(int scheduleId)
        {
            var result = await _emailLogsService.GetAll(scheduleId);

            return Ok(new ApiOkResponse(result));
        }

        //[DisplayName("Send Message Again")]
        //[HttpPost]
        //[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        //[ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.Created)]
        //public async Task<IActionResult> SendMessageAgain(int scheduleId)
        //{
        //    var result = await _emailLogsService.GetAll(scheduleId);

        //    return Ok(new ApiOkResponse(result, StatusCodes.Status201Created));
        //}
    }
}