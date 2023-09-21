using Microsoft.EntityFrameworkCore;

public class ERPContext : DbContext
{
    public ERPContext() : base()
    {
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=erp;Username=postgres;Password=mysecretpassword");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Login = "admin", Password = "admin" }
        );

        modelBuilder.Entity<Nomenclature>().HasData(
                new Nomenclature { Id = 1, Name = "System unit", Code = "A.413" },
                new Nomenclature { Id = 2, Name = "Processor", Code = "A.613" },
                new Nomenclature { Id = 3, Name = "Graphic card", Code = "A.443" },
                new Nomenclature { Id = 4, Name = "RAM", Code = "A.412" },
                new Nomenclature { Id = 5, Name = "Hard disk", Code = "A.113" },
                new Nomenclature { Id = 6, Name = "PC", Code = "A.463" },
                new Nomenclature { Id = 7, Name = "Monitor", Code = "A.543" },
                new Nomenclature { Id = 8, Name = "Keyboard", Code = "A.653" },
                new Nomenclature { Id = 9, Name = "Mouse", Code = "A.123" },
                new Nomenclature { Id = 10, Name = "Cooler", Code = "A.459" }
        );
    }
}