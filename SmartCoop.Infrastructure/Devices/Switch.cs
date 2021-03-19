using System.ComponentModel;
using System.Device.Gpio;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using SmartCoop.Core.Devices;
using SmartCoop.Infrastructure.Annotations;

namespace SmartCoop.Infrastructure.Devices
{
    public class Switch : ISwitch
    {
        private GpioController _gpioController;
        private bool _on;
        private string _name;

        public string Name
        {
            get => _name;
            set
            {
                if (value == _name) return;
                _name = value;
                OnPropertyChanged();
            }
        }

        [JsonIgnore]
        public bool On
        {
            get => _on;
            private set
            {
                if (value == _on) return;
                _on = value;
                OnPropertyChanged();
            }
        }

        public int Pin { get; set; }

        public void Initialize()
        {
            Dispose();
            try
            {
                _gpioController = new GpioController();
                _gpioController.OpenPin(Pin, PinMode.Output);
            }
            catch 
            {
            }
        }

        public void TurnOn()
        {
            _gpioController?.Write(Pin, PinValue.High);
            On = true;
            // todo send notification
        }

        public void TurnOff()
        {
            _gpioController?.Write(Pin, PinValue.Low);
            On = false;
            // todo send notification
        }

        public void Dispose()
        {
            _gpioController?.ClosePin(Pin);
            _gpioController?.Dispose();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}