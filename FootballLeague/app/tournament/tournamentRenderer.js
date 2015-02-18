
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
            team1: makeTeam(pair[0], results[roundIndex][matchIndex], matchIndex * 2, roundIndex),
            team2: makeTeam(pair[1], results[roundIndex][matchIndex], matchIndex * 2 + 1, roundIndex),
            renderTeam: renderTeam
        };
    }

    function makeRound(matches, roundIndex) {
        return {
            roundIndex: roundIndex,
            matches: matches,
            renderMatch: renderMatch
        };
    }
    
    function getWinners(teams, results, roundIndex) {
        function evaluateWinners(teams, results) {
            var winners = [];
            for (var j = 0; j < previousRoundResults.length; j++) {
                var matchResult = previousRoundResults[j];
                var winnerIndicator = _.reduce(matchResult, function (acc, set) { return set[0] > set[1] ? acc + 1 : acc - 1; });
                var winner = teams[j][winnerIndicator > 0 ? 1 : 0];
                winners.push(winner);
            }

            var newMatches =
            _.chain(winners)
            .groupBy(function (winner, index) { return Math.floor(index / 2); })
            .toArray()
            .value();

            return newMatches;
        }

        if (roundIndex == 0) {
            return teams;
        }
        
        for (var i = 0; i < roundIndex; i++) {
            var previousRoundResults = results[i];
            teams = evaluateWinners(teams, previousRoundResults);
        }
                
        return teams;
    }

    var render = function ($container, teams, results) {
        var rounds = [],
            numberOfRounds = results.length;

        _.chain(_.range(numberOfRounds))
            .each(function (roundIndex) {
                var matches = _.chain(getWinners(teams, results, roundIndex))
                    .map(function (pair, index) { return makeMatch(pair, results, index, roundIndex); })
                    .value();
                rounds.push(makeRound(matches, roundIndex));
            });

        $container.html(renderBracket({ renderRound: renderRound, rounds: rounds }));
    }


    return {
        render: render
    }
}