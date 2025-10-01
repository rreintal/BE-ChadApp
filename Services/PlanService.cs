using Domain;
using Domain.Domain;
using Microsoft.EntityFrameworkCore;
using Public.DTO.Plan;
using Services.Contracts;
using Services.Mappers;

namespace Services;

public class PlanService(AppDbContext dbContext) : IPlanService
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<WorkoutPlanDto> CreatePlan(Guid userId, CreateWorkoutPlanRequest dto)
    {
        var isNameExists =
            await _dbContext.WorkoutPlans
                .AnyAsync(p => p.UserId == userId && p.Name == dto.Name);
        
        if (isNameExists)
        {
            throw new InvalidOperationException("Plan with same name exists."); // TODO: create Result<T> (FluentResult või OneOf)
        }
        
        var plan = new WorkoutPlan()
        {
            UserId = userId,
            Name = dto.Name,
            Description = dto.Description
        };

        var result = await _dbContext.WorkoutPlans.AddAsync(plan);
        await _dbContext.SaveChangesAsync();
        return new WorkoutPlanDto()
        {
            Id = result.Entity.Id,
            CreatedAt = result.Entity.CreatedAt,
            Name = result.Entity.Name,
            Description = result.Entity.Description,
        };
    }

    public async Task<IEnumerable<WorkoutPlanDto>> GetAllPlansList(Guid userId)
    {
        var items = await _dbContext.WorkoutPlans.Where(p => p.UserId == userId)
            .Include(p => p.Workouts)
            .ToListAsync();
        return items.Select(WorkoutPlanMapper.Map);
    }
}