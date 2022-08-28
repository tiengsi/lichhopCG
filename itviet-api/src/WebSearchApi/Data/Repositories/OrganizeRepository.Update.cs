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
    Task<bool> UpdateOrganizeAsync(OrganizeModel model);

  }

  public partial class OrganizeRepository : RepositoryBase<OrganizeModel>, IOrganizeRepository
  {
    public async Task<bool> UpdateOrganizeAsync(OrganizeModel model)
    {
      try
      {
        var entityUpdate = await _dataContext.OrganizeModel.FirstOrDefaultAsync(x => x.OrganizeId == model.OrganizeId);
        if (entityUpdate != null)
        {
          entityUpdate.OrganizeParentId = model.OrganizeParentId;
          entityUpdate.Name = model.Name;
          entityUpdate.IsActive = model.IsActive;
          entityUpdate.Address = model.Address;
          entityUpdate.CodeName = model.CodeName;
          entityUpdate.Order = model.Order;
          entityUpdate.OtherName = model.OtherName;
          entityUpdate.Phone = model.Phone;         
          entityUpdate.UpdatedDate = DateTime.Now;
          await _dataContext.SaveChangesAsync();
          return true;
        }
        return false;
      }
      catch (System.Exception ex)
      {
        throw ex;
      }
    }
  }
}
