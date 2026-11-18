using Microsoft.EntityFrameworkCore;
using RepairIT.Shared.Dtos;
using RepairIT.Shared.Modeles;
using RepairIT.UsersService.Data;
using RepairIT.UsersService.Services;

namespace RepairIT.UsersService.Services;

public class UtilisateurService : IUtilisateurService
{
    private readonly UsersContext _context;
    private readonly ILogger<UtilisateurService> _logger;

    public UtilisateurService(UsersContext context, ILogger<UtilisateurService> logger)
    {
        _context = context;
        _logger = logger;
    }

    // return ts les users, triés par nom puis  prénom
    public async Task<List<Utilisateur>> ListerAsync()
    {
        _logger.LogInformation("Liste des utilisateurs demandée");
        return await _context.Utilisateurs
            .OrderBy(u => u.Nom)
            .ThenBy(u => u.Prenom)
            .ToListAsync();
    }

    // erturn l'user correspondant à l'Id, ou null s'il n'existe pas
    public async Task<Utilisateur?> ObtenirParIdAsync(int id)
    {
        _logger.LogInformation("Utilisateur demandé avec ID {Id}", id);
        var utilisateur = await _context.Utilisateurs.FirstOrDefaultAsync(u => u.Id == id);
        if (utilisateur == null)
            _logger.LogWarning("Utilisateur avec ID {Id} non trouvé", id);
        else
            _logger.LogInformation("Utilisateur avec ID {Id} trouvé", id);
        return utilisateur;
    }

    // Crée un utilisateur ou null si l'e-mail est déjà utilisé
    public async Task<Utilisateur?> CreerAsync(UtilisateurCreationDto dto)
    {
        var email = dto.Email.Trim().ToLower();
        _logger.LogInformation("Création d'un nouvel utilisateur avec l'adresse e-mail {Email}", email);

        if (await _context.Utilisateurs.AnyAsync(u => u.Email == email))
        {
            _logger.LogWarning("Conflit lors de la création de l'utilisateur : l'adresse e-mail est déjà utilisée");
            return null;
        }
        _logger.LogInformation("Création de l'utilisateur avec l'adresse e-mail {Email} validée", email);
        var utilisateur = new Utilisateur
        {
            Nom = dto.Nom.Trim(),
            Prenom = dto.Prenom.Trim(),
            Email = email,
            Role = dto.Role,
            DateCreation = DateTime.UtcNow
        };

        _logger.LogInformation("Ajout de l'utilisateur à la base de données");
        _context.Utilisateurs.Add(utilisateur);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Utilisateur créé avec ID {Id}", utilisateur.Id);
        return utilisateur;
    }
}