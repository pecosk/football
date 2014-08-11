using System.Data.Entity;

namespace FootballLeague.Models
{
    public class FootballContext : DbContext
    {
        public virtual IDbSet<User> Users { get; set; }
        public virtual IDbSet<Match> Matches { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => u.Id).Property(u => u.Name).IsRequired();

            modelBuilder.Entity<Match>().HasKey(m => m.Id).Property(m => m.PlannedTime).IsRequired();
            modelBuilder.Entity<Match>().HasOptional(m => m.Creator);
        }
    }
}