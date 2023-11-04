using Domain.Models;

namespace Infrastructure.Token;

public interface IAccessManager
{
    Task CreateUser(User user);
    Task<bool> ValidateCredentialsAsync(string email, string password);
    bool ValidateToken(string token);
    string GenerateJwtToken(string email);
}