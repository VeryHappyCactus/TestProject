using DAL.Enteties.ClientOperations.Request;
using DAL.Enteties.ClientOperations.Result;

namespace DAL.Respositories
{
    public interface IClientOperationRepository
    {
        public Task<ClientWithdrawOperationResult> CreateClientOperation(ClientWithdrawOperationRequest request);
        public Task<ClientOperationResult?> GetClientOperationById(Guid clientOperationId);
        public Task<IEnumerable<ClientOperationResult>?> GetClientOperations(ClientOperationsRequest request);
    }
}
