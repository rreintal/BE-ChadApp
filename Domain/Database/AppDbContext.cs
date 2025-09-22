using Domain.Database.Contracts;
using Domain.Domain;
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
    public DbSet<WorkoutPlan> WorkoutPlans { get; set; } = default!;
    public DbSet<Workout> Workouts { get; set; } = default!;
    public DbSet<WorkoutExercise> WorkoutExercises { get; set; } = default!;
    public DbSet<WorkoutSession> WorkoutSessions { get; set; } = default!;
    public DbSet<SessionExercise> SessionExercises { get; set; } = default!;
    public DbSet<SetRecord> SetRecords { get; set; } = default!;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        DisableCascadeDelete(builder);
        ConfigureCreatedAtForEntities(builder);
        
        // === Cascade deletes ===

        // When a WorkoutSession is deleted → its SessionExercises are deleted
        builder.Entity<WorkoutSession>()
            .HasMany(ws => ws.SessionExercises)
            .WithOne(se => se.Session)
            .HasForeignKey(se => se.SessionId)
            .OnDelete(DeleteBehavior.Cascade);

        // When a SessionExercise is deleted → its SetRecords are deleted
        builder.Entity<SessionExercise>()
            .HasMany(se => se.SetRecords)
            .WithOne(sr => sr.SessionExercise)
            .HasForeignKey(sr => sr.SessionExerciseId)
            .OnDelete(DeleteBehavior.Cascade);
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