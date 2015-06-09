(function () {
    'use strict';
    angular
        .module('fileUpload')
        .controller('MyCtrl', MyCtrl);

    MyCtrl.$inject = ['$location', '$upload'];
    $('#loading').hide();
    function MyCtrl($location, $upload) {
        alert('no')

        /* jshint validthis:true */
        var vm = this;
        vm.title = 'MyCtrl';
        vm.uploadPic = function (files) {
            $scope.formUpload = true;
            if (files != null) {
                var file = files[0];
                $scope.generateThumb(file);
                $scope.errorMsg = null;
                eval($scope.uploadScript);
            }
        }
        vm.onFileSelect = function ($files, user) {
            //$files: an array of files selected, each file has name, size, and type.
            for (var i = 0; i < $files.length; i++) {
                var file = $files[i];
                vm.upload = $upload.upload({
                    url: 'Uploads/UploadHandler.ashx',
                    data: { name: user.Name },
                    file: file, // or list of files ($files) for html5 only
                }).progress(function (evt) {
                    //console.log('percent: ' + parseInt(100.0 * evt.loaded / evt.total));
                }).success(function (data, status, headers, config) {
                    alert('Uploaded successfully ' + file.name);
                }).error(function (err) {
                    alert('Error occured during upload');
                });
            }
        };
    }
})();
