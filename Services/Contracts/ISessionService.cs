using Public.DTO.Workout;

namespace Services.Contracts;

public interface ISessionService
{
    public Task<WorkoutSessionDto?> StartSession(Guid userID, Guid workoutID);
    public Task<bool> SaveSession(FinishWorkoutSessionRequest request, Guid userId);

    public Task<List<WorkoutSessionDto>> GetSessions(Guid workoutPlanID, Guid userID);
}