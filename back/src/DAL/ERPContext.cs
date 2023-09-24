using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

public sealed class ERPContext : DbContext
{
    public DbSet<Nomenclature> Nomenclatures { get; set; }
    public DbSet<User> Users { get; set; }
    // public DbSet<Nomenclature> nomenclatures;
    public ERPContext() : base()
    {
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=erp;Username=postgres;Password=mysecretpassword");
        optionsBuilder.LogTo(Console.WriteLine);
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

        modelBuilder.Entity<NomenclatureRelations>().HasData(
                new NomenclatureRelations { Id = 1, ParrentId = 6, ChildId = 1 },
                new NomenclatureRelations { Id = 2, ParrentId = 1, ChildId = 2 },
                new NomenclatureRelations { Id = 3, ParrentId = 1, ChildId = 3 },
                new NomenclatureRelations { Id = 4, ParrentId = 1, ChildId = 4 },
                new NomenclatureRelations { Id = 5, ParrentId = 1, ChildId = 5 },
                new NomenclatureRelations { Id = 6, ParrentId = 6, ChildId = 7 },
                new NomenclatureRelations { Id = 7, ParrentId = 6, ChildId = 8 },
                new NomenclatureRelations { Id = 8, ParrentId = 6, ChildId = 9 },
                new NomenclatureRelations { Id = 9, ParrentId = 6, ChildId = 10 }
        );

        modelBuilder.Entity<Nomenclature>()
            .HasMany(e => e.ParrentsId)
            .WithOne(e => e.NomenclatureParrents)
            .HasForeignKey(e => e.ParrentId)
            .IsRequired();

        modelBuilder.Entity<Nomenclature>()
            .HasMany(e => e.ChildsId)
            .WithOne(e => e.NomenclatureChilds)
            .HasForeignKey(e => e.ChildId)
            .IsRequired();
    }
}