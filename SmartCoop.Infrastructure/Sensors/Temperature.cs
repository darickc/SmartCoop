using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Timers;
using Iot.Device.OneWire;
using SmartCoop.Core.Sensors.Temperature;
using SmartCoop.Core.Services;
using SmartCoop.Infrastructure.Annotations;

namespace SmartCoop.Infrastructure.Sensors
{
    public class Temperature : ITemperature
    {
        private OneWireThermometerDevice _dev;
        private Timer _timer;
        private double _temp;
        private IOneWire _oneWireDevice;
        private IMessageService _messageService;
        public string Name { get; set; }

        [JsonIgnore]
        public double Temp
        {
            get => _temp;
            private set
            {
                if (value.Equals(_temp)) return;
                _temp = value;
                OnPropertyChanged();
            }
        }

        public IOneWire OneWireDevice
        {
            get => _oneWireDevice;
            set
            {
                if (value.Equals(_oneWireDevice)) return;
                _oneWireDevice = value;
                OnPropertyChanged();
            }
        }

        public int ReadFrequency { get; set; } = 10;
        public TempType UnitType { get; set; }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        public void Initialize(IMessageService messageService)
        {
            _messageService = messageService;
            try
            {
                _dev = new(OneWireDevice.BusId, OneWireDevice.DevId);
                _timer = new Timer(TimeSpan.FromSeconds(ReadFrequency).TotalMilliseconds);
                _timer.Elapsed += async (sender, args) => await GetTemperature();
                _timer.Start();
            }
            catch
            {
              
            }
        }

        public void HandleMessage(string message)
        {
            throw new NotImplementedException();
        }

        private async Task GetTemperature()
        {

            try
            {
                var temp = await ReadTemperature();
                if (Math.Abs(temp - Temp) > .1)
                {
                    Temp = temp;
                    // todo send notification
                }
            }
            catch
            {
                _timer.Stop();
            }
        }

        public async Task<double> ReadTemperature()
        {
            var temp = await _dev.ReadTemperatureAsync();
            if (UnitType == TempType.Fahrenheit)
            {
                return temp.DegreesFahrenheit;
            }

            return temp.DegreesCelsius;
        }

        // Make sure you can access the bus device before requesting a device scan (or run using sudo)
        // $ sudo chmod a+rw /sys/bus/w1/devices/w1_bus_master1/w1_master_*

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}