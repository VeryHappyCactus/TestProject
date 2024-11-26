using AutoMapper;

using Common.Queue.Message.ClientOperation.Request;
using Common.Queue.Message.ClientOperation.Result;
using Common.Queue.Message;

using DAL;

namespace BackendLogic.Jobs
{
    public class GetExchangeCourseJob : BaseJob<ExchangeCourseRequestMessage>
    {
        public GetExchangeCourseJob(IUnitOfWork unitOfWork, IMapper mapper)
           : base(unitOfWork, mapper)
        {
        }

        protected override async Task<BaseMessage> ExecuteInnerAsync(ExchangeCourseRequestMessage message)
        {
            object? result = null;

            if (message.Date.HasValue)
                result = await _unitOfWork.ClientOperationRepository.GetExchangeCourseByDate(message.Date.Value);
            else
                result = await _unitOfWork.ClientOperationRepository.GetExchangeCourses();

            return new MessageCollection()
            {
                BaseMessages = _mapper.Map<ExchangeCourseResultMessage[]>(result)
            };
        }
    }
}

