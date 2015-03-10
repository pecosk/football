footballApp.controller("flTournamentDetailCtrl", function ($scope, $resource, $stateParams) {
    var User = $resource('api/users');
    var Tournament = $resource('api/tournament/:id');
    var TournamentTeam = $resource('api/tournamentteam/:id');
        
    var tournament;
    $scope.tournamentPromise = Tournament.get({ id: $stateParams.id }, function (data) {
        tournament = data;
        $scope.bracket = new Bracket(tournament.Matches, tournament.Size);
        $scope.isLoaded = true;
    }).$promise;    
}).directive("flMatch", function () {
    return {
        restrict: 'EA',
        replace: true,
        scope: {
            match: "=match",
            matchIndex: "=matchIndex",
            roundIndex: "=roundIndex"            
        },
        controller: function ($scope) {
            $scope.isEditing = false;
            $scope.isVisible = function (index) {
                return index === 0;
            }
            $scope.changeEditState = function () {
                $scope.isEditing = !$scope.isEditing;
            }
            $scope.getScores = function (index) {
                return index === 1 ?
                    $scope.match.sets.map(function (set) { return set.ScoreTeam1; }) :
                    $scope.match.sets.map(function (set) { return set.ScoreTeam2; });
            }
        },
        templateUrl: 'app/tournament/templates/match-template.html'
    }
}).directive("ngBracket", function () {
    return {
        restrict: 'EA',
        replace: true,
        scope: {
            bracket: "=bracket"
        },
        templateUrl: 'app/tournament/templates/bracket-template.html'
    }
}).directive("flRound", function () {
    return {
        restrict: 'EA',
        replace: true,
        scope: {
            round: "=",
            roundIndex: "@"
        },
        controller: function ($scope) {
            $scope.getClass = function () {
                return "round" + $scope.roundIndex;
            }

            $scope.isVisible = function (index) {
                return index !== $scope.round.matches.length - 1;
            }
        },
        templateUrl: 'app/tournament/templates/round-template.html'
    }
}).directive("flTeam", function () {
    return {
        restrict: 'EA',
        replace: true,
        scope: {
            index: "@",
            team: "=",
            roundIndex: "@",
            scores: "=",
            editState: "="
        },
        controller: function($scope) {
            $scope.isWinner = function (teamNumber, score) {
                if(teamNumber === 1 && score.Team1Score > score.Team2Score ||
                   teamNumber === 2 && score.Team1Score < score.Team2Score) {
                    return "winner";
                }
                else if (score.Team1Score !== score.Team2Score) {
                    return "looser";
                }
            }
        },
        templateUrl: 'app/tournament/templates/team-template.html'
    }
});