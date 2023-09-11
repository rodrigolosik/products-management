namespace Infrastructure.Configurations;

public class TokenConfiguration
{
    public string? Issuer { get; set; }
    public string? Audience { get; set; }
    public string? SecretJwtKey { get; set; }
}
