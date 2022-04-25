$(document).ready(function () {
    $('#modal-confirmation').modal('show');

    //$(".modalok").click(function () {
    //    $("#modal-confirmation").modal('hide');
    //});
});


$(document).ready(function () {
    $('#OfficeMobile,#PersonalMobile,#HomePhone,#OfficePhone,#PersonalEmail')
        .bind('blur', function (e) {
            validateCheckboxes(e.target.id);
        });

    //adding disabled class
    disableFieldUpdate();
    $("#idSubmitOrUpdate").click(function () {
        if (!validatePhoneNumber())
            return false;

    });

    $("#idSubmitOrUpdate").click(function () {
        if (!validateEmailDomain())
            return false;

    });


});

$(window).on('load', function () {
    //disabling checkbox according to value
    disableCheckbox();

    $('.classMenuItem').removeClass('active');
    sessionStorage.setItem("currentModule", "Subscription");


    var checkboxes = document.querySelectorAll('.subscriptionPreference');
    var selectedelement = [];
    for (var checkbox of checkboxes) {
        if (checkbox.checked) {
            var s = String(checkbox.name);
            var name = s.slice(2, s.length);
            selectedelement.push(name);
        }
    }

    var optionvalue = "";
    var x = document.getElementById("channeloption");
    for (var Y of selectedelement) {
        var displayvalue = subscription(Y);
        if (x != null && Y != x.value) {
            optionvalue += "<option id='channeloption' value = '" + Y + "'>" + displayvalue + "</option>";
        }
        else if (x == null) {
            optionvalue += "<option id='channeloption' value = '" + Y + "'>" + displayvalue + "</option>";
        }
    }
    $('#PreferredChannel').append(optionvalue);
});

function validateCheckboxes(textBox) {

    var textBoxVal = $('#' + textBox).val();
    var preferredChannel = $('#PreferredChannel').val();

    if (textBox == 'OfficeMobile') {

        $("#IsVoiceOfficeMobileParent,#IsTextOfficeMobileParent,#IsWhatsAppOfficeMobileParent").addClass('disableField');

        if (textBoxVal != null && textBoxVal != '' && textBoxVal != undefined) {
            $("#IsVoiceOfficeMobile,#IsTextOfficeMobile").attr("checked", "checked");
            $("#IsVoiceOfficeMobile,#IsTextOfficeMobile").prop("checked", true);
            $("#IsWhatsAppOfficeMobileParent").removeClass('disableField');
        }
        else {
            $("#IsVoiceOfficeMobile,#IsTextOfficeMobile,#IsWhatsAppOfficeMobile").removeAttr("checked");
            $("#IsVoiceOfficeMobile,#IsTextOfficeMobile,#IsWhatsAppOfficeMobile").prop("checked", false);
        }
    }

    else if (textBox == 'PersonalMobile') {
        if (textBoxVal != null && textBoxVal != '' && textBoxVal != undefined) {
            $("#IsVoicePersonalMobile,#IsTextPersonalMobile").attr("checked", "checked");
            $("#IsVoicePersonalMobile,#IsTextPersonalMobile").prop("checked", true);
            $("#IsVoicePersonalMobileParent,#IsTextPersonalMobileParent,#IsWatsAppPersonalMobileParent").removeClass('disableField');
        }
        else {
            $("#IsVoicePersonalMobile,#IsTextPersonalMobile,#IsWatsAppPersonalMobile").removeAttr("checked");
            $("#IsVoicePersonalMobile,#IsTextPersonalMobile,#IsWatsAppPersonalMobile").prop("checked", false);
            $("#IsVoicePersonalMobileParent,#IsTextPersonalMobileParent,#IsWatsAppPersonalMobileParent").addClass('disableField');
        }
    }

    else if (textBox == 'HomePhone') {
        if (textBoxVal != null && textBoxVal != '' && textBoxVal != undefined) {
            $("#IsVoiceHomePhone").attr("checked", "checked");
            $("#IsVoiceHomePhone").prop("checked", true);
            $("#IsVoiceHomePhoneParent").removeClass('disableField');
        }
        else {
            $("#IsVoiceHomePhone").removeAttr("checked");
            $("#IsVoiceHomePhone").prop("checked", false);
            $("#IsVoiceHomePhoneParent").addClass('disableField');
        }
    }

    else if (textBox == 'OfficePhone') {

        $("#IsVoiceOfficePhoneParent").addClass('disableField');
        if (textBoxVal != null && textBoxVal != '' && textBoxVal != undefined) {
            $("#IsVoiceOfficePhone").attr("checked", "checked");
            $("#IsVoiceOfficePhone").prop("checked", true);
        }
        else {
            $("#IsVoiceOfficePhone").removeAttr("checked");
            $("#IsVoiceOfficePhone").prop("checked", false);
        }
    }

    else if (textBox == 'OfficialEmail') {

        $("#IsOfficialEmailParent").addClass('disableField');
        if (textBoxVal != null && textBoxVal != '' && textBoxVal != undefined) {
            $("#IsOfficialEmail").attr("checked", "checked");
            $("#IsOfficialEmail").prop("checked", true);
        }
        else {
            $("#IsOfficialEmail").removeAttr("checked");
            $("#IsOfficialEmail").prop("checked", false);
        }
    }

    else if (textBox == 'PersonalEmail') {
        if (textBoxVal != null && textBoxVal != '' && textBoxVal != undefined) {
            $("#IsPersonalEmail").attr("checked", "checked");
            $("#IsPersonalEmail").prop("checked", true);
            $("#IsPersonalEmailParent").removeClass('disableField');
        }
        else {
            $("#IsPersonalEmail").removeAttr("checked");
            $("#IsPersonalEmail").prop("checked", false);
            $("#IsPersonalEmailParent").addClass('disableField');
        }
    }
    subscriptionPreferenceschanged(preferredChannel);
}

function disableFieldUpdate() {
    let disableEdit = ["#idFirstName", "#idLastName", "#OfficialEmail"];

    disableEdit.forEach((v, i) => {
        if ($(v).val() != undefined && $(v).val() != "")
            $(v).addClass("disableField");
    });

}

function disableCheckbox() {
    let disableCheckBox = ["OfficialEmail", "PersonalEmail", "OfficeMobile", "PersonalMobile", "HomePhone", "OfficePhone"];
    let currentEle = "";
    disableCheckBox.forEach((v, i) => {
        currentEle = $("#" + v).val();

        if (currentEle == undefined || currentEle == "")
            currentEle = $("#" + v).text();

        if (v == 'OfficeMobile') {

            $("#IsVoiceOfficeMobileParent,#IsTextOfficeMobileParent,#IsWhatsAppOfficeMobileParent").addClass('disableField');

            if (currentEle != undefined && currentEle != "" && currentEle != null) {
                $("#IsVoiceOfficeMobile,#IsTextOfficeMobile").attr("checked", "checked");
                $("#IsVoiceOfficeMobile,#IsTextOfficeMobile").prop("checked", true);
                $("#IsWhatsAppOfficeMobileParent").removeClass('disableField');
            }

        }

        else if (v == 'PersonalMobile') {

            if (currentEle != undefined && currentEle != "" && currentEle != null)
                $("#IsVoicePersonalMobileParent,#IsTextPersonalMobileParent,#IsWatsAppPersonalMobileParent").removeClass('disableField');
            else
                $("#IsVoicePersonalMobileParent,#IsTextPersonalMobileParent,#IsWatsAppPersonalMobileParent").addClass('disableField');

        }

        else if (v == 'HomePhone') {

            if (currentEle != undefined && currentEle != "" && currentEle != null)
                $("#IsVoiceHomePhoneParent").removeClass('disableField');
            else
                $("#IsVoiceHomePhoneParent").addClass('disableField');


        }

        else if (v == 'OfficePhone') {

            $("#IsVoiceOfficePhoneParent").addClass('disableField');

            if (currentEle != undefined && currentEle != "" && currentEle != null) {
                $("#IsVoiceOfficePhone").attr("checked", "checked");
                $("#IsVoiceOfficePhone").prop("checked", true);
            }

        }

        else if (v == 'OfficialEmail') {

            $("#IsOfficialEmailParent").addClass('disableField');
            if (currentEle != undefined && currentEle != "" && currentEle != null) {
                $("#IsOfficialEmail").attr("checked", "checked");
                $("#IsOfficialEmail").prop("checked", true);

            }

        }

        else if (v == 'PersonalEmail') {

            if (currentEle != undefined && currentEle != "" && currentEle != null)
                $("#IsPersonalEmailParent").removeClass('disableField');
            else
                $("#IsPersonalEmailParent").addClass('disableField');

        }

    });

}

function validateEmailDomain(element) {
    var a = document.getElementById("OfficialEmail").value.toLowerCase();
    var emailLegalReg = /^([A-Za-z0-9_\-\.])+\@@(westpharma)+\.((com))$/;

    var position = $(event.currentTarget).position();

    if (emailLegalReg.test(a)) {
        return true;
    }
    else {
        const Toast = Swal.mixin({
            toast: true,
            position: "absolute",
            showConfirmButton: false,
            addClass: "classOfficialEmailValidator",
            timer: 5000
        });
        Toast.fire({
            icon: 'error',
            title: "Work Email is not valid. Domain should be 'westpharma.com'"
        });
        document.getElementById("OfficialEmail").style.borderColor = "red";
        return false;
    }
}
function validatePhoneNumber(element) {
    var WorkMobile = document.getElementById("OfficeMobile").value;
    var PersonalMobile = document.getElementById("PersonalMobile").value;
    var PersonalEmail = document.getElementById("PersonalEmail").value;
    var flag = true;
    if (WorkMobile.length == 0 && PersonalMobile.length == 0 && PersonalEmail.length == 0) {
        subscriptionAlert("Please enter either one of WorkMobile, PersonalMobile or PersonalEmail");
        flag = false;
    }

    return flag;

}
function subscriptionAlert(text) {
    $("#idmodalsmText").text(text);
    $('#idAlertToggle').click();
}
function showLoader() {
    $('.divLoader').show();
}

function hideLoader() {
    $('.divLoader').hide();
}

function SubscriptionFeedback() {
    let subscriptionFeedback = {};
    subscriptionFeedback["SubscriptionId"] = $('#SubscriptionId').val();
    subscriptionFeedback["Feedback"] = $('input[type=radio][name=Feedback]:checked').val();
    subscriptionFeedback["FeedbackChannel"] = $('#FeedbackChannel').val();
    showLoader();
    $.ajax({
        url: "/was/Subscription/subscriptionFeedback",
        type: "POST",
        data: subscriptionFeedback,
        success: function (result) {
            hideLoader();
        },
        error: function (errorMessage) {
            hideLoader();
        }
    });
}

function subscriptionPreferenceschanged(channelpreference) {
    var checkboxes = document.querySelectorAll('.subscriptionPreference');
    var selectedelement = [];
    for (var checkbox of checkboxes) {
        if (checkbox.checked) {
            var s = String(checkbox.name);
            var name = s.slice(2, s.length);
            selectedelement.push(name);
        }
    }


    var optionvalue = "";
    optionvalue += "<option disabled> Please Select Your Preferred Channel</option>";
    for (var Y of selectedelement) {
        var displayvalue = subscription(Y);
        if (Y == channelpreference) {
            optionvalue += "<option id='channeloption' selected value = '" + Y + "'>" + displayvalue + "</option>";
        }
        else {
            optionvalue += "<option id='channeloption' value = '" + Y + "'>" + displayvalue + "</option>";
        }
    }
    $('#PreferredChannel').empty();
    $('#PreferredChannel').append(optionvalue);
}

function subscription(Y) {
    var optionvalue = "";
    if (Y == "OfficialEmail") {
        optionvalue = "Work Email";
    }
    else if (Y == "PersonalEmail") {
        optionvalue = "Personal Email";
    }
    else if (Y == "TextOfficeMobile") {
        optionvalue = "SMS on Work Mobile";
    }
    else if (Y == "TextPersonalMobile") {
        optionvalue = "SMS on Personal Mobile";
    }
    else if (Y == "WhatsAppOfficeMobile") {
        optionvalue = "Message on WhatsApp on Work Mobile";
    }
    else if (Y == "WhatsAppPersonalMobile") {
        optionvalue = "Message on WhatsApp on Personal Mobile"
    }
    return optionvalue;
}