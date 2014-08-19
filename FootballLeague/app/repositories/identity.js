
footballApp.factory('identityRepository', function ($http) {
    return {
        getIdentity: function (callback, fallback) {
            $http.get(identity).success(callback).error(fallback);
        }
    }
});