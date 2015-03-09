footballApp
    .controller("flTournamentsCtrl", function ($scope, $resource, $state) {
        var Tournament = $resource('api/tournament/:id', { id: '@id' }, 
            { update: { method: 'PUT', params: { state: '@state' } } });        
        var stateNames = ['Registration', 'InProgress', 'Finished'];
        var elimantionNames = ['Single', 'Double'];
        $scope.tournaments = [];
        $scope.isFull = function (tournament) {
            return tournament.Teams.length === tournament.Size;
        }

        $scope.getStateName = function (i) {
            return stateNames[Number(i)];
        }
        $scope.eleminationName = function (i) {
            return elimantionNames[Number(i)];
        }        

        $scope.refresh = function () {
            // result: Tournament { Name, EliminationType, State, Size, Teams, Matches }
            Tournament.query().$promise.then(function (result) {
                result.forEach(extendTournament)
                $scope.tournaments = result;
            });
        };

        $scope.start = function (tournament) {
            tournament.State = 1;
            Tournament.update({ id: tournament.Id, state: 'InProgress' });
        }

        $scope.refresh();

        function extendTournament(tournament) {            
            Object.defineProperty(tournament, 'NumberOfTeams', {
                get: function () { return this.Size === this.Teams.length ? this.Size : (this.Teams.length + '/' + this.Size); },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(tournament, 'MatchesPlayed', {
                get: function () { return (this.Matches.length + '/' + (Number(this.Size) - 1)); },
                enumerable: true,
                configurable: true
            });
        }           
    })
    .filter('toStateName', function () {
        var stateNames = ['Registration', 'InProgress', 'Finished'];
        return function (input) {
            return stateNames[Number(input)];
        }
    }).filter('toEliminationTypeName', function () {        
        return function (input) {
            return input === 0 ? 'Single Elimination' : 'Double Elimination';
        }
    });