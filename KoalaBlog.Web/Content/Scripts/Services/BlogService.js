app.factory("BlogService", ["AjaxService", function (AjaxService) {
    return {
        createBlog: function (postData, successFunction, errorFunction, failedFunction) {
            AjaxService.AjaxPost(postData, '../Blog/CreateBlog', successFunction, errorFunction, failedFunction);
        },
        getBlogs: function (getParam, successFunction, errorFunction, failedFunction) {
            AjaxService.AjaxGetWithData(getParam, "../Blog/GetBlogs", successFunction, errorFunction, failedFunction);
        },
        getOwnBlogs: function (getParam, successFunction, errorFunction, failedFunction) {
            AjaxService.AjaxGetWithData(getParam, "../Blog/GetOwnBlogs", successFunction, errorFunction, failedFunction);
        },
        like: function (postData, successFunction, errorFunction, failedFunction) {
            AjaxService.AjaxPost(postData, "../Blog/Like", successFunction, errorFunction, failedFunction);
        },
        collect: function (postData, successFunction, errorFunction, failedFunction) {
            AjaxService.AjaxPost(postData, "../Blog/Collect", successFunction, errorFunction, failedFunction);
        }
    }
}]);