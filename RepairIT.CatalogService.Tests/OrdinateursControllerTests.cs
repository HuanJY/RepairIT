using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using RepairIT.CatalogService.Controllers;
using RepairIT.CatalogService.Services;
using RepairIT.Shared.Modeles;

namespace RepairIT.CatalogService.Tests;

// Tests unitaires du contrôleur : le service est remplacé par un mock (Moq)
public class OrdinateursControllerTests
{
    private readonly Mock<IOrdinateurService> _serviceMock = new();
    private readonly OrdinateursController _controller;

    public OrdinateursControllerTests()
    {
        _controller = new OrdinateursController(_serviceMock.Object, NullLogger<OrdinateursController>.Instance);
    }

    [Fact]
    public async Task Lister_RetourneOkAvecLaListe()
    {
        _serviceMock.Setup(s => s.ListerAsync()).ReturnsAsync(new List<Ordinateur> { new() { Id = 1, Nom = "XPS 13" } });

        var resultat = await _controller.Lister();

        var ok = Assert.IsType<OkObjectResult>(resultat);
        Assert.Single(Assert.IsType<List<Ordinateur>>(ok.Value));
    }

    [Fact]
    public async Task ObtenirParId_OrdinateurExistant_RetourneOk()
    {
        _serviceMock.Setup(s => s.ObtenirParIdAsync(1)).ReturnsAsync(new Ordinateur { Id = 1, Nom = "XPS 13" });

        var resultat = await _controller.ObtenirParId(1);

        Assert.IsType<OkObjectResult>(resultat);
    }

    [Fact]
    public async Task ObtenirParId_OrdinateurInexistant_RetourneNotFound()
    {
        _serviceMock.Setup(s => s.ObtenirParIdAsync(42)).ReturnsAsync((Ordinateur?)null);

        var resultat = await _controller.ObtenirParId(42);

        Assert.IsType<NotFoundResult>(resultat);
    }

    [Fact]
    public async Task Ajouter_OrdinateurValide_RetourneCreated()
    {
        var ordinateur = new Ordinateur { Nom = "XPS 13" };
        _serviceMock.Setup(s => s.AjouterAsync(ordinateur)).ReturnsAsync(new Ordinateur { Id = 7, Nom = "XPS 13" });

        var resultat = await _controller.Ajouter(ordinateur);

        var cree = Assert.IsType<CreatedAtActionResult>(resultat);
        Assert.Equal(7, cree.RouteValues!["id"]);
    }

    [Fact]
    public async Task MettreAJour_OrdinateurExistant_RetourneNoContent()
    {
        _serviceMock.Setup(s => s.MettreAJourAsync(1, It.IsAny<Ordinateur>())).ReturnsAsync(true);

        var resultat = await _controller.MettreAJour(1, new Ordinateur());

        Assert.IsType<NoContentResult>(resultat);
    }

    [Fact]
    public async Task MettreAJour_OrdinateurInexistant_RetourneNotFound()
    {
        _serviceMock.Setup(s => s.MettreAJourAsync(42, It.IsAny<Ordinateur>())).ReturnsAsync(false);

        var resultat = await _controller.MettreAJour(42, new Ordinateur());

        Assert.IsType<NotFoundResult>(resultat);
    }

    [Fact]
    public async Task Supprimer_OrdinateurExistant_RetourneNoContent()
    {
        _serviceMock.Setup(s => s.SupprimerAsync(1)).ReturnsAsync(true);

        var resultat = await _controller.Supprimer(1);

        Assert.IsType<NoContentResult>(resultat);
    }

    [Fact]
    public async Task Supprimer_OrdinateurInexistant_RetourneNotFound()
    {
        _serviceMock.Setup(s => s.SupprimerAsync(42)).ReturnsAsync(false);

        var resultat = await _controller.Supprimer(42);

        Assert.IsType<NotFoundResult>(resultat);
    }
}
