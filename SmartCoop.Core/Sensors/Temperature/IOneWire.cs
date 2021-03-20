namespace SmartCoop.Core.Sensors.Temperature
{
    public interface IOneWire
    {
        string BusId { get; set; }
        string DevId { get; set; }
    }
}