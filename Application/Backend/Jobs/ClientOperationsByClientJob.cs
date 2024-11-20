using AutoMapper;

using Common.Queue.Message;
using Common.Queue.Message.ClientOperation.Request;
using Common.Queue.Message.ClientOperation.Result;

using DAL;
using DAL.Enteties.ClientOperations.Request;
using DAL.Enteties.ClientOperations.Result;

namespace Backend.Jobs
{
    public class ClientOperationsByClientJob : BaseJob<ClientOperationsRequestMessage>
    {
        public ClientOperationsByClientJob(IUnitOfWork unitOfWork, IMapper mapper)
           : base(unitOfWork, mapper)
        {
        }

        protected override async Task<BaseMessage> ExecuteInnerAsync(ClientOperationsRequestMessage message)
        {
            ClientOperationsRequest request = _mapper.Map<ClientOperationsRequest>(message);

            IEnumerable<ClientOperationResult>? result = await _unitOfWork.ClientOperationRepository.GetClientOperations(request);

            return new MessageCollection()
            {
                BaseMessages = _mapper.Map<ClientOperationResultMessage[]>(result)
            };
        }
    }
}
