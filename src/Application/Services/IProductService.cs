using Application.Dtos;

namespace Application.Services;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> ListAllAsync();
    Task<ProductDto> GetByIdAsync(int id);
    Task CreateAsync(ProductDto dto);
    Task DeleteAsync(int id);
    Task UpdateAsync(int id, ProductDto dto);
}
