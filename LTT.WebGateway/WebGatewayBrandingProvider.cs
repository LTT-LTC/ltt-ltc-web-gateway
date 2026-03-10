using Microsoft.Extensions.Localization;
using LTT.WebGateway.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace LTT.WebGateway;

[Dependency(ReplaceServices = true)]
public class WebGatewayBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<WebGatewayResource> _localizer;

    public WebGatewayBrandingProvider(IStringLocalizer<WebGatewayResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
