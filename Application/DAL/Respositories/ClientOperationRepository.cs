using DAL.Contexts;
using DAL.Enteties.ClientOperations.Request;
using DAL.Enteties.ClientOperations.Result;

namespace DAL.Respositories
{
    public class ClientOperationRepository : IClientOperationRepository
    {
        private IDataContext _context;

        public ClientOperationRepository(IDataContext dataContext) 
        { 
            _context = dataContext;
        }

        public async Task<ClientWithdrawOperationResult> CreateClientOperation(ClientWithdrawOperationRequest request)
        {
            return await _context.Procedure.ClientOperationProcedure.WithdrawAsync(request);
        }

        public async Task<ClientOperationResult?> GetClientOperationById(Guid clientOperationId)
        {
            return await _context.Function.CleintOperationFunction.GetOperationByIdAsync(clientOperationId);
        }

        public async Task<IEnumerable<ClientOperationResult>?> GetClientOperations(ClientOperationsRequest request)
        {
            return await _context.Function.CleintOperationFunction.GetOperationsAsync(request);
        }
    }
}
