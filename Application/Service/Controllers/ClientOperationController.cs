using Microsoft.AspNetCore.Mvc;

using AutoMapper;
using MediatR;

using MediatorHandlers.Handlers.ClientOperations.Request;
using MediatorHandlers.Handlers.ClientOperations.Result;

using Service.Models;
using Service.Models.ClientOperations.Request;
using Service.Models.ClientOperations.Result;

namespace Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
   
    public class ClientOperationController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public ClientOperationController(
            IMediator mediator,
            IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost("CreateClientWithdrawOperation")]
        public async Task<ResultContext<ClientWithdrawOperationResult>> CreateClientWithdrawOperation(ClientWithdrawOperationRequest request)
        {
            CreateClientWithdrawOperationRequest createOperation = _mapper.Map<CreateClientWithdrawOperationRequest>(request);
            CreateClientWithdrawOperationResult? result = await _mediator.Send(createOperation);
            
            return _mapper.Map<ResultContext<ClientWithdrawOperationResult>>(result);
        }

        [HttpPost("GetClientOperation")]
        public async Task<ResultContext<ClientOperationResult>> GetClientOperation(ClientOperationRequest request)
        {
            
            GetClientOperationRequest getOperation = _mapper.Map<GetClientOperationRequest>(request);
            GetClientOperationResult? result = await _mediator.Send(getOperation);

            return _mapper.Map<ResultContext<ClientOperationResult>>(result);
        }

        [HttpPost("GetClientOperations")]
        public async Task<ResultContext<IEnumerable<ClientOperationResult>>> GetClientOperations(ClientOperationsRequest request)
        {

            GetClientOperationsRequest getOperations = _mapper.Map<GetClientOperationsRequest>(request);
            IEnumerable<GetClientOperationResult>? result = await _mediator.Send(getOperations);

            return _mapper.Map<ResultContext<IEnumerable<ClientOperationResult>>>(result);
        }

    }
}
