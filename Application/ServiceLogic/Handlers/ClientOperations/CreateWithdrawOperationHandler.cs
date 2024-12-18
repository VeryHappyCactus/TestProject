﻿using AutoMapper;

using Common.Enums.Errors;
using Common.Queue.Message;
using Common.Queue.Message.ClientOperation.Request;

using ServiceLogic.Exceptions;
using ServiceLogic.Handlers.ClientOperations.Request;
using ServiceLogic.Handlers.ClientOperations.Result;
using ServiceLogic.Managers;

namespace ServiceLogic.Handlers.ClientOperations
{
    public class CreateWithdrawOperationHandler : BaseRequestHandler<CreateClientWithdrawOperationRequest, CreateClientWithdrawOperationResult, ClientWithdrawOperationRequestMessage>
    {
        public CreateWithdrawOperationHandler(IQueueManager queueManager, IMapper mapper)
            : base(queueManager, mapper) 
        {
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
                        _handlerStatus = MessageEventHandlerStatuses.Error;
                        throw new HandlerException(nameof(CommonErrorTypes), (int)CommonErrorTypes.BadRequest, "Result message body is empty", HandlerErrorTypes.RequestError);
                    }
                    else if (context.Body is ErrorMessage errorMessage)
                    {
                        _handlerStatus = MessageEventHandlerStatuses.Error;
                        _exception = GetMessageException(errorMessage.ErrorCode);
                    }
                    else
                    {
                        _handlerStatus = MessageEventHandlerStatuses.Success;
                        _result = _mapper.Map<CreateClientWithdrawOperationResult>(context.Body);
                    }
                }
            }
            finally
            {
                _eventWaitHandle.Set();
            }
        }

        private Exception GetMessageException(int errorCode)
        {

            (string errorCclass, string message, HandlerErrorTypes errorType) = errorCode switch
            {
                (int)CommonErrorTypes.Internal => 
                (
                    nameof(CommonErrorTypes), 
                    "Internal error", 
                    HandlerErrorTypes.RequestError
                ),
                (int)ClientWithdrawOperationErrorTypes.AmountError => 
                (
                    nameof(ClientWithdrawOperationErrorTypes), 
                    "Amount error, range is wrong", 
                    HandlerErrorTypes.RequestError
                ),
                (int)ClientWithdrawOperationErrorTypes.ClientMissing => 
                (
                    nameof(ClientWithdrawOperationErrorTypes), 
                    "Client is missing", 
                    HandlerErrorTypes.RequestError
                ),
                (int)ClientWithdrawOperationErrorTypes.ExchangeMissing => 
                (
                    nameof(ClientWithdrawOperationErrorTypes), 
                    "Exchange rate is missing", 
                    HandlerErrorTypes.RequestError
                ),
                (int)ClientWithdrawOperationErrorTypes.AddressError => 
                (
                    nameof(ClientWithdrawOperationErrorTypes), 
                    "Wrong address",
                    HandlerErrorTypes.RequestError
                ),
                _ => 
                (
                    nameof(CommonErrorTypes), 
                    "Unknown error code", 
                    HandlerErrorTypes.RequestError
                )
            };

           return new HandlerException(errorCclass, errorCode, message, errorType);
        }
    }
}
