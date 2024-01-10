using Nop.Web.Framework.Models;
using System.Collections.Generic;

namespace Nop.Plugin.Widgets.SalesCampaigns.Models
{    
    /// <summary>
    /// Represents a product model to add to the SalesCampaigns
    /// </summary>
    public partial record AddProductToSalesCampaignsModel : BaseNopModel
    {
        #region Ctor

        public AddProductToSalesCampaignsModel()
        {
            SelectedProductIds = new List<int>();
        }
        #endregion

        #region Properties

        public int SalesCampaignsId { get; set; }

        public IList<int> SelectedProductIds { get; set; }
        #endregion
    }
}
