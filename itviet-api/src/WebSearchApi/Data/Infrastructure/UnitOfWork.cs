namespace WebApi.Data.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private WebApiDbContext _dbContext;

        public UnitOfWork(WebApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Commit()
        {
            _dbContext.SaveChanges();
        }
    }
}
