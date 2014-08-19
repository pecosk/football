
footballApp.factory('matchRepository', function ($http) {
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