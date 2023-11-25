using Isatays.FTGO.AccountService.Api.Data;

namespace Isatays.FTGO.AccountService.Api.Services.Interfaces;

public interface IRepository
{
    Task<TEntity> Add<TEntity>(TEntity entity) where TEntity : Entity;
}
