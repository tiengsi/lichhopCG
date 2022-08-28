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
  public partial interface IScheduleRepository 
  {
    Task<bool> UpdatePersonalNotesAsync(PersonalNotesModel model);
    Task<bool> UpdateScheduledResultDocumentAsync(ScheduledResultDocumentModel model);
    Task<bool> UpdateScheduledResultReportAsync(ScheduledResultReportModel model);
    Task<bool> UpdatePersonalScheduleAsync(PersonalScheduleModel model);

    Task<bool> UpdateIsShareFieldOfScheduleFileAttachmentByIdAsync(int id,bool isShare);
  }

  public partial class ScheduleRepository : RepositoryBase<ScheduleModel>, IScheduleRepository
  {
    public async Task<bool> UpdatePersonalNotesAsync(PersonalNotesModel model)
    {
      try
      {
        var entityUpdate = await _dataContext.PersonalNotesModel.FirstOrDefaultAsync(x => x.PersonalNotesId == model.PersonalNotesId);
        if (entityUpdate != null)
        {
          entityUpdate.ScheduleId = model.ScheduleId;
          entityUpdate.UserId = model.UserId;
          entityUpdate.ContentNote = model.ContentNote;
          entityUpdate.Title = model.Title;
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

    public async Task<bool> UpdatePersonalScheduleAsync(PersonalScheduleModel model)
    {
      try
      {
        var entityUpdate = await _dataContext.PersonalScheduleModel.FirstOrDefaultAsync(x => x.PersonalScheduleId == model.PersonalScheduleId);
        if (entityUpdate != null)
        {
          entityUpdate.IsActive = model.IsActive;
          entityUpdate.UserId = model.UserId;
          entityUpdate.ToDate = model.ToDate;
          entityUpdate.Fromdate = model.Fromdate;
          entityUpdate.Description = model.Description;
          entityUpdate.Title = model.Title;
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

    public async Task<bool> UpdateScheduledResultDocumentAsync(ScheduledResultDocumentModel model)
    {
      try
      {
        var entityUpdate = await _dataContext.ScheduledResultDocumentModel.FirstOrDefaultAsync(x => x.ScheduledResultDocumentId == model.ScheduledResultDocumentId);
        if (entityUpdate != null)
        {
          entityUpdate.ScheduleId = model.ScheduleId;
          entityUpdate.Status = model.Status;
          entityUpdate.Path = model.Path;
          entityUpdate.Title = model.Title;
          entityUpdate.DocumentUpdatedDate = model.DocumentUpdatedDate;
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

    public async Task<bool> UpdateScheduledResultReportAsync(ScheduledResultReportModel model)
    {
      try
      {
        var entityUpdate = await _dataContext.ScheduledResultReportModel.FirstOrDefaultAsync(x => x.ScheduledResultReportId == model.ScheduledResultReportId);
        if (entityUpdate != null)
        {
          entityUpdate.ScheduleId = model.ScheduleId;
          entityUpdate.Path = model.Path;
          entityUpdate.UserId = model.UserId;
          entityUpdate.ReportTime = model.ReportTime;
          entityUpdate.ReportContent = model.ReportContent;
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

    public async Task<bool> UpdateIsShareFieldOfScheduleFileAttachmentByIdAsync(int id,bool isShare)
    {
      try
      {
        var entityUpdate = await _dataContext.ScheduleFilesAttachment.FirstOrDefaultAsync(x => x.Id == id);
        if (entityUpdate != null)
        {         
          entityUpdate.IsShare = isShare;
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
