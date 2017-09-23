fileShareApp.controller('efHomeController', ['$scope', '$location','$http', function ($scope, $location,$http) {

    $scope.efBrowseFile = "Click to add File";


    $scope.files = [];

    //2. a simple model that want to pass to Web API along with selected files  
    $scope.jsonData = {
        name: "supreeth",
        comments: "files"
    };
    //3. listen for the file selected event which is raised from directive  
    $scope.$on("seletedFile", function (event, args) {
        $scope.$apply(function () {
            //add the file object to the scope's files collection  
            $scope.files.push(args.file);
        });
    });

    $scope.Upload = function (data) {
        var data = document.getElementById('searchFile');
        var selectedFile = data.files[0];

        $http({
            method: 'POST',
            url: "http://localhost:1782/api/FileManager/UploadFile",
            headers: { 'Content-Type': application/octet-stream},

            transformRequest: function (data) {
                var formData = new FormData();
                formData.append("model", angular.toJson(data.model));
                for (var i = 0; i < data.files.length; i++) {
                    formData.append("file" + i, data.files[i]);
                }
                return formData;
            },
            data: { model: $scope.jsonData, files: $scope.files }
        }).then(function (response) {
                var data = response;
                // This function handles success
            }, function (response) {
                var data = response;
                // this function handles error
            });
    }

}]);

fileShareApp.directive('uploadFiles', function () {
    return {
        scope: true,        //create a new scope  
        link: function (scope, el, attrs) {
            el.bind('change', function (event) {
                var files = event.target.files;
                //iterate files since 'multiple' may be specified on the element  
                for (var i = 0; i < files.length; i++) {
                    //emit event upward  
                    scope.$emit("seletedFile", { file: files[i] });
                }
            });
        }
    };
});