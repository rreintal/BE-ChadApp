using Domain.Database.Contracts;

namespace Domain;

public class TrainingSessionExercise : DomainEntity
{
    public Guid ExerciseId { get; set; }
    public Exercise? Exercise { get; set; }

    public ICollection<TrainingSessionSet?> TrainingSessionSets { get; set; } = default!;

    public string? Comment { get; set; }
    
    // TODO: for length: StartedAt, FinishedAt? not necessary??
}