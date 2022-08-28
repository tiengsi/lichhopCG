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
    Task<bool> DeleteEmailTemplateByIdAsync(int emailTemplateId);
    Task<bool> DeleteEmailTemplateByOrganizeIdAsync(int organizeId);

  }

  public partial class EmailTemplateRepository : RepositoryBase<EmailTemplateModel>, IEmailTemplateRepository
  {
    public async Task<bool> DeleteEmailTemplateByIdAsync(int emailTemplateId)
    {
      try
      {
        var result = await _dataContext.EmailTemplateModel.Where(x => x.EmailTemplateId == emailTemplateId).FirstOrDefaultAsync();
        if (result == null) return false;

        _dataContext.EmailTemplateModel.Remove(result);
        await _dataContext.SaveChangesAsync();
        return true;
      }
      catch (System.Exception ex)
      {

        throw ex;
      }
    }

    public async Task<bool> DeleteEmailTemplateByOrganizeIdAsync(int organizeId)
    {
      try
      {
        var result = await _dataContext.EmailTemplateModel.Where(x => x.OrganizeId == organizeId).ToListAsync();
        if (result == null) return false;
        _dataContext.EmailTemplateModel.RemoveRange(result);
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
