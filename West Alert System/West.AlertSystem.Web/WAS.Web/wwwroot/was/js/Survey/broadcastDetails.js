$(document).ready(function () {

    getLocationReport();
    getDepartmentReport();
});

function getLocationReport(isManualRefresh) {

    if (isManualRefresh)
        $("#idLocationChart").html("").html(getRefreshLoader());

    $.ajax({
        url: "/was/Survey/GetSubmissionReportByLocation",
        type: "POST",
        data: {
            Id: "" + ($("#idBroadcastId").val())
        },
        success: function (d) {
            if (d != undefined && d != null) {
                $("#idLocationChart").html("");
                $("#idLocationChart").html(d);
            }
        },
        error: function (errorMessage) {
        }
    });
}

function getDepartmentReport(isManualRefresh) {

    if (isManualRefresh)
        $("#idDepartmentChart").html("").html(getRefreshLoader());

    $.ajax({
        url: "/was/Survey/GetSubmissionReportByDepartment",
        type: "POST",
        data: {
            Id: "" + ($("#idBroadcastId").val())
        },
        success: function (d) {
            if (d != undefined && d != null) {
                $("#idDepartmentChart").html("");
                $("#idDepartmentChart").html(d);
            }
        },
        error: function (errorMessage) {
        }
    });
}