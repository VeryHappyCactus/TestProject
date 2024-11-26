using MediatR;

using ServiceLogic.Handlers.ClientOperations.Result;

namespace ServiceLogic.Handlers.ClientOperations.Request
{
    public class GetExchangeCourseRequest : IRequest<IEnumerable<GetExchangeCourseResult>?>
    {
        public DateTime? Date { get; set; }
    }
}
