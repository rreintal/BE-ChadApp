using Domain.Domain;
using Public.DTO.Workout;

namespace Services.Mappers;

public static class WorkoutSessionMapper
{
    public static WorkoutSessionDto Map(WorkoutSession session)
    {
        return new WorkoutSessionDto()
        {
            Id = session.Id,
            Notes = session.Notes,
            StartedAt = session.StartedAt,
            Exercises = session.SessionExercises
                .OrderBy(e => e.Position)
                .Select(e =>
                {
                    if (e.WorkoutExercise == null)
                    {
                        throw new InvalidOperationException($"WorkoutExercise is missing for SessionExercise {e.Id}");
                    }

                    return new WorkoutSessionExerciseDto()
                    {
                        Id = e.Id,
                        ExerciseId = e.ExerciseId,
                        Name = e.Exercise.Name,
                        Position = e.Position,
                        TargetReps = e.WorkoutExercise.TargetReps,
                        TargetSets = e.WorkoutExercise.TargetSets,
                        Sets = e.SetRecords
                            .OrderBy(set => set.SetIndex)
                            .Select(set => new WorkoutSessionExerciseSetDto()
                            {
                                Id = set.Id,
                                Reps = set.Reps,
                                Weight = set.Weight,
                                Index = set.SetIndex
                            }).ToList()

                    };
                }).ToList()
        };
    }
}