namespace Public.DTO.Plan;

public class WorkoutPlanDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }

    public ICollection<PlanWorkoutDto> Workouts { get; set; } = new List<PlanWorkoutDto>();
}

public class PlanWorkoutDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}