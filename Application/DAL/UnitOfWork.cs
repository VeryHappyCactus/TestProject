using DAL.Respositories;

namespace DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        public IClientOperationRepository ClientOperationRepository { get; init; }

        public UnitOfWork(IDataContext dataContext)
        {
            this.ClientOperationRepository = new ClientOperationRepository(dataContext);
        }

    }
}
