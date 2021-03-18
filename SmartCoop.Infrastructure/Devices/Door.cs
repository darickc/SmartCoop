using System.ComponentModel;
using System.Device.Gpio;
using System.Runtime.CompilerServices;
using SmartCoop.Core.Devices;
using SmartCoop.Infrastructure.Annotations;

namespace SmartCoop.Infrastructure.Devices
{
    public class Door : IDoor
    {
        private GpioController _gpioController;

        public bool IsOpen { get; private set; }
        public bool IsClosed { get; private set; }
        public int OpenPinNumber { get; set; }
        public int ClosedPinNumber { get; set; }
        public string Name { get; set; }

        public void Initialize()
        {
            Dispose();
            try
            {
                _gpioController = new GpioController();
                _gpioController.OpenPin(OpenPinNumber, PinMode.InputPullDown);
                _gpioController.OpenPin(ClosedPinNumber, PinMode.InputPullDown);
                _gpioController.RegisterCallbackForPinValueChangedEvent(OpenPinNumber, PinEventTypes.Falling & PinEventTypes.Rising, Callback);
                _gpioController.RegisterCallbackForPinValueChangedEvent(ClosedPinNumber, PinEventTypes.Falling & PinEventTypes.Rising, Callback);
            }
            catch
            {
            }
        }

        private void Callback(object sender, PinValueChangedEventArgs pinvaluechangedeventargs)
        {
            if (pinvaluechangedeventargs.PinNumber == OpenPinNumber)
            {
                IsOpen = pinvaluechangedeventargs.ChangeType == PinEventTypes.Rising;
            }
            if (pinvaluechangedeventargs.PinNumber == ClosedPinNumber)
            {
                IsClosed = pinvaluechangedeventargs.ChangeType == PinEventTypes.Rising;
            }
        }

        public void Open()
        {
            // todo start motor to go up
        }

        public void Close()
        {
            // todo  start motor to go down
        }

        public void Dispose()
        {
            _gpioController?.ClosePin(OpenPinNumber);
            _gpioController?.ClosePin(ClosedPinNumber);
            _gpioController?.Dispose();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}