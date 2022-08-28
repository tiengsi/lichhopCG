using WebApi.Data.Infrastructure;
using WebApi.Models;

namespace WebApi.Data.Repositories
{
    public interface IPostRepository : IRepository<PostModel>
    {
    }

    public class PostRepository : RepositoryBase<PostModel>, IPostRepository
    {
        public PostRepository(WebApiDbContext context) : base(context)
        {
        }
    }
}
