using Microsoft.EntityFrameworkCore;

public class ERPContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;

    public ERPContext()
    {
        Database.EnsureCreated();
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=erp;Username=postgres;Password=mysecretpassword");
    }
}
