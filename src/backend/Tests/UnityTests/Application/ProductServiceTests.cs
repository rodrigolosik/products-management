using Application.Dtos;
using Application.Services;
using Domain.Models;
using Infrastructure.Repositories;

namespace UnityTests.Application;

public sealed class ProductServiceTests
{
    private readonly IProductRepository _productRepositoryMock;

    public ProductServiceTests()
    {
        _productRepositoryMock = Substitute.For<IProductRepository>();
    }

    [Fact]
    public async Task ListAllAsync_ShouldReturn_ListOfProducts()
    {

        // Arrange
        var autoFixture = new Fixture();
        var products = autoFixture.Build<Product>().CreateMany(5);

        var productService = new ProductService(_productRepositoryMock);

        _productRepositoryMock.ListAllAsync().Returns(products);

        // Act
        var result = await productService.ListAllAsync();

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().HaveCount(5);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturn_Product()
    {

        // Arrange
        var autoFixture = new Fixture();
        var product = autoFixture.Build<Product>().Create();

        var productService = new ProductService(_productRepositoryMock);

        _productRepositoryMock.GetByIdAsync(1).Returns(product);

        // Act
        var result = await productService.GetByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateProduct_Successfully()
    {
        // Arrange
        var autoFixture = new Fixture();
        var productDto = autoFixture.Build<ProductDto>().Create();
        var product = autoFixture.Build<Product>().Create();

        var productService = new ProductService(_productRepositoryMock);

        await _productRepositoryMock.CreateAsync(product);

        // Act
       await productService.CreateAsync(productDto);

        // Assert
        _productRepositoryMock.Received(1);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteProduct_Successfully()
    {
        // Arrange
        int id = 1;
        var autoFixture = new Fixture();
        var product = autoFixture.Create<Product>();

        var productService = new ProductService(_productRepositoryMock);

        await _productRepositoryMock.DeleteAsync(id);
        _productRepositoryMock.GetByIdAsync(id).Returns(product);

        // Act
        await productService.DeleteAsync(id);

        // Assert
        _productRepositoryMock.Received(1);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateProduct_Successfully()
    {
        // Arrange
        int id = 1;
        var autoFixture = new Fixture();
        var productDto = autoFixture.Build<ProductDto>().Create();
        var product = autoFixture.Build<Product>().Create();

        var productService = new ProductService(_productRepositoryMock);

        await _productRepositoryMock.UpdateAsync(id, product);

        // Act
        await productService.UpdateAsync(id, productDto);

        // Assert
        _productRepositoryMock.Received(1);
    }
}
