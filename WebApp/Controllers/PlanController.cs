using System.Net;
using Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Public.DTO.Plan;
using Services.Contracts;

namespace BE_ChadApp.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class PlanController(IPlanService planService) : ControllerBase
{
    private IPlanService PlanService { get; set; } = planService;

    [HttpPost("New")]
    public async Task<IActionResult> CreatePlan([FromBody] CreatePlanRequest data)
    {
        var userId = User.GetUserId();
        var plan = await PlanService.CreatePlan(userId: userId, dto: data);
        return Ok(plan);
    }

    [HttpGet("Own/All")]
    public async Task<IActionResult> GetAllUserPlans()
    {
        var userId = User.GetUserId();
        var result = await PlanService.GetAllPlansList(userId);
        return Ok(result);
    }
}