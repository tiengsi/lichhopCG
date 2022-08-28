using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Helpers.Domains;
using WebApi.Models.Dtos;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Route("api/participants")]
    [ApiController]
    [Authorize]
    public class ParticipantController : ControllerBase
    {
        private readonly IParticipantService _participantService;

        public ParticipantController(IParticipantService participantService)
        {
            _participantService = participantService;
        }

        [DisplayName("Get All Of Participant For Select")]
        [HttpGet]
        [Route("select")]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll(int organizeId)
        {
            var result = await _participantService.GetParticipantForSelect(organizeId);

            return Ok(new ApiOkResponse(result));
        }

        [DisplayName("Choose Participants")]
        [HttpGet]
        [Route("{participantId}/choose")]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Choose(string participantId)
        {
            try
            {
                var result = await _participantService.ChooseParticipant(participantId);

                return Ok(new ApiOkResponse(result));
            }
            catch (Exception ex)
            {

                return Ok(new ApiOkResponse(null));
            }
        }

        [DisplayName("Choose Receiver")]
        [HttpGet]
        [Route("{participantId}/receiver")]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Receiver(string participantId)
        {
            var result = await _participantService.ChooseReceiver(participantId);

            return Ok(new ApiOkResponse(result));
        }
    }
}
