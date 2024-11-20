using DAL.Enteties.ClientOperations.Request;
using DAL.Enteties.ClientOperations.Result;

namespace DAL.StoredProcedures
{
    public interface IClientOperationProcedure
    {
        public Task<ClientWithdrawOperationResult> WithdrawAsync(ClientWithdrawOperationRequest model);
    }
}
