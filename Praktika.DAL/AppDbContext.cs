using Microsoft.EntityFrameworkCore;

namespace Praktika.DAL 
{
    public class AppDbContext : DbContext
    {
        public DbSet<Teams> Teams { get; set; }

        private string ConnectionString =>  "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Team;Integrated Security=True;";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConnectionString);
            }
        }
    }
}