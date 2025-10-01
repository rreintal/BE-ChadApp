using Domain;
using Domain.Domain;
using Microsoft.EntityFrameworkCore;
using Public.DTO.Workout;
using Services.Contracts;
using Services.Mappers;

namespace Services;

public class WorkoutService(AppDbContext dbContext) : IWorkoutService
{
    public async Task<WorkoutDto> CreateWorkout(CreateWorkoutRequest request)
    {
        var workout = new Workout()
        {
            Name = request.Name,
            WorkoutPlanId = request.WorkoutPlanId,
            WorkoutExercises = request.Exercises.Select(e => new WorkoutExercise()
            {
                ExerciseId = e.ExerciseId,
                Position = e.Position,
                TargetReps = e.TargetReps,
                TargetSets = e.TargetSets
            }).ToList()
        };

        var saved = (await dbContext.Workouts.AddAsync(workout)).Entity;
        await dbContext.SaveChangesAsync();
        return WorkoutMapper.Map(saved);
    }

    public async Task<WorkoutDto?> GetWorkoutById(Guid userId, Guid workoutId)
    {
        var result = await dbContext.Workouts
            .Where(w => w.Id == workoutId && w.WorkoutPlan.UserId == userId)
            .Include(w => w.WorkoutExercises)
            .FirstOrDefaultAsync();

        if (result == null)
        {
            return null;
        }
        
        return WorkoutMapper.Map(result); 
    }
}