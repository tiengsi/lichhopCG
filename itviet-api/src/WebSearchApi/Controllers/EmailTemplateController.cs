using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Services;

namespace WebApi.Controllers
{
  [Route("api/emailtemplate")]
  [ApiController]
  [Authorize]
  public partial class EmailTemplateController : ControllerBase
  {
    private readonly IEmailTemplateService _emailTemplateService;

    private readonly IStatisticalService _statisticalService;
    private readonly IEmailAndSmsService _emailAndSmsService;


    public EmailTemplateController(IEmailTemplateService emailTemplateService, IStatisticalService statisticalService, IEmailAndSmsService emailAndSmsService)
    {
      _emailTemplateService = emailTemplateService;
      _statisticalService = statisticalService;
      _emailAndSmsService = emailAndSmsService;
    }




  }
}
