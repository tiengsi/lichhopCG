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
  public partial interface IScheduleRepository : IRepository<ScheduleModel>
  {
    Task AssignValueUserIdInSchedule(int userId, bool isSuperAdmin);
    Task AssignValueLocationIdInSchedule(int locationId);
    //Task DeleteScheduleByScheduleTitleTemplateId(int scheduleTitleTemplateId);
    Task AssignValueScheduleTitileTemplateIdInSchedule(int scheduleTitleTemplateId);
  }

  public partial class ScheduleRepository : RepositoryBase<ScheduleModel>, IScheduleRepository
  {
    private WebApiDbContext _dataContext;
    private AutoMapper.IMapper _mapper;
    public ScheduleRepository(WebApiDbContext context, AutoMapper.IMapper mapper) : base(context)
    {
      _dataContext = context;
      _mapper = mapper;
    }
    public async Task AssignValueScheduleTitileTemplateIdInSchedule(int scheduleTitleTemplateId)
    {
      var schedules = await _dataContext.ScheduleModel.Where(m => m.ScheduleTitleTemplateId == scheduleTitleTemplateId).ToListAsync();
      foreach (var item in schedules)
      {
        item.ScheduleTitleTemplateId = null;
        _dataContext.ScheduleModel.Update(item);
      }
    }
    public async Task AssignValueUserIdInSchedule(int userId, bool isSuperAdmin)
    {
      var schedules = await _dataContext.ScheduleModel.Where(m => m.Id == userId).ToListAsync();
      foreach (var item in schedules)
      {
        if (isSuperAdmin == false)
        {
          item.Id = null;
          _dataContext.ScheduleModel.Update(item);
        }
      }
    }
    public async Task AssignValueLocationIdInSchedule(int locationId)
    {
      var schedules = await _dataContext.ScheduleModel.Where(m => m.LocationId == locationId).ToListAsync();
      foreach (var item in schedules)
      {
        item.LocationId = null;
        _dataContext.ScheduleModel.Update(item);

      }
    }
    //public async Task DeleteScheduleByScheduleTitleTemplateId(int scheduleTitleTemplateId)
    //{
    //  var scheduleModels = await _dataContext.ScheduleModel.Where(m => m.ScheduleTitleTemplateId == scheduleTitleTemplateId).ToListAsync();
    //  _dataContext.ScheduleModel.RemoveRange(scheduleModels);
    //  foreach (var item in scheduleModels)
    //  {
    //    var personalNotes = await _dataContext.PersonalNotesModel.Where(m => m.ScheduleId == item.ScheduleId).ToListAsync();
    //    _dataContext.PersonalNotesModel.RemoveRange(personalNotes);
    //    var scheduleResultDocumet = await _dataContext.ScheduledResultDocumentModel.Where(m => m.ScheduleId == item.ScheduleId).ToListAsync();
    //    _dataContext.ScheduledResultDocumentModel.RemoveRange(scheduleResultDocumet);
    //    var scheduleResultReports = await _dataContext.ScheduledResultReportModel.Where(m => m.ScheduleId == item.ScheduleId).ToListAsync();
    //    _dataContext.ScheduledResultReportModel.RemoveRange(scheduleResultReports);
    //    var emailLogsInSchedule = await _dataContext.EmailLogsModel.Where(m => m.ScheduleId == item.ScheduleId).ToListAsync();
    //    _dataContext.EmailLogsModel.RemoveRange(emailLogsInSchedule);
    //    var otherParticipant = await _dataContext.OtherParticipantModel.Where(m => m.ScheduleId == item.ScheduleId).ToListAsync();
    //    _dataContext.OtherParticipantModel.RemoveRange(otherParticipant);
    //    foreach (var itemEmail in otherParticipant)
    //    {
    //      var emailLogsInOtherparticipant = await _dataContext.EmailLogsModel.Where(m => m.OtherParticipantId == itemEmail.OtherParticipantId).ToListAsync();
    //      _dataContext.EmailLogsModel.RemoveRange(emailLogsInOtherparticipant);
    //    }
    //  }

    //}
  }
}
