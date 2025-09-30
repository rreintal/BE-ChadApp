using Public.DTO.Plan;

namespace Services.Contracts;

public interface IPlanService
{
    public Task<PlanDto> CreatePlan(Guid userId, CreatePlanRequest dto);
    public Task<IEnumerable<PlanDto>> GetAllPlansList(Guid userId);
}