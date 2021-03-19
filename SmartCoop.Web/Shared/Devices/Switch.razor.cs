using System.ComponentModel;
using Microsoft.AspNetCore.Components;
using SmartCoop.Core.Devices;

namespace SmartCoop.Web.Shared.Devices
{
    public partial class Switch
    {
        [Parameter] public ISwitch Device { get; set; }
        [Parameter] public bool Editing { get; set; }
        
        public void TurnOn()
        {
            Device.TurnOn();
        }

        public void TurnOff()
        {
            Device.TurnOff();
        }
        
    }
}
