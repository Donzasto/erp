using Microsoft.EntityFrameworkCore;

public class ERPContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;

    public ERPContext(DbContextOptions<ERPContext> optionsBuilder) : base(optionsBuilder)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Login = "admin", Password = "admin" }
        );
    }
}