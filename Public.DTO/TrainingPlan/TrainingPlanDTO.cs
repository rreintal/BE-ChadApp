using Public.DTO.Base;

namespace Public.DTO.TrainingPlan;

public class TrainingPlanDTO : IdentifiableDTO
{
    public string Name { get; set; } = default!;
}