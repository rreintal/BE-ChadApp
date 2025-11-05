using Domain;
using Domain.Domain;
using Microsoft.EntityFrameworkCore;
using Public.DTO.Workout;
using Services.Contracts;
using Services.Mappers;

namespace Services;

public class SessionService(AppDbContext dbContext) : ISessionService
{
    public async Task<WorkoutSessionDto?> StartSession(Guid userID, Guid workoutID)
    {
        var requestedWorkout = await dbContext.Workouts
            .Include(w => w.WorkoutExercises)
            .ThenInclude(e => e.Exercise)
            .ThenInclude(e => e.SessionExercises)
            .FirstOrDefaultAsync(w => w.WorkoutPlan.UserId == userID && w.Id == workoutID);
        
        if (requestedWorkout == null)
        {
            return null; // TODO: return error (Result<T>)
        }

        var sessionID = Guid.NewGuid();
        var session = new WorkoutSession()
        {
            Id = sessionID,
            UserId = userID,
            WorkoutId = requestedWorkout.Id,
            CreatedAt = DateTime.UtcNow,
            StartedAt = DateTime.UtcNow,
            Completed = false,
            SessionExercises = requestedWorkout.WorkoutExercises
                .OrderBy(e => e.Position)
                .Select(e => new SessionExercise()
                {
                    Id = Guid.NewGuid(),
                    ExerciseId = e.ExerciseId,
                    Position = e.Position,
                    SessionId = sessionID,
                    SetRecords = new List<SetRecord>(),
                    WorkoutExercise = e

                }).ToList()
        };
        
        await dbContext.WorkoutSessions.AddAsync(session);
        await dbContext.SaveChangesAsync();

        var result = WorkoutSessionMapper.Map(session);
        return result;
    }

    public async Task<bool> SaveSession(FinishWorkoutSessionRequest request, Guid userId)
{
    var session = await dbContext.WorkoutSessions
        .Include(s => s.SessionExercises)
        .ThenInclude(se => se.SetRecords)
        .FirstOrDefaultAsync(s =>
            s.Id == request.Id &&
            s.UserId == userId);

    if (session == null) {
        return false;
    }

    session.Completed = true;
    session.Notes = request.Notes;
    session.EndedAt = DateTime.UtcNow;
    

    foreach (var exerciseDto in request.Exercises)
    {
        var exercise = session.SessionExercises.FirstOrDefault(e => e.Id == exerciseDto.Id);

        if (exercise == null) 
            continue;
        
        foreach (var setDto in exerciseDto.Sets)
        {
            var set = new SetRecord
            {
                Id = Guid.NewGuid(),
                SessionExerciseId = exercise.Id,
                SetIndex = setDto.Index,
                Weight = setDto.Weight,
                Reps = setDto.Reps,
                Completed = setDto.Completed,
                Skipped = setDto.Skipped
            };
            
            dbContext.Entry(set).State = EntityState.Added;
            exercise.SetRecords.Add(set);
        }
    }
    
    await dbContext.SaveChangesAsync();   
   
    return true;
}

    public async Task<List<WorkoutSessionDto>> GetSessions(Guid workoutPlanID, Guid userID)
    {
        var sessions = await dbContext.WorkoutSessions.Where(session =>
                session.Workout.WorkoutPlanId == workoutPlanID && session.Workout.WorkoutPlan.UserId == userID)
            .Include(session => session.SessionExercises)
            .ThenInclude(se => se.SetRecords)
            .Include(session => session.SessionExercises)
            .ThenInclude(se => se.WorkoutExercise)
            .Include(session => session.SessionExercises)
            .ThenInclude(se => se.Exercise)
            .ToListAsync();
        
        return sessions.Select(WorkoutSessionMapper.Map).ToList();
    }
}