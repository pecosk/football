using System.Data.Entity;

namespace FootballLeague.Models
{
    public class FootballContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => u.Id)
                .Property(u => u.Name).IsRequired();
        }
    }
}