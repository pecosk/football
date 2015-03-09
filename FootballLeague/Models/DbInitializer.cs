using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using FootballLeague.Models.Tournament;

namespace FootballLeague.Models
{
    public class DbInitializer : CreateDatabaseIfNotExists<FootballContext>
    {
        protected override void Seed(FootballContext context)
        {
            base.Seed(context);
            if (System.Diagnostics.Debugger.IsAttached == false)
                System.Diagnostics.Debugger.Launch();

            context.Users.Add(CreateUser("Marek", "Sedlacek", "marek.sedlacek@erni.sk", "sea"));
            context.Users.Add(CreateUser("Peter", "Cilics", "peter.cilics@erni.sk", "pci"));
            context.Users.Add(CreateUser("Edwin", "Wilhel", "edwin.wilhel@erni.sk", "ehl"));
            context.Users.Add(CreateUser("Alfons", "Gisler", "alfons.gisler@erni.sk", "alg"));
            context.Users.Add(CreateUser("Peter", "Tkac", "peter.tkac@erni.sk", "pec"));
            context.Users.Add(CreateUser("David", "Golias", "david.golias@erni.sk", "dgs"));
            context.Users.Add(CreateUser("Heihachi", "Mishima", "heihachi.mishima@erni.sk", "hmi"));
            context.Users.Add(CreateUser("Jin", "Kazama", "jin.kazama@erni.sk", "jkz"));
            context.Users.Add(CreateUser("Todd", "Terje", "todd.terje@erni.sk", "ttd"));
            context.SaveChanges();

            context.Tournaments.Add(new Tournament.Tournament("clash of titans", EliminationType.Single, TournamentState.Registration, 4));
            context.Tournaments.Add(new Tournament.Tournament("clash of gnomes", EliminationType.Double, TournamentState.Registration, 4));
            context.SaveChanges();

            var tournament = context.Tournaments.First();
            context.TournamentTeams.Add(new TournamentTeam(tournament, "Teple fasirky", context.Users.ToList()[1], context.Users.ToList()[2]));
            context.TournamentTeams.Add(new TournamentTeam(tournament, "Studeny caj", context.Users.ToList()[3], context.Users.ToList()[4]));
            context.TournamentTeams.Add(new TournamentTeam(tournament, "Ruzove jednorozce", context.Users.ToList()[5], context.Users.ToList()[6]));
            //context.TournamentTeams.Add(new TournamentTeam("Care bears", context.Users[7], context.Users[8]));        

            context.SaveChanges();
        }

        private User CreateUser(string firstName, string lastName, string mail, string name)
        {
            return new User
            {
                FirstName = firstName,
                LastName = lastName,
                Mail = mail,
                Name = name
            };
        }        
    }
}