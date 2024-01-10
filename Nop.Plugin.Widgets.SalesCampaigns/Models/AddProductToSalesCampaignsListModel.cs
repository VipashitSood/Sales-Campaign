using Nop.Web.Areas.Admin.Models.Catalog;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Widgets.SalesCampaigns.Models
{
    /// <summary>
    /// Represents a product list model to add to the SalesCampaigns
    /// </summary>
    public partial record AddProductToSalesCampaignsListModel : BasePagedListModel<ProductModel>
    {
    }
}
