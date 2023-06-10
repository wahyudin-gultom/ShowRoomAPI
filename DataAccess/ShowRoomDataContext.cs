using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using ShowRoomAPI.Models.Entitas;

public class ShowRoomDataContext : DbContext
{
	private void ResetAllRegeneralColumn()
	{
        var DataRemoved = ChangeTracker.Entries().Where(m => m.State == EntityState.Deleted).Select(m => m.Entity).ToList();
        foreach (var item in DataRemoved)
        {
            var attributes = (GeneralColumn)item;
            if (attributes == null) continue;

            attributes.IsRemoved = true;
        }

        var DataInserted = ChangeTracker.Entries().Where(m => m.State == EntityState.Added).Select(m => m.Entity).ToList();
        foreach (var item in DataInserted)
        {
            var attributes = (GeneralColumn)item;
            if (attributes == null) continue;

            attributes.CreatedDate = DateTime.UtcNow;
            attributes.IsRemoved = false;
        }

        var DataUpdated = ChangeTracker.Entries().Where(m => m.State == EntityState.Modified).Select(m => m.Entity).ToList();
        foreach (var item in DataUpdated)
        {
            var attributes = (GeneralColumn)item;
            if (attributes == null) continue;

            attributes.UpdatedDate = DateTime.UtcNow;
        }
    }

    public ShowRoomDataContext(DbContextOptions<ShowRoomDataContext> options): base(options)
	{

	}

    public override int SaveChanges()
    {
        ResetAllRegeneralColumn();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ResetAllRegeneralColumn();
        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Car>().HasQueryFilter(m => !m.IsRemoved);
		modelBuilder.UseSerialColumns();
	}

	public DbSet<Car> Cars { get; set; }
}