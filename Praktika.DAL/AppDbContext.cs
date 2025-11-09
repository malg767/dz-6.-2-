using Microsoft.EntityFrameworkCore;
using Praktika.DAL.Entities;

namespace Praktika.DAL 
{
    public class AppDbContext : DbContext
    {
        public DbSet<Team> Teams { get; set; }      
        public DbSet<Match> Matches { get; set; }
        public DbSet<Player> Players { get; set; }

        private string ConnectionString =>  "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Teams;Integrated Security=True;";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>()
                  .HasMany(p => p.Matches)
                  .WithMany(m => m.Players)
                  .UsingEntity<Dictionary<string, object>>(
                      "PlayerMatch",
                      j => j.HasOne<Match>().WithMany().HasForeignKey("MatchId").OnDelete(DeleteBehavior.Cascade),
                      j => j.HasOne<Player>().WithMany().HasForeignKey("PlayerId").OnDelete(DeleteBehavior.Restrict)
                  );


            modelBuilder.Entity<Match>()
                .HasOne(m => m.Opponent)
                .WithMany() 
                .HasForeignKey("OpponentId") 
                .OnDelete(DeleteBehavior.Restrict);
           
            modelBuilder.Entity<Match>()
                .HasIndex("OpponentId");
        }
    }
}