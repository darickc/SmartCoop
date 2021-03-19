using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SmartCoop.Core.Coop;
using SmartCoop.Core.Devices;
using SmartCoop.Infrastructure.Annotations;

namespace SmartCoop.Infrastructure.Coop
{
    public class Coop : ICoop
    {
        private const string FileName = "coop.json";
        private List<IDevice> _devices = new();
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

        public void Initialize()
        {
            foreach (var device in Devices)
            {
                device.Initialize();
            }
        }

        public async Task Save()
        {
            var json = JsonConvert.SerializeObject(Devices, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });
            await File.WriteAllTextAsync(FileName, json);
        }

        public void Load()
        {
            if (File.Exists(FileName))
            {
                var json = File.ReadAllText(FileName);
                Devices = JsonConvert.DeserializeObject<List<IDevice>>(json, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                });
            }
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