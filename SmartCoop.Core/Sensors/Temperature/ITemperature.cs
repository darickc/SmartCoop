using SmartCoop.Core.Devices;

namespace SmartCoop.Core.Sensors.Temperature
{
    public interface ITemperature : IDevice
    {
        double Temp { get; }
        IOneWire OneWireDevice { get; set; }
        int ReadFrequency { get; set; }
        TempType UnitType { get; set; }

    }
}