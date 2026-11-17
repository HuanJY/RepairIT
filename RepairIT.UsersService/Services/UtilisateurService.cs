using Microsoft.EntityFrameworkCore;
using RepairIT.Shared.Dtos;
using RepairIT.Shared.Modeles;
using RepairIT.UsersService.Data;

namespace RepairIT.UsersService.Services;

public class UtilisateurService
{
    private readonly UsersContext _context;

    public UtilisateurService(UsersContext context)
    {
        _context = context;
    }

    // return ts les users, triés par nom puis  prénom
    public async Task<List<Utilisateur>> ListerAsync()
    {
        return await _context.Utilisateurs
            .OrderBy(u => u.Nom)
            .ThenBy(u => u.Prenom)
            .ToListAsync();
    }

    // erturn l'user correspondant à l'Id, ou null s'il n'existe pas
    public async Task<Utilisateur?> ObtenirParIdAsync(int id)
    {
        return await _context.Utilisateurs.FirstOrDefaultAsync(u => u.Id == id);
    }

    // Crée un utilisateur ou null si l'e-mail est déjà utilisé
    public async Task<Utilisateur?> CreerAsync(UtilisateurCreationDto dto)
    {
        var email = dto.Email.Trim().ToLower();

        if (await _context.Utilisateurs.AnyAsync(u => u.Email == email))
            return null;

        var utilisateur = new Utilisateur
        {
            Nom = dto.Nom.Trim(),
            Prenom = dto.Prenom.Trim(),
            Email = email,
            Role = dto.Role,
            DateCreation = DateTime.UtcNow
        };

        _context.Utilisateurs.Add(utilisateur);
        await _context.SaveChangesAsync();

        return utilisateur;
    }
}