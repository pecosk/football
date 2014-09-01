
footballApp.controller('matchController', function ($scope, $rootScope, $resource, ngTableParams, $filter) {
    var Match = $resource('api/matches/:id', { id: '@id' }, { 'update': { method: 'PUT', params: { teamId: '@teamId' } } });
    var User = $resource('api/users');

    $scope.submit = function () {
        var date = $scope.date;
        var time = $scope.time;
        var dateTime = date.getFullYear() + '-' + (date.getMonth() + 1) + '-' + date.getDate() + 'T' + time.getHours() + ':' + time.getMinutes();
        Match.save({ PlannedTime: dateTime, Invites: $scope.selectedUsers.map(cleanUser) }).$promise.then(function () { reloadMatches(); });
    };

    $scope.open = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();
        $scope.opened = true;
    };

    $scope.toggleParticipation = function (matchId, teamId) {
        Match.update({ id: matchId, teamId: teamId }, function () { reloadMatches(); });
    };

    $scope.forJoin = function (match, team) {
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
            && match.Invites.filter(function (i) { return i.Name == user.Name }).length == 1;
    };

    $scope.minDate = new Date();
    $scope.date = new Date();
    $scope.time = new Date();

    loadUsers();

    function loadUsers() {
        User.query().$promise.then(function (data) {
            $scope.users = data;
        });
    };

    function cleanUser(user) {
        return {
            Id: user.Id,
            Name: user.Name
        };
    }

    function transformMatches(match) {
        function extendTeam(team) {
            var currentUser = $rootScope.identity;
            return Object.create(team, {
                isFull: { value: function () { return this.Member1 && this.Member2 } },
                isEmpty: { value: function () { return !this.Member1 && !this.Member2 } },
                hasMember: {
                    value: function (user) {
                        return ((this.Member1 && (this.Member1.Id === user.Id)) || (this.Member2 && (this.Member2.Id === user.Id)))
                    }, enumerable: true
                },
            });
        }

        return Object.create(match, {
            CreatorName: { value: match.Creator.Name },
            Team1: { value: extendTeam(match.Team1) },
            Team2: { value: extendTeam(match.Team2) },
            containsPlayer: { value: function (user) { return this.Team1.hasMember(user) || this.Team2.hasMember(user); }, enumerable: true },
        });
    }

    function reloadMatches() {
        Match.query().$promise.then(function (result) {
            $scope.serverMatches = result;
            $scope.matches = result.map(transformMatches);

            if (typeof ($scope.tableParams) == 'undefined') {
                $scope.tableParams = new ngTableParams({
                    page: 1,
                    count: 10,
                    sorting: { PlannedTime: 'asc' }
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