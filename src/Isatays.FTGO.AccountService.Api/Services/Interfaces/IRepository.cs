using Isatays.FTGO.AccountService.Api.Data;
using KDS.Primitives.FluentResult;

namespace Isatays.FTGO.AccountService.Api.Services.Interfaces;

public interface IRepository
{
    Task<Result<bool>> AuthorizeCard(string cardNumber, DateTime expirationDate, int cardCode);

    Task<TEntity> Add<TEntity>(TEntity entity) where TEntity : Entity;
}
