using System;
using System.Collections.Generic;
using SmartCoop.Core.Devices;
using SmartCoop.Core.Sensors;

namespace SmartCoop.Core.Coop
{
    public interface ICoop : IDisposable
    {
        List<IDevice> Devices { get; }
        void Initialize();
    }
}