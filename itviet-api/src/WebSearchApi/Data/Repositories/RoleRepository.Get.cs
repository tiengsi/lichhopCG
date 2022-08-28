using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data.Infrastructure;
using WebApi.Models;

namespace WebApi.Data.Repositories
{
  public partial interface IRoleRepository : IRepository<RoleModel>
  {
    Task<IEnumerable<UserRoleModel>> GetRoleListByUserIdAsync(int userId);
    Task<IEnumerable<RoleModel>> GetRoleInfoByRoleIdAsync(IEnumerable<int> roleId);
    Task<bool> CheckUserIsSuperAdminAsync(int userId);
    Task<RoleModel> GetRoleInfoByRoleName(string roleName);
    Task<RoleModel> GetRoleInfoByRoleIdAsync(int roleId);
  }

  public partial class RoleRepository : RepositoryBase<RoleModel>, IRoleRepository
  {  

    public async Task<IEnumerable<UserRoleModel>> GetRoleListByUserIdAsync(int userId)
    {
      var result = await _dataContext.UserRoles.Where(x => x.UserId==userId).ToListAsync();
      return result;

      //var newClacc = new WebApiContextDesignFactory();
      //using (WebApiDbContext newdb = newClacc.CreateDbContext(null))
      //{
      //  var result =await  newdb.UserRoles.Where(x => x.UserId==userId).ToListAsync();
      //  return result;
      //}

    }

    public async Task<IEnumerable<RoleModel>> GetRoleInfoByRoleIdAsync(IEnumerable<int> roleId)
    {

      var result = await _dataContext.Roles.Where(x => roleId.Any(y => y==x.Id)).ToListAsync();
      return result;


      //var newClacc = new WebApiContextDesignFactory();
      //using (WebApiDbContext newdb = newClacc.CreateDbContext(null))
      //{
      //  var result = await newdb.Roles.Where(x => roleId.Any(y => y==x.Id)).ToListAsync();
      //  return result;
      //}
    }
    public async Task<bool> CheckUserIsSuperAdminAsync(int userId)
    {
      var data = await _dataContext.UserRoles.Where(m => m.UserId == userId && m.RoleId == 1).CountAsync();
      if (data == 0) return false;
      return true;
    }

    public async Task<RoleModel> GetRoleInfoByRoleName(string roleName)
    {
      var data = await _dataContext.Roles.Where(m => m.Name==roleName).FirstOrDefaultAsync();      
      return data;
    }

    public async Task<RoleModel> GetRoleInfoByRoleIdAsync(int roleId)
    {
      var data = await _dataContext.Roles.Where(m => m.Id==roleId).FirstOrDefaultAsync();
      return data;
    }
  }

}
