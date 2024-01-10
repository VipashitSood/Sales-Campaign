using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Directory;
using Nop.Core.Domain.Discounts;
using Nop.Services.Catalog;
using Nop.Services.Configuration;
using Nop.Services.Customers;
using Nop.Services.Directory;
using Nop.Services.Discounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using Nop.Plugin.Widgets.SalesCampaigns.Domain;

namespace Nop.Plugin.Widgets.SalesCampaigns.Services
{
    public partial class OverridePriceCalculationService : PriceCalculationService
    {
        #region Fields

        private readonly CatalogSettings _catalogSettings;
        private readonly CurrencySettings _currencySettings;
        private readonly ICategoryService _categoryService;
        private readonly ICurrencyService _currencyService;
        private readonly ICustomerService _customerService;
        private readonly IDiscountService _discountService;
        private readonly IManufacturerService _manufacturerService;
        private readonly IProductAttributeParser _productAttributeParser;
        private readonly IProductService _productService;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly IStoreContext _storeContext;
        private readonly ISalesCampaignsService _salesCampaigns;
        private readonly ISettingService _settingService;
        #endregion

        #region Ctor

        public OverridePriceCalculationService(CatalogSettings catalogSettings,
            CurrencySettings currencySettings,
            ICategoryService categoryService,
            ICurrencyService currencyService,
            ICustomerService customerService,
            IDiscountService discountService,
            IManufacturerService manufacturerService,
            IProductAttributeParser productAttributeParser,
            IProductService productService,
            IStaticCacheManager staticCacheManager,
            IStoreContext storeContext,
            ISalesCampaignsService salesCampaigns,
            ISettingService settingService)
            : base(catalogSettings,
             currencySettings,
             categoryService,
             currencyService,
             customerService,
             discountService,
             manufacturerService,
             productAttributeParser,
             productService,
             staticCacheManager,
             storeContext)
        {
            _catalogSettings = catalogSettings;
            _currencySettings = currencySettings;
            _categoryService = categoryService;
            _currencyService = currencyService;
            _customerService = customerService;
            _discountService = discountService;
            _manufacturerService = manufacturerService;
            _productAttributeParser = productAttributeParser;
            _productService = productService;
            _staticCacheManager = staticCacheManager;
            _storeContext = storeContext;
            _salesCampaigns = salesCampaigns;
            _settingService = settingService;
        }

        #endregion

        #region Final Price

        /// <summary>
        /// Gets the final price
        /// </summary>
        /// <param name="product">Product</param>
        /// <param name="customer">The customer</param>
        /// <param name="overriddenProductPrice">Overridden product price. If specified, then it'll be used instead of a product price. For example, used with product attribute combinations</param>
        /// <param name="additionalCharge">Additional charge</param>
        /// <param name="includeDiscounts">A value indicating whether include discounts or not for final price computation</param>
        /// <param name="quantity">Shopping cart item quantity</param>
        /// <param name="rentalStartDate">Rental period start date (for rental products)</param>
        /// <param name="rentalEndDate">Rental period end date (for rental products)</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the final price without discounts, Final price, Applied discount amount, Applied discounts
        /// </returns>
        public override async Task<(decimal priceWithoutDiscounts, decimal finalPrice, decimal appliedDiscountAmount, List<Discount> appliedDiscounts)> GetFinalPriceAsync(Product product,
            Customer customer,
            decimal? overriddenProductPrice,
            decimal additionalCharge,
            bool includeDiscounts,
            int quantity,
            DateTime? rentalStartDate,
            DateTime? rentalEndDate)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            var store = await _storeContext.GetCurrentStoreAsync();
            var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(NopCatalogDefaults.ProductPriceCacheKey,
                product,
                overriddenProductPrice,
                additionalCharge,
                includeDiscounts,
                quantity,
                await _customerService.GetCustomerRoleIdsAsync(customer),
                store);

            //we do not cache price if this not allowed by settings or if the product is rental product
            //otherwise, it can cause memory leaks (to store all possible date period combinations)
            if (!_catalogSettings.CacheProductPrices || product.IsRental)
                cacheKey.CacheTime = 0;

            decimal rezPrice;
            decimal rezPriceWithoutDiscount;
            decimal discountAmount;
            List<Discount> appliedDiscounts;

            (rezPriceWithoutDiscount, rezPrice, discountAmount, appliedDiscounts) = await _staticCacheManager.GetAsync(cacheKey, async () =>
            {
                var discounts = new List<Discount>();
                var appliedDiscountAmount = decimal.Zero;

                //initial price
                var price = overriddenProductPrice ?? product.Price;

                //tier prices
                var tierPrice = await _productService.GetPreferredTierPriceAsync(product, customer, store.Id, quantity);
                if (tierPrice != null)
                    price = tierPrice.Price;

                //additional charge
                price += additionalCharge;

                //rental products
                if (product.IsRental)
                    if (rentalStartDate.HasValue && rentalEndDate.HasValue)
                        price *= _productService.GetRentalPeriods(product, rentalStartDate.Value, rentalEndDate.Value);

                var priceWithoutDiscount = price;

                if (includeDiscounts)
                {
                    //discount
                    var (tmpDiscountAmount, tmpAppliedDiscounts) = await GetDiscountAmountAsync(product, customer, price);
                    price -= tmpDiscountAmount;

                    if (tmpAppliedDiscounts?.Any() ?? false)
                    {
                        discounts.AddRange(tmpAppliedDiscounts);
                        appliedDiscountAmount = tmpDiscountAmount;
                    }


                    //sale campagin discount price
                    if (product.OldPrice <= decimal.Zero)
                    {
                        //check of enable & disable button
                        var store = await _storeContext.GetCurrentStoreAsync();
                        var settings = await _settingService.LoadSettingAsync<SalesCampaignsSettings>(store.Id);
                        if (settings.Enable)
                        {
                            var activeCampaign = await _salesCampaigns.GetPublicSaleCampaignByProductIdAsyn(product.Id);
                            if (activeCampaign != null)
                            {                               
                                //check for perctage type 
                                if (activeCampaign.DiscountTypeId == (int)Nop.Plugin.Widgets.SalesCampaigns.Domain.DiscountType.Percentage)
                                {
                                    var result = (decimal)((float)product.Price * (float)activeCampaign.DiscountValue / 100f);
                                    price -= result;

                                    // check Override Cents       
                                    if (activeCampaign.OverrideCents == true)
                                    {
                                        var overrideCent = Math.Truncate(price);
                                        price = overrideCent + activeCampaign.OverrideCentsValue;
                                    }

                                }
                                //discount type FixedAmount
                                else if (activeCampaign.DiscountTypeId == (int)Nop.Plugin.Widgets.SalesCampaigns.Domain.DiscountType.FixedAmount)
                                {
                                    //var fixAmt = product.Price - activeCampaign.DiscountValue;
                                    var fixAmt = price - activeCampaign.DiscountValue;
                                    price = fixAmt;
                                }                             
                            }
                        }
                    }

                }

                if (price < decimal.Zero)
                    price = decimal.Zero;

                if (priceWithoutDiscount < decimal.Zero)
                    priceWithoutDiscount = decimal.Zero;

                return (priceWithoutDiscount, price, appliedDiscountAmount, discounts);
            });

            return (rezPriceWithoutDiscount, rezPrice, discountAmount, appliedDiscounts);
        }

        #endregion
    }
}
