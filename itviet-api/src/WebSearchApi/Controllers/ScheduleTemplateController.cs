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
    [Route("api/schedules-template")]
    [ApiController]
    [Authorize]
    public class ScheduleTemplateController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;
        private readonly IStatisticalService _statisticalService;
        public ScheduleTemplateController(IScheduleService scheduleService, IStatisticalService statisticalService)
        {
            _scheduleService = scheduleService;
            _statisticalService = statisticalService;
        }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="host"></param>
    /// <param name="locationId"></param>
    /// <param name="active"></param>
    /// <param name="status"></param>
    /// <param name="selectAllWeek"></param>
    /// <param name="sortOrder"></param>
    /// <param name="sortField"></param>
    /// <returns></returns>
        [DisplayName("Get All Of Schedule By Week")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
        //[Authorize(Roles = "SuperAdmin,Admin")]
        [Route("new-version-by-week")]
        public async Task<IActionResult> GetAllByWeek(
            int host,
            int? locationId,
            int active,
            int status,
            bool selectAllWeek,
            string sortOrder,
            string sortField)
        {
            var result = await _scheduleService.GetAllTemplateByWeek(host,locationId, active, status, selectAllWeek, sortOrder, sortField);

            return Ok(new ApiOkResponse(result));
        }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="host"></param>
    /// <param name="locationId"></param>
    /// <param name="active"></param>
    /// <param name="status"></param>
    /// <param name="selectAllWeek"></param>
    /// <param name="sortOrder"></param>
    /// <param name="sortField"></param>
    /// <returns></returns>
    [DisplayName("Get All Of Schedule Template By Week and OrganizeId")]
    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    //[Authorize(Roles = "SuperAdmin,Admin")]
    [Route("GetAllScheduleTemplateByOrganizeIdAndWeek")]
    public async Task<IActionResult> GetAllScheduleTempByOrganizeIdAndWeekAsync(
                                                                              int organizeId,
                                                                              int host,
                                                                              int? locationId,
                                                                              int active,
                                                                              int status,
                                                                              bool selectAllWeek,
                                                                              string sortOrder,
                                                                              string sortField)
    {
      var result = await _scheduleService.GetAllScheduleTemplateByWeekAndOrganizeIdAsync(organizeId,host, locationId, active, status, selectAllWeek, sortOrder, sortField);

      return Ok(new ApiOkResponse(result));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [DisplayName("Create Schedule Template")]
        [HttpPost]
        [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
      
        public async Task<IActionResult> Create([FromBody] ScheduleTemplateForAddDto model)
        {
            if (string.IsNullOrEmpty(model.ScheduleTitle))
            {
                return BadRequest("ScheduleTitle is required");
            }

            if (string.IsNullOrEmpty(model.ScheduleTime))
            {
                return BadRequest("ScheduleTime is required");
            }

            var result = await _scheduleService.CreateTemplate(model);

            return Ok(new ApiOkResponse(result, StatusCodes.Status201Created));
        }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="scheduleId"></param>
    /// <returns></returns>
        [DisplayName("Get A Schedule Templete")]
        [HttpGet]
        [Route("{scheduleId}")]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetById(int scheduleId)
        {
            var result = await _scheduleService.GetTemplateById(scheduleId);

            return Ok(new ApiOkResponse(result));
        }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
        [DisplayName("Update Schedule Template")]
        [HttpPut]
        [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]

        public async Task<IActionResult> UpdateScheduleTemplateAsync([FromBody] ScheduleTemplateForAddDto model)
        {
            if (model.ScheduleId == 0)
            {
                return BadRequest("ScheduleId is required");
            }

            if (string.IsNullOrEmpty(model.ScheduleTime))
            {
                return BadRequest("ScheduleTime is required");
            }

            var result = await _scheduleService.UpdateScheduleTemplateAsync(model);

            return Ok(new ApiOkResponse(result, StatusCodes.Status200OK));
        }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="scheduleId"></param>
    /// <returns></returns>
        [DisplayName("Delete Schedule Template")]
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
        [Route("{scheduleId}")]
        
        public async Task<IActionResult> DeleteScheduleTemplateAsync(int scheduleId)
        {
            var result = await _scheduleService.DeleteScheduleTemplateAsync(scheduleId);

            return Ok(new ApiOkResponse(result));
        }

    }
}
