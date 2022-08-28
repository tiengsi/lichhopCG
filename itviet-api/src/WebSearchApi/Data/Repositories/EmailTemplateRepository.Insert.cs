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
    Task<bool> InsertEmailTemplateAsync(EmailTemplateModel model);
  }


  public partial class EmailTemplateRepository : RepositoryBase<EmailTemplateModel>, IEmailTemplateRepository
  {
    public async Task<bool> InsertEmailTemplateAsync(EmailTemplateModel model)
    {
      try
      {
        var result = await _dataContext.EmailTemplateModel.AddAsync(model);
        await _dataContext.SaveChangesAsync();
        return true;
      }
      catch (System.Exception ex)
      {

        throw ex;
      }

    }
  }
}
