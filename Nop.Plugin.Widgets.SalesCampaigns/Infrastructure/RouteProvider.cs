using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.SalesCampaigns.Infrastructure
{
    /// <summary>
    /// Represents the plugin route provider
    /// </summary>
    public class RouteProvider : IRouteProvider
    {
        /// <summary>
        /// Gets a priority of route provider
        /// </summary>
        public int Priority => int.MaxValue;

        /// <summary>
        /// Register routes
        /// </summary>
        /// <param name="endpointRouteBuilder">Route builder</param>

        public void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapControllerRoute(SalesCampaignsDefaults.ConfigurationRouteName,
                "Plugins/SalesCampaigns/Configure",
                new { controller = "SalesCampaigns", action = "Configure", area = AreaNames.Admin });
        }      
    }
}
