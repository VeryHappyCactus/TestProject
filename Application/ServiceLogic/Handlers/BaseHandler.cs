using AutoMapper;
using MediatR;

using Common.Queue.Consumer.Delegates;
using Common.Queue.Message;

using ServiceLogic.Managers;

namespace ServiceLogic.Handlers
{
    public abstract class BaseHandler<THandlerRequest, THandlerResponse> : IRequestHandler<THandlerRequest, THandlerResponse>
        where THandlerRequest : IRequest<THandlerResponse>
        where THandlerResponse : class
    {
        protected readonly IQueueManager _queueManager;
        protected readonly IMapper _mapper;
       
        protected readonly AsyncMessageEventHadler _eventHandler;

        public BaseHandler(IQueueManager queueManager, IMapper mapper)
        {
            if (queueManager == null)
                throw new ArgumentNullException(nameof(queueManager));
            
            if (mapper == null)
                throw new ArgumentNullException(nameof(mapper));

            _queueManager = queueManager;
            _mapper = mapper;
            _eventHandler = OnAsyncMessageEventHadler;

        }

        public abstract Task<THandlerResponse> Handle(THandlerRequest request, CancellationToken cancellationToken);
        public abstract Task OnAsyncMessageEventHadler(MessageContext context);


      
    }
}
