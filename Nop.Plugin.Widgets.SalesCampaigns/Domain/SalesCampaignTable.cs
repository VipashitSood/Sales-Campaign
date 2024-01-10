using Nop.Core;
using Nop.Web.Framework.Mvc.ModelBinding;
using System;
using System.ComponentModel.DataAnnotations;


namespace Nop.Plugin.Widgets.SalesCampaigns.Domain
{
    public partial class SalesCampaignTable : BaseEntity
    {
        #region  Sales Campaigns Setting

        /// <summary>
        /// Gets or sets the Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Assign To Category
        /// </summary>
        public bool AssignToCategory { get; set; }

        /// <summary>
        /// Gets or sets the Category Identifier
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the Widget Zone Identifier
        /// </summary>
        public int WidgetZoneId { get; set; }

        /// <summary>
        /// Gets or sets the Discount Type Identifier
        /// </summary>
        public int DiscountTypeId { get; set; }
        /// <summary>
        /// Gets or sets the Discount Value
        /// </summary>
        public decimal DiscountValue { get; set; }

        /// <summary>
        /// Gets or sets the OverrideCents 
        /// </summary>
        public bool OverrideCents { get; set; }

        /// <summary>
        /// Gets or sets the Campaign State Identifier
        /// </summary>
        public int CampaignStateId { get; set; }

        /// <summary>
        /// Gets or sets the Override Cents Value
        /// </summary>

        [Range(0.1, 0.99)]
        [RegularExpression(@"^\d+(.\d{0,2})?$")]
        public decimal OverrideCentsValue { get; set; }

        #endregion

        #region Countdown Timer

        /// <summary>
        /// Gets or sets the Clock Type Identifier
        /// </summary>
        public int ClockTypeId { get; set; }
        #endregion

        #region Scheduling

        /// <summary>
        /// Gets or sets the From Date
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [UIHint("DateNullable")]
        public DateTime FromDate { get; set; }

        /// <summary>
        /// Gets or sets the To Date
        /// </summary>
        [UIHint("DateNullable")]
        public DateTime ToDate { get; set; }

        /// <summary>
        /// Gets or sets the Schedule Pattern Type Identifier
        /// </summary>
        public int SchedulePatternTypeId { get; set; }

        /// <summary>
        /// Gets or sets the From Time
        /// </summary>
        [UIHint("TimeNullable")]
        public DateTime FromTime { get; set; }

        /// <summary>
        /// Gets or sets the To Time
        /// </summary>
        [UIHint("TimeNullable")]
        public DateTime ToTime { get; set; }
        #endregion

        #region Price
        /// <summary>
        /// Gets or sets the Price
        /// </summary>
        public decimal Price { get; set; }
        #endregion
    }
}
