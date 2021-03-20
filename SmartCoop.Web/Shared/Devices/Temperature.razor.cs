using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using SmartCoop.Core.Sensors.Temperature;
using SmartCoop.Infrastructure.Sensors;

namespace SmartCoop.Web.Shared.Devices
{
    public partial class Temperature
    {
        private bool _editing;
        [Parameter] public ITemperature Device { get; set; }

        [Parameter]
        public bool Editing
        {
            get => _editing;
            set
            {
                if (value.Equals(_editing)) return;
                _editing = value;
                if (value)
                {
                    GetOneWireDevices();
                }
            }
        }

        public bool Loading { get; set; }
        public List<IOneWire> Devices { get; set; }

        private async void GetOneWireDevices()
        {
            Loading = true;
            Devices = await OneWire.GetAvailableOneWires();
            Loading = false;
            await InvokeAsync(StateHasChanged);
        }

        public void UseDevice(IOneWire oneWire)
        {
            Device.OneWireDevice = oneWire;
        }
    }
}
