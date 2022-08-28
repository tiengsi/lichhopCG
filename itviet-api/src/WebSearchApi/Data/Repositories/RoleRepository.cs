using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data.Infrastructure;
using WebApi.Models;

namespace WebApi.Data.Repositories
{
  public partial interface IRoleRepository : IRepository<RoleModel>
  {

  }

  public partial class RoleRepository : RepositoryBase<RoleModel>, IRoleRepository
  {
    private WebApiDbContext _dataContext;
    public RoleRepository(WebApiDbContext context) : base(context)
    {
      _dataContext = context;
    }
 


  }

}
