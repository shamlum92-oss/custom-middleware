using Microsoft.EntityFrameworkCore;
using CUSTOMMIDDLEWARE.Models;

namespace CUSTOMMIDDLEWARE.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Blog> Blogs => Set<Blog>();
}