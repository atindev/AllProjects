
let groupElementArrayMapping = [{ id: "#TextCheckbox", element: ".classSMSSubscribers" },
{ id: "#EmailCheckbox", element: ".classEmailSubscribers" },
{ id: "#WhatsappCheckbox", element: ".classWhatsAppSubscribers" }];

$(document).ready(function () {
    sessionStorage.setItem("currentModule", "");
    $('.classGroupCount').hide();
    clearGroupsDropdown();
    LoadTimeZones();
    setStartAndEndDateByDefault();

    let isUpdate = $("#idIsUpdate").val();
    if (isUpdate != undefined && isUpdate != "" && isUpdate.toLocaleLowerCase() == "true")
        getBroadcastSurveyDetails();

    //Initialize Select2 Elements
    $('.select2').select2();

    //Initialize Select2 Elements
    $('.select2bs4').select2({
        theme: 'bootstrap4'
    });

    $('#idRefreshNotification').click(function (e) {
        showLoader();
        clearAllSelections();
        hideLoader();
    });
    $('#idResetFollowupTime').click(function (e) {
        $("#idFollowUpDate").val("");
    });

    $('#idAlertForGroupnavigation').click(function (e) {
        $("#idAlertPopupContent").text("Survey data added above will be discarded");
    });

    $('#idAlertConfirm').click(function (e) {
        window.location.href = '/was/group/List?defaultTab=People';
    });

    $('.classNotificationType').click(function () {
        hideGroupCountElements();
        hideShowDGADUserDropdown();
    });

    $('#idSelectPeople').change(function (event, source) {

        if (source != undefined && source == "forUpdate")
            return false;

        let totalCount = 0, emailCount = 0, smsCount = 0, voiceCount = 0, whatsappCount = 0;


        setGroupCount(totalCount, smsCount, emailCount, voiceCount, whatsappCount);

        if ($(this).find("option:selected").length == 0) {
            $('.classGroupCount').hide();
            return false;
        }
        let peopleList = {};
        peopleList["Ids"] = $("#idSelectGroups").val();
        peopleList["IgnoreQueryFilters"] = true;
        showLoader();
        peopleList["SubscriptionIds"] = $("#idSelectPeople").val();

        GetPeopleAndGroupCount(peopleList);
    });

    $('#idSelectGroups').change(function (event, source) {

        if (source != undefined && source == "forUpdate")
            return false;

        let totalCount = 0, emailCount = 0, smsCount = 0, voiceCount = 0, whatsappCount = 0;

        setGroupCount(totalCount, smsCount, emailCount, voiceCount, whatsappCount);

        if ($(this).find("option:selected").length == 0) {
            $('.classGroupCount').hide();
            return false;
        }
        let peopleList = {};
        peopleList["Ids"] = $("#idSelectGroups").val();
        peopleList["IgnoreQueryFilters"] = true;
        showLoader();
        peopleList["SubscriptionIds"] = $("#idSelectPeople").val();

        GetPeopleAndGroupCount(peopleList);
    });

    function ReviewNotification() {
        $('#modal-send').modal('show');
    }

    $('#SendResponse').click(function (e) {
        $(".classforApproval").trigger('click', 'showpopup');
    });

    $('.classPublishselectCheck').click(function (e, source) {

        if (source != undefined && source == "showpopup") {
            return true;
        }
        else {
            if (!checkPublishSelection()) {
                showEmptyTextAlert("Select atleast one publishing Type");
                return false;
            }
            if (!checkUserGroupSelection()) {
                return false;
            }
            if (!checkForFollowupDate()) {
                return false;
            }

            //for confirmation popup
            var groupdropdown = document.getElementById("idSelectGroups");
            var peopledropdown = document.getElementById("idSelectPeople");
            var selectedGroups = "";
            var selectedPeoples = "";
            for (var i = 0; i < groupdropdown.length; i++) {
                if (groupdropdown[i].selected) {
                    selectedGroups += "<span class='chipdesign'><i class='fas fa-users'></i>" + groupdropdown[i].text + "</span>";
                }
            }
            for (var i = 0; i < peopledropdown.length; i++) {
                if (peopledropdown[i].selected) {
                    selectedPeoples += "<span class='chipdesign ChipPeopleColor'><i class='fas fa-user'></i>" + peopledropdown[i].text + "</span>";
                }
            }
            var groups = selectedGroups.slice(0, -2);
            var peoples = selectedPeoples.slice(0, -2);
            var peopleModal = document.getElementById("Modal-PeopleName");
            var groupModal = document.getElementById("Modal-GroupName");

            peopleModal.innerHTML = groups;
            groupModal.innerHTML = peoples;

            if ($("#EmailCheckbox").prop("checked")) {
                var dGroupdropdown = document.getElementById("idSelectDGroups");
                var adPeopledropdown = document.getElementById("idSelectADPeople");
                var selectedDGroups = "";
                var selectedADPeoples = "";
                for (var i = 0; i < dGroupdropdown.length; i++) {
                    if (dGroupdropdown[i].selected) {
                        selectedDGroups += "<span class='chipdesign'><i class='fas fa-users'></i>" + dGroupdropdown[i].text + "</span>";
                    }
                }
                for (var i = 0; i < adPeopledropdown.length; i++) {
                    if (adPeopledropdown[i].selected) {
                        selectedADPeoples += "<span class='chipdesign ChipPeopleColor'><i class='fas fa-user'></i>" + adPeopledropdown[i].text + "</span>";
                    }
                }

                var dGroups = selectedDGroups.slice(0, -2);
                var adPeople = selectedADPeoples.slice(0, -2);
                var adPeopleModal = document.getElementById("Modal-ADPeopleName");
                var dGroupModal = document.getElementById("Modal-DGroupName");
                dGroupModal.innerHTML = dGroups;
                adPeopleModal.innerHTML = adPeople;

                $("#Modal-ADPeopleName").show();
                $('#Modal-DGroupName').show();
            }
            else {
                $("#Modal-ADPeopleName").hide();
                $('#Modal-DGroupName').hide();
            }

            ReviewNotification();
        }
        return false;
    });

    $('#idStartDate').change(function (event, source) {

        let nextDate = getNextDay(event.currentTarget.value);

        $('#idEndDate').attr({
            'value': nextDate,
            'min': nextDate
        });
        $('#idEndDate').val(nextDate);

        $('#idFollowUpDate').attr({
            'min': nextDate,
            'max': nextDate
        });
        $('#idFollowUpDate').val('');
    });

    $('#idEndDate').change(function (event, source) {
        let nextDate = getNextDay($("#idStartDate").val());

        $('#idFollowUpDate').attr({
            'min': nextDate,
            'max': event.currentTarget.value,
        });
        $('#idFollowUpDate').val('');
    });

    $('#SendResponse').click(function () {
        showLoader();
        let request = {};
        request["SurveyId"] = $("#idSurveyId").val();
        request["StartTime"] = $('#idStartDate').val();
        request["EndTime"] = $('#idEndDate').val();
        if ($('#idFollowUpDate').val() != undefined && $('#idFollowUpDate').val() != "") {
            request["FollowUpTime"] = $('#idFollowUpDate').val();
        }
        request["TimeZone"] = $('#SelectTimeZone').val();
        request["TimeZoneOffset"] = moment.tz.zone($('#SelectTimeZone').val()).parse(Date.parse($('#idStartDate').val()));
        request["BroadcastedTimeZone"] = moment.tz.guess();
        request["SubscriptionId"] = $("#idSelectPeople").val();
        request["GroupId"] = $("#idSelectGroups").val();
        request["IsText"] = $("#TextCheckbox").prop("checked");
        request["IsEmail"] = $("#EmailCheckbox").prop("checked");
        request["IsWhatsApp"] = $("#WhatsappCheckbox").prop("checked");

        var dGroup = [];
        var selecteddGroup = $("#idSelectDGroups").select2('data');
        $.each(selecteddGroup, function (index, data) {
            dGroup.push({
                EmailId: data.emailId,
                Id: data.id,
                Name: data.text
            });
        });
        request["DistributionGroups"] = dGroup;
        request["IsWizard"] = $("#idForWizard").prop("checked");

        var adPeople = [];
        var selectedPeople = $("#idSelectADPeople").select2('data');
        $.each(selectedPeople, function (index, data) {
            adPeople.push({
                EmailId: data.id,
                FirstName: data.firstName,
                LastName: data.lastName,
                Location: data.location,
                Department: data.department
            });
        });
        request["ADPeople"] = adPeople;

        //only for update
        if (isUpdate != undefined && isUpdate != "" && isUpdate.toLocaleLowerCase() == "true")
            request["BroadcastId"] = $("#idBroadcastId").val();

        $.ajax({
            url: "/was/Survey/BroadcastSurvey",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(request),
            success: function (d) {
                window.location.href = `/WAS/Survey/GetAnswerwiseReport?id=${d.Id}`;
                hideLoader();
            },
            error: function (errorMessage) {
                hideLoader();
            }
        });
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
                            text: item.LastName + ", " + item.FirstName,
                            id: item.UserPrincipalName,
                            department: item.Department,
                            location: item.Location,
                            firstName: item.FirstName,
                            lastName: item.LastName
                        }
                    })
                };
            }
        }
    });

    $('#idSelectDGroups').select2({
        minimumInputLength: 3,
        ajax: {
            url: '/was/Graph/GetDistributionLists',
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
                            text: item.Name,
                            id: item.Id,
                            emailId: item.EmailId
                        }
                    })
                };
            }
        }
    });
});

$(window).on('load', function () {
    setTimeout(function () { scrollTop(); }, 500);
});

function clearGroupsDropdown() {
    $('#idSelectGroups').val("");
    $('#idselectForGroups .select2-selection__choice__remove').each(function (index, element) {
        element.click();
    })
}

function clearPeopleDropdown() {
    $('#idSelectPeople').val("");
    $('#idselectForPeople .select2-selection__choice__remove').each(function (index, element) {
        element.click();
    })
}

function clearDGroupsDropdown() {
    $('#idSelectDGroups').val(null).trigger('change');
}

function clearADPeopleDropdown() {
    $('#idSelectADPeople').val(null).trigger('change');
}

function clearAllSelections() {

    //group selection
    clearGroupsDropdown();
    $('.classGroupCount').hide();

    //people selection
    clearPeopleDropdown();

    // AD people selection
    clearADPeopleDropdown();

    //Distribution group selection
    clearDGroupsDropdown();

    //publishing Types
    if ($("#TextCheckbox").prop("checked")) {
        $("#TextCheckbox").prop('checked', false);
    }
    if ($("#EmailCheckbox").prop("checked")) {
        $("#EmailCheckbox").prop('checked', false);
    }
    if ($("#WhatsappCheckbox").prop("checked")) {
        $("#WhatsappCheckbox").prop('checked', false);
    }

    //execution type
    $("#idSinglePage").prop("checked", "true");

    //date and time
    setStartAndEndDateByDefault();
    $('#SelectTimeZone').val(moment.tz.guess());

    //just for hiding opend groups and people tab
    if ($("#idselectForGroups .select2-selection").attr("aria-expanded") == "true")
        $("#idselectForGroups .select2-search__field").click()
    if ($("#idselectForPeople .select2-selection").attr("aria-expanded") == "true")
        $("#idselectForPeople .select2-search__field").click();
    scrollTop();
}

function scrollTop() {
    window.scrollTo(0, 50);
}

function setGroupCount(totalCount, smsCount, emailCount, voiceCount, whatsappCount) {
    $('#idTotalSubscribers span').text(totalCount);
    $('.classSMSSubscribers span').text(smsCount);
    $('.classEmailSubscribers span').text(emailCount);
    $('.classWhatsAppSubscribers span').text(whatsappCount);
}

function hideGroupCountElements() {
    //hiding the count according to voice,text,email selection
    $('.classGroupCount').show();
    groupElementArrayMapping.forEach(function (v, i) {
        if (!$(v.id).prop('checked'))
            $(v.element).hide();
    });
}

function hideShowDGADUserDropdown() {
    if ($("#EmailCheckbox").prop("checked")) {
        $("#idSelectForDGroups").show();
        $('#idSelectForADPeople').show();
    }
    else {
        $("#idSelectForDGroups").hide();
        $('#idSelectForADPeople').hide();
    }
}

function checkPublishSelection() {
    //checking atleast one publishing type is selected
    let flag = false;
    groupElementArrayMapping.forEach(function (v, i) {
        if ($(v.id).prop('checked')) {
            flag = true;
        }
    });
    return flag;
}

function checkUserGroupSelection() {
    if ($('#idSelectGroups').val() == '' && $('#idSelectPeople').val() == '') {
        if ($("#EmailCheckbox").prop("checked")) {
            if ($('#idSelectDGroups').val() == '' && $('#idSelectADPeople').val() == '') {
                showEmptyTextAlert('Select at least one group/people/distribution group/AD people');
                return false;
            }
            else { return true; }
        }
        showEmptyTextAlert('Select at least one group/people');
        return false;
    }
    else { return true; }
}

function LoadTimeZones() {

    var timeZones = moment.tz.names();
    let offsetTmz = [];

    for (let i in timeZones) {
        offsetTmz.push(`(GMT${moment.tz(timeZones[i]).format('Z')}) ${timeZones[i]}`);
    }
    timeZones = offsetTmz.sort();

    for (var i in timeZones) {
        $('#SelectTimeZone').append('<option value="' + timeZones[i].substring(12, timeZones[i].length) + '">' + timeZones[i] + '</option>');
    }
    $('#SelectTimeZone').val(moment.tz.guess());
}

function showEmptyTextAlert(text) {
    $("#idmodalsmText").text(text);
    $('#idAlertToggle').click();
}

function setStartAndEndDateByDefault() {

    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1;
    var yyyy = today.getFullYear();
    var h = today.getHours();
    var m = today.getMinutes();
    var s = today.getSeconds();
    if (dd < 10) {
        dd = '0' + dd
    }
    if (mm < 10) {
        mm = '0' + mm
    }
    if (h < 10) {
        h = '0' + h
    }
    if (m < 10) {
        m = '0' + m
    }
    today = yyyy + '-' + mm + '-' + dd + 'T' + h + ':' + m;

    $('#idStartDate').attr({
        'value': today,
        'min': today
    });
    $('#idStartDate').val(today);

    let nextDate = getNextDay($("#idStartDate").val());
    $('#idEndDate').attr({
        'value': nextDate,
        'min': nextDate
    });
    $("#idEndDate").attr('max', '');
    $('#idEndDate').val(nextDate);

    $("#idFollowUpDate").attr('max', '');
    $('#idFollowUpDate').attr({
        'min': nextDate,
        'max': nextDate
    });
    $("#idFollowUpDate").val("");
}

function getNextDay(date) {

    let nextDate = new Date(date);
    nextDate.setDate(new Date(date).getDate() + 1);

    var dd = nextDate.getDate();
    var mm = nextDate.getMonth() + 1; //January is 0 so need to add 1 to make it 1!
    var yyyy = nextDate.getFullYear();
    var h = nextDate.getHours();
    var m = nextDate.getMinutes();
    var s = nextDate.getSeconds();
    if (dd < 10) {
        dd = '0' + dd
    }
    if (mm < 10) {
        mm = '0' + mm
    }
    if (h < 10) {
        h = '0' + h
    }
    if (m < 10) {
        m = '0' + m
    }
    let today = yyyy + '-' + mm + '-' + dd + 'T' + h + ':' + m;
    return today;
}

function checkForFollowupDate() {
    if ((new Date($('#idFollowUpDate').val()) > new Date($('#idEndDate').val())) || (new Date($('#idFollowUpDate').val()) < new Date($('#idStartDate').val()))) {
        showEmptyTextAlert('Follow up date Should be in between start and end date');
        return false;
    }
    else
        return true;
}

function getBroadcastSurveyDetails() {

    let broadcastId = $("#idBroadcastId").val();

    if (broadcastId == undefined || idBroadcastId == "")
        return false;

    showLoader();
    $.ajax({
        url: "/was/Survey/GetBroadcastById",
        type: "POST",
        data: {
            Id: broadcastId
        },
        success: function (d) {
            if (d != undefined && d != null) {
                setSavedData(d);
            }
            hideLoader();
            if ($(".select2-selection__choice").length > 0 && $(".select2-results").length > 0)
                $(".select2-selection__choice")[0].click();
            scrollTop();
        },
        error: function (errorMessage) {
            hideLoader();
        }
    });
}

function setSavedData(d) {

    clearAllSelections();

    if (d != undefined && d != null) {

        //group selection
        if (d.GroupId != null && d.GroupId.length > 0) {
            d.GroupId.forEach(function (v, i) {
                $('#idSelectGroups option[value="' + v + '"]').prop('selected', true).trigger('change', 'forUpdate');
            });
            $("#idSelectGroups").trigger("change");
        }

        //people selection
        if (d.SubscriptionId != null && d.SubscriptionId.length > 0) {
            d.SubscriptionId.forEach(function (v, i) {
                $('#idSelectPeople option[value="' + v + '"]').prop('selected', true).trigger('change', 'forUpdate');
            });
            $("#idSelectPeople").trigger("change");
        }

        //distribution group selection
        if (d.DistributionGroups != null && d.DistributionGroups.length > 0) {
            d.DistributionGroups.forEach(function (d, i) {
                var data =
                {
                    text: d.Name,
                    id: d.Id,
                    emailId: d.EmailId
                }
                // Fetch the preselected item, and add to the control
                $('#idSelectDGroups').select2("trigger", "select", { data: data });
            });
        }

        //ad people selection
        if (d.ADPeople != null && d.ADPeople.length > 0) {
            d.ADPeople.forEach(function (d, i) {

                var data = {
                    text: d.LastName + ", " + d.FirstName,
                    id: d.EmailId,
                    department: d.Department,
                    location: d.Location,
                    firstName: d.FirstName,
                    lastName: d.LastName
                };
                // Fetch the preselected item, and add to the control
                $('#idSelectADPeople').select2("trigger", "select", { data: data });
            });
        }

        //publishing Types and message content
        if (d.IsText)
            $("#TextCheckbox").prop('checked', true);
        if (d.IsEmail)
            $("#EmailCheckbox").prop('checked', true);
        if (d.IsWhatsApp)
            $("#WhatsappCheckbox").prop('checked', true);

        //hide or show AD people and distribution group dropdowns based on publishing type selection
        hideShowDGADUserDropdown();

        //execution type
        if (d.IsWizard)
            $("#idForWizard").prop("checked", "true");

        //timezone
        $('#SelectTimeZone').val(d.TimeZone);

        //start date
        $('#idStartDate').attr({
            'value': d.StartTime
        });
        $("#idStartDate").val(d.StartTime);

        //end date
        let nextDate = getNextDay(d.StartTime);
        $('#idEndDate').attr({
            'value': d.EndTime,
            'min': nextDate
        });
        $("#idEndDate").val(d.EndTime);

        //followup time
        $('#idFollowUpDate').attr({
            'min': nextDate,
            'max': d.EndTime,
        });
        $('#idFollowUpDate').val('');
        if (d.FollowUpTime != undefined && d.FollowUpTime != null && d.FollowUpTime != "") {
            $('#idFollowUpDate').attr({
                'value': d.FollowUpTime
            });
            $("#idFollowUpDate").val(d.FollowUpTime);
        }

    }
}