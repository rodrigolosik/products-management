using Dapper;
using Domain.Models;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Infrastructure.Repositories;

public class UserRepository : BaseRepository, IUserRepository
{
    private readonly ILogger<UserRepository> _logger;
    private readonly IDbConnection _connection;

    public UserRepository(IDbConnection connection, ILogger<UserRepository> logger) : base(logger, connection)
    {
        _logger = logger;
        _connection = connection;
    }

    public async Task CreateAsync(User user)
    {
        try
        {
            TryOpenConnection();
            await _connection.ExecuteAsync("INSERT INTO users (Name, Email, Password) VALUES (@name, @email, @password)", new
            {
                name = user.Name,
                email = user.Email,
                password = user.Password
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        finally
        {
            _connection.Close();
        }
    }

    public async Task<User?> ValidateCredentialsAsync(string email, string password)
    {
        try
        {
            TryOpenConnection();
            return await _connection.QueryFirstOrDefaultAsync<User>("SELECT * FROM users WHERE email = @email AND password = @password", new
            {
                email,
                password
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return null;
        }
        finally
        {
            _connection.Close();
        }
    }
}
