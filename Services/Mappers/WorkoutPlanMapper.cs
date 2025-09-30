using Domain.Domain;
using Public.DTO.Plan;

namespace Services.Mappers;

public static class WorkoutPlanMapper
{
    public static PlanDto Map(WorkoutPlan plan)
    {
        return new PlanDto()
        {
            Id = plan.Id,
            Name = plan.Name,
            Description = plan.Description,
            CreatedAt = plan.CreatedAt
        };
    }
}