
$(document).ready(function () {
    $('.select2').select2();

    //Initialize Select2 Elements
    $('.select2bs4').select2({
        theme: 'bootstrap4'
    })

    $('#idSurveyIdForDelete').click(function (e) {
        showLoader();
    });

    $('#idStartDate').val("");
    $('#idEndDate').val("");

    $('#idResetSearch').click(function (e) {
        $("#idStartDate,#idEndDate").val("");
        $("#idStartDate,#idEndDate").attr("value", "");
        $("#idFilterByBroadcastName").val("");
        $("#idFilterByStatus").val("-1");
        getPagedDataForBroadcast();
    });

    $('#idStartDate').change(function (event, source) {

        let nextDate = event.currentTarget.value;

        $('#idEndDate').attr({
            'min': nextDate
        });
        $('#idEndDate').val("");
        getPagedDataForBroadcast();
    });

    $('#idEndDate').change(function (event, source) {
        getPagedDataForBroadcast();
    });

    $('#idSelectADPeople').select2({
        minimumInputLength: 3,
        ajax: {
            url: '/was/Graph/GetADUsers',
            data: function (params) {
                var queryParameters = {
                    searchString: params.term,
                }
                return queryParameters;
            },
            processResults: function (data) {
                return {
                    results: $.map(data, function (item) {
                        return {
                            text: item.FullName,
                            id: item.UserPrincipalName,
                            value: item.UserPrincipalName
                        }
                    })
                };
            }
        }
    });

    $('#idFilterByStatus').change(function (event) {
        getPagedDataForBroadcast();
    });
});

function updateUserDetails() {
    populateUserDetailsforGrid($(".ad-user-grid"));
    populateUserPicturesforGrid($(".img-circle-grid"));
    bindEmptylist("GridSurveys", "#custom-content-below-home-Surveys");
}

function updateBroadcastUserDetails() {
    hideLoader();
    $("#GridBroadcastedSurveys .e-spinner-pane").hide();
    populateUserDetailsforGrid($(".ad-user-grid"));
    populateUserPicturesforGrid($(".img-circle-grid"));
    updateTimezone($('.timeSpanConvert'));
}

function bindEmptylist(gridId,perentElement) {
    var grid = document.getElementById(gridId).ej2_instances[0];
    let element = "#" + gridId + " .e-row";
    if (grid == undefined || grid.dataSource == undefined || $(element).length == 0) {
        var emptyContent = '<div class="col-12 bg-white emptyTemplateWas text-center mb-5">' +
            '<img src = "/was/img/survey_images/No_Survey_Available.svg" width = "300" alt = "" />';
          
        if (gridId == "GridSurveys") {
            emptyContent += '<h2>No Surveys Available</h2>' +
                '<p>To create a new survey click on the Create Survey button</p>'+
                '<a href="/WAS/Survey/Create" class="btn" title="Create new survey">Create Survey</a>' +
                '</div >';
        }
        else {
            emptyContent += '<h2>None of the survey broadcasted yet</h2></div >';
        }
        $(perentElement).html("").html(emptyContent);
    }
}

function deleteSurveys(event) {
    let id = $(event).attr("id");
    let name = $(event).attr("surveyname");
    let SurveyType = $(event).attr("surveytype");
    
    if (SurveyType == "Survey") 
        $("#idDeleteSurveyText").text("Are you sure you want to delete " + name + " ?");
    else
        $("#idDeleteSurveyText").text("Are you sure you want to delete the broadcasted survey " + name + " ?");

    $("#idSurveyIdForDelete").val(id);
    $("#idSurveyType").val(SurveyType);
}

function shareSurveys(event) {
    let id = $(event).attr("shareSurveyid");
    $("#idSurveyIdForShare").val(id);
    var selectedPeoples = "";
    $.ajax({
        url: "/was/Survey/GetAllSharedPeopleNamesById",
        type: "GET",
        data: {
            Id: id
        },
        success: function (peopleList) {
            if (peopleList != undefined && peopleList != null) {
                for (var i = 0; i < peopleList.PeopleName.length; i++) {
                    selectedPeoples += "<span class='chipdesign ChipPeopleColor'><i class='fas fa-user'></i>" + peopleList.PeopleName[i] + "</span>";
                }
                if (peopleList.PeopleName.length > 0) {
                    $('#idSharedPeople').show();
                    selectedPeoples += "<br /><br />";
                }
                else {
                    $('#idSharedPeople').hide();
                }
            }
            document.getElementById("sharedNames").innerHTML = selectedPeoples;
         },
        error: function (errorMessage) {
            hideLoader();
        }
    })
    
}

$('#idFilterBySubject').bind("enterKey", function (e) {
    getPagedData();
});

$('#idFilterBySubject').keyup(function (e) {
    if (e.keyCode == 13 || $(this).val() == "") {
        $(this).trigger("enterKey");
    }
});


$('#idFilterByBroadcastName').bind("enterKey", function (e) {
    getPagedDataForBroadcast();
});

$('#idFilterByBroadcastName').keyup(function (e) {
    if (e.keyCode == 13 || $(this).val() == "") {
        $(this).trigger("enterKey");
    }
});

function getPagedData() {

    let nameFilter = "";
    let createdByFilter = "";

    nameFilter = $('#idFilterBySubject').val();
    createdByFilter = document.getElementById('emails').ej2_instances[0].value;

    var grid = document.getElementById('GridSurveys').ej2_instances[0]; // Grid instance
    grid.pageSettings.currentPage = 1;
    grid.query = new ej.data.Query()
        .addParams('NameFilter', nameFilter)
        .addParams('CreatedByFilter', createdByFilter);
}

function getPagedDataForBroadcast() {

    showLoader();

    let NameFilter = "", StatusFilter = "", StartTime = "", EndTime = "", offset = "",createdByFilter = "";;

    if ($("#idFilterByStatus").find("option:selected").val() == -1)
        StatusFilter = "";
    else
        StatusFilter = $("#idFilterByStatus").find("option:selected").val();

    if ($("#idFilterByBroadcastName").val().trim()!="")
        NameFilter = $('#idFilterByBroadcastName').val().trim();

    if ($("#idStartDate").val() != "") {
        StartTime = $("#idStartDate").val();
        offset = moment.tz.zone(moment.tz.guess()).parse(Date.parse(EndTime));
    }

    if ($("#idEndDate").val() != "") {
        EndTime = $("#idEndDate").val();
        offset = moment.tz.zone(moment.tz.guess()).parse(Date.parse(EndTime));
    }
    createdByFilter = document.getElementById('createdemail').ej2_instances[0].value;

    var grid = document.getElementById('GridBroadcastedSurveys').ej2_instances[0]; // Grid instance
    grid.pageSettings.currentPage = 1;
    grid.query = new ej.data.Query()
        .addParams('NameFilter', NameFilter)
        .addParams('StatusFilter', StatusFilter)
        .addParams('StartTime', StartTime)
        .addParams('EndTime', EndTime)
        .addParams('TimeZoneOffset', offset)
        .addParams('CreatedByFilter', createdByFilter);
    
}


function cloneCurrentSurvey(event) {
    let surveyId = $(event).attr("id");
    $("#idSurveyIdForClone").val(surveyId);
}

$(window).on('load', function () {
    if ($('#idTabOption').val() == "broadcast")
        $('#custom-content-below-profile-tab').click();
});

function broadcastGridactionBegin() {
    showLoader();
}