using Microsoft.EntityFrameworkCore;
using RepairIT.Shared.Modeles;

namespace RepairIT.UsersService.Data;


public class UsersContext : DbContext
{
    // user table
    public DbSet<Utilisateur> Utilisateurs => Set<Utilisateur>();

    
    public UsersContext(DbContextOptions<UsersContext> options) : base(options)
    {
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 2 users ne peuvent avoir le mm mail
        modelBuilder.Entity<Utilisateur>()
            .HasIndex(u => u.Email)
            .IsUnique();
    }
}