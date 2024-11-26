using AutoMapper;

using ServiceLogic.Configs.Mapping.Converters;

using CommonMessage = Common.Queue.Message;
using CommonCORequest = Common.Queue.Message.ClientOperation.Request;
using CommonCOResult = Common.Queue.Message.ClientOperation.Result;

using HandlerCORequest = ServiceLogic.Handlers.ClientOperations.Request;
using HandlerCOResult = ServiceLogic.Handlers.ClientOperations.Result;

//using HandlerCommonRequest = ServiceLogic.Handlers.CommonModels.Request;
//using HandlerCommonResult = ServiceLogic.Handlers.CommonModels.Result;

namespace ServiceLogic.Configs.Mapping
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            //======Requests==========================================================================
            CreateMap<CommonCORequest.ClientDepartmentAddressMessage, HandlerCORequest.ClientDepartmentAddressRequest>().ReverseMap();
            CreateMap<CommonCORequest.ClientWithdrawOperationRequestMessage, HandlerCORequest.CreateClientWithdrawOperationRequest>().ReverseMap();
            CreateMap<CommonCORequest.ClientOperationRequestMessage, HandlerCORequest.GetClientOperationRequest>().ReverseMap();
            CreateMap<CommonCORequest.ClientOperationsRequestMessage, HandlerCORequest.GetClientOperationsRequest>().ReverseMap();

            //======Results==========================================================================
            CreateMap<CommonCOResult.ExchangeCourseResultMessage, HandlerCOResult.GetExchangeCourseResult>().ReverseMap();
            CreateMap<CommonCOResult.ClientWithdrawOperationResultMessage, HandlerCOResult.CreateClientWithdrawOperationResult>().ReverseMap();
            CreateMap<CommonCOResult.ClientOperationResultMessage, HandlerCOResult.GetClientOperationResult>().ReverseMap();

            CreateMap<CommonMessage.MessageCollection, IEnumerable<HandlerCOResult.GetClientOperationResult>>()
                .ConvertUsing<MessageCollectionConverter<IEnumerable<HandlerCOResult.GetClientOperationResult>>>();

        }
    }

 
}
