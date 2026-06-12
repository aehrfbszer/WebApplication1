using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Domain;

[Index(nameof(Email), IsUnique = true)]
public class User(string name, string email)
{
    public Guid Id { get; set; }
    public string Name { get; set; } = name;
    public string Email { get; set; } = email;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}