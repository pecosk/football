
footballApp.controller('MatchesController', function ($scope, $rootScope, $resource, ngTableParams, $filter) {
    var Match = $resource('api/matches/:id', { id: '@id' }, {
        update: { method: 'PUT', params: { teamId: '@teamId' } },
        updateScore: { method: 'PUT', params: { sets: '@t1Score' } }
    });

    $scope.alerts = [];

    $scope.closeAlert = function (index) {
        $scope.alerts.splice(index, 1);
    };

    $scope.toggleParticipation = function (matchId, teamId) {
        Match.update({ id: matchId, teamId: teamId }, function () { $scope.reloadMatches(); });
    };

    $scope.updateScore = function (match) {
        Match.updateScore({ id: match.Id }, match.Sets, function () { $scope.reloadMatches(); });
    };

    $scope.forJoin = function (match) {
        var user = $rootScope.identity;
        return $rootScope.registered && !match.containsPlayer(user);
    };

    $scope.forLeave = function (match, team) {
        var user = $rootScope.identity;
        return $rootScope.registered && team.hasMember(user);
    };

    $scope.isInvited = function (match) {
        var user = $rootScope.identity;
        return $rootScope.registered
            && !match.containsPlayer(user)
            && match.Invites.filter(function (i) { return i.Name == user.Name; }).length == 1;
    };

    $scope.isCurrentUser = function (u) {
        var user = $rootScope.identity;
        return u && u.Name === user.Name;
    };

    function transformMatches(match) {
        function extendTeam(team) {
            return Object.create(team, {
                isFull: { value: function () { return this.Member1 && this.Member2; } },
                isEmpty: { value: function () { return !this.Member1 && !this.Member2; } },
                hasMember: {
                    value: function (user) {
                        return ((this.Member1 && (this.Member1.Id === user.Id)) || (this.Member2 && (this.Member2.Id === user.Id)));
                    }
                },
            });
        }

        match.Team1 = extendTeam(match.Team1);
        match.Team2 = extendTeam(match.Team2);
        return Object.create(match, {
            containsPlayer: { value: function(user) { return this.Team1.hasMember(user) || this.Team2.hasMember(user); } },
            canEditScore: { value: function() { return this.containsPlayer($rootScope.identity) && this.Sets.length; } },
            canAddSet: { value: function() { return this.containsPlayer($rootScope.identity) && this.Sets.length < 3; } },
            addSet: { value: function() { this.Sets.push({ Team1Score: 0, Team2Score: 0 }); } },
            calculateTeam1Score: { value: function() { return this.Sets.reduce(function(previousValue, set) { return previousValue + (set.Team1Score > set.Team2Score); }, 0); } },
            calculateTeam2Score: { value: function() { return this.Sets.reduce(function(previousValue, set) { return previousValue + (set.Team1Score < set.Team2Score); }, 0); } },
            makeTooltip: { value: function() { return match.Sets.reduce(function(acc, item) { return acc + item.Team1Score + ':' + item.Team2Score + '||'; }, ''); } }
        });
    }

    $scope.reloadMatches = function () {
        Match.query().$promise.then(function (result) {
            var plannedServerMatches = result.filter(function (x) {
                return new Date(x.PlannedTime) >= new Date();
            });
            $scope.plannedMatches = plannedServerMatches.map(transformMatches);

            if (typeof ($scope.tableParams) === 'undefined') {
                $scope.tableParams = new ngTableParams({
                    page: 1,
                    count: 10,
                    sorting: { PlannedTime: 'asc' }
                }, {
                    total: $scope.plannedMatches.length,
                    getData: function ($defer, params) {
                        var orderedData = params.sorting()
                            ? $filter('orderBy')($scope.plannedMatches, params.orderBy())
                            : $scope.plannedMatches;
                        $defer.resolve(orderedData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                    }
                });
            }
            else {
                $scope.tableParams.reload();
            }

            var finishedServerMatches = result.filter(function (x) {
                return new Date(x.PlannedTime) < new Date();
            });
            $scope.finishedMatches = finishedServerMatches.map(transformMatches);

            if (typeof ($scope.tableParams2) === 'undefined') {
                $scope.tableParams2 = new ngTableParams({
                    page: 1,
                    count: 10,
                    sorting: { PlannedTime: 'asc' }
                }, {
                    total: $scope.finishedMatches.length,
                    getData: function ($defer, params) {
                        var orderedData = params.sorting()
                            ? $filter('orderBy')($scope.finishedMatches, params.orderBy())
                            : $scope.finishedMatches;
                        $defer.resolve(orderedData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                    }
                });
            }
            else {
                $scope.tableParams2.reload();
            }
        });
    };

    $scope.reloadMatches();    
});