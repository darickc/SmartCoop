using System.ComponentModel;
using System.Device.Gpio;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SmartCoop.Core.Devices;
using SmartCoop.Core.Services;
using SmartCoop.Infrastructure.Annotations;

namespace SmartCoop.Infrastructure.Devices
{
    public class Switch : ISwitch
    {
        private GpioController _gpioController;
        private IMessageService _messageService;
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

        public bool On
        {
            get => _on;
            set
            {
                if (value == _on) return;
                _on = value;
                OnPropertyChanged();
            }
        }

        public int Pin { get; set; }

        public Task Initialize(IMessageService messageService)
        {
            _messageService = messageService;
            Dispose();
            try
            {
                _gpioController = new GpioController();
                _gpioController.OpenPin(Pin, PinMode.Output);

                if (On)
                {
                    TurnOn();
                }
                else
                {
                    TurnOff();
                }
            }
            catch 
            {
            }

            return Task.CompletedTask;
        }

        public void HandleMessage(string message, string payload)
        {
            if (message == "set")
            {
                switch (payload?.ToLower())
                {
                    case "on":
                        TurnOn();
                        break;
                    case "off":
                        TurnOff();
                        break;
                }
            }
        }

        public void TurnOn()
        {
            _gpioController?.Write(Pin, PinValue.Low);
            On = true;
            _messageService.SendMessage($"{Name}", "ON", this);
        }

        public void TurnOff()
        {
            _gpioController?.Write(Pin, PinValue.High);
            On = false;
            _messageService.SendMessage($"{Name}", "OFF", this);
        }

        public void Dispose()
        {
            if (_gpioController?.IsPinOpen(Pin) == true)
            {
                _gpioController?.ClosePin(Pin);
            }
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