using System;
using System.Collections.Generic;

namespace FootballLeague.Models
{
    public class Match
    {
        public int Id { get; set; }
        public DateTime PlannedTime { get; set; }
        public virtual User Creator { get; set; }
        public virtual IList<User> Players { get; set; }
    }
}