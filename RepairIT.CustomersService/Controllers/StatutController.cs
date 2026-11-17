using Microsoft.AspNetCore.Mvc;

namespace RepairIT.CustomersService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatutController : ControllerBase
{
    // GET /api/statut  200 : indique que le service répond
    [HttpGet]
    public IActionResult ObtenirStatut()
    {
        return Ok(new { Service = "RepairIT.CustomersService", Description = "gestion des clients", Etat = "En construction" });
    }
}