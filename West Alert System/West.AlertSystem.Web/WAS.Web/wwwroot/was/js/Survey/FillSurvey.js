
$(document).ready(function () {

    $(document).on("click", ".classStarImage", function (event) {
        currentIndex = $(event.currentTarget).attr("currentindex");
        oldIndex = $(".ratingSelected").attr("currentindex");

        if  ($(event.currentTarget).hasClass("ratingSelected")) {
                  $($(event.currentTarget).parentsUntil(".star_option_preview").parent()[0]).find(".ratingSelected").removeClass("ratingSelected");
            }
            else {
                  $($(event.currentTarget).parentsUntil(".star_option_preview").parent()[0]).find(".ratingSelected").removeClass("ratingSelected");
                  $(event.currentTarget).addClass("ratingSelected");
            }
    });

    //to remove the validation class in element click
    $('.answer-entry').click(function () {
        $(this).parents('.SurveyMainParent').removeClass('validation-box');
        //for wizard
        $(this).parents('.questionParent').removeClass('validation-box');
    });

    $('.other-opt-inpt').click(function () {
        $(this).removeClass('validation-box');
    });

    $('.classMultiChoice').click(function () {

        const isOtherRadio = $(this).data("otherradio");
        if (isOtherRadio) {
            $(this).parents('.other-radio-parent-div').find('.other-radio-input-div').show();
        }
        else {
            $(this).parents('.radioQuestionPreview').find('.other-radio-input-div').hide();
        }
    })

    $('.classMultiSelect').click(function () {

        let isOtherChnkBoxSelected = false;
        isOtherChnkBoxSelected = $(this).data('otherchkbox') && $(this).prop('checked');
        if (!isOtherChnkBoxSelected) {
            const currentOptions = $(this).parents('.checkBoxQuestionPreview').find('.classMultiSelect');
            currentOptions.each(function ()
            {
                if ($(this).data('otherchkbox') && $(this).prop('checked')) {
                    isOtherChnkBoxSelected = true;
                }
            });
        }

        if (isOtherChnkBoxSelected) {
            $(this).parents('.checkBoxQuestionPreview').find('.other-chkbox-input-div').show();
        } else {
            $(this).parents('.checkBoxQuestionPreview').find('.other-chkbox-input-div').hide();
        }
    });

    $(document).on("click", "#idSubmitSurvey", function (event) {
        var request = createSubmitRequest();

        if (!validateQuestions(request))
            return false;

        saveSurveyAnswers(request);
    });

    $(document).on("click", ".classSmiley", function (event) {
        if ($(event.currentTarget).hasClass("classSmileyActive")) {
            $($(event.currentTarget).parentsUntil(".star_option_preview").parent()[0]).find(".classSmileyActive").removeClass("classSmileyActive");
        }
        else {
            $($(event.currentTarget).parentsUntil(".star_option_preview").parent()[0]).find(".classSmileyActive").removeClass("classSmileyActive");
            $(event.currentTarget).addClass("classSmileyActive");

        }
    });

    $(document).on("click", ".classSliderPart", function (event) {
        var elements = $($(event.currentTarget).parentsUntil(".star_option_preview").parent()[0]).find(".classSliderPart");
        var currentText = parseInt($(event.currentTarget).text());

        if ($(event.currentTarget).hasClass("sliderActive")) {
            $(event.currentTarget).removeClass("sliderActive");
            //removing all succeding class
            for(var i = currentText; i < elements.length; i++)
                $(elements[i]).removeClass("sliderActive");
        }
        else {
            $($(event.currentTarget).parentsUntil(".star_option_preview").parent()[0]).find(".sliderActive").removeClass("sliderActive");
            currentText = currentText - 1;
            elements.each(function (index, element) {
                if(index<=currentText)
                   $(element).addClass("sliderActive");
            });

        }
    });

});

function createSubmitRequest() {

    let request = {};
    request["BroadcastId"] = $("#idBroadcastId").val();
    request["Email"] = $("#idCurrentUser").val();
    request["SurveyStartTime"] = $("#idSurveyStartTime").val();
    let obj = {};
    request["Answers"] = [];
    let optionName = "";
    $(".SurveyMainParent").each(function (index, elment) {

        obj = {};
        obj["QuestionNumber"] = $($(elment)[0]).attr("questionnumber");
        obj["QuestionType"] = $($(elment)[0]).attr("questiontype");
        obj["QuestionText"] = $($(elment)[0]).find('.classQuestionText').text();
        obj["IsRequired"] = $($(elment)[0]).attr("isrequired");
        obj["shortAnswerLength"] = $($(elment)[0]).attr("shortAnswerLength");
        //elapsed time only for wizard 
        if ($($(elment)[0]).attr("timetaken") != undefined && $($(elment)[0]).attr("timetaken") != "")
            obj["ElapsedTime"] = $($(elment)[0]).attr("timetaken");

        const commentVal = $(elment).find('.comment-txt').val();
        obj["Comments"] = commentVal != null ? commentVal : "";
        obj["Answer"] = [];

        if ($($(elment)[0]).attr("questiontype") == "1") {
            if ($($(elment)[0]).find(".classShortAnswer").val().trim()!="")
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
                if (isOtherOptionSelected ) {
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
                    } else
                    {
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
        request["Answers"].push(obj);
    });

    return request;
}

function showLoader() {
    $('.divLoader').show();
}

function hideLoader() {
    $('.divLoader').hide();
}

function clearAllSelection() {
    $(".classShortAnswer").val("");
    $(".classBooleanAnswer").prop('checked', false);
    $(".classMultiChoice").prop('checked', false);
    $(".classMultiSelect").prop('checked', false);
    $(".ratingSelected").removeClass("ratingSelected");
    $(".sliderActive").removeClass("sliderActive");
    $(".classSmileyActive").removeClass("classSmileyActive");
    $(".validation-box").removeClass('validation-box');
    $(".comment-txt").val("");
    $(".other-opt-inpt").val("");
    $(".other-opt-div").hide();
    $('.other-opt-inpt').removeClass('validation-box');
}

function validateQuestions(submittedData) {

    let isValid = true;
    let isShortAnswer = true;
    let isLongAnswer = true;

    for (const questionAnswer of submittedData.Answers) {
        let isQuestionValid = true;

        if (questionAnswer.IsRequired == 'false' && (questionAnswer.QuestionType == "3" || questionAnswer.QuestionType == "4"))
        {
            if (questionAnswer.OtherOption != undefined && questionAnswer.OtherOption.trim() == "") {
                $(`.SurveyMainParent[questionnumber="${questionAnswer.QuestionNumber}"]`).find('.other-opt-inpt').addClass('validation-box');
                isValid = false;
            }
        }
        else if (questionAnswer.IsRequired == 'true' && (questionAnswer.QuestionType == "3" || questionAnswer.QuestionType == "4"))
        {
            if (questionAnswer.OtherOption != undefined && questionAnswer.OtherOption.trim() == "") {
                $(`.SurveyMainParent[questionnumber="${questionAnswer.QuestionNumber}"]`).find('.other-opt-inpt').addClass('validation-box');
                isValid = false;
                isQuestionValid = false;
            }
            else if (questionAnswer.OtherOption == undefined && questionAnswer.Answer.length <= 0) {
                $(`.SurveyMainParent[questionnumber="${questionAnswer.QuestionNumber}"]`).addClass('validation-box');
                isValid = false;
                isQuestionValid = false;
            }
        }
        else if (questionAnswer.IsRequired == 'true' && questionAnswer.Answer.length <= 0) {
            $(`.SurveyMainParent[questionnumber="${questionAnswer.QuestionNumber}"]`).addClass('validation-box');
            isValid = false;
            isQuestionValid = false;
        }
        else if (questionAnswer.IsRequired == 'true' && (questionAnswer.QuestionType == "1")) {
            if (questionAnswer.shortAnswerLength == '') {
                questionAnswer.shortAnswerLength = 10;
            }
            if (questionAnswer.Answer != null && questionAnswer.Answer[0].length < parseInt(questionAnswer.shortAnswerLength)) {
                $(`.SurveyMainParent[questionnumber="${questionAnswer.QuestionNumber}"]`).addClass('validation-box');
                isShortAnswer = false;
                isQuestionValid = false;
                isValid = false;
            }
            else if (questionAnswer.Answer != null && questionAnswer.Answer[0].length > 2000) {
                $(`.SurveyMainParent[questionnumber="${questionAnswer.QuestionNumber}"]`).addClass('validation-box');
                isQuestionValid = false;
                isValid = false;
                isLongAnswer = false;
            }
        }

        if (isQuestionValid)
            $(`.SurveyMainParent[questionnumber="${questionAnswer.QuestionNumber}"]`).removeClass('validation-box');
        if (!isShortAnswer) {
            showEmptyTextAlert(`Please enter minimum of ${questionAnswer.shortAnswerLength} characters`);
        }
        if (!isLongAnswer) {
            showEmptyTextAlert(`Please enter maximum of 2000 characters`);
        }
    }
    
    if (!isValid && isShortAnswer && isLongAnswer) {
        showEmptyTextAlert('Please answer all the mandatory questions.');
    }
    return isValid;
}
function saveSurveyAnswers(submittedData) {
        showLoader();
        $.ajax({
            url: "/was/Survey/SubmitSurvey",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(submittedData),
            success: function (d) {
                if (d != undefined && d != null) {
                    if (d.Success) {
                        $("#idMainContent").html("");
                        $("#idSurveyCompleted").removeClass("DN");
                        $("#idAlertText").text("You have successfully completed this survey");
                    }
                    else if (!d.Success && d.IsAlreadySubmitted) {
                        $("#idMainContent").html("");
                        $("#idSurveyCompleted").removeClass("DN");
                        $("#idAlertText").text("You have already completed this survey");
                    }
                }
                hideLoader();
            },
            error: function (errorMessage) {
                hideLoader();
            }
        });
}
function showEmptyTextAlert(text) {
    $("#idmodalsmText").text(text);
    $('#idAlertToggle').click();
}