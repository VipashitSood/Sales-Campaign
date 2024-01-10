using Nop.Core;
using Nop.Plugin.Widgets.SalesCampaigns.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.SalesCampaigns.Services
{
    public interface ISalesCampaignsService
    {
        #region Sales Campaign Service

        /// <summary>
        /// Insert Sales Campaigns Record
        /// </summary>
        /// <param name="salescampaign"></param>
        /// <returns></returns>
        Task InsertSalesCampaignsAsync(SalesCampaignTable salescampaign);
        /// <summary>
        /// Update Sales Campaigns Record
        /// </summary>
        /// <param name="salescampaign"></param>
        /// <returns></returns>
        Task UpdateSalesCampaignsAsync(SalesCampaignTable salescampaign);
        /// <summary>
        /// Delete Sales Campaigns Record
        /// </summary>
        /// <param name="salescampaign"></param>
        /// <returns></returns>
        Task DeleteSalesCampaignsAsync(SalesCampaignTable salescampaign);
        /// <summary>
        /// Get Sales Campaigns By Identifier
        /// </summary>
        ///<param name="salescampaignId">sales campaign identifier</param>
        /// <returns></returns>
        Task<SalesCampaignTable> GetSalesCampaignsByIdAsync(int salescampaignId);
        /// <summary>
        /// Get All Sales Campaigns Records
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="showHidden"></param>
        /// <param name="overridePublished"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        Task<IPagedList<SalesCampaignTable>> GetAllSalesCampaignsAsync(int pageIndex = 0, int pageSize = int.MaxValue,
        bool showHidden = false, bool? overridePublished = null, int categoryId = 0);
        #endregion

        #region Sales Campaigns Product Mapping 

        /// <summary>
        /// Get Product By Sales Campaigns Identifier
        /// </summary>
        /// <param name="salescampaignId"> sales campaign identifier</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="showHidden"></param>
        /// <returns></returns>
        Task<IPagedList<SalesCampaignsProductMapping>> GetProductBySalesCampaignsIdAsync(int salescampaignId,
           int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);
        /// <summary>
        /// Get Product Price By Sales Campaign Identifier
        /// </summary>
        /// <param name="salescampaignId"> sales campaign identifier</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="showHidden"></param>
        /// <returns></returns>
        Task<IPagedList<SalesCampaignsProductMapping>> GetProductPriceBySalesCampaignsIdAsync(int salescampaignId,
           int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);
        /// <summary>
        /// Get Product Sales Campaigns By Identifier
        /// </summary>
        /// <param name="productsalescampaignId"> product sales campaign identifier</param>
        /// <returns></returns>
        Task<SalesCampaignsProductMapping> GetProductSalesCampaignsByIdAsync(int productsalescampaignId);
        /// <summary>
        /// Insert Product SalesCampaigns Record
        /// </summary>
        /// <param name="productsalescampaign"></param>
        /// <returns></returns>
        Task InsertProductSalesCampaignsAsync(SalesCampaignsProductMapping productsalescampaign);
        /// <summary>
        /// Update Product Sales Campaigns Record
        /// </summary>
        /// <param name="salescampaignproduct"></param>
        /// <returns></returns>
        Task UpdateProductSalesCampaignsAsync(SalesCampaignsProductMapping salescampaignproduct);
        /// <summary>
        /// Delete Product Sales Campaigns Record
        /// </summary>
        /// <param name="salescampaignproduct"></param>
        /// <returns></returns>
        Task DeleteProductSalesCampaignsAsync(SalesCampaignsProductMapping salescampaignproduct);
        /// <summary>
        /// Find Product Sales Campaign Record
        /// </summary>
        /// <param name="source"></param>
        /// <param name="productId"></param>
        /// <param name="salescampaignId"></param>
        /// <returns></returns>
        SalesCampaignsProductMapping FindProductSalesCampaign(IList<SalesCampaignsProductMapping> source, int productId, int salescampaignId);
        /// <summary>
        /// Check Existing Product Identifier
        /// </summary>
        /// <param name="productId"> product identifier</param>
        /// <returns></returns>
        int CheckExistingProductID(int productId);
        /// <summary>
        /// Get active sales campign by product identifiers
        /// </summary>
        /// <param name="productId"> product identifier</param>
        /// <returns></returns>
        Task<SalesCampaignTable> GetPublicSaleCampaignByProductIdAsyn(int productId);
        #endregion
    }
}
