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

function TeamPlaceholder() {
    return {
        TeamName: 'TBD'
    }
}

function Bracket(matches, size) {    
    var rounds = [];
    var firstRoundSize = size / 2;
    var numberOfMatches = size - 1;
    var firstRoundMatches = matches.slice(0, firstRoundSize);

    var previousRoundSize = 0;
    var nextRoundSize = size / 2;
    while (nextRoundSize >= 1) {
        rounds.push(new Round(matches.slice(previousRoundSize, nextRoundSize)));
        previousRoundSize = nextRoundSize;
        nextRoundSize = nextRoundSize / 2;
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
        rounds: rounds
    }
}
