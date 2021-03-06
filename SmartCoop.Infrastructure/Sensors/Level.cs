using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Timers;
using Iot.Device.Hcsr04;
using SmartCoop.Core.Sensors;
using SmartCoop.Core.Services;
using SmartCoop.Infrastructure.Annotations;

namespace SmartCoop.Infrastructure.Sensors
{
    public class Level : ILevel
    {
        private Hcsr04 _sonar;
        private Timer _timer;
        private int _percent;
        private IMessageService _messageService;

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
        public double LowValue { get; set; }
        public double HighValue { get; set; }

        public void Dispose()
        {
            _timer?.Dispose();
            _sonar?.Dispose();
        }

        public Task Initialize(IMessageService messageService)
        {
            _messageService = messageService;
            Initialize();
            return Task.CompletedTask;
        }

        private void Initialize()
        {
            Dispose();
            try
            {
                _sonar = new Hcsr04(TriggerPin, EchoPin);
                _timer = new Timer(TimeSpan.FromSeconds(ReadFrequency).TotalMilliseconds);
                _timer.Elapsed += (sender, args) => GetPercent();
                _timer.Start();
                GetPercent();
            }
            catch
            {
            }
        }

        public void HandleMessage(string message, string payload)
        {
        }

        private void GetPercent()
        {
            var distance = GetAvgDistance();
            int percent = (int) Math.Round((distance - LowValue) / (HighValue - LowValue) * 100);

            if (Percent != percent)
            {
                Percent =  percent;
                _messageService.SendMessage($"{Name}", Percent.ToString("0.#"));
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
            var count = 0;
            while (count < 4)
            {
                if (_sonar.TryGetDistance(out var distance))
                {
                    return Math.Round(distance.Centimeters, 2) ;
                }
                count++;
            }

            return 0;
        }

        public void SetLow()
        {
            Initialize();
            LowValue = GetAvgDistance(); ;
            Dispose();
        }

        public void SetHigh()
        {
            Initialize();
            HighValue = GetAvgDistance();
            Dispose();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}