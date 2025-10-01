namespace Public.DTO.Workout;

public class CreateWorkoutRequest
{
    public Guid WorkoutPlanId { get; set; }
    public string Name { get; set; } = default!;
    public ICollection<WorkoutExerciseRequest> Exercises { get; set; } = new List<WorkoutExerciseRequest>();
}

public class WorkoutExerciseRequest
{
    public Guid ExerciseId { get; set; }
    public int Position { get; set; }
    public int TargetReps { get; set; }
    public int TargetSets { get; set; }
}