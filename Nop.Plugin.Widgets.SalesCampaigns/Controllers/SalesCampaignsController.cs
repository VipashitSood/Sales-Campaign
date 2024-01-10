using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Cms;
using Nop.Plugin.Widgets.SalesCampaigns.Domain;
using Nop.Plugin.Widgets.SalesCampaigns.Factories;
using Nop.Plugin.Widgets.SalesCampaigns.Models;
using Nop.Plugin.Widgets.SalesCampaigns.Services;
using Nop.Services.Catalog;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc;
using Nop.Web.Framework.Mvc.Filters;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace Nop.Plugin.Widgets.SalesCampaigns.Controllers
{
    [AuthorizeAdmin]
    [Area(AreaNames.Admin)]
    [AutoValidateAntiforgeryToken]
    public class SalesCampaignsController : BasePluginController
    {
        #region Fields
        private readonly ILocalizationService _localizationService;
        private readonly INotificationService _notificationService;
        private readonly IPermissionService _permissionService;
        private readonly IPictureService _pictureService;
        private readonly ISalesCampaignsModelFactory _salesCampaignsModelFactory;
        private readonly ISalesCampaignsService _salesCampaignsService;
        private readonly ISettingService _settingService;
        private readonly IStoreContext _storeContext;
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly ISalesCampaignsService _salesCampaigns;
        #endregion

        #region Ctor
        public SalesCampaignsController(ILocalizationService localizationService,
            INotificationService notificationService,
            IPermissionService permissionService,
            IPictureService pictureService,
            ISalesCampaignsModelFactory salesCampaignsModelFactory,
            ISalesCampaignsService salesCampaignsService,
            ISettingService settingService,
            IStoreContext storeContext,
            ICategoryService categoryService,
            IProductService productService,
            ISalesCampaignsService salesCampaigns)
        {
            _localizationService = localizationService;
            _notificationService = notificationService;
            _permissionService = permissionService;
            _pictureService = pictureService;
            _salesCampaignsModelFactory = salesCampaignsModelFactory;
            _salesCampaignsService = salesCampaignsService;
            _settingService = settingService;
            _storeContext = storeContext;
            _categoryService = categoryService;
            _productService = productService;
            _salesCampaigns = salesCampaigns;
        }

        #endregion

        #region Configure
        public async Task<IActionResult> Configure()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            var storeId = await _storeContext.GetActiveStoreScopeConfigurationAsync();
            var settings = await _settingService.LoadSettingAsync<SalesCampaignsSettings>(storeId);
            var widgetSettings = await _settingService.LoadSettingAsync<WidgetSettings>(storeId);

            var model = new ConfigurationModel
            {
                Enable = settings.Enable,
                ActiveStoreScopeConfiguration = storeId
            };

            if (storeId > 0)
            {
                model.Enable_OverrideForStore = await _settingService.SettingExistsAsync(widgetSettings, setting => setting.ActiveWidgetSystemNames, storeId);
            }

            return View("~/Plugins/Widgets.SalesCampaigns/Views/Admin/Configure.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> Configure(ConfigurationModel model)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();


            var storeId = await _storeContext.GetActiveStoreScopeConfigurationAsync();
            var settings = await _settingService.LoadSettingAsync<SalesCampaignsSettings>(storeId);
            var widgetSettings = await _settingService.LoadSettingAsync<WidgetSettings>(storeId);

            if (!ModelState.IsValid)
                return await Configure();


            settings.Enable = model.Enable;
            await _settingService.SaveSettingAsync(settings);


            if (model.Enable && !widgetSettings.ActiveWidgetSystemNames.Contains(SalesCampaignsDefaults.SystemName))
                widgetSettings.ActiveWidgetSystemNames.Add(SalesCampaignsDefaults.SystemName);
            if (!model.Enable && widgetSettings.ActiveWidgetSystemNames.Contains(SalesCampaignsDefaults.SystemName))
                widgetSettings.ActiveWidgetSystemNames.Remove(SalesCampaignsDefaults.SystemName);
            await _settingService.SaveSettingOverridablePerStoreAsync(widgetSettings, setting => setting.ActiveWidgetSystemNames, model.Enable_OverrideForStore, storeId, false);

            await _settingService.ClearCacheAsync();

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Plugins.Saved"));

            return await Configure();
        }
        #endregion

        #region List/Create/Edit/Delete/View
        public async Task<IActionResult> List()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            // prepare model
            var model = await _salesCampaignsModelFactory.PrepareSalesCampaignsSearchModelAsync(new SalesCampaignsSearchModel());
            return View("~/Plugins/Widgets.SalesCampaigns/Views/Admin/List.cshtml", model);

        }

        [HttpPost]
        public virtual async Task<IActionResult> List(SalesCampaignsSearchModel searchModel)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            //prepare model
            var model = await _salesCampaignsModelFactory.PrepareSalesCampaignsListsModelAsync(searchModel);
            return Json(model);
        }

        public virtual async Task<IActionResult> Create()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            //prepare model
            var model = await _salesCampaignsModelFactory.PrepareSalesCampaignsModelAsync(new SalesCampaignsModel(), null);
            return View("~/Plugins/Widgets.SalesCampaigns/Views/Admin/Create.cshtml", model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> Create(SalesCampaignsModel model, bool continueEditing)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {             
                var salesCampaigns = new SalesCampaignTable();
                salesCampaigns.Id = model.Id;
                salesCampaigns.Name = model.Name;
                #region  Comment Category part 
                //salesCampaigns.AssignToCategory = model.AssignToCategory;
                //salesCampaigns.CategoryId = model.SearchCategoryId;
                #endregion
                salesCampaigns.WidgetZoneId = model.WidgetZoneId;
                salesCampaigns.DiscountTypeId = model.DiscountTypeId;
                salesCampaigns.DiscountValue = model.DiscountValue;
                salesCampaigns.OverrideCents = model.OverrideCents;
                salesCampaigns.OverrideCentsValue = model.OverrideCentsValue;
                //extra columns
                salesCampaigns.ClockTypeId = model.ClockTypeId;
                salesCampaigns.FromDate = Convert.ToDateTime(model.FromDate);
                salesCampaigns.ToDate = Convert.ToDateTime(model.ToDate);
                salesCampaigns.FromTime = Convert.ToDateTime(model.FromTime);
                salesCampaigns.ToTime = model.ToTime;
                if(salesCampaigns.SchedulePatternTypeId == 0)
                {
                    salesCampaigns.SchedulePatternTypeId = (int)SchedulePatternType.Everyday;
                }
                else
                {
                    salesCampaigns.SchedulePatternTypeId = model.SchedulePatternTypeId;
                }
               
                salesCampaigns.Price = model.Price;

                //set default values for the new model
                if (salesCampaigns.CampaignStateId == 0)
                {
                    salesCampaigns.CampaignStateId = (int)CampaignStateType.Pending;  //assign value
                }

                await _salesCampaignsService.InsertSalesCampaignsAsync(salesCampaigns);


                if (!continueEditing)
                    return RedirectToAction("List");

                return RedirectToAction("Edit", new { id = salesCampaigns.Id });

            }
            //prepare model
            model = await _salesCampaignsModelFactory.PrepareSalesCampaignsModelAsync(model, null);
            return View("~/Plugins/Widgets.SalesCampaigns/Views/Admin/Create.cshtml", model);
        }

        public virtual async Task<IActionResult> Edit(int id)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            //try to get a salesCampaigns with the specified id
            var salesCampaigns = await _salesCampaignsService.GetSalesCampaignsByIdAsync(id);
            if (salesCampaigns == null)
                return RedirectToAction("List");

            //prepare model
            var model = await _salesCampaignsModelFactory.PrepareSalesCampaignsModelAsync(null, salesCampaigns);
            return View("~/Plugins/Widgets.SalesCampaigns/Views/Admin/Edit.cshtml", model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> Edit(SalesCampaignsModel model, bool continueEditing)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            //try to get a salesCampaigns with the specified id
            var salesCampaigns = await _salesCampaignsService.GetSalesCampaignsByIdAsync(model.Id);
            if (salesCampaigns == null)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                salesCampaigns = new SalesCampaignTable();
                salesCampaigns.Id = model.Id;
                salesCampaigns.Name = model.Name;
                #region Comment Category Part 
                //salesCampaigns.CategoryId = model.SearchCategoryId;
                //salesCampaigns.AssignToCategory = model.AssignToCategory;
                #endregion
                salesCampaigns.WidgetZoneId = model.WidgetZoneId;
                salesCampaigns.DiscountTypeId = model.DiscountTypeId;
                salesCampaigns.DiscountValue = model.DiscountValue;
                salesCampaigns.OverrideCents = model.OverrideCents;
                salesCampaigns.OverrideCentsValue = model.OverrideCentsValue;
                //extra columns
                salesCampaigns.ClockTypeId = model.ClockTypeId;
                salesCampaigns.FromDate = Convert.ToDateTime(model.FromDate);
                salesCampaigns.ToDate = Convert.ToDateTime(model.ToDate);
                salesCampaigns.FromTime = Convert.ToDateTime(model.FromTime);
                salesCampaigns.ToTime = model.ToTime;
                salesCampaigns.SchedulePatternTypeId = model.SchedulePatternTypeId;
                salesCampaigns.Price = model.Price;
                //set default values for the new model
                if (model != null)
                {
                    salesCampaigns.CampaignStateId = (int)CampaignStateType.Pending;  //assign value
                }

                await _salesCampaignsService.UpdateSalesCampaignsAsync(salesCampaigns);

                if (!continueEditing)
                    return RedirectToAction("List");

                return RedirectToAction("Edit", new { id = salesCampaigns.Id });
            }

            //prepare model
            model = await _salesCampaignsModelFactory.PrepareSalesCampaignsModelAsync(model, null, true);
            return View("~/Plugins/Widgets.SalesCampaigns/Views/Admin/Edit.cshtml", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> Delete(int id)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            //try to get a product with the specified id
            var salesCampaigns = await _salesCampaignsService.GetSalesCampaignsByIdAsync(id);
            if (salesCampaigns == null)
                return RedirectToAction("List");

            await _salesCampaignsService.DeleteSalesCampaignsAsync(salesCampaigns);
            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Plugins.Widgets.SalesCampaigns.Delete"));
            return RedirectToAction("List");

        }

        [HttpPost]
        public virtual async Task<IActionResult> InlineDelete(int id)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            //try to get a product with the specified id
            var banner = await _salesCampaignsService.GetSalesCampaignsByIdAsync(id);
            if (banner == null)
                return RedirectToAction("List");

            await _salesCampaignsService.DeleteSalesCampaignsAsync(banner);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Plugins.Widgets.SalesCampaigns.Delete"));

            return new NullJsonResult();
        }
        //change view name
        public virtual async Task<IActionResult> ViewSalesCampaigns(int id)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            //try to get a salesCampaigns with the specified id
            var salesCampaigns = await _salesCampaignsService.GetSalesCampaignsByIdAsync(id);
            if (salesCampaigns == null)
                return RedirectToAction("List");

            //prepare model
            var model = await _salesCampaignsModelFactory.PrepareSalesCampaignsModelAsync(null, salesCampaigns);
            return View("~/Plugins/Widgets.SalesCampaigns/Views/Admin/View.cshtml", model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> ViewSalesCampaigns(SalesCampaignsModel model, bool continueEditing)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            //try to get a salesCampaigns with the specified id
            var salesCampaigns = await _salesCampaignsService.GetSalesCampaignsByIdAsync(model.Id);
            if (salesCampaigns == null)
                return RedirectToAction("List");

            if (ModelState.IsValid)
                {
                    salesCampaigns = new SalesCampaignTable();
                    salesCampaigns.Id = model.Id;
                    salesCampaigns.Name = model.Name;

                    //comment category part
                    //salesCampaigns.CategoryId = model.SearchCategoryId;
                    salesCampaigns.WidgetZoneId = model.WidgetZoneId;
                    salesCampaigns.DiscountTypeId = model.DiscountTypeId;
                    salesCampaigns.DiscountValue = model.DiscountValue;
                    salesCampaigns.OverrideCentsValue = model.OverrideCentsValue;
                    //extra columns
                    salesCampaigns.ClockTypeId = model.ClockTypeId;
                    salesCampaigns.FromDate = Convert.ToDateTime(model.FromDate);
                    salesCampaigns.ToDate = Convert.ToDateTime(model.ToDate);
                    salesCampaigns.FromTime = Convert.ToDateTime(model.FromTime);
                    salesCampaigns.ToTime = model.ToTime;
                    salesCampaigns.SchedulePatternTypeId = model.SchedulePatternTypeId;

                    await _salesCampaignsService.UpdateSalesCampaignsAsync(salesCampaigns);

                    if (!continueEditing)
                        return RedirectToAction("List");

                    return RedirectToAction("View", new { id = salesCampaigns.Id });
                }
           

            //prepare model
            model = await _salesCampaignsModelFactory.PrepareSalesCampaignsModelAsync(model, null, true);
            return View("~/Plugins/Widgets.SalesCampaigns/Views/Admin/View.cshtml", model);
        }

        #endregion

        #region Actions Buttons
        public virtual async Task<IActionResult> Schedule(int id)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            //try to get a salesCampaigns with the specified id
            var salesCampaigns = await _salesCampaignsService.GetSalesCampaignsByIdAsync(id);

            if(salesCampaigns == null)
                throw new ArgumentException("No Campaign found with the specified id");

            if (salesCampaigns.CampaignStateId == (int)CampaignStateType.Pending)
            {
                salesCampaigns.CampaignStateId = (int)CampaignStateType.Scheduled;
            }
            else if (salesCampaigns.CampaignStateId == (int)CampaignStateType.Scheduled)
            {
                salesCampaigns.CampaignStateId = (int)CampaignStateType.Pending;
            }

            await _salesCampaignsService.UpdateSalesCampaignsAsync(salesCampaigns);

            return RedirectToAction("List");
        }

        public virtual async Task<IActionResult> StartImmediately(int id)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            //try to get a salesCampaigns with the specified id
            var salesCampaigns = await _salesCampaignsService.GetSalesCampaignsByIdAsync(id);

            if (salesCampaigns == null)
                throw new ArgumentException("No Campaign found with the specified id");

            if (salesCampaigns.CampaignStateId == (int)CampaignStateType.Pending)
            {
                salesCampaigns.CampaignStateId = (int)CampaignStateType.Running;
            }
            else if (salesCampaigns.CampaignStateId == (int)CampaignStateType.Running)
            {
                salesCampaigns.CampaignStateId = (int)CampaignStateType.Pending;
            }

            await _salesCampaignsService.UpdateSalesCampaignsAsync(salesCampaigns);

            return RedirectToAction("List");
        }
        #endregion

        #region Override Product Condition
        [HttpPost]
        public virtual async Task<IActionResult> SalesCampaignsProductList(SalesCampaignsProductSearchModel searchModel)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return await AccessDeniedDataTablesJson();

            //try to get a category with the specified id
            var sale = await _salesCampaigns.GetSalesCampaignsByIdAsync(searchModel.SalesCampaignId)
                ?? throw new ArgumentException("No sale campagin found with the specified id");
            //prepare model
            var model = await _salesCampaignsModelFactory.PrepareSaleCampProductListModelAsync(searchModel, sale);

            return Json(model);
        }

        public virtual async Task<IActionResult> SalesCampaignsProductUpdate(SalesCampaignsProductModel model)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            //try to get a product Sales Campaigns with the specified id
            var productSalesCampaigns = await _salesCampaigns.GetProductSalesCampaignsByIdAsync(model.Id)
                ?? throw new ArgumentException("No product category mapping found with the specified id");

            //fill entity from product
            productSalesCampaigns = model.ToEntity(productSalesCampaigns);
            await _salesCampaigns.UpdateProductSalesCampaignsAsync(productSalesCampaigns);

            return new NullJsonResult();
        }

        public virtual async Task<IActionResult> SalesCampaignsProductDelete(int id)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            //try to get a product SalesCampaigns with the specified id 
            var productSalesCampaigns = await _salesCampaigns.GetProductSalesCampaignsByIdAsync(id)
                ?? throw new ArgumentException("No product category mapping found with the specified id", nameof(id));

            await _salesCampaigns.DeleteProductSalesCampaignsAsync(productSalesCampaigns);

            return new NullJsonResult();
        }

        public virtual async Task<IActionResult> SalesCampaignsProductAddPopup(int salescampaignId)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            //prepare model
            var model = await _salesCampaignsModelFactory.PrepareAddProductToSaleCampSearchModelAsync(new SalesCampaignsAddProductSearchModel());
            return View("~/Plugins/Widgets.SalesCampaigns/Views/Admin/SalesCampaignsProductAddPopup.cshtml", model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> SalesCampaignsProductAddPopupList(SalesCampaignsAddProductSearchModel searchModel)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return await AccessDeniedDataTablesJson();

            //prepare model
            var model = await _salesCampaignsModelFactory.PrepareAddProductToSaleCampListModelAsync(searchModel);
            return Json(model);
        }
        [HttpPost]
        [FormValueRequired("save")]
        public virtual async Task<IActionResult> SalesCampaignsProductAddPopup(AddProductToSalesCampaignsModel model)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageCategories))
                return AccessDeniedView();

            //get selected products
            var selectedProducts = await _productService.GetProductsByIdsAsync(model.SelectedProductIds.ToArray());
            if (selectedProducts.Any())
            {
                var existingProductCategories = await _salesCampaigns.GetProductBySalesCampaignsIdAsync(model.SalesCampaignsId, showHidden: true);
                foreach (var product in selectedProducts)
                {
                    //whether product category with such parameters already exists
                    if (_salesCampaigns.FindProductSalesCampaign(existingProductCategories, product.Id, model.SalesCampaignsId) != null)
                        continue;
                    //check same product can't be selected
                    var existingID = _salesCampaigns.CheckExistingProductID(product.Id);

                    if (existingID == product.Id)
                    {
                        _notificationService.ErrorNotification(await _localizationService.GetResourceAsync("Plugins.Widgets.SalesCampaigns.Validation.AlreadyExists"));
                    
                    }
                    else
                    {
                        //insert the new product category mapping
                        await _salesCampaigns.InsertProductSalesCampaignsAsync(new SalesCampaignsProductMapping
                        {
                            SalesCampaignsId = model.SalesCampaignsId,
                            ProductId = product.Id,
                            IsState = true,
                            Price = product.Price
                        });
                    }
                }
            }

            ViewBag.RefreshPage = true;

            return View("~/Plugins/Widgets.SalesCampaigns/Views/Admin/SalesCampaignsProductAddPopup.cshtml", new SalesCampaignsAddProductSearchModel());
        }
        #endregion

        #region Preview
        [HttpPost]
        public virtual async Task<IActionResult> SalesCampaignsProductPreviewList(PreviewSearchModel searchModel)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return await AccessDeniedDataTablesJson();

            //prepare model
            var model = await _salesCampaignsModelFactory.PreparePreviewListModellAsync(searchModel);
            return Json(model);
        }

        #endregion

    }
}
