using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data.Infrastructure;
using WebApi.Models;

namespace WebApi.Data.Repositories
{
  public partial interface IBrandNameRepository
  {
    Task<bool> UpdateVNPT_BrandNameAsync(VNPT_BrandNameModel model);
    Task<bool> UpdateViettel_BrandNameAsync(Viettel_BrandNameModel model);

  }

  public partial class BrandNameRepository : RepositoryBase<BrandNameModel>, IBrandNameRepository
  {
    public async Task<bool> UpdateVNPT_BrandNameAsync(VNPT_BrandNameModel model)
    {
      try
      {
        var entityUpdate = await _dataContext.VNPT_BrandNameModel.FirstOrDefaultAsync(x => x.BrandNameId == model.BrandNameId);
        if (entityUpdate != null)
        {
         
          entityUpdate.BrandName = model.BrandName;
          entityUpdate.PhoneNumber = model.PhoneNumber;
          entityUpdate.ApiLink = model.ApiLink;
          entityUpdate.ApiPassword = model.ApiPassword;
          entityUpdate.ApiUserName = model.ApiUserName;       
          entityUpdate.UpdatedDate = DateTime.Now;
          entityUpdate.ContractType = model.ContractType;
          entityUpdate.IsActive = model.IsActive;
       
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

    public async Task<bool> UpdateViettel_BrandNameAsync(Viettel_BrandNameModel model)
    {
      try
      {
        var entityUpdate = await _dataContext.Viettel_BrandNameModel.FirstOrDefaultAsync(x => x.BrandNameId == model.BrandNameId);
        if (entityUpdate != null)
        {

          entityUpdate.BrandName = model.BrandName;
          entityUpdate.CPCode = model.CPCode;
          entityUpdate.ApiLink = model.ApiLink;
          entityUpdate.ApiPassword = model.ApiPassword;
          entityUpdate.ApiUserName = model.ApiUserName;
          entityUpdate.UpdatedDate = DateTime.Now;
          entityUpdate.ContractType = model.ContractType;
          entityUpdate.IsActive = model.IsActive;

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
