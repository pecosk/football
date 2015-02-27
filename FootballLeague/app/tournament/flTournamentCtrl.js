footballApp.controller("flTournamentCtrl", function ($scope, $resource) {
    $scope.bracket = new Bracket();
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
                    $scope.match.sets.map(function (set) { return set.scoreTeam1; }) :
                    $scope.match.sets.map(function (set) { return set.scoreTeam2; });
            }
        },
        templateUrl: 'app/tournament/match-template.html'
    }
}).directive("ngBracket", function () {
    return {
        restrict: 'EA',
        replace: true,
        scope: {
            bracket: "=bracket"
        },
        templateUrl: 'app/tournament/bracket-template.html'
    }
}).directive("flRound", function () {
    return {
        restrict: 'EA',
        replace: true,
        scope: {
            round: "=round",
            roundIndex: "=roundindex"
        },
        controller: function ($scope) {
            $scope.getClass = function () {
                return "round" + $scope.roundIndex + " col-md-3 col-sm-6 col-xs-6";
            }

            $scope.isVisible = function (index) {
                return index !== $scope.round.matches.length - 1;
            }
        },
        templateUrl: 'app/tournament/round-template.html'
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
            $scope.getScore = function (teamNumber) {
                return teamNumber === '1' ? score.scoreTeam1 : score.scoreTeam2;
            }
        },
        templateUrl: 'app/tournament/team-template.html'
    }
});