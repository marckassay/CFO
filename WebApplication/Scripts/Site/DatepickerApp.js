var DatepickerApp = angular.module('DatepickerApp', ['ngRoute', 'ui.bootstrap']);

DatepickerApp.controller('DatepickerDemoCtrl', DatepickerDemoCtrl);

var configFunction = function ($routeProvider, $httpProvider) {
    $routeProvider.
        when('/QueryServiceWebRole/:DateEx', {
            templateUrl: function (params) { return '/homeController/QueryServiceWebRole?DateEx=' + params.DateEx; }
        });
}
configFunction.$inject = ['$routeProvider', '$httpProvider'];

DatepickerApp.config(configFunction);