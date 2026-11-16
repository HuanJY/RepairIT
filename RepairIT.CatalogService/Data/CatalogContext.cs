using Microsoft.EntityFrameworkCore;
using RepairIT.Shared.Modeles;

namespace RepairIT.CatalogService.Data;

public class CatalogContext : DbContext
{
    // table des ordi
    public DbSet<Ordinateur> Ordinateurs => Set<Ordinateur>();

    public CatalogContext(DbContextOptions<CatalogContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    
        modelBuilder.Entity<Ordinateur>()
            .Property(o => o.Prix)
            .HasConversion<double>();
    }
}