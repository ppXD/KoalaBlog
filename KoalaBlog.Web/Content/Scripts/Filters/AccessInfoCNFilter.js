app.filter('accessInfoCN', function () {
    return function (input) {
        if (input == 0) {
            return "仅自己可见";
        } else if (input == 1) {
            return "好友圈";
        } else if (input == 2) {
            return "公开";
        } else if (input == 3) {
            return "群可见";
        }
    }
});