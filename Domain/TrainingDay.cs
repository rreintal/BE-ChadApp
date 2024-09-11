using Domain.Database.Contracts;

namespace Domain;

public class TrainingDay : DomainEntity
{
    public Guid TrainingPlanId { get; set; }
    public TrainingPlan? TrainingPlan { get; set; }

    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }

    public ICollection<TrainingDayExercise?> TrainingDayExercises { get; set; } = default!;
    public ICollection<TrainingSession> TrainingSessions { get; set; } = default!;
    
    public string Name { get; set; } = default!;
}