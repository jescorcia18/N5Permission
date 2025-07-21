using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using NSPermissions.Application.Services.Permissions.Imp;
using NSPermissions.Application.Services.Permissions.Interfaces;
using NSPermissions.Domain.Interfaces;
using NSPermissions.Domain.Interfaces.UnitOfWork;
using NSPermissions.Infrastructure.Persistence;
using NSPermissions.Infrastructure.Provider.ElasticSearch;
using NSPermissions.Infrastructure.Provider.Kafka;
using NSPermissions.Infrastructure.UnitOfWork;


namespace NSPermissions.Application.IoC;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IPermissionService, PermissionService>();

        //ElasticSearch
        var url = configuration["Elasticsearch:Uri"];
        var settings = new ConnectionSettings(new Uri(url!)).DefaultIndex("permissions");
        var client = new ElasticClient(settings);
        services.AddSingleton<IElasticClient>(client);
        services.AddScoped<IPermissionIndexService, PermissionIndexService>();

        //Kafka
        services.AddSingleton<IPermissionKafkaProducer, PermissionKafkaProducer>();

        return services;
    }
}
