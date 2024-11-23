using Microsoft.AspNetCore.Mvc;

using AutoMapper;
using MediatR;

using HandlerRequest = MediatorHandlers.Handlers.ClientOperations.Request;
using HandlerResult = MediatorHandlers.Handlers.ClientOperations.Result;

using ServiceModels = Service.Models;
using ServiceRequest = Service.Models.ClientOperations.Request;
using ServiceResult = Service.Models.ClientOperations.Result;


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
        public async Task<ServiceResult.ClientWithdrawOperationResult> CreateClientWithdrawOperation(ServiceRequest.ClientWithdrawOperationRequest request)
        {
            HandlerRequest.CreateClientWithdrawOperationRequest createOperation = _mapper.Map<HandlerRequest.CreateClientWithdrawOperationRequest>(request);
            HandlerResult.CreateClientWithdrawOperationResult? result = await _mediator.Send(createOperation);
            
            return _mapper.Map<ServiceResult.ClientWithdrawOperationResult>(result);
        }

        [HttpPost("GetClientOperation")]
        public async Task<ServiceResult.ClientOperationResult> GetClientOperation(ServiceRequest.ClientOperationRequest request)
        {

            HandlerRequest.GetClientOperationRequest getOperation = _mapper.Map<HandlerRequest.GetClientOperationRequest>(request);
            HandlerResult.GetClientOperationResult? result = await _mediator.Send(getOperation);

            return _mapper.Map<ServiceResult.ClientOperationResult>(result);
        }

        [HttpPost("GetClientOperations")]
        public async Task<IEnumerable<ServiceResult.ClientOperationResult>> GetClientOperations(ServiceRequest.ClientOperationsRequest request)
        {

            HandlerRequest.GetClientOperationsRequest getOperations = _mapper.Map<HandlerRequest.GetClientOperationsRequest>(request);
            IEnumerable<HandlerResult.GetClientOperationResult>? result = await _mediator.Send(getOperations);

            return _mapper.Map<IEnumerable<ServiceResult.ClientOperationResult>>(result);
        }

        [HttpPost("GetExchangeCourseByDate")]
        public async Task<IEnumerable<ServiceResult.ExchangeCourseResult>?> GetExchangeCourseByDate(ServiceRequest.ExchangeCourseRequest request)
        {

            HandlerRequest.GetExchangeCourseRequest getOperations = _mapper.Map<HandlerRequest.GetExchangeCourseRequest>(request);
            IEnumerable<HandlerResult.ExchangeCourseResult>? result = await _mediator.Send(getOperations);

            return _mapper.Map<IEnumerable<ServiceResult.ExchangeCourseResult>>(result);
        }

        [HttpGet("GetExchangeCourses")]
        public async Task<IEnumerable<ServiceResult.ExchangeCourseResult>?> GetExchangeCourseByDate()
        {

            HandlerRequest.GetExchangeCourseRequest getOperations = _mapper.Map<HandlerRequest.GetExchangeCourseRequest>(new HandlerRequest.GetExchangeCourseRequest());
            IEnumerable<HandlerResult.ExchangeCourseResult>? result = await _mediator.Send(getOperations);

            return _mapper.Map<IEnumerable<ServiceResult.ExchangeCourseResult>>(result);
        }

    }
}
