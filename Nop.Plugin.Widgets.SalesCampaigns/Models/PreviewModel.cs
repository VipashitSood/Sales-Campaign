using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nop.Plugin.Widgets.SalesCampaigns.Models
{
    public partial record PreviewModel : BaseNopEntityModel
    {
        public PreviewModel()
        {
            PreviewSearchModel = new PreviewSearchModel();
        }
        #region Properties

        [NopResourceDisplayName("Plugins.Widgets.SalesCampaigns.Fields.SalesCampaigns")]
        public int SalesCampaignsId { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.SalesCampaigns.Fields.Product")]
        public int ProductId { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.SalesCampaigns.Fields.Product")]
        public string ProductName { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.SalesCampaigns.Fields.Price")]
        public decimal Price { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.SalesCampaigns.Fields.SalePrice")]
        public decimal SalePrice { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.SalesCampaigns.Fields.OverrideCentsValue")]
        [Range(0.1, 0.99)]
        [RegularExpression(@"^\d+(.\d{0,2})?$")]
        public decimal OverrideCentsValue { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.SalesCampaigns.Fields.DiscountType")]
        public int DiscountTypeId { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.SalesCampaigns.Fields.SearchDiscountType")]
        public IList<SelectListItem> AvailableDiscountType { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.SalesCampaigns.Fields.OverrideCents")]
        public bool OverrideCents { get; set; }
        public PreviewSearchModel PreviewSearchModel { get; set; }

        #endregion
    }
}
