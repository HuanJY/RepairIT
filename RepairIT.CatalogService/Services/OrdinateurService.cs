using Microsoft.EntityFrameworkCore;
using RepairIT.CatalogService.Data;
using RepairIT.Shared.Modeles;
using RepairIT.Shared.Modeles;
using RepairIT.CatalogService.Services;
namespace RepairIT.CatalogService.Services;

public class OrdinateurService : IOrdinateurService
{
    private readonly CatalogContext _context;
    private readonly ILogger<OrdinateurService> _logger;

    // Le contexte et le logger sont fournis par l'injection de dépendances
    public OrdinateurService(CatalogContext context, ILogger<OrdinateurService> logger)
    {
        _context = context;
        _logger = logger;
    }

    // renvoie tous les ordi triés par marque puis par nom
    public async Task<List<Ordinateur>> ListerAsync()
    {
        return await _context.Ordinateurs
            .OrderBy(o => o.Marque)
            .ThenBy(o => o.Nom)
            .ToListAsync();
    }

    // renvoi l'ordi correspondant à l'Id, ou null s'il n'existe pas
    public async Task<Ordinateur?> ObtenirParIdAsync(int id)
    {
        return await _context.Ordinateurs.FirstOrDefaultAsync(o => o.Id == id);
    }

    // aj un ordinateur et le renvoie avec l'Id généré
    public async Task<Ordinateur> AjouterAsync(Ordinateur ordinateur)
    {
        // L'Id est toujours gen par la base
        ordinateur.Id = 0;

        _context.Ordinateurs.Add(ordinateur);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Ordinateur créé avec ID {Id}", ordinateur.Id);
        return ordinateur;
    }

    // met à jour un ordinateur ; false sinon
    public async Task<bool> MettreAJourAsync(int id, Ordinateur modifie)
    {
        var existant = await ObtenirParIdAsync(id);
        if (existant == null)
            return false;

        CopierInformations(modifie, existant);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Ordinateur avec ID {Id} mis à jour", id);
        return true;
    }

    // supp un ordinateur ; false s'il n'existe pas
    public async Task<bool> SupprimerAsync(int id)
    {
        var existant = await ObtenirParIdAsync(id);
        if (existant == null)
            return false;

        _context.Ordinateurs.Remove(existant);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Ordinateur avec ID {Id} supprimé", id);
        return true;
    }

    // copie les infos modifiables (id ne change pas)
    private static void CopierInformations(Ordinateur source, Ordinateur cible)
    {
        cible.Nom = source.Nom;
        cible.Marque = source.Marque;
        cible.Processeur = source.Processeur;
        cible.RamGo = source.RamGo;
        cible.StockageGo = source.StockageGo;
        cible.Type = source.Type;
        cible.Prix = source.Prix;
        cible.Stock = source.Stock;
    }
}