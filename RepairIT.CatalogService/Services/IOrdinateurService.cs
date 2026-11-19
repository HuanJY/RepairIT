using RepairIT.Shared.Modeles;

namespace RepairIT.CatalogService.Services;

public interface IOrdinateurService
{
    Task<List<Ordinateur>> ListerAsync();

    Task<Ordinateur?> ObtenirParIdAsync(int id);

    Task<Ordinateur> AjouterAsync(Ordinateur ordinateur);

    Task<bool> MettreAJourAsync(int id, Ordinateur modifie);

    Task<bool> SupprimerAsync(int id);
}
