using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data.Infrastructure;
using WebApi.Data.Repositories;
using WebApi.Helpers.Domains;
using WebApi.Models;
using WebApi.Models.Dtos;
using WebApi.Models.Settings;

namespace WebApi.Services
{
  public partial interface IRoleService
  {

  }

  public partial class RoleService : IRoleService
  {
    RoleManager<RoleModel> _roleManager;
    IUnitOfWork _unitOfWork;
    ILogger<RoleService> _logger;
    IRoleRepository _roleRepository;
    IPermissionRepository _permissionRepository;
    public RoleService(RoleManager<RoleModel> roleManager, IUnitOfWork unitOfWork, ILogger<RoleService> logger,
        IRoleRepository roleRepository,
        IPermissionRepository permissionRepository)
    {
      _roleManager = roleManager;
      _unitOfWork = unitOfWork;
      _logger = logger;
      _roleRepository = roleRepository;
      _permissionRepository=permissionRepository;
    }

    private async Task<FunctionResult> CheckBusinessRuleBeforeInsertNewRoleAsync(RoleDto role)
    {
      var result = new FunctionResult();
      var roleExisted = await _roleRepository.GetRoleInfoByRoleName(role.Name);
      if (roleExisted!=null) result.AddError("Tên role này đã tồn tại trong hệ thống, vui lòng chọn tên role khác");

      return result;
    }

    private async Task<FunctionResult> AddPermissionMasterRoleMappingForNewRoleAsync(List<PermissionsMasterDataModel> permissionMasterRAW, string roleName)
    {
      var result = new FunctionResult();
      //var permissionMasterFobidden = permissionMasterRAW.Where(x => Constants.PermissionMasterForbidden.Any(y => y==x.NamePermission));
      var numberremisisonRemove = permissionMasterRAW.RemoveAll(match => Constants.PermissionMasterForbidden.Any(y => y==match.NamePermission));
      var permissionMasterRole = new List<PermissionMasterAndRoleMappingModel>();
      foreach (var permissionMaster in permissionMasterRAW)
      {
        var isAllow = false;
        if (string.IsNullOrEmpty(permissionMaster.NamePermissionParent)==true ||
          permissionMaster.NamePermissionParent.Contains(',')==true) isAllow=true;
        var newObj = new PermissionMasterAndRoleMappingModel
        {
          NamePermission = permissionMaster.NamePermission,
          RoleName=roleName,
          IsActive=true,
          IsAllow=isAllow,
          CreatedBy=Constants.SuperAdmin,
          UpdatedBy=Constants.SuperAdmin,
          CreatedDate=DateTime.Now,
          UpdatedDate=DateTime.Now
        };
        permissionMasterRole.Add(newObj);
      }
      var resultInsert = await _permissionRepository.InsertPermissionMasterAndRoleMappingByNewRoleAsync(permissionMasterRole);
      if (resultInsert==false) result.AddError("Có lỗi trong quá trình thêm Permission cho Role mới");
      return result;
    }


  }
}
