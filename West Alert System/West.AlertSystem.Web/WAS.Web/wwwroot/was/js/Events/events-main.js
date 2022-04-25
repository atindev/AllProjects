
 
//for grid search start
$('#idActiveEvntsDropdown').change(function (event) {
    let selectedTxt = "";
    if ($(this).find("option:selected").val() == -1)
        selectedTxt = "";
    else
        selectedTxt = $(this).find("option:selected").text();
    searchGrid("GridActiveEvents",selectedTxt);
});
$('#idArchEvntsDropdown').change(function (event) {
    let selectedTxt = "";
    if ($(this).find("option:selected").val() == -1)
        selectedTxt = "";
    else
        selectedTxt = $(this).find("option:selected").text();
    searchGrid("GridArchEvents", selectedTxt);
});
function searchGrid(element, text) {
    var gridObj = document.getElementById(element).ej2_instances[0];
    gridObj.search(text);
}
$('#idsearchByActveEvnte').bind("enterKey", function (e) {
    var searchString = $('#idsearchByActveEvnte').val();
    searchGrid("GridActiveEvents", searchString);
});
$('#idsearchByActveEvnte').keyup(function (e) {
    if (e.keyCode == 13 || $(this).val() == "") {
        $(this).trigger("enterKey");
    }
});
$('#idsearchByArchEvnte').bind("enterKey", function (e) {
    var searchString = $('#idsearchByArchEvnte').val();
    searchGrid("GridArchEvents", searchString);
});
$('#idsearchByArchEvnte').keyup(function (e) {
    if (e.keyCode == 13 || $(this).val() == "") {
        $(this).trigger("enterKey");
    }
});
function clearNewEventContent() {

    $('#modal-lg .modal-title').text("Create Event");
    $("#idOperationType").val("Create");
    $('#idNewEventName').val("");
    $('#idNewEventStatus').val("");
    $('#idNewEventDescription').val("");
    $('#idEventID').val("");
    $("#idNewEventType").val($("#idNewEventType option:first").val());
    $("#idNewEventUrgency").val($("#idNewEventUrgency option:first").val());
}
function ArchiveEvent(e) {
    let eventID = e.getAttribute("eventid");
    $("#modal-sm #idArchiveEventID").val(eventID);
}
$(".closepopup").click(function () {
    $("#idAlertPopup").hide();
});
//for event Update
function updateRow(event) {
    let target = event.target;
    if (target.getAttribute("value") == "idArchiveEventButton" || target.getAttribute("value") == "eventDetails" || target.getAttribute("value") == "peopleDetails") 
        return false;

    $('#idAddOrUpdateEvent').click();
    $('#modal-lg .modal-title').text("Update Event");
    $("#idOperationType").val("Update");
    $('#idEventID').val((event.data.Id == null || event.data.Id == "") ? "" : event.data.Id);
    $('#idNewEventName').val((event.data.Name == null || event.data.Name == "") ? "" : event.data.Name);
    $('#idNewEventStatus').val((event.data.Status == null || event.data.Status == "") ? "" : event.data.Status);
    $('#idNewEventDescription').val((event.data.Description == null || event.data.Description == "") ? "" : event.data.Description);
    $('#idEventCreatedBy').val("");
    if (event.data.TypeId == null)
        $("#idNewEventType").val($("#idNewEventType option:first").val());
    else
        $('#idNewEventType').val(event.data.TypeId);
    if (event.data.UrgencyId == null)
        $("#idNewEventUrgency").val($("#idNewEventUrgency option:first").val());
    else
        $('#idNewEventUrgency').val(event.data.UrgencyId);

}
function updateActiveUserDetails() {
    populateUserDetailsforGrid($(".ad-user-grid"));
    populateUserPicturesforGrid($(".img-circle-grid"));
}
function updateArchivedUserDetails() {
    populateUserDetailsforGrid($(".ad-user-grid"));
    populateUserPicturesforGrid($(".img-circle-grid"));
}

function showEmptyTextAlert(text) {
    $("#idmodalsmText").text(text);
    $('#idAlertToggle').click();
}
$(document).ready(function () {

    $('#idbutnCreateEvent').click(function (e) {
       
        if ($("#idNewEventName").val() == "") {
            showEmptyTextAlert('Please enter event name');
            return false;
        }
        let eventViewModel = {};
        if ($("#idOperationType").val() == "Create") {
            eventViewModel["CreatedBy"] = $("#idEventCreatedBy").val();
        }
        else {
            eventViewModel["Id"] = $("#idEventID").val();
        }

        eventViewModel["ModifiedBy"] = $("#idEventCreatedBy").val();
        eventViewModel["Name"] = $("#idNewEventName").val();
        eventViewModel["TypeId"] = ($("#idNewEventType").val() == "") ? 0 : parseInt($("#idNewEventType").val());
        eventViewModel["UrgencyId"] = ($("#idNewEventUrgency").val() == "") ? 0 : parseInt($("#idNewEventUrgency").val());
        eventViewModel["Status"] = $("#idNewEventStatus").val();
        eventViewModel["Description"] = $("#idNewEventDescription").val();
        showLoader();

        $.ajax({
            url: "/was/Event/AddEvent",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(eventViewModel),
            success: function (d) {
                if (d.IsNameExist != undefined && d.IsNameExist != null && d.IsNameExist) {
                    hideLoader();
                    return false;
                }
                if (d.Success && d.Id != undefined && d.Name != undefined) {
                    location.reload();
                }
            },
            error: function (errorMessage) { // error callback 
                hideLoader();

            }
        });

    })

});


