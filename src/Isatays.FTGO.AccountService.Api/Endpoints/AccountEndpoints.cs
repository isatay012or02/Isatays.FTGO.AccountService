namespace Isatays.FTGO.AccountService.Api.Endpoints;

public static class AccountEndpoints
{
    public static void ConfigureAccountEndpoints(this WebApplication app)
    {
        app.MapPost("api/account", AuthorizeCreditCard).WithName("");
    }

    public static async Task<IResult> AuthorizeCreditCard()
    {
        return Results.Ok();
    }
}
