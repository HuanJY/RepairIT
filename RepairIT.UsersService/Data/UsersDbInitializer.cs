using Microsoft.EntityFrameworkCore;
using RepairIT.Shared.Enums;
using RepairIT.Shared.Modeles;

namespace RepairIT.UsersService.Data;

// Initialisation de la base des utilisateurs au démarrage de l'API
public static class UsersDbInitializer
{
    // Applique les migrations puis ajoute user1 (administrateur), user2 et user3 (gestionnaires) si la table est vide
    public static void Initialiser(UsersContext context, ILogger logger)
    {
        context.Database.Migrate();

        if (context.Utilisateurs.Any())
            return;

        // Mêmes comptes que ceux demandés dans Keycloak (user1, user2, user3)
        context.Utilisateurs.AddRange(
            new Utilisateur { Nom = "Un", Prenom = "User", Email = "user1@repairit.fr", Role = RoleUtilisateur.Administrateur, DateCreation = DateTime.UtcNow },
            new Utilisateur { Nom = "Deux", Prenom = "User", Email = "user2@repairit.fr", Role = RoleUtilisateur.Gestionnaire, DateCreation = DateTime.UtcNow },
            new Utilisateur { Nom = "Trois", Prenom = "User", Email = "user3@repairit.fr", Role = RoleUtilisateur.Gestionnaire, DateCreation = DateTime.UtcNow }
        );
        context.SaveChanges();
        logger.LogInformation("Utilisateurs initialisés avec le jeu de données de départ");
    }
}
