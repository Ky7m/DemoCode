"use strict";
angular.module("todoApp", ["ngRoute"])
.config(["$routeProvider", "$httpProvider", function ($routeProvider) {
    $routeProvider.when("/Home", {
        controller: "homeCtrl",
        templateUrl: "/App/Views/Home.html"
    }).when("/TodoList", {
        controller: "todoListCtrl",
        templateUrl: "/App/Views/TodoList.html"
    }).when("/UserData", {
        controller: "userDataCtrl",
        templateUrl: "/App/Views/UserData.html"
    }).otherwise({ redirectTo: "/Home" });
}]);