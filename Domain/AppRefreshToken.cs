namespace Domain;

public class AppRefreshToken
{
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
    
    public Guid Id { get; set; }
    public string JWT { get; set; } = default!;
    public string RefreshToken { get; set; } = Guid.NewGuid().ToString();

    public DateTime ExpirtationDT { get; set; } = DateTime.UtcNow.AddDays(7);
}