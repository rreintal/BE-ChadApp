using Domain.Domain;
using Public.DTO.Workout;

namespace Services.Mappers;

public class WorkoutMapper
{
    public static WorkoutDto Map(Workout workout)
    {
        return new WorkoutDto()
        {
            Id = workout.Id,
            Name = workout.Name,
            CreatedAt = workout.CreatedAt,
            Exercises = workout.WorkoutExercises.Select(e => Map(e))
                .OrderBy(e => e.Position)
                .ToList()
        };
    }

    private static WorkoutExerciseDto Map(WorkoutExercise exercise)
    {
        return new WorkoutExerciseDto()
        {
            Id = exercise.Id,
            ExerciseId = exercise.ExerciseId,
            Position = exercise.Position,
            TargetReps = exercise.TargetReps,
            TargetSets = exercise.TargetSets
        };
    }
}