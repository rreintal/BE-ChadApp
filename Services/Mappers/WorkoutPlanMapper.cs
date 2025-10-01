using Domain.Domain;
using Public.DTO.Plan;

namespace Services.Mappers;

public static class WorkoutPlanMapper
{
    public static WorkoutPlanDto Map(WorkoutPlan plan)
    {
        return new WorkoutPlanDto()
        {
            Id = plan.Id,
            Name = plan.Name,
            Description = plan.Description,
            CreatedAt = plan.CreatedAt,
            Workouts = plan.Workouts.Select(w => new PlanWorkoutDto()
            {
                Id = w.Id,
                Name = w.Name
            }).ToList()
        };
    }
}