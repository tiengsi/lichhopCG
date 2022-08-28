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
    Task<bool> DeleteVNPT_BrandNameByIdAsync(int brandNameId);
    Task<bool> DeleteViettel_BrandNameByIdAsync(int brandNameId);
    Task<bool> DeleteBrandNameByOrganizeIddAsync(int organizeId);

  }

  public partial class BrandNameRepository : RepositoryBase<BrandNameModel>, IBrandNameRepository
  {
    public async Task<bool> DeleteVNPT_BrandNameByIdAsync(int brandNamedId)
    {
      try
      {
        var result = await _dataContext.VNPT_BrandNameModel.Where(x => x.BrandNameId == brandNamedId).FirstOrDefaultAsync();
        if (result == null) return false;

        _dataContext.VNPT_BrandNameModel.Remove(result);
        await _dataContext.SaveChangesAsync();
        return true;
      }
      catch (System.Exception ex)
      {

        throw ex;
      }
    }

    public async Task<bool> DeleteViettel_BrandNameByIdAsync(int brandNamedId)
    {
      try
      {
        var result = await _dataContext.Viettel_BrandNameModel.Where(x => x.BrandNameId == brandNamedId).FirstOrDefaultAsync();
        if (result == null) return false;

        _dataContext.Viettel_BrandNameModel.Remove(result);
        await _dataContext.SaveChangesAsync();
        return true;
      }
      catch (System.Exception ex)
      {

        throw ex;
      }
    }

    public async Task<bool> DeleteBrandNameByOrganizeIddAsync(int organizeId)
    {
      try
      {
        var resultVNPT = await _dataContext.VNPT_BrandNameModel.Where(x => x.OrganizeId == organizeId).ToListAsync();
     
        _dataContext.VNPT_BrandNameModel.RemoveRange(resultVNPT);
       var  resultVieetel = await _dataContext.Viettel_BrandNameModel.Where(x => x.OrganizeId == organizeId).ToListAsync();
     
        _dataContext.Viettel_BrandNameModel.RemoveRange(resultVieetel);

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
