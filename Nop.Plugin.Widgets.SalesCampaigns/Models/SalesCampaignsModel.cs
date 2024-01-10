using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nop.Plugin.Widgets.SalesCampaigns.Models
{
    public record SalesCampaignsModel : BaseNopEntityModel
    {
        public SalesCampaignsModel()
        {           
            AvailableDiscountType = new List<SelectListItem>();
            AvailableDiscountType = new List<SelectListItem>();
            SalesCampaignsProductSearchModel = new SalesCampaignsProductSearchModel();
            PreviewSearchModel = new PreviewSearchModel();
            AvailableCategories = new List<SelectListItem>();
        }

        #region Properties In Use

        public int CategoryId { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.SalesCampaigns.Fields.FromTime")]      
        public DateTime FromTime { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.SalesCampaigns.Fields.ToTime")]        
        public DateTime ToTime { get; set; }

        //creation date
        [NopResourceDisplayName("Plugins.Widgets.SalesCampaigns.Fields.FromDate")]
        [UIHint("DateNullable")]
        public DateTime? FromDate { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.SalesCampaigns.Fields.ToDate")]
        [UIHint("DateNullable")]
        public DateTime? ToDate { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.SalesCampaigns.Fields.Name")]
        [Required(ErrorMessage = "The sales campaign name is required.")]
        public string Name { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.SalesCampaigns.Fields.State")]
        public string State { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.SalesCampaigns.Fields.Schedule Pattern")]
        public string SchedulePattern { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.SalesCampaigns.Fields.Action")]
        public string Action { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.SalesCampaigns.Fields.ActionType")]
        public int ActionTypeId { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.SalesCampaigns.Fields.Command")]
        public bool Command { get; set; }

        #region comment category part

        [NopResourceDisplayName("Plugins.Widgets.SalesCampaigns.Fields.AssignToCategory")]
        public bool AssignToCategory { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.SalesCampaigns.Fields.CategoryName")]
        public string CategoryName { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.SalesCampaigns.Fields.SearchCategory")]
        public IList<SelectListItem> AvailableCategories { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.SalesCampaigns.Fields.CategoryName")]
        public int SearchCategoryId { get; set; }

        #endregion

        [NopResourceDisplayName("Plugins.Widgets.SalesCampaigns.Fields.WidgetZone")]
        public int WidgetZoneId { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.SalesCampaigns.Fields.SearchWidgetZone")]
        public IList<SelectListItem> AvailableWidgetZones { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.SalesCampaigns.Fields.DiscountType")]
        public int DiscountTypeId { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.SalesCampaigns.Fields.SearchDiscountType")]
        public IList<SelectListItem> AvailableDiscountType { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.SalesCampaigns.Fields.OverrideCents")]
        public bool OverrideCents { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.SalesCampaigns.Fields.OverrideCentsValue")]
        [Range(0.0, 0.99)]
        [RegularExpression(@"^\d+(.\d{0,2})?$")]
        public decimal OverrideCentsValue { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.SalesCampaigns.Fields.DiscountValue")]
        [Required(ErrorMessage = "Discount Value must not be empty.")]
        public decimal DiscountValue { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.SalesCampaigns.Fields.SchedulePatternType")]
        public int SchedulePatternTypeId { get; set; }
       
        [NopResourceDisplayName("Plugins.Widgets.SalesCampaigns.Fields.ClockType")]
        public int ClockTypeId { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.SalesCampaigns.Fields.State")]
        public int CampaignStateId { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.SalesCampaigns.Fields.Price")]
        public decimal Price { get; set; }
        public SalesCampaignsProductSearchModel SalesCampaignsProductSearchModel { get; set; }

        public PreviewSearchModel PreviewSearchModel { get; set; }
        
        #endregion
    }
}
