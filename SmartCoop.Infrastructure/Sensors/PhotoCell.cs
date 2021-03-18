using System.ComponentModel;
using System.Runtime.CompilerServices;
using SmartCoop.Core.Sensors;
using SmartCoop.Infrastructure.Annotations;

namespace SmartCoop.Infrastructure.Sensors
{
    public class PhotoCell : IPhotoCell
    {
        public string Name { get; set; }
        public bool On { get; private set; }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public void Initialize()
        {
            throw new System.NotImplementedException();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}