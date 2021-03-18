using Microsoft.AspNetCore.Components;
using SmartCoop.Core.Coop;
using SmartCoop.Infrastructure.Coop;

namespace SmartCoop.Web.Pages
{
    public partial class Index
    {
        [Inject] public ICoop Coop { get; set; }
    }
}
