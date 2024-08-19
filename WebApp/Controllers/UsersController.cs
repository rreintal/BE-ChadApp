using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mail;
using Domain;
using Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Public.DTO;

namespace BE_ChadApp.Controllers;

//[Route("api/v{version:apiVersion}/[controller]")]
[Route("api/v1/[controller]")]
[ApiController]
//[ApiVersion("1")]
public class UsersController : ControllerBase
{
    
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IConfiguration _Configuration;
    private readonly AppDbContext _context;

    public UsersController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IConfiguration configuration, AppDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _Configuration = configuration;
        _context = context;
    }
    
    
    [HttpPost]
    public async Task<ActionResult<JWTResponse>> Register(RegisterUser data)
    {
        // check if user already exists!
        
        MailAddress? email;
        MailAddress.TryCreate(data.Email, out email);
        var isValidEmail = email != null;
        if (!isValidEmail)
        {
            var invalidEmailResponse = new RestApiResponse()
            {
                Message = "Invalid email",
                Status = HttpStatusCode.BadRequest
            };
            return BadRequest(invalidEmailResponse);
        }
        
        var appUser = new AppUser()
        {
            Email = data.Email,
            AppRefreshTokens = new List<AppRefreshToken>() {},
            UserName = data.Email
        };
        var identityResult = await _userManager.CreateAsync(appUser);

        if (!identityResult.Succeeded)
        {
            return BadRequest(new RestApiResponse()
            {
                Message = string.Join(",",identityResult.Errors.Select(e => e.Description)),
                Status = HttpStatusCode.BadRequest
            });
        };
        
        
        var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(appUser);
        var jwt = IdentityHelpers.GenerateJwt(
            claimsPrincipal.Claims,
            _Configuration.GetValue<string>("Jwt:Key")!,
            _Configuration.GetValue<string>("Jwt:Issuer")!,
            _Configuration.GetValue<string>("Jwt:Audience")!,
            _Configuration.GetValue<int>("Jwt:ExpirationTime")!
        );

        var refreshToken = new AppRefreshToken()
        {
            JWT = jwt,
            AppUser = appUser
        };
        
        var result = new JWTResponse()
        {
            JWT = jwt,
            RefreshToken = refreshToken.RefreshToken
        };

        await _context.SaveChangesAsync();
        return Ok(result);
    }
}