using Nop.Core;

namespace Nop.Plugin.Widgets.SalesCampaigns
{
    /// <summary>
    /// Represents plugin default vaues and constants
    /// </summary>
    public class SalesCampaignsDefaults
    {
        /// <summary>
        /// Gets the plugin system name
        /// </summary>
        public static string SystemName => "Widgets.SalesCampaigns";
        /// <summary>
        /// Gets the configuration route name
        /// </summary>
        public static string ConfigurationRouteName => "Plugin.Widgets.SalesCampaigns.Configure";
        /// <summary>
        /// Gets the name of the view component to place a widget into pages
        /// </summary>       
        public const string VIEW_COMPONENT = "WidgetsSalesCampaigns";

        /// <summary>
        /// Gets a name of the view component to embed tracking script on pages
        /// </summary>
        public const string TRACKING_VIEW_COMPONENT_NAME = "WidgetsSalesCampaigns";

        /// <summary>
        /// Name of the view component to display widget in public store (order summary)
        /// </summary>
        public const string SALESCAMPAGINS_PUBLIC_VIEW_COMPONENT = "SalesCampaginsPublic";

    }
}