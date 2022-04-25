function searchGrid(element, text) {
    var gridObj = document.getElementById(element).ej2_instances[0];
    gridObj.search(text);
}

$('#idsearchByText').bind("enterKey", function (e) {
    var searchString = $('#idsearchByText').val();
    searchGrid("GridOcrSubscriptionList", searchString);
});
$('#idsearchByText').keyup(function (e) {
    if (e.keyCode == 13 || $(this).val() == "") {
        $(this).trigger("enterKey");
    }
});

$(document).on("change", ".idOrEmailChange", function (event) {
    $('#ClearContent').show();
    $('#idValidateSubscription').show();
    $('#SubmitSubscription').hide();
});

function showEmptyTextAlert(text) {
    $("#idmodalsmText").text(text);
    $('#idAlertToggle').click();
}

$('#SubmitSubscription').click(function (e) {
    if (!$("#IsConsentYes").prop("checked")) {
        showEmptyTextAlert('Please select Acknowledgement & Consent');
        return false;
    }
    else {
        return true;
    }
});

function validate() {
    if ($("#FirstName").val() == "") {
        showEmptyTextAlert('Please enter FirstName');
        return false;
    }
    if ($("#LastName").val() == "") {
        showEmptyTextAlert('Please enter LastName');
        return false;
    }
    if ($("#EmployeeId").val() == "" && $("#OfficialEmail").val() == "" && $("#UserId").val() == "") {
        showEmptyTextAlert('Please provide at least one of Employee ID or Official Email or User ID.');
        return false;
    }
    if ($("#PersonalEmail").val() == "" && $("#PersonalMobile").val() == "") {
        showEmptyTextAlert('Please provide at least one of Personal Mobile or Personal Email.');
        return false;
    }
    $('#ErrorDiv').hide();
    $('#ErrorText').text("");
    $('#WarningDiv').hide();
    $('#WarningText').text("");
    return true;
}

function ValidateSubscription() {
    if (!validate()) {
        return false;
    }
    var employeeId = $('#EmployeeId').val();
    var userId = $('#UserId').val();
    var officialEmail = $('#OfficialEmail').val();
    var firstName = $('#FirstName').val();
    var lastName = $('#LastName').val();
    var email = '';
    if (employeeId != undefined && employeeId != '') {
        email = employeeId;
    } else if (officialEmail != undefined && officialEmail != '') {
        email = officialEmail;
    } else if (userId != undefined && userId != '') {
        email = userId;
    }

    if (email != undefined || email != '') {
        showLoader();
        let url = '/was/subscription/verifysubscription';
        $.ajax({
            url: url,
            type: 'GET',
            data: { emailId: email, firstName: firstName, lastName: lastName },
            success: function (result) {
                if (result.SubscriptionId != null) {
                    $('#ErrorDiv').hide();
                    $('#ErrorText').text("");
                    $('#WarningDiv').show();
                    $('#WarningText').text("We found an existing subscription for the provided Employee ID or Official Email or User ID, you can discard this profile");
                    $('#idValidateSubscription').hide();
                    $('#SubmitSubscription').hide();
                    $('#ClearContent').hide();
                    getSubscriptionDetailsById(result.SubscriptionId);
                    if (officialEmail != '') {
                        $("#IsOfficialEmail").prop("checked", true);
                        $("#IsOfficialEmailParent").addClass('disableField');
                    }
                    hideLoader();
                }
                else {
                    if (result.ADUser == undefined || result.ADUser == null) {
                        $('#ClearContent').show();
                        $('#WarningDiv').hide();
                        $('#WarningText').text("");
                        $('#ErrorDiv').show();
                        $('#ErrorText').text("Employee ID or Official Email or User ID not found");
                    }
                    else if (result.ADUser.FirstName.toUpperCase() != firstName.toUpperCase() || result.ADUser.LastName.toUpperCase() != lastName.toUpperCase()) {
                        $('#ClearContent').show();
                        $('#WarningDiv').hide();
                        $('#WarningText').text("");
                        $('#ErrorDiv').show();
                        $('#ErrorText').text("FirstName or LastName does not match with our records for the provided Employee ID or Official Email or User ID");
                    }
                    else {
                        $('#ClearContent').show();
                        $('#ErrorDiv').hide();
                        $('#ErrorText').text("");
                        $('#WarningDiv').hide();
                        $('#WarningText').text("");
                        $('#idValidateSubscription').hide();
                        $('#SubmitSubscription').show();
                        $('#OfficialEmail').val(result.ADUser.Email);
                        $('#EmployeeId').val(result.ADUser.EmployeeId);
                        $('#UserId').val(result.ADUser.UserId);
                        $("#IsOfficialEmail").prop("checked", true);
                        $("#IsOfficialEmailParent").addClass('disableField');
                    }
                }
                hideLoader();
            },
            error: function (err) {
                hideLoader();
            },
            complete: function (data) {
                hideLoader();
            }
        });
    }
}

function getSubscriptionDetailsById(id) {
    var subscriptionId = id;
    $.ajax({
        type: 'POST',
        url: '/was/Subscription/GetMaskedById',
        data: {
            id: subscriptionId
        },
        success: function (result) {
            if (result != undefined && result != null) {
                $("#idSubscriptionDetailView").html(result);
                $('#idShowSubscriptionDetails').click();
                populateUserPictures($(".img-circle"));
                $('#WarningDivSubscriptionDetail').show();
                $('#WarningHeaderSubscriptionDetail').text("Warning!");
                $('#WarningTextSubscriptionDetail').text("We found an existing subscription for the provided Employee ID or Official Email or User ID, you can discard the profile");
            }
            hideLoader();
        },
        error: function (err) {
            hideLoader();
        }
    });
}

function populateUserPictures(imgElement) {
    $(imgElement).each(function () {
        var emailId = $(this).data("useremail");
        var imageElement = $(this);
        if (emailId != undefined && emailId != '') {
            url = '/was/graph/getuserpicture';
            $.ajax({
                url: url,
                type: 'GET',
                data: { emailId: emailId },
                success: function (result) {
                    if (result != undefined && result != null && result.PictureBase64 != undefined && result.PictureBase64 != null)
                        $(imageElement).attr('src', result.PictureBase64)
                },
                error: function (err) {
                },
                complete: function (data) {
                }
            });
        }
    });
}

$('#ClearContent').click(function (e) {
    showLoader();
    $("#FirstName").val("");
    $("#LastName").val("");
    $("#EmployeeId").val("");
    $("#OfficialEmail").val("");
    $("#PersonalEmail").val("");
    $("#HomePhone").val("");
    $("#PersonalMobile").val("");
    $("#IsOfficialEmail").prop("checked", false);
    $("#IsPersonalEmail").prop("checked", false);
    $("#IsVoiceHomePhone").prop("checked", false);
    $("#IsVoicePersonalMobile").prop("checked", false);
    $("#IsTextPersonalMobile").prop("checked", false);
    $("#IsWatsAppPersonalMobile").prop("checked", false);
    $("#IsConsentYes").prop("checked", false);
    hideLoader();
});

function DiscardSubscription() {
    var subscriptionReviewId = $("#SubscriptionReviewId").val();
    showLoader();
    $.ajax({
        type: 'POST',
        url: '/was/Subscription/discardOcrSubscription',
        data: {
            subscriptionReviewId: subscriptionReviewId
        },
        success: function (result) {
            window.location.href = `/was/Subscription/SubscriptionReviewList`;
            hideLoader();
        },
        error: function (err) {
            hideLoader();
        }
    });
}

$(".selected-location").on("change", function () {
    showLoader();
        const language = $(this).val();
    window.location.href = `/was/Subscription/SubscriptionReviewList/${language}`;
    hideloader();
});