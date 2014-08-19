namespace FootballLeague.Tests.Data
{
    using System;

    using FootballLeague.Models;
    
    public class MatchBuilder
    {       
        private int? _id;
        
        private Team _team1;

        private Team _team2;

        private DateTime _plannedTime;

        private User _creator;

        private MatchBuilder() {}

        public static MatchBuilder Create()
        {
            return new MatchBuilder();            
        }

        public MatchBuilder WithId(int id)
        {
            _id = id;
            return this;
        }

        public MatchBuilder WithCreator(User creator)
        {
            _creator = creator;
            return this;
        }

        public MatchBuilder WithPlannedTime(DateTime plannedTime)
        {
            _plannedTime = plannedTime;
            return this;
        }

        public MatchBuilder WithTeamMember(User member)
        {
            if (_team1 == null)            
                _team1 = TeamBuilder.Create().WithMember(member).Build();            
            else if(!_team1.IsFull)
                _team1.SetMember(member);                                    
            else if (_team2 == null)
                _team2 = TeamBuilder.Create().WithMember(member).Build();            
            else if(!_team1.IsFull)
                _team2.SetMember(member);

            return this;
        }  

        public Match Build()
        {
            var match = new Match();
            if (_id.HasValue) match.Id = _id.Value;
            if (_creator != null) match.Creator = _creator;

            if (_team1 != null)
            {
                _team1.Parent = match;
                match.Team1 = _team1;
            }
            else
            {
                match.Team1 = new Team { Parent = match };
            }

            if (_team2 != null)
            {
                _team2.Parent = match;
                match.Team2 = _team2;
            }
            else
            {
                match.Team2 = new Team { Parent = match };
            }

            match.PlannedTime = _plannedTime;            

            return match;
        }                
    }

    public static class MatchData
    {
        public static Match MatchWithEmptyTeams
        {
            get
            {
                return MatchBuilder.Create().WithId(1).WithCreator(Users.Jurko).WithPlannedTime(DateTime.Now).Build();                                
            }
        }

        public static Match MatchWithTwoFullTeams
        {
            get
            {
                return MatchBuilder.Create()
                    .WithId(2)
                    .WithCreator(Users.Jurko)
                    .WithPlannedTime(DateTime.Now)
                    .WithTeamMember(Users.Ferko)
                    .WithTeamMember(Users.Jurko)
                    .WithTeamMember(Users.Dano)
                    .WithTeamMember(Users.Peto)
                    .Build();                                          
            }
        }
    }
}