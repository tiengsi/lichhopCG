using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data.Infrastructure;
using WebApi.Models;

namespace WebApi.Data.Repositories
{
  public interface IScheduleHistoryRepository : IRepository<ScheduleHistoryModel>
  {
    Task AssignValueUserIdInScheduleHistory(int userId, bool isSuperAdmin);
    Task AssignValueLocationIdInScheduleHistory(int locationId);
  }

  public class ScheduleHistoryRepository : RepositoryBase<ScheduleHistoryModel>, IScheduleHistoryRepository
  {
    private WebApiDbContext _dataContext;
    public ScheduleHistoryRepository(WebApiDbContext context) : base(context)
    {
      _dataContext = context;
    }
    public async Task AssignValueUserIdInScheduleHistory(int userId, bool isSuperAdmin)
    {
      var schedulesHistory = await _dataContext.ScheduleHistoryModel.Where(m => m.Id == userId).ToListAsync();
      foreach (var item in schedulesHistory)
      {
        if (isSuperAdmin == false)
        {
          item.Id = null;
          _dataContext.ScheduleHistoryModel.Update(item);
          await _dataContext.SaveChangesAsync();
        }
      }
    }
    public async Task AssignValueLocationIdInScheduleHistory(int locationId)
    {
      var schedulesHistory = await _dataContext.ScheduleHistoryModel.Where(m =>m.LocationId==locationId).ToListAsync();
      foreach (var item in schedulesHistory)
      {
          item.Id = null;
          _dataContext.ScheduleHistoryModel.Update(item);
        await _dataContext.SaveChangesAsync();
      }
    }
  }
}
