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
    Task<IEnumerable<VNPT_BrandNameModel>> GetVNPT_BrandNameAsync();
    Task<IEnumerable<VNPT_BrandNameModel>> GetVNPT_BrandNameListByOrganizeIdAsync(int organizeId);
    Task<VNPT_BrandNameModel> GetVNPT_BrandNameById(int brandNameId);


    Task<IEnumerable<Viettel_BrandNameModel>> GetViettel_BrandNameAsync();
    Task<IEnumerable<Viettel_BrandNameModel>> GetViettel_BrandNameListByOrganizeIdAsync(int organizeId);
    Task<Viettel_BrandNameModel> GetViettel_BrandNameById(int brandNameId);
    Task<IEnumerable<VNPT_BrandNameModel>> CheckVNPTBrandNameExistedAsync(VNPT_BrandNameModel vnpt_Model,int actionInsert);
    Task<IEnumerable<Viettel_BrandNameModel>> CheckVTBrandNameExistedAsync(Viettel_BrandNameModel viettel_Model,int actionInsert);
    //Task<IEnumerable<Viettel_BrandNameModel>> CheckVNPT_BrandNameExistedInViettel_BrandName(VNPT_BrandNameModel model);
    //Task<IEnumerable<VNPT_BrandNameModel>> CheckVT_BrandNameExistedInVNPT_BrandName(Viettel_BrandNameModel model);
  }

  public partial class BrandNameRepository : RepositoryBase<BrandNameModel>, IBrandNameRepository
  {
    public async Task<IEnumerable<VNPT_BrandNameModel>> GetVNPT_BrandNameAsync()
    {
      var brandList = await _dataContext.VNPT_BrandNameModel.ToListAsync();
      return brandList;
    }
    public async Task<IEnumerable<VNPT_BrandNameModel>> CheckVNPTBrandNameExistedAsync(VNPT_BrandNameModel vnpt_Model,int actionInsert)
    {
      if(actionInsert==1)
      {
        var VNPT_brandNameExistedInsert = await _dataContext.VNPT_BrandNameModel.Where(m => m.BrandName == vnpt_Model.BrandName && m.OrganizeId == vnpt_Model.OrganizeId).ToListAsync();
        return VNPT_brandNameExistedInsert;
      }
      var VNPT_brandNameExistedUpdated = await _dataContext.VNPT_BrandNameModel.Where(m => m.BrandName == vnpt_Model.BrandName && m.OrganizeId == vnpt_Model.OrganizeId && m.BrandNameId != vnpt_Model.BrandNameId).ToListAsync();
      return VNPT_brandNameExistedUpdated;
    }
    public async Task<IEnumerable<Viettel_BrandNameModel>> CheckVTBrandNameExistedAsync(Viettel_BrandNameModel viettel_Model,int actionInsert)
    {
      if(actionInsert==1)
      {
        var VT_brandNameExistedInsert = await _dataContext.Viettel_BrandNameModel.Where(m => m.BrandName == viettel_Model.BrandName && m.OrganizeId == viettel_Model.OrganizeId).ToListAsync();
        return VT_brandNameExistedInsert;
      }
      var VT_brandNameExistedUpdated = await _dataContext.Viettel_BrandNameModel.Where(m => m.BrandName == viettel_Model.BrandName && m.OrganizeId == viettel_Model.OrganizeId && m.BrandNameId != viettel_Model.BrandNameId).ToListAsync();
      return VT_brandNameExistedUpdated;
    }
    //public async Task<IEnumerable<Viettel_BrandNameModel>> CheckVNPT_BrandNameExistedInViettel_BrandName(VNPT_BrandNameModel model)
    //{
    //  var VT_brandNameExisted = await _dataContext.Viettel_BrandNameModel.Where(m => m.BrandName == model.BrandName && m.OrganizeId == model.OrganizeId).ToListAsync();
    //  return VT_brandNameExisted;
    //}
    //public async Task<IEnumerable<VNPT_BrandNameModel>> CheckVT_BrandNameExistedInVNPT_BrandName(Viettel_BrandNameModel model)
    //{
    //  var VNPT_brandNameExisted = await _dataContext.VNPT_BrandNameModel.Where(m => m.BrandName == model.BrandName && m.OrganizeId == model.OrganizeId).ToListAsync();
    //  return VNPT_brandNameExisted;
    //}

    public async Task<IEnumerable<VNPT_BrandNameModel>> GetVNPT_BrandNameListByOrganizeIdAsync(int organizeId)
    {
      var brandList = await _dataContext.VNPT_BrandNameModel.Where(m=>m.OrganizeId==organizeId).ToListAsync();
      return brandList;
    }
    public async Task<VNPT_BrandNameModel> GetVNPT_BrandNameById(int brandNameId)
    {
      var result = await _dataContext.VNPT_BrandNameModel.Where(m => m.BrandNameId == brandNameId).FirstOrDefaultAsync();
     
      return result;
     
    }

    public async Task<IEnumerable<Viettel_BrandNameModel>> GetViettel_BrandNameAsync()
    {
      var brandList = await _dataContext.Viettel_BrandNameModel.ToListAsync();
      return brandList;
    }
    public async Task<IEnumerable<Viettel_BrandNameModel>> GetViettel_BrandNameListByOrganizeIdAsync(int organizeId)
    {
      var brandList = await _dataContext.Viettel_BrandNameModel.Where(m => m.OrganizeId==organizeId).ToListAsync();
      return brandList;
    }
    public async Task<Viettel_BrandNameModel> GetViettel_BrandNameById(int brandNameId)
    {
      var result = await _dataContext.Viettel_BrandNameModel.Where(m => m.BrandNameId == brandNameId).FirstOrDefaultAsync();

      return result;

    }
  }
  
}
