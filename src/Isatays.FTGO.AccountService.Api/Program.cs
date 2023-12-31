using Isatays.FTGO.AccountService.Api.Endpoints;
using Isatays.FTGO.AccountService.Api.Feature.Extensions;
using Isatays.FTGO.AccountService.Api.Feature.MiddleWares;
using Serilog;

try
{
    var app = WebApplication.CreateBuilder(args).ConfigureBuilder().Build().ConfigureApp();

    app.UseMiddleware<LoggingMiddleware>();
    app.UseMiddleware<ExceptionHandleMiddleware>();

    //app.MapHealthChecks("/healthcheck");

    app.ConfigureAccountEndpoints();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "��������� ���� ��������� � ����������� ({ApplicationName})!");
}
finally
{
    Log.Information("{Msg}!", "������������� �������� ������������");
    await Log.CloseAndFlushAsync();
}
