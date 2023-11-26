using Isatays.FTGO.AccountService.Api.Data;
using Isatays.FTGO.AccountService.Api.Services.Interfaces;
using KDS.Primitives.FluentResult;

namespace Isatays.FTGO.AccountService.Api.Services;

public class AccountService : IAccountService
{
    private readonly IRepository _repository;
	private readonly ILogger<AccountService> _logger;

	public AccountService(IRepository repository, ILogger<AccountService> logger) => (_repository, _logger) = (repository, logger);

	public async Task<Result<bool>> AuthorizeCreditCard(string cardNumber, DateTime expirationDate, int cardCode)
	{
		Result<bool> card = null!;

		try
		{
			card = await _repository.AuthorizeCard(cardNumber, expirationDate, cardCode);
		}
		catch (Exception)
		{

		}

		return card;
	}

	public async Task<Result<Card>> AddCreditCard(string cardNumber, DateTime expirationDate, int cardCode)
	{
		Card result = null!;

		try
		{
			var card = new Card
			{
				CardNumber = cardNumber,
				ExpirationDate = expirationDate,
				CardCode = cardCode
			};

            result = await _repository.Add(card);
		}
		catch (Exception)
		{

		}

		return Result.Success(result);
	}
}
