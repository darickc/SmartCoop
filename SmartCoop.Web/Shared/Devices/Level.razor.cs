using Microsoft.AspNetCore.Components;
using SmartCoop.Core.Sensors;

namespace SmartCoop.Web.Shared.Devices
{
    public partial class Level
    {
        [Parameter] public ILevel Device { get; set; }
        [Parameter] public bool Editing { get; set; }
    }
}
