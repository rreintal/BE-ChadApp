using Domain.Database.Base;

namespace Domain.Database.Repositories;

public class AppRefreshTokenRepository : BaseRepository<AppDbContext, AppRefreshToken>
{
    public AppRefreshTokenRepository(AppDbContext context) : base(context)
    {
    }
}