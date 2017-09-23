// Declare a proxy to reference the hub.
var documentHubSignalR = $.connection.documentHub;
var connPromise = $.connection.hub.start();

documentHubSignalR.client.notifyFileShare = function (path) {
    $("#notifications").append("<div><a href='" + path + "'>New File Available</div>");
}

documentHubSignalR.client.notifyMessage = function (message) {
    $("#message").append("<div>" + message + "New File Available</div>");
}

$(document).on("click", "#btnSubmit", function () {
        // Start the connection.
        connPromise.done(function () {

            // Call the Send method on the hub server.
            documentHubSignalR.server.sendMessage($("#txtMessage").val());
        });
    });



fileShareApp.controller('efHomeController', ['$scope', '$location', '$http', function ($scope, $location, $http) {

    $scope.efBrowseFile = "Click to add File";
    $scope.files = [];


    $scope.jsonData = {
        name: "supreeth",
        comments: "files"
    };

    $scope.$on("seletedFile", function (event, args) {
        $scope.$apply(function () {

            $scope.files.push(args.file);
        });
    });

    $scope.Upload = function () {

        var _file = document.getElementById('searchFile');
        if (_file.files.length === 0) {
            return;
        }

        var data = new FormData();
        data.append('selectedFile', _file.files[0]);

        var request = new XMLHttpRequest();

        request.onreadystatechange = function () {
            if (request.readyState === 4) {
                try {

                    var resp = JSON.parse(request.response);
                    if (resp != undefined) {

                        // Start the connection.
                        connPromise.done(function () {

                            // Call the Send method on the hub server.
                            documentHubSignalR.server.shareFile(resp.room, resp.fileName);
                            $("#notifications").append("<div><a href='" + resp.fileName + "'>New File Available</div>");

                        });
                    }
                } catch (e) {
                    var resp = {
                        status: 'error',
                        data: 'Unknown error occurred: [' + request.responseText + ']'
                    };
                }

            }
        };

        request.open('POST', "http://localhost:1782/Home/UploadFile");
        request.send(data);
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