namespace Isatays.FTGO.AccountService.Api.Models;

public record AuthorizeCreditCardDto(Guid Id, string CreditNumber, DateTime ExpirationDate, int CardCode);

