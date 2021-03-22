using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using SmartCoop.Core.Devices;
using SmartCoop.Core.Services;

namespace SmartCoop.Core.Coop
{
    public interface ICoop : IDisposable, INotifyPropertyChanged
    {
        List<IDevice> Devices { get; set; }
        Task Initialize(IMessageService messageService = null);
        Task Save();
        void Load();
        List<IDevice> CopyDevices();
    }
}