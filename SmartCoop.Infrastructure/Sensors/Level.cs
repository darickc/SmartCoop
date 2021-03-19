using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using System.Timers;
using Iot.Device.Hcsr04;
using SmartCoop.Core.Sensors;
using SmartCoop.Infrastructure.Annotations;

namespace SmartCoop.Infrastructure.Sensors
{
    public class Level : ILevel
    {
        private Hcsr04 _sonar;
        private Timer _timer;
        private int _percent;

        public string Name { get; set; }

        [JsonIgnore]
        public int Percent
        {
            get => _percent;
            private set {
                if (value.Equals(_percent)) return;
                _percent = value;
                OnPropertyChanged();
            }
        }

        public int TriggerPin { get; set; }
        public int EchoPin { get; set; }
        public int ReadFrequency { get; set; } = 10;
        public double LowValue { get; private set; }
        public double HighValue { get; private set; }

        public void Dispose()
        {
            _timer?.Dispose();
            _sonar?.Dispose();
        }

        public void Initialize()
        {
            Dispose();
            try
            {
                _sonar = new Hcsr04(TriggerPin, EchoPin);
                _timer = new Timer(TimeSpan.FromSeconds(ReadFrequency).TotalMilliseconds);
                _timer.Elapsed += (sender, args) => GetPercent();
                _timer.Start();
            }
            catch
            {
            }
        }

        private void GetPercent()
        {
            var distance = GetAvgDistance();
            int percent = (int) Math.Round((distance - LowValue) / (HighValue - LowValue) * 100);

            if (Percent != percent)
            {
                Percent =  percent;
                // notify changed.
            }
        }

        private double GetAvgDistance()
        {
            var distance1 = GetDistance();
            var distance2 = GetDistance();
            var distance3 = GetDistance();

            return (distance1 + distance2 + distance3) / 3;
        }

        private double GetDistance()
        {
            if (_sonar.TryGetDistance(out var distance))
            {
                return distance.Centimeters;
            }

            return 0;
        }

        public void SetLow()
        {
            var distance = GetAvgDistance();
            LowValue = distance;
        }

        public void SetHigh()
        {
            var distance = GetAvgDistance();
            HighValue = distance;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}