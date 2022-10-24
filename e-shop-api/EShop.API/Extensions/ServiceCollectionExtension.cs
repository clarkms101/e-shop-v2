using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace e_shop_api.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddElasticsearch(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = new ConnectionSettings(new Uri(configuration["ElasticsearchSettings:Uri"]));
        var defaultIndex = configuration["ElasticsearchSettings:DefaultIndex"];
        if (!string.IsNullOrEmpty(defaultIndex))
            settings = settings.DefaultIndex(defaultIndex);
        var client = new ElasticClient(settings);
        services.AddSingleton<IElasticClient>(client);
    }
}