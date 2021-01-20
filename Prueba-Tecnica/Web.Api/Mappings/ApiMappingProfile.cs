using AutoMapper;
using Core.Models;

namespace Api.Mappings
{
    public class ApiMappingProfile : Profile
    {
        public ApiMappingProfile()
        {
            CreateMap<Models.Input.RoomModel, Room>();
            CreateMap<Room, Models.Output.RoomModel>();

            CreateMap<Models.Input.ReservationModel, Reservation>();
            CreateMap<Reservation, Models.Output.ReservationModel>();
        }
    }
}
