using Microsoft.OpenApi.Models;
using Serilog;
using System.Globalization;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.Http.Json;
using System.Reflection;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Isatays.FTGO.AccountService.Api.Feature.Extensions;

[ExcludeFromCodeCoverage]
public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder ConfigureBuilder(this WebApplicationBuilder builder)
    {
        #region Logging

        //Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();
        //builder.Logging.ClearProviders();
        //builder.Logging.AddSerilog(Log.Logger);

        _ = builder.Host.UseSerilog((hostContext, loggerConfiguration) =>
        {
            var assembly = Assembly.GetEntryAssembly();

            _ = loggerConfiguration.ReadFrom.Configuration(hostContext.Configuration)
                    .Enrich.WithProperty("Assembly Version", assembly?.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version)
                    .Enrich.WithProperty("Assembly Informational Version", assembly?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion);
        });

        #endregion Logging

        #region Serialisation

        _ = builder.Services.Configure<JsonOptions>(opt =>
        {
            opt.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            opt.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            opt.SerializerOptions.PropertyNameCaseInsensitive = true;
            opt.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            opt.SerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
        });

        #endregion Serialisation

        #region Swagger

        var ti = CultureInfo.CurrentCulture.TextInfo;
    
        _ = builder.Services.AddEndpointsApiExplorer();
        _ = builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "V1",
                Title = $"{ti.ToTitleCase(builder.Environment.EnvironmentName)} API",
                Description = "An API to show an implementation of TokenService.",
                Contact = new OpenApiContact
                {
                    Name = "Isatay Abdrakhmanov",
                    Email = "isaa012or02@gmail.com"
                }
            });
            options.TagActionsBy(api => new[] { api.GroupName });
            options.DocInclusionPredicate((name, api) => true);
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Заголовок авторизации с использованием схемы Bearer. Пример: \"Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        #endregion Swagger

        #region Project Dependencies

        _ = builder.Services.ConfigureDatabaseConnection(builder.Configuration);
        _ = builder.Services.ConfigureDependencyInjection();

        #endregion Project Dependencies

        return builder;
    }
    
    /// <summary>
    /// Расширяет <see cref="SwaggerGenOptions"/> добавлением комментариев из xml
    /// </summary>
    /// <param name="options"></param>
    /// <param name="assembly"></param>
    private static void AddXmlComment(this SwaggerGenOptions options, Assembly assembly)
    {
        var xmlFile = $"{assembly.GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        options.IncludeXmlComments(xmlPath);
    }
}
