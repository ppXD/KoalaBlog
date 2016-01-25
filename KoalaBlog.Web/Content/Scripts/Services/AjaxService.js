app.config(['$httpProvider', function ($httpProvider) {

    // Initialize get if not there
    if (!$httpProvider.defaults.headers.get) {
        $httpProvider.defaults.headers.get = {};
    }

    // Enables Request.IsAjaxRequest() in ASP.NET MVC
    $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';

    // Disable IE ajax request caching
    $httpProvider.defaults.headers.get['Cache-Control'] = 'no-cache';
    $httpProvider.defaults.headers.get['Pragma'] = 'no-cache';

}]).factory('AjaxService', ['$http', function ($http) {
    return {
        AjaxPost: function (data, route, successFunction, errorFunction, failedFunction) {
            $http.post(route, data).success(function (response, status, headers, config) {
                performAjaxResult(response, successFunction, failedFunction);
            }).error(function (response) {
                if (errorFunction) {
                    errorFunction(response);
                } else {
                    showAjaxErrorMsg(response);
                }
            });
        },

        AjaxGet: function (route, successFunction, errorFunction, failedFunction) {
            $http({ method: 'GET', url: route }).success(function (response, status, headers, config) {
                performAjaxResult(response, successFunction, failedFunction);
            }).error(function (response) {
                if (errorFunction) {
                    errorFunction(response);
                } else {
                    showAjaxErrorMsg(response);
                }
            });
        },

        AjaxGetWithData: function (data, route, successFunction, errorFunction, failedFunction) {
            $http({ method: 'GET', url: route, params: data }).success(function (response, status, headers, config) {
                performAjaxResult(response, successFunction, failedFunction);
            }).error(function (response) {
                if (errorFunction) {
                    errorFunction(response);
                } else {
                    showAjaxErrorMsg(response);
                }
            });
        }
    };
}]);