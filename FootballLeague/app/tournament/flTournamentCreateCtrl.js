angular.module('footballApp')
    .controller('flTournamentCreateCtrl', function ($scope, $resource, $state) {
        var Tournament = $resource('api/tournament/:id', { id: '@id' });

        $scope.tournament = {
            name: '',
            eliminationType: 'Single',
            state: 'Registration',
            size: '8'
        }

        $scope.submit = function () {
            var date = new Date();            
            $scope.tournament.dateTime = date.getFullYear() + '-' + (date.getMonth() + 1) + '-' + date.getDate() + 'T' + date.getHours() + ':' + date.getMinutes();
            Tournament.save($scope.tournament).$promise.then(
                function () {
                    $state.go('tournaments')
                },
                function (e) {
                    console.log(e); $scope.alerts.push({ msg: e.data.ExceptionMessage });
                });
        };                            
    });