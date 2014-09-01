
footballApp.controller('NewMatchController', function ($scope, $rootScope, $resource, ngTableParams, $filter) {
    var Match = $resource('api/matches/:id', { id: '@id' }, { 'update': { method: 'PUT', params: { teamId: '@teamId' } } });
    var User = $resource('api/users');

    $scope.alerts = [];

    $scope.submit = function () {
        var date = $scope.date;
        var time = $scope.time;
        var dateTime = date.getFullYear() + '-' + (date.getMonth() + 1) + '-' + date.getDate() + 'T' + time.getHours() + ':' + time.getMinutes();
        Match.save({ PlannedTime: dateTime, Invites: $scope.selectedUsers.map(cleanUser) }).$promise.then(
            function () { $scope.reloadMatches(); },
            function (e) {
                console.log(e); $scope.alerts.push({ msg: e.data.ExceptionMessage });
            });
    };

    $scope.closeAlert = function (index) {
        $scope.alerts.splice(index, 1);
    };

    $scope.open = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();
        $scope.opened = true;
    };

    $scope.minDate = new Date();
    $scope.date = new Date();
    $scope.time = new Date();

    loadUsers();

    function loadUsers() {
        User.query().$promise.then(function (data) {
            $scope.users = data;
        });
    };

    function cleanUser(user) {
        return {
            Id: user.Id,
            Name: user.Name
        };
    }
});