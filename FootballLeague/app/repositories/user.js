
footballApp.factory('userRepository', function ($http) {
    return {
        getUsers: function (callback) {
            $http.get(users).success(callback);
        },
        insertUser: function (callback, fallback) {
            $http.post(users).success(callback).error(fallback);
        },
        deleteUser: function (id, callback) {
            $http.delete(users + '/' + id).success(callback);
        }
    }
});