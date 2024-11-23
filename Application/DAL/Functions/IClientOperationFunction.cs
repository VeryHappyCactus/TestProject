using DAL.Enteties.ClientOperations.Request;
using DAL.Enteties.ClientOperations.Result;

namespace DAL.Functions
{
    public interface IClientOperationFunction
    {
        public Task<ClientOperationResult?> GetOperationByIdAsync(Guid requestId);
        public Task<IEnumerable<ClientOperationResult>?> GetOperationsAsync(ClientOperationsRequest model);
        public Task<IEnumerable<ExchangeCourseResult>?> GetExchangeCourseByDate(DateTime date);
        public Task<IEnumerable<ExchangeCourseResult>?> GetExchangeCourses();
    }
}
