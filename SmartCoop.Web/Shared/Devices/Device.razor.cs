using Microsoft.AspNetCore.Components;
using SmartCoop.Core.Devices;

namespace SmartCoop.Web.Shared.Devices
{
    public partial class Device
    {
        [Parameter] public IDevice Item { get; set; }
    }
}
