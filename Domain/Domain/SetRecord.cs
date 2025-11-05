using Domain.Database.Contracts;

namespace Domain.Domain;

public class SetRecord : DomainEntity
{
    public Guid SessionExerciseId { get; set; }
    public SessionExercise SessionExercise { get; set; } = null!;

    public int SetIndex { get; set; }

    public int Reps { get; set; }
    public decimal Weight { get; set; }

    public decimal? RPE { get; set; }

    public bool Completed { get; set; } = false;
    public bool Skipped { get; set; } = false;
}