using System.Text.Json.Serialization;

namespace RepairIT.Shared.Enums;

// catégorie d'un ordinateur du catalogue
[JsonConverter(typeof(JsonStringEnumConverter<TypeOrdinateur>))]
public enum TypeOrdinateur
{
    Portable,
    Fixe,
    ToutEnUn,
    Serveur
}