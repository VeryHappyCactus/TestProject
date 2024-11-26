using AutoMapper;

using MediatorHandlers.Configs.Mapping.Converters;

using CommonMessage = Common.Queue.Message;
using CommonCORequest = Common.Queue.Message.ClientOperation.Request;
using CommonCOResult = Common.Queue.Message.ClientOperation.Result;

using HandlerCORequest = MediatorHandlers.Handlers.ClientOperations.Request;
using HandlerCOResult = MediatorHandlers.Handlers.ClientOperations.Result;

//using HandlerCommonRequest = MediatorHandlers.Handlers.CommonModels.Request;
//using HandlerCommonResult = MediatorHandlers.Handlers.CommonModels.Result;

namespace MediatorHandlers.Configs.Mapping
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
