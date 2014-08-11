var footballApp = angular.module('footballApp', ['ngTable', 'ngRoute']);
var url = 'api/users';
var identity = 'api/identity';
var match = 'api/matches';

footballApp.config(['$routeProvider',
  function ($routeProvider) {
      $routeProvider.
        when('/matches', {
            templateUrl: 'Angular/CreateMatch.html',
            controller: 'footballController'
        })
        .otherwise({
            templateUrl: 'Angular/UsersTable.html',
          controller: 'footballController'
        });
  }]);

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
            $http.get(url).success(callback);
        },
        insertFootballer: function (callback, fallback) {
            $http.post(url).success(callback).error(fallback);
        },
        deleteFootballer: function (id, callback) {
            $http.delete(url + '/' + id).success(callback);
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