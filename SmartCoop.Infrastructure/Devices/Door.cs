using System.ComponentModel;
using System.Device.Gpio;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Timers;
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
        private Timer _timer;

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
        public int Timeout { get; set; } = 45;
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
                _gpioController.OpenPin(Pin1, PinMode.Output);
                _gpioController.OpenPin(Pin2, PinMode.Output);
                _gpioController.RegisterCallbackForPinValueChangedEvent(OpenPinNumber, PinEventTypes.Falling, Callback);
                _gpioController.RegisterCallbackForPinValueChangedEvent(ClosedPinNumber, PinEventTypes.Falling, Callback);

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

                _timer = new Timer(Timeout * 1000);
                _timer.Elapsed += Timer_Elapsed;
            }
            catch
            {
            }

            return Task.CompletedTask;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Stop();
            _messageService.SendMessage($"{Name}/state", "error", this);
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
            if (pinvaluechangedeventargs.ChangeType == PinEventTypes.Falling)
            {
                if (pinvaluechangedeventargs.PinNumber == OpenPinNumber && State == DoorState.Opening)
                {
                    Stop();
                    State = DoorState.Open;
                    _messageService.SendMessage($"{Name}/state", "open", this);
                }

                if (pinvaluechangedeventargs.PinNumber == ClosedPinNumber && State == DoorState.Closing)
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
                if (Timeout > 0)
                {
                    _timer.Start();
                }
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
                if (Timeout > 0)
                {
                    _timer.Start();
                }
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
            _timer.Stop();
        }

        public void Dispose()
        {
            if (_gpioController?.IsPinOpen(OpenPinNumber) == true)
            {
                _gpioController?.UnregisterCallbackForPinValueChangedEvent(OpenPinNumber, Callback);
                _gpioController?.ClosePin(OpenPinNumber);
            }
            if (_gpioController?.IsPinOpen(ClosedPinNumber) == true)
            {
                _gpioController?.UnregisterCallbackForPinValueChangedEvent(ClosedPinNumber, Callback);
                _gpioController?.ClosePin(ClosedPinNumber);
            }
            if (_gpioController?.IsPinOpen(Pin1) == true)
            {
                _gpioController?.ClosePin(Pin1);
            }
            if (_gpioController?.IsPinOpen(Pin2) == true)
            {
                _gpioController?.ClosePin(Pin2);
            }
            
            _gpioController?.Dispose();
            _timer?.Dispose();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}