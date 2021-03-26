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
    public class Door : IDoor
    {
        private GpioController _gpioController;
        private IMessageService _messageService;
        private DoorState _state;

        public DoorState State
        {
            get => _state;
            set
            {
                if (value == _state) return;
                _state = value;
                OnPropertyChanged();
            }
        }

        public int OpenPinNumber { get; set; }
        public int ClosedPinNumber { get; set; }
        public int Pin1 { get; set; }
        public int Pin2 { get; set; }
        public string Name { get; set; }

        public Task Initialize(IMessageService messageService)
        {
            _messageService = messageService;
            Dispose();
            try
            {
                _gpioController = new GpioController();
                _gpioController.OpenPin(OpenPinNumber, PinMode.InputPullUp);
                _gpioController.OpenPin(ClosedPinNumber, PinMode.InputPullUp);
                _gpioController.RegisterCallbackForPinValueChangedEvent(OpenPinNumber, PinEventTypes.Falling & PinEventTypes.Rising, Callback);
                _gpioController.RegisterCallbackForPinValueChangedEvent(ClosedPinNumber, PinEventTypes.Falling & PinEventTypes.Rising, Callback);

                // check what the status was last and make sure it is still in that state
                var openPinState = _gpioController.Read(OpenPinNumber);
                var closedPinState = _gpioController.Read(ClosedPinNumber);

                // should be closed but it is not, so close it
                if (State == DoorState.Closed || State == DoorState.Closing && closedPinState == PinValue.Low)
                {
                    State = DoorState.Open;
                    Close();
                }

                // should be open but it is not, so open it
                if (State == DoorState.Open || State == DoorState.Opening && openPinState == PinValue.Low)
                {
                    State = DoorState.Closed;
                    Open();
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
                    case "open":
                        Open();
                        break;
                    case "close":
                        Close();
                        break;
                    case "toggle":
                        Toggle();
                        break;
                }
            }
        }

        private void Callback(object sender, PinValueChangedEventArgs pinvaluechangedeventargs)
        {
            if (pinvaluechangedeventargs.ChangeType == PinEventTypes.Rising)
            {
                if (pinvaluechangedeventargs.PinNumber == OpenPinNumber)
                {
                    Stop();
                    State = DoorState.Open;
                    _messageService.SendMessage($"{Name}/state", "open", this);
                }

                if (pinvaluechangedeventargs.PinNumber == ClosedPinNumber)
                {
                    Stop();
                    State = DoorState.Closed;
                    _messageService.SendMessage($"{Name}/state", "closed", this);
                }
            }
        }

        public void Open()
        {
            if (State != DoorState.Open && State != DoorState.Opening)
            {
                State = DoorState.Opening;
                _gpioController?.Write(Pin1, PinValue.High);
                _gpioController?.Write(Pin2, PinValue.Low);
                _messageService.SendMessage($"{Name}/state", "opening", this);
            }
        }

        public void Close()
        {
            if (State != DoorState.Closed && State != DoorState.Closing)
            {
                State = DoorState.Closing;
                _gpioController?.Write(Pin1, PinValue.Low);
                _gpioController?.Write(Pin2, PinValue.High);
                _messageService.SendMessage($"{Name}/state", "closing", this);
            }
        }

        public void Toggle()
        {
            if (State == DoorState.Open || State == DoorState.Opening)
            {
                Close();
            }
            else
            {
                Open();
            }
        }

        private void Stop()
        {
            _gpioController?.Write(Pin1, PinValue.Low);
            _gpioController?.Write(Pin2, PinValue.Low);
        }

        public void Dispose()
        {
            if (_gpioController?.IsPinOpen(OpenPinNumber) == true)
            {
                _gpioController?.ClosePin(OpenPinNumber);
            }
            if (_gpioController?.IsPinOpen(ClosedPinNumber) == true)
            {
                _gpioController?.ClosePin(ClosedPinNumber);
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