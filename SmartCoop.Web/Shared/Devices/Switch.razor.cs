using System.ComponentModel;
using Microsoft.AspNetCore.Components;
using SmartCoop.Core.Devices;

namespace SmartCoop.Web.Shared.Devices
{
    public partial class Switch
    {
        [Parameter] public ISwitch Device { get; set; }

        protected override void OnInitialized()
        {
            Device.PropertyChanged += DeviceOnPropertyChanged;
        }

        private void DeviceOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            InvokeAsync(StateHasChanged);
        }

        public void TurnOn()
        {
            Device.TurnOn();
        }

        public void TurnOff()
        {
            Device.TurnOff();
        }

        public void Dispose()
        {
            Device.PropertyChanged -= DeviceOnPropertyChanged;
        }
    }
}
