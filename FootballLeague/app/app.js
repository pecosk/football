
var footballApp = angular
    .module('footballApp', ['ngTable', 'ngResource', 'cgBusy', 'ngRoute', 'ui.bootstrap', 'multi-select', 'xeditable', 'ui.router.state', 'ncy-angular-breadcrumb']);

var users = 'api/users';
var identity = 'api/identity';
var match = 'api/matches';

footballApp.run(function ($rootScope, $state, $stateParams, editableOptions, editableThemes) {
    $rootScope.$state = $state;
    $rootScope.$stateParams = $stateParams;
    editableThemes.bs3.inputClass = 'input-sm';
    editableOptions.theme = 'bs3';
});