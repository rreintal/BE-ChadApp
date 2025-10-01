using Public.DTO.Plan;

namespace Services.Contracts;

public interface IPlanService
{
    public Task<WorkoutPlanDto> CreatePlan(Guid userId, CreateWorkoutPlanRequest dto);
    public Task<IEnumerable<WorkoutPlanDto>> GetAllPlansList(Guid userId);
}