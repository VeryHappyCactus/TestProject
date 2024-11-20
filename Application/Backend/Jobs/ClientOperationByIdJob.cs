using AutoMapper;

using Common.Queue.Message;
using Common.Queue.Message.ClientOperation.Request;
using Common.Queue.Message.ClientOperation.Result;

using DAL;
using DAL.Enteties.ClientOperations.Result;

namespace Backend.Jobs
{
    public class ClientOperationByIdJob : BaseJob<ClientOperationRequestMessage>
    {
        public ClientOperationByIdJob(IUnitOfWork unitOfWork, IMapper mapper)
           : base(unitOfWork, mapper)
        {
        }
        
        protected override async Task<BaseMessage> ExecuteInnerAsync(ClientOperationRequestMessage message)
        {
            ClientOperationResult? result = await _unitOfWork.ClientOperationRepository.GetClientOperationById(message.ClientOperationId);
            return _mapper.Map<ClientOperationResultMessage>(result);
        }
    }
}
