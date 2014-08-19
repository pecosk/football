
footballApp.controller('matchController', function ($scope, $rootScope, $resource, ngTableParams, $filter) {
    var Match = $resource('api/matches/:id', { id: '@id' }, { 'update': { method: 'PUT' } });

    $scope.submit = function () {
        var date = $scope.date;
        var time = $scope.time;
        var dateTime = new Date(date.getFullYear(), date.getMonth(), date.getDate(), time.getHours(), time.getMinutes()).toISOString();            
        Match.save({ PlannedTime: dateTime }).$promise.then(function () { reloadMatches(); });
    };

    $scope.open = function($event) {
        $event.preventDefault();
        $event.stopPropagation();
        $scope.opened = true;
    };

    $scope.toggleParticipation = function (matchId) {
        Match.update({ id: matchId }, function () { reloadMatches(); });
    };
    
    $scope.forJoin = function (matchId) {
        return $rootScope.registered && !isUserInMatch(matchId, $rootScope.identity);
    };

    $scope.forLeave = function (matchId) {
        return $rootScope.registered && isUserInMatch(matchId, $rootScope.identity);
    };

    $scope.minDate = new Date();
    $scope.date = new Date();
    $scope.time = new Date();

    function transformMatches(match) {
        return {
            Id: match.Id,
            PlannedTime: match.PlannedTime,
            Creator: match.Creator.Name,
            Players: match.Players.map(function (user) { return user.Name; }).join(' ')
        };
    };

    function isUserInMatch(matchId, user) {
        var matches = $scope.serverMatches.filter(function (match) { return match.Id == matchId });
        if (matches.length == 0)
            return false;

        return matches[0].Players.filter(function (player) { return player.Id == user.Id }).length;
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