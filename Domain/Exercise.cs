using Domain.Database.Contracts;

namespace Domain;

public class Exercise : DomainEntity
{
    public ICollection<TrainingDayExercise?> TrainingDayExercises { get; set; } = default!;
    public ICollection<TrainingSessionExercise?> TrainingSessionExercises { get; set; } = default!;
    
    public string Name { get; set; } = default!;
}