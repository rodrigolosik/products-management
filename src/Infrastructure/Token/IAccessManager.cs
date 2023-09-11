using Domain.Models;

namespace Infrastructure.Token;

public interface IAccessManager
{
    Task CreateUser(User user);
    Task<bool> ValidateCredentials(string email, string password);
    string GenerateJwtToken(string email);
}