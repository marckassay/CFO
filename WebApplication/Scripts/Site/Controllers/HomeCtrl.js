function HomeCtrl($scope, $http) {
    $http.get('/Home/QueryServiceWebRole').success(function (data) {
        $scope.wod = data;
    });
}