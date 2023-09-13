using Domain.Models;

namespace Infrastructure.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<Product>> ListAllAsync();
    Task<Product> GetByIdAsync(int id);
    Task CreateAsync(Product product);
    Task DeleteAsync(int id);
    Task UpdateAsync(int id, Product product);
}
