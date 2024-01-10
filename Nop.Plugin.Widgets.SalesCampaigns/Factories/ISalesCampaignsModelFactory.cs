using Nop.Core.Domain.Catalog;
using Nop.Plugin.Widgets.SalesCampaigns.Domain;
using Nop.Plugin.Widgets.SalesCampaigns.Models;
using Nop.Web.Areas.Admin.Models.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.SalesCampaigns.Factories
{
    public interface ISalesCampaignsModelFactory
    {
        #region Sales Campaigns Factories

        /// <summary>
        /// Prepare SalesCampaigns search model
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the SalesCampaigns search model
        /// </returns>
        Task<SalesCampaignsSearchModel> PrepareSalesCampaignsSearchModelAsync(SalesCampaignsSearchModel searchModel);

        /// <summary>
        ///  Prepare SalesCampaigns list model
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the SalesCampaigns list model
        /// </returns>
        Task<SalesCampaignsListModel> PrepareSalesCampaignsListsModelAsync(SalesCampaignsSearchModel searchModel);

        /// <summary>
        /// Prepare SalesCampaigns model
        /// </summary>
        /// <param name="model"></param>
        /// <param name="salescampaigns"></param>
        /// <param name="excludeProperties"></param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the SalesCampaigns model
        /// </returns>
        Task<SalesCampaignsModel> PrepareSalesCampaignsModelAsync(SalesCampaignsModel model, SalesCampaignTable salescampaigns, bool excludeProperties = false);
        #endregion

        #region Override Product

        /// <summary>
        /// Prepare paged product list model
        /// </summary>
        /// <param name="searchModel">Sales Campaigns Product Search Model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the product list model
        /// </returns>
        Task<SalesCampaignsProductListModel> PrepareSaleCampProductListModelAsync(SalesCampaignsProductSearchModel searchModel, SalesCampaignTable salescampaigns);

        /// <summary>
        /// Prepare sales campaign product search model to add to the product
        /// </summary>
        /// <param name="searchModel">sales campaign product search model to add to the product</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the sales campaign product search model to add to the product
        /// </returns>
        Task<SalesCampaignsAddProductSearchModel> PrepareAddProductToSaleCampSearchModelAsync(SalesCampaignsAddProductSearchModel searchModel);

        /// <summary>
        /// Prepare paged sales campaign product list model to add to the product
        /// </summary>
        /// <param name="searchModel">sales campaign product search model to add to the product</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the sales campaign product list model to add to the product
        /// </returns>
        Task<AddProductToSalesCampaignsListModel> PrepareAddProductToSaleCampListModelAsync(SalesCampaignsAddProductSearchModel searchModel);
        #endregion

        #region Preview Product

        /// <summary>
        /// Prepare preview product price list
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the preview product price list model
        /// </returns>
        Task<PreviewListModel> PreparePreviewListModellAsync(PreviewSearchModel searchModel);
        #endregion
    }
}
