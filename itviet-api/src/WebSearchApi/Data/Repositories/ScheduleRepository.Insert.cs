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
    Task<bool> InsertPersonalScheduleAsync(PersonalScheduleModel model);
    Task<bool> InsertScheduledResultReportAsync(ScheduledResultReportModel model);
    Task<bool> InsertPersonalNotesAsync(PersonalNotesModel model);
    Task<bool> InsertScheduledResultDocumentAsync(ScheduledResultDocumentModel model);
  }

  public partial class ScheduleRepository : RepositoryBase<ScheduleModel>, IScheduleRepository
  {
    public async Task<bool> InsertPersonalNotesAsync(PersonalNotesModel model)
    {
      try
      {
        var result = await _dataContext.PersonalNotesModel.AddAsync(model);
        await _dataContext.SaveChangesAsync();
        return true;
      }
      catch (System.Exception ex)
      {

        throw ex;
      }

    }

    public async Task<bool> InsertPersonalScheduleAsync(PersonalScheduleModel model)
    {
      try
      {
        var result = await _dataContext.PersonalScheduleModel.AddAsync(model);
        await _dataContext.SaveChangesAsync();
        return true;
      }
      catch (System.Exception ex)
      {

        throw ex;
      }

    }

    public async Task<bool> InsertScheduledResultDocumentAsync(ScheduledResultDocumentModel model)
    {
      try
      {
        var result = await _dataContext.ScheduledResultDocumentModel.AddAsync(model);
        await _dataContext.SaveChangesAsync();
        return true;
      }
      catch (System.Exception ex)
      {

        throw ex;
      }
    }

    public async Task<bool> InsertScheduledResultReportAsync(ScheduledResultReportModel model)
    {
      try
      {
        var result = await _dataContext.ScheduledResultReportModel.AddAsync(model);
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
