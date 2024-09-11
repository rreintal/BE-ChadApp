using Domain.Database.Contracts;

namespace Domain;

public class TrainingSessionSet : DomainEntity
{
    public Guid TrainingSessionExerciseId { get; set; }
    public TrainingSessionExercise? TrainingSessionExercise { get; set; }
    
    public float Weight { get; set; } = default!;
    public int Reps { get; set; } = default!;
}