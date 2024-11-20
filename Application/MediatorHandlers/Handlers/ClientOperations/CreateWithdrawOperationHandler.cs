using AutoMapper;

using Common.Enums.Errors;
using Common.Queue.Consumer;
using Common.Queue.Message;
using Common.Queue.Message.ClientOperation.Request;
using Common.Queue.Producer;

using MediatorHandlers.Handlers.ClientOperations.Request;
using MediatorHandlers.Handlers.ClientOperations.Result;
using MediatorHandlers.Managers;

namespace MediatorHandlers.Handlers.ClientOperations
{
    public class CreateWithdrawOperationHandler :  BaseHandler<CreateClientWithdrawOperationRequest, HandlerResultContext<CreateClientWithdrawOperationResult>>
    {
        private readonly string _key = Guid.NewGuid().ToString();
        private readonly object _lock = new object();

        private HandlerResultContext<CreateClientWithdrawOperationResult>? _result;

        public CreateWithdrawOperationHandler(IQueueManager queueManager, IMapper mapper)
            : base(queueManager, mapper) 
        {
        }

        public override async Task<HandlerResultContext<CreateClientWithdrawOperationResult>> Handle(CreateClientWithdrawOperationRequest request, CancellationToken cancellationToken)
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
                    Body = _mapper.Map<ClientWithdrawOperationRequestMessage>(request)
                });

                _eventWaitHandle.WaitOne(_timeout);

                lock (_lock)
                {
                    if (_result == null)
                    {
                        return new HandlerResultContext<CreateClientWithdrawOperationResult>()
                        {
                            Error = new HandlerErrorContext()
                            {
                                Message = "Message did not delivery",
                                ErrorCode = (int)CommonErrorTypes.Internal
                            }
                        };
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

        public override async Task OnAsyncMessageEventHadler(MessageContext context)
        {

            try
            {
                lock (_lock)
                {
                    if (_result != null)
                        return;

                    if (context.Body == null)
                    {
                        _result = new HandlerResultContext<CreateClientWithdrawOperationResult>()
                        {
                            Error = GetErrorContext((int)CommonErrorTypes.Internal)
                        };

                        return;
                    }

                    if (context.Body is ErrorMessage errorMessage)
                    {
                        _result = new HandlerResultContext<CreateClientWithdrawOperationResult>()
                        {
                            Error = GetErrorContext(errorMessage.ErrorCode)
                        };
                    }
                    else
                    {
                        _result = new HandlerResultContext<CreateClientWithdrawOperationResult>()
                        {
                            Value = _mapper.Map<CreateClientWithdrawOperationResult>(context.Body)
                        };
                    }
                }
            }
            finally
            {
                _eventWaitHandle.Set();
            }
        }

        private bool Validate(CreateClientWithdrawOperationRequest request, out HandlerErrorContext? errorContext)
        {
            errorContext = null;

            const int amountMin = 100;
            const int amountMax = 100_000;

            if (request.Amount < amountMin && request.Amount > amountMax)
            {
                errorContext = GetErrorContext((int)ClientWithdrawOperationErrorTypes.AmountError);
                return true;
            }
            else if (request.DepartmentAddress == null
                || string.IsNullOrEmpty(request.DepartmentAddress.StreetName)
                || string.IsNullOrEmpty(request.DepartmentAddress.BuildingNumber))
            {
                errorContext = GetErrorContext((int)ClientWithdrawOperationErrorTypes.AddressError);
                return true;
            }

            return false;
        }

        private HandlerErrorContext GetErrorContext(int errorCode)
        {
            return new HandlerErrorContext()
            {
                ErrorCode = errorCode,
                Message = errorCode switch
                {
                    (int)CommonErrorTypes.Internal => "Internal error",
                    (int)ClientWithdrawOperationErrorTypes.AmountError => "Amount error, range is wrong",
                    (int)ClientWithdrawOperationErrorTypes.ClientMissing => "Client is missing",
                    (int)ClientWithdrawOperationErrorTypes.ExchangeMissing => "Exchange rate is missing",
                    (int)ClientWithdrawOperationErrorTypes.AddressError => "Wrong address",
                    _ => null
                }
            };
        }
    }
}
