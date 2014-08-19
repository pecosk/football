
footballApp.controller('identityController', function ($scope, $rootScope, $resource) {
    var Identity = $resource('api/identity');
    var User = $resource('api/users/:id');

    $rootScope.registered = false;

    Identity.get().$promise.then(function (user) {
        $rootScope.identity = user;
        $rootScope.registered = true;
    }, function () {
        $rootScope.identity = null;
        $rootScope.registered = false;
    });

    $scope.register = function () {
        User.save().$promise.then(function (user) {
            $rootScope.registered = true;
            $rootScope.identity = user;
            $rootScope.$emit("reloadUsers", { registered: true });
        });
    };

    $scope.unregister = function () {
        User.remove({ id: $rootScope.identity.Id }).$promise.then(function () {
            $rootScope.registered = false;
            $rootScope.identity = null;
            $rootScope.$emit("reloadUsers", { registered: false });
        });
    };
});