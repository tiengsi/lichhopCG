using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data.Infrastructure;
using WebApi.Models;

namespace WebApi.Data.Repositories
{

  public interface IRepresentativeRepository : IRepository<RepresentativeModel>
  {
    Task DeleteRepresentativeByDepartmentIdAsync(DepartmentModel model);
  }

  public class RepresentativeRepository : RepositoryBase<RepresentativeModel>, IRepresentativeRepository
  {
    private WebApiDbContext _dataContext;
    public RepresentativeRepository(WebApiDbContext context) : base(context)
    {
      _dataContext = context;
    }
    public async Task DeleteRepresentativeByDepartmentIdAsync(DepartmentModel model)
    {
      var representatives = await _dataContext.RepresentativeModel.Where(m => m.DepartmentId == model.Id).ToListAsync();
      _dataContext.RepresentativeModel.RemoveRange(representatives);
      await _dataContext.SaveChangesAsync();
    }
  }
}
