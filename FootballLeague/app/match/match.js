
footballApp.controller('matchController', function ($scope, $rootScope, $resource, ngTableParams, $filter) {
    var Match = $resource('api/matches/:id', { id: '@id' }, { 'update': { method: 'PUT' } });

    $scope.submit = function () {
        var date = $scope.date;
        var time = $scope.time;
        var dateTime = new Date(date.getFullYear(), date.getMonth(), date.getDate(), time.getHours(), time.getMinutes()).toISOString();
        Match.save({ PlannedTime: dateTime }).$promise.then(function () { reloadMatches(); });
    };

    $scope.open = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();
        $scope.opened = true;
    };

    $scope.toggleParticipation = function (matchId) {
        Match.update({ id: matchId }, function () { reloadMatches(); });
    };

    $scope.forJoin = function (match, team) {
        return $rootScope.registered && !isUserInMatch(match, team);
    };

    $scope.forLeave = function (match, team) {
        return $rootScope.registered && isUserInMatch(match, team);
    };

    $scope.minDate = new Date();
    $scope.date = new Date();
    $scope.time = new Date();

    function transformMatches(match) {
        function extendTeam(team) {
            return Object.create(team, {
                isFull: { value: function () { this.Member1 && this.Member2 } },
                isEmpty: { value: function () { !this.Member1 && !this.Member2 } },
                hasMember: { value: function (user) { (this.Member1 && (this.Member1.id === user.id)) || (this.Member2 && (this.Member2.id === user.id)) } }
            });
        }

        return Object.create(match, {
            CreatorName: { value: match.Creator.Name },
            Team1: { value: extendTeam(match.Team1) },
            Team2: { value: extendTeam(match.Team2) },
            containsPlayer: { value: function (user) { return this.Team1.hasMember(user) || this.Team2.hasMember(user); } }
        });
    }

    function isUserInMatch(match, team) {
        var user = $rootScope.identity;

        return team.hasMember(user) || match.containsPlayer(user);
    };

    function reloadMatches() {
        Match.query().$promise.then(function (result) {
            $scope.serverMatches = result;
            $scope.matches = result.map(transformMatches);

            if (typeof ($scope.tableParams) == 'undefined') {
                $scope.tableParams = new ngTableParams({
                    page: 1,
                    count: 10,
                    sorting: { Name: 'asc' }
                }, {
                    total: $scope.matches.length,
                    getData: function ($defer, params) {
                        var orderedData = params.sorting()
                            ? $filter('orderBy')($scope.matches, params.orderBy())
                            : $scope.matches;
                        $defer.resolve(orderedData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                    }
                });
            }
            else {
                $scope.tableParams.reload();
            }
        });
    };

    reloadMatches();
});