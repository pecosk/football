namespace FootballLeague.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;    

    public class Team
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        public User Member1 { get; set; }
        
        public User Member2 { get; set; }        

        public bool IsEmpty
        {
            get
            {
                return Member1 == null && Member2 == null;   
            }            
        }
        
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

        public void SetMember(User user)
        {
            if (IsFull)            
                return;

            if (Member1 == null) Member1 = user;
            else Member2 = user;
        }

        public void RemoveMember(User user)
        {
            if (user.Equals(Member1)) Member1 = null;
            else if (user.Equals(Member2)) Member2 = null;

        }
    }
}