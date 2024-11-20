using DAL.Enteties.ClientOperations.Request;
using DAL.Enteties.ClientOperations.Result;

namespace DAL.Functions
{
    public interface IClientOperationFunction
    {
        public Task<ClientOperationResult?> GetOperationByIdAsync(Guid requestId);
        public Task<ClientOperationResult[]?> GetOperationsAsync(ClientOperationsRequest model);
    }
}
