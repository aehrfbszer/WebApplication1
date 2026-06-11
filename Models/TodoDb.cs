namespace WebApplication1.Models;

using Microsoft.EntityFrameworkCore;

class TodoDb(DbContextOptions<TodoDb> options) : DbContext(options)
{
    public DbSet<Todo> Todos => Set<Todo>();
}