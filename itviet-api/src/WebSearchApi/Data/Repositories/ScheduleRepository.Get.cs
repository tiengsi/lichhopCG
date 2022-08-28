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
    Task<IEnumerable<PersonalNotesModel>> GetPersonalNotesByScheduleIdAndUserIdAsync(int scheduleId, int userId);
    Task<IEnumerable<ScheduledResultDocumentModel>> GetScheduledResultDocumentByScheduleIdAsync(int scheduleId);
    Task<IEnumerable<ScheduledResultReportModel>> GetScheduledResultReportByScheduleIdAsync(int scheduleId);
    Task<IEnumerable<PersonalScheduleModel>> GetPersonalScheduleByUserIdAsync(int userId);
    Task<IEnumerable<ScheduleModel>> GetScheduleByOrganizeIdAsync(int organizeId);
   

  }

  public partial class ScheduleRepository : RepositoryBase<ScheduleModel>, IScheduleRepository
  {
    public async Task<IEnumerable<PersonalNotesModel>> GetPersonalNotesByScheduleIdAndUserIdAsync(int scheduleId, int userId)
    {
      var data = _dataContext.PersonalNotesModel.Where(x => x.ScheduleId == scheduleId && x.UserId == userId).Include(x => x.User);
      var result = await data.ToListAsync();
      return await Task.Run(() => result);
    }

    public async Task<IEnumerable<PersonalScheduleModel>> GetPersonalScheduleByUserIdAsync(int userId)
    {
      var data = _dataContext.PersonalScheduleModel.Where(x => x.UserId == userId);
      var result = await data.ToListAsync();
      for (int i = 0; i < result.Count; i++)
      {
        var userInfo = _dataContext.Users.Where(x => x.Id == result[i].UserId).FirstOrDefault();
        result[i].User = userInfo;
      }
      return await Task.Run(() => result);
    }

    public async Task<IEnumerable<ScheduledResultDocumentModel>> GetScheduledResultDocumentByScheduleIdAsync(int scheduleId)
    {
      var data = _dataContext.ScheduledResultDocumentModel.Where(x => x.ScheduleId == scheduleId);
      var result = data.ToListAsync();
      return await Task.Run(() => result);
    }

    public async Task<IEnumerable<ScheduledResultReportModel>> GetScheduledResultReportByScheduleIdAsync(int scheduleId)
    {
      var data = _dataContext.ScheduledResultReportModel.Where(x => x.ScheduleId == scheduleId);
      var result = await data.ToListAsync();
      for (int i = 0; i < result.Count; i++)
      {
        var userInfo = _dataContext.Users.Where(x => x.Id == result[i].UserId).FirstOrDefault();
        result[i].User = userInfo;
      }
      return await Task.Run(() => result);
    }

    public async Task<IEnumerable<ScheduleModel>> GetScheduleByOrganizeIdAsync(int organizeId)
    {
      var data = _dataContext.ScheduleModel.Where(x => x.OrganizeId == organizeId);
      var result = await data.ToListAsync();
      //for (int i = 0; i < result.Count; i++)
      //{
      //  var userInfo = _dataContext.Users.Where(x => x.Id == result[i].UserId).FirstOrDefault();
      //  result[i].User = userInfo;
      //}
      return result;
    }

  }
}
