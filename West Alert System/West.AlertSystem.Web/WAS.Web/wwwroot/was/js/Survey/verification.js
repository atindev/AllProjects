
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
                        getQuestionList();
                        $('.steps ul').addClass('step-2');
                    }
                }
            }
            else {
                $('.steps ul').removeClass('step-2');
                $("#spanEmailError").text("");
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
                        if (answerList != undefined && answerList != null && answerList.length > 2) {
                            $($(".actions li")[0]).hide();
                            verifyAnswers(answerList);
                        }
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
                $('.actions ul').addClass('step-last');
            }
            else {
                $('.steps ul').removeClass('step-3');
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
     
    $('#wizard .actions li:last-child').append('<a class="btn btn-block btn-warning" id="idSubmitOrUpdate" href="" style="visibility:hidden">VERIFY</a>');

    $('.classAlertClose').click(function (event) {
        if ($("#idAlertPopupSubmit").attr("value") == "blocked")
            location.reload();
    });

});

function closeMe() {
    window.opener = self;
    window.close();
}

function showLoader() {
    $('.divLoader').show();
}

function hideLoader() {
    $('.divLoader').hide();
}

function validateEmailDomain(element) { 
    var a = document.getElementById("OfficialEmail").value.toLowerCase();
    var emailLegalReg = /^[a-zA-Z0-9._%+-]+@westpharma\.com$/;
    var employeeIdReg = /^\d{1,10}$/;

    if ((a.indexOf("@") == -1) || emailLegalReg.test(a) || employeeIdReg.test(a)) {
        $("#OfficialEmail").removeAttr('style');
        return true;
    }
    else {
        Toast.fire({
            icon: 'error',
            title: "Entered input is not acceptable for Employee Id  or Work Email \n In case of work email, it has to be of @westpharma.com domain"
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

function clearTriangulationvalidation() {
    questionList = [];
    answerList = [];
    currentQuestionIndex = 1;
    $("#idAlertPopupSubmit").attr("value", "active");
    $("#idValidationSection").html("");
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
    var currentValue = $("#OfficialEmail").val();
    request["Questions"] = answerList;
    request["Email"] = currentValue;

    var momentVariable = moment.utc(new Date(), "");
    convertedDate = momentVariable.clone().tz(moment.tz.guess()).format("MMM DD, YYYY hh:mm:ss A");
    request["AttemptON"] = convertedDate + " " + moment.tz.guess();
    request["AttemptFrom"] = "Survey";

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
                showLoader();
                let broadcastId = $("#idBroadcastId").val();
                let path = "/WAS/Survey/AuthenticateSurvey?broadcastId=" + broadcastId + "&email=" + currentValue;
                $("#idSubmitOrUpdate").attr("href", path);
                $("#idSubmitOrUpdate")[0].click();
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