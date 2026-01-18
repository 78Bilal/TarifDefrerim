using Microsoft.EntityFrameworkCore;

namespace YemekTarifleri.Models {
    public class AppDbContext : DbContext {
        public AppDbContext() { }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base (options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=./wwwroot/YemekTarifileri.db");
        }

        public DbSet<TarifModel> Tarifler { get; set; }
    }
}