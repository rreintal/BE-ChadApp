using Domain.Database.Contracts;

namespace Domain;

public class TrainingPlan : DomainEntity
{
    public ICollection<TrainingDay?> TrainingDays { get; set; } = default!;
    
    public string Name { get; set; } = default!;
}