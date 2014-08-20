﻿using System;

namespace FootballLeague.Models
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web.Script.Serialization;

    public class Match
    {
        public Match()
        {
            Team1 = new Team();
            Team2 = new Team();
        }

        public int Id { get; set; }
        public DateTime PlannedTime { get; set; }
        public User Creator { get; set; }        
        public virtual Team Team1 { get; set; }
        public virtual Team Team2 { get; set; }
        
        [NotMapped]
        [ScriptIgnore]
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