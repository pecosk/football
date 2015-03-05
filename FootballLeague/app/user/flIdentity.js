angular.module('footballApp').factory('flIdentity', function () {
    return {
        currentUser: undefined,
        isAuthenticated: function () {
            return !!this.currentUser;
        }
    }
})