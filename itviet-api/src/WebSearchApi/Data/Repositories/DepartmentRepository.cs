using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data.Infrastructure;
using WebApi.Models;
namespace WebApi.Data.Repositories
{
  public interface IDepartmentRepository : IRepository<DepartmentModel>
  {
    Task<bool> CheckUserRepresentativeExistedAsync(List<int> userRepresentative);
   

  }

  public class DepartmentRepository : RepositoryBase<DepartmentModel>, IDepartmentRepository
  {
    private WebApiDbContext _dataContext;
    public DepartmentRepository(WebApiDbContext context) : base(context)
    {
      _dataContext = context;
    }

    public async Task<bool> CheckUserRepresentativeExistedAsync(List<int> userRepresentative)
    {
      foreach (var item in userRepresentative)
      {
        var userIdInRepresentative = await _dataContext.RepresentativeModel.Where(m => m.UserId == item).CountAsync();
        if (userIdInRepresentative == 0) return false;
      }
      return true;
    }
   
  }
}
