using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;
using Nop.Core;
using Nop.Core.Domain.Cms;
using Nop.Core.Infrastructure;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Plugins;
using Nop.Web.Framework;
using Nop.Web.Framework.Infrastructure;
using Nop.Web.Framework.Menu;

namespace Nop.Plugin.Widgets.SalesCampaigns
{
    /// <summary>
    /// Plugin
    /// </summary>
    public class SalesCampaignsPlugin : BasePlugin, IWidgetPlugin, IAdminMenuPlugin
    {
        private readonly ILocalizationService _localizationService;
        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;
        private readonly WidgetSettings _widgetSettings;

        public SalesCampaignsPlugin(ILocalizationService localizationService,
            ISettingService settingService,
            IWebHelper webHelper,
            WidgetSettings widgetSettings)
        {
            _localizationService = localizationService;
            _settingService = settingService;
            _webHelper = webHelper;
            _widgetSettings = widgetSettings;
        }

        /// <summary>
        /// Gets widget zones where this widget should be rendered
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the widget zones
        /// </returns>

        public Task<IList<string>> GetWidgetZonesAsync()
        {

            return Task.FromResult<IList<string>>(new List<string> {
               PublicWidgetZones.ProductDetailsTop,
               PublicWidgetZones.ProductDetailsAfterPictures,
               PublicWidgetZones.ProductDetailsBeforeCollateral,
               PublicWidgetZones.ProductDetailsBeforePictures,
               PublicWidgetZones.ProductDetailsBottom,
               PublicWidgetZones.ProductDetailsOverviewBottom,
               PublicWidgetZones.ProductDetailsOverviewTop,
            });            
        }

        /// <summary>
        /// Gets a configuration page URL
        /// </summary>
        public override string GetConfigurationPageUrl()
        {
            return $"{_webHelper.GetStoreLocation()}Admin/SalesCampaigns/Configure";
        }

        /// <summary>
        /// Gets a name of a view component for displaying widget
        /// </summary>
        /// <param name="widgetZone">Name of the widget zone</param>
        /// <returns>View component name</returns>
        public string GetWidgetViewComponentName(string widgetZone)
        {
   
            if (widgetZone is null)
                throw new ArgumentNullException(nameof(widgetZone));

            if (widgetZone.Equals(PublicWidgetZones.ProductDetailsTop) ||
                widgetZone.Equals(PublicWidgetZones.ProductDetailsAfterPictures) ||
                widgetZone.Equals(PublicWidgetZones.ProductDetailsBeforeCollateral) ||
                widgetZone.Equals(PublicWidgetZones.ProductDetailsBeforePictures) ||
                widgetZone.Equals(PublicWidgetZones.ProductDetailsBottom) ||
                widgetZone.Equals(PublicWidgetZones.ProductDetailsOverviewBottom) ||
                widgetZone.Equals(PublicWidgetZones.ProductDetailsOverviewTop)
                )
            {
                return SalesCampaignsDefaults.VIEW_COMPONENT;
            }

            return string.Empty;
        }

        /// <summary>
        /// Install plugin
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        public override async Task InstallAsync()
        {

            //settings
            if (!_widgetSettings.ActiveWidgetSystemNames.Contains(SalesCampaignsDefaults.SystemName))
            {
                _widgetSettings.ActiveWidgetSystemNames.Add(SalesCampaignsDefaults.SystemName);
                await _settingService.SaveSettingAsync(_widgetSettings);
            }
            var settings = new SalesCampaignsSettings
            {
                Enable = true
            };
            await _settingService.SaveSettingAsync(settings);

            await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
            {
                ["Admin.Common.Save"] = "Save",
                ["Admin.Common.View"] = "View",
                ["Admin.Common.Edit"] = "Edit",
                ["Admin.Common.Delete"] = "Delete",
                ["Admin.Common.Search"] = "Search",
                ["Plugins.Widgets.SalesCampaigns.Enable"] = "Enable",
                ["Plugins.Widgets.SalesCampaigns.Fields.Instructions"] = "\r\n <div class=\"callout bg-gray-light no-margin\">\r\n                    <p>Here is a list of all the sales campaigns you have created for your store. Please read the following items you should know about Sales Campaigns:</p>\r\n     <li>\r\n               1. If a sale is <b> scheduled </b> you do not need to click the [Start immediately] button, the sale will start at the listed start time. If the sale is <b> not scheduled </b> or you would like <br/> to manually start it you can click the [Start immediately] button and it will start immediately. You can check under the state column to see whether it is active or <br/> not. You can also stop immediately sales campaigns using the [Stop immediately] button, even if it is a timed sale.\r\n        </li>\r\n                       <li>\r\n                   2. When the campaign is scheduled or running you cannot edit it. You can only [View] what settings you have been setup.\r\n                        </li>\r\n           <li>\r\n          3. <b>Notes: </b> All times are UTC times. Current UTC time: Monday, October 10, 2022 5:08:54 AM\r\n    </li>\r\n          </ul>\r\n       </div>   \r\n            <br />",
                ["Plugins.Widgets.SalesCampaigns.Fields.Command"] = "Command",
                ["Plugins.Widgets.SalesCampaigns.Fields.Name"]="Name",
                ["Plugins.Widgets.SalesCampaigns.Fields.Name.Hint"]= "The name of the sales campaign. Internal use only.",
                ["Plugins.Widgets.SalesCampaigns.Fields.State"] = "State",
                ["Plugins.Widgets.SalesCampaigns.Fields.FromDate"] ="Frome Date",
                ["Plugins.Widgets.SalesCampaigns.Fields.FromDate.Hint"] = "The date from which the item will be displayed.",
                ["Plugins.Widgets.SalesCampaigns.Fields.ToDate"] ="To Date",
                ["Plugins.Widgets.SalesCampaigns.Fields.ToDate.Hint"] = "The date to which the item will be displayed.",
                ["Plugins.Widgets.SalesCampaigns.Fields.SchedulePattern"] = "Schedule Pattern",
                ["Plugin.Widgets.SalesCampaigns.SalesCampaignSettings"]= "Sales Campaign Settings",
                ["Plugin.Widgets.SalesCampaigns.CountdownTimer"] = "Countdown Timer",
                ["Plugins.Widgets.SalesCampaigns.Fields.AssignToCategory"] = "Add products to category",
                ["Plugins.Widgets.SalesCampaigns.Fields.AssignToCategory.Hint"] = "Specifies whether the products in this campaign will be mapped to a specific category.",
                ["Plugins.Widgets.SalesCampaigns.Fields.WidgetZone"] = "Widget Zone",
                ["Plugins.Widgets.SalesCampaigns.Fields.WidgetZone.Hint"] = "The widget zone on the product details page, where the sales campaign will appear.",
                ["Plugins.Widgets.SalesCampaigns.Fields.DiscountType"] = "Discount Type",
                ["Plugins.Widgets.SalesCampaigns.Fields.DiscountType.Hint"] = "The discount type.",
                ["Plugins.Widgets.SalesCampaigns.Fields.CategoryName"] = "Category",
                ["Plugins.Widgets.SalesCampaigns.Fields.CategoryName.Hint"] = "Choose the desired category to which the products will be mapped.",
                ["Plugins.Widgets.SalesCampaigns.Fields.DiscountValue"] = "Discount",
                ["Plugins.Widgets.SalesCampaigns.Fields.DiscountValue.Hint"] = "The discount Value.",
                ["Plugins.Widgets.SalesCampaigns.Fields.OverrideCents"] = "Override Cents",
                ["Plugins.Widgets.SalesCampaigns.Fields.OverrideCents.Hint"] = "The Override cents field is required.",
                ["Plugins.Widgets.SalesCampaigns.Fields.Selecttype"] = "Select Type",
                ["Plugin.Widgets.SalesCampaigns.Scheduling"] = "Scheduling",
                ["Plugins.Widgets.SalesCampaigns.Fields.SchedulePatternType"] = "Schedule Pattern",
                ["Plugins.Widgets.SalesCampaigns.Fields.SchedulePatternType.Hint"] = "The schedule pattern",
                ["Plugin.Widgets.SalesCampaigns.Schedule"] = "Schedule",
                ["Plugin.Widgets.SalesCampaigns.Summary"] = "Summary",
                ["Plugins.Widgets.SalesCampaigns.Fields.FromTime"] = "From Time",
                ["Plugins.Widgets.SalesCampaigns.Fields.FromTime.Hint"] = "The time from which the item will be displayed.",
                ["Plugins.Widgets.SalesCampaigns.Fields.ToTime"] = "To Time",
                ["Plugins.Widgets.SalesCampaigns.Fields.ToTime.Hint"] = "The time to which the item will be displayed.",
                ["Plugin.Widgets.SalesCampaigns.SummaryNote"] = "\r\n <div class=\"callout bg-gray-light no-margin\">\r\n      <p>Occurs {<b>everyday</b>} between {<b>no starting time</b>} and {<b>no ending time</b>}. Schedule will be used between {<b>no starting date</b>} and {<b>no ending date</b>}.<br> <b>NOTE:</b> Store time zone: (<b>UTC+01:00</b>) <b>Amsterdam, Berlin, Bern, Rome, Stockholm, Vienna</b>. Current time: <b>Tuesday</b>, <b>December 6, 2022 12:09:11 PM</b>  </p>\r\n     </div>   \r\n           <br />",
                ["Plugin.Widgets.SalesCampaigns.SummaryNotes"] = "\r\n <div class=\"callout bg-gray-light no-margin\">\r\n              <p> Occurs {<b>everyday</b>} between {<b >00:00 </b>} and {<b> 23:30 </b>}. Schedule will be used between {<b> 6/15/2019 </b>} and {<b> 6/17/2025 </b>}. </br> <b>Notes: </b> Store time zone: (<b> UTC+01:00 </b>) <b> Amsterdam, Berlin, Bern, Rome, Stockholm, Vienna.</b> Current time: <b> Tuesday, </b> <b>October 18, 2022 12:16:54 PM </b>    </p>\r\n          </div>   \r\n           <br />",
                ["Plugin.Widgets.SalesCampaigns.SummaryNotessss"] = "Occurs everyday between no starting time and no ending time. Schedule will be used between no starting date and no ending date. NOTE: Store time zone: UTC+01:00 Amsterdam, Berlin, Bern, Rome, Stockholm, Vienna. Current time: Tuesday, December 6, 2022 12:09:11 PM",
                ["Plugins.Widgets.SalesCampaigns.Category.Select"] ="Select Category",
                ["Plugins.Widgets.SalesCampaigns.Fields.ClockType"] = "Select Type",
                ["Plugins.Widgets.SalesCampaigns.View"] = "View",
                ["Plugins.Widgets.SalesCampaigns.Edit"] = "Edit",
                ["Plugins.Widgets.SalesCampaigns.Delete"] = "Delete",
                ["Plugins.Widgets.SalesCampaigns.AddNew"] = "AddNew",
                ["Plugins.Widgets.SalesCampaigns.BackToList"] = "(Back To Sales Campaigns List)",
                ["Plugins.Widgets.SalesCampaigns.Admin.Configuration.Settings"] = "Sales Campaign",
                ["Plugins.Widgets.SalesCampaigns.Fields.Action"] = "Action",
                ["Plugins.Widgets.SalesCampaigns.Fields.Action.Schedule"] = "Schedule",
                ["Plugins.Widgets.SalesCampaigns.Fields.Action.Unschedule"] = "Unschedule",
                ["Plugins.Widgets.SalesCampaigns.Fields.Action.StartImmediately"] = "Start Immediately",
                ["Plugins.Widgets.SalesCampaigns.Fields.Action.StopImmediately"] = "Stop Immediately",
                ["Plugins.Widgets.SalesCampaigns.Fields.State.Scheduled"] = "Scheduled",
                ["Plugins.Widgets.SalesCampaigns.Fields.State.Running"] = "Running",
                ["Plugins.Widgets.SalesCampaigns.Fields.State.Pending"] = "Pending",
                ["Plugins.Widgets.SalesCampaigns.Fields.OverrideProductConditions"] = "OverrideProductConditions",
                ["Plugins.Widgets.SalesCampaigns.Fields.Preview"] = "Preview",
                ["Plugins.Widgets.SalesCampaigns.Fields.SearchProductName"] = "Product Name",
                ["Plugins.Widgets.SalesCampaigns.Fields.SearchCategory"] = "Category",
                ["Plugins.Widgets.SalesCampaigns.Fields.SearchManufacturer"] = "Manufacturer",
                ["Plugins.Widgets.SalesCampaigns.Fields.SearchStore"] = "Store",
                ["Plugins.Widgets.SalesCampaigns.Fields.SearchVendor"] = "Vendor",
                ["Plugins.Widgets.SalesCampaigns.Fields.SearchProductType"] = "Product Type",
                ["Plugins.Widgets.SalesCampaigns.Fields.Product"] = "Product",
                ["Plugins.Widgets.SalesCampaigns.Fields.SalesCampaigns"] = "Sales Campaigns",
                ["Plugins.Widgets.SalesCampaigns.Fields.FeaturedProduct"] = "Featured Product",
                ["Plugins.Widgets.SalesCampaigns.Fields.IsState"] = "State",
                ["Plugins.Widgets.SalesCampaigns.Fields.Price"] = "Price",
                ["Plugins.Widgets.SalesCampaigns.Fields.SalePrice"] = "Sale Price",
                ["Enums.Nop.Plugin.Widgets.SalesCampaigns.Domain.ClockType.Format1"] = "Sale ends in # days, # hours, # minutes, # seconds",
                ["Enums.Nop.Plugin.Widgets.SalesCampaigns.Domain.ClockType.Format2"] = "Sale ends in # hours, # minutes, # seconds",
                ["Enums.Nop.Plugin.Widgets.SalesCampaigns.Domain.ClockType.Format3"] = "Sale ends in # days",
                ["Enums.Nop.Plugin.Widgets.SalesCampaigns.Domain.ClockType.Format4"] = "On Sale until Saturday, January 1, 2014, 0:00:00 AM",
                ["Enums.Nop.Plugin.Widgets.SalesCampaigns.Domain.ClockType.Format5"] = "On Sale until Saturday, January 1, 2014",
                ["Enums.Nop.Plugin.Widgets.SalesCampaigns.Domain.ClockType.Format6"] = "On Sale until Jan 1, 2014, 0:00:00 AM ",
                ["Enums.Nop.Plugin.Widgets.SalesCampaigns.Domain.ClockType.Format7"] = "On Sale until Jan 1, 2014",
                ["Plugins.Widgets.SalesCampaigns.Categories.Products.AddNew"] = "Add a new product",
                ["Plugins.Widgets.SalesCampaigns.Categories.Products.SaveBeforeEdit"] = "You need to create sale Campagin before you can add products for this page.",
                ["Plugins.Widgets.SalesCampaigns.Validation.AlreadyExists"] ="This product is already aline with widget, Kindly choose another widget.",
                ["Plugins.Widgets.SalesCampaigns.Menu.Settings"]= "Configuration",
                ["Plugins.Widgets.SalesCampaigns.ManageSalesCampaign"] = "Sales Campaigns",
                ["Plugins.Widgets.SalesCampaigns.Admin.Manage.Settings"] = "Manage Sales Campaigns",
                ["Plugins.Widgets.SalesCampaigns.Fields.Preview.Note"] = "In this only those discount product price preview which has not Old Price.",
                ["Plugins.Widgets.SalesCampaigns.Fields.Name.Required"] = "The sales campaign name is required.",
                ["Plugins.Widgets.SalesCampaigns.Fields.DiscountValue.Required"] = "Discount Value must not be empty.",
                ["Plugins.Widgets.SalesCampaigns.Fields.DiscountValue.GreaterThan.Required"] = "The discount value must be greater than 0.",
                ["Plugins.Widgets.SalesCampaigns.Fields.OverrideCentsValue"] = "Override Cents Value",
                ["Plugins.Widgets.SalesCampaigns.Fields.OverrideCentsValue.Hint"] = "Override the cents on the calculated price."

            });

            await base.InstallAsync();
        }

        /// <summary>
        /// Uninstall plugin
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        public override async Task UninstallAsync()
        {
            //settings
            await _settingService.DeleteSettingAsync<SalesCampaignsSettings>();
            if (!_widgetSettings.ActiveWidgetSystemNames.Contains(SalesCampaignsDefaults.SystemName))
            {
                _widgetSettings.ActiveWidgetSystemNames.Remove(SalesCampaignsDefaults.SystemName);
                await _settingService.SaveSettingAsync(_widgetSettings);
            }

            //locales
            await _localizationService.DeleteLocaleResourcesAsync("Plugins.Widgets.SalesCampaignsSettings");

            await base.UninstallAsync();
        }

        /// <summary>
        /// Gets a value indicating whether to hide this plugin on the widget list page in the admin area
        /// </summary>
        public bool HideInWidgetList => false;

        /// <summary>
        /// Create SiteMap
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task ManageSiteMapAsync(SiteMapNode rootNode)
        {
            var menuItem = new SiteMapNode()
            {
                SystemName = "Manage SalesCampaign",
                Title = await _localizationService.GetResourceAsync("Plugins.Widgets.SalesCampaigns.ManageSalesCampaign"),
                ControllerName = "",
                ActionName = "#",
                IconClass = "far fa-dot-circle",
                Visible = true,
                RouteValues = new RouteValueDictionary() { { "area", AreaNames.Admin } },
            };
            var SubMenuItem = new SiteMapNode()
            {
                SystemName = "SalesCampaigns.Settings",
                Title = await _localizationService.GetResourceAsync("Plugins.Widgets.SalesCampaigns.Menu.Settings"),
                ControllerName = "SalesCampaigns",
                ActionName = "Configure",
                IconClass = "far fa-dot-circle",
                Visible = true,
                RouteValues = new RouteValueDictionary() { { "area", AreaNames.Admin } },
            };
            var SubMenuItema = new SiteMapNode()
            {
                SystemName = "SalesCampaigns.List",
                Title = await _localizationService.GetResourceAsync("Plugins.Widgets.SalesCampaigns.Admin.Manage.Settings"),
                ControllerName = "SalesCampaigns",
                ActionName = "List",
                IconClass = "far fa-dot-circle",
                Visible = true,
                RouteValues = new RouteValueDictionary() { { "area", AreaNames.Admin } },
            };
            var pluginNode = rootNode.ChildNodes.FirstOrDefault(x => x.SystemName == "Third party plugins");
            if (pluginNode != null)
            {
                pluginNode.ChildNodes.Add(menuItem);
                menuItem.ChildNodes.Add(SubMenuItem);
                menuItem.ChildNodes.Add(SubMenuItema);
            }
            else
            {
                rootNode.ChildNodes.Add(SubMenuItem);
            }
        }
    }
}