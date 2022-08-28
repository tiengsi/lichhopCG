using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data.Infrastructure;
using WebApi.Helpers;
using WebApi.Models;

namespace WebApi.Data.Repositories
{
  public partial interface IEmailTemplateRepository
  {
    Task<bool> UpdateEmailTemplateAsync(EmailTemplateModel model);

  }

  public partial class EmailTemplateRepository : RepositoryBase<EmailTemplateModel>, IEmailTemplateRepository
  {
    public async Task<bool> UpdateEmailTemplateAsync(EmailTemplateModel model)
    {
      try
      {
        var entityUpdate = await _dataContext.EmailTemplateModel.FirstOrDefaultAsync(x => x.EmailTemplateId == model.EmailTemplateId);
        if (entityUpdate != null)
        {
          entityUpdate.OrganizeId = model.OrganizeId;
          entityUpdate.FileName = model.FileName;
          entityUpdate.IsActive = model.IsActive;
          entityUpdate.FilePath = model.FilePath;
          entityUpdate.Title = model.Title;
          entityUpdate.IsActive = model.IsActive;
          entityUpdate.UpdatedDate = DateTime.Now;
          await _dataContext.SaveChangesAsync();
          return true;
        }
        return false;
      }
      catch (System.Exception ex)
      {
        throw ex;
      }
    }
  }
}
