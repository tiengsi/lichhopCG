using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApi.Helpers.Extensions;

namespace WebApi.Data.Infrastructure
{
  public abstract class RepositoryBase<T> : IRepository<T> where T : class
  {
    #region Properties
    private WebApiDbContext _dataContext;
    private readonly DbSet<T> dbSet;
    #endregion

    protected RepositoryBase(WebApiDbContext dataContext)
    {
      _dataContext = dataContext;
      dbSet = _dataContext.Set<T>();
    }

    #region Implementation

    public virtual void Add(T entity)
    {
      dbSet.Add(entity);
    }

    public  void Add_Ex(T entity)
    {
      var newClacc = new WebApiContextDesignFactory();
      using (WebApiDbContext dBContext = newClacc.CreateDbContext(null))
      {
        dBContext.Set<T>().Add(entity);
        dBContext.SaveChanges();
      }
    }

    public virtual void AddMulti(List<T> entity)
    {
      dbSet.AddRange(entity);
    }

    public  void AddMulti_Ex(List<T> entity)
    {
      var newClacc = new WebApiContextDesignFactory();
      using (WebApiDbContext dBContext = newClacc.CreateDbContext(null))
      {
        dBContext.Set<T>().AddRange(entity);
        dBContext.SaveChanges();
      }
    }

    public virtual void Update(T entity)
    {
      _dataContext.Attach(entity);
      _dataContext.Entry(entity).State = EntityState.Modified;
    }

    public void Update_Ex(T entity)
    {
      var newClacc = new WebApiContextDesignFactory();
      using (WebApiDbContext dBContext = newClacc.CreateDbContext(null))
      {
        dBContext.Set<T>().Update(entity);
        dBContext.SaveChanges();
      }
    }

    public virtual void Delete(T entity)
    {
      _dataContext.Remove(entity);
    }

    public virtual void DeleteMulti(Expression<Func<T, bool>> where)
    {
      IEnumerable<T> objects = dbSet.Where<T>(where).AsEnumerable();
      foreach (T obj in objects)
        dbSet.Remove(obj);
    }

    public async ValueTask<List<T>> GetMultiPaging(Expression<Func<T, bool>> predicate, string sortBy, bool isDescending, int index = 0, int size = 20, string[] includes = null)
    {
      int skipCount = index < 0 ? 0 : (index - 1) * size;
      IQueryable<T> _resetSet;

      if (includes != null && includes.Count() > 0)
      {
        var query = _dataContext.Set<T>().Include(includes.First());
        foreach (var include in includes.Skip(1))
          query = query.Include(include);
        _resetSet = predicate != null ? query.Where<T>(predicate).AsQueryable() : query.AsQueryable();
      }
      else
      {
        _resetSet = predicate != null ? _dataContext.Set<T>().Where<T>(predicate).AsQueryable() : _dataContext.Set<T>().AsQueryable();
      }
      var sortExpression = string.Empty;
      if (!string.IsNullOrEmpty(sortBy))
      {
        sortExpression = sortBy + " " + (isDescending ? "DESC" : "ASC");
      }
      _resetSet = _resetSet.OrderByCustom(sortExpression);
      _resetSet = skipCount == 0 ? _resetSet.Take(size) : _resetSet.Skip(skipCount).Take(size);
      return await _resetSet.AsQueryable().ToListAsync();
    }

    public async ValueTask<T> GetSingleById(int id)
    {
      var items = await dbSet.FindAsync(id);
      return items;
    }

    public async ValueTask<T> GetSingleById(string id)
    {
      var items = await dbSet.FindAsync(id);
      return items;
    }

    public async Task<int> Count(Expression<Func<T, bool>> where)
    {
      var count = await _dataContext.Set<T>().CountAsync(where);
      return count;
    }

    public async Task<int> CountAll()
    {
      var count = await _dataContext.Set<T>().CountAsync();
      return count;
    }

    public async Task<List<T>> GetAll(string[] includes = null)
    {
      //HANDLE INCLUDES FOR ASSOCIATED OBJECTS IF APPLICABLE
      if (includes != null && includes.Count() > 0)
      {
        var query = _dataContext.Set<T>().Include(includes.First());
        foreach (var include in includes.Skip(1))
          query = query.Include(include);
        var buildQuery = await query.AsQueryable().ToListAsync();
        return buildQuery;
      }

      var buildQ = await _dataContext.Set<T>().AsQueryable().ToListAsync();
      return buildQ;
    }

    public async Task<List<TType>> GetAllSelect<TType>(Expression<Func<T, bool>> predicate, Expression<Func<T, TType>> select = null, string[] includes = null) where TType : class
    {
      //HANDLE INCLUDES FOR ASSOCIATED OBJECTS IF APPLICABLE
      if (includes != null && includes.Count() > 0)
      {
        var query = _dataContext.Set<T>().Include(includes.First());
        foreach (var include in includes.Skip(1))
          query = query.Include(include);
        var buildQuery = await query.Where<T>(predicate).Select(select).AsQueryable().ToListAsync();
        return buildQuery;
      }

      var buildQ = await _dataContext.Set<T>().Where<T>(predicate).Select(select).AsQueryable().ToListAsync();
      return buildQ;
    }

    public async Task<T> GetSingleByCondition(Expression<Func<T, bool>> expression, string[] includes = null)
    {
      if (includes != null && includes.Count() > 0)
      {
        var query = _dataContext.Set<T>().Include(includes.First());
        foreach (var include in includes.Skip(1))
          query = query.Include(include);
        var buildQuery = await query.FirstOrDefaultAsync(expression);
        return buildQuery;
      }

      var buildQ = await _dataContext.Set<T>().FirstOrDefaultAsync(expression);
      return buildQ;
    }

    public async Task<List<T>> GetMulti(Expression<Func<T, bool>> predicate, string[] includes = null)
    {
      //HANDLE INCLUDES FOR ASSOCIATED OBJECTS IF APPLICABLE
      if (includes != null && includes.Count() > 0)
      {
        var query = _dataContext.Set<T>().Include(includes.First());
        foreach (var include in includes.Skip(1))
          query = query.Include(include);
        var buildQuery = await query.Where<T>(predicate).AsQueryable<T>().ToListAsync();
        return buildQuery;
      }

      var buildQ = await _dataContext.Set<T>().Where<T>(predicate).AsQueryable<T>().ToListAsync();
      return buildQ;
    }

    public async Task<List<T>> GetMulti_Ex(Expression<Func<T, bool>> predicate, string[] includes = null)
    {
      var newClacc = new WebApiContextDesignFactory();
      using (WebApiDbContext dBContext = newClacc.CreateDbContext(null))
      {
        //HANDLE INCLUDES FOR ASSOCIATED OBJECTS IF APPLICABLE
        if (includes != null && includes.Count() > 0)
        {
          var query = dBContext.Set<T>().Include(includes.First());
          foreach (var include in includes.Skip(1))
            query = query.Include(include);
          var buildQuery = await query.Where<T>(predicate).AsQueryable<T>().ToListAsync();
          return buildQuery;
        }

        var buildQ = await dBContext.Set<T>().Where<T>(predicate).AsQueryable<T>().ToListAsync();
        return buildQ;
      }
    }

    public async Task<bool> CheckContains(Expression<Func<T, bool>> predicate)
    {
      int count = await _dataContext.Set<T>().CountAsync<T>(predicate);
      return count > 0;
    }
    #endregion
  }
}
