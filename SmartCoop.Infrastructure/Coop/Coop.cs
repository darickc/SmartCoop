using System.Collections.Generic;
using SmartCoop.Core.Coop;
using SmartCoop.Core.Devices;

namespace SmartCoop.Infrastructure.Coop
{
    public class Coop : ICoop
    {
        public List<IDevice> Devices { get; } = new();
        public void Initialize()
        {
            foreach (var device in Devices)
            {
                device.Initialize();
            }
        }

        public void Dispose()
        {
            foreach (var device in Devices)
            {
                device.Dispose();
            }
        }
    }
}