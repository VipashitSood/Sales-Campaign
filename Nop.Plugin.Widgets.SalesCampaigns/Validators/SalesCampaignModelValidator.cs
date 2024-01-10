using FluentValidation;
using Nop.Plugin.Widgets.SalesCampaigns.Models;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;

namespace Nop.Plugin.Widgets.SalesCampaigns.Validators
{
    public class SalesCampaignModelValidator : BaseNopValidator<SalesCampaignsModel>
    {
        public SalesCampaignModelValidator(ILocalizationService localizationService)
        {
            RuleFor(model => model.Name)
                .NotEmpty()
                .WithMessageAwait(localizationService.GetResourceAsync("Plugins.Widgets.SalesCampaigns.Fields.Name.Required"));

            RuleFor(model => model.DiscountValue)
                .NotEmpty()
                .WithMessageAwait(localizationService.GetResourceAsync("Plugins.Widgets.SalesCampaigns.Fields.DiscountValue.Required"));

            RuleFor(model => model.DiscountValue)
                .GreaterThan(0)
                .WithMessageAwait(localizationService.GetResourceAsync("Plugins.Widgets.SalesCampaigns.Fields.DiscountValue.GreaterThan.Required"));
        }
    }
}