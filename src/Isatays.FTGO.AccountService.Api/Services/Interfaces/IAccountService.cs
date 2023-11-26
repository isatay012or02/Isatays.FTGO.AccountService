using Isatays.FTGO.AccountService.Api.Data;
using KDS.Primitives.FluentResult;

namespace Isatays.FTGO.AccountService.Api.Services.Interfaces;

public interface IAccountService
{
    Task<Result<bool>> AuthorizeCreditCard(string cardNumber, DateTime expirationDate, int cardCode);

    Task<Result<Card>> AddCreditCard(string cardNumber, DateTime expirationDate, int cardCode);
}
