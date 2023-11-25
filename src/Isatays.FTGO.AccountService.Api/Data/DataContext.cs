using Microsoft.EntityFrameworkCore;

namespace Isatays.FTGO.AccountService.Api.Data;

public class DataContext : DbContext
{
	public DataContext(DbContextOptions<DataContext> options) : base(options) { }

	public DbSet<Card> Cards { get; set; }
}
