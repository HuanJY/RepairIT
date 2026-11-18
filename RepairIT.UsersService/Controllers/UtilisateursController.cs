using Microsoft.AspNetCore.Mvc;
using RepairIT.Shared.Dtos;
using RepairIT.UsersService.Services;

namespace RepairIT.UsersService.Controllers;

// API /api/utilisateurs
[ApiController]
[Route("api/[controller]")]
public class UtilisateursController : ControllerBase
{
    private readonly IUtilisateurService _service;
    private readonly ILogger<UtilisateursController> _logger;

    public UtilisateursController(IUtilisateurService service, ILogger<UtilisateursController> logger)
    {
        _service = service;
        _logger = logger;
    }

    // GET /api/utilisateurs : 200 (liste des users)
    [HttpGet]
    public async Task<IActionResult> Lister()
    {
        _logger.LogInformation("Liste des utilisateurs demandée");
        return Ok(await _service.ListerAsync());
    }

    // GET /api/utilisateurs/{id} : 200 si trouvé, 404 sinon
    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenirParId(int id)
    {
        _logger.LogInformation("Utilisateur demandé avec ID {Id}", id);
        var utilisateur = await _service.ObtenirParIdAsync(id);
        if (utilisateur == null)
        {
            _logger.LogWarning("Utilisateur avec ID {Id} non trouvé", id);
            return NotFound();
        }
        _logger.LogInformation("Utilisateur avec ID {Id} trouvé", id);
        return Ok(utilisateur);
    }

    // POST /api/utilisateurs  201 : utilisateur créé /  409 :  si l'e-mail est déjà utilisé
    [HttpPost]
    public async Task<IActionResult> Creer(UtilisateurCreationDto dto)
    {
        _logger.LogInformation("Création d'un nouvel utilisateur avec l'adresse e-mail {Email}", dto.Email);
        var cree = await _service.CreerAsync(dto);
        if (cree == null)
        {
            _logger.LogWarning("Conflit lors de la création de l'utilisateur : l'adresse e-mail est déjà utilisée");
            return Conflict("Un utilisateur utilise déjà cette adresse e-mail.");
        }
        _logger.LogInformation("Utilisateur créé avec ID {Id}", cree.Id);
        return CreatedAtAction(nameof(ObtenirParId), new { id = cree.Id }, cree);
    }
}