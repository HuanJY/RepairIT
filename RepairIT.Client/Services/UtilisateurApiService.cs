using System.Net;
using System.Net.Http.Json;
using RepairIT.Shared.Dtos;
using RepairIT.Shared.Modeles;

namespace RepairIT.Client.Services;

public class UtilisateurApiService : IUtilisateurApiService
{
    private const string Route = "api/utilisateurs";
    private readonly HttpClient _httpClient;

    public UtilisateurApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Utilisateur>> ListerAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<Utilisateur>>(Route) ?? [];
    }

    public async Task<HttpStatusCode> CreerAsync(UtilisateurCreationDto dto)
    {
        var reponse = await _httpClient.PostAsJsonAsync(Route, dto);
        return reponse.StatusCode;
    }
}
