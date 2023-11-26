namespace Isatays.FTGO.AccountService.Api.Models;

public record AddCreditCardDto(string CardNumber, DateTime ExpirationDate, int CardCode);
