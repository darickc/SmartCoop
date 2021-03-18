using Microsoft.AspNetCore.Components;
using SmartCoop.Core.Sensors.Temperature;

namespace SmartCoop.Web.Shared.Devices
{
    public partial class Temperature
    {
        [Parameter] public ITemperature Device { get; set; }
    }
}
