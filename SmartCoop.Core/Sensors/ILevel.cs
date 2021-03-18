using SmartCoop.Core.Devices;

namespace SmartCoop.Core.Sensors
{
    public interface ILevel : IDevice
    {
        int Percent { get; }
        int TriggerPin { get; set; }
        int EchoPin { get; set; }
        int ReadFrequency { get; set; }

        double LowValue { get; }
        double HighValue { get; }

        void SetLow();
        void SetHigh();
    }
}