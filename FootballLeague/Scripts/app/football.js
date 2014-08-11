var footballApp = angular.module('footballApp', ['ngTable', 'ngRoute']);
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

footballApp.controller('matchController', function ($scope, matchesRepository) {
    $scope.submit = function () {
        var dateTime = $scope.date + 'T' + $scope.time;
        matchesRepository.insertMatch(dateTime, function () { alert('match inserted'); });
    }
});

footballApp.controller('footballController', function ($scope, $filter, footballersRepository, identityRepository, ngTableParams) {
    $scope.registered = false;

    identityRepository.getIdentity(function (user) {
        $scope.identity = user;
        $scope.registered = true;
    }, function () {
        $scope.identity = null;
        $scope.registered = false;
    });

    $scope.register = function () {
        footballersRepository.insertFootballer(function (user) {
            $scope.registered = true;
            $scope.identity = user;
            reloadFootballers();
        });
    };

    $scope.unregister = function () {
        footballersRepository.deleteFootballer($scope.identity.Id, function () {
            $scope.registered = false;
            $scope.identity = null;
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
        }
    }
});