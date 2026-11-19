using System.Net;
using RepairIT.Shared.Dtos;
using RepairIT.Shared.Modeles;

namespace RepairIT.Client.Services;

public interface IUtilisateurApiService
{
    Task<List<Utilisateur>> ListerAsync();

    Task<HttpStatusCode> CreerAsync(UtilisateurCreationDto dto);
}
