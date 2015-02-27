function Set() {
    var 
        scoreTeam1 = Math.random() < 0.5 ? 8 : Math.floor(Math.random() * 8) + 1,
        scoreTeam2 = scoreTeam1 !== 8 ? 8 : (Math.floor(Math.random() * 8));
    return {
        scoreTeam1: scoreTeam1,
        scoreTeam2 : scoreTeam2
    }
}

function Team(name, id) {
    return {
        name: name,
        id : id
    }
}

function Match(team1, team2, sets) {
    var team1,
        team2,
        sets;

    team1 = team1 || new Team("no name");
    team2 = team2 || new Team("no name");
    sets = sets || [new Set(), new Set(), new Set()];

    function getWinner(){
        if (sets.length > 0) {
            var winnerIndicator = this.sets.reduce(function (acc, set) { return (set.scoreTeam1 > set.scoreTeam2) ? acc + 1 : acc - 1; }, 0);
            return winnerIndicator > 0 ? this.team1 : this.team2;            
        }        
    }


    return {
        team1: team1,
        team2: team2,
        sets: sets,
        getWinner: getWinner
    }
}

function Round(matches) {
    var isLast = function (match) {
        var isLast = this.matches.indexOf(match) == this.matches.length - 1;
        return isLast;
    }
    return {
        matches: matches,
        isLast: isLast
    }
}

function Bracket(firstRound) {
    var rounds = firstRound || makeRounds();

    function makeRounds() {        
        var rounds = [];
        var round1 = new Round([
            new Match(new Team("team1", 1), new Team("team2", 2)),
            new Match(new Team("team3", 3), new Team("team4", 4)),
            new Match(new Team("team5", 5), new Team("team6", 6)),
            new Match(new Team("team7", 7), new Team("team8", 8)),
            new Match(new Team("team9", 9), new Team("team10", 10)),
            new Match(new Team("team11", 11), new Team("team12", 12)),
            new Match(new Team("team13", 13), new Team("team14", 14)),
            new Match(new Team("team15", 15), new Team("team16", 16)),
        ]);
        rounds.push(round1);
        var nextRound = round1;
        while(true){
            nextRound = makeNextRound(nextRound);
            rounds.push(nextRound)
            if (nextRound.matches.length == 1)
                break;
        }
                
        return rounds;
    }

    function makeNextRound(round) {
        var winners = _.chain(round.matches)
                    .map(function (match) { return match.getWinner(); })
         var nextRound = _.chain(round.matches)
                    .map(function (match) { return match.getWinner(); })
                    .groupBy(function (winner, index) { return Math.floor(index / 2); })
                    .map(function (pair) { return new Match(pair[0], pair[1]); })
                    .toArray()
                    .value();

         return new Round(nextRound);
    }

    return {
        winner: "",
        rounds: makeRounds()
    }
}
