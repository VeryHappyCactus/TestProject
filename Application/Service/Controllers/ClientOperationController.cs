using Microsoft.AspNetCore.Mvc;

using AutoMapper;
using MediatR;

using HandlerCommon = ServiceLogic.Handlers.CommonModels;
using HandlerCORequest = ServiceLogic.Handlers.ClientOperations.Request;
using HandlerCOResult = ServiceLogic.Handlers.ClientOperations.Result;

using ServiceModels = Service.Models;
using ServiceCORequest = Service.Models.ClientOperations.Request;
using ServiceCOResult = Service.Models.ClientOperations.Result;

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
        public async Task<ServiceCOResult.ClientWithdrawOperationResult> CreateClientWithdrawOperation(ServiceCORequest.ClientWithdrawOperationRequest request)
        {
            HandlerCORequest.CreateClientWithdrawOperationRequest createOperation = _mapper.Map<HandlerCORequest.CreateClientWithdrawOperationRequest>(request);
            HandlerCOResult.CreateClientWithdrawOperationResult? result = await _mediator.Send(createOperation);
            
            return _mapper.Map<ServiceCOResult.ClientWithdrawOperationResult>(result);
        }

        [HttpPost("GetClientOperation")]
        public async Task<ServiceCOResult.ClientOperationResult> GetClientOperation(ServiceCORequest.ClientOperationRequest request)
        {

            HandlerCommon.Request.DefaultRequest getOperation = _mapper.Map<HandlerCommon.Request.DefaultRequest>(request);
            object? result = await _mediator.Send(getOperation);

            return _mapper.Map<ServiceCOResult.ClientOperationResult>(result);
        }

        [HttpPost("GetClientOperations")]
        public async Task<IEnumerable<ServiceCOResult.ClientOperationResult>> GetClientOperations(ServiceCORequest.ClientOperationsRequest request)
        {

            HandlerCommon.Request.DefaultRequest getOperation = _mapper.Map<HandlerCommon.Request.DefaultRequest>(request);
            object? result = await _mediator.Send(getOperation);

            return _mapper.Map<IEnumerable<ServiceCOResult.ClientOperationResult>>(result);
        }

        [HttpPost("GetExchangeCourseByDate")]
        public async Task<IEnumerable<ServiceCOResult.ExchangeCourseResult>?> GetExchangeCourseByDate(ServiceCORequest.ExchangeCourseRequest request)
        {
            HandlerCommon.Request.DefaultRequest getOperation = _mapper.Map<HandlerCommon.Request.DefaultRequest>(request);
            object? result = await _mediator.Send(getOperation);

            return _mapper.Map<IEnumerable<ServiceCOResult.ExchangeCourseResult>>(result);
        }

        [HttpGet("GetExchangeCourses")]
        public async Task<IEnumerable<ServiceCOResult.ExchangeCourseResult>?> GetExchangeCourseByDate()
        {
            HandlerCommon.Request.DefaultRequest getOperation = _mapper.Map<HandlerCommon.Request.DefaultRequest>(new ServiceCORequest.ExchangeCourseRequest());
            object? result = await _mediator.Send(getOperation);

            return _mapper.Map<IEnumerable<ServiceCOResult.ExchangeCourseResult>>(result);
        }
    }
}
