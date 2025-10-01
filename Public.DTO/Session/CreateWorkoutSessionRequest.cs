namespace Public.DTO.Workout;

public class CreateWorkoutSessionRequest
{
    public Guid WorkoutId { get; set; }

    public DateTime StartedAt { get; set; } // TODO: in future make backend assign a startedAt!
    public DateTime EndedAt { get; set; }

    public string? Notes { get; set; }

    public ICollection<CreateWorkoutSessionExerciseRequest> Exercises { get; set; } = default!;
}

public class CreateWorkoutSessionExerciseRequest
{
    public Guid ExerciseId { get; set; }
    public CreateSetRecordRequest SetRecord { get; set; } = default!;
}


public class CreateSetRecordRequest
{
    public int Index { get; set; }
    public int? Reps { get; set; }
    public decimal? Weight { get; set; }
    public decimal? RPE { get; set; }
    public bool Completed { get; set; }
    public bool Skipped { get; set; }
}