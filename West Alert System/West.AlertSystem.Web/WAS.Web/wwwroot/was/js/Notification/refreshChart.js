function RefreshChart(i, container, notificationId, spinner) {
    if (container != '' && notificationId != '') {
        $(spinner).addClass('fa-spin');
        let url = '/was/notification/getlatestdeliverystatus';
        showLoader();
        $.ajax({
            url: url,
            type: 'GET',
            data: { notificationId: notificationId },
            success: function (result) {
                var chart = document.getElementById(container + i).ej2_instances[0];
                if (chart != undefined && chart != null) {
                    if (container == 'textContainer') {
                        chart.series[0].dataSource = result.DeliveryStatusTexts;
                    } else if (container == 'whatsappContainer') {
                        chart.series[0].dataSource = result.DeliveryStatusWhatsApps;
                    } else {
                        chart.series[0].dataSource = result.DeliveryStatusVoices;
                    }
                    chart.refreshSeries();
                    chart.refreshChart();
                }
                $(spinner).removeClass('fa-spin');
                hideLoader();
            },
            error: function (errorMessage) {
                hideLoader();
            }
        });
    }
}

function getfailureReport(event) {
    var grid = document.getElementById("GridFailedNotifications").ej2_instances[0];
    grid.dataSource = [];
    $("#modal-FailedNotification .modal-title").text("");

    if (event.point.x == "Failed" || event.point.x == "Undelivered") {
        showLoader();

        let notificationId = $(event.series.controlParent.element).attr("notificationid");
        let type = $(event.series.controlParent.element).attr("typeof");

        if (notificationId == undefined || type == undefined) {
            hideLoader();
            return false;
        }

        let request = {};
        request["Id"] = notificationId;
        request["PublishingType"] = type;
        request["Status"] = "undelivered";
        if (event.point.x == "Failed")
            request["Status"] = "failed";
        $("#modal-FailedNotification .modal-title").text(type +" Failure Report");

        var ajax = new ej.base.Ajax({
            url: "/was/Notification/GetFailedNotifications",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(request),
        });
        ajax.send().then(function (data) {
            grid.dataSource = [];
            if (data != null && data.length > 0 && JSON.parse(data).FailedNotifications.length > 0) {
                grid.dataSource = JSON.parse(data).FailedNotifications;
            }
            $("#idFailedNotificationClick").click();
            hideLoader();

        }).catch(function () {
            grid.dataSource = [];
            hideLoader();
        });

    }
}

$('.text').each(function () {
    var $this = $(this);
    var t = $this.text();
    $this.html(t.replace('&lt', '<').replace('&gt', '>'));
});