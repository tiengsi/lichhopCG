using WebApi.Data.Infrastructure;
using WebApi.Models;

namespace WebApi.Data.Repositories
{

    public interface IParticipantsRepository : IRepository<ParticipantsModel>
    {
    }

    public class ParticipantsRepository : RepositoryBase<ParticipantsModel>, IParticipantsRepository
    {
        public ParticipantsRepository(WebApiDbContext context) : base(context)
        {
        }
    }

    public interface IParticipantsTemplateRepository : IRepository<ParticipantsTemplateModel>
    {
    }

    public class ParticipantsTemplateRepository : RepositoryBase<ParticipantsTemplateModel>, IParticipantsTemplateRepository
    {
        public ParticipantsTemplateRepository(WebApiDbContext context) : base(context)
        {
        }
    }
}
