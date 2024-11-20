using DAL.Respositories;

namespace DAL
{
    public interface IUnitOfWork
    {
        public IClientOperationRepository ClientOperationRepository { get; init; }
    }
}
