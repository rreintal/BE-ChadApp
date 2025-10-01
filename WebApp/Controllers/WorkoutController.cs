using Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Public.DTO.Workout;
using Services.Contracts;

namespace BE_ChadApp.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class WorkoutController(IWorkoutService workoutService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateWorkoutRequest data)
    {
        var result = await workoutService.CreateWorkout(data);
        return Ok(result);
    }

    [HttpPost("{workoutId}")]
    public async Task<IActionResult> GetWorkoutById(Guid workoutId)
    {
        var userId = User.GetUserId();
        var result = await workoutService.GetWorkoutById(userId, workoutId);
        return Ok(result);
    }
}