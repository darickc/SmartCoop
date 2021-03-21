using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SmartCoop.Core.Sensors;
using SmartCoop.Core.Services;
using SmartCoop.Infrastructure.Annotations;

namespace SmartCoop.Infrastructure.Sensors
{
    public class PhotoCell : IPhotoCell
    {
        private bool _on;
        private IMessageService _messageService;
        public string Name { get; set; }

        [JsonIgnore]
        public bool On
        {
            get => _on;
            set
            {
                if (value.Equals(_on)) return;
                _on = value;
                OnPropertyChanged();
            }
        }

        public void Dispose()
        {
        }

        public Task Initialize(IMessageService messageService)
        {
            _messageService = messageService;
            return Task.CompletedTask;
        }

        public void HandleMessage(string message, string payload)
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}