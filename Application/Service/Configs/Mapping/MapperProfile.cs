using AutoMapper;

using ServiceModels = Service.Models;
using ServiceCORequest = Service.Models.ClientOperations.Request;
using ServiceCOResult = Service.Models.ClientOperations.Result;

using HandlerModels = ServiceLogic.Handlers;
using HandlerCORequest = ServiceLogic.Handlers.ClientOperations.Request;
using HandlerCOResult = ServiceLogic.Handlers.ClientOperations.Result;

using HandlerCommonEnums = ServiceLogic.Handlers.Enums;
using HandlerCommonRequest = ServiceLogic.Handlers.CommonModels.Request;

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

            this.CreateMap<HandlerCORequest.GetClientOperationRequest, HandlerCommonRequest.DefaultRequest>()
                .ConvertUsing(x => new HandlerCommonRequest.DefaultRequest
                (x, x.GetType(), typeof(ServiceCOResult.ClientOperationResult), HandlerCommonEnums.CommonClientOperationTypes.GetClientOperation.ToString()));

            this.CreateMap<HandlerCORequest.GetClientOperationsRequest, HandlerCommonRequest.DefaultRequest>()
                .ConvertUsing(x => new HandlerCommonRequest.DefaultRequest
                (x, x.GetType(), typeof(ServiceCOResult.ClientOperationResult), HandlerCommonEnums.CommonClientOperationTypes.GetClientOperations.ToString()));

            this.CreateMap<HandlerCORequest.GetExchangeCourseRequest, HandlerCommonRequest.DefaultRequest>()
                .ConvertUsing(x => new HandlerCommonRequest.DefaultRequest
                (x, x.GetType(), typeof(ServiceCOResult.ExchangeCourseResult), HandlerCommonEnums.CommonClientOperationTypes.GetExchangeCourse.ToString()));

            //======Results==========================================================================
            this.CreateMap<ServiceCOResult.ExchangeCourseResult, HandlerCOResult.GetExchangeCourseResult>().ReverseMap();
            this.CreateMap<ServiceCOResult.ClientWithdrawOperationResult, HandlerCOResult.CreateClientWithdrawOperationResult>().ReverseMap();
            this.CreateMap<ServiceCOResult.ClientOperationResult, HandlerCOResult.GetClientOperationResult>().ReverseMap();

            //======Other==========================================================================

            this.CreateMap(typeof(ServiceModels.ResultContext<>), typeof(HandlerModels.HandlerResultContext<>)).ReverseMap();
        }
    }
}
