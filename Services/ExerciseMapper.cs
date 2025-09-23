using System.Data;
using Domain.Domain;
using Public.DTO;

namespace Services;

public class ExerciseMapper
{
    public static GetExercise Map(Exercise exercise)
    {
        return new GetExercise()
        {
            Id = exercise.Id,
            Name = exercise.Name
        };
    } 
}