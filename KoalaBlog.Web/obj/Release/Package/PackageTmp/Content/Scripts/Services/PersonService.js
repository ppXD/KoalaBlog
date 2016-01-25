app.factory("PersonService", ["AjaxService", function (AjaxService) {
    return {
        getPersonInfo: function (successFunction, errorFunction, failedFunction) {
            AjaxService.AjaxGet("../Person/GetPersonInfo", successFunction, errorFunction, failedFunction);
        }
    }
}]);