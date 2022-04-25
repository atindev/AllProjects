$(function () {
    if ($('.notification-sending').length > 0) {
        const notificationId = $("#notificaitionId").val();
        let intervalId = setInterval(() => {
            showLoader();
            var ajax = new ej.base.Ajax(`/WAS/Notification/GetNotificationById/${notificationId}`, 'GET', true);
            ajax.send().then(function (result) {

                $("#accordion").html('');
                var fragment = document.createElement('div');
                fragment.innerHTML = result;

                ej.base.append([fragment], document.getElementById('accordion'), true);
                updateTimezone($('.timeSpanConvert'));
                hideLoader();
                stopTimer(intervalId);
            });
        }, 15000);
    }
});


function stopTimer(intervalId) {

    var isRefresh = true;
    var $report_tab = $('.report-tab');
    var tabLength = $report_tab.length;
    let tabsToRefresh = tabLength;

    $report_tab.each(function (index) {
        if ($(this).data('tab') == 'email') {
            tabsToRefresh--;
        }
    });

    if (tabLength == 1 && $report_tab.data('data') == 'email') {
        isRefresh = false;
    }
    else if ($('.delivery-report').length == tabsToRefresh) {
        isRefresh = false;
    }

    if (($('.notification-sending').length == 0 && !isRefresh) || $('.notification-failed').length>0 ) {
        clearInterval(intervalId);
    }
}