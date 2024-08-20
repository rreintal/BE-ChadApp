using Domain.Database.Contracts;
using Domain.Database.Repositories;

namespace Domain.Database;

public class AppUOW : IAppUow
{
    private readonly AppDbContext _context;

    public AppUOW(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public AppRefreshTokenRepository AppRefreshTokenRepository =>
        _appRefreshTokenRepository ??= new AppRefreshTokenRepository(_context);
    
    private AppRefreshTokenRepository? _appRefreshTokenRepository;
}