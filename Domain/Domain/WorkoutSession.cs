using Domain.Database.Contracts;

namespace Domain.Domain;

public class WorkoutSession : DomainEntity
{
    public Guid UserId { get; set; }
    public AppUser User { get; set; } = null!;
    public Guid WorkoutId { get; set; }
    public Workout Workout { get; set; } = null!;

    public DateTime StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
    
    public ICollection<SessionExercise> SessionExercises { get; set; } = new List<SessionExercise>();
    
    public string? Notes { get; set; }

    public bool Completed { get; set; }

    // TODO: add FINISHED prop
}