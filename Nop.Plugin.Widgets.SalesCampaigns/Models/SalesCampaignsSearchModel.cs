using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using System.Collections.Generic;

namespace Nop.Plugin.Widgets.SalesCampaigns.Models
{
    public record SalesCampaignsSearchModel : BaseSearchModel
    {
        public SalesCampaignsSearchModel()
        {
            AvailableCategories = new List<SelectListItem>();
        }      

        [NopResourceDisplayName("Plugins.Widgets.SalesCampaigns.Fields.CategoryName")]
        public int SearchCategoryId { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.SalesCampaigns.Fields.CategoryName")]
        public IList<SelectListItem> AvailableCategories { get; set; }

    }
}
