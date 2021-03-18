using System;
using System.ComponentModel;
using Microsoft.AspNetCore.Components;
using SmartCoop.Core.Devices;

namespace SmartCoop.Web.Shared.Devices
{
    public partial class Door : IDisposable
    {
        [Parameter] public IDoor Device { get; set; }

        protected override void OnInitialized()
        {
            Device.PropertyChanged += DeviceOnPropertyChanged;
        }

        private void DeviceOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            StateHasChanged();
        }

        public void Open()
        {
            Device.Open();
        }

        public void Close()
        {
            Device.Close();
        }

        public void Dispose()
        {
            Device.PropertyChanged -= DeviceOnPropertyChanged;
        }
    }
}
