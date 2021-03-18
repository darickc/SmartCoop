using SmartCoop.Core.Devices;

namespace SmartCoop.Core.Sensors.Temperature
{
    public interface ITemperature : IDevice
    {
        double Temp { get; }
        string BusId { get; set; }
        string DevId { get; set; }
        int ReadFrequency { get; set; }
        TempType UnitType { get; set; }

    }
}