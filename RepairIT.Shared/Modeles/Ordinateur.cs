using System.ComponentModel.DataAnnotations;
using RepairIT.Shared.Enums;

namespace RepairIT.Shared.Modeles;

// Ordinateur du catalogue
public class Ordinateur
{
    // Id généré par la BDD
    public int Id { get; set; }

    [Required(ErrorMessage = "Le nom est obligatoire.")]
    [StringLength(100)]
    public string Nom { get; set; } = string.Empty;

    [Required(ErrorMessage = "La marque est obligatoire.")]
    [StringLength(50)]
    public string Marque { get; set; } = string.Empty;

    [Required(ErrorMessage = "Le processeur est obligatoire.")]
    [StringLength(100)]
    public string Processeur { get; set; } = string.Empty;

    // Mémre Go
    [Range(1, 1024, ErrorMessage = "La RAM doit être comprise entre 1 et 1024 Go.")]
    public int RamGo { get; set; } = 8;

    // cpacité de stockage Go
    [Range(1, 65536, ErrorMessage = "Le stockage doit être compris entre 1 et 65536 Go.")]
    public int StockageGo { get; set; } = 256;

    public TypeOrdinateur Type { get; set; } = TypeOrdinateur.Portable;

    // prix de vente en euros
    [Range(0, 100000, ErrorMessage = "Le prix doit être compris entre 0 et 100 000 €.")]
    public decimal Prix { get; set; }

    // nb d'exemplaires disponibles
    [Range(0, 10000, ErrorMessage = "Le stock doit être compris entre 0 et 10 000.")]
    public int Stock { get; set; }
}