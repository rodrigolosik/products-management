using Domain.Models;
using Infrastructure.Configurations;
using Infrastructure.Repositories;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Token;

public sealed class AccessManager : IAccessManager
{
    private readonly TokenConfiguration _tokenConfiguration;
    private readonly IUserRepository _userRepository;

    public AccessManager(IOptions<TokenConfiguration> options, IUserRepository userRepository)
    {
        _tokenConfiguration = options.Value;
        _userRepository = userRepository;
    }

    public async Task CreateUser(User user)
    {
        await _userRepository.CreateAsync(user);
    }

    public async Task<bool> ValidateCredentialsAsync(string email, string password)
    {
        var user = await _userRepository.ValidateCredentialsAsync(email, password);

        return user is not null;
    }


    public string GenerateJwtToken(string email)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenConfiguration.SecretJwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _tokenConfiguration.Issuer,
            audience: _tokenConfiguration.Audience,
            claims: new[] { new Claim(ClaimTypes.Email, email) },
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public bool ValidateToken(string token)
    {
        try
        {
            var publicKey = _tokenConfiguration.SecretJwtKey;

            var tokenValidationParameters = new TokenValidationParameters()
            {
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validationToken);

            var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}
