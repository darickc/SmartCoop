using AutoMapper;
using SmartCoop.Infrastructure.Devices;
using SmartCoop.Infrastructure.Sensors;

namespace SmartCoop.Infrastructure.Mappings
{
    public class DeviceConfiguration : Profile
    {
        public DeviceConfiguration()
        {
            CreateMap<Door, Door>();
            CreateMap<Switch, Switch>();
            CreateMap<Level, Level>();
            CreateMap<PhotoCell, PhotoCell>();
            CreateMap<Temperature, Temperature>();
        }
    }
}