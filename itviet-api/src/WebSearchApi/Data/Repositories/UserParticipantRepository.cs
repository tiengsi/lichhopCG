using WebApi.Data.Infrastructure;
using WebApi.Models;

namespace WebApi.Data.Repositories
{
    public interface IUserParticipantRepository : IRepository<UserParticipantModel>
    {
    }

    public class UserParticipantRepository : RepositoryBase<UserParticipantModel>, IUserParticipantRepository
    {
        public UserParticipantRepository(WebApiDbContext context) : base(context)
        {
        }
    }
}
