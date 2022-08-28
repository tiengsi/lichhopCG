using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data.Infrastructure;
using WebApi.Models;

namespace WebApi.Data.Repositories
{

  public interface IEmailLogsRepository : IRepository<EmailLogsModel>
  {
    Task AssignValueUserIdInEmailLog(int userId, bool isSuperAdmin);
  }

  public class EmailLogsRepository : RepositoryBase<EmailLogsModel>, IEmailLogsRepository
  {
    private WebApiDbContext _dataContext;
    public EmailLogsRepository(WebApiDbContext context) : base(context)
    {
      _dataContext = context;
    }
    public async Task AssignValueUserIdInEmailLog(int userId, bool isSuperAdmin)
    {
      var emailLogs = await _dataContext.EmailLogsModel.Where(m => m.UserId == userId).ToListAsync();
      foreach (var item in emailLogs)
      {
        if (isSuperAdmin == false)
        {
          item.UserId = null;
          _dataContext.EmailLogsModel.Update(item);
        }
      }
    }
  }
}
