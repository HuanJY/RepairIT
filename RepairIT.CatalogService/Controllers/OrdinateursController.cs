using Microsoft.AspNetCore.Mvc;
using RepairIT.CatalogService.Services;
using RepairIT.Shared.Modeles;

namespace RepairIT.CatalogService.Controllers;

// api catalogue : /api/ordinateurs
[ApiController]
[Route("api/[controller]")]
public class OrdinateursController : ControllerBase
{
    private readonly IOrdinateurService _service;
    private readonly ILogger<OrdinateursController> _logger;

    // Le service et le logger sont fournis par l'injection de dépendances
    public OrdinateursController(IOrdinateurService service, ILogger<OrdinateursController> logger)
    {
        _service = service;
        _logger = logger;
    }
    // get /api/ordinateurs 200 : liste du catalogue
    [HttpGet]
    public async Task<IActionResult> Lister()
    {
        _logger.LogInformation("Liste des ordinateurs demandée");
        return Ok(await _service.ListerAsync());
    }

    // get /api/ordinateurs/{id} 200 si trouvé, 404 sinon
    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenirParId(int id)
    {
        _logger.LogInformation("Ordinateur demandé avec ID {Id}", id);
        var ordinateur = await _service.ObtenirParIdAsync(id);
        if (ordinateur == null)
        {
            _logger.LogWarning("Ordinateur avec ID {Id} non trouvé", id);
            return NotFound();
        }
        _logger.LogInformation("Ordinateur avec ID {Id} trouvé", id);
        return Ok(ordinateur);
    }

    // post /api/ordinateurs  201 : ordinateur créé ou 400 si les données sont invalides
    [HttpPost]
    public async Task<IActionResult> Ajouter(Ordinateur ordinateur)
    {
        _logger.LogInformation("Ajout d'un nouvel ordinateur");
        var cree = await _service.AjouterAsync(ordinateur);
        _logger.LogInformation("Ordinateur créé avec ID {Id}", cree.Id);
        return CreatedAtAction(nameof(ObtenirParId), new { id = cree.Id }, cree);
    }

    // put /api/ordinateurs/{id}  204 si modifié et 404 si introuvable
    [HttpPut("{id:int}")]
    public async Task<IActionResult> MettreAJour(int id, Ordinateur ordinateur)
    {
        _logger.LogInformation("Mise à jour de l'ordinateur avec ID {Id}", id);
        if (!await _service.MettreAJourAsync(id, ordinateur))
        {
            _logger.LogWarning("Ordinateur avec ID {Id} non trouvé pour mise à jour", id);
            return NotFound();
        }

        _logger.LogInformation("Ordinateur avec ID {Id} mis à jour", id);
        return NoContent();
    }

    // delete /api/ordinateurs/{id} 204 si supprimé et404 si introuvable
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Supprimer(int id)
    {
        _logger.LogInformation("Suppression de l'ordinateur avec ID {Id}", id);
        if (!await _service.SupprimerAsync(id))
        {
            _logger.LogWarning("Ordinateur avec ID {Id} non trouvé pour suppression", id);
            return NotFound();
        }

        _logger.LogInformation("Ordinateur avec ID {Id} supprimé", id);
        return NoContent();
    }
}