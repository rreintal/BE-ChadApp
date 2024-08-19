using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Domain.Database;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<AppDbContext>();
        // this is needed for creating the AppDbContext object for "dotnet ef" CLI tool
        builder.UseNpgsql("Host=localhost:5432;Database=trainingApp;Username=admin;Password=development;Port=5432;");
        
        return new AppDbContext(builder.Options);
    }
}