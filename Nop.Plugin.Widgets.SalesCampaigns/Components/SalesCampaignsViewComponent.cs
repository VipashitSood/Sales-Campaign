using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Widgets.SalesCampaigns.Domain;
using Nop.Plugin.Widgets.SalesCampaigns.Factories;
using Nop.Plugin.Widgets.SalesCampaigns.Services;
using Nop.Web.Framework.Components;
using System;
using System.Threading.Tasks;
using Nop.Web.Models.Catalog;

namespace Nop.Plugin.Widgets.SalesCampaigns.Components
{
    [ViewComponent(Name = SalesCampaignsDefaults.VIEW_COMPONENT)]
    public class SalesCampaignsViewComponent : NopViewComponent
    {
        #region Field
        private readonly SalesCampaignsSettings _salesCampaignsSettings;
        private readonly ISalesCampaignsService _salesCampaignsService;
        private readonly ISalesCampaignsPublicModelFactory _salesCampaignsPublicModelFactory;
        #endregion

        #region Ctor
        public SalesCampaignsViewComponent(SalesCampaignsSettings salesCampaignsSettings,
            ISalesCampaignsService salesCampaignsService,
            ISalesCampaignsPublicModelFactory salesCampaignsPublicModelFactory)
        {
            _salesCampaignsSettings = salesCampaignsSettings;
            _salesCampaignsService = salesCampaignsService;
            _salesCampaignsPublicModelFactory = salesCampaignsPublicModelFactory;
        }
        #endregion
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
        {
            if (!_salesCampaignsSettings.Enable)
                return Content(string.Empty);

            var productDetailsModel = (ProductDetailsModel)additionalData;
            var activeCampaign = await _salesCampaignsService.GetPublicSaleCampaignByProductIdAsyn(productDetailsModel.Id);
            if (activeCampaign is null)
                return Content(string.Empty);

            //To get the id of the widet zone selected
            WidgetZone key = (WidgetZone)Enum.Parse(typeof(WidgetZone), widgetZone);
            var widgetId = (int)key;

            //check widgetId
            if (activeCampaign.WidgetZoneId != widgetId)
                return Content(string.Empty);

            //prepare model
            var salesCampaignModel = await _salesCampaignsPublicModelFactory.PrepareSalesCampaignsOverviewModels(activeCampaign);
            return View("~/Plugins/Widgets.SalesCampaigns/Views/Public/Default.cshtml", salesCampaignModel);

        }

    }
}
