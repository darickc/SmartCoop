using System.ComponentModel;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using SmartCoop.Core.Sensors;
using SmartCoop.Infrastructure.Annotations;

namespace SmartCoop.Infrastructure.Sensors
{
    public class PhotoCell : IPhotoCell
    {
        private bool _on;
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

        public void Initialize()
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