namespace FootballLeague.Models.Repositories
{
    using System.Linq;

    public class TeamRepository : ITeamRepository
    {
        private readonly FootballContext _context;

        public TeamRepository(FootballContext context = null)
        {
            _context = context ?? new FootballContext();
        }

        public Team GetTeam(int id)
        {
            return _context.Teams.FirstOrDefault(t => t.Id == id);
        }

        public void AddMatchParticipantToTeam(User user, Team team)
        {
            if (team.IsFull || team.Parent.Contains(user))
                return;

            team.SetMember(user);
            _context.Users.Attach(user);
            _context.SaveChanges();
        }

        public void RemoveMatchParticipantFromTeam(User user, Team team)
        {
            if (team.IsEmpty || !team.Contains(user))
                return;

            team.RemoveMember(user);
            _context.SaveChanges();
        }

    }
}