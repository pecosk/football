using System;

namespace FootballLeague.Models
{
    public class Match
    {
        public int Id { get; set; }
        public DateTime PlannedTime { get; set; }
        public User Creator { get; set; }
    }
}