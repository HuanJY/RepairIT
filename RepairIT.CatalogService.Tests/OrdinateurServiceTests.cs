using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using RepairIT.CatalogService.Data;
using RepairIT.CatalogService.Services;
using RepairIT.Shared.Enums;
using RepairIT.Shared.Modeles;

namespace RepairIT.CatalogService.Tests;

// Tests unitaires du service du catalogue
public class OrdinateurServiceTests
{
    private readonly CatalogContext _context;
    private readonly OrdinateurService _service;

    public OrdinateurServiceTests()
    {
        var options = new DbContextOptionsBuilder<CatalogContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new CatalogContext(options);
        _service = new OrdinateurService(_context, NullLogger<OrdinateurService>.Instance);
    }

    // Crée un ordinateur de test
    private static Ordinateur CreerOrdinateur(string nom = "XPS 13", string marque = "Dell")
    {
        return new Ordinateur
        {
            Nom = nom,
            Marque = marque,
            Processeur = "Intel Core i7",
            RamGo = 16,
            StockageGo = 512,
            Type = TypeOrdinateur.Portable,
            Prix = 1299m,
            Stock = 3
        };
    }

    [Fact]
    public async Task AjouterAsync_OrdinateurValide_EstEnregistreEnBase()
    {
        var cree = await _service.AjouterAsync(CreerOrdinateur());

        Assert.True(cree.Id > 0);
        Assert.Equal(1, await _context.Ordinateurs.CountAsync());
    }

    [Fact]
    public async Task AjouterAsync_IdFourniParLAppelant_EstIgnore()
    {
        var ordinateur = CreerOrdinateur();
        ordinateur.Id = 999;

        var cree = await _service.AjouterAsync(ordinateur);

        Assert.NotEqual(999, cree.Id);
    }

    [Fact]
    public async Task ListerAsync_PlusieursOrdinateurs_TriesParMarquePuisNom()
    {
        await _service.AjouterAsync(CreerOrdinateur("XPS 15", "Dell"));
        await _service.AjouterAsync(CreerOrdinateur("MacBook Air", "Apple"));
        await _service.AjouterAsync(CreerOrdinateur("Latitude", "Dell"));

        var liste = await _service.ListerAsync();

        Assert.Equal(new[] { "MacBook Air", "Latitude", "XPS 15" }, liste.Select(o => o.Nom));
    }

    [Fact]
    public async Task ObtenirParIdAsync_IdExistant_RetourneLOrdinateur()
    {
        var cree = await _service.AjouterAsync(CreerOrdinateur());

        var trouve = await _service.ObtenirParIdAsync(cree.Id);

        Assert.NotNull(trouve);
        Assert.Equal("XPS 13", trouve.Nom);
    }

    [Fact]
    public async Task ObtenirParIdAsync_IdInexistant_RetourneNull()
    {
        var trouve = await _service.ObtenirParIdAsync(42);

        Assert.Null(trouve);
    }

    [Fact]
    public async Task MettreAJourAsync_OrdinateurExistant_ModifieLesInformations()
    {
        var cree = await _service.AjouterAsync(CreerOrdinateur());
        var modifie = CreerOrdinateur("XPS 13 Plus");
        modifie.Prix = 1499m;
        modifie.Stock = 10;

        var resultat = await _service.MettreAJourAsync(cree.Id, modifie);

        var enBase = await _service.ObtenirParIdAsync(cree.Id);
        Assert.True(resultat);
        Assert.Equal("XPS 13 Plus", enBase!.Nom);
        Assert.Equal(1499m, enBase.Prix);
        Assert.Equal(10, enBase.Stock);
    }

    [Fact]
    public async Task MettreAJourAsync_OrdinateurInexistant_RetourneFalse()
    {
        var resultat = await _service.MettreAJourAsync(42, CreerOrdinateur());

        Assert.False(resultat);
    }

    [Fact]
    public async Task SupprimerAsync_OrdinateurExistant_LeRetireDuCatalogue()
    {
        var cree = await _service.AjouterAsync(CreerOrdinateur());

        var resultat = await _service.SupprimerAsync(cree.Id);

        Assert.True(resultat);
        Assert.Empty(await _service.ListerAsync());
    }

    [Fact]
    public async Task SupprimerAsync_OrdinateurInexistant_RetourneFalse()
    {
        await _service.AjouterAsync(CreerOrdinateur());

        var resultat = await _service.SupprimerAsync(42);

        Assert.False(resultat);
        Assert.Single(await _service.ListerAsync());
    }

    [Fact]
    public async Task Initialiser_BaseSqlite_ReappliquerPreserveLesDonnees()
    {
        using var connexion = new SqliteConnection("Data Source=:memory:");
        await connexion.OpenAsync();
        var options = new DbContextOptionsBuilder<CatalogContext>()
            .UseSqlite(connexion)
            .Options;
        using var context = new CatalogContext(options);

        CatalogDbInitializer.Initialiser(context, NullLogger.Instance);
        Assert.Equal(6, await context.Ordinateurs.CountAsync());

        var ordinateur = await context.Ordinateurs.FirstAsync();
        ordinateur.Stock = 42;
        await context.SaveChangesAsync();

        CatalogDbInitializer.Initialiser(context, NullLogger.Instance);

        context.ChangeTracker.Clear();
        Assert.Equal(6, await context.Ordinateurs.CountAsync());
        Assert.Equal(42, (await context.Ordinateurs.FindAsync(ordinateur.Id))!.Stock);
    }
}
