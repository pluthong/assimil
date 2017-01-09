

/* lesson Controller */

angular.module('lessonMod', []).controller('LessonCtrl', ['$scope', '$http', '$timeout', function ($scope, $http, $timeout, $rootScope) {
    $scope.sound = null;
    $scope.classplaying = false;
    $scope.init = function (lessonId) {
        $scope.lessonId = lessonId;
        $scope.loading = true;
        $scope.getdata(function () {
            $scope.loading = false;
            $('.body-content-loading-overlay').attr("style", "display:none");
        });
    };

    // service data
    $scope.getdata = function (func) {
        var data = { lessonId: $scope.lessonId };
        $http({ method: 'POST', url: '/Lesson/GetLesson', data: data }).success(function (result) {

            $scope.lesson = result;
            $scope.englishLesson = result.EnglishLesson;
            $scope.frenchLesson = result.FrenchLesson;
       
            $timeout(function () {
                // run action
                if (func) func();

            }, 3000);

        });
    };

}]);