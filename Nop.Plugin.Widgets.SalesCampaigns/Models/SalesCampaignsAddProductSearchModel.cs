using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Widgets.SalesCampaigns.Models
{
    /// <summary>
    /// Represents a product search model to add to the category
    /// </summary>
    public partial record SalesCampaignsAddProductSearchModel : BaseSearchModel
    {
        #region Ctor

        public SalesCampaignsAddProductSearchModel()
        {
            AvailableCategories = new List<SelectListItem>();
            AvailableManufacturers = new List<SelectListItem>();
            AvailableStores = new List<SelectListItem>();
            AvailableVendors = new List<SelectListItem>();
            AvailableProductTypes = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [NopResourceDisplayName("Plugins.Widgets.SalesCampaigns.Fields.SearchProductName")]
        public string SearchProductName { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.SalesCampaigns.Fields.SearchCategory")]
        public int SearchCategoryId { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.SalesCampaigns.Fields.SearchManufacturer")]
        public int SearchManufacturerId { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.SalesCampaigns.Fields.SearchStore")]
        public int SearchStoreId { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.SalesCampaigns.Fields.SearchVendor")]
        public int SearchVendorId { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.SalesCampaigns.Fields.SearchProductType")]
        public int SearchProductTypeId { get; set; }

        public IList<SelectListItem> AvailableCategories { get; set; }

        public IList<SelectListItem> AvailableManufacturers { get; set; }

        public IList<SelectListItem> AvailableStores { get; set; }

        public IList<SelectListItem> AvailableVendors { get; set; }

        public IList<SelectListItem> AvailableProductTypes { get; set; }

        #endregion
    }
}