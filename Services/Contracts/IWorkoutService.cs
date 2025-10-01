using Public.DTO.Workout;

namespace Services.Contracts;

public interface IWorkoutService
{
    public Task<WorkoutDto> CreateWorkout(CreateWorkoutRequest request);
    public Task<WorkoutDto?> GetWorkoutById(Guid userId, Guid workoutId);
}