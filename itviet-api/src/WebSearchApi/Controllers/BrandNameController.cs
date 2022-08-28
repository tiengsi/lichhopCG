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
  [Route("api/brandname")]
  [ApiController]
  [Authorize]
  public partial class BrandNameController : ControllerBase
  {
    private readonly IBrandNameService _brandService;

    private readonly IStatisticalService _statisticalService;
    private readonly IEmailAndSmsService _emailAndSmsService;


    public BrandNameController(IBrandNameService brandNameService, IStatisticalService statisticalService, IEmailAndSmsService emailAndSmsService)
    {
      _brandService = brandNameService;
      _statisticalService = statisticalService;
      _emailAndSmsService = emailAndSmsService;


    }




  }
}
