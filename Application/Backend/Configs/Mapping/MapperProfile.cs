using AutoMapper;

using CommonCORequest = Common.Queue.Message.ClientOperation.Request;
using CommonCOResult = Common.Queue.Message.ClientOperation.Result;

using DalCORequest = DAL.Enteties.ClientOperations.Request;
using DalCOResult = DAL.Enteties.ClientOperations.Result;

namespace Backend.Configs.Mapping
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            //======Requests==========================================================================
            this.CreateMap<CommonCORequest.ClientDepartmentAddressMessage, DalCORequest.ClientDepartmentAddressRequest>()
                .ForMember(dest => dest.street_name, opt => opt.MapFrom(source => source.StreetName))
                .ForMember(dest => dest.building_number, opt => opt.MapFrom(source => source.BuildingNumber));

            this.CreateMap<CommonCORequest.ClientWithdrawOperationRequestMessage, DalCORequest.ClientWithdrawOperationRequest>()
                .ForMember(dest => dest.client_id, opt => opt.MapFrom(source => source.ClientId))
                .ForMember(dest => dest.amount, opt => opt.MapFrom(source => source.Amount))
                .ForMember(dest => dest.currency, opt => opt.MapFrom(source => source.CurrencyISOName))
                .ForMember(dest => dest.department_address, opt => opt.MapFrom(source => source.DepartmentAddress));

            this.CreateMap<CommonCORequest.ClientOperationsRequestMessage, DalCORequest.ClientOperationsRequest>()
                .ForMember(dest => dest.client_id, opt => opt.MapFrom(source => source.ClientId))
                .ForMember(dest => dest.department_address, opt => opt.MapFrom(source => source.DepartmentAddress));

            //======Results==========================================================================
            this.CreateMap<CommonCOResult.CurrentExchangeCourseResultMessage, DalCOResult.CurrentExchangeCourseResult>().ReverseMap();
            this.CreateMap<CommonCOResult.ClientOperationResultMessage, DalCOResult.ClientOperationResult>().ReverseMap();
            this.CreateMap<DalCOResult.ClientWithdrawOperationResult, CommonCOResult.ClientWithdrawOperationResultMessage>()
                .ForMember(dest => dest.ClientOperationId, opt => opt.MapFrom(source => source.ClientOperationId));
        }
    }
}
