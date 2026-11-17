using Microsoft.AspNetCore.Mvc;
using RepairIT.Shared.Dtos;
using RepairIT.UsersService.Services;

namespace RepairIT.UsersService.Controllers;

// API /api/utilisateurs
[ApiController]
[Route("api/[controller]")]
public class UtilisateursController : ControllerBase
{
    private readonly UtilisateurService _service;

    public UtilisateursController(UtilisateurService service)
    {
        _service = service;
    }

    // GET /api/utilisateurs : 200 (liste des users)
    [HttpGet]
    public async Task<IActionResult> Lister()
    {
        return Ok(await _service.ListerAsync());
    }

    // GET /api/utilisateurs/{id} : 200 si trouvé, 404 sinon
    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenirParId(int id)
    {
        var utilisateur = await _service.ObtenirParIdAsync(id);
        if (utilisateur == null)
            return NotFound();

        return Ok(utilisateur);
    }

    // POST /api/utilisateurs  201 : utilisateur créé /  409 :  si l'e-mail est déjà utilisé
    [HttpPost]
    public async Task<IActionResult> Creer(UtilisateurCreationDto dto)
    {
        var cree = await _service.CreerAsync(dto);
        if (cree == null)
            return Conflict("Un utilisateur utilise déjà cette adresse e-mail.");

        return CreatedAtAction(nameof(ObtenirParId), new { id = cree.Id }, cree);
    }
}