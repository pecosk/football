function Round(matches, roundSize) {
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

function Bracket(rounds) {            
    return {
        winner: "",
        rounds: rounds
    }
}
