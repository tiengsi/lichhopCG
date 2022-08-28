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
  public partial interface IOrganizeService
  {
    Task<IEnumerable<OrganizesTreeDto>> GetOrganizesTreeAsync();
    Task<OrganizeDto> GetOrganizeByIdAsync(int organizeId);

  }

  public partial class OrganizeService : IOrganizeService
  {
    public async Task<IEnumerable<OrganizesTreeDto>> GetOrganizesTreeAsync()
    {
      var allOrganizeList = await _organzieRepository.GetOrganizeListAsync();
      var result = new List<OrganizesTreeDto>();
      var organizeParentList = allOrganizeList.Where(x => x.OrganizeParentId == null);    

      foreach (var parentOrgInfo in organizeParentList)
      {
        var newObj = new OrganizesTreeDto()
        {
          OrganizeId = parentOrgInfo.OrganizeId,
          Name = parentOrgInfo.Name,
          Address = parentOrgInfo.Address,
          CodeName = parentOrgInfo.CodeName,
          IsActive = parentOrgInfo.IsActive,
          OtherName = parentOrgInfo.OtherName,
          Order = parentOrgInfo.Order,
          Phone = parentOrgInfo.Phone,          
        };
        newObj.SubOrganizeList = GetSubOrganizeFunc(allOrganizeList, parentOrgInfo.OrganizeId);      
        result.Add(newObj);
      }   

      return result;
    }

    public async Task<OrganizeDto> GetOrganizeByIdAsync(int organizeId)
    {
      var getItem = await _organzieRepository.GetSingleById(organizeId);
      if (getItem == null) return new OrganizeDto();
      var dtoObj = new OrganizeDto()
      {
        OrganizeId = getItem.OrganizeId,
        Name = getItem.Name,
        Address = getItem.Address,
        CodeName = getItem.CodeName,
        IsActive = getItem.IsActive,
        OtherName = getItem.OtherName,
        Order = getItem.Order,
        Phone = getItem.Phone,
        OrganizeParentId = getItem.OrganizeParentId
      };
      return dtoObj;
    }

  }
}
