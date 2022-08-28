using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data.Infrastructure;
using WebApi.Models;

namespace WebApi.Data.Repositories
{
  public interface IScheduleTitleTemplateRepository : IRepository<ScheduleTitleTemplateModel>
  {
    Task<bool> DeleteListScheduleTitleTemplateByOrganizeIdAsync(int organizeId);
  }

  public class ScheduleTitleTemplateRepository : RepositoryBase<ScheduleTitleTemplateModel>, IScheduleTitleTemplateRepository
  {
    private WebApiDbContext _dataContext;
    private AutoMapper.IMapper _mapper;
    public ScheduleTitleTemplateRepository(WebApiDbContext context,AutoMapper.IMapper mapper) : base(context)
    {
      _dataContext = context;
      _mapper = mapper;
    }
    public async Task<bool> DeleteListScheduleTitleTemplateByOrganizeIdAsync(int organizeId)
    {
      try
      {
        var result = await _dataContext.ScheduleTitleTemplateModel.Where(m => m.OrganizeId == organizeId).ToListAsync();
        if (result == null) return false;
        _dataContext.ScheduleTitleTemplateModel.RemoveRange(result);
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
