using DAL.Enteties.ClientOperations.Request;
using DAL.Enteties.ClientOperations.Result;

namespace DAL.StoredProcedures.Interfaces
{
    public interface IClientOperationProcedure
    {
        public Task<ClientWithdrawOperationResult> WithdrawAsync(ClientWithdrawOperationRequest model);
    }
}
