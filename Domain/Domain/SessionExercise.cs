using Domain.Database.Contracts;

namespace Domain.Domain;

public class SessionExercise : DomainEntity
{
    public Guid SessionId { get; set; }
    public WorkoutSession Session { get; set; } = null!;

    public Guid ExerciseId { get; set; }
    public Exercise Exercise { get; set; } = null!;

    public Guid? WorkoutExerciseId { get; set; }
    public WorkoutExercise? WorkoutExercise { get; set; }

    public int Position { get; set; }
    public bool Skipped { get; set; }

    public ICollection<SetRecord> SetRecords { get; set; } = new List<SetRecord>();
}

