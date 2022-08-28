using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApi.Data.Infrastructure;
using WebApi.Models;

namespace WebApi.Data.Repositories
{
  public interface IJobScheduleRepository : IRepository<JobScheduleModel>
  {
    Task<List<JobScheduleModel>> GetMultiJobAsync(Expression<Func<JobScheduleModel, bool>> predicate, string[] includes = null);
    Task<JobScheduleModel> GetSingleById_ExAsync(int id);
    Task<bool> UpdateStatusJobScheduleAsync(JobScheduleModel model);
  }

  public class JobScheduleRepository : RepositoryBase<JobScheduleModel>, IJobScheduleRepository
  {
    private WebApiDbContext _dataContext;
    private AutoMapper.IMapper _mapper;
    public JobScheduleRepository(WebApiDbContext context, AutoMapper.IMapper mapper) : base(context)
    {
      _dataContext = context;
      _mapper = mapper;
    }

    public async Task<List<JobScheduleModel>> GetMultiJobAsync(Expression<Func<JobScheduleModel, bool>> predicate, string[] includes = null)
    {
      try
      {
        var newClacc = new WebApiContextDesignFactory();
        using (WebApiDbContext newdb = newClacc.CreateDbContext(null))
        {
          if (includes != null && includes.Count() > 0)
          {
            var query = newdb.Set<JobScheduleModel>().Include(includes.First());
            foreach (var include in includes.Skip(1))
              query = query.Include(include);
            var buildQuery = await query.Where<JobScheduleModel>(predicate).AsQueryable<JobScheduleModel>().ToListAsync();
            return buildQuery;
          }

          var buildQ = await newdb.Set<JobScheduleModel>().Where<JobScheduleModel>(predicate).AsQueryable<JobScheduleModel>().ToListAsync();
          return buildQ;
        }     

      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    public async Task<JobScheduleModel> GetSingleById_ExAsync(int id)
    {
      try
      {
        var newClacc = new WebApiContextDesignFactory();
        using (WebApiDbContext newdb = newClacc.CreateDbContext(null))
        {
          var result = await newdb.JobScheduleModel.FirstOrDefaultAsync(x => x.Id==id);
          return result;
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    public async Task<bool> UpdateStatusJobScheduleAsync(JobScheduleModel model)
    {
      try
      {
        var newClacc = new WebApiContextDesignFactory();
        using (WebApiDbContext newdb = newClacc.CreateDbContext(null))
        {
          var entityUpdate = await newdb.JobScheduleModel.FirstOrDefaultAsync(x => x.Id == model.Id);
          if (entityUpdate != null)
          {
            entityUpdate.IsExecuted = model.IsExecuted;
            await newdb.SaveChangesAsync();
            return true;
          }
          return false;
        }
      }
      catch (System.Exception ex)
      {
        throw ex;
      }
    }
  }

}
