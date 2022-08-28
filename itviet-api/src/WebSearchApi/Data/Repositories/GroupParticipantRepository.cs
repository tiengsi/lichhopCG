using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data.Infrastructure;
using WebApi.Models;

namespace WebApi.Data.Repositories
{
  public interface IGroupParticipantRepository : IRepository<GroupParticipantModel>
  {
    Task<IEnumerable<GroupParticipantModel>> GetListGroupParticipantByOrganizeIdAsync(int organizeId);
    Task<bool> DeleteListGroupParticipantByOrganizeIdAsync(int organizeId);
  }

  public class GroupParticipantRepository : RepositoryBase<GroupParticipantModel>, IGroupParticipantRepository
  {
    private WebApiDbContext _dataContext;
    public GroupParticipantRepository(WebApiDbContext context) : base(context)
    {
      _dataContext = context;
    }
    public async Task<IEnumerable<GroupParticipantModel>> GetListGroupParticipantByOrganizeIdAsync(int organizeId)
    {
      var groupParticipants = await _dataContext.GroupParticipantModel.Where(m => m.OrganizeId == organizeId).ToListAsync();
      return groupParticipants;
    }
    
    public async Task<bool> DeleteListGroupParticipantByOrganizeIdAsync(int organizeId)
    {
      try
      {
        var result = await _dataContext.GroupParticipantModel.Where(m => m.OrganizeId == organizeId).ToListAsync();
        if (result == null) return false;
        _dataContext.GroupParticipantModel.RemoveRange(result);
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

