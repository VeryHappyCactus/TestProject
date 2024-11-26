using AutoMapper;

using ServiceModels = Service.Models;
using ServiceCORequest = Service.Models.ClientOperations.Request;
using ServiceCOResult = Service.Models.ClientOperations.Result;

using HandlerModels = ServiceLogic.Handlers;
using HandlerCORequest = ServiceLogic.Handlers.ClientOperations.Request;
using HandlerCOResult = ServiceLogic.Handlers.ClientOperations.Result;

using HandlerCommonEnums = ServiceLogic.Handlers.Enums;
using HandlerCommonRequest = ServiceLogic.Handlers.CommonModels.Request;
using ServiceLogic.Handlers.ClientOperations.Request;

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
            this.CreateMap<ServiceCORequest.ExchangeCourseRequest, HandlerCORequest.GetExchangeCourseRequest>().ReverseMap();


            this.CreateMap<ServiceCORequest.ClientOperationRequest, HandlerCommonRequest.DefaultRequest>()
                .ConstructUsing((src, context) => new HandlerCommonRequest.DefaultRequest
                (
                    context.Mapper.Map<HandlerCORequest.GetClientOperationRequest>(src),
                    typeof(HandlerCORequest.GetClientOperationRequest),
                    typeof(HandlerCOResult.GetClientOperationResult),
                    HandlerCommonEnums.CommonClientOperationTypes.GetClientOperation.ToString())
                );

            this.CreateMap<ServiceCORequest.ClientOperationsRequest, HandlerCommonRequest.DefaultRequest>()
                .ConstructUsing((src, context) => new HandlerCommonRequest.DefaultRequest
                (
                    context.Mapper.Map<HandlerCORequest.GetClientOperationsRequest>(src), 
                    typeof(HandlerCORequest.GetClientOperationsRequest), 
                    typeof(IEnumerable<HandlerCOResult.GetClientOperationResult>), 
                    HandlerCommonEnums.CommonClientOperationTypes.GetClientOperations.ToString())
                );
                
            this.CreateMap<ServiceCORequest.ExchangeCourseRequest, HandlerCommonRequest.DefaultRequest>()
                .ConstructUsing((src, context) => new HandlerCommonRequest.DefaultRequest
                (
                    context.Mapper.Map<HandlerCORequest.GetExchangeCourseRequest>(src),
                    typeof(HandlerCORequest.GetExchangeCourseRequest),
                    typeof(IEnumerable<HandlerCOResult.GetExchangeCourseResult>),
                    HandlerCommonEnums.CommonClientOperationTypes.GetExchangeCourse.ToString())
                );

            //======Results==========================================================================
            this.CreateMap<ServiceCOResult.ExchangeCourseResult, HandlerCOResult.GetExchangeCourseResult>().ReverseMap();
            this.CreateMap<ServiceCOResult.ClientWithdrawOperationResult, HandlerCOResult.CreateClientWithdrawOperationResult>().ReverseMap();
            this.CreateMap<ServiceCOResult.ClientOperationResult, HandlerCOResult.GetClientOperationResult>().ReverseMap();

            //======Other==========================================================================

            this.CreateMap(typeof(ServiceModels.ResultContext<>), typeof(HandlerModels.HandlerResultContext<>)).ReverseMap();
        }
    }
}
