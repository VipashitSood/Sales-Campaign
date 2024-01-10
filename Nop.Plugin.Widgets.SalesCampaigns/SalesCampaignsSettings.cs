using Nop.Core.Configuration;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Widgets.SalesCampaigns
{
    public class SalesCampaignsSettings : ISettings
    {
        [NopResourceDisplayName("Plugins.Widgets.SalesCampaigns.Enable")]
        public bool Enable { get; set; }
    }
}