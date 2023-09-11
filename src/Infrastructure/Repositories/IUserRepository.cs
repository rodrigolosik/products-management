using Domain.Models;

namespace Infrastructure.Repositories;

public interface IUserRepository
{
    Task CreateAsync(User user);
    Task<User?> ValidateCredentialsAsync(string email, string password);

}
