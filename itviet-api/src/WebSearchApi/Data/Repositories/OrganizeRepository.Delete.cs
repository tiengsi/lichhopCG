using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data.Infrastructure;
using WebApi.Helpers;
using WebApi.Models;

namespace WebApi.Data.Repositories
{
  public partial interface IOrganizeRepository 
  {
    Task<bool> DeleteOrganizeByIdAsync(int organizeId);

  }

  public partial class OrganizeRepository : RepositoryBase<OrganizeModel>, IOrganizeRepository
  {
    public async Task<bool> DeleteOrganizeByIdAsync(int organizeId)
    {
      try
      {
        var result = await _dataContext.OrganizeModel.Where(x => x.OrganizeId == organizeId).FirstOrDefaultAsync();
        if (result == null) return false;

        _dataContext.OrganizeModel.Remove(result);
        await _dataContext.SaveChangesAsync();
        return true;
      }
      catch (System.Exception ex)
      {

        throw ex;
      }
    }
  }
}
