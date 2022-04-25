function updateUserDetails() {
    populateUserDetailsforGrid($(".ad-user-grid"));
    populateUserPicturesforGrid($(".img-circle-grid"));
}

$(document).ready(function () {

    $("#idGotoNotifications").click(function () {
        sessionStorage.setItem("currentModule","Notifications");
    });
    $("#idCreateNotification").click(function () {
        sessionStorage.setItem("currentModule", "");
    });
    if ($('#Incoming').val() == "IncomingMessageTab")
        $('#custom-content-below-profile-tab').click();
});

