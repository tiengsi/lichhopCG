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
    Task<FunctionResult> DeleteRoleByIdAsync(int roleId);
  }

  public partial class RoleService : IRoleService
  {

    public async Task<FunctionResult> DeleteRoleByIdAsync(int roleId)
    {
      var result = new FunctionResult();
      
      if (roleId<=0) result.AddError("Id không hợp lệ");      
      var foundItem = await _roleRepository.GetRoleInfoByRoleIdAsync(roleId);
      if (foundItem==null) result.AddError("Không tồn tại role này trong hệ thống");

      if(foundItem!=null && foundItem.Name==Constants.SuperAdmin) result.AddError("Bạn không được phép xóa superadmin");
      if (result.IsSuccess==true)
      {
        
        var resultDelete = await _roleRepository.DeleteRoleByIdAsync(foundItem);
        if (resultDelete==false)
        {
          result.AddError("Đã có lỗi xảy ra trong quá trình xóa role");
          return result;
        }
        //remove all PermissionMasterAndRole follow by deleted Role
        resultDelete=await _permissionRepository.DeletePermissionMasterAndRoleMappingByNewRoleAsync(foundItem.Name);
      }
      return result;


    }
  }
}
