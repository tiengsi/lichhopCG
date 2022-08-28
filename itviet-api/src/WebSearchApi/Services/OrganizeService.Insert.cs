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
    Task<FunctionResult> CreateOrganizeAsync(OrganizeDto model);
    //List<string> ValidateFieldsOrganizeInsert(OrganizeDto model);
  }

  public partial class OrganizeService : IOrganizeService
  {
    public async Task<FunctionResult> CreateOrganizeAsync(OrganizeDto model)
    {
      //var result = new FunctionResult();
      var map = model.ToModel();
      map.CreatedDate = DateTime.Now;
      var result = ValidateRequiredFieldsOrganize(model);
      if (result.IsSuccess == false) return result;
        var isExist = await CheckContainsFieldsInsertOrganize(model);
        if(isExist)
        {
          result.AddError($"{model.Name} và {model.CodeName} đã tồn tại trong hệ thống!");
          return result;
        }
        var isSucess=await _organzieRepository.InsertOrganizeAsync(map);
        if(isSucess==false)
        {
          result.AddError("Insert không thành công");
          return result;
        }  
      result.SetData(map.OrganizeId);
      return result;
    }
   
    
  }
}
