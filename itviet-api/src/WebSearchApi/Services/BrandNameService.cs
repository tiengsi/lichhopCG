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
      if (model.PhoneNumber.Length<10) result.AddError("Số điện thoại nhập vào không hợp lệ");
      if (regex.IsMatch(model.PhoneNumber)) result.AddError("Số điện thoại chỉ bao gồm số");
      return result;
    }

    private async Task<FunctionResult> ValidateBRVNPTBrandNameAsync(VNPT_BrandNameModel vnpt_Model)
    {
      var result = new FunctionResult();
      var branchNameExisted = await _brandNameRepository.GetVNPT_BrandNameListByOrganizeIdAsync(vnpt_Model.OrganizeId);
      if (branchNameExisted.Any()) result.AddError("Đơn vị này đã có branchName, vui lòng xóa branchname đã tồn tại trước khi thêm mới");
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
      if (branchNameExisted.Any()) result.AddError("Đơn vị này đã có branchName, vui lòng xóa branchname đã tồn tại trước khi thêm mới");
      return result;
    }

    private FunctionResult ValidateRequiredFieldsViettelBrandName(Viettel_BrandNameModel model)
    {
      var result = ValidateRequiredFieldsBrandName(model);
      if (string.IsNullOrEmpty(model.CPCode)) result.AddError("Yêu cầu nhập trường CPCode");
      return result;
    }

    private FunctionResult ValidateRequiredFieldsBrandName(BrandNameModel model)
    {
      var result = new FunctionResult();

      if (string.IsNullOrEmpty(model.ApiUserName)) result.AddError("Yêu cầu nhập trường API UserName!");
      if (string.IsNullOrEmpty(model.ApiPassword)) result.AddError("Yêu cầu nhập trường API Password!");
      if (string.IsNullOrEmpty(model.BrandName)) result.AddError("Yêu cầu nhập trường BrandName");
      if (string.IsNullOrEmpty(model.ApiLink)) result.AddError("Yêu cầu nhập trường API Link");
      if (model.OrganizeId <= 0) result.AddError("OrganizeId phải là số nguyên dương và lớn hơn 0");
      return result;
    }


    private FunctionResult CheckBrandNameId(int brandNameId)
    {
      var result = new FunctionResult();
      if (brandNameId <=0)
      {
        result.AddError($"BrandNameId phải là số nguyên dương và lớn hơn 0");
      }
      return result;
    }

  }
}
