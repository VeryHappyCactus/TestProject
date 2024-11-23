using AutoMapper;

using Common.Enums.Errors;
using Common.Queue.Message;
using Common.Queue.Message.ClientOperation.Request;
using Common.Queue.Message.ClientOperation.Result;

using DAL;
using DAL.Enteties.ClientOperations.Request;
using DAL.Enteties.ClientOperations.Result;

namespace Backend.Jobs
{
    public class ClientWithdrawOperationJob : BaseJob<ClientWithdrawOperationRequestMessage>
    {
        public ClientWithdrawOperationJob(IUnitOfWork unitOfWork, IMapper mapper) 
            : base(unitOfWork, mapper)
        { 
        }

        protected override async Task<BaseMessage> ExecuteInnerAsync(ClientWithdrawOperationRequestMessage message)
        {
            ClientWithdrawOperationRequest request = _mapper.Map<ClientWithdrawOperationRequest>(message);

            if (Validate(request, out ErrorMessage? erroMessage) && erroMessage != null)
                return erroMessage;

            ClientWithdrawOperationResult result = await _unitOfWork.ClientOperationRepository.CreateClientOperation(request);

            if (result.ErrorCode.HasValue)
                return GetError(result.ErrorCode.Value);

            return _mapper.Map<ClientWithdrawOperationResultMessage>(result);
        }

        private bool Validate(ClientWithdrawOperationRequest request, out ErrorMessage? erroMessage)
        {
            erroMessage = null;

            const int amountMin = 100;
            const int amountMax = 100_000;
            
            if (request.amount < amountMin || request.amount > amountMax)
            {
                erroMessage = GetError((int)ClientWithdrawOperationErrorTypes.AmountError);
                return true;
            }
            else if (request.department_address == null 
                || string.IsNullOrEmpty(request.department_address.street_name)
                || string.IsNullOrEmpty(request.department_address.building_number))
            {
                erroMessage = GetError((int)ClientWithdrawOperationErrorTypes.AddressError);
                return true;
            }
            
            return false;
        }

        private ErrorMessage GetError(int errorCode)
        {
            return new ErrorMessage()
            {
                ErrorCode = errorCode,
                MessageType = nameof(ClientWithdrawOperationRequestMessage)
            };
        }
    }
}
