using Isatays.FTGO.AccountService.Api.Common.Exceptions;
using Isatays.FTGO.AccountService.Api.Data;
using Isatays.FTGO.AccountService.Api.Services.Interfaces;
using KDS.Primitives.FluentResult;
using Microsoft.EntityFrameworkCore;

namespace Isatays.FTGO.AccountService.Api.Services;

public class Repository : IRepository
{
    private readonly DataContext _dbContext;
    private readonly ILogger<Repository> _logger;

	public Repository(DataContext dbContext, ILogger<Repository> logger) => (_dbContext, _logger) = (dbContext, logger);

    public async Task<Result<bool>> AuthorizeCard(string cardNumber, DateTime expirationDate, int cardCode)
    {
        Card? result = null!;
        try
        {
            result = await _dbContext.Cards
                        .Where(x => x.CardNumber == cardNumber
                        && x.ExpirationDate == expirationDate
                        && x.CardCode == cardCode)
                        .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Ошибка при разрешении оплачивания через карту. Описание: {ex.Message}");
        }

        return Result.Success(result is null);
    }

    public async Task<TEntity> Add<TEntity>(TEntity entity) where TEntity : Entity
    {
        using (var transaction = _dbContext.Database.BeginTransaction())
        {
            try
            {
                await _dbContext.Set<TEntity>().AddAsync(entity);
                await _dbContext.SaveChangesAsync();

                transaction.Commit();
            }
            catch (DatabaseException ex)
            {
                transaction.Rollback();
                _logger.LogError($"Не удалось добавить данные, ошибка на уровне базы данных. Описание: {ex.Message}");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError($"Не удалось добавить данные. Описание: {ex.Message}");
            }
        }

        return entity;
    }
}
