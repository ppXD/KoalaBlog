app.filter('diffdate', function () {
    return function (input) {
        var now = new Date();
        var splitDate = input.split(/[^0-9]/);

        //月份减一。
        var date = new Date(splitDate[0], splitDate[1] - 1, splitDate[2], splitDate[3], splitDate[4], splitDate[5]);

        var nowYear = now.getFullYear(); var dateYear = date.getFullYear();
        var nowMonth = now.getMonth() + 1; var dateMonth = date.getMonth() + 1;
        var nowDate = now.getDate(); var dateDate = date.getDate();
        var nowHours = now.getHours(); var dateHours = date.getHours();
        var nowMinutes = now.getMinutes(); var dateMinutes = date.getMinutes();
        var nowSeconds = now.getSeconds(); var dateSeconds = date.getSeconds();

        if (nowYear - dateYear == 0 && nowMonth - dateMonth == 0 && nowDate - dateDate == 0 && (nowHours - dateHours == 0 || (nowHours - dateHours == 1 && nowMinutes + 60 - dateMinutes < 60))) {
            if (nowHours - dateHours == 1) {
                return nowMinutes + 60 - dateMinutes + "分钟前";
            } else if (nowMinutes - dateMinutes > 0) {
                return nowMinutes - dateMinutes + "分钟前";
            } else {
                if (nowSeconds - dateSeconds >= 0 && nowSeconds - dateSeconds < 10) {
                    return "刚刚";
                } else {
                    return (Math.floor((nowSeconds - dateSeconds) / 10)) * 10 + "秒前";
                }
            }
        } else {
            if (nowDate - dateDate == 0) {
                return "今天 " + dateHours + ":" + (dateMinutes < 10 ? "0" + dateMinutes : dateMinutes);
            } else if (nowYear - dateYear == 0) {
                return dateMonth + "月" + dateDate + "日" + " " + dateHours + ":" + (dateMinutes < 10 ? "0" + dateMinutes : dateMinutes);
            }
            return dateYear + "-" + dateMonth + "-" + dateDate + " " + dateHours + ":" + (dateMinutes < 10 ? "0" + dateMinutes : dateMinutes);
        }
    }
})