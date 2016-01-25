app.factory("ContentService", ["AjaxService", '$http', function (AjaxService, $http) {
    return {        
        uploadImage: function (postData, successFunction, errorFunction, failedFunction) {
            var formData = new FormData();
            //Take the first selected file
            formData.append("image", postData[0]);

            $http.post("../Content/UploadImage", formData, {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).success(function (response) {
                successFunction(response);
            }).error(function () {
                errorFunction(response);
            });
        },
        deleteImage: function (getParam, successFunction, errorFunction, failedFunction) {
            AjaxService.AjaxGetWithData(getParam, "../Content/DeleteImage", successFunction, errorFunction, failedFunction);
        },
        getOwnContents: function (getParam, successFunction, errorFunction, failedFunction) {
            AjaxService.AjaxGetWithData(getParam, "../Content/GetOwnContents", successFunction, errorFunction, failedFunction);
        }
    }
}]);