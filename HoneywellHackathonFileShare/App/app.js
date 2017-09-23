var fileShareApp = angular.module('fileShareApp', ['ngRoute']);


fileShareApp.config(['$locationProvider', '$routeProvider', function ($locationProvider, $routeProvider) {
    $routeProvider
    .when("/Home", {
        templateUrl: "App/efHomePage.html",
        controller:"efHomeController"
    })
    .otherwise({
        redirectTo:"/Home"
    });
    $locationProvider.html5Mode({
        enabled: true,
        requireBase: false
    });
}]);