using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
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
    Task<bool> UpdateRoleByIdAsync(RoleModel model);
  }

  public partial class RoleRepository : RepositoryBase<RoleModel>, IRoleRepository
  {

    public async Task<bool> UpdateRoleByIdAsync(RoleModel model)
    {
      try
      {
        var entityUpdate = await _dataContext.Roles.FirstOrDefaultAsync(x => x.Id == model.Id);

        entityUpdate.Description = model.Description;
        entityUpdate.NormalizedName = model.NormalizedName;
        entityUpdate.UpdatedDate = DateTime.Now;
        await _dataContext.SaveChangesAsync();
        return true;
      }
      catch (System.Exception ex)
      {
        return false;
      }
    }


  }

}
