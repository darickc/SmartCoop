using System;
using System.ComponentModel;

namespace SmartCoop.Core.Devices
{
    public interface IDevice : IDisposable, INotifyPropertyChanged
    {
        string Name { get; set; }
        void Initialize();
    }
}