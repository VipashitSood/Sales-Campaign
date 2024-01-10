using Nop.Core.Domain.Catalog;
using Nop.Data;
using Nop.Plugin.Widgets.SalesCampaigns.Domain;
using Nop.Plugin.Widgets.SalesCampaigns.Models;
using Nop.Plugin.Widgets.SalesCampaigns.Services;
using Nop.Services.Catalog;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Seo;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.Catalog;
using Nop.Web.Framework.Models.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.SalesCampaigns.Factories
{
    public class SalesCampaignsModelFactory : ISalesCampaignsModelFactory
    {

        #region Fields
        private readonly IBaseAdminModelFactory _baseAdminModelFactory;
        private readonly ILocalizationService _localizationService;
        private readonly ISalesCampaignsService _salesCampaigns;
        private readonly IProductService _productService;
        private readonly IUrlRecordService _urlRecordService;
        #endregion

        #region ctor
        public SalesCampaignsModelFactory(IBaseAdminModelFactory baseAdminModelFactory,
            ILocalizationService localizationService,
            ISalesCampaignsService salesCampaigns,
            IProductService productService,
             IUrlRecordService urlRecordService
)
        {
            _baseAdminModelFactory = baseAdminModelFactory;
            _localizationService = localizationService;
            _salesCampaigns = salesCampaigns;
            _productService = productService;
            _urlRecordService = urlRecordService;
        }
        #endregion

        #region Utilities
        /// <summary>
        /// Prepare Sales Product Search Model
        /// </summary>
        /// <param name="searchModel"></param>
        /// <param name="salesCampaign"></param>
        /// <returns></returns>
        protected virtual SalesCampaignsProductSearchModel PrepareSalesProductSearchModel(SalesCampaignsProductSearchModel searchModel, SalesCampaignTable salesCampaign)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            if (salesCampaign == null)
                throw new ArgumentNullException(nameof(salesCampaign));

            searchModel.SalesCampaignId = salesCampaign.Id;

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }
        #endregion

        #region Method

        /// <summary>
        /// Prepare SalesCampaigns search model
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the SalesCampaigns search model
        /// </returns>
        public async Task<SalesCampaignsSearchModel> PrepareSalesCampaignsSearchModelAsync(SalesCampaignsSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //prepare available categories
            await _baseAdminModelFactory.PrepareCategoriesAsync(searchModel.AvailableCategories);

            //prepare page parameters
            searchModel.SetGridPageSize();
            return searchModel;
        }

        /// <summary>
        ///  Prepare SalesCampaigns list model
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the SalesCampaigns list model
        /// </returns>
        public virtual async Task<SalesCampaignsListModel> PrepareSalesCampaignsListsModelAsync(SalesCampaignsSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //get salescampaigns
            var salescampaigns = await _salesCampaigns.GetAllSalesCampaignsAsync(showHidden: true,
                pageIndex: searchModel.Page - 1, pageSize: searchModel.PageSize,
                overridePublished: searchModel.SearchCategoryId == 0 ? null : (bool?)(searchModel.SearchCategoryId == 1));

            //prepare grid model
            var model = await new SalesCampaignsListModel().PrepareToGridAsync(searchModel, salescampaigns, () =>
            {
                return salescampaigns.SelectAwait(async salescampaign =>
                {
                    //sample manual code.
                    var salescampaignsModel = new SalesCampaignsModel();
                    salescampaignsModel.Id = salescampaign.Id;
                    salescampaignsModel.Name = salescampaign.Name;
                    salescampaignsModel.FromDate = salescampaign.FromDate;
                    salescampaignsModel.ToDate = salescampaign.ToDate;
                    salescampaignsModel.SchedulePatternTypeId = salescampaign.SchedulePatternTypeId;
                    salescampaignsModel.SchedulePattern = Enum.GetName(typeof(SchedulePatternType), salescampaign.SchedulePatternTypeId);
                    salescampaignsModel.CampaignStateId = salescampaign.CampaignStateId;
                    salescampaignsModel.State = Enum.GetName(typeof(CampaignStateType), salescampaignsModel.CampaignStateId);
                    return salescampaignsModel;
                });
            });

            return model;
        }

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
        public virtual async Task<SalesCampaignsModel> PrepareSalesCampaignsModelAsync(SalesCampaignsModel model, SalesCampaignTable salescampaigns, bool excludeProperties = false)
        {
            if (salescampaigns != null)
            {
                if (model == null)
                {
                    //fill in model values from the entity   
                    model = new SalesCampaignsModel();
                    model.Id = salescampaigns.Id;
                    #region   Comment Category part 
                    //model.SearchCategoryId = salescampaigns.CategoryId;
                    //model.CategoryName = (await _categoryService.GetCategoryByIdAsync(salescampaigns.CategoryId)).Name;
                    //model.AssignToCategory = salescampaigns.AssignToCategory;
                    //model.CategoryId = salescampaigns.CategoryId;
                    #endregion
                    model.Name = salescampaigns.Name;
                    model.WidgetZoneId = salescampaigns.WidgetZoneId;
                    model.DiscountTypeId = salescampaigns.DiscountTypeId;
                    model.DiscountValue = salescampaigns.DiscountValue;
                    model.OverrideCents = salescampaigns.OverrideCents;
                    model.OverrideCentsValue = salescampaigns.OverrideCentsValue;
                    //extra columns 
                    model.ClockTypeId = salescampaigns.ClockTypeId;
                    model.FromDate = salescampaigns.FromDate;
                    model.ToDate = salescampaigns.ToDate;
                    model.FromTime = salescampaigns.FromTime;
                    model.ToTime = salescampaigns.ToTime;
                    model.SchedulePatternTypeId = salescampaigns.SchedulePatternTypeId;

                    //set default values for the new model
                    if (salescampaigns == null)
                    {
                        model.CampaignStateId = (int)CampaignStateType.Pending;  //assign value
                    }
                    //model.PerctageIn = "%";
                    model.Price = salescampaigns.Price;
                }
                PrepareSalesProductSearchModel(model.SalesCampaignsProductSearchModel, salescampaigns);
            }
            //model.PerctageIn = "%";
            await _baseAdminModelFactory.PrepareCategoriesAsync(model.AvailableCategories,
                      defaultItemText: await _localizationService.GetResourceAsync("Select Any Category"));
            return model;
        }
        #endregion

        #region Override Product Tab

        /// <summary>
        /// Prepare paged category product list model
        /// </summary>
        /// <param name="searchModel">Category product search model</param>
        /// <param name="category">Category</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the category product list model
        /// </returns>
        public virtual async Task<SalesCampaignsProductListModel> PrepareSaleCampProductListModelAsync(SalesCampaignsProductSearchModel searchModel, SalesCampaignTable salescampaigns)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            if (salescampaigns == null)
                throw new ArgumentNullException(nameof(salescampaigns));

            //get product sale
            var productSales = await _salesCampaigns.GetProductBySalesCampaignsIdAsync(salescampaigns.Id,
                showHidden: true,
                pageIndex: searchModel.Page - 1, pageSize: searchModel.PageSize);

            //prepare grid model 
            var model = await new SalesCampaignsProductListModel().PrepareToGridAsync(searchModel, productSales, () =>
            {
                return productSales.SelectAwait(async productSale =>
                    {
                        //fill in model values from the entity
                        var salesCampaignsProductModel = new SalesCampaignsProductModel();
                        salesCampaignsProductModel.Id = productSale.Id;
                        salesCampaignsProductModel.ProductId = productSale.ProductId;
                        salesCampaignsProductModel.SalesCampaignsId = productSale.SalesCampaignsId;
                        salesCampaignsProductModel.IsState = productSale.IsState;
                        salesCampaignsProductModel.Price = productSale.Price;
                        //fill in additional values (not existing in the entity)                        
                        salesCampaignsProductModel.ProductName = (await _productService.GetProductByIdAsync(productSale.ProductId))?.Name;

                        return salesCampaignsProductModel;
                    });
            });

            return model;
        }

        /// <summary>
        /// Prepare Sales Campaigns product search model to add to the product
        /// </summary>
        /// <param name="searchModel">Sales Campaigns product search model to add to the product</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the Sales Campaigns product search model to add to the product
        /// </returns>
        public virtual async Task<SalesCampaignsAddProductSearchModel> PrepareAddProductToSaleCampSearchModelAsync(SalesCampaignsAddProductSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //prepare available categories
            await _baseAdminModelFactory.PrepareCategoriesAsync(searchModel.AvailableCategories);

            //prepare available manufacturers
            await _baseAdminModelFactory.PrepareManufacturersAsync(searchModel.AvailableManufacturers);

            //prepare available stores
            await _baseAdminModelFactory.PrepareStoresAsync(searchModel.AvailableStores);

            //prepare available vendors
            await _baseAdminModelFactory.PrepareVendorsAsync(searchModel.AvailableVendors);

            //prepare available product types
            await _baseAdminModelFactory.PrepareProductTypesAsync(searchModel.AvailableProductTypes);

            //prepare page parameters
            searchModel.SetPopupGridPageSize();

            return searchModel;
        }

        /// <summary>
        /// Prepare paged product list model to add to the category
        /// </summary>
        /// <param name="searchModel">Product search model to add to the category</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the product list model to add to the category
        /// </returns>
        public virtual async Task<AddProductToSalesCampaignsListModel> PrepareAddProductToSaleCampListModelAsync(SalesCampaignsAddProductSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //get products
            var products = await _productService.SearchProductsAsync(showHidden: true,
            categoryIds: new List<int> { searchModel.SearchCategoryId },
            manufacturerIds: new List<int> { searchModel.SearchManufacturerId },
            storeId: searchModel.SearchStoreId,
            vendorId: searchModel.SearchVendorId,
            productType: searchModel.SearchProductTypeId > 0 ? (ProductType?)searchModel.SearchProductTypeId : null,
            keywords: searchModel.SearchProductName,
            pageIndex: searchModel.Page - 1, pageSize: searchModel.PageSize);

            //prepare grid model 
            var model = await new AddProductToSalesCampaignsListModel().PrepareToGridAsync(searchModel, products, () =>
            {
                return products.SelectAwait(async product =>
                {
                    var productModel = product.ToModel<ProductModel>();

                    productModel.SeName = await _urlRecordService.GetSeNameAsync(product, 0, true, false);

                    return productModel;
                });
            });

            return model;
        }
        #endregion

        #region  preview

        /// <summary>
        /// Prepare preview product price list
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the preview product price list model
        /// </returns>
        public virtual async Task<PreviewListModel> PreparePreviewListModellAsync(PreviewSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));


            //get product sale
            var previewPrices = await _salesCampaigns.GetProductPriceBySalesCampaignsIdAsync(searchModel.SalesCampaignId,
                showHidden: true,
                pageIndex: searchModel.Page - 1, pageSize: searchModel.PageSize);

            var disc = (await _salesCampaigns.GetSalesCampaignsByIdAsync(searchModel.SalesCampaignId));

            //prepare grid model 
            var model = await new PreviewListModel().PrepareToGridAsync(searchModel, previewPrices, () =>
            {

                return previewPrices.SelectAwait(async previewPrice =>
                {
                    //fill in model values from the entity
                    var product = (await _productService.GetProductByIdAsync(previewPrice.ProductId));
                    var previewModel = new PreviewModel();

                    //if old Price is Zero then only discount apply
                    if (product.OldPrice == 0 && product.Price != 0)
                    {
                        previewModel.Id = previewPrice.Id;
                        previewModel.ProductId = previewPrice.ProductId;
                        previewModel.SalesCampaignsId = previewPrice.SalesCampaignsId;
                        previewModel.Price = product.Price;                       
                        previewModel.DiscountTypeId = disc.DiscountTypeId;

                        //discount type Perctage
                        var dic = 100 - disc.DiscountValue;
                        var salePrice = product.Price;
                        if (dic > 0)
                        {
                            salePrice = product.Price * (dic / 100);
                        }

                        //discount type FixedAmount
                        var fixAmt = (product.Price - disc.DiscountValue);

                        //check Discount Type Id
                        if(previewModel.DiscountTypeId == (int)DiscountType.Percentage)
                        {
                            previewModel.SalePrice = salePrice;
                            // check Override Cents  
                            if (disc.OverrideCents == true)
                            {
                                previewModel.OverrideCentsValue = disc.OverrideCentsValue;
                                var twoDigit = Math.Truncate(salePrice);
                                previewModel.SalePrice = twoDigit + disc.OverrideCentsValue;
                            }

                        }
                        else if(previewModel.DiscountTypeId == (int)DiscountType.FixedAmount)
                        {
                            previewModel.SalePrice = fixAmt;
                        }


                        //fill in additional values (not existing in the entity)
                        previewModel.ProductName = (await _productService.GetProductByIdAsync(previewPrice.ProductId))?.Name;
                    }

                    return previewModel;
                });
            });
            return model;
        }

        #endregion
    }
}

