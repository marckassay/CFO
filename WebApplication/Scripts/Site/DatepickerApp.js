var DatepickerApp = angular.module('DatepickerApp', ['ngRoute', 'ui.bootstrap']);

DatepickerApp.controller('DatepickerDemoCtrl', DatepickerDemoCtrl);

var configFunction = function ($routeProvider, $httpProvider) {
    $routeProvider.
        when('#/QueryServiceWebRole', {
            template: ' ',
            controller: 'HomeCtrl'
        });
}
configFunction.$inject = ['$routeProvider', '$httpProvider'];

DatepickerApp.config(configFunction);