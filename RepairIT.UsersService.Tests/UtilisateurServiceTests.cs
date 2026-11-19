using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using RepairIT.Shared.Dtos;
using RepairIT.Shared.Enums;
using RepairIT.Shared.Modeles;
using RepairIT.UsersService.Data;
using RepairIT.UsersService.Services;

namespace RepairIT.UsersService.Tests;

// Tests unitaires du service des utilisateurs 
public class UtilisateurServiceTests
{
    private readonly UtilisateurService _service;

    public UtilisateurServiceTests()
    {
        var options = new DbContextOptionsBuilder<UsersContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _service = new UtilisateurService(new UsersContext(options), NullLogger<UtilisateurService>.Instance);
    }

    private static UtilisateurCreationDto CreerDto(string nom = "Martin", string email = "alice.martin@repairit.fr")
    {
        return new UtilisateurCreationDto
        {
            Nom = nom,
            Prenom = "Alice",
            Email = email,
            Role = RoleUtilisateur.Administrateur
        };
    }

    [Fact]
    public async Task CreerAsync_NouvelUtilisateur_ValeursParDefautAppliquees()
    {
        var cree = await _service.CreerAsync(CreerDto());

        Assert.NotNull(cree);
        Assert.True(cree.Id > 0);
        Assert.Equal(Utilisateur.UserKeycloakParDefaut, cree.UserKeycloak);
        Assert.True(cree.EstActif);
        Assert.Equal(RoleUtilisateur.Administrateur, cree.Role);
    }

    [Fact]
    public async Task CreerAsync_EmailAvecMajusculesEtEspaces_EstNormalise()
    {
        var cree = await _service.CreerAsync(CreerDto(email: "  Alice.Martin@RepairIT.fr "));

        Assert.Equal("alice.martin@repairit.fr", cree!.Email);
    }

    [Fact]
    public async Task CreerAsync_EmailDejaUtilise_RetourneNull()
    {
        await _service.CreerAsync(CreerDto());

        var doublon = await _service.CreerAsync(CreerDto(nom: "Autre", email: "ALICE.MARTIN@repairit.fr"));

        Assert.Null(doublon);
        Assert.Single(await _service.ListerAsync());
    }

    [Fact]
    public async Task ListerAsync_PlusieursUtilisateurs_TriesParNom()
    {
        await _service.CreerAsync(CreerDto("Martin", "a@repairit.fr"));
        await _service.CreerAsync(CreerDto("Durand", "b@repairit.fr"));

        var liste = await _service.ListerAsync();

        Assert.Equal(new[] { "Durand", "Martin" }, liste.Select(u => u.Nom));
    }

    [Fact]
    public async Task ObtenirParIdAsync_IdInexistant_RetourneNull()
    {
        var trouve = await _service.ObtenirParIdAsync(42);

        Assert.Null(trouve);
    }

    [Fact]
    public async Task ObtenirParIdAsync_IdExistant_RetourneUtilisateur()
    {
        var cree = await _service.CreerAsync(CreerDto());

        var trouve = await _service.ObtenirParIdAsync(cree!.Id);

        Assert.NotNull(trouve);
        Assert.Equal(cree.Id, trouve.Id);
        Assert.Equal(cree.Email, trouve.Email);
    }

    [Fact]
    public async Task Initialiser_BaseSqlite_ReappliquerPreserveLesDonnees()
    {
        using var connexion = new SqliteConnection("Data Source=:memory:");
        await connexion.OpenAsync();
        var options = new DbContextOptionsBuilder<UsersContext>()
            .UseSqlite(connexion)
            .Options;
        using var context = new UsersContext(options);

        UsersDbInitializer.Initialiser(context, NullLogger.Instance);
        Assert.Equal(3, await context.Utilisateurs.CountAsync());

        var utilisateur = await context.Utilisateurs.FirstAsync();
        utilisateur.Nom = "Modifie";
        await context.SaveChangesAsync();

        UsersDbInitializer.Initialiser(context, NullLogger.Instance);

        context.ChangeTracker.Clear();
        Assert.Equal(3, await context.Utilisateurs.CountAsync());
        Assert.Equal("Modifie", (await context.Utilisateurs.FindAsync(utilisateur.Id))!.Nom);
    }
}
