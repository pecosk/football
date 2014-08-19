using System;

namespace FootballLeague.Models
{
    public class Match
    {        
        public int Id { get; set; }
        public DateTime PlannedTime { get; set; }
        public virtual User Creator { get; set; }

        public virtual Team Team1 { get; set; }
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
    }
}