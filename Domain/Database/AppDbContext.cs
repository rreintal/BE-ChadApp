using Domain.Database.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Domain;

public class AppDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
{
    public DbSet<AppUser> AppUsers { get; set; } = default!;
    public DbSet<AppRefreshToken> RefreshTokens { get; set; } = default!;
    public DbSet<Exercise> Exercises { get; set; } = default!;
    public DbSet<TrainingDay> TrainingDays { get; set; } = default!;
    public DbSet<TrainingDayExercise> TrainingDayExercises { get; set; } = default!;
    public DbSet<TrainingPlan> TrainingPlans { get; set; } = default!;
    public DbSet<TrainingSession> TrainingSessions { get; set; } = default!;
    public DbSet<TrainingSessionExercise> TrainingSessionExercises { get; set; } = default!;
    public DbSet<TrainingSessionSet> TrainingSessionSets { get; set; } = default!;
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        ;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        DisableCascadeDelete(builder);
        ConfigureCreatedAtForEntities(builder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var now = DateTime.UtcNow;

        foreach (var changedEntity in ChangeTracker.Entries())
        {
            if (changedEntity.Entity is IDomainEntityTimestamp entity)
            {
                switch (changedEntity.State)
                {
                    case EntityState.Added:
                        entity.CreatedAt = now;
                        break;

                    /*
                    case EntityState.Modified:
                        Entry(entity).Property(x => x.CreatedDate).IsModified = false;
                        entity.UpdatedDate = now;
                        break;
                        */
                }
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }

    private void DisableCascadeDelete(ModelBuilder builder)
    {
        foreach (var relationship in builder.Model
                     .GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }   
    }

    private void ConfigureCreatedAtForEntities(ModelBuilder builder)
    {
        // Apply configuration to all entities inheriting from DomainEntity
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            if (typeof(DomainEntity).IsAssignableFrom(entityType.ClrType))
            {
                builder.Entity(entityType.ClrType)
                    .Property<DateTime>("CreatedAt")
                    .HasDefaultValueSql("NOW()"); // Set default value to NOW() in PostgreSQL
                
            }
        }
    }
}