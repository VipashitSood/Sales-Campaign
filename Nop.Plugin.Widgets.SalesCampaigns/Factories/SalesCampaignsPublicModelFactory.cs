using Nop.Plugin.Widgets.SalesCampaigns.Domain;
using Nop.Plugin.Widgets.SalesCampaigns.Models;
using Nop.Plugin.Widgets.SalesCampaigns.Services;
using System;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.SalesCampaigns.Factories
{
    public class SalesCampaignsPublicModelFactory : ISalesCampaignsPublicModelFactory
    {
        #region Fields
        private readonly ISalesCampaignsService _salesCampaignsService;
        #endregion

        #region ctor
        public SalesCampaignsPublicModelFactory(
            ISalesCampaignsService salesCampaignsService)
        {
            _salesCampaignsService = salesCampaignsService;
        }
        #endregion

        #region Method

        /// <summary>
        /// Prepare Sales Campaigns Overview Models
        /// </summary>
        /// <param name="salesCampaign"></param>
        /// <returns>
        ///  A task that represents the asynchronous operation
        /// The task result contains the Sales Campaigns Overview Models
        /// </returns>
        public virtual async Task<SalesCampaignsModel> PrepareSalesCampaignsOverviewModels(SalesCampaignTable salesCampaign)
        {           

            if (salesCampaign == null)
                throw new ArgumentNullException(nameof(salesCampaign));
                 
            var model = new SalesCampaignsModel()
            {
                Id =salesCampaign.Id,
                Name = salesCampaign.Name,
                CategoryId = salesCampaign.CategoryId,
                FromDate = salesCampaign.FromDate,
                ToDate = salesCampaign.ToDate,
                CampaignStateId = salesCampaign.CampaignStateId,
                ClockTypeId = salesCampaign.ClockTypeId,
                DiscountValue = salesCampaign.DiscountValue,
                ToTime = salesCampaign.ToTime
            };
            return await Task.FromResult(model);
        }
        #endregion
    }
}
