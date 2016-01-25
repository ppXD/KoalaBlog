//function performAjaxResult(response, successFunction, failedFunction) {
//    if (!response.result.IsSucceed && response.result.IsException) {
//        showAjaxExceptionMsg(response.result);
//        return;
//    }
//    if (response.result.IsSucceed) {
//        if (successFunction) {
//            successFunction(response, status);
//        } else {
//            showAjaxSuccessMsg(response.result);
//        }
//    } else if (failedFunction) {
//        failedFunction(response, status);
//    } else {
//        showAjaxFailedMsg(response.result);
//    }
//}

function performAjaxResult(response, successFunction, failedFunction) {
    if (successFunction) {
        successFunction(response, status);
    } else {
        showAjaxSuccessMsg(response);
    }
}

function showAjaxSuccessMsg(result) {
    clearAllMsg();
    $('body').animate({ scrollTop: 0 }, 200);
    var successDiv = $('<div class="successMsg alert alert-success alert-dismissible" role="alert"><button type="button" class="close" data-dismiss="alert"><span aria-hidden="true">&times;</span><span class="sr-only">关闭</span></button><strong class="title"></strong><pre class="detail" style="display:none"></pre></div>');
    successDiv.find(".title").text(result.Title ? result.Title : "操作成功");
    if (result.Detail) {
        successDiv.find(".detail").show().text(result.Detail);
    }
    $(document.body).prepend(successDiv);
    setTimeout(function () {
        successDiv.slideUp(300);
    }, 2000);
}

function showAjaxFailedMsg(result) {
    clearAllMsg();
    $('body').animate({ scrollTop: 0 }, 200);
    var failedDiv = $('<div class="failedMsg alert alert-warning alert-dismissible" role="alert"><button type="button" class="close" data-dismiss="alert"><span aria-hidden="true">&times;</span><span class="sr-only">关闭</span></button><strong class="title"></strong><pre class="detail" style="display:none"></pre></div>');
    if (result.Title) {
        failedDiv.find(".title").text(result.Title);
    }
    if (result.Detail) {
        failedDiv.find(".detail").show().text(result.Detail);
    }
    $(document.body).prepend(failedDiv);
}

function showAjaxErrorMsg(result) {
    if (result.Detail == "") {
        return;
    }
    clearAllMsg();
    $('body').animate({ scrollTop: 0 }, 200);
    var errorDiv = $('<div class="ui negative message"><i class="close icon"></i><div class="header"></div><p class="detail"></p></div>');
    if (result.Detail) {
        errorDiv.find(".header").text(result.Title);
        errorDiv.find(".detail").text(result.Detail);
    }

    $(document.body).prepend(errorDiv);
}

function clearAllMsg() {
    $(".ui .negative .message").remove();
}