﻿
var footballApp = angular.module('footballApp', ['ngTable', 'ngResource', 'ngRoute', 'ui.bootstrap', 'multi-select']);
var users = 'api/users';
var identity = 'api/identity';
var match = 'api/matches';

footballApp.config(['$routeProvider',
  function ($routeProvider) {
      $routeProvider.
        when('/matches', {
            templateUrl: 'app/match/newMatch.html',
            controller: 'matchController'
        })
        .otherwise({
            templateUrl: 'app/user/users.html',
          controller: 'userController'
        });
  }]);