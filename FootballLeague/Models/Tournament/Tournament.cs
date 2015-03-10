using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Linq;
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

        public List<TournamentRound> Rounds{ get; set; }

        public User Creator { get; set; }

        public Tournament() { }

        public Tournament(string name, EliminationType eliminationType, TournamentState state, int size)
        {
            Name = name;
            EliminationType = eliminationType;
            State = state;
            Size = size;

            Teams = new List<TournamentTeam>();
            Rounds = new List<TournamentRound>();
        }

        internal void Start()
        {
            State = TournamentState.InProgress;
            CreateFirstRound();
            CreateRemainingRounds();
        }

        private void CreateFirstRound()
        {
            var pairs = Teams.Select((value, index) => new { value, index }).GroupBy(x => x.index / 2, x => x.value);
            var matches = pairs.Select(
                (x, index) =>
                {
                    var t = new TournamentMatch(x.First(), x.Last(), index);
                    t.Sets.Add(new TournamentSet());
                    t.Sets.Add(new TournamentSet());
                    t.Sets.Add(new TournamentSet());
                    return t;
                }).ToList();

            Rounds.Add(new TournamentRound(0, matches));
        }

        private void CreateRemainingRounds()
        {
            var numberOfRounds = (int)Math.Log(Size, 2); 
            Enumerable.Range(1, numberOfRounds - 1).ToList().ForEach(x =>
            {
                var round = TournamentRound.CreateEmptyRound(Size, x);
                Rounds.Add(round);
            });
        }

    }
}