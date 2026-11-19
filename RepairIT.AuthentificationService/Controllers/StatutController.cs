using Microsoft.AspNetCore.Mvc;

namespace RepairIT.AuthentificationService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatutController : ControllerBase
{
    private readonly ILogger<StatutController> _logger;

    public StatutController(ILogger<StatutController> logger)
    {
        _logger = logger;
    }

    // GET /api/statut → 200 : indique que le service répond
    [HttpGet]
    public IActionResult ObtenirStatut()
    {
        _logger.LogInformation("Statut demandé");
        return Ok(new { Service = "RepairIT.AuthentificationService", Description = "authentification via Keycloak", Etat = "En construction" });
    }
}
