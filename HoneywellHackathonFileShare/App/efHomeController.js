fileShareApp.controller('efHomeController', ['$scope', '$location', '$http', function ($scope, $location, $http) {


    hub = $.connection.DocumentHub;
    $scope.connectionEstablished = '';
    //connecting the client to the signalr hub
    $.connection.hub.start().done(function () {
        $scope.connectionEstablished = "Connected to Signalr Server";
    }).fail(function () {
        $scope.connectionEstablished = "failed in connecting to the signalr server";
    });

    // Declare a proxy to reference the hub.
    var documentHubSignalR = $.connection.DocumentHub;
    var connPromise = $.connection.hub.start();

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
                            documentHubSignalR.server.send(resp.room, resp.fileName);

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

    $scope.Download = function () {

    };

}]);

documentHubSignalR.client.notifyFileShare = function (path) {
    $("#notifications").append("<div><a href='" + path +"'>New File Available</div>");
}

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