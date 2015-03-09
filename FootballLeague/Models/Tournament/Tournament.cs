using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FootballLeague.Models.Tournament
{
    public class Tournament
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public EliminationType EliminationType { get; set; }
        
        public TournamentState State { get; set; }

        public int Size { get; set; }

        public List<TournamentTeam> Teams { get; set; }

        public List<TournamentMatch> Matches { get; set; }

        public User Creator { get; set; }

        public Tournament() { }

        public Tournament(string name, EliminationType eliminationType, TournamentState state, int size)
        {
            Name = name;
            EliminationType = eliminationType;
            State = state;
            Size = size;

            Teams = new List<TournamentTeam>();
            Matches = new List<TournamentMatch>();
        }
    }
}