namespace FootballLeague.Tests.Data
{
    using FootballLeague.Models;
    
    public class TeamBuilder
    {
        private int? _id;

        private User _member1;

        private User _member2;        

        private TeamBuilder() {}

        public static TeamBuilder Create()
        {
            return new TeamBuilder();            
        }

        public TeamBuilder WithId(int id)
        {
            _id = id;
            return this;
        }

        public TeamBuilder WithMember(User member)
        {
            if (_member1 == null) 
                _member1 = member;
            else 
                _member2 = member;               
            
            return this;
        }        

        public Team Build()
        {
            var team = new Team();
            if (_id.HasValue) team.Id = _id.Value;

            if (_member1 != null) team.SetMember(_member1);

            if (_member2 != null) team.SetMember(_member2);            

            return team;
        }        
    }

    public static class TeamData
    {
        public static Team TeamWithFerko
        {
            get
            {
                var team = TeamBuilder.Create().WithId(1).WithMember(Users.Ferko).Build();

                return team;
            }
        }

        public static Team TeamWithFerkoJurko
        {
            get
            {
                var team = TeamBuilder.Create().WithId(2).WithMember(Users.Ferko).WithMember(Users.Jurko).Build();

                return team;
            }
        }

        public static Team TeamWithDanoPeto
        {
            get
            {
                var team = TeamBuilder.Create().WithId(3).WithMember(Users.Dano).WithMember(Users.Peto).Build();

                return team;
            }
        }

        public static Team EmptyTeam
        {
            get
            {
                return TeamBuilder.Create().WithId(4).Build();
            }
        }     
    }
}