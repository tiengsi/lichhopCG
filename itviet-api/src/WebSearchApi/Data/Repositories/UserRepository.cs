using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data.Infrastructure;
using WebApi.Models;
using WebApi.Models.Dtos;

namespace WebApi.Data.Repositories
{
  public interface IUserRepository : IRepository<UserModel>
  {
    Task<bool> CheckUserRoleAsync(List<string> roles);
  }

  public class UserRepository : RepositoryBase<UserModel>, IUserRepository
  {
    private WebApiDbContext _dataContext;
    public UserRepository(WebApiDbContext context) : base(context)
    {
      _dataContext = context;
    }
    public async Task<bool> CheckUserRoleAsync(List<string> roles)
    {
      foreach (var item in roles)
      {
        var role = await _dataContext.Roles.Where(m => m.Name == item).CountAsync();
        if (role == 0) return false;
      }
      return true;
    }
  }
}
