using Microsoft.AspNetCore.Mvc;
using RepairIT.CatalogService.Services;
using RepairIT.Shared.Modeles;

namespace RepairIT.CatalogService.Controllers;

// api catalogue : /api/ordinateurs
[ApiController]
[Route("api/[controller]")]
public class OrdinateursController : ControllerBase
{
    private readonly OrdinateurService _service;

    public OrdinateursController(OrdinateurService service)
    {
        _service = service;
    }

    // get /api/ordinateurs 200 : liste du catalogue
    [HttpGet]
    public async Task<IActionResult> Lister()
    {
        return Ok(await _service.ListerAsync());
    }

    // get /api/ordinateurs/{id} 200 si trouvé, 404 sinon
    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenirParId(int id)
    {
        var ordinateur = await _service.ObtenirParIdAsync(id);
        if (ordinateur == null)
            return NotFound();

        return Ok(ordinateur);
    }

    // post /api/ordinateurs  201 : ordinateur créé ou 400 si les données sont invalides
    [HttpPost]
    public async Task<IActionResult> Ajouter(Ordinateur ordinateur)
    {
        var cree = await _service.AjouterAsync(ordinateur);
        return CreatedAtAction(nameof(ObtenirParId), new { id = cree.Id }, cree);
    }

    // put /api/ordinateurs/{id}  204 si modifié et 404 si introuvable
    [HttpPut("{id:int}")]
    public async Task<IActionResult> MettreAJour(int id, Ordinateur ordinateur)
    {
        if (!await _service.MettreAJourAsync(id, ordinateur))
            return NotFound();

        return NoContent();
    }

    // delete /api/ordinateurs/{id} 204 si supprimé et404 si introuvable
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Supprimer(int id)
    {
        if (!await _service.SupprimerAsync(id))
            return NotFound();

        return NoContent();
    }
}