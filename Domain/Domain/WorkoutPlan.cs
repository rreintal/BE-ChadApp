using System.ComponentModel.DataAnnotations;
using Domain.Database.Contracts;

namespace Domain.Domain;

public class WorkoutPlan : DomainEntity
{
    public Guid UserId { get; set; }
    public AppUser User { get; set; } = null!;

    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    public ICollection<Workout> Workouts { get; set; } = new List<Workout>();
}