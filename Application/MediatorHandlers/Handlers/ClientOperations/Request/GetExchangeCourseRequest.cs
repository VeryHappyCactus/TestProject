using MediatR;

using MediatorHandlers.Handlers.ClientOperations.Result;

namespace MediatorHandlers.Handlers.ClientOperations.Request
{
    public class GetExchangeCourseRequest : IRequest<IEnumerable<ExchangeCourseResult>?>
    {
        public DateTime? Date { get; set; }
    }
}
