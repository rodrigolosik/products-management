using Dapper;
using Domain.Models;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Infrastructure.Repositories;

public class ProductRepository : BaseRepository, IProductRepository
{
    private readonly ILogger<ProductRepository> _logger;
    private readonly IDbConnection _dbConnection;

    public ProductRepository(ILogger<ProductRepository> logger, IDbConnection dbConnection) : base(logger, dbConnection)
    {
        _logger = logger;
        _dbConnection = dbConnection;
    }

    public async Task CreateAsync(Product product)
    {
        try
        {
            TryOpenConnection();
            await _dbConnection.ExecuteAsync("INSERT INTO products (Name, Price, Quantity, Description) VALUES (@name, @price, @quantity, @description)", new
            {
                name = product.Name,
                price = product.Price,
                quantity = product.Quantity,
                description = product.Description
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    public async Task DeleteAsync(int id)
    {
        try
        {
            TryOpenConnection();
            await _dbConnection.ExecuteAsync("DELETE FROM products WHERE id = @id", new { id });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    public async Task<Product> GetByIdAsync(int id)
    {
        try
        {
            return await _dbConnection.QueryFirstOrDefaultAsync<Product>("SELECT * FROM products WHERE id = @id", new { id });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return null;
        }

    }

    public async Task<IEnumerable<Product>> ListAllAsync()
    {
        try
        {
            var products = await _dbConnection.QueryAsync<Product>("SELECT * FROM products");
            return products;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return null;
        }
    }

    public async Task UpdateAsync(int id, Product product)
    {
        try
        {
            TryOpenConnection();
            await _dbConnection.ExecuteAsync("UPDATE products SET Name = @name, Quantity = @quantity, Price = @price, Description = @description WHERE id = @id", new
            {
                id,
                name = product.Name,
                quantity = product.Quantity,
                price = product.Price,
                description = product.Description
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }
}
