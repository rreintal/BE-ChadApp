using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using Domain;
using Domain.Database.Contracts;
using Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    private readonly IAppUow _uow;

    public UsersController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IConfiguration configuration, AppDbContext context, IAppUow uow)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _Configuration = configuration;
        _context = context;
        _uow = uow;
    }

    [HttpPost("Login")]
    public async Task<ActionResult<JWTResponse>> Login(Login data)
    {
        var user = await _userManager.FindByEmailAsync(data.Email);
        var userExists = user != null;
        if (!userExists)
        {
            return BadRequest(new RestApiResponse()
            {
                Message = "Username/password problem",
                Status = HttpStatusCode.BadRequest
            });
        }

        var signInResult = await _signInManager.CheckPasswordSignInAsync(user, data.Password, false);

        if (!signInResult.Succeeded)
        {
            return BadRequest(new RestApiResponse()
            {
                Message = "Username/password problem",
                Status = HttpStatusCode.BadRequest
            });
        }

        var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(user);
        var jwt = IdentityHelpers.GenerateJwt(
            claimsPrincipal.Claims,
            _Configuration.GetValue<string>("Jwt:Key")!,
            _Configuration.GetValue<string>("Jwt:Issuer")!,
            _Configuration.GetValue<string>("Jwt:Audience")!,
            _Configuration.GetValue<int>("Jwt:ExpirationTime")!
        );
        
        // TODO: remove expired refreshTokens!

        var refreshToken = new AppRefreshToken()
        {
            JWT = jwt,
            AppUserId = user.Id
        };

        var result = new JWTResponse()
        {
            JWT = jwt,
            RefreshToken = refreshToken.RefreshToken
        };
        
        await _context.RefreshTokens.AddAsync(refreshToken);
        await _context.SaveChangesAsync();
        
        return Ok(result);
    }
    
    
    [HttpPost("Register")]
    public async Task<ActionResult<JWTResponse>> Register(RegisterUser data)
    {
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

        var userExists = (await _context.AppUsers
            .FirstOrDefaultAsync(au => au.NormalizedEmail == data.Email.ToUpper())) != null;
        
        if (userExists)
        {
            return BadRequest(new RestApiResponse()
            {
                Message = "Username/password problem!",
                Status = HttpStatusCode.BadRequest
            });
        }
        
        var appUser = new AppUser()
        {
            Email = data.Email,
            AppRefreshTokens = new List<AppRefreshToken>() {},
            UserName = data.Email
        };
        var identityResult = await _userManager.CreateAsync(appUser, data.Password);

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
        
        await _context.RefreshTokens.AddAsync(refreshToken);
        await _context.SaveChangesAsync();
        
        return Ok(result);
    }

    
    [Authorize]
    [HttpPost("RefreshToken")]
    public async Task<ActionResult<JWTResponse>> RefreshToken(RefreshTokenModel data)
    {
        JwtSecurityToken jwtToken;
        try
        {
            jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(data.JWT);
            if (jwtToken == null)
            {
                return BadRequest(new RestApiResponse()
                {
                    Status = HttpStatusCode.BadRequest,
                    Message = "No token!"
                });
            }
        }
        catch (Exception e)
        {
            return BadRequest(new RestApiResponse()
            {
                Status = HttpStatusCode.BadRequest,
                Message = $"Cant parse token, {e.Message}"
            });
        }
        
        if (!IdentityHelpers.ValidateToken(
                data.JWT,
                _Configuration.GetValue<string>("Jwt:Key")!,
                _Configuration.GetValue<string>("Jwt:Issuer")!,
                _Configuration.GetValue<string>("Jwt:Audience")!))
        {
            return BadRequest(new RestApiResponse()
            {
                Status = HttpStatusCode.BadRequest,
                Message = $"JWT validation fail"
            });
        }
        
        var userEmail = jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
        if (userEmail == null)
        {
            return BadRequest(new RestApiResponse()
            {
                Status = HttpStatusCode.BadRequest,
                Message = "No email in jwt"
            });
        }
        
        var appUser = await _userManager.FindByEmailAsync(userEmail);
        if (appUser == null)
        {
            return BadRequest(new RestApiResponse()
            {
                Message = "User does not exist!",
                Status = HttpStatusCode.BadRequest
            });
        }
        var tokenFromDatabase = await _context.RefreshTokens
            .Where(t => t.RefreshToken == data.RefreshToken)
            .FirstOrDefaultAsync();
        
        if (tokenFromDatabase == null)
        {
            return BadRequest(new RestApiResponse()
            {
                Message = "Refresh token doesn't exist!",
                Status = HttpStatusCode.BadRequest
            });
        }

        var tokenIsExpired = tokenFromDatabase.ExpirtationDT < DateTime.UtcNow; 
        if (tokenIsExpired)
        {
            return BadRequest(new RestApiResponse()
            {
                Message = "Refresh token is expired!",
                Status = HttpStatusCode.BadRequest
            });
        }

        var userClaims = await _signInManager.CreateUserPrincipalAsync(appUser);
        var jwt = IdentityHelpers.GenerateJwt(
                userClaims.Claims,
                _Configuration.GetValue<string>("Jwt:Key")!,
                _Configuration.GetValue<string>("Jwt:Issuer")!,
                _Configuration.GetValue<string>("Jwt:Audience")!,
                _Configuration.GetValue<int>("Jwt:ExpirationTime")!);

        var refreshToken = new AppRefreshToken()
        {
            AppUserId = appUser.Id,
            JWT = jwt
        };
        
        _uow.AppRefreshTokenRepository.Delete(tokenFromDatabase);
        await _uow.AppRefreshTokenRepository.AddAsync(refreshToken);
        await _uow.SaveChangesAsync();

        var response = new JWTResponse()
        {
            JWT = refreshToken.JWT,
            RefreshToken = refreshToken.RefreshToken
        };

        return Ok(response);
    }
}