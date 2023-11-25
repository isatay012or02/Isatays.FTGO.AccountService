using Isatays.FTGO.AccountService.Api.Endpoints;
using Isatays.FTGO.AccountService.Api.Feature.Extensions;
using Isatays.FTGO.AccountService.Api.Feature.MiddleWares;
using Serilog;

var app = WebApplication.CreateBuilder(args).ConfigureBuilder().Build().ConfigureApp();

try
{
    app.UseMiddleware<LoggingMiddleware>();
    app.UseMiddleware<ExceptionHandleMiddleware>();

    app.MapHealthChecks("/healthcheck");

    app.ConfigureAccountEndpoints();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Программа была выброшена с исплючением ({ApplicationName})!");
}
finally
{
    Log.Information("{Msg}!", "Высвобождение ресурсов логгирования");
    await Log.CloseAndFlushAsync();
}
