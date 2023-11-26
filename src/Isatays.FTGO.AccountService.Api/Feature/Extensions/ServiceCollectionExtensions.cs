using Isatays.FTGO.AccountService.Api.Data;
using Isatays.FTGO.AccountService.Api.Services;
using Isatays.FTGO.AccountService.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Isatays.FTGO.AccountService.Api.Feature.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureDatabaseConnection(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Ftgo")!;

        services.AddDbContext<DataContext>(options =>
        {
            options.UseNpgsql(connectionString,
                sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        3,
                        TimeSpan.FromSeconds(10),
                        null);
                });
        });

        return services;
    }

    public static IServiceCollection ConfigureDependencyInjection(this IServiceCollection services)
    {
        services.AddScoped<IAccountService, Services.AccountService>();
        services.AddScoped<IRepository, Repository>();

        return services;
    }
}
