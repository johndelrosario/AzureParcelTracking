using AutoMapper;
using AzureParcelTracking.Application.Models;
using AzureParcelTracking.Commands.Models;

namespace AzureParcelTracking.Application
{
    internal class MapperProfile : Profile
    {
        public MapperProfile()
        {
            AllowNullCollections = true;

            CreateMap<NewConsignment, ConsignmentRecord>();
            CreateMap<ConsignmentRecord, Consignment>();
            CreateMap<Address, AddressRecord>().ReverseMap();
            CreateMap<NewTracking, TrackingRecord>();
            CreateMap<TrackingRecord, Tracking>();
        }
    }
}