namespace FootballLeague.Tests.Models
{
    using FootballLeague.Tests.Data;

    using NUnit.Framework;

    public class TeamModelTest
    {
        [Test]
        public void IsFull_WhenPlayersAreSet_ThenReturnsTrue()
        {
            var team = TeamData.TeamWithFerkoJurko;

            Assert.IsTrue(team.IsFull);
        }

        [Test]
        public void IsEmpty_WhenNoPlayersAreSet_ThenReturnsTrue()
        {
            var team = TeamData.EmptyTeam;

            Assert.IsTrue(team.IsEmpty);
        }

        [Test]
        public void Contains_WhenHasPlayer_ThenReturnTrue()
        {
            var team = TeamData.TeamWithFerkoJurko;

            Assert.IsTrue(team.Contains(Users.Ferko));
            Assert.IsTrue(team.Contains(Users.Jurko));
        }

        [Test]
        public void SetPlayer_WhenUserIsSet_ThenContainsReturnsFalse()
        {
            var team = TeamData.EmptyTeam;

            team.SetMember(Users.Jurko);

            Assert.IsTrue(team.Contains(Users.Jurko));
        }

        [Test]
        public void RemovePlayer_WhenUserIsRemoved_ThenContainsReturnsFalse()
        {
            var team = TeamData.TeamWithFerkoJurko;

            team.RemoveMember(Users.Jurko);

            Assert.IsTrue(team.Contains(Users.Ferko));
            Assert.IsFalse(team.Contains(Users.Jurko));
        }

        [Test]
        public void SetPlayer_WhenTeamIsFull_ThenSetPlayerDoesNothing()
        {
            var team = TeamData.TeamWithFerkoJurko;

            team.SetMember(Users.Peto);

            Assert.IsFalse(team.Contains(Users.Peto));
        }

        [Test]
        public void RemovePlayer_WhenTeamIsEmpty_ThenDoesNothing()
        {
            var team = TeamData.EmptyTeam;

            team.RemoveMember(Users.Ferko);

            Assert.IsTrue(team.IsEmpty);
        } 
    }
}