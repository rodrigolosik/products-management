using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Infrastructure.Security;

public class SigningConfiguration
{
    public Guid Id { get; } = Guid.NewGuid();
    public SecurityKey Key { get; }
    public SigningCredentials SigningCredentials { get; }

    public SigningConfiguration(string secretJwtKey)
    {
        Key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(secretJwtKey));

        SigningCredentials = new(
            Key, SecurityAlgorithms.HmacSha256Signature);
    }
}
