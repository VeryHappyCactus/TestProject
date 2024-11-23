using AutoMapper;

using ServiceModels = Service.Models;
using ServiceCORequest = Service.Models.ClientOperations.Request;
using ServiceCOResult = Service.Models.ClientOperations.Result;

using HandlerModels = MediatorHandlers.Handlers;
using HandlerCORequest = MediatorHandlers.Handlers.ClientOperations.Request;
using HandlerCOResult = MediatorHandlers.Handlers.ClientOperations.Result;

namespace Service.Configs.Mapping
{

    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            //======Requests==========================================================================
            this.CreateMap<ServiceCORequest.ClientDepartmentAddressRequest, HandlerCORequest.ClientDepartmentAddressRequest>().ReverseMap();
            this.CreateMap<ServiceCORequest.ClientWithdrawOperationRequest, HandlerCORequest.CreateClientWithdrawOperationRequest>().ReverseMap();
            this.CreateMap<ServiceCORequest.ClientOperationRequest, HandlerCORequest.GetClientOperationRequest>().ReverseMap();
            this.CreateMap<ServiceCORequest.ClientOperationsRequest, HandlerCORequest.GetClientOperationsRequest>().ReverseMap();

            //======Results==========================================================================
            this.CreateMap<ServiceCOResult.CurrentExchangeCourseResult, HandlerCOResult.ExchangeCourseResult>().ReverseMap();
            this.CreateMap<ServiceCOResult.ClientWithdrawOperationResult, HandlerCOResult.CreateClientWithdrawOperationResult>().ReverseMap();
            this.CreateMap<ServiceCOResult.ClientOperationResult, HandlerCOResult.GetClientOperationResult>().ReverseMap();

            //======Other==========================================================================

            this.CreateMap(typeof(ServiceModels.ResultContext<>), typeof(HandlerModels.HandlerResultContext<>)).ReverseMap();
        }
    }
}
