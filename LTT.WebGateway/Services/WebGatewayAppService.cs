using LTT.WebGateway.Localization;
using Volo.Abp.Application.Services;

namespace LTT.WebGateway.Services;

/* Inherit your application services from this class. */
public abstract class WebGatewayAppService : ApplicationService
{
    protected WebGatewayAppService()
    {
        LocalizationResource = typeof(WebGatewayResource);
    }
}