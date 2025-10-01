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
    [HttpPost]
    public async Task<IActionResult> Finish(CreateWorkoutSessionRequest request)
    {
        var success = await sessionService.SaveSession(request, userId: User.GetUserId());
        return success ? Ok() : BadRequest();
    }
}