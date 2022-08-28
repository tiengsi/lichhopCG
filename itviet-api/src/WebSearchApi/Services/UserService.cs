using AutoMapper;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebApi.Data.Infrastructure;
using WebApi.Data.Repositories;
using WebApi.Helpers.Domains;
using WebApi.Helpers.Exceptions;
using WebApi.Models;
using WebApi.Models.Dtos;

namespace WebApi.Services
{
  public interface IUserService
  {
    Task<FunctionResult> CreateUserAsync(UserForCreateDto model);

    Task<PaginationSet<UserForListDto>> GetAll(string filter, bool? isOfficer, string sortOrder, string sortField, int organizeId, int index, int pageSize);

    Task<List<OfficerForSelectDto>> GetOfficerForSelect(int departmentId, int organizeId, int isHost);

    Task<FunctionResult> DeleteUserByIdAsync(int userId);

    Task<UserForCreateDto> GetUserById(int userId);

    Task<UserForCreateDto> GetByUserName(string username);

    Task<FunctionResult> UpdateUSerByIdAsync(UserForCreateDto model);

    Task<int> ChangePassword(ChangePasswordDto model, string currentUserName);
    Task<int> ResetPassword(ResetPasswordDto model);
    Task<PaginationSet<UserForListDto>> GetListUserByOrganizeIdAsync(string filter, bool? isOfficer, string sortOrder, string sortField, int organizeId, int index = 1, int pageSize = 10);
    Task<FunctionResult> DeleteListUserByOrganizeIdAsync(int organizeId);
  }

  public class UserService : IUserService
  {
    private readonly UserManager<UserModel> _userManager;
    private readonly ILogger<UserService> _logger;
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IEmailLogsRepository _emailLogRepository;
    private readonly IMapper _mapper;
    private readonly SignInManager<UserModel> _signInManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IScheduleRepository _scheduleRepository;
    private readonly IScheduleTemplateRepository _scheduleTemplateRepository;
    private readonly IScheduleHistoryRepository _scheduleHistoryRepository;

    public UserService(UserManager<UserModel> userManager,
        ILogger<UserService> logger,
        IUserRepository userRepository,
        IMapper mapper,
        IRoleRepository roleRepository,
        SignInManager<UserModel> signInManager,
        IDepartmentRepository departmentRepository,
        IEmailLogsRepository emailLogRepository,
        IUnitOfWork unitOfWork,
        IScheduleRepository scheduleRepository,
        IScheduleHistoryRepository scheduleHistoryRepository,
        IScheduleTemplateRepository scheduleTemplateRepository)
    {
      _userManager = userManager;
      _logger = logger;
      _userRepository = userRepository;
      _mapper = mapper;
      _roleRepository = roleRepository;
      _signInManager = signInManager;
      _departmentRepository = departmentRepository;
      _emailLogRepository = emailLogRepository;
      _unitOfWork = unitOfWork;
      _scheduleRepository = scheduleRepository;
      _scheduleHistoryRepository = scheduleHistoryRepository;
      _scheduleTemplateRepository = scheduleTemplateRepository;
    }

    public async Task<int> ChangePassword(ChangePasswordDto model, string currentUserName)
    {
      var foundUser = await _userRepository.GetSingleByCondition(x => x.UserName.Equals(model.UserName));
      if (foundUser == null)
      {
        _logger.LogWarning($"{model.UserName} is not exist!");
        throw new BusinessException($"{model.UserName} không tồn tại", StatusCodes.Status404NotFound);
      }
      if (!model.UserName.Equals(currentUserName))
      {
        _logger.LogWarning($"{model.UserName} is not match!");
        throw new BusinessException($"{model.UserName} không đúng yêu cầu", StatusCodes.Status500InternalServerError);
      }

      var checkOldPass = await _signInManager.CheckPasswordSignInAsync(foundUser, model.OldPassword, false);

      //check password incorrect
      if (!checkOldPass.Succeeded)
      {
        _logger.LogWarning($"Old password is not match!");
        throw new BusinessException("Mật khẩu cũ bạn nhập không chính xác", StatusCodes.Status500InternalServerError);
      }

      var token = await _userManager.GeneratePasswordResetTokenAsync(foundUser);
      var result = await _userManager.ResetPasswordAsync(foundUser, token, model.NewPassword);

      if (!result.Succeeded)
      {
        throw new BusinessException("Thay đổi mật khẩu không thành công!", StatusCodes.Status500InternalServerError);
      }

      return foundUser.Id;
    }

    public async Task<int> ResetPassword(ResetPasswordDto model)
    {
      var foundUser = await _userRepository.GetSingleById(model.UserId);
      if(foundUser == null)
      {
        _logger.LogWarning($"{model.UserId} is not exist!");
        throw new BusinessException($"{model.UserId} không tồn tại", StatusCodes.Status404NotFound);
      }
      var token = await _userManager.GeneratePasswordResetTokenAsync(foundUser);
      var result = await _userManager.ResetPasswordAsync(foundUser, token, model.NewPassword);

      if (!result.Succeeded)
      {
        throw new BusinessException("Thay đổi mật khẩu không thành công!", StatusCodes.Status500InternalServerError);
      }

      return foundUser.Id;
    }
    private FunctionResult ValidateRequiredFieldsUser(UserForCreateDto model)
    {
      var result = new FunctionResult();
      var regex = new Regex(@"\D");
      if (model.PhoneNumber == null || string.IsNullOrEmpty(model.PhoneNumber)==true) result.AddError("Số điện thoại bắt buộc nhập");
      if (model.PhoneNumber != null &&regex.IsMatch(model.PhoneNumber)) result.AddError("Số điện thoại chỉ bao gồm số");
      if (string.IsNullOrEmpty(model.UserName)) result.AddError("Yêu cầu nhập tên username!");
      if (!model.Roles.Any()) result.AddError("Yêu cầu nhập roles!");
      if (string.IsNullOrEmpty(model.Password)) result.AddError("Yêu cầu nhập password");
      if (model.OrganizeId <0) result.AddError("OrganizeId phải là số nguyên dương");
      if (model.DptId < 0) result.AddError("Mã bộ phận phải là số nguyên dương");
      if (model.PhoneNumber != null &&model.PhoneNumber.Length < 10) result.AddError("Số điện thoại nhập vào không hợp lệ");
      return result;
    }
    public bool CheckIsExistSpace(UserForCreateDto model)
    {
      if (model.UserName.Contains(" ")) return false;
      return true;
    }
    public async Task<FunctionResult> CreateUserAsync(UserForCreateDto model)
    {

      var isValid = ValidateRequiredFieldsUser(model);
      if (isValid.IsSuccess == false) return isValid;
      var isExist = await _userRepository.CheckContains(x => x.UserName.Equals(model.UserName));
      if (isExist)
      {
        isValid.AddError($"Tài khoản {model.UserName} đã tồn tại");
        return isValid;
      }
      var checkRole = await _userRepository.CheckUserRoleAsync(model.Roles);
      if (checkRole == false)
      {
        isValid.AddError("Role này không phù hợp");
        return isValid;
      }
      var isExistSpace = CheckIsExistSpace(model);
      if (isExistSpace==false)
      {
        isValid.AddError("Tên tài khoản không được chứa khoảng trắng");
        return isValid;
      }
      var user = model.ToModel();
      var result = await _userManager.CreateAsync(user, model.Password);
      if (!result.Succeeded)
      {
        isValid.AddError("Tạo user không thành công");
        return isValid;
      }
      var foundUser = await _userManager.FindByNameAsync(user.UserName);
      await _userManager.AddToRolesAsync(foundUser, model.Roles);
      isValid.SetData(foundUser.Id);
      return isValid;
    }

    public async Task<FunctionResult> UpdateUSerByIdAsync(UserForCreateDto model)
    {
      var isExist = await _userRepository.CheckContains(x => x.Id == model.Id);
      var response = new FunctionResult();
      if (!isExist)
      {
        response.AddError($"Mã user này không tồn tại");
        return response;
      }
      var isValid = ValidateRequiredFieldsUser(model);
      if (isValid.IsSuccess == false) return isValid;
      var isCheckContains = await _userRepository.CheckContains(x => x.UserName.Equals(model.UserName)&&x.Id!=model.Id);
      if (isCheckContains)
      {
        response.AddError($"Tài khoản {model.UserName} đã tồn tại");
        return response;
      }
      var checkRole = await _userRepository.CheckUserRoleAsync(model.Roles);
      if (checkRole==false)
      {
        response.AddError("Role này không phù hợp");
        return response;
      }
      var user = model.ToModel();
      try
      {
        var foundUser = await _userRepository.GetSingleById(model.Id);
        foundUser.FullName = model.FullName;
        foundUser.Email = model.Email;
        //foundUser.UserName = model.UserName;
        foundUser.UpdatedDate = DateTime.Now;
        foundUser.PhoneNumber = model.PhoneNumber;        
        foundUser.OfficerPosition = model.OfficerPosition;
        foundUser.DptId = model.DptId == 0 ? null : model.DptId;
        foundUser.IsHost = model.IsHost;
        foundUser.IsShow = model.IsShow;
        foundUser.ShortName = model.ShortName;
        foundUser.OrganizeId = model.OrganizeId == 0 ? null : model.OrganizeId;
        //await _userManager.RemovePasswordAsync(foundUser);
        //await _userManager.AddPasswordAsync(foundUser, model.Password);
        var updateUser = await _userManager.UpdateAsync(foundUser);
        if (!updateUser.Succeeded)
        {
          response.AddError("Cập nhật user không thành công");
          return response;
        }
        // remove old role
        var userRoles = await _userManager.GetRolesAsync(foundUser);
        if (userRoles.Any())
        {
          await _userManager.RemoveFromRolesAsync(foundUser, userRoles);
        }
        // add new role
        await _userManager.AddToRolesAsync(foundUser, model.Roles);
        response.SetData(foundUser.Id);
      }
      catch (Exception ex)
      {
        throw ex;
      }
      return response;
    }
    public async Task<FunctionResult> DeleteUserByIdAsync(int userId)
    {
      var isExist = await _userRepository.CheckContains(x => x.Id == userId);
      var response = new FunctionResult();
      if (!isExist)
      {
        response.AddError("Mã user không hợp lệ");
        return response;
      }

      var checkContainsRoleSuper = await _roleRepository.CheckUserIsSuperAdminAsync(userId);
      if (checkContainsRoleSuper)
      {
        response.AddError("SuperAdmin không được xóa");
        return response;
      }
      bool isSuperAdmin = false;
      await _scheduleTemplateRepository.AssignValueUserIdInScheduleTemplate(userId, isSuperAdmin);
      await _scheduleRepository.AssignValueUserIdInSchedule(userId, isSuperAdmin);
      await _scheduleHistoryRepository.AssignValueUserIdInScheduleHistory(userId, isSuperAdmin);
      await _emailLogRepository.AssignValueUserIdInEmailLog(userId, isSuperAdmin);
      _unitOfWork.Commit();
      var foundUser = await _userRepository.GetSingleById(userId);
      var result = await _userManager.DeleteAsync(foundUser);
      if (!result.Succeeded)
      {
        response.AddError("Xóa user không thành công");
        return response;
      }
      response.SetData(foundUser.Id);
      return response;
    }
    public async Task<FunctionResult> DeleteListUserByOrganizeIdAsync(int organizeId)
    {
      var result = new FunctionResult();
      var listUserByOrganizeId = await _userRepository.GetMulti(m => m.OrganizeId == organizeId);
      if (listUserByOrganizeId.Count() == 0) return result;
      foreach (var item in listUserByOrganizeId)
      {
        var userRole = await _roleRepository.CheckUserIsSuperAdminAsync(item.Id);
        var isSuperAdmin = true;
        if (userRole==true)
        {
          item.OrganizeId = null;
          _userRepository.Update(item);
        }
        else
        {
          isSuperAdmin = false;
          _userRepository.Delete(item);
        }
        await _scheduleTemplateRepository.AssignValueUserIdInScheduleTemplate(item.Id, isSuperAdmin);
        await _emailLogRepository.AssignValueUserIdInEmailLog(item.Id, isSuperAdmin);
        await _scheduleRepository.AssignValueUserIdInSchedule(item.Id, isSuperAdmin);
        await _scheduleHistoryRepository.AssignValueUserIdInScheduleHistory(item.Id, isSuperAdmin);
      }
      _unitOfWork.Commit();
      return result;
    }

    public async Task<PaginationSet<UserForListDto>> GetAll(string filter, bool? isOfficer, string sortOrder, string sortField, int organizeId, int index, int pageSize)
    {
      var userRole = await _roleRepository.GetSingleByCondition(x => x.Name.Equals("User"));
      var isDesc = sortOrder == "desc" ? true : false;

      Expression<Func<UserModel, bool>> baseFilter = user => !user.UserName.Equals("superadmin");

      if (isOfficer.HasValue)
      {
        if (isOfficer.Value)
        {
          baseFilter = baseFilter.And(x => x.UserRoles.Any(a => a.RoleId == userRole.Id));
        }
        else
        {
          baseFilter = baseFilter.And(x => !x.UserRoles.Any(a => a.RoleId == userRole.Id));
        }
      }

      try
      {
        baseFilter = baseFilter.And(x => x.OrganizeId == organizeId);
      }
      catch { }

      if (!string.IsNullOrEmpty(filter))
      {
        filter = filter.ToLower();
        baseFilter = baseFilter.And(x => x.UserName.ToLower().Contains(filter) || x.FullName.ToLower().Contains(filter));
      }
      var total = await _userRepository.Count(baseFilter);
      var listUser = await _userRepository.GetMultiPaging(baseFilter, sortField, isDesc, index, pageSize, new string[] { "UserRoles", "UserRoles.Role", "Department" });

      var userMap = _mapper.Map<IEnumerable<UserForListDto>>(listUser);

      return new PaginationSet<UserForListDto>()
      {
        Items = userMap,
        TotalCount = total,
        Page = index
      };
    }

    public async Task<UserForCreateDto> GetUserById(int userId)
    {
      var foundUser = await _userRepository.GetSingleById(userId);

      if (foundUser == null)
      {
        return null;
      }
      var userMap = _mapper.Map<UserForCreateDto>(foundUser);
      var roles = await _userManager.GetRolesAsync(foundUser);
      userMap.Roles = roles.ToList();
      return userMap;
    }

    public async Task<PaginationSet<UserForListDto>> GetListUserByOrganizeIdAsync(string filter, bool? isOfficer, string sortOrder, string sortField, int organizeId, int index = 1, int pageSize = 10)
    {
      var isDesc = sortOrder == "desc" ? true : false;
      Expression<Func<UserModel, bool>> baseFilter = user => user == user;
      baseFilter = baseFilter.And(x => x.OrganizeId == organizeId);
      var total = await _userRepository.Count(baseFilter);
      var listUser = await _userRepository.GetMultiPaging(baseFilter, sortField, isDesc, index, pageSize, new string[] { "UserRoles", "UserRoles.Role", "Department" });
      var userMap = _mapper.Map<IEnumerable<UserForListDto>>(listUser);

      return new PaginationSet<UserForListDto>()
      {
        Items = userMap,
        TotalCount = total,
        Page = index
      };
      //var foundUser = await _userRepository.GetMulti(m => m.OrganizeId == organizeId);
      //if (foundUser == null) return new List<UserForCreateDto>();      
      //var userMap = _mapper.Map<List<UserForCreateDto>>(foundUser);
      //var listRoles = new List<string>(); 
      //var users = new List<UserForCreateDto>();
      //foreach(var item in foundUser)
      //{
      //  var roless = await _userManager.GetRolesAsync(item); // lấy ra list roles tương ứng
      //  listRoles.AddRange(roless);
      //  var userMap1 = _mapper.Map<UserForCreateDto>(item);
      //  userMap1.Roles.AddRange(listRoles);
      //  listRoles.RemoveRange(0, listRoles.Count());
      //  users.Add(userMap1);
      //}
      //return users;

    }
    public async Task<UserForCreateDto> GetByUserName(string username)
    {
      var foundUser = await _userRepository.GetSingleByCondition(x => x.UserName.ToLower() == username.ToLower());
      if (foundUser == null)
      {
        _logger.LogWarning($"UserId {username} is not found");
        throw new BusinessException($"UserI {username} không tìm thấy", StatusCodes.Status404NotFound);
      }

      var userMap = _mapper.Map<UserForCreateDto>(foundUser);
      var roles = await _userManager.GetRolesAsync(foundUser);
      userMap.Roles = roles.ToList();
      return userMap;
    }

    public async Task<List<OfficerForSelectDto>> GetOfficerForSelect(int departmentId, int organizeId, int isHost)
    {
      var userRole = await _roleRepository.GetSingleByCondition(x => x.Name.Equals("User"));

      Expression<Func<UserModel, bool>> baseFilter = user => !user.UserName.Equals("superadmin");

      baseFilter = baseFilter.And(x => x.UserRoles.Any(a => a.RoleId == userRole.Id));

      if (departmentId != 0)
      {
        //var departmentChild = await _departmentRepository.GetAllSelect(x => x.ParentId == departmentId, x => new { x.Id });
        //departmentChild.Add(new { Id = departmentId });
        baseFilter = baseFilter.And(x => x.DptId == departmentId);
      }
      if(organizeId != 0)
      {
        baseFilter = baseFilter.And(x => x.OrganizeId == organizeId);
      }

      if (isHost != -1)
      {
        if (isHost == 0)
        {
          baseFilter = baseFilter.And(x => !x.IsHost);
        }
        else
        {
          baseFilter = baseFilter.And(x => x.IsHost);
        }
      }

      var listUser = await _userRepository.GetMulti(baseFilter);

      var userMap = _mapper.Map<List<OfficerForSelectDto>>(listUser);

      return userMap;
    }


  }
}
