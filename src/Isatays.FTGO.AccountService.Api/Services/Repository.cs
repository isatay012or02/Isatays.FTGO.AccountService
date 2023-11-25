using Isatays.FTGO.AccountService.Api.Common.Exceptions;
using Isatays.FTGO.AccountService.Api.Data;
using Isatays.FTGO.AccountService.Api.Services.Interfaces;

namespace Isatays.FTGO.AccountService.Api.Services;

public class Repository : IRepository
{
    private readonly DataContext _dbContext;
    private readonly ILogger _logger;

	public Repository(DataContext dbContext, ILogger logger) => (_dbContext, _logger) = (dbContext, logger);

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
