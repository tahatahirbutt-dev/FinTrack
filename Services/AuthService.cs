using FinTrack.Data;
using FinTrack.Models;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Services;

public class AuthService
{
    private readonly AppDbContext _db;

    public AuthService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<(bool Success, string Message, User? User)> RegisterAsync(string name, string email, string password)
    {
        if (await _db.Users.AnyAsync(u => u.Email == email))
            return (false, "Email already registered.", null);

        var user = new User
        {
            Name = name,
            Email = email.Trim().ToLower(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            CreatedAt = DateTime.Now
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return (true, "Registration successful.", user);
    }

    public async Task<(bool Success, string Message, User? User)> LoginAsync(string email, string password)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email.Trim().ToLower());
        if (user == null)
            return (false, "No account found with this email.", null);

        if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            return (false, "Incorrect password.", null);

        return (true, "Login successful.", user);
    }
}
