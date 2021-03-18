using SmartCoop.Core.Sensors;

namespace SmartCoop.Core.Devices
{
    public interface ISwitch : IDevice
    {
        bool On { get; }
        int Pin { get; set; }
        
        void TurnOn();
        void TurnOff();
    }
}