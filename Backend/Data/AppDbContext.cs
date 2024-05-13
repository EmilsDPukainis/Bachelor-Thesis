using Microsoft.EntityFrameworkCore;
using Shared.Models;

public class AppDbContext : DbContext
{

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        Database.EnsureCreated();

    }
    public DbSet<Jobs> Jobs { get; set; }

    public DbSet<Item> Items { get; set; }
    public DbSet<CheckIns> CheckIns { get; set; }
    public DbSet<User> Users { get; set; }

}

