using System.Net.Http.Json;
using RepairIT.Shared.Modeles;

namespace RepairIT.Client.Services;

public class OrdinateurApiService : IOrdinateurApiService
{
    private const string Route = "api/ordinateurs";
    private readonly HttpClient _httpClient;

    public OrdinateurApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Ordinateur>> ListerAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<Ordinateur>>(Route) ?? [];
    }

    public async Task<bool> AjouterAsync(Ordinateur ordinateur)
    {
        var reponse = await _httpClient.PostAsJsonAsync(Route, ordinateur);
        return reponse.IsSuccessStatusCode;
    }

    public async Task<bool> MettreAJourAsync(Ordinateur ordinateur)
    {
        var reponse = await _httpClient.PutAsJsonAsync($"{Route}/{ordinateur.Id}", ordinateur);
        return reponse.IsSuccessStatusCode;
    }

    public async Task<bool> SupprimerAsync(int id)
    {
        var reponse = await _httpClient.DeleteAsync($"{Route}/{id}");
        return reponse.IsSuccessStatusCode;
    }
}
