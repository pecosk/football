namespace FootballLeague.Tests.Models.Repositories
{
    using System.Collections.Generic;
    using System.Linq;

    using FootballLeague.Models;
    using FootballLeague.Models.Repositories;
    using FootballLeague.Tests.Data;

    using NUnit.Framework;

    using Rhino.Mocks;

    public class TeamRepositoryTest : RepositoryTestBase
    {
        FootballContext _context;

        [SetUp]
        public void SetUp()
        {
            _context = MockRepository.GenerateMock<FootballContext>();
        }

        [Test]
        public void AddTeamMember_WhenNotYetInMatch_ThenAddsTeamMember()
        {
            var match = MatchBuilder.Create().WithTeamMember(Users.Ferko).Build();            
            var repo = new TeamRepository(_context);
            _context.Users = MockContextData(_context, c => c.Users, new List<User>().AsQueryable());
            _context.Expect(c => c.SaveChanges()).Return(0);
            _context.Users.Expect(e => e.Attach(Arg<User>.Is.Same(Users.Dano))).Return(null);

            repo.AddMatchParticipantToTeam(Users.Dano, match.Team1);

            _context.VerifyAllExpectations();
            _context.Users.VerifyAllExpectations();
            Assert.IsTrue(match.Contains(Users.Dano));
            Assert.IsTrue(match.Contains(Users.Ferko));
        }

        [Test]
        public void AddMatchParticipant_NullPlayers_CreatesOneParticipant()
        {            
            var match = MatchBuilder.Create().Build();          
            var repo = new TeamRepository(_context);
            _context.Users = MockContextData(_context, c => c.Users, new List<User>().AsQueryable());
            _context.Expect(c => c.SaveChanges()).Return(0);
            _context.Users.Expect(e => e.Attach(Arg<User>.Is.Same(Users.Ferko))).Return(null);

            repo.AddMatchParticipantToTeam(Users.Ferko, match.Team1);

            _context.VerifyAllExpectations();
            _context.Users.VerifyAllExpectations();
            Assert.IsTrue(match.Contains(Users.Ferko));
        }

        [Test]
        public void AddMatchParticipant_PlayerExists_DoesNothing()
        {            
            var match = MatchData.MatchWithTwoFullTeams;
            var repo = new TeamRepository(_context);
            _context.Expect(c => c.SaveChanges()).Repeat.Never();

            repo.AddMatchParticipantToTeam(Users.Ferko, match.Team1);

            _context.VerifyAllExpectations();
            Assert.IsTrue(match.Contains(Users.Ferko));
        }

        [Test]
        //TODO wouldn't it make more sense to switch user to other team?
        public void AddMatchParticipant_WhenInOtherTeam_DoesNothing()
        {
            var match = MatchBuilder.Create().WithTeamMember(Users.Ferko).WithTeamMember(Users.Dano).Build();
            match.Team1.SetMember(Users.Dano);
            var repo = new TeamRepository(_context);
            _context.Expect(c => c.SaveChanges()).Repeat.Never();

            repo.AddMatchParticipantToTeam(Users.Dano, match.Team2);

            _context.VerifyAllExpectations();
            Assert.IsFalse(match.Team2.Contains(Users.Dano));
        }

        [Test]
        public void RemoveMatchParticipant_IfNotYetInMatch_DoesNothing()
        {            
            var match = MatchBuilder.Create().WithTeamMember(Users.Ferko).Build();        
            var repo = new TeamRepository(_context);
            _context.Expect(c => c.SaveChanges()).Repeat.Never();

            repo.RemoveMatchParticipantFromTeam(Users.Dano, match.Team1);

            _context.VerifyAllExpectations();
            Assert.IsFalse(match.Contains(Users.Dano));
        }

        [Test]
        public void RemoveMatchParticipant_NullPlayers_DoesNothing()
        {
            var user = new User();
            var match = MatchBuilder.Create().Build();
            var repo = new TeamRepository(_context);
            _context.Expect(c => c.SaveChanges()).Repeat.Never();

            repo.RemoveMatchParticipantFromTeam(user, match.Team1);

            _context.VerifyAllExpectations();
            Assert.IsNull(match.Team1.Member1);
        }

        [Test]
        public void RemoveMatchParticipant_PlayerExists_RemovesMatchParticipant()
        {         
            var match = MatchBuilder.Create().WithTeamMember(Users.Ferko).Build();            
            var repo = new TeamRepository(_context);
            _context.Expect(c => c.SaveChanges()).Return(0);

            repo.RemoveMatchParticipantFromTeam(Users.Ferko, match.Team1);

            _context.VerifyAllExpectations();
            Assert.IsFalse(match.Contains(Users.Ferko));
        }
    }
}