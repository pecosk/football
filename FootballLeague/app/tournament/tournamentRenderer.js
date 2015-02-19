
function tournamentRenderer() {
    var renderTeam = _.template($('#team-template').remove().text()),
        renderMatch = _.template($('#match-template').remove().text()),
        renderRound = _.template($('#round-template').remove().text()),
        renderBracket = _.template($('#bracket-template').remove().text());

    function makeTeam(name, scores, teamIndex, roundNumber) {
        return {
            name: name,
            roundIndex: roundNumber,
            teamIndex: teamIndex,
            scores: scores.map(function(score) { return score[teamIndex % 2]; })
        };
    }

    function makeMatch(pair, results, matchIndex, roundIndex) {
        return {
            team1: makeTeam(pair[0], results[matchIndex], matchIndex * 2, roundIndex),
            team2: makeTeam(pair[1], results[matchIndex], matchIndex * 2 + 1, roundIndex),
            result: results[matchIndex],
            renderTeam: renderTeam,
            getWinner: function() {
                var winnerIndicator = this.result.reduce(function(acc, set) {
                        return set[0] > set[1] ? acc + 1 : acc - 1;                    
                });

                return winnerIndicator > 0 ? this.team1 : this.team2;
            }
        };
    }

    function Match(team1, team2) {
        
    }

    function makeRound(matches, roundIndex) {
        return {
            roundIndex: roundIndex,
            matches: getMatchesForRound(matches, roundIndex),
            renderMatch: renderMatch
        };
    }
    
    function getMatchesForRound(matches, roundIndex) {       
        
        var nextRoundMatches = matches;

        for (var i = 0; i < roundIndex; i++) {         
            nextRoundMatches =
           _.chain(matches)
            .map(function (match) { return match.getWinner(); })
            .groupBy(function (winner, index) { return Math.floor(index / 2); })
            .map(function (pair, index) { return makeMatch(pair, results, index, 0); })
            .toArray()
            .value();
        }

        return nextRoundMatches;
    }

    var render = function ($container, teams, results) {
        var rounds = [],
            numberOfRounds = results.length;

        var resultsPerRound = results.reduce(function(acc, item) {
            acc.push(item);
            return acc;
        });

        var initialMatches = teams.map(function (pair, index) { return makeMatch(pair, resultsPerRound[0], index, 0); });
        _.chain(_.range(numberOfRounds))
            .each(function (roundIndex) {                                
                rounds.push(makeRound(initialMatches, roundIndex));
            });

        $container.html(renderBracket({ renderRound: renderRound, rounds: rounds }));
    }


    return {
        render: render
    }
}