using Microsoft.AspNetCore.Identity;

namespace Domain;

public class AppUser : IdentityUser<Guid>
{
    public ICollection<AppRefreshToken>? AppRefreshTokens { get; set; }
}