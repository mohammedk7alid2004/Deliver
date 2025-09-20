
namespace Deliver.Dal.Data;

public class ApplicationDbContext:IdentityDbContext<ApplicationUser,IdentityRole<int>,int>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) : base(dbContextOptions)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyAllConfigurations();

        base.OnModelCreating(modelBuilder);
    }
}
