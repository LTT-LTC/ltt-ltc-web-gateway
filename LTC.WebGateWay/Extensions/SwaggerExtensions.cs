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
                            var endpoint = ToSwaggerUiRelativePath(path.Value);
                            if (!string.IsNullOrWhiteSpace(endpoint))
                            {
                                options.SwaggerEndpoint(endpoint, clusterName);
                            }
                        }
                    }
                }
            }
        }

        private static string? ToSwaggerUiRelativePath(string? configuredPath)
        {
            if (string.IsNullOrWhiteSpace(configuredPath))
            {
                return null;
            }

            // Keep fully qualified URLs unchanged.
            if (Uri.TryCreate(configuredPath, UriKind.Absolute, out _))
            {
                return configuredPath;
            }

            var trimmed = configuredPath.Trim();
            if (trimmed.StartsWith('/'))
            {
                trimmed = trimmed[1..];
            }

            // Swagger UI route is "ltc/swagger"; using ../ keeps proxy/path-base prefixes.
            if (trimmed.StartsWith("ltc/", StringComparison.OrdinalIgnoreCase))
            {
                return $"../{trimmed["ltc/".Length..]}";
            }

            return $"./{trimmed}";
        }
    }
}
