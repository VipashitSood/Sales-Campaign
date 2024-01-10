using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Widgets.SalesCampaigns.Models
{
    public partial record SalesCampaignsProductModel : BaseNopEntityModel
    {
        #region Properties

        [NopResourceDisplayName("Plugins.Widgets.SalesCampaigns.Fields.SalesCampaigns")]
        public int SalesCampaignsId { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.SalesCampaigns.Fields.Product")]
        public int ProductId { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.SalesCampaigns.Fields.Product")]
        public string ProductName { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.SalesCampaigns.Fields.IsState")]
        public bool IsState { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.SalesCampaigns.Fields.Price")]
        public decimal Price { get; set; }
        [NopResourceDisplayName("Plugins.Widgets.SalesCampaigns.Fields.SalePrice")]
        public decimal SalePrice { get; set; }

        #endregion
    }
}
