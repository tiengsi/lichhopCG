using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace WebApi.Data.Infrastructure
{
    public interface IRepository<T> where T : class
    {
        // Marks an entity as new
        void Add(T entity);
        void Add_Ex(T entity);

        // Marks an entity as new
        void AddMulti(List<T> entity);
        void AddMulti_Ex(List<T> entity);
        // Marks an entity as modified
        void Update(T entity);
        void Update_Ex(T entity);

        // Marks an entity to be removed
        void Delete(T entity);

        //Delete multi records
        void DeleteMulti(Expression<Func<T, bool>> where);

        //get pagging, condition, sort by name
        ValueTask<List<T>> GetMultiPaging(Expression<Func<T, bool>> filter, string sortBy, bool isDescending, int index = 0, int size = 50, string[] includes = null);

        // Get an entity by int id
        ValueTask<T> GetSingleById(int id);

        ValueTask<T> GetSingleById(string id);

        Task<T> GetSingleByCondition(Expression<Func<T, bool>> expression, string[] includes = null);

        Task<List<T>> GetAll(string[] includes = null);

        Task<List<TType>> GetAllSelect<TType>(Expression<Func<T, bool>> predicate, Expression<Func<T, TType>> select, string[] includes = null) where TType : class;

        Task<List<T>> GetMulti(Expression<Func<T, bool>> predicate, string[] includes = null);
        Task<List<T>> GetMulti_Ex(Expression<Func<T, bool>> predicate, string[] includes = null);

        Task<int> Count(Expression<Func<T, bool>> where);

        Task<int> CountAll();

        Task<bool> CheckContains(Expression<Func<T, bool>> predicate);
    }
}
