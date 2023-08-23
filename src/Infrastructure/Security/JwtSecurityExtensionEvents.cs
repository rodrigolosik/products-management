using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Infrastructure.Security;

public class JwtSecurityExtensionEvents : JwtBearerEvents
{

    private readonly ILogger<JwtSecurityExtensionEvents> _logger;

    public JwtSecurityExtensionEvents(ILogger<JwtSecurityExtensionEvents> logger)
    {
        _logger = logger;
    }

    public override Task Challenge(JwtBearerChallengeContext context)
    {
        _logger.LogError("Token invalido, expirado ou nao informado...");
        return base.Challenge(context);
    }
}
