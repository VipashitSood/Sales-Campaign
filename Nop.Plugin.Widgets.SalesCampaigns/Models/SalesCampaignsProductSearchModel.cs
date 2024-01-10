using Nop.Web.Framework.Models;

namespace Nop.Plugin.Widgets.SalesCampaigns.Models
{ 
    public partial record SalesCampaignsProductSearchModel : BaseSearchModel
    {
        #region Properties

        public int SalesCampaignId { get; set; }
        #endregion
    }
}
