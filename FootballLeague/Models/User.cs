using System.Collections.Generic;

namespace FootballLeague.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Inactive { get; set; }
        public IList<Match> Matches { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as User;
            return other != null && Id == other.Id;
        }
    }
}