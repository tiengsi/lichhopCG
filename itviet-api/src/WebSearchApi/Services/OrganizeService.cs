using AutoMapper;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebApi.Data.Infrastructure;
using WebApi.Data.Repositories;
using WebApi.Helpers;
using WebApi.Helpers.Domains;
using WebApi.Helpers.Exceptions;
using WebApi.Helpers.Extensions;
using WebApi.Models;
using WebApi.Models.Dtos;
using WebApi.Models.Enums;

namespace WebApi.Services
{
  public partial interface IOrganizeService
  {

  }

  public partial class OrganizeService : IOrganizeService
  {
    private readonly IOrganizeRepository _organzieRepository;
    private readonly IScheduleRepository _scheduleRepository;
    private readonly IParticipantsRepository _participantRepository;
    private readonly IParticipantsTemplateRepository _participantTemplateRepository;
   
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<OrganizeService> _logger;
    private readonly IMapper _mapper;
    private readonly IOtherParticipantRepository _otherParticipantRepository;
    private readonly IOtherParticipantTemplateRepository _otherParticipantTemplateRepository;
    private readonly IEmailAndSmsService _emailAndSmsService;
    private readonly IEmailLogsRepository _emailLogsRepository;
    private readonly IBrandNameRepository _brandRepository;
    private readonly IEmailTemplateRepository _emailTemplateRepository;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IUserRepository _userRepository;
    private readonly IDepartmentService _departmentService;
    private readonly IUserService _userService;
    private readonly ILocationService _locationService;
    private readonly IGroupParticipantService _groupParticipantService;
    private readonly IScheduleTitleTemplateService _scheduleTitleTemplateService;


    public OrganizeService(IOrganizeRepository repository, IUnitOfWork unitOfWork, ILogger<OrganizeService> logger,
        IMapper mapper,
        IParticipantsRepository participantRepository,
        IParticipantsTemplateRepository participantTemplateRepository,
        IOtherParticipantRepository otherParticipantRepository,
        IOtherParticipantTemplateRepository otherParticipantTemplateRepository,
        IEmailAndSmsService emailAndSmsService,
        IEmailLogsRepository emailLogsRepository,
        IScheduleRepository scheduleRepository,
        IBrandNameRepository brandNameRepository,
        IEmailTemplateRepository emailTemplateRepository,
        IDepartmentRepository departmentRepository,
        IUserRepository userRepository,
        IDepartmentService departmentService,
        IUserService userService,
        ILocationService locationService,
        IGroupParticipantService groupParticipantService,
        IScheduleTitleTemplateService scheduleTitleTemplateService
    )
    {
      _organzieRepository = repository;
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
      _brandRepository = brandNameRepository;
      _emailTemplateRepository = emailTemplateRepository;
      _departmentRepository = departmentRepository;
      _userRepository = userRepository;
      _departmentService = departmentService;
      _userService = userService;
      _locationService = locationService;
      _groupParticipantService = groupParticipantService;
      _scheduleTitleTemplateService = scheduleTitleTemplateService;

    }

    private List<OrganizesTreeDto> GetSubOrganizeFunc(IEnumerable<OrganizeModel> source, int parentId)
    {
      var result = new List<OrganizesTreeDto>();
      var subOrganizeList = source.Where(x => x.OrganizeId != parentId && x.OrganizeParentId == parentId);
      if (subOrganizeList.Count() == 0) return null;
      foreach (var organizreInfo in subOrganizeList)
      {
        var newObj = new OrganizesTreeDto()
        {
          OrganizeId = organizreInfo.OrganizeId,
          Name = organizreInfo.Name,
          Address = organizreInfo.Address,
          CodeName = organizreInfo.CodeName,
          IsActive = organizreInfo.IsActive,
          OtherName = organizreInfo.OtherName,
          Order = organizreInfo.Order,
          Phone = organizreInfo.Phone,
          OrganizeParentId = parentId
        };
        newObj.SubOrganizeList = GetSubOrganizeFunc(source, organizreInfo.OrganizeId);
        result.Add(newObj);
      }
      return result;
    }
    private async Task<bool> CheckContainsFieldsInsertOrganize(OrganizeDto model)
    {
      var value = await _organzieRepository.CheckContains(m => m.Name == model.Name && m.CodeName == model.CodeName);

      return value;
    }
    private async Task<bool>CheckContaintsFieldsUpdateOrganize(OrganizeDto model)
    {
      var value = await _organzieRepository.CheckContains(m => m.Name == model.Name && m.CodeName == model.CodeName&&m.OrganizeId!=model.OrganizeId);
      return value;
    }
    private FunctionResult ValidateRequiredFieldsOrganize(OrganizeDto model)
    {
      var result = new FunctionResult();
      var regex = new Regex(@"\D");
      if (string.IsNullOrEmpty(model.Name))
      {
        result.AddError("Yêu cầu nhập tên đơn vị");
      }
      if (model.OrganizeParentId<=0)
      {
        result.AddError($"Đơn vị cha phải là số nguyên dương và lớn hơn 0");
      }
      if (model.Order<0)
      {
        result.AddError($"Thứ tự phải là số nguyên dương");
      }
      if (model.Phone == null || string.IsNullOrEmpty(model.Phone)==true) result.AddError("Số điện thoại bắt buộc nhập");
      if (model.Phone != null &&regex.IsMatch(model.Phone)) result.AddError("Số điện thoại chỉ bao gồm số");
      if (model.Phone != null &&model.Phone.Length < 10) result.AddError("Số điện thoại nhập vào không hợp lệ");
      return result;
    }
    private FunctionResult CheckOrganizeId(int organizeId)
    {
      var result = new FunctionResult();
      if (organizeId<=0)
      {
        result.AddError($"Mã đơn vị phải là số nguyên dương và lớn hơn 0");
      }
      return result;
    }
  }
}
