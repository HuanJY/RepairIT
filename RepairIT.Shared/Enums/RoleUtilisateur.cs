using System.Text.Json.Serialization;

namespace RepairIT.Shared.Enums;

// rôle d'un utilisateur de l'application
[JsonConverter(typeof(JsonStringEnumConverter<RoleUtilisateur>))]
public enum RoleUtilisateur
{
    Gestionnaire,
    Administrateur
}