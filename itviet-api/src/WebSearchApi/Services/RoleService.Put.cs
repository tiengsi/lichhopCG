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
    Task<FunctionResult> UpdateRoleByIdAsync(RoleDto model);
  }

  public partial class RoleService : IRoleService
  {
    public async Task<FunctionResult> UpdateRoleByIdAsync(RoleDto model)
    {
      var result = new FunctionResult();
      var updateModel = model.ToModel();
      updateModel.Id=model.Id;
      var roleExisted = await _roleRepository.GetRoleInfoByRoleIdAsync(model.Id);
      if (roleExisted==null)
      {
        result.AddError("Không tìm thấy tên role này trong hệ thống");
        return result;
      }

      var isUpdate = await _roleRepository.UpdateRoleByIdAsync(updateModel);
      if (isUpdate==false) result.AddError("Đã có lỗi xảy ra trong lúc cập nhật role vào Database");
      return result;
    }


  }
}
