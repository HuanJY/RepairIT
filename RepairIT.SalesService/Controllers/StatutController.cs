using Microsoft.AspNetCore.Mvc;

namespace RepairIT.SalesService.Controllers;


[ApiController]
[Route("api/[controller]")]
public class StatutController : ControllerBase
{
    // GET /api/statut  200 : indique que le service répond
    [HttpGet]
    public IActionResult ObtenirStatut()
    {
        return Ok(new { Service = "RepairIT.SalesService", Description = "gestion des ventes", Etat = "En construction" });
    }
}