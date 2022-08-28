using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data.Infrastructure;
using WebApi.Helpers;
using WebApi.Models;

namespace WebApi.Data.Repositories
{
  public partial interface IOrganizeRepository 
  {
    Task<IEnumerable<OrganizeModel>> GetOrganizeListAsync();

  }

  public partial class OrganizeRepository : RepositoryBase<OrganizeModel>, IOrganizeRepository
  {
    public async Task<IEnumerable<OrganizeModel>> GetOrganizeListAsync()
    {
      var organizeList = await _dataContext.OrganizeModel.ToListAsync();
      return organizeList;
    }
  }
}
