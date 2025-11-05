namespace Public.DTO.Workout;

public class FinishWorkoutSessionRequest // TODO: rename FINISH ...
{
    public Guid Id { get; set; }
    public string? Notes { get; set; }

    public ICollection<FinishWorkoutSessionExerciseRequest> Exercises { get; set; } = default!;
}

public class FinishWorkoutSessionExerciseRequest
{
    public Guid Id { get; set; }
    public int Position { get; set; }
    public ICollection<FinishSetRecordRequest> Sets { get; set; } = default!;
}


public class FinishSetRecordRequest
{
    public int Index { get; set; }
    public int Reps { get; set; }
    public decimal Weight { get; set; }
    public decimal? RPE { get; set; }
    public bool Completed { get; set; }
    public bool Skipped { get; set; }
}