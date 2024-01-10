using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Widgets.SalesCampaigns.Models
{
    /// <summary>
    /// Represents plugin configuration model
    /// </summary>
    public record ConfigurationModel : BaseNopModel
    {        		
		public int ActiveStoreScopeConfiguration { get; set; }  

        [NopResourceDisplayName("Plugins.Widgets.SalesCampaigns.Enable")]
        public bool Enable { get; set; }
        public bool Enable_OverrideForStore { get; set; }

	}
}