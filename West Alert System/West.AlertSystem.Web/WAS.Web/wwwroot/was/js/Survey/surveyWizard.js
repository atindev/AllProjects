var answeredQues = 1;
var currentQuestion = 1;
var currentTime = new Date();
var current = 1;

$(document).ready(function () {

    var current_fs, next_fs, previous_fs; //top divs
    var opacity;

    current = 1;
    currentTime = new Date();
    currentQuestion = 1;

    setProgressBar(current);

    $(".next").click(function () {

        $(".validation-box").removeClass('validation-box');

        if (currentTime == "" || currentTime == undefined)
            currentTime = new Date();

        let questionAnswer = createCurrentAnswerRequest($(".SurveyMainParent")[currentQuestion - 1]);
        if (!validateQuestions(questionAnswer, 'Please answer this required question to move to next.'))
            return false;

        current_fs = $(this).parent();
        next_fs = $(this).parent().next();

        //Add Class Active
        $("#progressbar li").eq($(".step_field").index(next_fs)).addClass("active");

        //show the next div
        next_fs.show();
        //hide the current div with style
        current_fs.animate({ opacity: 0 }, {
            step: function (now) {
                // for making fielset appear animation
                opacity = 1 - now;

                current_fs.css({
                    'display': 'none',
                    'position': 'relative'
                });
                next_fs.css({ 'opacity': opacity });
            },
            duration: 500
        });
        setProgressBar(++current);

        //adding the elapsed time
        if ($($(".SurveyMainParent")[currentQuestion - 1]).attr("timetaken") == undefined || $($(".SurveyMainParent")[currentQuestion - 1]).attr("timetaken") == "") {
            var seconds = Math.round((new Date().getTime() - currentTime.getTime()) / 1000);
            $($(".SurveyMainParent")[currentQuestion - 1]).attr("timetaken", seconds);
        }
        currentTime = new Date();

        ++currentQuestion;
    });

    $(".previous").click(function () {
        $(".validation-box").removeClass('validation-box');

        // Validate question before moving to previous question
        let questionAnswer = createCurrentAnswerRequest($(".SurveyMainParent")[currentQuestion - 1]);
        if (!validateQuestions(questionAnswer, 'Please answer this required question to move to previous.'))
            return false;

        current_fs = $(this).parent();
        previous_fs = $(this).parent().prev();

        //show the previous div
        previous_fs.show();

        //hide the current div with style
        current_fs.animate({ opacity: 0 }, {
            step: function (now) {
                // for making div appear animation
                opacity = 1 - now;

                current_fs.css({
                    'display': 'none',
                    'position': 'relative'
                });
                previous_fs.css({ 'opacity': opacity });
            },
            duration: 500
        });
        setProgressBar(--current);

        --currentQuestion;
    });

    $(".submit").click(function () {

        let questionAnswer = createCurrentAnswerRequest($(".SurveyMainParent")[currentQuestion - 1]);
        if (!validateQuestions(questionAnswer))
            return false;

        var request = createSubmitRequest();

        showLoader();
        $.ajax({
            url: "/was/Survey/SubmitSurvey",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(request),
            success: function (d) {
                if (d != undefined && d != null) {
                    if (d.Success) {

                        //we dont need any buttons after clicking submit
                        $(".last_survey, .final_previous, .submit").hide();
                        //show success messege
                        $(".SurveySuccessmsg").fadeIn("500");
                        //remove animation from progress bar
                        $(".progress-bar").removeClass("progress-bar-striped", "progress-bar-animated");

                    }
                    else if (!d.Success && d.IsAlreadySubmitted) {
                        $("#idMainContent").html("");
                        $("#idSurveyCompleted").removeClass("DN");
                        $("#idAlertText").text("You have already completed this survey");
                    }
                    else
                        location.reload();
                }
                hideLoader();
            },
            error: function (errorMessage) {
                hideLoader();
            }
        });



    })
});

function createCurrentAnswerRequest(elment) {

    obj = {};
    obj["QuestionNumber"] = $($(elment)[0]).attr("questionnumber");
    obj["QuestionType"] = $($(elment)[0]).attr("questiontype");
    obj["QuestionText"] = $($(elment)[0]).find('.classQuestionText').text();
    obj["IsRequired"] = $($(elment)[0]).attr("isrequired");
    obj["shortAnswerLength"] = $($(elment)[0]).attr("shortAnswerLength");

    const commentVal = $(elment).find('.comment-txt').val();
    obj["Comments"] = commentVal != null ? commentVal : "";
    obj["Answer"] = [];

    if ($($(elment)[0]).attr("questiontype") == "1") {
        if ($($(elment)[0]).find(".classShortAnswer").val().trim() != "")
            obj["Answer"].push($($(elment)[0]).find(".classShortAnswer").val().trim());
    }
    else if ($($(elment)[0]).attr("questiontype") == "2") {
        optionName = "optionName" + obj["QuestionNumber"];
        if ($('input[name="' + optionName + '"]:checked').val() != undefined) {
            obj["Answer"].push($('input[name="' + optionName + '"]:checked').val());
        }
    }
    else if ($($(elment)[0]).attr("questiontype") == "3") {
        optionName = "optionName" + obj["QuestionNumber"];
        if ($('input[name="' + optionName + '"]:checked').val() != undefined) {
            const isOtherOptionSelected = $('input[name="' + optionName + '"]:checked').data('otherradio');
            if (isOtherOptionSelected) {
                const inputVal = $(elment).find('.other-radio-input-elmnt').val().trim();
                obj.OtherOption = inputVal;
            }
            else {
                obj["Answer"].push($('input[name="' + optionName + '"]:checked').val());
            }
        }
    }
    else if ($($(elment)[0]).attr("questiontype") == "4") {
        optionName = "optionName" + obj["QuestionNumber"];
        if ($('input[name="' + optionName + '"]:checked').length > 0) {
            $('input[name="' + optionName + '"]:checked').each(function (index, item) {
                if ($(item).data('otherchkbox')) {
                    const inputVal = $(elment).find('.other-chkbox-input-elmnt').val().trim();
                    obj.OtherOption = inputVal;
                } else {
                    obj["Answer"].push($(item).val());
                }
            });
        }
    }
    else if ($($(elment)[0]).attr("questiontype") == "5") {
        let ratingType = $($(elment)[0]).attr("ratingType");
        if (ratingType == "1" || ratingType == "2") {
            if ($($(elment)[0]).find(".ratingSelected").length > 0)
                obj["Answer"].push($($(elment)[0]).find(".ratingSelected").attr("value"));
        }
        else if (ratingType == "3") {
            if ($($(elment)[0]).find(".classSmileyActive").length > 0)
                obj["Answer"].push($($(elment)[0]).find(".classSmileyActive").attr("value"));
        }
        else if (ratingType == "4") {
            if ($($(elment)[0]).find(".sliderActive").length > 0) {
                let eleLength = $($(elment)[0]).find(".sliderActive").length;
                let maxVal = $($($(elment)[0]).find(".sliderActive")[eleLength - 1]).text().trim();
                obj["Answer"].push(maxVal);
            }
        }
    }
    return obj;
}

function validateQuestions(questionAnswer, errorMsg) {
     
    let isValid = true;
    let isShortAnswer = true;
    let isLongAnswer = true;

    if (questionAnswer.IsRequired == 'false' && (questionAnswer.QuestionType == "3" || questionAnswer.QuestionType == "4")) {
        if (questionAnswer.OtherOption != undefined && questionAnswer.OtherOption.trim() == "") {
            $(`.SurveyMainParent[questionnumber="${questionAnswer.QuestionNumber}"]`).find('.other-opt-inpt').addClass('validation-box');
            isValid = false;
        }
    }
    else if (questionAnswer.IsRequired == 'true' && (questionAnswer.QuestionType == "3" || questionAnswer.QuestionType == "4")) {
        if (questionAnswer.OtherOption != undefined && questionAnswer.OtherOption.trim() == "") {
            $(`.SurveyMainParent[questionnumber="${questionAnswer.QuestionNumber}"]`).find('.other-opt-inpt').addClass('validation-box');
            isValid = false;
            isQuestionValid = false;
        }
        else if (questionAnswer.OtherOption == undefined && questionAnswer.Answer.length <= 0) {
            $(`.questionParent[questionnumber="${questionAnswer.QuestionNumber}"]`).addClass('validation-box');
            isValid = false;
            isQuestionValid = false;
        }
    }
    else if (questionAnswer.IsRequired == 'true' && questionAnswer.Answer.length <= 0) {
        $(`.questionParent[questionnumber="${questionAnswer.QuestionNumber}"]`).addClass('validation-box');
        isValid = false;
        isQuestionValid = false;
    }
    else if (questionAnswer.IsRequired == 'true' && (questionAnswer.QuestionType == "1")) {
        if (questionAnswer.shortAnswerLength == '') {
            questionAnswer.shortAnswerLength = 10;
        }
        if (questionAnswer.Answer != null && questionAnswer.Answer[0].length < parseInt(questionAnswer.shortAnswerLength)) {
            $(`.questionParent[questionnumber="${questionAnswer.QuestionNumber}"]`).addClass('validation-box');
            isShortAnswer = false;
            isValid = false;
            isQuestionValid = false;
        }
        else if (questionAnswer.Answer != null && questionAnswer.Answer[0].length > 2000) {
            $(`.questionParent[questionnumber="${questionAnswer.QuestionNumber}"]`).addClass('validation-box');
            isLongAnswer = false;
            isValid = false;
            isQuestionValid = false;
        }
        if (!isShortAnswer) {
            showEmptyTextAlert(`Please enter minimum of ${questionAnswer.shortAnswerLength} characters`);
        }
        if (!isLongAnswer) {
            showEmptyTextAlert(`Please enter maximum of 2000 characters`);
        }
    }

   
    if (!isValid && isShortAnswer && isLongAnswer) {
        showEmptyTextAlert(errorMsg);
    }

    return isValid;
}

function setProgressBar(curStep) {
    var steps = $(".step_field").length;
    var percent = parseFloat(100 / steps) * curStep;
    percent = percent.toFixed();
    $(".progress-bar")
        .css("width", percent + "%")
}

function navigateToQuestion(element) {

    var selectedQuestionNumber = element.id;
    var errorMsg = 'Please answer this required question to move to next.';

    // Get the count of answered question by active li
    answeredQues = $('ul#progressbar li.active').length;

    if (selectedQuestionNumber == currentQuestion || selectedQuestionNumber > answeredQues) { return false; }

    // Validate question before moving to next
    if (selectedQuestionNumber < currentQuestion)
        errorMsg = 'Please answer this required question to move to previous.';
    let questionAnswer = createCurrentAnswerRequest($(".SurveyMainParent")[currentQuestion - 1]);
    if (!validateQuestions(questionAnswer, errorMsg))
        return false;

    $(".validation-box").removeClass('validation-box');

    current_fs = $("#step_field_" + currentQuestion);
    selected_fs = $("#step_field_" + selectedQuestionNumber);

    //show the selected div
    selected_fs.show();

    // Activate page numbers
    $("#progressbar li").eq($(".step_field").index(selected_fs)).addClass("active");

    //hide the current div with style
    current_fs.animate({ opacity: 0 }, {
        step: function (now) {
            // for making div appear animation
            opacity = 1 - now;

            current_fs.css({
                'display': 'none',
                'position': 'relative'
            });
            selected_fs.css({ 'opacity': opacity });
        },
        duration: 500
    });

    // set progress bar to selected question
    setProgressBar(selectedQuestionNumber);

    // set question and page selection
    currentQuestion = selectedQuestionNumber;
    current = selectedQuestionNumber;
}

