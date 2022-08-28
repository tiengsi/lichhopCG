using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data.Infrastructure;
using WebApi.Models;

namespace WebApi.Data.Repositories
{
  public interface IOtherParticipantRepository : IRepository<OtherParticipantModel>
  {
    Task DeleteGroupParticipantIdInOtherParticipant(int groupParticipantId);

  }

  public class OtherParticipantRepository : RepositoryBase<OtherParticipantModel>, IOtherParticipantRepository
  {
    private WebApiDbContext _dataContext;
    public OtherParticipantRepository(WebApiDbContext context) : base(context)
    {
      _dataContext = context;
    }
    public async Task DeleteGroupParticipantIdInOtherParticipant(int groupParticipantId)
    {
      var otherParticipantModels = await _dataContext.OtherParticipantModel.Where(m => m.GroupParticipantId == groupParticipantId).ToListAsync();
      _dataContext.OtherParticipantModel.RemoveRange(otherParticipantModels);
      foreach(var item in otherParticipantModels)
      {
       var emailLogs=_dataContext.EmailLogsModel.Where(m => m.OtherParticipantId == item.OtherParticipantId).ToList();
        _dataContext.EmailLogsModel.RemoveRange(emailLogs);
      }
    }
  }

  public interface IOtherParticipantTemplateRepository : IRepository<OtherParticipantTemplateModel>
  {
  }

  public class OtherParticipantTemplateRepository : RepositoryBase<OtherParticipantTemplateModel>, IOtherParticipantTemplateRepository
  {
    public OtherParticipantTemplateRepository(WebApiDbContext context) : base(context)
    {
    }
  }
}
