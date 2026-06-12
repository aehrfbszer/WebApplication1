namespace WebApplication1.Data;

using Microsoft.EntityFrameworkCore;
using WebApplication1.Domain;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Todo> Todos => Set<Todo>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Product> Products => Set<Product>();
}