using Microsoft.EntityFrameworkCore;
using RepairIT.Shared.Enums;
using RepairIT.Shared.Modeles;

namespace RepairIT.CatalogService.Data;

public static class CatalogDbInitializer
{
    public static void Initialiser(CatalogContext context, ILogger logger)
    {
        context.Database.Migrate();
        logger.LogInformation("Application des migrations de la base de données du catalogue");
        if (context.Ordinateurs.Any())
            return;

        context.Ordinateurs.AddRange(CreerJeuDeDonnees());
        context.SaveChanges();
        logger.LogInformation("Catalogue initialisé avec le jeu de données de départ");
    }

    private static List<Ordinateur> CreerJeuDeDonnees()
    {
        return new List<Ordinateur>
        {
            new() { Nom = "XPS 13", Marque = "Dell", Processeur = "Intel Core i7", RamGo = 16, StockageGo = 512, Type = TypeOrdinateur.Portable, Prix = 1299m, Stock = 4 },
            new() { Nom = "ThinkPad T14", Marque = "Lenovo", Processeur = "AMD Ryzen 7", RamGo = 32, StockageGo = 1024, Type = TypeOrdinateur.Portable, Prix = 1490m, Stock = 6 },
            new() { Nom = "MacBook Air M3", Marque = "Apple", Processeur = "Apple M3", RamGo = 16, StockageGo = 512, Type = TypeOrdinateur.Portable, Prix = 1399m, Stock = 2 },
            new() { Nom = "OptiPlex 7010", Marque = "Dell", Processeur = "Intel Core i5", RamGo = 16, StockageGo = 512, Type = TypeOrdinateur.Fixe, Prix = 849m, Stock = 8 },
            new() { Nom = "iMac 24", Marque = "Apple", Processeur = "Apple M3", RamGo = 8, StockageGo = 256, Type = TypeOrdinateur.ToutEnUn, Prix = 1599m, Stock = 1 },
            new() { Nom = "ProLiant ML30", Marque = "HP", Processeur = "Intel Xeon E-2314", RamGo = 32, StockageGo = 4096, Type = TypeOrdinateur.Serveur, Prix = 2190m, Stock = 0 }
        };
    }
}
