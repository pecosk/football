using System.Data.Entity;

namespace FootballLeague.Models
{
    public class FootballContext : DbContext
    {
        public virtual IDbSet<User> Users { get; set; }
        public virtual IDbSet<Match> Matches { get; set; }
        public virtual IDbSet<Team> Teams { get; set; } 

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => u.Id).Property(u => u.Name).IsRequired();

            modelBuilder.Entity<Match>().HasKey(m => m.Id).Property(m => m.PlannedTime).IsRequired();
            modelBuilder.Entity<Match>().HasOptional(m => m.Creator);            
            modelBuilder.Entity<Match>().HasRequired(m => m.Team1);
            modelBuilder.Entity<Match>().HasRequired(m => m.Team2);

            modelBuilder.Entity<Team>().HasKey(t => t.Id);
            modelBuilder.Entity<Team>().HasOptional(t => t.Member1);
            modelBuilder.Entity<Team>().HasOptional(t => t.Member2);            
        }
    }
}