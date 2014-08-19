
footballApp.controller('identityController', function ($scope, $rootScope, userRepository, identityRepository) {
    $rootScope.registered = false;

    identityRepository.getIdentity(function (user) {
        $rootScope.identity = user;
        $rootScope.registered = true;
    }, function () {
        $rootScope.identity = null;
        $rootScope.registered = false;
    });

    $scope.register = function () {
        userRepository.insertUser(function (user) {
            $rootScope.registered = true;
            $rootScope.identity = user;
            $rootScope.$emit("reloadUsers", { registered: true });
        });
    };

    $scope.unregister = function () {
        userRepository.deleteUser($rootScope.identity.Id, function () {
            $rootScope.registered = false;
            $rootScope.identity = null;
            $rootScope.$emit("reloadUsers", { registered: false });
        });
    };
});