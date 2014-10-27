
var footballApp = angular.module('footballApp', ['ngTable', 'ngResource', 'ngRoute', 'ui.bootstrap', 'multi-select', 'xeditable']);
var users = 'api/users';
var identity = 'api/identity';
var match = 'api/matches';

footballApp.config([
    '$routeProvider',
    function($routeProvider) {
        $routeProvider.
            when('/matches', {
                templateUrl: 'app/match/matches.html',
                controller: 'MatchesController'
            })
            .otherwise({
                templateUrl: 'app/user/users.html',
                controller: 'userController'
            });
    }
]);

footballApp.run(function (editableOptions, editableThemes) {
    editableThemes.bs3.inputClass = 'input-sm';    
    editableOptions.theme = 'bs3';
});