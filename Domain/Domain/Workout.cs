using Domain.Database.Contracts;

namespace Domain.Domain;

public class Workout : DomainEntity
{
    public Guid WorkoutPlanId { get; set; }
    public WorkoutPlan WorkoutPlan { get; set; } = default!;
    public string Name { get; set; } = default!;
    
    public ICollection<WorkoutExercise> WorkoutExercises { get; set; } = new List<WorkoutExercise>();
    public ICollection<WorkoutSession> WorkoutSessions { get; set; } = new List<WorkoutSession>();
}