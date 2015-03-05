footballApp
    .controller("flTournamentsCtrl", function ($scope) {
        function registerFor(tournament) {

        }

        function view(tournament) {

        }

        function makeTournament(name, type, teams, matchesplayed, state) {
            return {
                Name: name,
                Type: type,
                Teams: teams,
                MatchesPlayed: matchesplayed,
                State: state,
                Action: function () {
                    if (this.State === 'registration') {
                        register(this);
                    }
                }
            }
        }
        
        $scope.tournaments = [
            makeTournament('street fighter', 'singleElimination', '5/16', '0/15', 'registration'),
            makeTournament('tekken 3', 'singleElimination', '16', '4/15', 'inProgress'),
            makeTournament('king of fighters', 'singleElimination', '16', '4/15', 'inProgress'),
            makeTournament('dead or alive', 'singleElimination', '16', '4/15', 'inProgress'),
             makeTournament('street fighter', 'singleElimination', '5/16', '0/15', 'registration'),
            makeTournament('tekken 3', 'singleElimination', '16', '4/15', 'inProgress'),
            makeTournament('king of fighters', 'singleElimination', '16', '4/15', 'inProgress'),
            makeTournament('dead or alive', 'singleElimination', '16', '4/15', 'inProgress'),
             makeTournament('street fighter', 'singleElimination', '5/16', '0/15', 'registration'),
            makeTournament('tekken 3', 'singleElimination', '16', '4/15', 'inProgress'),
            makeTournament('king of fighters', 'singleElimination', '16', '4/15', 'inProgress'),
            makeTournament('dead or alive', 'singleElimination', '16', '4/15', 'inProgress'),
             makeTournament('street fighter', 'singleElimination', '5/16', '0/15', 'registration'),
            makeTournament('tekken 3', 'singleElimination', '16', '4/15', 'inProgress'),
            makeTournament('king of fighters', 'singleElimination', '16', '4/15', 'inProgress'),
            makeTournament('dead or alive', 'singleElimination', '16', '4/15', 'inProgress'),
        ]                
    });