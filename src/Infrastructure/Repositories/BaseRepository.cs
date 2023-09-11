using Microsoft.Extensions.Logging;
using System.Data;

namespace Infrastructure.Repositories
{
    public abstract class BaseRepository
    {
        private readonly ILogger<BaseRepository> _logger;
        private readonly IDbConnection _connection;

        public BaseRepository(ILogger<BaseRepository> logger, IDbConnection connection)
        {
            _logger = logger;
            _connection = connection;
        }

        protected void TryOpenConnection()
        {
            _logger.LogInformation("Trying to open the connection");
            if (_connection.State != ConnectionState.Open)
            {
                _logger.LogInformation("Opening db connection");
                _connection.Open();
                _logger.LogInformation("Connection opened successfully");
            }
        }
    }
}
