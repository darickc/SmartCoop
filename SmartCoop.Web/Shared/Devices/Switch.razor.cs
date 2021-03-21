using System.ComponentModel;
using Microsoft.AspNetCore.Components;
using SmartCoop.Core.Devices;

namespace SmartCoop.Web.Shared.Devices
{
    public partial class Switch
    {
        private bool _on;
        [Parameter] public ISwitch Device { get; set; }
        [Parameter] public bool Editing { get; set; }

        public bool On
        {
            get => Device.On;
            set
            {
                if (value)
                    Device.TurnOn();
                else
                    Device.TurnOff();
            }
        }
    }
}
