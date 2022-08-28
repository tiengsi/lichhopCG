using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Services
{
  public partial interface IBrandNameService
  {

    Task<FunctionResult> DeleteVNPT_BrandNameByIdAsync(int brandNameId);
    Task<FunctionResult> DeleteViettel_BrandNameByIdAsync(int brandNameId);
    Task<int> DeleteBrandNameByOrganizeIddAsync(int organizeId);
  }

  public partial class BrandNameService : IBrandNameService
  {
    public async Task<FunctionResult> DeleteVNPT_BrandNameByIdAsync(int brandNameId)
    {
      try
      {
        var isCheck = CheckBrandNameId(brandNameId);
        if (isCheck.IsSuccess == false) return isCheck;
        var isExist = await _brandNameRepository.GetVNPT_BrandNameById(brandNameId);
        if (isExist!=null)
        {
          isCheck.AddError("Mã brandname không tồn tại");
          return isCheck;
        }
        var result = await _brandNameRepository.DeleteVNPT_BrandNameByIdAsync(brandNameId);
        if (result == false) return isCheck;
        isCheck.SetData(brandNameId);
        return isCheck;
      }
      catch (Exception ex)
      {

        throw ex;
      }
    }

    public async Task<FunctionResult> DeleteViettel_BrandNameByIdAsync(int brandNameId)
    {
      try
      {
        var isCheck = CheckBrandNameId(brandNameId);
        if (isCheck.IsSuccess == false) return isCheck;
        var isExist = await _brandNameRepository.GetViettel_BrandNameById(brandNameId);
        if (isExist!=null)
        {
          isCheck.AddError("Mã brandname không tồn tại");
          return isCheck;
        }
        var result = await _brandNameRepository.DeleteViettel_BrandNameByIdAsync(brandNameId);
        if (result == false) return isCheck;
        isCheck.SetData(brandNameId);
        return isCheck;
      }
      catch (Exception ex)
      {

        throw ex;
      }
    }
    public async Task<int> DeleteBrandNameByOrganizeIddAsync(int organizeId)
    {
      try
      {
        var result = await _brandNameRepository.DeleteBrandNameByOrganizeIddAsync(organizeId);
        if (result == true)
          return 1;
        else
          return -1;
      }
      catch (Exception ex)
      {

        throw ex;
      }
    }

  }
}
