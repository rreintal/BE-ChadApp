using System.Security.Claims;
using Domain.Database.Contracts;
using Microsoft.AspNetCore.Identity;

namespace Domain.Domain;

public class AppUser : IdentityUser<Guid>, IDomainEntityId<Guid>
{
    public ICollection<AppRefreshToken> AppRefreshTokens { get; set; } = new List<AppRefreshToken>();
}

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        var id = user.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                 ?? throw new UnauthorizedAccessException("User id claim missing");

        return Guid.Parse(id);
    }
}