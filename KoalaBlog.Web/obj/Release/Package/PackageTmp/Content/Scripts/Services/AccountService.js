app.factory("AccountService", ["AjaxService", function (AjaxService) {
    return {
        Login: function (postData, successFunction, errorFunction, failedFunction) {
            AjaxService.AjaxPost(postData, '../Account/Login', successFunction, errorFunction, failedFunction);
        },
        Register: function (postData, successFunction, errorFunction, failedFunction) {
            AjaxService.AjaxPost(postData, '../Account/Register', successFunction, errorFunction, failedFunction);
        },
        LogOff: function (successFunction, errorFunction, failedFunction) {
            AjaxService.AjaxGet("../Account/LogOff", successFunction, errorFunction, failedFunction);
        }
    }
}]);