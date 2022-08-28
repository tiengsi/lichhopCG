using AutoMapper;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
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
  public partial interface IEmailTemplateService
  {
    Task<FunctionResult> UpdateEmailTemplateAsync(EmailTemplateDto model);

  }

  public partial class EmailTemplateService : IEmailTemplateService
  {
    public async Task<FunctionResult> UpdateEmailTemplateAsync(EmailTemplateDto model)
    {
      var map = model.ToModel();
      map.EmailTemplateId = model.EmailTemplateId;
      var result = ValidateRequiredFieldsEmailTemplate(model);
      if (result.IsSuccess == false) return result;

      var isCheck = CheckEmailTemplateId(model.EmailTemplateId);
      if (isCheck.IsSuccess == false) return isCheck;

      var isExist = await CheckContaintsFieldsUpdateEmailTemplateAsync(model);
      if (isExist)
      {
        result.AddError($"{model.Title} và {model.OrganizeId} đã tồn tại trong hệ thống!");
        return result;
      }
      var checkCountOrganizeId = await CountOrganizeIdUpdateEmailTemplateAsync(model);
      if (checkCountOrganizeId.IsSuccess == false) return checkCountOrganizeId;
      var oldEmailTemplateInfo=await _emailTemplateRepository.GetEmailTemplateByIdAsync(model.EmailTemplateId);

      //Check Having new file temple be uploaded, we must clean old files on server
      if (oldEmailTemplateInfo.FilePath.Equals(map.FilePath)==false)
      {
        var deletePath = $"{Directory.GetCurrentDirectory()}\\Upload\\{oldEmailTemplateInfo.FilePath.Replace(_uploadSettings.EmailTemplate, nameof(_uploadSettings.EmailTemplate))}";
        var deleteHTMLPath = deletePath.Replace(".docx", ".html");

        if (File.Exists(deletePath)) File.Delete(deletePath);      
        if (File.Exists(deleteHTMLPath))
        {
          File.Delete(deleteHTMLPath);
          _logger.LogInformation($"Deleted EmailTemplate Old before Update new File: Deleted {deletePath}");
        }
        var folderImage = deletePath.Substring(0, deletePath.Length - 5) + "_files";
        if (Directory.Exists(folderImage))
        {
          Directory.Delete(folderImage, true);
        }
        //generate new html file
        result=ConvertDocumentToHtmlAsync(model.FilePath);
      }

      var isSucess = await _emailTemplateRepository.UpdateEmailTemplateAsync(map);
      if (isSucess == false)
      {
        result.AddError("Không tìm thấy mã emailtemplate này!");
        return result;
      }
      result.SetData(map.EmailTemplateId);
      return result;
    }




  }
}
