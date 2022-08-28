using AutoMapper;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApi.Data;
using WebApi.Data.Infrastructure;
using WebApi.Data.Repositories;
using WebApi.Helpers.Domains;
using WebApi.Helpers.Exceptions;
using WebApi.Helpers.Extensions;
using WebApi.Models;
using WebApi.Models.Dtos;

namespace WebApi.Services
{
  public interface IGroupParticipantService
  {
    Task<FunctionResult> CreateGroupParticipantAsync(GroupParticipantForCreateDto model);

    Task<PaginationSet<GroupParticipantForListDto>> GetAll(string name, string sortOrder, string sortField, int organizeId, int index = 1, int pageSize = 10);

    //Task<List<GroupMeetingForSelectDto>> GetAllForSelect();

    //Task<List<UserForListDto>> GetUserByGroupMeetingId(int groupMeetingId);

    Task<FunctionResult> DeleteGroupParticipantByIdAsync(int id);
    Task<bool> DeleteListGroupParticipantByOrganizeIdAsync(int organizeId);

    Task<GroupParticipantForCreateDto> GetGroupParticipantByIdAsync(int id);

    Task<FunctionResult> UpdateGroupParticipantAsync(GroupParticipantForCreateDto model);
    Task<IEnumerable<GroupParticipantForCreateDto>> GetListGroupParticipantByOrganizeIdAsync(int organizeId);
  }
  public class GroupParticipantService : IGroupParticipantService
  {
    IGroupParticipantRepository _repository;
    IGroupDepartmentRepository _groupDepartmentRepository;
    IUnitOfWork _unitOfWork;
    ILogger<GroupParticipantService> _logger;
    IUserParticipantRepository _userParticipantRepository;
    IOtherParticipantRepository _otherParticipantRepository;
    private readonly IMapper _mapper;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IUserRepository _userRepository;
    private readonly IScheduleRepository _scheduleRepository;
    private readonly IEmailLogsRepository _emailLogsRepository;


    public GroupParticipantService(
        IGroupParticipantRepository repository,
        IGroupDepartmentRepository groupDepartmentRepository,
        IUnitOfWork unitOfWork,
        ILogger<GroupParticipantService> logger,
        IUserParticipantRepository userParticipantRepository,
        IOtherParticipantRepository otherParticipantRepository,
        IDepartmentRepository departmentRepository,
        IUserRepository userRepository,
        IScheduleRepository scheduleRepository,
        IEmailLogsRepository emailLogsRepository,
        IMapper mapper)
    {
      _unitOfWork = unitOfWork;
      _logger = logger;
      _repository = repository;
      _mapper = mapper;
      _groupDepartmentRepository = groupDepartmentRepository;
      _userParticipantRepository = userParticipantRepository;
      _otherParticipantRepository = otherParticipantRepository;
      _departmentRepository = departmentRepository;
      _userRepository = userRepository;
      _scheduleRepository = scheduleRepository;
      _emailLogsRepository = emailLogsRepository;

    }

    public FunctionResult ValidateRequiredFields(GroupParticipantForCreateDto model)
    {
      var response = new FunctionResult();
      if (string.IsNullOrEmpty(model.Name)) response.AddError("Yêu cầu nhập name!");
      if (model.OrganizeId <= 0) response.AddError("Mã đơn vị phải là số nguyên dương và lớn hơn 0");
      return response;
    }
    public async Task<FunctionResult> CheckContainFields(GroupParticipantForCreateDto model)
    {
      var response = new FunctionResult();
      foreach (var item in model.DepartmentIds)
      {
        var isExistDepartmentId = await _departmentRepository.CheckContains(m => m.Id == item);
        if (isExistDepartmentId == false) response.AddError($"Mã department={item} không tồn tại");
      }
      foreach (var item in model.UserIds)
      {
        var isExistUserId = await _userRepository.CheckContains(m => m.Id == item);
        if (isExistUserId == false) response.AddError($"Mã user={item} không tồn tại");
      }
      //foreach (var item in model.OtherParticipants)
      //{
      //  var isExistScheduleId = await _scheduleRepository.CheckContains(m => m.ScheduleId == item.ScheduleId);
      //  if (isExistScheduleId == false) response.AddError($"Mã schedule={item.ScheduleId} không tồn tại");
      //}
      return response;
    }
    public async Task<FunctionResult> CreateGroupParticipantAsync(GroupParticipantForCreateDto model)
    {
      var result = ValidateRequiredFields(model);
      if (result.IsSuccess == false) return result;
      var checkContainFields = await CheckContainFields(model);
      if (checkContainFields.IsSuccess == false) return checkContainFields;
      var isExist = await _repository.CheckContains(x => x.GroupParticipantName.ToLower().Equals(model.Name.ToLower()) && x.OrganizeId == model.OrganizeId);
      if (isExist)
      {
        result.AddError($"{model.Name} và {model.OrganizeId} đã tồn tại");
        return result;
      }
      var createModel = new GroupParticipantModel();
      createModel.UpdateGroupParticipant(model);
      _repository.Add(createModel);
      _unitOfWork.Commit();
      result.SetData(createModel.GroupParticipantId);
      return result;
    }

    public async Task<PaginationSet<GroupParticipantForListDto>> GetAll(
        string name,
        string sortOrder,
        string sortField,
        int organizeId,
        int index = 1,
        int pageSize = 10)
    {
      Expression<Func<GroupParticipantModel, bool>> baseFilter = post => true;
      var isDesc = sortOrder == "desc" ? true : false;
      if (!string.IsNullOrEmpty(name))
      {
        name = name.ToLower();
        baseFilter = baseFilter.And(x => x.GroupParticipantName.ToLower().Contains(name));
      }

      if (organizeId != 0)
      {
        baseFilter = baseFilter.And(x => x.OrganizeId == organizeId);
      }

      var total = await _repository.Count(baseFilter);
      var result = await _repository.GetMultiPaging(baseFilter, sortField, isDesc, index, pageSize, new string[] {
                "GroupDepartmentModels.Department",
                "UserParticipantModels.User",
                "OtherParticipantModels"
            });

      var mappResult = _mapper.Map<List<GroupParticipantForListDto>>(result);

      return new PaginationSet<GroupParticipantForListDto>()
      {
        Items = mappResult,
        TotalCount = total,
        Page = index
      };
    }

    public async Task<FunctionResult> DeleteGroupParticipantByIdAsync(int id)
    {
      var result = new FunctionResult();
      var isExist = await _repository.CheckContains(x => x.GroupParticipantId == id);
      if (!isExist)
      {
        result.AddError("Không tìm thấy mã này!");
        return result;
      }
      var oldOtherParticipant = await _otherParticipantRepository.GetMulti(x => x.GroupParticipantId == id);
      foreach (var item in oldOtherParticipant)
      {
        _otherParticipantRepository.Delete(item);
        _emailLogsRepository.DeleteMulti(m => m.OtherParticipantId == item.OtherParticipantId);
      }

      var foundItem = await _repository.GetSingleById(id);
      _repository.Delete(foundItem);
      _unitOfWork.Commit();
      result.SetData(foundItem.GroupParticipantId);
      return result;
    }
    public async Task<bool> DeleteListGroupParticipantByOrganizeIdAsync(int organizeId)
    {
      var listGroupParticipantByOrganizeId = await _repository.GetMulti(m => m.OrganizeId == organizeId);
      if (listGroupParticipantByOrganizeId.Count() == 0) return true;
      foreach (var item in listGroupParticipantByOrganizeId)
      {
        await _otherParticipantRepository.DeleteGroupParticipantIdInOtherParticipant(item.GroupParticipantId);
      }
      var isSuccess = await _repository.DeleteListGroupParticipantByOrganizeIdAsync(organizeId);
      if (isSuccess == true) return true;
      return false;
    }

    public async Task<GroupParticipantForCreateDto> GetGroupParticipantByIdAsync(int id)
    {
      var foundItem = await _repository.GetSingleByCondition(x => x.GroupParticipantId == id, new string[] {
                "GroupDepartmentModels",
                "UserParticipantModels",
                "OtherParticipantModels"
            });
      if (foundItem == null)
      {
        return null;
      }
      var mappResult = _mapper.Map<GroupParticipantForCreateDto>(foundItem);
      return mappResult;
    }
    public async Task<IEnumerable<GroupParticipantForCreateDto>> GetListGroupParticipantByOrganizeIdAsync(int organizeId)
    {
      var groupParticipants = await _repository.GetListGroupParticipantByOrganizeIdAsync(organizeId);
      if (groupParticipants.Count() == 0) return new List<GroupParticipantForCreateDto>();
      var map = _mapper.Map<IEnumerable<GroupParticipantForCreateDto>>(groupParticipants);
      return map;
    }

    public async Task<FunctionResult> UpdateGroupParticipantAsync(GroupParticipantForCreateDto model)
    {
      var result = ValidateRequiredFields(model);
      if (result.IsSuccess == false) return result;
      var checkContainFields = await CheckContainFields(model);
      if (checkContainFields.IsSuccess == false) return checkContainFields;
      var foundItem = await _repository.GetSingleById(model.Id);
      if (foundItem == null)
      {
        result.AddError($"Không tìm thấy bản ghi {model.Id}");
        return result;
      }
      var isExist = await _repository.CheckContains(x => x.GroupParticipantName.ToLower().Equals(model.Name.ToLower()) && x.OrganizeId == model.OrganizeId && x.GroupParticipantId != model.Id);
      if (isExist)
      {
        result.AddError($"{model.Name} và {model.OrganizeId} đã tồn tại");
        return result;
      }
      // delete old department
      var departments = await _groupDepartmentRepository.GetMulti(x => x.GroupParticipantId == model.Id);
      foreach (var item in departments)
      {
        _groupDepartmentRepository.Delete(item);
      }
      // delete old users
      var users = await _userParticipantRepository.GetMulti(x => x.GroupParticipantId == model.Id);
      foreach (var item in users)
      {
        _userParticipantRepository.Delete(item);
      }

      // delete old other participants
      var oldOtherParticipant = await _otherParticipantRepository.GetMulti(x => x.GroupParticipantId == model.Id);
      foreach (var item in oldOtherParticipant)
      {
        _otherParticipantRepository.Delete(item);
        _emailLogsRepository.DeleteMulti(m => m.OtherParticipantId == item.OtherParticipantId);
      }
      foundItem.UpdateGroupParticipant(model);
      _repository.Update(foundItem);
      _unitOfWork.Commit();
      result.SetData(foundItem.GroupParticipantId);
      return result;
    }
  }
}
