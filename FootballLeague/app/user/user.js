
footballApp.controller('userController', function ($scope, $rootScope, $filter, userRepository, ngTableParams) {
    $rootScope.$on("reloadUsers", function (event, args) {
        reloadUsers();
    });

    reloadUsers();

    function reloadUsers() {
        userRepository.getUsers(function (result) {
            $scope.users = result;

            if (typeof ($scope.tableParams) == 'undefined') {
                $scope.tableParams = new ngTableParams({
                    page: 1,
                    count: 10,
                    sorting: { Name: 'asc' }
                }, {
                    total: $scope.users.length,
                    getData: function ($defer, params) {
                        var orderedData = params.sorting()
                            ? $filter('orderBy')($scope.users, params.orderBy())
                            : $scope.users;
                        $defer.resolve(orderedData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                    }
                });
            }
            else
                $scope.tableParams.reload();
        });
    }
});