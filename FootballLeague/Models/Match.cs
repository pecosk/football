using System;

namespace FootballLeague.Models
{
    public class Match
    {        
        public int Id { get; set; }
        public DateTime PlannedTime { get; set; }
        public virtual User Creator { get; set; }

        public int Team1Id { get; set; }
        public virtual Team Team1 { get; set; }

        public int Team2Id { get; set; }
        public virtual Team Team2 { get; set; }

        public object IsFull
        {
            get
            {
                return Team1.IsFull && Team2.IsFull;
            }
        }

        public bool Contains(User user)
        {
            return Team1.Contains(user) || Team2.Contains(user);
        }

        public Team GetTeam(int teamId)
        {
            if (Team1.Id == teamId) return Team1;
            if (Team2.Id == teamId) return Team2;

            throw new ArgumentException("No team with such id exists");
        }
    }
}