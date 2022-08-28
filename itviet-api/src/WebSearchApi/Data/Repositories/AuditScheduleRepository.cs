using WebApi.Data.Infrastructure;
using WebApi.Models;

namespace WebApi.Data.Repositories
{
    public interface IAuditScheduleRepository: IRepository<AuditScheduleModel>
    {
    }

    public class AuditScheduleRepository: RepositoryBase<AuditScheduleModel>, IAuditScheduleRepository
    {
        public AuditScheduleRepository(WebApiDbContext context) : base(context)
        {
        }
    }
}
