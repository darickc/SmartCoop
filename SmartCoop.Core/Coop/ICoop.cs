using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using SmartCoop.Core.Devices;

namespace SmartCoop.Core.Coop
{
    public interface ICoop : IDisposable, INotifyPropertyChanged
    {
        List<IDevice> Devices { get; set; }
        void Initialize();
        Task Save();
        void Load();
    }
}