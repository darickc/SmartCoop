using SmartCoop.Core.Sensors;

namespace SmartCoop.Core.Devices
{
    public interface IDoor : IDevice
    {
        bool IsOpen { get; }
        bool IsClosed { get; }
        int OpenPinNumber { get; }
        int ClosedPinNumber { get; }
        void Open();
        void Close();
    }
}