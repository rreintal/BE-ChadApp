using Public.DTO.Workout;

namespace Services.Contracts;

public interface ISessionService
{
    public Task<bool> SaveSession(CreateWorkoutSessionRequest request, Guid userId);
}