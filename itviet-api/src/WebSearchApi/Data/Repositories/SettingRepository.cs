using WebApi.Data.Infrastructure;
using WebApi.Models;

namespace WebApi.Data.Repositories
{
    public interface ISettingRepository : IRepository<SettingModel>
    {
    }

    public class SettingRepository : RepositoryBase<SettingModel>, ISettingRepository
    {
        public SettingRepository(WebApiDbContext context) : base(context)
        {
        }
    }
}
