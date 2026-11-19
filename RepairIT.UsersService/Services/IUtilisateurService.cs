using RepairIT.Shared.Dtos;
using RepairIT.Shared.Modeles;

namespace RepairIT.UsersService.Services;

public interface IUtilisateurService
{
    Task<List<Utilisateur>> ListerAsync();

    Task<Utilisateur?> ObtenirParIdAsync(int id);

    Task<Utilisateur?> CreerAsync(UtilisateurCreationDto dto);
}
