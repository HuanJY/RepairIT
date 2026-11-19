using RepairIT.Shared.Modeles;

namespace RepairIT.Client.Services;

public interface IOrdinateurApiService
{
    Task<List<Ordinateur>> ListerAsync();
    Task<bool> AjouterAsync(Ordinateur ordinateur);
    Task<bool> MettreAJourAsync(Ordinateur ordinateur);
    Task<bool> SupprimerAsync(int id);
}
