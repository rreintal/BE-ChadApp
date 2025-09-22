using Domain.Database.Contracts;

namespace Domain.Domain;

public class WorkoutExercise : DomainEntity
{
    public Guid WorkoutId { get; set; }
    public Workout Workout { get; set; } = null!;

    public Guid ExerciseId { get; set; }
    public Exercise Exercise { get; set; } = null!;

    public int Position { get; set; }

    // TODO: can have different reps count in different sets- later..
    public int TargetSets { get; set; }
    public int TargetReps { get; set; }
    
    public string? Notes { get; set; }

    public ICollection<SessionExercise> SessionExercises { get; set; } = new List<SessionExercise>();
}