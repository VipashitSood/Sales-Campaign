using Nop.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.SalesCampaigns.Domain
{
    /// <summary>
    /// Represents a product sales campaigns mapping
    /// </summary>
    public partial class SalesCampaignsProductMapping : BaseEntity
    {
        /// <summary>
        /// Gets or sets the product identifier
        /// </summary>
        public int ProductId { get; set; }
        /// <summary>
        /// Gets or sets the salescampaigns identifier
        /// </summary>
        public int SalesCampaignsId { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the product is state
        /// </summary>
        public bool IsState { get; set; }
        /// <summary>
        /// Gets or sets the price identifier
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// Gets or sets the sale price identifier
        /// </summary>
        public decimal SalePrice { get; set; }
    }
}
