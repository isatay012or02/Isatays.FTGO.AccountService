using Isatays.FTGO.AccountService.Api.Data;
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
}
