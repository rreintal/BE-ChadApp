using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Public.DTO;
using Services;

namespace BE_ChadApp.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class ExercisesController : ControllerBase
{
    private readonly AppDbContext _DbContext;

    public ExercisesController(AppDbContext dbContext)
    {
        _DbContext = dbContext;
    }

    [HttpGet("All")]
    public async Task<ActionResult<IEnumerable<GetExercise>>> Get()
    {
        var result = (await _DbContext.Exercises.ToListAsync()).Select(e => ExerciseMapper.Map(e));
        return Ok(result); 
    }
}