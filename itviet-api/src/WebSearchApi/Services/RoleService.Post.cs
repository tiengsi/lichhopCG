using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.Models.Settings;

namespace WebApi.Services
{
  public partial interface IRoleService
  {

    Task<FunctionResult> CreateNewRoleAsync(RoleDto model);
  }

  public partial class RoleService : IRoleService
  {

    public async Task<FunctionResult> CreateNewRoleAsync(RoleDto model)
    {
      var result = new FunctionResult();
      var createModel = model.ToModel();
      createModel.CreatedDate=DateTime.Now;
      result =await CheckBusinessRuleBeforeInsertNewRoleAsync(model);
      if (result.IsSuccess==false) return result;
      var isCreate = await _roleRepository.CreateNewRoleAsync(createModel);
      if (isCreate==false)
      {
        result.AddError("Đã có lỗi xảy ra trong lúc thêm role mới vào Database");
        return result;
      }
      var allPermissionMaster = await _permissionRepository.GetPermissionsMasterDataAsync();
      result =await AddPermissionMasterRoleMappingForNewRoleAsync(allPermissionMaster.ToList(), model.Name);
      result.SetData(createModel.Id);
      return result;
    }

    

  }
}
