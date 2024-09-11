using Domain.Database.Contracts;

namespace Domain;

public class TrainingSession : DomainEntity
{
    public ICollection<TrainingSessionExercise?> TrainingSessionExercises { get; set; } = default!;
    
    public Guid TrainingDayId { get; set; }
    public TrainingDay? TrainingDay { get; set; }
    
    public DateTime StartedAt { get; set; }
    public DateTime FinishedAt { get; set; }

    public string? Comment { get; set; }
}