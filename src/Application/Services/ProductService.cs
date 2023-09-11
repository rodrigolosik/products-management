using Application.Dtos;
using Domain.Models;
using Infrastructure.Repositories;
using Mapster;

namespace Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<ProductDto>> ListAllAsync()
    {
        var products = await _productRepository.ListAllAsync();

        var result = products.Adapt<IEnumerable<ProductDto>>();

        return result;
    }

    public async Task CreateAsync(ProductDto dto)
    {
        var product = dto.Adapt<Product>();

        await _productRepository.CreateAsync(product);
    }

    public async Task<ProductDto> GetByIdAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);

        return product.Adapt<ProductDto>();
    }

    public async Task DeleteAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);

        if (product is null) return;

        await _productRepository.DeleteAsync(id);
    }

    public async Task UpdateAsync(int id, ProductDto dto)
    {
        var product = dto.Adapt<Product>();

        await _productRepository.UpdateAsync(id, product);
    }
}
