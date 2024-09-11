using Domain.Database.Contracts;
using Microsoft.AspNetCore.Identity;

namespace Domain;

public class AppUser : IdentityUser<Guid>, IDomainEntityId<Guid>
{
    public ICollection<AppRefreshToken>? AppRefreshTokens { get; set; }
}