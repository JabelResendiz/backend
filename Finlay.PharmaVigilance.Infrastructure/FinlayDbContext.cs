

using Finlay.PharmaVigilance.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Finlay.PharmaVigilance.Infrastructure;


public class FinlayDbContext : IdentityDbContext<User,Role,int>
{
    public FinlayDbContext(DbContextOptions options) : base(options) {

    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
      {
      var entries = ChangeTracker.Entries<User>();

      foreach (var entry in entries)
      {
            if (entry.State == EntityState.Added)
            {
                  entry.Entity.CreatedAt = DateTime.UtcNow;
                  entry.Entity.UpdatedAt = DateTime.UtcNow;
            }

            if (entry.State == EntityState.Modified)
            {
                  entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
      }

      return await base.SaveChangesAsync(cancellationToken);
      }


    public DbSet<Employee> Employees { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e=> e.UserRole)
                  .IsRequired();
            entity.Property(e=> e.CreatedAt)
                  .IsRequired();
            entity.Property(e=> e.UpdatedAt)
                  .IsRequired();
            // entity.Property(e=> e.RefreshToken)
            //       .IsRequired();
        });


        builder.Entity<Role>(entity => 
        {
            entity.HasKey(e => e.Id);
            
        });

        // builder.Entity<Employee>(entity => 
        // {
        //     entity.HasKey(e => e.Id);
        //     entity.Property(e=> e.Name)
        //           .IsRequired();
        //     entity.Property(e=> e.UserRole)
        //           .IsRequired();
        //     entity.Property(e=> e.Email)
        //           .IsRequired();
        //     entity.Property(e=> e.UserName)
        //           .IsRequired();
        // });


    }
}