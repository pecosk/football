angular.module("footballApp")
    .config(function ($breadcrumbProvider) {
        $breadcrumbProvider.setOptions({
            prefixStateName: 'tournaments'            
        });
    })
    .config(function ($stateProvider, $urlRouterProvider) {
        $stateProvider
          .state('matches', {
              url: '/matches',
              templateUrl: 'app/match/matches.html',
              ncyBreadcrumb: {
                  label: 'matches',
                  controller: 'MatchesController'
              }
          })        
        .state('tournaments', {
            url: '/tournaments',
            templateUrl: 'app/tournament/tournaments.html',
            ncyBreadcrumb: {
                label: 'Tournaments',
                controller: 'flTournamentsCtrl'
            }
        })
        .state('tournaments-detail', {
            url: '/tournaments.detail',
            templateUrl: 'app/tournament/tournamentDetail.html',
            ncyBreadcrumb: {
                parent: 'tournaments',
                label: 'detail',
                controller: 'flTournamentDetailCtrl'
            }
        })
        .state('tournaments-join', {
            url: '/tournaments.join',
            templateUrl: 'app/tournament/tournamentJoin.html',
            ncyBreadcrumb: {
                parent: 'tournaments',
                label: 'join',
                controller: 'flTournamentJoinCtrl'
            }
        })
        .state('tournaments-create', {
            url: '/tournaments.create',
            templateUrl: 'app/tournament/tournamentCreate.html',
            ncyBreadcrumb: {
                parent: 'tournaments',
                label: 'create',
                controller: 'flTournamentCreateCtrl'
            }
        })
        .state('users', {
            url: '/users',
            templateUrl: 'app/user/users.html',
            ncyBreadcrumb: {
                label: 'users',
                controller: 'userController'
            }
        });
    });