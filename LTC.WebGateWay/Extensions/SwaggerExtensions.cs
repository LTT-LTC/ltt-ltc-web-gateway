using Swashbuckle.AspNetCore.SwaggerUI;

namespace LTC.WebGateWay.Extensions
{
    public static class SwaggerExtensions
    {
        public static void ConfigureSwaggerEndpoints(
            this SwaggerUIOptions options,
            IConfiguration reverseProxyConfig
        )
        {
            var clustersSection = reverseProxyConfig.GetSection("Clusters");
            foreach (var cluster in clustersSection.GetChildren())
            {
                var clusterName = cluster.Key;
                var destinations = cluster.GetSection("Destinations");
                foreach (var destination in destinations.GetChildren())
                {
                    var swaggers = destination.GetSection("Swaggers").GetChildren();
                    foreach (var swagger in swaggers)
                    {
                        var paths = swagger.GetSection("Paths").GetChildren();
                        foreach (var path in paths)
                        {
                            options.SwaggerEndpoint(path.Value!, clusterName);
                        }
                    }
                }
            }
        }
    }
}
