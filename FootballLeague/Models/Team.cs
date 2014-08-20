namespace FootballLeague.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web.Script.Serialization;    

    public class Team
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        public User Member1 { get; set; }
        
        public User Member2 { get; set; }

        [ScriptIgnore]
        public bool IsEmpty
        {
            get
            {
                return Member1 == null && Member2 == null;   
            }            
        }

        [ScriptIgnore]
        public bool IsFull
        {
            get
            {
                return Member1 != null && Member2 != null;
            }
        }

        public bool Contains(User user)
        {
            return user.Equals(Member1) || user.Equals(Member2);
        }

        public User GetMember(User user)
        {
            if (user.Equals(Member1))
                return Member1;
            else if (user.Equals(Member2))
                return Member2;
            else 
                return null;
        }

        public bool SetMember(User user)
        {
            if (Member1 == null)
                Member1 = user;
            else if (Member2 == null)
                Member2 = user;
            else
                return false;

            return true;
        }

        public bool RemoveMember(User user)
        {
            if (user.Equals(Member1))
                Member1 = null;
            else if (user.Equals(Member2))
                Member2 = null;
            else
                return false;

            return true;
        }
    }
}