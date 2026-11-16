using System.ComponentModel.DataAnnotations;
using RepairIT.Shared.Enums;

namespace RepairIT.Shared.Modeles;

// user de l'app 
public class Utilisateur
{
    // valeur provisoire 
    public const string UserKeycloakParDefaut = "non-lie-keycloak";

    // Id gen par la BDD
    public int Id { get; set; }

    // id du compte Keycloak associé
    [Required]
    [StringLength(100)]
    public string UserKeycloak { get; set; } = UserKeycloakParDefaut;

    [Required]
    [StringLength(50)]
    public string Nom { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Prenom { get; set; } = string.Empty;

    // e-mail unique en bdd
    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;

    public RoleUtilisateur Role { get; set; } = RoleUtilisateur.Gestionnaire;

    // compte désactivé ne pourra plus se connecter
    public bool EstActif { get; set; } = true;

    public DateTime DateCreation { get; set; }
}