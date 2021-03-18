using SmartCoop.Core.Devices;

namespace SmartCoop.Core.Sensors
{
    public interface IPhotoCell : IDevice
    {
        bool On { get; }
    }
}