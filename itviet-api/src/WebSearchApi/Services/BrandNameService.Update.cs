using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Services
{
  public partial interface IBrandNameService
  {
    Task<FunctionResult> UpdateVNPT_BrandNameAsync(VNPT_BrandNameModel model);
    Task<FunctionResult> UpdateViettel_BrandNameAsync(Viettel_BrandNameModel model);

  }

  public partial class BrandNameService : IBrandNameService
  {
    public async Task<FunctionResult> UpdateVNPT_BrandNameAsync(VNPT_BrandNameModel vnpt_Model)
    {
      var result = new FunctionResult();
      int actionInsert = 0;
      result = ValidateRequiredFieldsVNPTBrandName(vnpt_Model);
      if (result.IsSuccess == false) return result;
      var isCheck = CheckBrandNameId(vnpt_Model.BrandNameId);
      if (isCheck.IsSuccess == false) return isCheck;
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
      var isSucess = await _brandNameRepository.UpdateVNPT_BrandNameAsync(vnpt_Model);
      if (isSucess == false)
      {
        result.AddError("Không tìm thấy mã brandname này!");
        return result;
      }
      result.SetData(vnpt_Model.BrandNameId);
      return result;
    }

    public async Task<FunctionResult> UpdateViettel_BrandNameAsync(Viettel_BrandNameModel viettel_Model)
    {
      var result = new FunctionResult();
      int actionInsert = 0; // actionInsert=0 để kiểm tra việc update
      result = ValidateRequiredFieldsViettelBrandName(viettel_Model);
      if (result.IsSuccess == false) return result;
      var isCheck = CheckBrandNameId(viettel_Model.BrandNameId);
      if (isCheck.IsSuccess == false) return isCheck;
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
      var isSucess = await _brandNameRepository.UpdateViettel_BrandNameAsync(viettel_Model);
      if (isSucess == false)
      {
        result.AddError("Không tìm thấy mã brandname này!");
        return result;
      }
      result.SetData(viettel_Model.BrandNameId);


      return result;
    }

  }
}
