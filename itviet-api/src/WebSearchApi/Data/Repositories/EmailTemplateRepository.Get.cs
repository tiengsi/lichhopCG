using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data.Infrastructure;
using WebApi.Models;

namespace WebApi.Data.Repositories
{
  public partial interface IEmailTemplateRepository
  {
    Task<IEnumerable<EmailTemplateModel>> GetListEmailTemplateAsync();
    Task<IEnumerable<EmailTemplateModel>> GetListEmailTemplateByOrganizeIdAsync(int organizeId);
    Task<EmailTemplateModel> GetEmailTemplateByIdAsync(int emailTemplateId);

  }

  public partial class EmailTemplateRepository : RepositoryBase<EmailTemplateModel>, IEmailTemplateRepository
  {
    public async Task<IEnumerable<EmailTemplateModel>> GetListEmailTemplateAsync()
    {
      var emailTemplateList = await _dataContext.EmailTemplateModel.ToListAsync();
      return emailTemplateList;
    }
    public async Task<IEnumerable<EmailTemplateModel>>GetListEmailTemplateByOrganizeIdAsync(int organizeId)
    {
      var emailTemplateList = await _dataContext.EmailTemplateModel.Where(m => m.OrganizeId == organizeId).ToListAsync();
      return emailTemplateList;
    }
    public async Task<EmailTemplateModel> GetEmailTemplateByIdAsync(int emailTemplateId)
    {
      var result = await _dataContext.EmailTemplateModel.Where(m => m.EmailTemplateId== emailTemplateId).FirstOrDefaultAsync();

      return result;

    }
  }

}
