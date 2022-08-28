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
    Task<bool> DeleteRoleByIdAsync(RoleModel model);
  }

  public partial class RoleRepository : RepositoryBase<RoleModel>, IRoleRepository
  {

    public async Task<bool> DeleteRoleByIdAsync(RoleModel model)
    {
      try
      {
        var result = await _dataContext.Roles.Where(x => x.Id == model.Id).ToListAsync();      
        _dataContext.Roles.RemoveRange(result);
        await _dataContext.SaveChangesAsync();
        return true;
      }
      catch (System.Exception ex)
      {
        return false;
        throw ex;
      }
    }


  }

}
