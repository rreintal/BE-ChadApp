namespace Public.DTO.Workout;

public class WorkoutSessionDto
{
    public Guid Id { get; set; }
    public DateTime StartedAt { get; set; }
    public string? Notes { get; set; }
    public ICollection<WorkoutSessionExerciseDto> Exercises { get; set; } = new List<WorkoutSessionExerciseDto>();
}

public class WorkoutSessionExerciseDto
{
    public Guid Id { get; set; }
    public Guid ExerciseId { get; set; }
    public string Name { get; set; } = default!;
    public int Position { get; set; }

    public int TargetReps { get; set; }
    public int TargetSets { get; set; }
    public ICollection<WorkoutSessionExerciseSetDto> Sets { get; set; } = new List<WorkoutSessionExerciseSetDto>();
}

public class WorkoutSessionExerciseSetDto
{
    public Guid Id { get; set; }
    public decimal Weight { get; set; }
    public int Reps { get; set; }
    public int Index { get; set; }
}