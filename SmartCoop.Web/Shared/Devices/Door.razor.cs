using Microsoft.AspNetCore.Components;
using SmartCoop.Core.Devices;

namespace SmartCoop.Web.Shared.Devices
{
    public partial class Door
    {
        [Parameter] public IDoor Device { get; set; }
        [Parameter] public bool Editing { get; set; }
        
        public void Open()
        {
            Device.Open();
        }

        public void Close()
        {
            Device.Close();
        }

    }
}
