using System;
using System.Threading.Tasks;

namespace SmartCoop.Core.Services
{
    public interface IMessageService : IDisposable
    {
        Task Connect();
        void SendMessage(string topic, string payload);
    }
}