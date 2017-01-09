// Directives Module


var dirmod = angular.module("directiveMod", []);

/* Directive  tpsAudioDuration */

dirmod.directive('tpsAudioDuration', function () {
    return {
        restrict: 'E',
        link: function (scope,element,attrs) {
            var src = attrs["trackDocid"];
            var audio = new Audio();
            audio.src = src;
            audio.addEventListener('loadedmetadata', function () {
                var duration = Math.round(audio.duration);
                var min = Math.floor(duration / 60);
                var sec = duration % 60;
                scope.audiourl = min + ' : ' + sec;
            });
        },
        template: "<div class='duration'>{{audiourl}}</div>"
    };

});


dirmod.directive('tpsPlaying', ['$rootScope', function ($rootScope) {
    return {
        link: function (scope, element, attrs) {

            // initializze resource to false
            $rootScope.lockRessource = false;

            element.on("click", function () {

                console.log($rootScope.lockRessource);

                // check if resource blocked
                if ($rootScope.lockRessource)
                    return;
                
                // Create the new instance of audio
                scope.sound = new Audio();
               
                // get attributes from directive
                var start = attrs["startPlay"];
                var end = attrs["endPlay"];
                var src = attrs["tpsPlaying"];
                // set audio.src
                scope.sound.src = src;

                // attach event when audio's metadata is loaded
                scope.sound.addEventListener('loadedmetadata', function () {
                    // add class
                    element.addClass("play-sentence");
                    // set duration
                    scope.sound.currentTime = start;
                    // play audio
                    scope.sound.play();
                    // resource blocked
                    $rootScope.lockRessource = true;
                });

                // attach event when audio's status is set to pause
                scope.sound.addEventListener('pause', function () {
                    // seek to 0
                    scope.sound.currentTime = 0;
                    // remove class 
                    element.removeClass("play-sentence");
                    // release resource
                    $rootScope.lockRessource = false;
                });

                // attach event when audio's currentTime is updated
                scope.sound.addEventListener('timeupdate', function () {

                    // pause if currentTime equals end
                    if (scope.sound.currentTime > end)
                    {
                        scope.sound.pause();
                    }
                      
                });

            });
          
        }
    };
}]);