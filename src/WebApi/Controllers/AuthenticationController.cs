using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebApi.Security;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly ILogger<AuthenticationController> _logger;
    private readonly AccessManager _accessManager;

    public AuthenticationController(ILogger<AuthenticationController> logger, AccessManager accessManager)
    {
        _logger = logger;
        _accessManager = accessManager;
    }

    [AllowAnonymous]
    [HttpPost]
    [ProducesResponseType(typeof(Token), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public ActionResult<Token> Post([FromBody] User usuario)
    {
        _logger.LogInformation($"Recebida solicitação para o usuário: {usuario?.UserID}");

        if (usuario is not null && _accessManager.ValidateCredentials(usuario))
        {
            _logger.LogInformation($"Sucesso na autenticação do usuário: {usuario.UserID}");
            return _accessManager.GenerateToken(usuario);
        }
        else
        {
            _logger.LogError($"Falha na autenticação do usuário: {usuario?.UserID}");
            return new UnauthorizedResult();
        }
    }
}
