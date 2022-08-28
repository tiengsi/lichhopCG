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
using WebApi.Helpers.Domains;
using WebApi.Models;
using WebApi.Models.Dtos;
using WebApi.Services;

namespace WebApi.Controllers
{
  [Route("api/organizes")]
  [ApiController]
  [Authorize]
  public partial class OrganizeController : ControllerBase
  {
    private readonly IOrganizeService _OrganizeService;
    private readonly IScheduleService _scheduleService;
  
    private readonly IStatisticalService _statisticalService;
    private readonly IEmailAndSmsService _emailAndSmsService;
    private readonly IBrandNameService _brandNameService;
    private readonly IDepartmentService _departmentService;
    private readonly IUserService _userService;


    public OrganizeController(IOrganizeService OrganizeService, IStatisticalService statisticalService,
                              IEmailAndSmsService emailAndSmsService ,IBrandNameService brandNameService,
                              IDepartmentService departmentService, IScheduleService scheduleService,
                              IUserService userService)
    {
      _OrganizeService = OrganizeService;
      _statisticalService = statisticalService;
      _emailAndSmsService = emailAndSmsService;
      _brandNameService = brandNameService;
      _departmentService = departmentService;
      _scheduleService = scheduleService;
      _userService=userService;


    }


   

  }
}
