using Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Public.DTO.Workout;
using Services.Contracts;

namespace BE_ChadApp.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class SessionController(ISessionService sessionService) : ControllerBase
{

    [HttpGet("result/{workoutPlanID}")]
    public async Task<IActionResult> Get(Guid workoutPlanID)
    {
        var result = await sessionService.GetSessions(workoutPlanID, User.GetUserId());
        return Ok(result);
    }

    [HttpPost("start/{workoutID}")]
    public async Task<IActionResult> Start(Guid workoutID)
    {
        var userId = User.GetUserId();
        var result = await sessionService.StartSession(userID: userId, workoutID: workoutID);
        if (result == null)
        {
            return BadRequest();
        }
        
        return Ok(result);
    }
    
    [HttpPost("finish")]
    public async Task<IActionResult> Finish([FromBody] FinishWorkoutSessionRequest request)
    {
        var success = await sessionService.SaveSession(request, userId: User.GetUserId());
        return success ? Ok() : BadRequest();
    }
}