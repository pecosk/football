using System;

namespace FootballLeague.Models
{
    public class Match
    {
        public int Id { get; set; }
        public DateTime PlannedTime { get; set; }
        public virtual User Creator { get; set; }
    }
}