using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iot.Device.OneWire;
using SmartCoop.Core.Sensors.Temperature;

namespace SmartCoop.Infrastructure.Sensors
{
    public class OneWire : IOneWire
    {
        private static readonly List<IOneWire> Devices = new();
        private static readonly SemaphoreSlim Lock = new(1, 1);

        public string BusId { get; set; }
        public string DevId { get; set; }

        public OneWire() {}

        public OneWire(string busId, string devId)
        {
            BusId = busId;
            DevId = devId;
        }

        public static async Task<List<IOneWire>> GetAvailableOneWires()
        {
            await Lock.WaitAsync();
            if (Devices.Any())
            {
                Lock.Release();
                return Devices;
            }
            Devices.Clear();
            try
            {
                foreach (string busId in OneWireBus.EnumerateBusIds())
                {
                    OneWireBus bus = new(busId);
                    await bus.ScanForDeviceChangesAsync();
                    foreach (string devId in bus.EnumerateDeviceIds())
                    {
                        OneWireDevice dev = new(busId, devId);
                        if (OneWireThermometerDevice.IsCompatible(busId, devId))
                        {
                            Devices.Add(new OneWire(busId, devId));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Devices.Add(new OneWire("Failed", "Failed"));
            }
            Lock.Release();
            return Devices;
        }
    }
}