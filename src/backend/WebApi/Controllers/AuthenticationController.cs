using Domain.Models;
using Infrastructure.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebApi.Models;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class AuthenticationController : ControllerBase
{
    private readonly ILogger<AuthenticationController> _logger;
    private readonly IAccessManager _accessManager;

    public AuthenticationController(ILogger<AuthenticationController> logger, IAccessManager accessManager)
    {
        _logger = logger;
        _accessManager = accessManager;
    }

    [HttpPost("auth")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> Authenticate([FromBody] Login model)
    {
        var isValid = await _accessManager.ValidateCredentialsAsync(model?.Email, model?.Password);

        if (!isValid)
            return Unauthorized("Credentials are invalid");

        var token = _accessManager.GenerateJwtToken(model.Email);

        return Ok(token);
    }

    [HttpPost("create")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> Create([FromBody] User user)
    {
        await _accessManager.CreateUser(user);
        return Created("", user);
    }

    [HttpGet("validate/{token}")]
    public IActionResult ValidateToken(string token) {
        var isValid = _accessManager.ValidateToken(token);
        return Ok(isValid);
    }
}
