 
var questionList = [];
var currentQuestionIndex = 1;
var answerList = [];
$(document).ready(function () {
    clearTriangulationvalidation();

    $("#wizard").steps({
        titleTemplate: '<span class="number">#index#</span> <span class="w_title">#title#</span>',
        headerTag: "h4",
        bodyTag: "section",
        transitionEffect: "fade",
        enableAllSteps: true,
        transitionEffectSpeed: 500,
        onStepChanging: function (event, currentIndex, newIndex) {
            removeClassFromsucceedingElements(newIndex);

            if (newIndex === 1) {
                $("#WorkEmail").text("");
                $("#spanUsername").text("");
                $("#spandesignation").text("");
                $("#LocationName").text("");
                $("#manager").text("");
                $("#imgUser").attr("src", "");
                $("#imgManager").attr("src", "");
                $("#EmployeeId").text("");
                $("#OfficePhone").val("");
                $("#OfficeMobile").val("");
                $("#PersonalEmail").val("");
                $("#PersonalMobile").val("");
                $("#HomePhone").val("");
                $("#ShiftId").val("");
               
                if ($("#OfficialEmail").val() == "") {
                    $('.steps ul li').removeClass('done');
                    $("#captcha").text("");
                    $("#spanEmailError").text("Please enter Employee ID or User Id or Work Email");
                    $("#OfficialEmail").focus();
                    return false;
                }
                else {
                    if (!validateEmailDomain()) {
                        $('.steps ul li').removeClass('done');
                        return false;
                    } else {
                        $('.steps ul li').removeClass('done');
                        $("#spanEmailError").text("");
                        var captchaResponse = $('#idEnableCaptcha').val() === 'true' ? grecaptcha.getResponse() : "1";
                        if (captchaResponse.length == 0) {
                            $("#captcha").text("Please verify captcha");
                            return false;
                        }
                        else {
                            getQuestionList();
                            $('.steps ul').addClass('step-2');
                        }
                    }
                }
            }
            else {
                $('.steps ul').removeClass('step-2');
                $("#spanEmailError").text("");
                $("#captcha").text("");
            }

            if (newIndex === 2) {
                var obj = {};
                let currentSelection = $('input[type=radio][name=Answers]:checked').val();
                let currentType = $('input[type=radio][name=Answers]:checked').attr("questiontype");
                if (currentQuestionIndex == 3)
                {
                    if ($('.validationOptions').is(':checked') && currentSelection != undefined) {
                        obj = {};
                        obj.Answer = currentSelection;
                        obj.Type = currentType;
                        answerList.push(obj);
                        if (answerList != undefined && answerList != null && answerList.length>2)
                        verifyAnswers(answerList);
                    }
                    else {
                        if ($(".validationOptions").length > 0 && currentSelection == undefined) {
                            showAlertpopup("Please complete the verification");
                        }
                        $('.steps ul li').removeClass('done');
                        return false;
                    }
                }
                else if (currentQuestionIndex == 2) {
                    if ($('.validationOptions').is(':checked') && currentSelection != undefined) {
                        obj = {};
                        obj.Answer = currentSelection;
                        obj.Type = currentType;
                        answerList.push(obj);
                        setValidationContent(questionList[currentQuestionIndex]);
                        currentQuestionIndex = 3;
                        return false;
                    }
                    else {
                        if ($(".validationOptions").length > 0 && currentSelection == undefined) {
                            showAlertpopup("Please complete the verification");
                        }
                        $('.steps ul li').removeClass('done');
                        return false;
                    }
                }
                else if (currentQuestionIndex == 1) {
                    if ($('.validationOptions').is(':checked') && currentSelection != undefined) {
                        obj = {};
                        obj.Answer = currentSelection;
                        obj.Type = currentType;
                        answerList.push(obj);
                        setValidationContent(questionList[currentQuestionIndex]);
                        currentQuestionIndex = 2;
                        return false;
                    }
                    else {
                        if ($(".validationOptions").length>0 && currentSelection == undefined) {
                            showAlertpopup("Please complete the verification");
                        }
                        $('.steps ul li').removeClass('done');
                        return false;
                    }
                }
                else {
                    $('.steps ul li').removeClass('done');
                    return false;
                }
            }
            else {
                $('.steps ul').removeClass('step-2');
            }

            if (newIndex === 3) {
                $('.steps ul').addClass('step-3');
            }
            else {
                $('.steps ul').removeClass('step-3');
            }

            if (newIndex === 4) {
                var ShiftId = $('#ShiftId option:selected').val();
                if (ShiftId == '' || (ShiftId == undefined)) {
                    $("#SpanShiftmessage").text("Please select the shift");
                    return false;
                }
                else {
                    $('.steps ul').addClass('step-4');
                    $('.actions ul').addClass('step-4');
                    $("#SpanShiftmessage").text("");
                }
                return validatePhoneNumber();
            }
            else {
                $('.steps ul').removeClass('step-4');
                $('.actions ul').removeClass('step-4');
                $("#SpanShiftmessage").text("");
            }

            if (newIndex === 5) {
                $('.steps ul').addClass('step-5');
                $('.actions ul').addClass('step-last');
            }
            else {
                $('.steps ul').removeClass('step-5');
                $('.actions ul').removeClass('step-last');
            }

            return true;
        },
        onStepChanged: function (event, currentIndex, priorIndex) {
            removeClassFromsucceedingElements(currentIndex+1);
        },
        labels: {
            finish: "Submit",
            next: "Next",
            previous: "Previous"
        }
    });
    // Custom Steps Jquery Steps
    $('.wizard > .steps li a').click(function () {
        $(this).parent().addClass('checked');
        $(this).parent().prevAll().addClass('checked');
        $(this).parent().nextAll().removeClass('checked');
    });
    // Custom Button Jquery Steps
    $('.forward').click(function () {
        $("#wizard").steps('next');
    });
    $('.backward').click(function () {
        $("#wizard").steps('previous');
    });
    // Checkbox
    $('.checkbox-circle label').click(function () {
        $('.checkbox-circle label').removeClass('active');
        $(this).addClass('active');
    });
    //append a submit type button
    $('#wizard .actions li:last-child').append('<button type="submit" id="idSubmitOrUpdate" class="btn btn-block btn-warning">SUBSCRIBE</button>');


    $('#OfficeMobile,#PersonalMobile,#HomePhone,#OfficePhone,#PersonalEmail')
        .bind('blur', function (e) {
            validateCheckboxes(e.target.id);
        });

    $('.classAlertClose').click(function (event) {
        if ($("#idAlertPopupSubmit").attr("value") == "blocked")
            location.reload();
    });

});

function closeMe() {
    window.opener = self;
    window.close();
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

function showLoader() {
    $('.divLoader').show();
}

function hideLoader() {
    $('.divLoader').hide();
}

function getsubscriptiondetails() {
    var emailId = $("#idEmployeeId").val();
    if (emailId != "") {
        $('#noRecord').attr("style", "display: none");
        $('#foundRecord').attr("style", "display: flex;");
        showLoader();
        $("#IsOfficialEmail").prop("checked", true);
        $.ajax({
            type: "GET",
            url: "/was/Graph/getuserdetails?emailId=" + emailId,
            dataType: "json",
            success: function (result) {
                if (result != undefined && result != null) {
                    populateExistingUserDetails(result);
                }
                else {
                    hideLoader();
                    $('a[href="#next"]').parent().attr("style", "display: none;");
                    $('#foundRecord').attr("style", "display: none;");
                    $('#noRecord').attr("style", "display: flex");
                    return false;
                }
            },
            error: function (errorMessage) {
                hideLoader();
            }
        });
    }
}

function populateExistingUserDetails(user) {
    var adUser = user;
    if (adUser.Email != "") {
        $.ajax({
            type: "GET",
            url: "/was/Subscription/getsubscriptiondetails?emailId=" + adUser.Email,
            dataType: "json",
            success: function (result) {
                $("#WorkEmail").text(adUser.Email);
                $("#spanUsername").text(adUser.FullName);
                $("#spandesignation").text(adUser.Designation);
                $("#LocationName").text(adUser.Location);
                $("#manager").text(adUser.ReportManager);
                if (adUser.PictureBase64 != null) {
                    $("#imgUser").attr("src", adUser.PictureBase64);
                } else {
                    $("#imgUser").attr("src", "/was/img/user2-160x160.jpg");
                }
                if (adUser.ManagerPictureBase64 != null) {
                    $("#imgManager").attr("src", adUser.ManagerPictureBase64);
                } else {
                    $("#imgManager").attr("src", "/was/img/user2-160x160.jpg");
                }
                $("#EmployeeId").text(adUser.EmployeeId);
                $("#Upn").val(adUser.UserPrincipalName);
                if ($('#UserEmailID') != 'undefined' || $('#UserEmailID').val() == null || $('#UserEmailID').val() == "") {
                    $('#UserEmailID').val(adUser.Email);
                    $('#Username').val(adUser.FullName);
                }
                clearAllCheckbox();
                
                populateDialCodeWithFlag($(".country-code"));
                disableCheckbox();
                hideLoader();
            },
            error: function (errorMessage) {
                hideLoader();
            }
        });
    }
}

function validateEmailDomain(element) { 
    var a = document.getElementById("OfficialEmail").value.toLowerCase();
    var emailLegalReg = /^[a-zA-Z0-9._%+-]+@westpharma\.com$/;
    var employeeIdReg = /^\d{1,10}$/;

    if ((a.indexOf("@")==-1) || emailLegalReg.test(a) || employeeIdReg.test(a)) {
        $("#OfficialEmail").removeAttr('style');
        return true;
    }
    else {
        Toast.fire({
            icon: 'error',
            title: "Entered input is not acceptable for Employee Id or User Id or Work Email \n In case of work email, it has to be of @westpharma.com domain"
        });
        document.getElementById("OfficialEmail").style.borderColor = "red";
        return false;
    }
}

const Toast = Swal.mixin({
    toast: true,
    position: "absolute",
    showConfirmButton: false,
    addClass: "classOfficialEmailValidator",
    timer: 10000
});

function enableSubscription(result) {

    if (result.IsPersonalEmail && result.PersonalEmail != null && result.PersonalEmail != "") {
        $("#IsPersonalEmail").attr("checked", "checked");
        $("#IsPersonalEmail").prop("checked", true);
    }

    if (result.IsTextPersonalMobile && result.PersonalMobile != null && result.PersonalMobile != "") {
        $("#IsTextPersonalMobile").attr("checked", "checked");
        $("#IsTextPersonalMobile").prop("checked", true);
    }
    
    if (result.IsVoicePersonalMobile && result.PersonalMobile != null && result.PersonalMobile != "") {
        $("#IsVoicePersonalMobile").attr("checked", "checked");
        $("#IsVoicePersonalMobile").prop("checked", true);
    }
     
    if (result.IsWhatsAppPersonalMobile && result.PersonalMobile != null && result.PersonalMobile != "") {
        $("#IsWatsAppPersonalMobile").attr("checked", "checked");
        $("#IsWatsAppPersonalMobile").prop("checked", true);
    }

    if (result.IsVoiceHomePhone && result.PersonalMobile != null && result.PersonalMobile != "") {
        $("#IsVoiceHomePhone").attr("checked", "checked");
        $("#IsVoiceHomePhone").prop("checked", true);
    }
    
}

function clearAllCheckbox() {

    $("#IsPersonalEmail").removeAttr("checked");
    $("#IsPersonalEmail").prop("checked", false);

    $("#IsTextPersonalMobile").removeAttr("checked");
    $("#IsTextPersonalMobile").prop("checked", false);

    $("#IsVoicePersonalMobile").removeAttr("checked");
    $("#IsVoicePersonalMobile").prop("checked", false);

    $("#IsWatsAppPersonalMobile").removeAttr("checked");
    $("#IsWatsAppPersonalMobile").prop("checked", false);

    $("#IsVoiceHomePhone").removeAttr("checked");
    $("#IsVoiceHomePhone").prop("checked", false);

    $("#IsVoiceOfficeMobile,#IsTextOfficeMobile,#IsWhatsAppOfficeMobile").removeAttr("checked");
    $("#IsVoiceOfficeMobile,#IsTextOfficeMobile,#IsWhatsAppOfficeMobile").prop("checked", false);

    $("#IsVoiceOfficePhone").removeAttr("checked");
    $("#IsVoiceOfficePhone").prop("checked", false);
}

function clearTriangulationvalidation() {
    questionList = [];
    answerList = [];
    currentQuestionIndex = 1;
    $("#idAlertPopupSubmit").attr("value", "active");
    $("#idValidationSection").html("");
}

function getRandomQuestion(availableQuestions) {
    let item = availableQuestions[Math.floor(Math.random() * availableQuestions.length)];
    return item;
}

function getValidationQuestion() {

    let randomQuestion = getRandomQuestion(availableQuestions);
    if (randomQuestion != undefined && randomQuestion != null) {



    }
}

function getQuestionList() {
    clearTriangulationvalidation();
    var emailId = $("#OfficialEmail").val();
    if (emailId != "") {
        showLoader();
        $.ajax({
            type: "GET",
            url: "/was/Graph/GetValidationList?emailId=" + emailId,
            dataType: "json",
            success: function (result) {
                if (result != undefined && result != null) {
                    if (result.IsUserBlocked) {
                        showAlertpopup("Your account is blocked for " + ($("#idUserBlockedTime").val()) + " minutes, please try again after that");
                        triggerFirstStep();
                    }
                    else if (result.Questions != null && result.Questions.length > 0) {
                        questionList = shuffleArray(result.Questions);
                        currentQuestionIndex = 1;
                        setValidationContent(questionList[0]);
                        $("#idEmployeeId").val(result.EmployeeId);
                    }
                    else {
                        showAlertpopup("Please enter a valid Employee ID or User Id or Work Email");
                        triggerFirstStep();
                    }
                }
                else {
                    showAlertpopup("Please enter a valid Employee ID or User Id or Work Email");
                    triggerFirstStep();
                }
                hideLoader();
            },
            error: function (errorMessage) {
                hideLoader();
            }
        });
    }
}

function setValidationContent(data) {
    let list = "";
    $("#idValidationSection").html("");

    //question heading 
    if (data != undefined && data != null && data.Question != undefined && data.Question != null) {
        list += "<div class='row'>";
        list += "<div class='col-md-12'>";
        list += "<div class='form-group'>";
        list += "<label id='idquestionText'>" + data.Question + "</label>";
        list += "</div>"; //form-group
        list += "</div>"; //col-md-12
        list += "</div>"; //row

        if (data.Options != undefined && data.Options.length > 3) {
            let options = data.Options;

            //first option section start
            list += "<div class='row'>";

            list += "<div class='col-md-6'>";
            list += "<div class='d-inline-block customRadioWizard'>";
            list += "<input type='radio' id='idAnswer1' class='validationOptions' name='Answers' questiontype='" + data.Type +"' value='" + options[0] + "' />";
            list += "<label for=idAnswer1><span></span>" + options[0] + "</label>";
            list += "</div>"; //customRadioWizard
            list += "</div>"; //col-md-6

            list += "<div class='col-md-6'>";
            list += "<div class='d-inline-block customRadioWizard'>";
            list += "<input type='radio' id='idAnswer2' class='validationOptions' name='Answers' questiontype='" + data.Type +"' value='" + options[1] + "' />";
            list += "<label for=idAnswer2><span></span>" + options[1] + "</label>";
            list += "</div>"; //customRadioWizard
            list += "</div>"; //col-md-6

            list += "</div>"; //row
            //first option section ends

            //second option section start
            list += "<div class='row'>";

            list += "<div class='col-md-6'>";
            list += "<div class='d-inline-block customRadioWizard'>";
            list += "<input type='radio' id='idAnswer3'class='validationOptions' name='Answers' questiontype='" + data.Type +"' value='" + options[2] + "' />";
            list += "<label for=idAnswer3><span></span>" + options[2] + "</label>";
            list += "</div>"; //customRadioWizard
            list += "</div>"; //col-md-6

            list += "<div class='col-md-6'>";
            list += "<div class='d-inline-block customRadioWizard'>";
            list += "<input type='radio' id='idAnswer4' class='validationOptions' name='Answers' questiontype='" + data.Type +"' value='" + options[3] + "' />";
            list += "<label for=idAnswer4><span></span>" + options[3] + "</label>";
            list += "</div>"; //customRadioWizard
            list += "</div>"; //col-md-6

            list += "</div>"; //row
            //second option section ends
        }

        if (list != "")
            $("#idValidationSection").html(list);
    }

   
}

function showAlertpopup(text) {
    $("#idmodalpopupText").text(text);
    $("#idAlertpopup").click();
}

function shuffleArray(array) {
    var currentIndex = array.length, randomIndex;

    // While there remain elements to shuffle...
    while (0 !== currentIndex) {

        // Pick a remaining element...
        randomIndex = Math.floor(Math.random() * currentIndex);
        currentIndex--;

        // And swap it with the current element.
        [array[currentIndex], array[randomIndex]] = [
            array[randomIndex], array[currentIndex]];
    }

    return array;
}

function verifyAnswers() {
    if ($("#OfficialEmail").val() == undefined || $("#OfficialEmail").val() == "")
        return false;

    showLoader();
    let request = {};
    let currentValue = $("#OfficialEmail").val();
    request["Questions"] = answerList;
    request["Email"] = currentValue;

    var momentVariable = moment.utc(new Date(), "");
    convertedDate = momentVariable.clone().tz(moment.tz.guess()).format("MMM DD, YYYY hh:mm:ss A");
    request["AttemptON"] = convertedDate + " " + moment.tz.guess();
    request["AttemptFrom"] = "Subscription";

    $.ajax({
        url: "/was/Subscription/ValidateAnswers",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(request),
        success: function (data) {
            hideLoader();
            if (data == undefined || data == null || data == "" || data == "Failed to authenticate") {
                showAlertpopup("Failed to authenticate");
                return false;
            }
            else if (data == "Blocked") {
                $("#idAlertPopupSubmit").attr("value", "blocked");
                let blockedTime = $("#idUserBlockedTime").val();
                showAlertpopup("You selected a wrong answer, your account will be blocked for " + blockedTime + " minutes");
            }
            else if (data == "Verified") {
                $('.steps ul').addClass('step-2');
                getsubscriptiondetails();
            }
        },
        error: function () {
            hideLoader();
        }
    });
}

function removeClassFromsucceedingElements(step) {
    for (i = step; i < 6; i++) {
        $($("#wizard li[role='tab']")[i]).removeClass("checked");
        $($("#wizard li[role='tab']")[i]).removeClass("done");
        $($("#wizard li[role='tab']")[i]).removeClass("current");
    }
}

function triggerFirstStep() {
    if ($($($("#wizard li")[0]).find("a")).length > 0)
        $($($("#wizard li")[0]).find("a")).click();
    if ($("#wizard li").length > 1)
        $($("#wizard li")[1]).removeClass("done");
}