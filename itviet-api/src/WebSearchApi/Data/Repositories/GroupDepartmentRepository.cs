using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data.Infrastructure;
using WebApi.Models;

namespace WebApi.Data.Repositories
{
  public interface IGroupDepartmentRepository : IRepository<GroupDepartmentModel>
  {
    Task DeleteGroupDepartmentByDepartmentIdAsync(DepartmentModel model);
  }

  public class GroupDepartmentRepository : RepositoryBase<GroupDepartmentModel>, IGroupDepartmentRepository
  {
    private WebApiDbContext _dataContext;
    public GroupDepartmentRepository(WebApiDbContext context) : base(context)
    {
      _dataContext = context;
    }
    public async Task DeleteGroupDepartmentByDepartmentIdAsync(DepartmentModel model)
    {
      var groupDepartments = await _dataContext.GroupDepartmentModel.Where(m => m.DepartmentId == model.Id).ToListAsync();
      _dataContext.GroupDepartmentModel.RemoveRange(groupDepartments);
      await _dataContext.SaveChangesAsync();
    }
  }
}
