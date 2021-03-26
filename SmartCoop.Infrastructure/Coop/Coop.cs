using System.Collections.Generic;
using System.ComponentModel;
using System.Device.Gpio;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SmartCoop.Core.Coop;
using SmartCoop.Core.Devices;
using SmartCoop.Core.Services;
using SmartCoop.Infrastructure.Annotations;

namespace SmartCoop.Infrastructure.Coop
{
    public class Coop : ICoop
    {
        private const string FileName = "coop.json";
        private List<IDevice> _devices = new();
        private IMessageService _messageService;
        private ILogger _logger;
        private SemaphoreSlim _semiphore = new(1,1);

        public Coop(ILogger<Coop> logger)
        {
            _logger = logger;
        }

        public Coop()
        {
            
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public List<IDevice> Devices
        {
            get => _devices;
            set
            {
                if (value.Equals(_devices)) return;
                _devices = value;
                OnPropertyChanged();
            }
        }

        public async Task Initialize(IMessageService messageService = null)
        {
            try
            {
                var gpio = new GpioController();
            }
            catch
            {
                _logger.LogInformation("GPIO not available");
            }
            if (messageService != null)
            {
                _messageService = messageService;
                _messageService.OnMessage += MessageService_OnMessage;
            }
            foreach (var device in Devices)
            {
                await device.Initialize(_messageService);
            }
        }

        private async void MessageService_OnMessage(object sender, System.EventArgs e)
        {
            // save state changes in case of power loss
            await _semiphore.WaitAsync();
            await Save();
            _semiphore.Release();
        }

        public async Task Save()
        {
            var json = Serialize();
            await File.WriteAllTextAsync(FileName, json);
        }

        public void Load()
        {
            if (File.Exists(FileName))
            {
                var json = File.ReadAllText(FileName);
                Devices = Deserialize(json);
            }
        }

        public List<IDevice> CopyDevices()
        {
            var json = Serialize();
            return Deserialize(json);
        }

        private string Serialize()
        {
            var json = JsonConvert.SerializeObject(Devices, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });
            return json;
        }

        private List<IDevice> Deserialize(string json)
        {
            var devices = JsonConvert.DeserializeObject<List<IDevice>>(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });
            return devices;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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