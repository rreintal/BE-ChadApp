namespace Public.DTO.Workout;

public class WorkoutDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<WorkoutExerciseDto> Exercises { get; set; } = new List<WorkoutExerciseDto>();
    public DateTime CreatedAt { get; set; }
}

public class WorkoutExerciseDto
{
    public Guid Id { get; set; }
    public Guid ExerciseId { get; set; }
    public int Position { get; set; }
    public int TargetReps { get; set; }
    public int TargetSets { get; set; }
}
