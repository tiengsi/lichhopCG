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

    Task<FunctionResult> DeleteOrganizeByIdAsync(int organizeId);
  }

  public partial class OrganizeService : IOrganizeService
  {
    public async Task<FunctionResult> DeleteOrganizeByIdAsync(int organizeId)
    {
      try
      {
        var isCheck = CheckOrganizeId(organizeId);
        //if (isCheck.IsSuccess == false) return isCheck;
        var isExist = await _organzieRepository.CheckContains(m => m.OrganizeId == organizeId);
        if (!isExist)
        {
          isCheck.AddError("Mã đơn vị không tồn tại");
          return isCheck;
        }
        bool isPass;
        try
        {
          isPass = await _brandRepository.DeleteBrandNameByOrganizeIddAsync(organizeId);
        } catch(Exception e)
        {
          throw e;
        }
        try
        {          
          isPass = await _emailTemplateRepository.DeleteEmailTemplateByOrganizeIdAsync(organizeId);
        }
        catch (Exception e)
        {
          throw e;
        }
        try
        {         
          isPass = await _scheduleRepository.DeleteScheduleTypeByOrganizeIdAsync(organizeId);        
        }
        catch (Exception e)
        {
          throw e;
        }
        try
        {
          isPass = await _locationService.DeleteListLoacationByOrganizeIdAsync(organizeId);         
        }
        catch (Exception e)
        {
          throw e;
        }
        try
        {
          isPass = await _groupParticipantService.DeleteListGroupParticipantByOrganizeIdAsync(organizeId);          
        }        
        catch (Exception e)
        {
          throw e;
        }
        try
        {          
          isPass = await _scheduleTitleTemplateService.DeleteListScheduleTitleTemplateByOrganizeIdsAsync(organizeId);
        }
        catch (Exception e)
        {
          throw e;
        }
        var result = await _organzieRepository.DeleteOrganizeByIdAsync(organizeId);
        if (result == false) return isCheck;
        isCheck.SetData(organizeId);
        return isCheck;
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    
  }
}
