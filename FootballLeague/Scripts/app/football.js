var footballApp = angular.module('footballApp', ['ngTable', 'ngRoute', 'ui.bootstrap']);
var users = 'api/users';
var identity = 'api/identity';
var match = 'api/matches';

footballApp.config(['$routeProvider',
  function ($routeProvider) {
      $routeProvider.
        when('/matches', {
            templateUrl: 'Angular/CreateMatch.html',
            controller: 'matchController'
        })
        .otherwise({
            templateUrl: 'Angular/UsersTable.html',
          controller: 'footballController'
        });
  }]);

footballApp.controller('matchController', function ($scope, $rootScope, matchesRepository, ngTableParams, $filter) {
    $scope.submit = function () {
        var date = $scope.date;
        var time = $scope.time;
        var dateTime = date.getFullYear() + '-' + (date.getMonth() + 1) + '-' + date.getDate() + 'T' + time.getHours() + ':' + time.getMinutes();
        matchesRepository.insertMatch(dateTime, function () { reloadMatches(); });
    };
    $scope.open = function($event) {
        $event.preventDefault();
        $event.stopPropagation();
        $scope.opened = true;
    };
    function getMatches() {
        return $scope.matches;
    };
    function transformMatches(match) {
        return {
            Id: match.Id,
            PlannedTime: match.PlannedTime,
            Creator: match.Creator.Name,
            Players: match.Players.map(function (user) { return user.Name; }).join(' ')
        };
    };
    reloadMatches();
    $scope.toggleParticipation = function (matchId) {
        matchesRepository.toggleMatchParticipation(matchId, function () { reloadMatches(); });
    };
    $scope.forJoin = function (matchId) {
        return $rootScope.registered && !isUserInMatch(matchId, $rootScope.identity);
    };
    $scope.forLeave = function (matchId) {
        return $rootScope.registered && isUserInMatch(matchId, $rootScope.identity);
    };
    function isUserInMatch(matchId, user) {
        var matches = $scope.serverMatches.filter(function (match) { return match.Id == matchId });
        if (matches.length == 0)
            return false;

        return matches[0].Players.filter(function (player) { return player.Id == user.Id }).length;
    };
    function reloadMatches() {
        matchesRepository.getPlannedMatches(function (result) {
            $scope.serverMatches = result;
            $scope.matches = result.map(transformMatches);

            if (typeof ($scope.tableParams) == 'undefined') {
                $scope.tableParams = new ngTableParams({
                    page: 1,
                    count: 10,
                    sorting: { Name: 'asc' }
                }, {
                    total: getMatches().length,
                    getData: function ($defer, params) {
                        var orderedData = params.sorting()
                            ? $filter('orderBy')(getMatches(), params.orderBy())
                            : getMatches();
                        $defer.resolve(orderedData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                    }
                });
            }
            else
                $scope.tableParams.reload();
        });
    };
    $scope.minDate = new Date();
    $scope.date = new Date();
    $scope.time = new Date();
});

footballApp.controller('footballController', function ($scope, $rootScope, $filter, footballersRepository, identityRepository, ngTableParams) {
    $rootScope.registered = false;

    identityRepository.getIdentity(function (user) {
        $rootScope.identity = user;
        $rootScope.registered = true;
    }, function () {
        $rootScope.identity = null;
        $rootScope.registered = false;
    });

    $scope.register = function () {
        footballersRepository.insertFootballer(function (user) {
            $rootScope.registered = true;
            $rootScope.identity = user;
            reloadFootballers();
        });
    };

    $scope.unregister = function () {
        footballersRepository.deleteFootballer($rootScope.identity.Id, function () {
            $rootScope.registered = false;
            $rootScope.identity = null;
            reloadFootballers();
        });
    };

    function getFootballers() {
        return $scope.footballers;
    }

    reloadFootballers();
    function reloadFootballers() {
        footballersRepository.getFootballers(function (result) {
            $scope.footballers = result;

            if (typeof ($scope.tableParams) == 'undefined') {
                $scope.tableParams = new ngTableParams({
                    page: 1,
                    count: 10,
                    sorting: { Name: 'asc' }
                }, {
                    total: getFootballers().length,
                    getData: function ($defer, params) {
                        var orderedData = params.sorting()
                            ? $filter('orderBy')(getFootballers(), params.orderBy())
                            : getFootballers();
                        $defer.resolve(orderedData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                    }
                });
            }
            else
                $scope.tableParams.reload();
        });
    }
});

footballApp.factory('footballersRepository', function ($http) {
    return {
        getFootballers: function (callback) {
            $http.get(users).success(callback);
        },
        insertFootballer: function (callback, fallback) {
            $http.post(users).success(callback).error(fallback);
        },
        deleteFootballer: function (id, callback) {
            $http.delete(users + '/' + id).success(callback);
        }
    }
});

footballApp.factory('identityRepository', function ($http) {
    return {
        getIdentity: function (callback, fallback) {
            $http.get(identity).success(callback).error(fallback);
        }
    }
});

footballApp.factory('matchesRepository', function ($http) {
    return {
        insertMatch: function (date, callback) {
            $http.post(match, { PlannedTime: date }).success(callback);
        },
        getPlannedMatches: function (callback) {
            $http.get(match).success(callback);
        },
        toggleMatchParticipation: function (matchId, callback) {
            $http.put(match + '/' + matchId).success(callback);
        }
    }
});