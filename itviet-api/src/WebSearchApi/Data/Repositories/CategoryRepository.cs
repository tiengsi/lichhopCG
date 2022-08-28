using WebApi.Data.Infrastructure;
using WebApi.Models;

namespace WebApi.Data.Repositories
{
    public interface ICategoryRepository : IRepository<CategoryModel>
    {
    }

    public class CategoryRepository : RepositoryBase<CategoryModel>, ICategoryRepository
    {
        public CategoryRepository(WebApiDbContext context) : base(context)
        {
        }
    }
}
