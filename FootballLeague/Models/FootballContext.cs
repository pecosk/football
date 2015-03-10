using System.Data.Entity;

namespace FootballLeague.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class FootballContext : DbContext
    {
        public FootballContext() 
        {
            Database.SetInitializer<FootballContext>(new DbInitializer());
        }

        public virtual IDbSet<User> Users { get; set; }
        public virtual IDbSet<Match> Matches { get; set; }
        public virtual IDbSet<Team> Teams { get; set; }
        public virtual IDbSet<Set> Sets { get; set; }

        public virtual IDbSet<Tournament.Tournament> Tournaments { get; set; }
        public virtual IDbSet<Tournament.TournamentRound> TournamentRounds { get; set; }
        public virtual IDbSet<Tournament.TournamentMatch> TournamentMatches { get; set; }
        public virtual IDbSet<Tournament.TournamentTeam> TournamentTeams { get; set; }
        public virtual IDbSet<Tournament.TournamentSet> TournamentSets { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => u.Id).Property(u => u.Name).IsRequired();

            modelBuilder.Entity<Match>().HasKey(m => m.Id).Property(m => m.PlannedTime).IsRequired();
            modelBuilder.Entity<Match>().HasOptional(m => m.Creator);
            modelBuilder.Entity<Match>().HasRequired(m => m.Team1).WithOptional().Map(m => m.MapKey("Team1_Id"));
            modelBuilder.Entity<Match>().HasRequired(m => m.Team2).WithOptional().Map(m => m.MapKey("Team2_Id"));
            modelBuilder.Entity<Match>().HasMany(m => m.Invites).WithMany();

            modelBuilder.Entity<Team>().HasKey(t => t.Id);
            modelBuilder.Entity<Team>().HasOptional(t => t.Member1);
            modelBuilder.Entity<Team>().HasOptional(t => t.Member2);

            modelBuilder.Entity<Set>().HasKey(s => s.Id);
            modelBuilder.Entity<Set>().HasRequired(s => s.Match).WithMany(m => m.Sets);            

            modelBuilder.Entity<Tournament.Tournament>().HasKey(t => t.Id).Property(t => t.Name).IsRequired();
            modelBuilder.Entity<Tournament.Tournament>().HasOptional(t => t.Teams);

            modelBuilder.Entity<Tournament.TournamentRound>().HasKey(m => m.Id);
            modelBuilder.Entity<Tournament.TournamentRound>().HasRequired(m => m.Tournament).WithMany(t => t.Rounds);            

            modelBuilder.Entity<Tournament.TournamentMatch>().HasKey(m => m.Id);
            modelBuilder.Entity<Tournament.TournamentMatch>().HasRequired(m => m.Round).WithMany(t => t.Matches);
            modelBuilder.Entity<Tournament.TournamentMatch>().HasOptional(m => m.Team1);
            modelBuilder.Entity<Tournament.TournamentMatch>().HasOptional(m => m.Team2);

            modelBuilder.Entity<Tournament.TournamentTeam>().HasKey(t => t.Id);
            modelBuilder.Entity<Tournament.TournamentTeam>().HasRequired(m => m.Tournament).WithMany(t => t.Teams);
            modelBuilder.Entity<Tournament.TournamentTeam>().HasOptional(t => t.Member1);
            modelBuilder.Entity<Tournament.TournamentTeam>().HasOptional(t => t.Member2);

            modelBuilder.Entity<Tournament.TournamentSet>().HasKey(s => s.Id);
            modelBuilder.Entity<Tournament.TournamentSet>().HasRequired(s => s.TournamentMatch).WithMany(m => m.Sets);
        }
    }
}