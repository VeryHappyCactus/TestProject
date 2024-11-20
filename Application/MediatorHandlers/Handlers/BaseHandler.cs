using AutoMapper;
using MediatR;

using Common.Queue.Consumer.Delegates;
using Common.Queue.Message;

using MediatorHandlers.Managers;

namespace MediatorHandlers.Handlers
{
    public abstract class BaseHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        protected readonly IQueueManager _queueManager;
        protected readonly IMapper _mapper;
        protected readonly EventWaitHandle _eventWaitHandle;
        protected readonly TimeSpan _timeout;
        protected readonly AsyncMessageEventHadler _eventHandler;

        public BaseHandler(IQueueManager queueManager, IMapper mapper)
        {
            if (queueManager == null)
                throw new ArgumentNullException(nameof(queueManager));
            
            if (mapper == null)
                throw new ArgumentNullException(nameof(mapper));

            _queueManager = queueManager;
            _mapper = mapper;
            _eventWaitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);
            _timeout = TimeSpan.FromSeconds(30_000);
            _eventHandler = OnAsyncMessageEventHadler;
        
        }
        public abstract Task OnAsyncMessageEventHadler(MessageContext context);
        public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
    }
}
