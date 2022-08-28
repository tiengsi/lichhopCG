using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data.Infrastructure;
using WebApi.Models;

namespace WebApi.Data.Repositories
{
  public interface IScheduleTemplateRepository : IRepository<ScheduleTemplateModel>
  {
    Task AssignValueUserIdInScheduleTemplate(int userId, bool isSuperAdmin);
    Task AssignValueLocationIdInScheduleTemplate(int locationId);
    Task<IEnumerable<ScheduleTemplateModel>> GetScheduleTemplateByOrganizeIdAsync(int organizeId);
    //Task DeleteScheduleTemplateByScheduleTitleTemplateId(int scheduleTitleTemplateId);
    Task AssignValueScheduleTitileTemplateIdInScheduleTemplate(int scheduleTitleTemplateId);
  }

  public class ScheduleTemplateRepository : RepositoryBase<ScheduleTemplateModel>, IScheduleTemplateRepository
  {
    private WebApiDbContext _dataContext;
    private AutoMapper.IMapper _mapper;
    public ScheduleTemplateRepository(WebApiDbContext context, AutoMapper.IMapper mapper) : base(context)
    {
      _dataContext = context;
      _mapper = mapper;
    }
    public async Task AssignValueScheduleTitileTemplateIdInScheduleTemplate(int scheduleTitleTemplateId)
    {
      var scheduleTemplates = await _dataContext.ScheduleTemplateModel.Where(m => m.ScheduleTitleTemplateId == scheduleTitleTemplateId).ToListAsync();
      foreach(var item in scheduleTemplates)
      {
        item.ScheduleTitleTemplateId = null;
        _dataContext.ScheduleTemplateModel.Update(item);
      }  
    }
    //public async Task DeleteScheduleTemplateByScheduleTitleTemplateId(int scheduleTitleTemplateId)
    //{
    //  var scheduleTemplateModels = await _dataContext.ScheduleTemplateModel.Where(m => m.ScheduleTitleTemplateId == scheduleTitleTemplateId).ToListAsync();
    //  _dataContext.ScheduleTemplateModel.RemoveRange(scheduleTemplateModels);
    //  foreach (var item in scheduleTemplateModels)
    //  {
    //    var otherParticipantTemplates = await _dataContext.OtherParticipantTemplateModel.Where(m => m.ScheduleId == item.ScheduleId).ToListAsync();
    //    _dataContext.OtherParticipantTemplateModel.RemoveRange(otherParticipantTemplates);
    //  }
    //}
    public async Task<IEnumerable<ScheduleTemplateModel>> GetScheduleTemplateByOrganizeIdAsync(int organizeId)
    {
      var data = _dataContext.ScheduleTemplateModel.Where(x => x.OrganizeId == organizeId);
      var result = await data.ToListAsync();
      //for (int i = 0; i < result.Count; i++)
      //{
      //  var userInfo = _dataContext.Users.Where(x => x.Id == result[i].UserId).FirstOrDefault();
      //  result[i].User = userInfo;
      //}
      return result;
    }
    public async Task AssignValueUserIdInScheduleTemplate(int userId, bool isSuperAdmin)
    {
      var schedulesTemplate = await _dataContext.ScheduleTemplateModel.Where(m => m.Id == userId).ToListAsync();
      foreach (var item in schedulesTemplate)
      {
        if (isSuperAdmin == false)
        {
          item.Id = null;
          _dataContext.ScheduleTemplateModel.Update(item);
        }
      }
    }
    public async Task AssignValueLocationIdInScheduleTemplate(int locationId)
    {
      var schedulesTemplate = await _dataContext.ScheduleTemplateModel.Where(m => m.LocationId == locationId).ToListAsync();
      foreach (var item in schedulesTemplate)
      {
        item.Id = null;
        _dataContext.ScheduleTemplateModel.Update(item);
      }
    }
  }
}
