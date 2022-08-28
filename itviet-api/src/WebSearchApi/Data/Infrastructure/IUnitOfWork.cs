namespace WebApi.Data.Infrastructure
{
    public interface IUnitOfWork
    {
        void Commit();
    }
}
