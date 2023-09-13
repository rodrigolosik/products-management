using Application.Dtos;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute.ExceptionExtensions;
using System.Net;
using WebApi.Controllers;

namespace UnityTests.WebApi.Controllers;

public sealed class ProductsControllerTests
{
    private readonly IProductService _productServiceMock;
    private readonly ILogger<ProductsController> _loggerMock;

    public ProductsControllerTests()
    {
        _productServiceMock = Substitute.For<IProductService>();
        _loggerMock = Substitute.For<ILogger<ProductsController>>();
    }

    [Fact]
    public async Task Get_ShouldReturn_Products_With_Success()
    {
        // Arrange
        Fixture fixture = new Fixture();
        var products = fixture.CreateMany<ProductDto>(5);

        var productsController = new ProductsController(_loggerMock, _productServiceMock);

        _productServiceMock.ListAllAsync().Returns(products);

        // Act
        var result = await productsController.Get();

        // Assert
        result
            .Should()
            .BeOfType<OkObjectResult>()
            .Which
            .Value
            .Should()
            .BeEquivalentTo(products);
    }

    [Fact]
    public async Task Get_Should_Throws_Exception_When_Trying_To_Get_Products()
    {
        // Arrange
        var productsController = new ProductsController(_loggerMock, _productServiceMock);

        _productServiceMock.ListAllAsync().Throws<Exception>();

        // Act
        var result = await productsController.Get();

        // Assert
        result
            .Should()
            .BeOfType<ObjectResult>()
            .Which
            .StatusCode
            .Should()
            .Be((int)HttpStatusCode.InternalServerError);
    }

    [Fact]
    public async Task GetById_ShouldReturn_Product_With_Success()
    {
        // Arrange
        int id = 1;
        Fixture fixture = new Fixture();
        var product = fixture.Create<ProductDto>();

        var productsController = new ProductsController(_loggerMock, _productServiceMock);

        _productServiceMock.GetByIdAsync(id).Returns(product);

        // Act
        var result = await productsController.GetById(id);

        // Assert
        result
            .Should()
            .BeOfType<OkObjectResult>()
            .Which
            .Value
            .Should()
            .BeEquivalentTo(product);
    }

    [Fact]
    public async Task GetById_Should_Throws_Exception_When_Trying_To_Get_Product()
    {
        // Arrange
        int id = 1;
        var productsController = new ProductsController(_loggerMock, _productServiceMock);

        _productServiceMock.GetByIdAsync(id).Throws<Exception>();

        // Act
        var result = await productsController.GetById(id);

        // Assert
        result
            .Should()
            .BeOfType<ObjectResult>()
            .Which
            .StatusCode
            .Should()
            .Be((int)HttpStatusCode.InternalServerError);
    }

    [Fact]
    public async Task GetById_Should_Return_BadRequest_When_Trying_To_Get_Product_Passing_Invalid_Product_Id()
    {
        // Arrange
        int id = 0;
        var productsController = new ProductsController(_loggerMock, _productServiceMock);
        await _productServiceMock.GetByIdAsync(id);

        // Act
        var result = await productsController.GetById(id);

        // Assert
        result
            .Should()
            .BeOfType<BadRequestObjectResult>()
            .Which
            .StatusCode
            .Should()
            .Be((int)HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Create_Should_Create_With_Success()
    {
        // Arrange
        Fixture fixture = new Fixture();
        var product = fixture.Create<ProductDto>();

        var productsController = new ProductsController(_loggerMock, _productServiceMock);

        await _productServiceMock.CreateAsync(product);

        // Act
        var result = await productsController.Create(product);

        // Assert
        result
            .Should()
            .BeOfType<CreatedResult>()
            .Which
            .Value
            .Should()
            .BeEquivalentTo(product);
    }

    [Fact]
    public async Task Create_Should_Throws_Exception_When_Trying_To_Create_Product()
    {
        // Arrange
        Fixture fixture = new Fixture();
        var product = fixture.Create<ProductDto>();
        var productsController = new ProductsController(_loggerMock, _productServiceMock);

        _productServiceMock.CreateAsync(product).Throws<Exception>();

        // Act
        var result = await productsController.Create(product);

        // Assert
        result
            .Should()
            .BeOfType<ObjectResult>()
            .Which
            .StatusCode
            .Should()
            .Be((int)HttpStatusCode.InternalServerError);
    }

    [Fact]
    public async Task Create_Should_Return_BadRequest_When_Trying_To_Create_Product_Passing_Invalid_Product()
    {
        // Arrange
        int id = 0;
        Fixture fixture = new Fixture();
        var product = fixture.Create<ProductDto>();
        var productsController = new ProductsController(_loggerMock, _productServiceMock);
        
        await _productServiceMock.CreateAsync(product);

        // Act
        var result = await productsController.Create(null);

        // Assert
        result
            .Should()
            .BeOfType<BadRequestObjectResult>()
            .Which
            .StatusCode
            .Should()
            .Be((int)HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Delete_Should_Delete_With_Success()
    {
        // Arrange
        int id = 1;
        Fixture fixture = new Fixture();
        var product = fixture.Create<ProductDto>();

        var productsController = new ProductsController(_loggerMock, _productServiceMock);

        await _productServiceMock.DeleteAsync(id);
        _productServiceMock.GetByIdAsync(id).Returns(product);

        // Act
        var result = await productsController.Delete(id);

        // Assert
        result
            .Should()
            .BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task Delete_Should_Throws_Exception_When_Trying_To_Delete_Product()
    {
        // Arrange
        int id = 1;
        var productsController = new ProductsController(_loggerMock, _productServiceMock);

        _productServiceMock.GetByIdAsync(id).Throws<Exception>();
        // Act
        var result = await productsController.Delete(id);

        // Assert
        result
            .Should()
            .BeOfType<ObjectResult>()
            .Which
            .StatusCode
            .Should()
            .Be((int)HttpStatusCode.InternalServerError);
    }

    [Fact]
    public async Task Delete_Should_Return_Bad_Request_When_Trying_To_Delete_Invalid_Product()
    {
        // Arrange
        int id = 1;
        var productsController = new ProductsController(_loggerMock, _productServiceMock);
        await _productServiceMock.GetByIdAsync(id);
        
        // Act
        var result = await productsController.Delete(id);

        // Assert
        result
            .Should()
            .BeOfType<BadRequestObjectResult>()
            .Which
            .StatusCode
            .Should()
            .Be((int)HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Update_Should_Update_With_Success()
    {
        // Arrange
        int id = 1;
        Fixture fixture = new Fixture();
        var product = fixture.Create<ProductDto>();

        var productsController = new ProductsController(_loggerMock, _productServiceMock);

        await _productServiceMock.UpdateAsync(id, product);

        // Act
        var result = await productsController.Update(id, product);

        // Assert
        result
            .Should()
            .BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task Update_Should_Throws_Exception_When_Trying_To_Update_Product()
    {
        // Arrange
        int id = 1;
        Fixture fixture = new Fixture();
        var product = fixture.Create<ProductDto>();
        var productsController = new ProductsController(_loggerMock, _productServiceMock);

        _productServiceMock.UpdateAsync(id, product).Throws<Exception>();

        // Act
        var result = await productsController.Update(id, product);

        // Assert
        result
            .Should()
            .BeOfType<ObjectResult>()
            .Which
            .StatusCode
            .Should()
            .Be((int)HttpStatusCode.InternalServerError);
    }

    [Fact]
    public async Task Update_Should_Return_BadRequest_When_Trying_To_Update_Product_Passing_Null_ProductDto()
    {
        // Arrange
        int id = 0;
        ProductDto? product = null;

        var productsController = new ProductsController(_loggerMock, _productServiceMock);

        await _productServiceMock.UpdateAsync(id, product);

        // Act
        var result = await productsController.Update(id, product);

        // Assert
        result
            .Should()
            .BeOfType<BadRequestObjectResult>()
            .Which
            .StatusCode
            .Should()
            .Be((int)HttpStatusCode.BadRequest);
    }

}
