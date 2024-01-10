using Nop.Plugin.Widgets.SalesCampaigns.Domain;
using Nop.Plugin.Widgets.SalesCampaigns.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.SalesCampaigns.Factories
{
    public interface ISalesCampaignsPublicModelFactory
    {
        /// <summary>
        /// Prepare Sales Campaigns Overview Models
        /// </summary>
        /// <param name="salesCampaigns"></param>
        /// <returns>
        ///  A task that represents the asynchronous operation
        /// The task result contains the Sales Campaigns Overview Models
        /// </returns>
        Task<SalesCampaignsModel> PrepareSalesCampaignsOverviewModels(SalesCampaignTable salesCampaigns);
    }
}
