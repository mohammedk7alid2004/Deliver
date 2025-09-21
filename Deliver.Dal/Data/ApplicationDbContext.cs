
namespace Deliver.Dal.Data;

public class ApplicationDbContext:IdentityDbContext<ApplicationUser,IdentityRole<int>,int>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) : base(dbContextOptions)
    {
    }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<Delivery> Deliveries { get; set; }
    public DbSet<VehicleType> vehicleTypes { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyAllConfigurations();

        base.OnModelCreating(modelBuilder);
    }
}
