footballApp.controller("flTournamentCtrl", function($scope, $resource) {
    var teams = [
        ["Team1", "Team2"],
        ["Team3", "Team4"],
        ["Team5", "Team6"],
        ["Team7", "Team8"],
        ["Team9", "Team10"],
        ["Team11", "Team12"],
        ["Team13", "Team14"],
        ["Team15", "Team16"]
    ];

    var round1 = [
        [2, 1],
        [2, 1],
        [5, 1],
        [2, 1],
        [2, 3],
        [2, 4],
        [2, 8],
        [4, 1]
    ];

    var round2 = [        
        [3, 2],
        [2, 4],
        [2, 8],
        [4, 1]
    ];

    var round3 = [
        [2, 1],
        [2, 3]
    ];

    var round4 = [
        [8, 0]
    ];

    var results = [
        round1,
        round2,
        round3,
        round4
    ];

    var team =
        "<div>" +
        "" +
        "" +
        "";

    var container = $("#bracket");
    

});