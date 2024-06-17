using Isatays.FTGO.AccountService.Api.Common.Errors;
using Isatays.FTGO.AccountService.Api.Data;
using Isatays.FTGO.AccountService.Api.Models;
using Isatays.FTGO.AccountService.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;

namespace Isatays.FTGO.AccountService.Api.Endpoints;

public static class AccountEndpoints
{
    public static void ConfigureAccountEndpoints(this WebApplication app)
    {
        app.MapPost("api/authorize-credit-card", AuthorizeCreditCard)
            .WithGroupName("Account")
            .Produces<bool>(StatusCodes.Status200OK, contentType: MediaTypeNames.Application.Json)
            .Produces<ApiError>(StatusCodes.Status400BadRequest, contentType: MediaTypeNames.Application.Json)
            .Produces<ApiError>(StatusCodes.Status404NotFound, contentType: MediaTypeNames.Application.Json)
            .Produces<ApiError>(StatusCodes.Status500InternalServerError, contentType: MediaTypeNames.Application.Json);

        app.MapPost("api/add-credit-card", AddCreditCard)
            .WithGroupName("Account")
            .Produces<Card>(StatusCodes.Status200OK, contentType: MediaTypeNames.Application.Json)
            .Produces<ApiError>(StatusCodes.Status400BadRequest, contentType: MediaTypeNames.Application.Json)
            .Produces<ApiError>(StatusCodes.Status404NotFound, contentType: MediaTypeNames.Application.Json)
            .Produces<ApiError>(StatusCodes.Status500InternalServerError, contentType: MediaTypeNames.Application.Json);
    }
    
    [Authorize(Roles = "Moderator")]
    private static async Task<IResult> AuthorizeCreditCard(IAccountService accountService, [FromBody] AuthorizeCreditCardDto request)
    {
        var result = await accountService.AuthorizeCreditCard(request.CreditNumber, request.ExpirationDate, request.CardCode);

        return Results.Ok(result.Value);
    }
    
    [Authorize(Roles = "Moderator")]
    private static async Task<IResult> AddCreditCard(IAccountService accountService, [FromBody] AddCreditCardDto request)
    {
        var result = await accountService.AddCreditCard(request.CardNumber, request.ExpirationDate, request.CardCode);

        return Results.Ok(result.Value);
    }
}
