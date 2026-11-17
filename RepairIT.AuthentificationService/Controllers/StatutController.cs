using Microsoft.AspNetCore.Mvc;

namespace RepairIT.AuthentificationService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatutController : ControllerBase
{
    // GET /api/statut  200 : indique que le service répond
    [HttpGet]
    public IActionResult ObtenirStatut()
    {
        return Ok(new { Service = "RepairIT.AuthentificationService", Description = "authentification via Keycloak", Etat = "En construction" });
    }
}