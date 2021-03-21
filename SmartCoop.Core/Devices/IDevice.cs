using System;
using System.ComponentModel;
using System.Threading.Tasks;
using SmartCoop.Core.Services;

namespace SmartCoop.Core.Devices
{
    public interface IDevice : IDisposable, INotifyPropertyChanged
    {
        string Name { get; set; }
        Task Initialize(IMessageService messageService);
        void HandleMessage(string message, string payload);
    }
}