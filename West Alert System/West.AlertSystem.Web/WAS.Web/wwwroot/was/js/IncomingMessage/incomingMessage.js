function searchGrid(element, text) {
    var gridObj = document.getElementById(element).ej2_instances[0];
    gridObj.search(text);
}

$('#idsearchByMessage').bind("enterKey", function (e) {
    var searchString = $('#idsearchByMessage').val();
    searchGrid("GridIncomingMessages", searchString);
});
$('#idsearchByMessage').keyup(function (e) {
    if (e.keyCode == 13 || $(this).val() == "") {
        $(this).trigger("enterKey");
    }
});

$('#idsearchByCall').bind("enterKey", function (e) {
    var searchString = $('#idsearchByCall').val();
    searchGrid("GridIncomingCalls", searchString);
});
$('#idsearchByCall').keyup(function (e) {
    if (e.keyCode == 13 || $(this).val() == "") {
        $(this).trigger("enterKey");
    }
});

function updateUserDetails() {
    populateUserDetailsforGrid($(".ad-user-grid"));
    populateUserPicturesforGrid($(".img-circle-grid"));
}

$(document).ready(function () {
    if ($('#IncomingCall').val() == "IncomingCallTab") {
        $('#custom-content-below-profile-tab').click();
    }
});