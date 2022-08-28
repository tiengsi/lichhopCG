using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Services
{
  public partial interface IBrandNameService
  {
    Task<FunctionResult> CreateVNPT_BrandNameAsync(VNPT_BrandNameModel model);
    Task<FunctionResult> CreateViettel_BrandNameAsync(Viettel_BrandNameModel model);

  }

  public partial class BrandNameService : IBrandNameService
  {
    public async Task<FunctionResult> CreateVNPT_BrandNameAsync(VNPT_BrandNameModel vnpt_Model)
    {
      var result = new FunctionResult();
      vnpt_Model.CreatedDate = DateTime.Now;
      result = ValidateRequiredFieldsVNPTBrandName(vnpt_Model);
      if (result.IsSuccess == false) return result;
      //result=await ValidateBRVNPTBrandNameAsync(vnpt_Model);
      //if (result.IsSuccess == false) return result;
      //await _brandNameRepository.DeleteBrandNameByOrganizeIddAsync(vnpt_Model.OrganizeId);
      var actionInsert = 1; // actionInsert=1 để kiểm tra việc insert
       var brandNameExisted = await CheckVNPT_BrandNameExistedAsync(vnpt_Model,actionInsert);
      if (brandNameExisted == true)
      {
        result.AddError($"{vnpt_Model.BrandName} và {vnpt_Model.OrganizeId} đã tồn tại");
        return result;
      }
      //brandNameExisted = await CheckVNPT_BrandNameExistedInViettel_BrandName(vnpt_Model);
      //if (brandNameExisted == true)
      //{
      //  result.AddError($"{vnpt_Model.BrandName} và {vnpt_Model.OrganizeId} đã tồn tại ở BrandName Viettel");
      //  return result;
      //}
      var isSucess = await _brandNameRepository.InsertVNPT_BrandNameAsync(vnpt_Model);
      if (isSucess == false)
      {
        result.AddError("Thêm brandname không thành công");
        return result;
      }

      result.SetData(vnpt_Model.BrandNameId);

      return result;
    }

    public async Task<FunctionResult> CreateViettel_BrandNameAsync(Viettel_BrandNameModel viettel_Model)
    {
      var result = new FunctionResult();
      viettel_Model.CreatedDate = DateTime.Now;
      result = ValidateRequiredFieldsViettelBrandName(viettel_Model);
      if (result.IsSuccess == false) return result;
      //result=await ValidateBRViettelBrandNameAsync(viettel_Model);
      //if (result.IsSuccess == false) return result;

      //await _brandNameRepository.DeleteBrandNameByOrganizeIddAsync(viettel_Model.OrganizeId);
      int actionInsert = 1;
      var brandNameExisted = await CheckVT_BrandNameExistedAsync(viettel_Model,actionInsert);
      if (brandNameExisted == true)
      {
        result.AddError($"{viettel_Model.BrandName} và {viettel_Model.OrganizeId} đã tồn tại");
        return result;
      }
      //brandNameExisted = await CheckVT_BrandNameExistedInVNPT_BrandName(viettel_Model);
      //if (brandNameExisted == true)
      //{
      //  result.AddError($"{viettel_Model.BrandName} và {viettel_Model.OrganizeId} đã tồn tại ở BrandName VNPT");
      //  return result;
      //}
      var isSucess = await _brandNameRepository.InsertViettel_BrandNameAsync(viettel_Model);
      if (isSucess == false)
      {
        result.AddError("Thêm brandname không thành công");
        return result;
      }
      result.SetData(viettel_Model.BrandNameId);

      return result;
    }
  }
}
