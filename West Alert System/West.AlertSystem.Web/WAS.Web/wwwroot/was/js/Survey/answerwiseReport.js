$(document).ready(function () {
    getBasicDetails();
    getAnswerwiseData();
    getShortAnswerData();
});
 
function getBasicDetails() {
    $.ajax({
        url: "/was/Survey/GetBasicBroadcastDetails",
        type: "POST",
        data: {
            Id: "" + ($("#idBroadcastId").val())
        },
        success: function (d) {
            if (d != undefined && d != null) {
                $("#idBasicDetails").html("");
                $("#idBasicDetails").html(d);
            }
            updateTimezone($('.timeSpanConvert'));

            //calling the timer function to refresh timeline info;
            refreshTimeline();
        },
        error: function (errorMessage) {
        }
    });
}
 
function refreshTimeline() {
    let timerId = null;
    if ($('.survey-sent').length == 0) {

        $(".sentdate-continuous-refresh").show();
        $(".totalaudience-continuous-refresh").show();
        timerId = setInterval(() =>
        {
            console.log("timer called");
            getBroadcastTimlineDetails(timerId);
        }, 15000);
    }
}

function getBroadcastTimlineDetails(timerId) {

    $.ajax({
        url: "/was/Survey/GetBroadcastDetailsToRefresh",
        type: "POST",
        data: {
            broadcastId: "" + ($("#idBroadcastId").val())
        },
        success: function (d) {
            if (d != undefined && d != null) {
                $(".broadcast-timeline-div").html("");
                $(".broadcast-timeline-div").html(d);
                
                if ($('.survey-sent').length == 0) {
                    $(".sentdate-continuous-refresh").show();
                    $(".totalaudience-continuous-refresh").show();
                 }
            }

            updateTimezone($('.timeSpanConvert'));

            if ($('.survey-sent').length > 0) {
                //updating the audience count
                const audCount = $("#totalAudiancehid").val();
                if (audCount != 0 && audCount != undefined && audCount != null)
                {
                    $("#audience-count").empty().html(`<span class="CountTitle">${audCount}</span>`)
                }

                $(".sentdate-continuous-refresh").hide();
                $(".totalaudience-continuous-refresh").hide();
                clearInterval(timerId);
            }
                
        },
        error: function (errorMessage) {
            
            $(".sentdate-continuous-refresh").hide();
            $(".totalaudience-continuous-refresh").hide();
        }
    });
}

function getAnswerwiseData(isManualRefresh) {

    $("#custom-content-below-home-tab-Surveys").click();

    if (isManualRefresh!=undefined && isManualRefresh)
        $("#idAnswerDetails").show().html("").html(getRefreshLoader());

    $.ajax({
        url: "/was/Survey/GetAnswerwiseData",
        type: "POST",
        data: {
            Id: "" + ($("#idBroadcastId").val())
        },
        success: function (d) {
            if (d != undefined && d != null) {
                $("#idAnswerDetails").html("");
                    $("#idAnswerDetails").html(d);
                    updateQuestionNumber();

                    //report is empty
                    if ($(".classEmptyOptionQuestions").length > 0) {
                        if ($("#idShortAnswerDetails .classSubLoader").length > 0)
                            $("#idAnswerDetails").hide();
                    }
              }
        },
        error: function (errorMessage) {
        }
    });
}

function manualRefresh() {
    manualRefreshBasicInfo();
    getAnswerwiseData(true);
    getLocationReport(true);
    getDepartmentReport(true);
    getShortAnswerData(true);
}

function manualRefreshBasicInfo() {

    $("#idBasicbroadcastInfo").html("").html(getRefreshLoader());

    //hiding the continuous refresh for sent date and total audience during the manual refresh.
    $(".sentdate-continuous-refresh").hide();
    $(".totalaudience-continuous-refresh").hide();

    $.ajax({
        url: "/was/Survey/GetBroadcastInfo",
        type: "POST",
        data: {
            broadcastId: "" + ($("#idBroadcastId").val())
        },
        success: function (d) {
            if (d != undefined && d != null) {
                $("#idBasicbroadcastInfo").html("");
                $("#idBasicbroadcastInfo").html(d);
                $("#idSubject").text($("#idSubjectHid").val());

            }
            updateTimezone($('.timeSpanConvert'));
        },
        error: function (errorMessage) {
        }
    });
}

function getShortAnswerData(isManualRefresh) {

    if (isManualRefresh != undefined && isManualRefresh)
        $("#idShortAnswerDetails").show().html("").html(getRefreshLoader());

    $.ajax({
        url: "/was/Survey/GetShortAnswerAnalysis",
        type: "POST",
        data: {
            Id: "" + ($("#idBroadcastId").val())
        },
        success: function (d) {
            if (d != undefined && d != null) {
                $("#idShortAnswerDetails").html("");
                    $("#idShortAnswerDetails").html(d);
                    updateQuestionNumber();

                    //report is empty
                    if ($(".classEmptyShortAnswerQuestions").length > 0) {
                        if ($("#idAnswerDetails .classSubLoader").length > 0)
                            $("#idShortAnswerDetails").hide();
                        else
                            $("#idShortAnswerDetails").show();
                    }
            }
        },
        error: function (errorMessage) {
        }
    });
}

function getRefreshLoader() {
    let list = "";
    list += `<div class="col-12 classSubLoader"><div class="card classNoBoxShadow"><div class="steps-img classAlignCenter AlignSpinnerFlex"><img src="/was/img/ajax_loader_was.svg" alt="loading image" /></div><br /></div></div>`;
    return list;
}

function updateQuestionNumber() {
    let value = 0;
    $(".classQuestionNumber").each(function (index, element) {
        if ($(element) != undefined && $(element) != null) {
            value = index + 1;
            $(element).text(value);
        }
    });
}