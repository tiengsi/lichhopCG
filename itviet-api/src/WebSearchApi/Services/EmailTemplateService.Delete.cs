using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Services
{
  public partial interface IEmailTemplateService
  {

    Task<FunctionResult> DeleteEmailTemplateByIdAsync(int emailTemplateId);
    Task<int> DeleteTemplateByOrganizeIdAsync(int organizeId);
  }

  public partial class EmailTemplateService : IEmailTemplateService
  {
    public async Task<FunctionResult> DeleteEmailTemplateByIdAsync(int emailTemplateId)
    {
      try
      {
        var result = CheckEmailTemplateId(emailTemplateId);
        if (result.IsSuccess == false) return result;
        var emailTemplateInfo = await _emailTemplateRepository.GetEmailTemplateByIdAsync(emailTemplateId);
        if (emailTemplateInfo==null)
        {
          result.AddError("Mã EmailTemplateId không tồn tại");
          return result;
        }
        var resultDelete = await _emailTemplateRepository.DeleteEmailTemplateByIdAsync(emailTemplateId);
        if (resultDelete == false) {
            result.AddError("Xóa mẫu thư thất bại");
          return result;
        }
        result.SetData(emailTemplateId);

        var deletePath = $"{Directory.GetCurrentDirectory()}\\Upload\\{emailTemplateInfo.FilePath.Replace(_uploadSettings.EmailTemplate, nameof(_uploadSettings.EmailTemplate))}";
        var deleteHTMLPath = deletePath.Replace(".docx", ".html");
        if (File.Exists(deleteHTMLPath)) File.Delete(deleteHTMLPath);

        if (File.Exists(deletePath))
        {
          // If file found, delete it    
          File.Delete(deletePath);
          _logger.LogInformation($"Deleted Email Template: Deleted {deletePath}");       
        }
        var folderImage = deletePath.Substring(0, deletePath.Length - 5) + "_files";
        if (Directory.Exists(folderImage))
        {
          Directory.Delete(folderImage, true);
        }
        return result;
      }
      catch (Exception ex)
      {

        throw ex;
      }
    }
    public async Task<int> DeleteTemplateByOrganizeIdAsync(int organizeId)
    {
      try
      {
        var result = await _emailTemplateRepository.DeleteEmailTemplateByOrganizeIdAsync(organizeId);
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
