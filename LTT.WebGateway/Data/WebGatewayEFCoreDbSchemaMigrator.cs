using Microsoft.EntityFrameworkCore;
using Volo.Abp.DependencyInjection;

namespace LTT.WebGateway.Data;

public class WebGatewayEFCoreDbSchemaMigrator : ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public WebGatewayEFCoreDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolve the WebGatewayDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<WebGatewayDbContext>()
            .Database
            .MigrateAsync();
    }
}
