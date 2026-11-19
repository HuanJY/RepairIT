using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using RepairIT.Shared.Dtos;
using RepairIT.Shared.Modeles;
using RepairIT.UsersService.Controllers;
using RepairIT.UsersService.Services;

namespace RepairIT.UsersService.Tests;

// Tests unitaires du contrôleur : le service est remplacé par un mock (Moq)
public class UtilisateursControllerTests
{
    private readonly Mock<IUtilisateurService> _serviceMock = new();
    private readonly UtilisateursController _controller;

    public UtilisateursControllerTests()
    {
        _controller = new UtilisateursController(_serviceMock.Object, NullLogger<UtilisateursController>.Instance);
    }

    [Fact]
    public async Task Creer_EmailDisponible_RetourneCreated()
    {
        _serviceMock.Setup(s => s.CreerAsync(It.IsAny<UtilisateurCreationDto>()))
            .ReturnsAsync(new Utilisateur { Id = 3, Nom = "Martin" });

        var resultat = await _controller.Creer(new UtilisateurCreationDto());

        var cree = Assert.IsType<CreatedAtActionResult>(resultat);
        Assert.Equal(3, cree.RouteValues!["id"]);
    }

    [Fact]
    public async Task Creer_EmailDejaUtilise_RetourneConflict()
    {
        _serviceMock.Setup(s => s.CreerAsync(It.IsAny<UtilisateurCreationDto>()))
            .ReturnsAsync((Utilisateur?)null);

        var resultat = await _controller.Creer(new UtilisateurCreationDto());

        Assert.IsType<ConflictObjectResult>(resultat);
    }

    [Fact]
    public async Task ObtenirParId_UtilisateurInexistant_RetourneNotFound()
    {
        _serviceMock.Setup(s => s.ObtenirParIdAsync(42)).ReturnsAsync((Utilisateur?)null);

        var resultat = await _controller.ObtenirParId(42);

        Assert.IsType<NotFoundResult>(resultat);
    }

    [Fact]
    public async Task ObtenirParId_UtilisateurExistant_RetourneUtilisateur()
    {
        var utilisateur = new Utilisateur { Id = 3, Nom = "Martin" };
        _serviceMock.Setup(s => s.ObtenirParIdAsync(3)).ReturnsAsync(utilisateur);

        var resultat = await _controller.ObtenirParId(3);

        var ok = Assert.IsType<OkObjectResult>(resultat);
        Assert.Same(utilisateur, ok.Value);
    }

    [Fact]
    public async Task Lister_RetourneOk()
    {
        _serviceMock.Setup(s => s.ListerAsync()).ReturnsAsync(new List<Utilisateur>());

        var resultat = await _controller.Lister();

        Assert.IsType<OkObjectResult>(resultat);
    }
}
