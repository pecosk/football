angular.module('footballApp')
    .controller('flTournamentJoinCtrl', function ($scope, $resource, $stateParams, $rootScope) {
        var User = $resource('api/users');
        var Tournament = $resource('api/tournament/:id');
        var TournamentTeam = $resource('api/tournamentteam/:id');
        
        $scope.isLoaded = false;
        $scope.tournamentPromise = Tournament.get({ id: $stateParams.id }, function (data) {
            $scope.tournament = data;
            $scope.isLoaded = true;
        }).$promise;

        $scope.usersPromise = User.query(function (data) {
            $scope.users = data;
        }).$promise;

        var currentUserId = $rootScope.identity.Id;        
        
        
        $scope.isRegistered = function () {
            if (!$scope.tournament) {
                return;
            }

            var currentUserId = $rootScope.identity.Id;
            return $scope.tournament.Teams.some(function (element) {
                return element.Member1.Id === currentUserId || element.Member2.Id === currentUserId;
            });
        }
        
        $scope.extractUserIds = function () {
            if (!$scope.tournament) {
                return;
            }

            return $scope.tournament.Teams
                .reduce(function (acc, item) {
                acc.push(item.Member1);
                acc.push(item.Member2);
                return acc;
                }, [])
                .map(function(user) {
                    return user.Id;
                });
        }

        function Team(name, partner) {
            return {
                TournamentId: $scope.tournament.Id,
                TeamName: name,
                Member1: $rootScope.identity,
                Member2: partner
            }
        }

        $scope.teamName = '';
        $scope.selectedPartner = null;
        $scope.submit = function () {
            var team = new Team($scope.teamName, $scope.selectedPartner);            
            TournamentTeam.save({ id: $scope.tournament.Id }, team);
            $scope.tournament.Teams.push(team);
        }

        $scope.leave = function () {
            var teamToDelete = $scope.tournament.Teams.filter(function (element) { return element.Member1.Id === currentUserId || element.Member2.Id === currentUserId })[0];                       
            TournamentTeam.delete({ id: teamToDelete.Id }, teamToDelete);
            var index = $scope.tournament.Teams.indexOf(teamToDelete);
            $scope.tournament.Teams.splice(index, 1);
        }
    })
    .filter('existingUsers', function () {
        return function (users, scope) {
            var registeredUserIds = scope.extractUserIds();
            return users && users
                    .filter(function (user) {
                        return registeredUserIds && registeredUserIds.every(function (id) { return user.Id !== id });
                    });            
        }
    });