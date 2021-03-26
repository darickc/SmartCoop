using System;
using System.Threading.Tasks;
using SmartCoop.Core.Devices;

namespace SmartCoop.Core.Services
{
    public interface IMessageService : IDisposable
    {
        event EventHandler OnMessage;
        Task Connect();
        void SendMessage(string topic, string payload, IDevice device = null);
    }
}