using AutoMapper;
using NexusEcom.DataAccess.DataTransferObjects;
using NexusEcom.DataAccess.Entities;

namespace NexusEcom.DataAccess.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Leave, LeaveResponseDto>()
                .ForMember(dest => dest.TotalDays,
                    opt => opt.MapFrom(src => (src.EndDate - src.StartDate).Days + 1)) // +1 to include start and end date
                .ReverseMap();

            CreateMap<CreateLeaveDto, Leave>();
            CreateMap<LeaveBalance, LeaveBalanceDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
