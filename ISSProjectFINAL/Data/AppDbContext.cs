using ISSProjectFINAL.Models;
using Microsoft.EntityFrameworkCore;

namespace ISSProjectFINAL.Data
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Form> Forms { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlite("Data Source=FormsApp.db");
        }
    }
}
