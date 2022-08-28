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
    Task<bool> InsertVNPT_BrandNameAsync(VNPT_BrandNameModel model);
    Task<bool> InsertViettel_BrandNameAsync(Viettel_BrandNameModel model);
  }


  public partial class BrandNameRepository : RepositoryBase<BrandNameModel>, IBrandNameRepository
  {
    public async Task<bool> InsertVNPT_BrandNameAsync(VNPT_BrandNameModel model)
    {
      try
      {
        var result = await _dataContext.VNPT_BrandNameModel.AddAsync(model);
        await _dataContext.SaveChangesAsync();
        return true;
      }
      catch (System.Exception ex)
      {
        throw ex;
      }
    }

    public async Task<bool> InsertViettel_BrandNameAsync(Viettel_BrandNameModel model)
    {
      try
      {
        var result = await _dataContext.Viettel_BrandNameModel.AddAsync(model);
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
