using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Data;
using Nop.Plugin.Widgets.SalesCampaigns.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.SalesCampaigns.Services
{
    public partial class SalesCampaignsService : ISalesCampaignsService
    {
        #region Fields
        private readonly IRepository<SalesCampaignTable> _salesCampaignsRepository;
        private readonly IRepository<SalesCampaignsProductMapping> _salesCampaignsProductRepository;
        private readonly IRepository<Product> _productRepository;

        #endregion

        #region Ctor
        public SalesCampaignsService(
            IRepository<SalesCampaignTable> salescampaignsRepository,
            IRepository<SalesCampaignsProductMapping> salesCampaignsProductRepository,
            IRepository<Product> productRepository)
        {
            _salesCampaignsRepository = salescampaignsRepository;
            _salesCampaignsProductRepository = salesCampaignsProductRepository;
            _productRepository = productRepository;
        }
        #endregion

        #region Sales Campaign Service

        /// <summary>
        /// Insert Sales Campaigns Record
        /// </summary>
        /// <param name="salescampaign"></param>
        /// <returns></returns>
        public virtual async Task InsertSalesCampaignsAsync(SalesCampaignTable salescampaign)
        {
            await _salesCampaignsRepository.InsertAsync(salescampaign);
        }
        /// <summary>
        /// Update Sales Campaigns Record
        /// </summary>
        /// <param name="salescampaign"></param>
        /// <returns></returns>
        public virtual async Task UpdateSalesCampaignsAsync(SalesCampaignTable salescampaign)
        {
            await _salesCampaignsRepository.UpdateAsync(salescampaign);
        }
        /// <summary>
        /// Delete Sales Campaigns Record
        /// </summary>
        /// <param name="salescampaign"></param>
        /// <returns></returns>
        public virtual async Task DeleteSalesCampaignsAsync(SalesCampaignTable salescampaign)
        {
            //salescampaigns.Deleted = true;

            await _salesCampaignsRepository.DeleteAsync(salescampaign);
            //await _salesCampaignsRepository.UpdateAsync(salescampaign);
        }
        /// <summary>
        /// Get Sales Campaigns By Identifier
        /// </summary>
        ///<param name="salescampaignId">sales campaign identifier</param>
        /// <returns></returns>
        public virtual async Task<SalesCampaignTable> GetSalesCampaignsByIdAsync(int salescampaignId)
        {
            return await _salesCampaignsRepository.GetByIdAsync(salescampaignId);
        }
        /// <summary>
        /// Get All Sales Campaigns Records
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="showHidden"></param>
        /// <param name="overridePublished"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public virtual async Task<IPagedList<SalesCampaignTable>> GetAllSalesCampaignsAsync(int pageIndex = 0, int pageSize = int.MaxValue,
        bool showHidden = false, bool? overridePublished = null, int categoryId = 0)
        {
            var query1 = _salesCampaignsRepository.Table;
            var salescampaigns = await _salesCampaignsRepository.GetAllPagedAsync(query =>
            {
                if (categoryId > 0)
                    query = query.Where(pr => pr.CategoryId == categoryId);
                return query.Distinct().OrderByDescending(a => a.Id);
            });
            //paging
            return new PagedList<SalesCampaignTable>(salescampaigns, pageIndex, pageSize);
        }
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
        public virtual async Task<IPagedList<SalesCampaignsProductMapping>> GetProductBySalesCampaignsIdAsync(int salescampaignId,
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            if (salescampaignId == 0)
                return new PagedList<SalesCampaignsProductMapping>(new List<SalesCampaignsProductMapping>(), pageIndex, pageSize);

            var query = from pc in _salesCampaignsProductRepository.Table
                        join p in _productRepository.Table on pc.ProductId equals p.Id
                        where pc.SalesCampaignsId == salescampaignId && !p.Deleted
                        orderby pc.Id
                        select pc;
            return await query.ToPagedListAsync(pageIndex, pageSize);
        }
        /// <summary>
        /// Get Product Price By Sales Campaign Identifier
        /// </summary>
        /// <param name="salescampaignId"> sales campaign identifier</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="showHidden"></param>
        /// <returns></returns>
        public virtual async Task<IPagedList<SalesCampaignsProductMapping>> GetProductPriceBySalesCampaignsIdAsync(int salescampaignId,
           int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            if (salescampaignId == 0)
                return new PagedList<SalesCampaignsProductMapping>(new List<SalesCampaignsProductMapping>(), pageIndex, pageSize);

            var query = from pc in _salesCampaignsProductRepository.Table
                        join p in _productRepository.Table on pc.ProductId equals p.Id
                        where pc.SalesCampaignsId == salescampaignId && p.OldPrice == 0
                        orderby pc.Id
                        select pc;
            return await query.ToPagedListAsync(pageIndex, pageSize);
        }
        /// <summary>
        /// Get Product Sales Campaigns By Identifier
        /// </summary>
        /// <param name="productsalescampaignId"> product sales campaign identifier</param>
        /// <returns></returns>
        public virtual async Task<SalesCampaignsProductMapping> GetProductSalesCampaignsByIdAsync(int salescampaignproductId)
        {
            return await _salesCampaignsProductRepository.GetByIdAsync(salescampaignproductId, cache => default);
        }
        /// <summary>
        /// Insert Product SalesCampaigns Record
        /// </summary>
        /// <param name="productsalescampaign"></param>
        /// <returns></returns>
        public virtual async Task InsertProductSalesCampaignsAsync(SalesCampaignsProductMapping salescampaignproduct)
        {
            await _salesCampaignsProductRepository.InsertAsync(salescampaignproduct);
        }
        /// <summary>
        /// Update Product Sales Campaigns Record
        /// </summary>
        /// <param name="salescampaignproduct"></param>
        /// <returns></returns>
        public virtual async Task UpdateProductSalesCampaignsAsync(SalesCampaignsProductMapping salescampaignproduct)
        {
            await _salesCampaignsProductRepository.UpdateAsync(salescampaignproduct);
        }
        /// <summary>
        /// Delete Product Sales Campaigns Record
        /// </summary>
        /// <param name="salescampaignproduct"></param>
        /// <returns></returns>
        public virtual async Task DeleteProductSalesCampaignsAsync(SalesCampaignsProductMapping salescampaignproduct)
        {
            await _salesCampaignsProductRepository.DeleteAsync(salescampaignproduct);
        }
        /// <summary>
        /// Find Product Sales Campaign Record
        /// </summary>
        /// <param name="source"></param>
        /// <param name="productId"></param>
        /// <param name="salescampaignId"></param>
        /// <returns></returns>
        public virtual SalesCampaignsProductMapping FindProductSalesCampaign(IList<SalesCampaignsProductMapping> source, int productId, int salescampaignId)
        {
            foreach (var productSalesCampaign in source)
                if (productSalesCampaign.ProductId == productId && productSalesCampaign.SalesCampaignsId == salescampaignId)
                    return productSalesCampaign;

            return null;
        }
        /// <summary>
        /// Check Existing Product Identifier
        /// </summary>
        /// <param name="productId"> product identifier</param>
        /// <returns></returns>
        public virtual int CheckExistingProductID(int productId)
        {
            var product = from bn in _salesCampaignsProductRepository.Table
                          where bn.ProductId == productId
                          select bn.ProductId;
            return product.FirstOrDefault();
        }
        /// <summary>
        /// Get active sales campign by product identifiers
        /// </summary>
        /// <param name="productId"> product identifier</param>
        /// <returns></returns>
        public virtual async Task<SalesCampaignTable> GetPublicSaleCampaignByProductIdAsyn(int productId)
        {
            var query = from sc in _salesCampaignsRepository.Table
                        join scp in _salesCampaignsProductRepository.Table on sc.Id equals scp.SalesCampaignsId
                        where scp.ProductId == productId
                        select sc;

            var runningCampaign = query.Where(q => q.CampaignStateId == (int)CampaignStateType.Running).OrderByDescending(x => x.DiscountValue).ToList();
            if (runningCampaign.Any())
            {
                return await Task.FromResult(runningCampaign.FirstOrDefault());
            }
            query = query.Where(q => q.CampaignStateId == (int)CampaignStateType.Scheduled).OrderByDescending(x => x.DiscountValue);
            //query = query.Where(q => DateTime.UtcNow.Ticks > q.FromDate.Ticks && DateTime.UtcNow.Ticks < q.ToDate.Ticks);
            query = query.Where(q => DateTime.UtcNow > q.FromDate && DateTime.UtcNow < q.ToDate);
            return await Task.FromResult(query.FirstOrDefault());
        }
        #endregion
    }
}
