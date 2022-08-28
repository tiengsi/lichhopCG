using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Services
{
  public partial interface IEmailTemplateService
  {
    Task<FunctionResult> CreateEmailTemplateAsync(EmailTemplateDto model);

  }

  public partial class EmailTemplateService:IEmailTemplateService
  {
    public async Task<FunctionResult> CreateEmailTemplateAsync(EmailTemplateDto model)
    {
      var map = model.ToModel();
      map.CreatedDate = DateTime.Now;
      var result = ValidateRequiredFieldsEmailTemplate(model);
      if (result.IsSuccess == false) return result;
      var isExist = await CheckContainsFieldsInsertEmailTemplateAsync(model);
      if (isExist)
      {
        result.AddError($"{model.Title} và {model.OrganizeId} đã tồn tại trong hệ thống!");
        return result;
      }
      var checkCountOrganizeId = await CountOrganizeIdAddEmailTemplateAsync(model);
      if (checkCountOrganizeId.IsSuccess == false) return checkCountOrganizeId;
      var isSucess = await _emailTemplateRepository.InsertEmailTemplateAsync(map);
      if (isSucess == false)
      {
        result.AddError("Insert không thành công");
        return result;
      }

      result=ConvertDocumentToHtmlAsync(model.FilePath);
      result.SetData(map.EmailTemplateId);
      return result;
    }
  }
}
