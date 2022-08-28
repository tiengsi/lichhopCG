using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using WebApi.Data;
using WebApi.Helpers.Domains;
using WebApi.Models;
using WebApi.Models.Dtos;
using WebApi.Services;

namespace WebApi.Controllers
{
  [Route("api/schedules")]
  [ApiController]
  [Authorize]
  public partial class ScheduleController : ControllerBase
  {
    private readonly IScheduleService _scheduleService;
    private readonly IStatisticalService _statisticalService;
    private readonly IEmailAndSmsService _emailAndSmsService;
    private readonly IScheduleFilesAttachmentService _filesAttachmentService;
    

    public ScheduleController(IScheduleService scheduleService, IStatisticalService statisticalService, IEmailAndSmsService emailAndSmsService,
        IScheduleFilesAttachmentService filesAttachmentService)
    {
      _scheduleService = scheduleService;
      _statisticalService = statisticalService;
      _emailAndSmsService = emailAndSmsService;
      _filesAttachmentService = filesAttachmentService;
    }


   

  }
}
