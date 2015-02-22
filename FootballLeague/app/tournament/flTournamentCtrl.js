footballApp.controller("flTournamentCtrl", function ($scope, $resource) {
    $scope.bracket = new Bracket();    
}).directive("flMatch", function () {
    return {
        restrict: 'EA',
        replace: true,
        scope: {
            match: "=match",            
        },
        templateUrl: 'app/tournament/match-template.html'
    }
}).directive("ngBracket", function () {
    return {
        restrict: 'EA',
        replace: true,
        scope: {
            bracket: "=bracket",
        },
        templateUrl: 'app/tournament/bracket-template.html'
    }
}).directive("flRound", function () {
    return {
        restrict: 'EA',
        replace: true,
        scope: {
            round: "=round"
        },
        templateUrl: 'app/tournament/round-template.html'
    }
});