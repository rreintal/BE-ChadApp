namespace Public.DTO;

public class RefreshTokenModel
{
    public string JWT { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
}