using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LTT.WebGateway.Data;

public class WebGatewayDbContextFactory : IDesignTimeDbContextFactory<WebGatewayDbContext>
{
    public WebGatewayDbContext CreateDbContext(string[] args)
    {
        WebGatewayEfCoreEntityExtensionMappings.Configure();
        
        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<WebGatewayDbContext>()
            .UseSqlServer(configuration.GetConnectionString("Default"));

        return new WebGatewayDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
