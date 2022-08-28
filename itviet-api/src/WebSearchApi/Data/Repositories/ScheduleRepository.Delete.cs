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
    Task<bool> DeletePersonalNotesByIdAsync(int personalNoteId);
    Task<bool> DeletePersonalNotesByScheduleIdAsync(int scheduleId);
    Task<bool> DeleteScheduledResultDocumentByScheduleIdAsync(int scheduleId);
    Task<bool> DeleteScheduledResultReportByScheduleIdAsync(int scheduleId);
    Task<bool> DeletePersonalScheduleAsync(int personalScheduleId);
    Task<bool> DeleteScheduleTypeByOrganizeIdAsync(int organizeId);
    

  }

  public partial class ScheduleRepository : RepositoryBase<ScheduleModel>, IScheduleRepository
  {
    public async Task<bool> DeletePersonalNotesByIdAsync(int personalNoteId)
    {
      try
      {
        var result = await _dataContext.PersonalNotesModel.Where(x => x.PersonalNotesId == personalNoteId).FirstOrDefaultAsync();
        if (result == null) return false;

        _dataContext.PersonalNotesModel.Remove(result);
        await _dataContext.SaveChangesAsync();
        return true;
      }
      catch (System.Exception ex)
      {

        throw ex;
      }
    }

    public async Task<bool> DeletePersonalScheduleAsync(int personalScheduleId)
    {
      try
      {
        var result = await _dataContext.PersonalScheduleModel.Where(x => x.PersonalScheduleId == personalScheduleId).FirstOrDefaultAsync();
        if (result == null) return false;

        _dataContext.PersonalScheduleModel.Remove(result);
        await _dataContext.SaveChangesAsync();
        return true;
      }
      catch (System.Exception ex)
      {

        throw ex;
      }
    }

    public async Task<bool> DeleteScheduleTypeByOrganizeIdAsync(int organizeId)
    {
      try
      {
        var result = await _dataContext.ScheduleTypeModel.Where(x => x.OrganizeId == organizeId).ToListAsync();
        if (result == null) return false;

        _dataContext.ScheduleTypeModel.RemoveRange(result);
        await _dataContext.SaveChangesAsync();
        return true;
      }
      catch (System.Exception ex)
      {

        throw ex;
      }
    }

    public async Task<bool> DeletePersonalNotesByScheduleIdAsync(int scheduleId)
    {
      try
      {
        var result = await _dataContext.PersonalNotesModel.Where(x => x.ScheduleId == scheduleId).ToListAsync();
        if (result == null) return false;

        _dataContext.PersonalNotesModel.RemoveRange(result);
        await _dataContext.SaveChangesAsync();
        return true;
      }
      catch (System.Exception ex)
      {

        throw ex;
      }
    }

    public async Task<bool> DeleteScheduledResultDocumentByScheduleIdAsync(int scheduleId)
    {
      try
      {
        var result = await _dataContext.ScheduledResultDocumentModel.Where(x => x.ScheduleId == scheduleId).ToListAsync();
        if (result == null) return false;

        _dataContext.ScheduledResultDocumentModel.RemoveRange(result);
        await _dataContext.SaveChangesAsync();
        return true;
      }
      catch (System.Exception ex)
      {

        throw ex;
      }
    }

    public async Task<bool> DeleteScheduledResultReportByScheduleIdAsync(int scheduleId)
    {
      try
      {
        var result = await _dataContext.ScheduledResultReportModel.Where(x => x.ScheduleId == scheduleId).ToListAsync();
        if (result == null) return false;

        _dataContext.ScheduledResultReportModel.RemoveRange(result);
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
