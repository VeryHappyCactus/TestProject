﻿using AutoMapper;
using MediatR;

using Common.Enums.Errors;
using Common.Queue.Consumer;
using Common.Queue.Message;
using Common.Queue.Producer;

using MediatorHandlers.Exceptions;
using MediatorHandlers.Managers;

namespace MediatorHandlers.Handlers
{
    public abstract class BaseRequestHandler<THandlerRequest, THandlerResponse, TRequestMessage> : BaseHandler<THandlerRequest, THandlerResponse>
        where THandlerRequest : IRequest<THandlerResponse>
        where THandlerResponse : class
        where TRequestMessage: BaseMessage, new()
    {
        protected readonly EventWaitHandle _eventWaitHandle;
        protected readonly string _key = Guid.NewGuid().ToString();
        protected readonly object _lock = new object();
        protected readonly TimeSpan _timeout;

        protected THandlerResponse _result;

        public BaseRequestHandler(IQueueManager queueManager, IMapper mapper)
            : base(queueManager, mapper)
        {
            _eventWaitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);
            _timeout = TimeSpan.FromSeconds(30_000);
        }

        public override async Task<THandlerResponse> Handle(THandlerRequest request, CancellationToken cancellationToken)
        {
            IQueueConsumer? consumer = null;
            IQueueProducer? producer = null;

            try
            {
                consumer = await _queueManager.GetOrCreateQueueConsumer();
                producer = await _queueManager.GetOrCreateQueueProducer();

                consumer.TryAddConsumer(_key, _eventHandler);

                await producer.PublishMessageAsync(new MessageContext()
                {
                    PublisherId = _key,
                    SessionId = _key,
                    RequestId = _key,
                    Body = _mapper.Map<TRequestMessage>(request)
                });

                _eventWaitHandle.WaitOne(_timeout);

                lock (_lock)
                {
                    if (_result == null)
                    {
                        throw new HandlerException(nameof(CommonErrorTypes), (int)CommonErrorTypes.Internal, "Timeout exception, result did not get in time", HandlerErrorTypes.Internal);
                    }
                }

            }
            finally
            {
                if (consumer != null)
                    consumer.TryRemoveConsumer(_key);
            }

            return _result;
        }
    }
}