
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
            scores: scores[teamIndex % 2]
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
    
    var render = function ($container, teams, results) {
        var rounds = [];
        _.chain(_.range(1))
            .each(function (roundIndex) {
                var matches = _.chain(teams)
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