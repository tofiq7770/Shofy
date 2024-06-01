using BackendMiniTask.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendMiniTask.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt) { }

        public DbSet<Slider> Slider { get; set; }
    }
}
