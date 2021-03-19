using AutoMapper;
using SmartCoop.Core.Coop;

namespace SmartCoop.Infrastructure.Mappings
{
    public class CoopProfile : Profile
    {
        public CoopProfile()
        {
            CreateMap<ICoop, ICoop>()
                .ConstructUsing(c => new Coop.Coop());
        }
    }
}