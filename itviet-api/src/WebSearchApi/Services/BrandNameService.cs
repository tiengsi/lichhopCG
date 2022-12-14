using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebApi.Data.Infrastructure;
using WebApi.Data.Repositories;
using WebApi.Models;

namespace WebApi.Services
{

  public partial interface IBrandNameService
  {

  }

  public partial class BrandNameService : IBrandNameService
  {
    private readonly IBrandNameRepository _brandNameRepository;
    private readonly IScheduleRepository _scheduleRepository;
    private readonly IParticipantsRepository _participantRepository;
    private readonly IParticipantsTemplateRepository _participantTemplateRepository;

    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<BrandNameService> _logger;
    private readonly IMapper _mapper;
    private readonly IOtherParticipantRepository _otherParticipantRepository;
    private readonly IOtherParticipantTemplateRepository _otherParticipantTemplateRepository;
    private readonly IEmailAndSmsService _emailAndSmsService;
    private readonly IEmailLogsRepository _emailLogsRepository;


    public BrandNameService(IBrandNameRepository repository, IUnitOfWork unitOfWork, ILogger<BrandNameService> logger,
        IMapper mapper,
        IParticipantsRepository participantRepository,
        IParticipantsTemplateRepository participantTemplateRepository,

        IOtherParticipantRepository otherParticipantRepository,
        IOtherParticipantTemplateRepository otherParticipantTemplateRepository,
        IEmailAndSmsService emailAndSmsService,
        IEmailLogsRepository emailLogsRepository,
        IScheduleRepository scheduleRepository
    )
    {
      _brandNameRepository = repository;
      _scheduleRepository = scheduleRepository;
      _unitOfWork = unitOfWork;
      _logger = logger;
      _mapper = mapper;
      _participantRepository = participantRepository;
      _participantTemplateRepository = participantTemplateRepository;
      _otherParticipantRepository = otherParticipantRepository;
      _otherParticipantTemplateRepository = otherParticipantTemplateRepository;
      _emailAndSmsService = emailAndSmsService;
      _emailLogsRepository = emailLogsRepository;

    }

    private FunctionResult ValidateRequiredFieldsVNPTBrandName(VNPT_BrandNameModel model)
    {
      var regex = new Regex(@"\D");
      var result = ValidateRequiredFieldsBrandName(model);
      if (model.PhoneNumber.Length<10) result.AddError("S??? ??i???n tho???i nh???p v??o kh??ng h???p l???");
      if (regex.IsMatch(model.PhoneNumber)) result.AddError("S??? ??i???n tho???i ch??? bao g???m s???");
      return result;
    }

    private async Task<FunctionResult> ValidateBRVNPTBrandNameAsync(VNPT_BrandNameModel vnpt_Model)
    {
      var result = new FunctionResult();
      var branchNameExisted = await _brandNameRepository.GetVNPT_BrandNameListByOrganizeIdAsync(vnpt_Model.OrganizeId);
      if (branchNameExisted.Any()) result.AddError("????n v??? n??y ???? c?? branchName, vui l??ng x??a branchname ???? t???n t???i tr?????c khi th??m m???i");
      return result;
    }
    private async Task<bool> CheckVNPT_BrandNameExistedAsync(VNPT_BrandNameModel vnpt_Model,int actionInsert)
    {
      var brandNameExisted = await _brandNameRepository.CheckVNPTBrandNameExistedAsync(vnpt_Model,actionInsert);
      if (brandNameExisted.Count() > 0) return true;
      return false;
    }
    private async Task<bool> CheckVT_BrandNameExistedAsync(Viettel_BrandNameModel viettel_Model,int actionInsert)
    {
      var brandNameExisted = await _brandNameRepository.CheckVTBrandNameExistedAsync(viettel_Model,actionInsert);
      if (brandNameExisted.Count() > 0) return true;
      return false;
    }
    //private async Task<bool> CheckVNPT_BrandNameExistedInViettel_BrandName(VNPT_BrandNameModel model)
    //{
    //  var brandNameExisted = await _brandNameRepository.CheckVNPT_BrandNameExistedInViettel_BrandName(model);
    //  if (brandNameExisted.Count() > 0) return true;
    //  return false;
    //}
    //private async Task<bool> CheckVT_BrandNameExistedInVNPT_BrandName(Viettel_BrandNameModel model)
    //{
    //  var brandNameExisted = await _brandNameRepository.CheckVT_BrandNameExistedInVNPT_BrandName(model);
    //  if (brandNameExisted.Count() > 0) return true;
    //  return false;
    //}

    private async Task<FunctionResult> ValidateBRViettelBrandNameAsync(Viettel_BrandNameModel viettel_Model)
    {
      var result = new FunctionResult();
      var branchNameExisted = await _brandNameRepository.GetViettel_BrandNameListByOrganizeIdAsync(viettel_Model.OrganizeId);
      if (branchNameExisted.Any()) result.AddError("????n v??? n??y ???? c?? branchName, vui l??ng x??a branchname ???? t???n t???i tr?????c khi th??m m???i");
      return result;
    }

    private FunctionResult ValidateRequiredFieldsViettelBrandName(Viettel_BrandNameModel model)
    {
      var result = ValidateRequiredFieldsBrandName(model);
      if (string.IsNullOrEmpty(model.CPCode)) result.AddError("Y??u c???u nh???p tr?????ng CPCode");
      return result;
    }

    private FunctionResult ValidateRequiredFieldsBrandName(BrandNameModel model)
    {
      var result = new FunctionResult();

      if (string.IsNullOrEmpty(model.ApiUserName)) result.AddError("Y??u c???u nh???p tr?????ng API UserName!");
      if (string.IsNullOrEmpty(model.ApiPassword)) result.AddError("Y??u c???u nh???p tr?????ng API Password!");
      if (string.IsNullOrEmpty(model.BrandName)) result.AddError("Y??u c???u nh???p tr?????ng BrandName");
      if (string.IsNullOrEmpty(model.ApiLink)) result.AddError("Y??u c???u nh???p tr?????ng API Link");
      if (model.OrganizeId <= 0) result.AddError("OrganizeId ph???i l?? s??? nguy??n d????ng v?? l???n h??n 0");
      return result;
    }


    private FunctionResult CheckBrandNameId(int brandNameId)
    {
      var result = new FunctionResult();
      if (brandNameId <=0)
      {
        result.AddError($"BrandNameId ph???i l?? s??? nguy??n d????ng v?? l???n h??n 0");
      }
      return result;
    }

  }
}
