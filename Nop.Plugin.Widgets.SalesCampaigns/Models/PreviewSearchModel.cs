using Nop.Web.Framework.Models;

namespace Nop.Plugin.Widgets.SalesCampaigns.Models
{
    public partial record PreviewSearchModel : BaseSearchModel
    {
        #region Properties
        public int SalesCampaignId { get; set; }
        #endregion
    }
}
