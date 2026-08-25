using Microsoft.EntityFrameworkCore;

namespace KitabKlubu.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Book> Books { get; set; }
    public DbSet<Article> Articles { get; set; }
    public DbSet<AdminUser> AdminUsers { get; set; }
}