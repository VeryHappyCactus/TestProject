using AutoMapper;

using Common.Queue.Message;
using DAL;

namespace Backend.Jobs
{
    public abstract class BaseJob<TIn> : IJob
        where TIn : BaseMessage
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;

        public BaseJob(IUnitOfWork unitOfWork, IMapper mapper)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException(nameof(unitOfWork));

            if (mapper == null)
                throw new ArgumentNullException(nameof(mapper));

            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        protected abstract Task<BaseMessage> ExecuteInnerAsync(TIn message);

        public async Task<BaseMessage> ExecuteAsync<T>(T message)
            where T : BaseMessage
        {
            TIn msg = (message as TIn)!;
            return await ExecuteInnerAsync(msg);
        }
    }
}
