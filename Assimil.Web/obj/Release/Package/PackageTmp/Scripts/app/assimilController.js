
/* assimil Controller */

angular.module('assimilMod', []).controller('AssimilCtrl', ['$scope', '$http', '$timeout', function ($scope, $http, $timeout) {

    $scope.loading = true;
    $scope.sound = null;
    $scope.pageSizes = [
        { pageSize: '5' },
        { pageSize: '10' },
        { pageSize: '20' },
        { pageSize: '30' }];

    // default items selected by page
    $scope.selectpageSize = $scope.pageSizes[0];

    // variable
    $scope.numTotalOfPage = 0;

    $scope.currentPage = 0;
    $scope.totalItems = 0;
    $scope.pageDimension = 0;
    $scope.ind = 0;

    $scope.call = function (func) {

        var data = { page: ($scope.currentPage + 1), itemsByPage: $scope.selectpageSize.pageSize };

        $http({ method: 'POST', url: '/Assimil/GetAssimils', data: data }).success(function (result) {
            $scope.assimils = result.Data;
            $scope.totalItems = result.NumItems;
            $scope.currentPage = result.CurrentP - 1;
            $scope.pageDimension = result.ItemsByPage;
            $scope.searchIndex();
            $scope.selectpageSize = $scope.pageSizes[$scope.ind];
            $scope.groupToPages();
            $scope.prev = ($scope.currentPage * $scope.selectpageSize.pageSize) + 1;
            $scope.succ = $scope.currentPage == ($scope.numTotalOfPage - 1) ? $scope.totalItems : (($scope.currentPage + 1) * $scope.selectpageSize.pageSize);


            $timeout(function () {
                // run action
                if (func) func();
            }, 3000);

        });
    };

    $scope.call(function () {
        $scope.attachEvent();
        $scope.loading = false;
        $('.body-content-loading-overlay').attr("style", "display:none");
    });

    // calculate num page 
    $scope.groupToPages = function () {
        $scope.numTotalOfPage = Math.ceil($scope.totalItems / $scope.selectpageSize.pageSize);
    };

    $scope.prevPage = function () {
        if ($scope.currentPage > 0) {
            $scope.loading = true;
            if ($('.track-list').find('.playback-button').hasClass('playing')) {
                $('.track-list').find('.playing').removeClass('playing').addClass('idle');
                $scope.sound.pause();
                $scope.sound.currentTime = 0;
            }
            $('.body-content-loading-overlay').attr("style", "");
            $scope.currentPage--;
            $scope.call(function () {
                $scope.attachEvent();
                $scope.loading = false;
                $('.body-content-loading-overlay').attr("style", "display:none");
            });
        }
    };

    $scope.nextPage = function () {
        if ($scope.currentPage < $scope.numTotalOfPage - 1) {
            $scope.loading = true;
            if ($('.track-list').find('.playback-button').hasClass('playing')) {
                $('.track-list').find('.playing').removeClass('playing').addClass('idle');
                $scope.sound.pause();
                $scope.sound.currentTime = 0;
            }
            $('.body-content-loading-overlay').attr("style", "");
            $scope.currentPage++;
            $scope.call(function () {
                $scope.attachEvent();
                $scope.loading = false;
                $('.body-content-loading-overlay').attr("style", "display:none");
            });
        }
    };

    $scope.selectPage = function () {
        $scope.loading = true;
        if ($('.track-list').find('.playback-button').hasClass('playing')) {
            $('.track-list').find('.playing').removeClass('playing').addClass('idle');
            $scope.sound.pause();
            $scope.sound.currentTime = 0;
        }
        $('.body-content-loading-overlay').attr("style", "");
        $scope.call(function () {
            $scope.attachEvent();
            $scope.loading = false;
            $('.body-content-loading-overlay').attr("style", "display:none");
        });
    };

    $scope.searchIndex = function () {
        for (var i = 0; i < $scope.pageSizes.length; i++) {
            if ($scope.pageSizes[i].pageSize == $scope.pageDimension) {
                $scope.ind = i;
                break;
            }
        }
    };

    $scope.attachEvent = function () {

        $('tr.track-list-row').click(function () {

            if ($(this).hasClass('idle')) {
                if ($('.track-list').find('.loading').hasClass('loading')) {
                    return;
                }

                if ($('.track-list').find('.playback-button').hasClass('playing')) {
                    $('.track-list').find('.playing').removeClass('playing').addClass('idle');
                    // stop play
                    $scope.sound.pause();
                    $scope.sound.currentTime = 0;
                }

                // adding loading class
                $(this).removeClass('idle').addClass('loading');
                $(this).find('.playback-button').removeClass('idle').addClass('loading');

                var obj = $(this);
                $timeout(function () {
                    // run action
                    $scope.play(obj);
                }, 2000);
            }
            else if ($(this).hasClass('loading')) {
                // do nothing
            }
            else {
                $(this).removeClass('playing').addClass('idle');
                $(this).find('.playback-button').removeClass('playing').addClass('idle');
                $scope.sound.pause();
                $scope.sound.currentTime = 0;
            }

        });
    };

    $scope.play = function (obj) {

        // get audio link
        var audioUrl = obj.find('.playback-button').attr('data-track-docid');

        // Create the new instance of audio
        $scope.sound = new Audio();

        // set audio.src
        $scope.sound.src = audioUrl;

        $scope.sound.addEventListener('loadedmetadata', function () {
            // play audio
            $scope.sound.play();
        });

        // attach event when audio is ready to play
        $scope.sound.addEventListener('playing', function () {
            obj.removeClass('loading').addClass('playing');
            obj.find('.playback-button').removeClass('loading').addClass('playing');
        });

        // attach event when audio has finished to play
        $scope.sound.addEventListener('ended', function () {
            $('.track-list').find('.playing').removeClass('playing').addClass('idle');
        });
    };

}]);









