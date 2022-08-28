using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;
namespace WebApi.Services
{
  public partial interface IEmailTemplateService
  {
    Task<IEnumerable<EmailTemplateDto>> GetAllEmailTemplateAsync();
    Task<IEnumerable<EmailTemplateDto>> GetListEmailTemplateByOrganizeIdAsync(int organizeId);
    Task<EmailTemplateDto> GetEmailTemplateByIdAsync(int emailTemplateId);


  }

  public partial class EmailTemplateService : IEmailTemplateService
  {
    public async Task<IEnumerable<EmailTemplateDto>> GetAllEmailTemplateAsync()
    {
      var emailTemplateList = await _emailTemplateRepository.GetListEmailTemplateAsync();
      var result = new List<EmailTemplateDto>();
      var hostURL = _appSettings.HostURL;
      foreach (var item in emailTemplateList)
      {
        var newObj = new EmailTemplateDto()
        {
          OrganizeId = item.OrganizeId,
          Title = item.Title,
          FileName = item.FileName,
          FilePath = $"{hostURL}\\{item.FilePath}",
          CloudinaryPublicId = item.CloudinaryPublicId,
          EmailTemplateId = item.EmailTemplateId,
          TypeEmail = item.TypeEmail,
          IsActvie=item.IsActive
        };
        result.Add(newObj);
      }
      return result;
    }

    public async Task<IEnumerable<EmailTemplateDto>> GetListEmailTemplateByOrganizeIdAsync(int organizeId)
    {
      var emailTemplateListByOrganizeId = await _emailTemplateRepository.GetListEmailTemplateByOrganizeIdAsync(organizeId);
      if (emailTemplateListByOrganizeId.Count()==0)
      {
        return null;
      }
      var result = new List<EmailTemplateDto>();
      var hostURL = _appSettings.HostURL;
      foreach (var item in emailTemplateListByOrganizeId)
      {
        var newObj = new EmailTemplateDto()
        {
          OrganizeId = item.OrganizeId,
          Title = item.Title,
          FileName = item.FileName,
          FilePath = $"{hostURL}\\{item.FilePath}",
          CloudinaryPublicId = item.CloudinaryPublicId,
          TypeEmail = item.TypeEmail,
          EmailTemplateId = item.EmailTemplateId,
          IsActvie = item.IsActive
        };
        result.Add(newObj);
      }
      return result;
    }
    public async Task<EmailTemplateDto> GetEmailTemplateByIdAsync(int emailTemplateId)
    {
      var result = await _emailTemplateRepository.GetEmailTemplateByIdAsync(emailTemplateId);
      if (result == null)
      {
        return null;
      }
      var hostURL = _appSettings.HostURL;
      var newObj = new EmailTemplateDto()
      {
        OrganizeId = result.OrganizeId,
        Title = result.Title,
        FileName = result.FileName,
        FilePath = $"{hostURL}\\{result.FilePath}",
        CloudinaryPublicId = result.CloudinaryPublicId,
        EmailTemplateId = result.EmailTemplateId,
        TypeEmail = result.TypeEmail,
        IsActvie =result.IsActive
      };
      return newObj;
    }

 
  }

}

