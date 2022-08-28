using AutoMapper;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using WebApi.Data.Infrastructure;
using WebApi.Data.Repositories;
using WebApi.Helpers;
using WebApi.Helpers.Domains;
using WebApi.Helpers.Exceptions;
using WebApi.Helpers.Extensions;
using WebApi.Models;
using WebApi.Models.Dtos;
using WebApi.Models.Enums;
namespace WebApi.Services
{
  public partial interface IBrandNameService
  {
    Task<IEnumerable<VNPT_BrandNameDto>> GetAllVNPT_BrandNameAsync();
    Task<IEnumerable<VNPT_BrandNameDto>> GetVNPT_BrandNameListByOrganizeIdAsync(int organizeId);
    Task<VNPT_BrandNameDto> GetVNPT_BrandNameById(int brandNameId);

    Task<IEnumerable<Viettel_BrandNameDto>> GetAllViettel_BrandNameAsync();
    Task<IEnumerable<Viettel_BrandNameDto>> GetViettel_BrandNameListByOrganizeIdAsync(int organizeId);
    Task<Viettel_BrandNameDto> GetViettel_BrandNameById(int brandNameId);
    Task<IEnumerable<BrandNameResultDto>> GetAllBrandNameByOrganizeIdAsync(int organizeId);
  }

  public partial class BrandNameService : IBrandNameService
  {
    public async Task<IEnumerable<BrandNameResultDto>> GetAllBrandNameByOrganizeIdAsync(int organizeId)
    {
      var VNPT_BrandNameList = await _brandNameRepository.GetVNPT_BrandNameListByOrganizeIdAsync(organizeId);
      var VT_BrandNameList = await _brandNameRepository.GetViettel_BrandNameListByOrganizeIdAsync(organizeId);
      var result = new List<BrandNameResultDto>();
      foreach (var item in VT_BrandNameList)
      {
        var response = new BrandNameResultDto()
        {
          BrandNameId = item.BrandNameId,
          Name = "Viettel" + "-" + item.BrandName
        };
        result.Add(response);
      }
      foreach (var item in VNPT_BrandNameList)
      {
        var response = new BrandNameResultDto()
        {
          BrandNameId = item.BrandNameId,
          Name ="VNPT"+ "-" + item.BrandName
        };
        result.Add(response);
      }
      return result;
    }
    public async Task<IEnumerable<VNPT_BrandNameDto>> GetAllVNPT_BrandNameAsync()
    {
      var brandNameList = await _brandNameRepository.GetVNPT_BrandNameAsync();
      var result = new List<VNPT_BrandNameDto>();
      foreach (var item in brandNameList)
      {
        var newObj = new VNPT_BrandNameDto()
        {
          OrganizeId = item.OrganizeId,
          BrandNameId = item.BrandNameId,
          ContractType = item.ContractType,
          IsActive = item.IsActive,
          ApiLink = item.ApiLink,
          ApiPass = item.ApiPassword,
          ApiUser = item.ApiUserName,
          PhoneNumber = item.PhoneNumber,
          BranchName = item.BrandName

        };
        result.Add(newObj);
      }
      return result;
    }

    public async Task<IEnumerable<VNPT_BrandNameDto>> GetVNPT_BrandNameListByOrganizeIdAsync(int organizeId)
    {
      var brandNameList = await _brandNameRepository.GetVNPT_BrandNameListByOrganizeIdAsync(organizeId);
      if (brandNameList.Count()==0)
      {
        return new List<VNPT_BrandNameDto>(); 
      }
      var result = new List<VNPT_BrandNameDto>();
      foreach (var item in brandNameList)
      {
        var newObj = new VNPT_BrandNameDto()
        {
          OrganizeId = item.OrganizeId,
          BrandNameId = item.BrandNameId,

          ContractType = item.ContractType,
          PhoneNumber = item.PhoneNumber,
          IsActive = item.IsActive,
          ApiLink = item.ApiLink,

          ApiPass = item.ApiPassword,
          ApiUser = item.ApiUserName,

          BranchName = item.BrandName

        };
        result.Add(newObj);
      }
      return result;
    }

    public async Task<VNPT_BrandNameDto> GetVNPT_BrandNameById(int brandNameId)
    {

      var result = await _brandNameRepository.GetVNPT_BrandNameById(brandNameId);
      if (result==null)
      {
        return null;
      }
      var newObj = new VNPT_BrandNameDto()
      {
        OrganizeId = result.OrganizeId,
        BrandNameId = result.BrandNameId,
        PhoneNumber = result.PhoneNumber,
        ContractType = result.ContractType,

        IsActive = result.IsActive,
        ApiLink = result.ApiLink,

        ApiPass = result.ApiPassword,
        ApiUser = result.ApiUserName,

        BranchName = result.BrandName

      };
      return newObj;
    }



    public async Task<IEnumerable<Viettel_BrandNameDto>> GetAllViettel_BrandNameAsync()
    {
      var brandNameList = await _brandNameRepository.GetViettel_BrandNameAsync();
      var result = new List<Viettel_BrandNameDto>();
      foreach (var item in brandNameList)
      {
        var newObj = new Viettel_BrandNameDto()
        {
          OrganizeId = item.OrganizeId,
          BrandNameId = item.BrandNameId,
          ContractType = item.ContractType,
          IsActive = item.IsActive,
          ApiLink = item.ApiLink,
          ApiPass = item.ApiPassword,
          ApiUser = item.ApiUserName,
          BranchName = item.BrandName,
          CPCode = item.CPCode

        };
        result.Add(newObj);
      }
      return result;
    }

    public async Task<IEnumerable<Viettel_BrandNameDto>> GetViettel_BrandNameListByOrganizeIdAsync(int organizeId)
    {
      var brandNameList = await _brandNameRepository.GetViettel_BrandNameListByOrganizeIdAsync(organizeId);
      if (brandNameList.Count()==0)
      {
        return new List<Viettel_BrandNameDto>();
      }
      var result = new List<Viettel_BrandNameDto>();
      foreach (var item in brandNameList)
      {
        var newObj = new Viettel_BrandNameDto()
        {
          OrganizeId = item.OrganizeId,
          BrandNameId = item.BrandNameId,
          CPCode=item.CPCode,
          ContractType = item.ContractType,

          IsActive = item.IsActive,
          ApiLink = item.ApiLink,

          ApiPass = item.ApiPassword,
          ApiUser = item.ApiUserName,

          BranchName = item.BrandName

        };
        result.Add(newObj);
      }
      return result;
    }

    public async Task<Viettel_BrandNameDto> GetViettel_BrandNameById(int brandNameId)
    {

      var result = await _brandNameRepository.GetViettel_BrandNameById(brandNameId);
      if (result==null)
      {
        return null;
      }
      var newObj = new Viettel_BrandNameDto()
      {
        OrganizeId = result.OrganizeId,
        BrandNameId = result.BrandNameId,
        CPCode = result.CPCode,
        ContractType = result.ContractType,

        IsActive = result.IsActive,
        ApiLink = result.ApiLink,

        ApiPass = result.ApiPassword,
        ApiUser = result.ApiUserName,

        BranchName = result.BrandName

      };
      return newObj;
    }
  }

}

