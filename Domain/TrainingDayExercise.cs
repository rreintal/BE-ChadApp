using Domain.Database.Contracts;

namespace Domain;

public class TrainingDayExercise : DomainEntity
{
    public Guid TrainingDayId { get; set; }
    public TrainingDay? TrainingDay { get; set; }

    public Guid ExerciseId { get; set; }
    public Exercise? Exercise { get; set; }

    public int PlannedSetsCount { get; set; }
}