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
  public partial interface IOrganizeRepository : IRepository<OrganizeModel>
  {





  }

  public partial class OrganizeRepository : RepositoryBase<OrganizeModel>, IOrganizeRepository
  {
    private WebApiDbContext _dataContext;
    private AutoMapper.IMapper _mapper;
    public OrganizeRepository(WebApiDbContext context, AutoMapper.IMapper mapper) : base(context)
    {
      _dataContext = context;
      _mapper = mapper;
    }








  }
}
