using System.ComponentModel.DataAnnotations;
using RepairIT.Shared.Enums;

namespace RepairIT.Shared.Dtos;

// données saisies pour créer un utilisateur 
public class UtilisateurCreationDto
{
    [Required(ErrorMessage = "Le nom est obligatoire.")]
    [StringLength(50)]
    public string Nom { get; set; } = string.Empty;

    [Required(ErrorMessage = "Le prénom est obligatoire.")]
    [StringLength(50)]
    public string Prenom { get; set; } = string.Empty;

    [Required(ErrorMessage = "L'e-mail est obligatoire.")]
    [EmailAddress(ErrorMessage = "L'e-mail n'est pas valide.")]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;

    public RoleUtilisateur Role { get; set; } = RoleUtilisateur.Gestionnaire;
}