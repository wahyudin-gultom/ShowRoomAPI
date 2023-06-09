using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using ShowRoomAPI.Models.Entitas;

public class ShowRoomDataContext : DbContext
{
	public ShowRoomDataContext(DbContextOptions<ShowRoomDataContext> options): base(options)
	{

	}

    public override int SaveChanges()
    {
		//edit
		var DataRemoved = ChangeTracker.Entries().Where(m => m.State == EntityState.Deleted).Select(m => m.Entity).ToList();
		foreach (var item in DataRemoved)
		{
			var attributes = (GeneralColumn)item;
			if (attributes == null) continue;

			attributes.IsRemoved = true;
		}
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
		//edit
        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Car>().HasQueryFilter(m => !m.IsRemoved);

		//modelBuilder.Entity<Car>().HasData(new Car[]
		//{
		//	new Car{ },
		//	new Car{ }

		//});
		modelBuilder.UseSerialColumns();
		
	}

	public DbSet<Car> Cars { get; set; }
}