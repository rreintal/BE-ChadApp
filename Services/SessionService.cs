using Domain.Domain;
using Public.DTO.Workout;
using Services.Contracts;

namespace Services;

public class SessionService : ISessionService
{
    public Task<bool> SaveSession(CreateWorkoutSessionRequest request, Guid userId)
    {
        var sessionId = Guid.NewGuid();
        var session = new WorkoutSession()
        {
            Id = sessionId,
            UserId = userId,
            WorkoutId = request.WorkoutId,
            StartedAt = request.StartedAt,
            EndedAt = request.EndedAt,
            Notes = request.Notes,
            SessionExercises = request.Exercises.Select(e =>
                new SessionExercise()
                {
                    SessionId = sessionId,
                    ExerciseId   = e.ExerciseId,
                    WorkoutExerciseId = 
                    
                }).ToList()
        };
        throw new NotImplementedException();
    }
}