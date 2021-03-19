using System;
using System.ComponentModel;
using Microsoft.AspNetCore.Components;
using SmartCoop.Core.Devices;

namespace SmartCoop.Web.Shared.Devices
{
    public partial class Device : IDisposable
    {
        [Parameter] public IDevice Item { get; set; }
        [Parameter] public bool Editing { get; set; }

        [Parameter] public EventCallback<IDevice> OnRemove { get; set; }

        public void Remove()
        {
            OnRemove.InvokeAsync(Item);
        }

        protected override void OnInitialized()
        {
            Item.PropertyChanged += DeviceOnPropertyChanged;
        }

        private void DeviceOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            InvokeAsync(StateHasChanged);
        }
        public void Dispose()
        {
            Item.PropertyChanged -= DeviceOnPropertyChanged;
        }
    }
}
