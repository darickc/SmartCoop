using System;

namespace SmartCoop.Core.Devices
{
    public interface IDoor : IDevice
    {
        DoorState State { get; set; }
        int OpenPinNumber { get; set; }
        int ClosedPinNumber { get; set; }
        int Pin1 { get; set; }
        int Pin2 { get; set; }
        int Timeout { get; set; }
        void Open();
        void Close();
        void Toggle();
    }
}