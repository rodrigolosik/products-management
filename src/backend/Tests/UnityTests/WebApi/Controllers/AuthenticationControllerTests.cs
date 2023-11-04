using Domain.Models;
using Infrastructure.Token;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using WebApi.Controllers;
using WebApi.Models;

namespace UnityTests.WebApi.Controllers;

public class AuthenticationControllerTests
{

    private readonly ILogger<AuthenticationController> _loggerMock;
    private readonly IAccessManager _accessManagerMock;

    public AuthenticationControllerTests()
    {
        _loggerMock = Substitute.For<ILogger<AuthenticationController>>();
        _accessManagerMock = Substitute.For<IAccessManager>();
    }

    [Fact]
    public async Task Authenticate_Should_Return_Token_With_Success_When_LoginCredentials_Are_Valid()
    {
        // Arrange
        Fixture fixture = new Fixture();

        var loginModel = fixture.Create<Login>();

        _accessManagerMock.ValidateCredentialsAsync(loginModel.Email, loginModel.Password).Returns(true);

        _accessManagerMock.GenerateJwtToken(loginModel.Email).Returns("tokenboladao");

        var authenticationController = new AuthenticationController(_loggerMock, _accessManagerMock);

        // Act
        var result = await authenticationController.Authenticate(loginModel);

        // Assert
        result
            .Should()
            .BeOfType<OkObjectResult>()
            .Which
            .Value
            .Should()
            .BeEquivalentTo("tokenboladao");
    }

    [Fact]
    public async Task Authenticate_Should_Return_Unauthorized_When_LoginCredentials_Are_InValid()
    {
        // Arrange
        Fixture fixture = new Fixture();

        var loginModel = fixture.Create<Login>();

        _accessManagerMock.ValidateCredentialsAsync(loginModel.Email, loginModel.Password).Returns(false);

        var authenticationController = new AuthenticationController(_loggerMock, _accessManagerMock);

        // Act
        var result = await authenticationController.Authenticate(loginModel);

        // Assert
        result
            .Should()
            .BeOfType<UnauthorizedObjectResult>()
            .Which
            .StatusCode
            .Should()
            .Be((int)HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Create_Should_Return_Success_When_Provide_Valid_User_Data()
    {
        Fixture fixture = new Fixture();

        var user = fixture.Create<User>();

        await _accessManagerMock.CreateUser(user);

        var authenticationController = new AuthenticationController(_loggerMock, _accessManagerMock);

        var result = await authenticationController.Create(user);

        result
            .Should()
            .BeOfType<CreatedResult>()
            .Which
            .StatusCode
            .Should()
            .Be((int)HttpStatusCode.Created);

    }

}
