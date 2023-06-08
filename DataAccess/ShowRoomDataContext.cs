using Microsoft.EntityFrameworkCore;
using ShowRoomAPI.Models.Entitas;

public class ShowRoomDataContext : DbContext
{
	public ShowRoomDataContext(DbContextOptions<ShowRoomDataContext> options): base(options)
	{

	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.UseSerialColumns();
	}

	public DbSet<Car> Cars { get; set; }
}