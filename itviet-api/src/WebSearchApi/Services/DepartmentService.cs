using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

namespace WebApi.Services
{
  public interface IDepartmentService
  {
    Task<IEnumerable<DepartmentDto>> GetAll(string name, string sortOrder, string sortField, int organizeId, bool? isActive = null);

    Task<IEnumerable<DepartmentModel>> GetDepartmentOfficer(string filter, string sortField);

    Task<FunctionResult> CreateDepartmentAsync(DepartmentDto model);

    Task<FunctionResult> DeleteDepartmentByIdAsync(int id);

    Task<DepartmentModel> GetById(int id);

    Task<FunctionResult> UpdateDepartmentByIdAsync(DepartmentDto model);

    Task<ReturnResultDto<int>> PathUpdate(int orderId, JsonPatchDocument<DepartmentModel> patchDepartment);

    Task<int> UpdateRepresentative(DepartmentRepresentativePayload model);
    Task<IEnumerable<DepartmentDto>> GetListDepartmentByOrganizeIdAsync(int organizeId);
    Task<FunctionResult> DeleteDepartmentByOrganizeIdAsync(int organizeId);
  }

  public class DepartmentService : IDepartmentService
  {
    private readonly IUserRepository _userRepository;
    private readonly IDepartmentRepository _repository;
    private readonly IRepresentativeRepository _rePresentativeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DepartmentService> _logger;
    private readonly IMapper _mapper;
    private readonly IGroupDepartmentRepository _groupDepartmentRepository;

    public DepartmentService(IDepartmentRepository repository, IUnitOfWork unitOfWork, ILogger<DepartmentService> logger, IMapper mapper,
        IRepresentativeRepository rePresentativeRepository, IUserRepository userRepository, IGroupDepartmentRepository groupDepartmentRepository)
    {
      _repository = repository;
      _unitOfWork = unitOfWork;
      _logger = logger;
      _mapper = mapper;
      _rePresentativeRepository = rePresentativeRepository;
      _userRepository = userRepository;
      _groupDepartmentRepository = groupDepartmentRepository;
    }

    public async Task<IEnumerable<DepartmentDto>> GetAll(string name, string sortOrder, string sortField, int organizeId, bool? isActive = null)
    {
      Expression<Func<DepartmentModel, bool>> baseFilter = d => true;
      var isDesc = sortOrder == "desc" ? true : false;
      if (!string.IsNullOrWhiteSpace(name))
      {
        name = name.ToLower();
        baseFilter = baseFilter.And(x => x.Name.ToLower().Contains(name));
      }

      if (organizeId != 0)
      {
        baseFilter = baseFilter.And(x => x.OrganizeId == organizeId);
      }
      if (isActive != null)
      {
        baseFilter = baseFilter.And(x => x.IsActive == isActive);
      }

      var departments = await _repository.GetMultiPaging(baseFilter, sortField, isDesc, 1, 500, new string[] { "RepresentativeModels.User" });
      List<DepartmentModel> objbind = new List<DepartmentModel>();

      var departTree = await GetTreeTable(null, objbind, departments, "");
      var departmentDtos = _mapper.Map<IEnumerable<DepartmentDto>>(departTree);
      foreach (var dto in departmentDtos)
      {
        var dpUser = departTree.FirstOrDefault(r => r.Id == dto.Id)?.RepresentativeModels.FirstOrDefault();

        if (dpUser == null) continue;

        dto.Representative = dpUser.User != null ? dpUser.User.FullName : "";
        dto.RepresentativeId = dpUser.UserId.ToString();
      }
      return departmentDtos;
    }
    public async Task<IEnumerable<DepartmentDto>> GetListDepartmentByOrganizeIdAsync(int organizeId)
    {
      var departments = await _repository.GetMulti(m => m.OrganizeId == organizeId);
      var departmentDtos = _mapper.Map<IEnumerable<DepartmentDto>>(departments);
      return departmentDtos;
    }


    public async Task<IEnumerable<DepartmentModel>> GetDepartmentOfficer(string filter, string sortField)
    {
      Expression<Func<DepartmentModel, bool>> baseFilter = x => x.IsActive;

      var departments = (await _repository.GetMultiPaging(baseFilter, sortField, false, 1, 500, new string[] { "SubDepartments", "Officers" }))
          .Where(x => x.ParentId == null).ToList();

      return departments;

      //if ((departments == null && !departments.Any()) || string.IsNullOrWhiteSpace(filter)) return departments;

      //List<DepartmentModel> objbind = new List<DepartmentModel>();
      //foreach (var dpm in departments)
      //{
      //    var tmpDpm = await UpdateOfficerRecursive(dpm, dpm, filter);
      //    objbind.Add(tmpDpm);
      //}

      //return objbind;
    }
    private FunctionResult ValidateRequiredFieldsDepartment(DepartmentDto model)
    {
      var result = new FunctionResult();
      //var regex = new Regex(@"\D");
      if (string.IsNullOrEmpty(model.Name)) result.AddError("Yêu cầu nhập tên bộ phận!");
      //if (string.IsNullOrEmpty(model.Email)) result.AddError("Yêu cầu nhập email!");
      if (model.OrganizeId <= 0) result.AddError("OrganizeId phải là số nguyên dương và lớn hơn 0");
      //if (model.UserRepresentative.Any() == false) result.AddError("Yêu cầu trường người đại diện phải có dữ liệu");
      //if (model.PhoneNumber == null || string.IsNullOrEmpty(model.PhoneNumber) == true) result.AddError("Số điện thoại bắt buộc nhập");
      //if (model.PhoneNumber != null && model.PhoneNumber.Length < 10) result.AddError("Số điện thoại nhập vào không hợp lệ");
      //if (model.PhoneNumber != null && regex.IsMatch(model.PhoneNumber)) result.AddError("Số điện thoại chỉ bao gồm số");
      return result;
    }
    private async Task<bool> CheckContainsFieldsInsertDepartmentAsync(DepartmentDto model)
    {
      var value = await _repository.CheckContains(x => x.Name.ToLower().Equals(model.Name.ToLower()) && x.OrganizeId == model.OrganizeId);
      return value;
    }
    private async Task<bool> CheckParentIdOffSubDepartmentEqualDepartmentIdAndOrganizeId(DepartmentDto model)
    {
      if (model.ParentId == null || model.ParentId == 0) return true;
      var isCheck = await _repository.CheckContains(m => m.Id == model.ParentId && m.OrganizeId == model.OrganizeId);
      return isCheck;
    }
    private async Task<bool> CheckContainsFieldsUpdateDepartmentAsync(DepartmentDto model)
    {
      var value = await _repository.CheckContains(x => x.Name.ToLower().Equals(model.Name.ToLower()) && x.OrganizeId == model.OrganizeId && x.Id != model.Id);
      return value;
    }
    private FunctionResult CheckValidationDepartmentId(DepartmentDto model)
    {
      var result = new FunctionResult();
      if (model.Id <= 0)
      {
        result.AddError($"Mã bộ phận phải là số nguyên dương và lớn hơn 0");
      }
      return result;
    }
    public async Task<FunctionResult> CreateDepartmentAsync(DepartmentDto model)
    {
      var result = ValidateRequiredFieldsDepartment(model);
      if (result.IsSuccess == false) return result;
      var isExist = await CheckContainsFieldsInsertDepartmentAsync(model);
      if (isExist)
      {
        result.AddError($"{model.Name} và {model.OrganizeId} đã tồn tại");
        return result;
      }
      //var checkUserRepresentative = await _repository.CheckUserRepresentativeExistedAsync(model.UserRepresentative);
      //if (checkUserRepresentative == false)
      //{
      //  result.AddError("Mã người đại diện không hợp lệ");
      //  return result;
      //}
      var isCheck = await CheckParentIdOffSubDepartmentEqualDepartmentIdAndOrganizeId(model);
      if (isCheck == false)
      {
        result.AddError("Đơn vị của phòng ban mới không trùng với đơn vị của phòng ban cấp cao hơn");
        return result;
      }
      var map = _mapper.Map<DepartmentModel>(model);

      if (map.ParentId == 0) map.ParentId = null;

      map.RepresentativeModels = new List<RepresentativeModel>();
      if (model.UserRepresentative != null && model.UserRepresentative.Any())
      {
        foreach (var item in model.UserRepresentative)
        {
          map.RepresentativeModels.Add(new RepresentativeModel()
          {
            UserId = item
          });
        }
      }
      _repository.Add(map);
      _unitOfWork.Commit();

      result.SetData(map.Id);
      return result;
    }

    public async Task<FunctionResult> DeleteDepartmentByOrganizeIdAsync(int organizeId)
    {
      var resultFinal = new FunctionResult();
      try
      {
        var departmentNeedToDeleteList = await _repository.GetMulti(m => m.OrganizeId == organizeId);
        if (departmentNeedToDeleteList.Count() == 0) return resultFinal;
        var dtpIdInUser = new List<UserModel>();
        var userListByChildDepartment = new List<UserModel>();
        var subDepartment = new List<DepartmentModel>();
        foreach (var departmentInfo in departmentNeedToDeleteList)
        {
          await _groupDepartmentRepository.DeleteGroupDepartmentByDepartmentIdAsync(departmentInfo);
          await _rePresentativeRepository.DeleteRepresentativeByDepartmentIdAsync(departmentInfo);
          var item = await _userRepository.GetMulti(m => m.DptId == departmentInfo.Id);
          dtpIdInUser.AddRange(item);
        }
        foreach (var item in dtpIdInUser)
        {
          item.DptId = null;
          _userRepository.Update(item);
        }
        // xóa department cha
        _repository.DeleteMulti(m => m.OrganizeId == organizeId);
        _unitOfWork.Commit();
      }
      catch (System.Exception ex)
      {
        resultFinal.AddError(ex.ToString());
        return resultFinal;
      }
      return resultFinal;
    }

    public async Task<FunctionResult> DeleteDepartmentByIdAsync(int id)
    {
      var result = new FunctionResult();
      var userListByDepartment = new List<UserModel>();
      var isExist = await _repository.CheckContains(x => x.Id == id);
      if (!isExist)
      {
        result.AddError("Không tìm thấy phòng ban này");
        return result;
      }
      var departmentNeedDeleteList = new List<DepartmentModel>();
      var foundItem = await _repository.GetSingleById(id);
      departmentNeedDeleteList.Add(foundItem);
      var departmentOfOrganizeList = await _repository.GetMulti(x => x.OrganizeId == foundItem.OrganizeId);
      var subDepartmentList = GetSubDepartmentFunc(departmentOfOrganizeList, foundItem.Id);
      departmentNeedDeleteList.AddRange(subDepartmentList);
      foreach (var item in departmentNeedDeleteList)
      {
        var userListByDepartmentChildId = await _userRepository.GetMulti(m => m.DptId == item.Id);
        userListByDepartment.AddRange(userListByDepartmentChildId);
      }
      foreach (var item in userListByDepartment)
      {
        item.DptId = null;
        _userRepository.Update(item);
      }
      foreach (var department in departmentNeedDeleteList)
      {
        await _groupDepartmentRepository.DeleteGroupDepartmentByDepartmentIdAsync(department);
        await _rePresentativeRepository.DeleteRepresentativeByDepartmentIdAsync(department);
        _repository.Delete(department);
      }
      _unitOfWork.Commit();
      result.SetData(foundItem.Id);
      return result;
    }

    private List<DepartmentModel> GetSubDepartmentFunc(IEnumerable<DepartmentModel> source, int parentId)
    {
      var result = new List<DepartmentModel>();
      var subDepartmentList = source.Where(x => x.Id != parentId && x.ParentId == parentId);
      if (subDepartmentList.Count() == 0) return result;
      result.AddRange(subDepartmentList);
      foreach (var departmentInfo in subDepartmentList)
      {

        var subDepartmentLevel2 = GetSubDepartmentFunc(source, departmentInfo.Id);
        if (subDepartmentLevel2 != null) result.AddRange(subDepartmentLevel2);
      }
      return result;
    }

    public async Task<DepartmentModel> GetById(int id)
    {
      var foundItem = await _repository.GetSingleByCondition(x => x.Id == id, new string[] { "RepresentativeModels" });
      if (foundItem == null)
      {
        return null;
      }
      return foundItem;
    }

    public async Task<FunctionResult> UpdateDepartmentByIdAsync(DepartmentDto model)
    {
      var result = ValidateRequiredFieldsDepartment(model);
      if (result.IsSuccess == false) return result;
      var isCheck = CheckValidationDepartmentId(model);
      if (isCheck.IsSuccess == false) return isCheck;
      var foundItem = await _repository.GetSingleByCondition(x => x.Id == model.Id.Value, new string[] { "RepresentativeModels" });
      if (foundItem == null)
      {
        result.AddError("Không tìm thấy mã phòng ban này!");
        return result;
      }
      //var checkUserRepresentative = await _repository.CheckUserRepresentativeExistedAsync(model.UserRepresentative);
      //if (checkUserRepresentative == false)
      //{
      //  result.AddError("Mã người đại diện không hợp lệ");
      //  return result;
      //}
      var isExist = await CheckContainsFieldsUpdateDepartmentAsync(model);
      if (isExist)
      {
        result.AddError($"{model.Name} và {model.OrganizeId} đã tồn tại trong hệ thống!");
        return result;
      }
      var isCheckParentId = await CheckParentIdOffSubDepartmentEqualDepartmentIdAndOrganizeId(model);
      if (isCheckParentId == false)
      {
        result.AddError("Đơn vị của phòng ban mới không trùng với đơn vị của phòng ban cấp cao hơn");
        return result;
      }
      if (foundItem.RepresentativeModels.Any())
      {
        foreach (var item in foundItem.RepresentativeModels)
        {
          _rePresentativeRepository.Delete(item);
        }
      }
      foundItem.UpdateDepartment(model);
      _repository.Update(foundItem);
      _unitOfWork.Commit();
      result.SetData(foundItem.Id);
      return result;
    }

    private async Task<List<DepartmentModel>> GetTreeTable(int? parent, List<DepartmentModel> objResult, List<DepartmentModel> objtemp, string space)
    {
      var objbind = CateChil(parent, objtemp.ToList());
      if (objbind != null && objbind.Count > 0)
      {
        foreach (var t in objbind)
        {
          var objtab = t;
          objtab.Name = space + objtab.Name;
          objResult.Add(objtab);
          await GetTreeTable(objtab.Id, objResult, objtemp, space + "--- ");
        }
      }
      else
      {
        objResult = objtemp;
      }

      return await Task.FromResult(objResult);
    }

    private List<DepartmentModel> CateChil(int? parent, List<DepartmentModel> objcate)
    {
      return objcate.Where(info => info.ParentId == parent).ToList();
    }

    private async Task<DepartmentModel> UpdateOfficerRecursive(DepartmentModel objResult, DepartmentModel objtemp, string keyFilter)
    {
      if (string.IsNullOrWhiteSpace(keyFilter)) return await Task.FromResult(objtemp);

      if (objtemp != null)
      {
        var tmpOfficers = objtemp.Officers.Where(x => x.FullName.ToLower().Contains(keyFilter.ToLower()))?.ToList();
        objtemp.Officers = tmpOfficers;

        if (objtemp.SubDepartments != null && objtemp.SubDepartments.Any())
        {
          foreach (var sub in objtemp.SubDepartments)
          {
            await UpdateOfficerRecursive(sub, sub, keyFilter);
          }
        }
      }

      return await Task.FromResult(objResult);
    }

    public async Task<ReturnResultDto<int>> PathUpdate(int orderId, JsonPatchDocument<DepartmentModel> patchDepartment)
    {
      var existItem = await _repository.GetSingleById(orderId);
      if (existItem == null)
      {
        _logger.LogError($"Not found Department");
        return new ReturnResultDto<int>()
        {
          Message = "Không tìm thấy phòng ban này",
          ResultCode = ResultCode.NOTFOUND
        };
      }

      patchDepartment.ApplyTo(existItem);
      _repository.Update(existItem);
      _unitOfWork.Commit();

      return new ReturnResultDto<int>()
      {
        Message = "Cập nhật phòng ban thành công",
        ResultCode = ResultCode.SUCCESS,
        Result = existItem.Id
      };
    }

    public async Task<int> UpdateRepresentative(DepartmentRepresentativePayload model)
    {
      var exist = await _repository.CheckContains(x => x.Id == model.DepartmentId);
      if (!exist)
      {
        _logger.LogWarning($"Not found department {model.DepartmentId}");
        throw new BusinessException($"Không tìm thấy phòng ban {model.DepartmentId}", StatusCodes.Status404NotFound);
      }

      _rePresentativeRepository.DeleteMulti(x => x.DepartmentId == model.DepartmentId);

      _rePresentativeRepository.Add(new RepresentativeModel()
      {
        UserId = model.RepresentativeId,
        DepartmentId = model.DepartmentId
      });
      _unitOfWork.Commit();

      return model.DepartmentId;
    }
  }
}
