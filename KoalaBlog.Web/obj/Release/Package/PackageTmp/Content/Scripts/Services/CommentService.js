app.factory("CommentService", ["AjaxService", function (AjaxService) {
    return {
        addComment: function (postData, successFunction, errorFunction, failedFunction) {
            AjaxService.AjaxPost(postData, '../Comment/AddComment', successFunction, errorFunction, failedFunction);
        },
        getComments: function (getParam, successFunction, errorFunction, failedFunction) {
            AjaxService.AjaxGetWithData(getParam, "../Comment/GetComments", successFunction, errorFunction, failedFunction);
        },
        like: function (postData, successFunction, errorFunction, failedFunction) {
            AjaxService.AjaxPost(postData, "../Comment/Like", successFunction, errorFunction, failedFunction);
        }
    }
}]);