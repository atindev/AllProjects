
var createdSurvey = [];
var questionNumber = 0;
var surveyParentText = "idSurveyParent";
var surveyMainText = "idSurveyMain";
var createButtonText = "idCreateButton";
var questionBodyText = "idQuestionContent";
var multiSelectDeleteText = "idMultiSelectDelete";
var multiSelectParentText = "idMultiSelectParentContent";
var multiChoiceDeleteText = "idMultiChoiceDelete";
var multiChoiceParentText = "idMultiChoiceParentContent";
var RatingDeleteText = "idRatingDelete";
var RatingParentText = "idRatingParentContent";
var maxLength = 100;
var maxOptionCount = 12;
var minOptionCount = 3;
var singleSelectDeleteText = "idSingleSelectDelete";
var singleSelectParentText = "idSingleSelectParentContent";
var surveyMainPreviewText = "idSurveyMainPreview";
var surveyUpdateText = "idSurveyUpdate";
var updateButtonText = "idUpdateButton";
var cancelButtonText = "idUpdateCancelButton";
var optionIdGlobal = "idOptionId";
var questionRequired = "idQuesRequired";
var otherComment = "idotherComment";
var otherCommentChk = "idotherCommentChk";
var otherOptionChk = "idotherOptionChk";
var otherOptionChoice = "idotherOptionChoice";
var otherOptionChoiceInpt = "otherOptionChoiceInpt";
var CLONE = "-Clone";
var isValid = true;
const shortAnswerMaxLength = 300;

$(document).ready(function () {
    clearAllSurvey();

    $(document).on("click", "#idSurveyAddQuestion", function (event) {

        if (!checkSurveySubject())
            return false;

        else if (questionNumber == 0) {
            createShortAnswerQuestion(++questionNumber);
            return false;
        }

        if ($("#" + surveyMainText + questionNumber).find(".questionContent").length == 0 || checkForLatestQuestion()) {
            if (!checkAndAddOrUpdateUnsavedQuestion())
                return false;
            createShortAnswerQuestion(++questionNumber);
        }
    });

    $(document).on("click", "#idSaveSurvey,#idUpdateSurvey", function (event) {

        if (!checkSurveyBeforeSave(event))
            return false;

        let isCreate = $(event.currentTarget).hasClass("createNewSurey") ? true : false;
        var request = {};
        request["Subject"] = $("#idSurveySubject").val();
        request["Description"] = $("#idSurveyDescription").val();
        request["Questions"] = createdSurvey;
        if (!isCreate) {
            request["Id"] = $("#idSurveyId").val();
            request["CreatedBy"] = $("#idCreatedBy").val();
        }

        showLoader();
        $.ajax({
            url: "/was/Survey/CreateUpdateSurvey",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(request),
            success: function (d) {
                if (isCreate) {
                    if (d.Id != undefined)
                        window.location.href = '/WAS/Survey/List';
                    else
                        location.reload();
                }
                else
                    window.location.href = '/WAS/Survey/List';

                hideLoader();
            },
            error: function (errorMessage) {
                hideLoader();
            }
        });
    });

    $(document).on("click", ".classCreateButton,.classUpdateButton", function (event) {
        let currentQuestionNumber = $(event.currentTarget).attr("questionnumber");
        let isCreate = $(event.currentTarget).hasClass('classCreateButton');
        let curentQuestionContent = "";
        let surveyBody = (isCreate) ? surveyMainText : surveyUpdateText;
        let isRequired = $('#' + questionRequired + currentQuestionNumber).prop('checked');
        let isCommentsEnabled = $('#' + otherCommentChk + currentQuestionNumber).prop('checked');
        let isOtherOptionEnabled = $('#' + otherOptionChk + currentQuestionNumber).prop('checked');

        curentQuestionContent = $("#" + surveyBody + currentQuestionNumber).find(".questionContent").val();
        isValid = true;

        if (curentQuestionContent == undefined || curentQuestionContent == "") {
            showEmptyTextAlert("Please input some Question");
            isValid = false;
            if (!isCreate)
                $(".classUpdateContent").attr("tabindex", -1).focus();
            return false;
        }
        if (!checkCurrentQuestionOptions(currentQuestionNumber, !isCreate)) {
            showEmptyTextAlert("Please add all the options");
            isValid = false;
            if (!isCreate)
                $(".classUpdateContent").attr("tabindex", -1).focus();
            return false;
        }
        if (isDuplicateQuestionOptions(currentQuestionNumber, !isCreate)) {
            showEmptyTextAlert("Please remove the duplicate options");
            isValid = false;
            if (!isCreate)
                $(".classUpdateContent").attr("tabindex", -1).focus();
            return false;
        }

        let obj = {};
        obj["QuestionType"] = $("#" + surveyBody + currentQuestionNumber).find(".selectQuestionTypes").val();
        obj["Question"] = curentQuestionContent;
        obj["QuestionNumber"] = currentQuestionNumber;
        obj["Options"] = [];
        obj["ShortAnswerPlaceHolder"] = "";
        obj["ShortAnswerLength"] = "";
        obj["IsRequired"] = isRequired;
        obj['IsCommentsEnabled'] = isCommentsEnabled;
        obj['IsOtherOptionEnabled'] = isOtherOptionEnabled;

        obj["RatingType"] = 0;
        let currentRatingType = parseInt($("#" + surveyBody + currentQuestionNumber).find(".ratingTypeSelection").val());
        if (parseInt(obj["QuestionType"]) > 1) {
            if (obj["QuestionType"] == '5' && currentRatingType == '4') {
                let ansObj = {};
                ansObj["Id"] = currentQuestionNumber + "" + 0;
                ansObj["Text"] = $('.selectedSliderValue').val();
                obj["Options"].push(ansObj);
            }
            else {
                $("#" + surveyBody + currentQuestionNumber).find(".selectOptions").each(function (index, element) {
                    if ($(element).val() != undefined && $(element).val() != "") {
                        let ansObj = {};
                        ansObj["Id"] = currentQuestionNumber + "" + index;
                        ansObj["Text"] = $(element).val();
                        obj["Options"].push(ansObj);
                    }
                });
            }
        }
        if (parseInt(obj["QuestionType"]) == 1) {
            obj["ShortAnswerPlaceHolder"] = $("#" + surveyBody + currentQuestionNumber).find(".classPlaceHolder").val().trim();
            obj["ShortAnswerLength"] = $("#" + surveyBody + currentQuestionNumber).find(".classShortAnswerLength").val();
        }

        if (parseInt(obj["QuestionType"]) == 5)
            obj["RatingType"] = currentRatingType;

        if (isCreate) // create
            createdSurvey.push(obj);
        else { //update
            let objIndex = createdSurvey.findIndex((obj => obj.QuestionNumber == currentQuestionNumber));
            if (objIndex != undefined && objIndex != -1)
                createdSurvey[objIndex] = {};
            createdSurvey[objIndex] = obj;
        }

        //replacing with preview
        $("#" + (surveyParentText + currentQuestionNumber)).empty().html(getQuestionPreview(obj, false, ""));
        updatePreviewQuestionNumber();
    });

    $(document).on("change", ".selectQuestionTypes", function (event) {
        let currentVal = $(event.currentTarget).val();
        let currentQuestion = $(event.currentTarget).attr("questionnumber");
        let content = "";
        let commentHtml = "";
        let otherOptionHtml = "";

        if (currentVal == "1") {
            content += "<div class='row'><div class='col-md-6'>";
            content += "<div class='form-group'>";
            content += "<label>Placeholder</label>";
            content += "<input type='text' class='form-control classPlaceHolder' placeholder='Write Here' maxlength='" + maxLength + "'>";
            content += "</div>";
            content += "<div class='form-group'><label>Minimum length of short answer expected </label>";
            content += "<input type='text' class='form-control classShortAnswerLength'>";
            content += "</div></div></div>";//form-group,col-md-6,row
        }
        if (currentVal == "2") {
            content = createSingleSelect(currentQuestion);
        }
        else if (currentVal == "3") {
            content = createMultiplechoice(currentQuestion);
            commentHtml += getCommentsChkBoxContent(currentQuestion);
            otherOptionHtml = getOtherOptionChkBoxContent(currentQuestion);
        }
        else if (currentVal == "4") {
            content = createMultiSelectOptions(currentQuestion);
            commentHtml += getCommentsChkBoxContent(currentQuestion);
            otherOptionHtml = getOtherOptionChkBoxContent(currentQuestion);
        }
        else if (currentVal == "5") {
            content = createRating(currentQuestion);
            commentHtml += getCommentsChkBoxContent(currentQuestion);
        }
        $("#" + (questionBodyText + currentQuestion)).html(content);
        $("#" + (questionBodyText + currentQuestion)).parents('.SurveyMainParent').find('.comment-section').html(commentHtml);
        $("#" + (questionBodyText + currentQuestion)).parents('.SurveyMainParent').find('.other-option-section').html(otherOptionHtml);

    });

    $(document).on("click", ".remove_option_MultiSelect", function (event) {
        let questNumber = $(event.currentTarget).attr("questionnumber");
        let optionNumber = $(event.currentTarget).attr("optionnumber");
        $("#" + multiSelectDeleteText + questNumber + optionNumber).remove();

        updateLabelText(questNumber, "Option ");

        //add plus button
        let currentCount = $("#" + questionBodyText + questNumber).find(".classMultiSelectOptions").length;
        if (currentCount < maxOptionCount && $("#" + questionBodyText + questNumber).find(".addOptionParent").length == 0)
            appendAddOption(questNumber, "add_option_MultiSelect");
    });

    $(document).on("click", ".add_option_MultiSelect", function (event) {
        let questNumber = $(event.currentTarget).attr("questionnumber");
        let currentCount = $("#" + questionBodyText + questNumber).find(".classMultiSelectOptions").length + 1;

        let list = "";
        list += "<div class='form-group with_Delete_btn' id='" + (multiSelectDeleteText + questNumber + currentCount) + "'>";
        list += "<label class='optionLabelText'>Option " + currentCount + "</label>";
        list += "<input type='text' class='form-control selectOptions classMultiSelectOptions' placeholder='Write Here' maxlength='" + maxLength + "'/>";
        list += "<img class='remove_option remove_option_MultiSelect' src='/was/img/survey_images/Option_Remove.svg' questionnumber='" + questNumber + "' optionnumber='" + currentCount + "' alt=''  width='30' />";
        list += "</div>";//form-group

        $("#" + multiSelectParentText + questNumber).append(list);

        updateLabelText(questNumber, "Option ");

        //removing plus button
        if (currentCount == maxOptionCount)
            $("#" + questionBodyText + questNumber).find(".addOptionParent").remove();
    });

    $(document).on("click", ".remove_option_Multichoice", function (event) {
        let questNumber = $(event.currentTarget).attr("questionnumber");
        let optionNumber = $(event.currentTarget).attr("optionnumber");
        $("#" + multiChoiceDeleteText + questNumber + optionNumber).remove();

        updateLabelText(questNumber, "Option ");

        //add plus button
        let currentCount = $("#" + questionBodyText + questNumber).find(".classMultiChoiceOptions").length;
        if (currentCount < maxOptionCount && $("#" + questionBodyText + questNumber).find(".addOptionParent").length == 0)
            appendAddOption(questNumber, "add_option_Multichoice");
    });

    $(document).on("click", ".add_option_Multichoice", function (event) {
        let questNumber = $(event.currentTarget).attr("questionnumber");
        let currentCount = $("#" + questionBodyText + questNumber).find(".classMultiChoiceOptions").length + 1;

        let list = "";
        list += "<div class='form-group with_Delete_btn' id='" + (multiChoiceDeleteText + questNumber + currentCount) + "'>";
        list += "<label class='optionLabelText'>Option " + currentCount + "</label>";
        list += "<input type='text' class='form-control selectOptions classMultiChoiceOptions' placeholder='Write Here' maxlength='" + maxLength + "'/>";
        list += "<img class='remove_option remove_option_Multichoice' src='/was/img/survey_images/Option_Remove.svg' questionnumber='" + questNumber + "' optionnumber='" + currentCount + "' alt=''  width='30' />";
        list += "</div>";//form-group

        $("#" + multiChoiceParentText + questNumber).append(list);

        updateLabelText(questNumber, "Option ");

        //removing plus button
        if (currentCount == maxOptionCount)
            $("#" + questionBodyText + questNumber).find(".addOptionParent").remove();
    });

    $(document).on("click", ".remove_option_Rating", function (event) {
        let questNumber = $(event.currentTarget).attr("questionnumber");
        let optionNumber = $(event.currentTarget).attr("optionnumber");
        $("#" + RatingDeleteText + questNumber + optionNumber).remove();

        updateLabelText(questNumber);

        //add plus button
        let currentCount = $("#" + questionBodyText + questNumber).find(".classRatingOptions").length;
        if (currentCount < 5 && $("#" + questionBodyText + questNumber).find(".addOptionParent").length == 0)
            appendAddOptionRating(questNumber, "add_option_Rating");
    });

    $(document).on("click", ".add_option_Rating", function (event) {
        let questNumber = $(event.currentTarget).attr("questionnumber");
        let currentCount = $("#" + questionBodyText + questNumber).find(".classRatingOptions").length + 1;

        let list = "";
        list += "<div class='form-group OptionRating rating_Delete_btn' id='" + (RatingDeleteText + questNumber + currentCount) + "'>";
        list += "<label class='optionLabelText'>" + currentCount + "</label>";
        list += "<input type='text' class='form-control selectOptions classRatingOptions' placeholder='Write Here' maxlength='" + maxLength + "'/>";
        list += "<img class='remove_option remove_option_Rating' src='/was/img/survey_images/Option_Remove.svg' questionnumber='" + questNumber + "' optionnumber='" + currentCount + "' alt=''  width='30' />";
        list += "</div>";//form-group

        $("#" + RatingParentText + questNumber).append(list);

        updateLabelText(questNumber);

        //removing plus button
        if (currentCount == 5)
            $("#" + questionBodyText + questNumber).find(".addOptionParent").remove();
    });

    $(document).on("click", ".edit_svy_question", function (event) {

        //reseting valid status
        isValid = true;

        //before edit calling the previous unsaved question and update;
        $('.classUpdateButton').click();

        //checking for the previous question is valid
        if (!isValid)
            return;

        let currentQuestionNumber = $(event.currentTarget).attr("questionnumber");
        //replacing with edit view
        $("#" + (surveyParentText + currentQuestionNumber)).empty().html(updateQuestion(currentQuestionNumber));

        //setting the question type
        var data = createdSurvey.filter((d) => d.QuestionNumber == currentQuestionNumber);
        if (data != undefined && data.length > 0) {
            $("#" + (surveyParentText + currentQuestionNumber)).find(".selectQuestionTypes").val(data[0].QuestionType);
            //set the rating type for rating question
            if (data[0].QuestionType == "5") {
                $("#" + (surveyParentText + currentQuestionNumber)).find(".ratingTypeSelection").val(data[0].RatingType);
                if (data[0].RatingType == "4") {
                    $("#" + (surveyParentText + currentQuestionNumber)).find(".selectedSliderValue").val(data[0].Options[0].Text);
                }
            }
        }
    });

    $(document).on("click", ".classUpdateCancelButton", function (event) {
        //reseting valid 
        isValid = true;
        let currentQuestionNumber = $(event.currentTarget).attr("questionnumber");
        let obj = createdSurvey.filter((d) => d.QuestionNumber == currentQuestionNumber);
        if (obj != undefined && obj.length > 0) {
            $("#" + (surveyParentText + currentQuestionNumber)).empty().html(getQuestionPreview(obj[0], false, ""));
            updatePreviewQuestionNumber();
        }
    });

    $(document).on("click", ".delete_svy_question", function (event) {
        let currentQuestionNumber = $(event.currentTarget).attr("questionnumber");
        $("#" + (surveyParentText + currentQuestionNumber)).remove();
        createdSurvey = createdSurvey.filter((d) => { return d.QuestionNumber != currentQuestionNumber });
        updatePreviewQuestionNumber();
    });

    $(document).on("click", ".clone_svy_question", function (event) {

        let currentQuestionNumber = $(event.currentTarget).attr("questionnumber");
        let questionToClone = createdSurvey.find(sur => sur.QuestionNumber == currentQuestionNumber);
        let indexOfCurrentQues = createdSurvey.findIndex(x => x.QuestionNumber == currentQuestionNumber);

        questionToClone = Object.assign({}, questionToClone);

        questionNumber++;
        questionToClone.QuestionNumber = questionNumber;
        questionToClone.Question = `${questionToClone.Question}${CLONE}`;
        createdSurvey.splice(indexOfCurrentQues + 1, 0, questionToClone);


        const newClonedContent = "<div class='SurveyMainParent' id='" + (surveyParentText + questionToClone.QuestionNumber) + "' questionnumber='" +
            questionToClone.QuestionNumber + "'>" + getQuestionPreview(questionToClone, false, "") + "</div>";

        $("#" + (surveyParentText + currentQuestionNumber)).after(newClonedContent);
        const $parentId = $(`#${surveyParentText}${questionToClone.QuestionNumber}`);
        //set the focus to the newly cloned question.
        focusAfterClone(questionToClone, $parentId);
        updatePreviewQuestionNumber();
    });

    //for view or update page
    if ($("#idSurveyId").val() != undefined && $("#idSurveyId").val() != "") {
        getSurveyDetails();
    }

});

function clearAllSurvey() {
    createdSurvey = [];
    questionNumber = 0;
    $("#idSurveyContent").empty();
}

function focusAfterClone(questionToClone, $parentId) {

    switch (questionToClone.QuestionType) {
        case "1":
            $parentId.find('textarea').focus();
            break;
        case "2":
            $parentId.find('.switch_toggle_Custom').attr("tabindex", -1).focus();
        case "3":
            $parentId.find('.radio').eq(1).attr("tabindex", -1).focus();
            break;
        case "4":
            $parentId.find('.checkbox').eq(1).attr("tabindex", -1).focus();
            break;
        case "5":
            $parentId.find('.col-md-12').eq(1).attr("tabindex", -1).focus();
    }
}

function createShortAnswerQuestion(questionNumber) {
    list = "";
    let parentId = surveyMainText + questionNumber;
    let createButtonId = createButtonText + questionNumber;

    list += "<div class='SurveyMainParent' id='" + (surveyParentText + questionNumber) + "' questionnumber='" + questionNumber + "'>";
    list += "<div class='SurveyQuestionCreate survey_card shadow fadeInUp' id='" + parentId + "' questionnumber='" + questionNumber + "'>";
    list += getShortAnswerContent(questionNumber);
    //delete button
    list += "<div class='col-md-12 topSurveyActions'><div class='row'><div class='col-md-8'>";
    list += "<div class='checkbox'><input id='idQuesRequired" + questionNumber + "' type='checkbox' value=''><label for='idQuesRequired" + questionNumber + "'>Required</label></div>";
    list += "<div class='comment-section'></div>";
    list += "<div class='other-option-section'></div></div>";
    list += "<div class='col-md-4 text-right'>";
    list += "<button type='button' class='btn--a delete_svy_question mr-2' questionnumber='" + questionNumber + "'><img src='/was/img/survey_images/Survey_question_Delete.svg' alt='' width='20'></button>";
    list += "<button type='button' id='" + createButtonId + "' questionnumber='" + questionNumber + "' class='btn classCreateButton'>Create Question</button>";
    list += "</div></div></div>";//col-md-6,row
    list += "</div></div>";//SurveyQuestionCreate,SurveyMainParent
    $("#idSurveyContent").append(list);
}

function getShortAnswerContent(questionNumber, existingData) {
    let questionBodyId = questionBodyText + questionNumber;
    let list = "";
    list += getQuestionAndType(questionNumber, existingData);
    list += "<div class='classQuestionBody' id='" + questionBodyId + "'>";//empty row for other question contents
    list += "<div class='row'><div class='col-md-6'>";
    list += "<div class='form-group'>";
    list += "<label>Placeholder</label>";
    if (existingData != undefined)
        list += `<input type='text' class='form-control classPlaceHolder' value='${escapeHtml(existingData.ShortAnswerPlaceHolder)}' placeholder='Write Here' maxlength=${shortAnswerMaxLength}>`;
    else
        list += "<input type='text' class='form-control classPlaceHolder' placeholder='Write Here' maxlength='" + shortAnswerMaxLength + "'>";
    list += "</div>";
    list += "<div class='form-group'>";
    list += "<label>Minimum length of short answer expected</label>";
    if (existingData != undefined) {
        const shortLengthValue = (existingData.ShortAnswerLength != null && existingData.ShortAnswerLength != undefined && existingData.ShortAnswerLength != "") ? existingData.ShortAnswerLength : '10';
        list += `<input type='text' class='form-control classShortAnswerLength' value='${escapeHtml(shortLengthValue)}'>`;
    }
    else {
        list += "<input type='text' class='form-control classShortAnswerLength'>";
    }
    list += "</div></div></div>";//form-group,col-md-6,row
    list += "</div>";//classQuestionBody
    if (existingData != undefined) {
        const checked = existingData.IsRequired ? 'checked' : '';

        list += "<div class='row'><div class='col-md-6'>";
        list += "<div class='form-group'>";

        list += "<div class='commentTop d-flex'>\
                    <div class='checkbox'>\
                    <input id='idQuesRequired" + questionNumber + "' type='checkbox' value='' " + checked + ">\
                    <label for='idQuesRequired" + questionNumber + "'>Required</label>\
                </div>";

        if (existingData.QuestionType == "3" || existingData.QuestionType == "4" || existingData.QuestionType == "5") {
            list += "<div class='comment-section'>" + getCommentsChkBoxContent(questionNumber, existingData) + "</div>";
        }
        else {
            list += "<div class='comment-section'></div>\
                    <div class='other-option-section'></div>";
        }

        if (existingData.QuestionType == "3" || existingData.QuestionType == "4") {
            list += "<div class='other-option-section'>" + getOtherOptionChkBoxContent(questionNumber, existingData) + "</div>";
        }

        list += "</div>";/*comment top end*/
    }

    return list;
}

function createQuestionTypeElement(questionNumber) {
    let list = "";
    let elementId = "idSelectSurveyQuestion" + questionNumber;
    list += "<select id='" + elementId + "' class='msg_q_select selectQuestionTypes' questionnumber='" + questionNumber + "'>";
    typesOfQuestions.forEach(function (value, index) {
        list += "<option value='" + value.Id + "'>" + value.Name + "</option>";
    });
    list += "</select>";//msg_q_select
    return list;
}

function showEmptyTextAlert(text) {
    $("#idmodalsmText").text(text);
    $('#idAlertToggle').click();
}

function checkForLatestQuestion() {
    let curentQuestionContent = $("#" + surveyMainText + questionNumber).find(".questionContent").val();
    if (curentQuestionContent == undefined || curentQuestionContent == "" || !checkCurrentQuestionOptions(questionNumber)) {
        showEmptyTextAlert("Please complete the current question");
        return false;
    }
    if (isDuplicateQuestionOptions(questionNumber, false)) {
        showEmptyTextAlert("Please remove the duplicate options");
        return false;
    }
    return true;
}

function createMultiSelectOptions(questionNumber) {
    let list = "";
    let selectCount = 1;

    list += "<div class='row'>";
    list += "<div class='col-md-6' id='" + (multiSelectParentText + questionNumber) + "'>";

    list += "<div class='form-group' id='" + (multiSelectDeleteText + questionNumber + selectCount) + "'>";
    list += "<label>Option 1</label>";
    list += "<input type='text' class='form-control selectOptions classMultiSelectOptions' placeholder='Write Here' maxlength='" + maxLength + "' />";
    list += "</div>";//form-group

    selectCount++;
    list += "<div class='form-group' id='" + (multiSelectDeleteText + questionNumber + selectCount) + "'>";
    list += "<label>Option 2</label>";
    list += "<input type='text' class='form-control selectOptions classMultiSelectOptions' placeholder='Write Here' maxlength='" + maxLength + "' />";
    list += "</div>";//form-group

    selectCount++;
    list += "<div class='form-group' id='" + (multiSelectDeleteText + questionNumber + selectCount) + "'>";
    list += "<label>Option 3</label>";
    list += "<input type='text' class='form-control selectOptions classMultiSelectOptions' placeholder='Write Here' maxlength='" + maxLength + "' />";
    list += "</div>";//form-group

    list += "</div></div>";//col-md-6,row

    list += "<div class='col-md-12 addOptionParent'>";
    list += "<a class='add_option add_option_MultiSelect' questionnumber='" + questionNumber + "'><i class='fas fa-plus' ></i> Add Option</a>";
    list += "</div>";//col-md-12,

    list += getotherOptionElementContent({ rowClass: true });
    list += getCommentInputElementContent({ rowClass: true });

    return list;
}

function createMultiplechoice(questionNumber) {
    let list = "";
    let selectCount = 1;


    list += "<div class='row'>";
    list += "<div class='col-md-6' id='" + (multiChoiceParentText + questionNumber) + "'>";

    list += "<div class='form-group' id='" + (multiChoiceDeleteText + questionNumber + selectCount) + "'>";
    list += "<label>Option 1</label>";
    list += "<input type='text' class='form-control selectOptions classMultiChoiceOptions' placeholder='Write Here' maxlength='" + maxLength + "' />";
    list += "</div>";//form-group

    selectCount++;
    list += "<div class='form-group' id='" + (multiChoiceDeleteText + questionNumber + selectCount) + "'>";
    list += "<label>Option 2</label>";
    list += "<input type='text' class='form-control selectOptions classMultiChoiceOptions' placeholder='Write Here' maxlength='" + maxLength + "' />";
    list += "</div>";//form-group

    selectCount++;
    list += "<div class='form-group' id='" + (multiChoiceDeleteText + questionNumber + selectCount) + "'>";
    list += "<label>Option 3</label>";
    list += "<input type='text' class='form-control selectOptions classMultiChoiceOptions' placeholder='Write Here' maxlength='" + maxLength + "' />";
    list += "</div>";//form-group

    list += "</div></div>";//col-md-6,row

    list += "<div class='col-md-12 addOptionParent'>";
    list += "<a class='add_option add_option_Multichoice' questionnumber='" + questionNumber + "'><i class='fas fa-plus' ></i> Add Option</a>";
    list += "</div>";//col-md-6,

    list += getotherOptionElementContent({ rowClass: true });
    list += getCommentInputElementContent({ rowClass: true });

    return list;
}

function createRating(questionNumber) {
    let list = "";


    list += "<div class='row'>";

    //for rating type dropdown
    list += getRatingTypeSelection(questionNumber);

    list += "<div class='col-md-6 mt-2' id='" + (RatingParentText + questionNumber) + "'>";

    list += getRatingQuestionContent("1", questionNumber);

    list += "</div></div>";//col-md-6,row
    list += "<div class='rating-add-option-section'></div>"
    list += getCommentInputElementContent({ rowClass: true });

    return list;
}

function createSingleSelect(questionNumber, existingData) {
    let list = "";
    let selectCount = 1;

    list += "<div class='row'>";
    list += "<div class='col-md-6' id='" + (singleSelectParentText + questionNumber) + "'>";

    list += "<div class='form-group' id='" + (singleSelectDeleteText + questionNumber + selectCount) + "'>";
    list += "<div class='d-flex align-items-center'><label class='d-inline-block mr-4 mb-0 mb-0'>Option 1</label></div >";
    if (existingData != undefined)
        list += `<input type='text' class='form-control selectOptions classSingleSelectOptions' value='${escapeHtml(existingData.Options[0].Text)}' placeholder='Write Here' maxlength=${maxLength} />`;
    else
        list += "<input type='text' class='form-control selectOptions classSingleSelectOptions' placeholder='Write Here' maxlength='" + maxLength + "' />";
    list += "</div>";//form-group

    selectCount++;
    list += "<div class='form-group' id='" + (singleSelectDeleteText + questionNumber + selectCount) + "'>";
    list += "<div class='d-flex align-items-center'><label class='d-inline-block mr-4 mb-0'>Option 2</label>";
    list += "</div >";
    if (existingData != undefined)
        list += `<input type='text' class='form-control selectOptions classSingleSelectOptions' value='${escapeHtml(existingData.Options[1].Text)}' placeholder='Write Here' maxlength=${maxLength} />`;
    else
        list += "<input type='text' class='form-control selectOptions classSingleSelectOptions' placeholder='Write Here' maxlength='" + maxLength + "' />";
    list += "</div>";//form-group

    list += "</div></div>";//col-md-6,row

    return list;
}

function appendAddOption(questionNumber, typeClass) {
    let parentElement = questionBodyText + questionNumber;
    let list = "";
    list += "<div class='col-md-12 addOptionParent'>";

    list += "<a class='add_option " + typeClass + "' questionnumber='" + questionNumber + "'><i class='fas fa-plus' ></i> Add Option</a>";
    list += "</div>";//col-md-12
    $("#" + parentElement).append(list);
}
function appendAddOptionRating(questionNumber, typeClass) {
    let parentElement = questionBodyText + questionNumber;
    let list = "";
    list += "<div class='col-md-12 addOptionParent'>";

    list += "<a class='add_option " + typeClass + "' questionnumber='" + questionNumber + "'><i class='fas fa-plus' ></i> Add Option</a>";
    list += "</div>";//col-md-12
    $("#" + parentElement).find('.rating-add-option-section').append(list);
}

function updateLabelText(questionNumber, labelText) {
    labelText = (labelText == undefined) ? "" : labelText;
    let count = 0;
    $("#" + questionBodyText + questionNumber).find(".optionLabelText").each(function (index, element) {
        if ($(element) != undefined && $(element) != null) {
            count = minOptionCount + index + 1;
            $(element).text(labelText + count);
        }
    });
}

function checkCurrentQuestionOptions(questionNumber, isUpdate) {
    let flag = true;
    let element = [];
    if (isUpdate != undefined && isUpdate)
        element = $("#" + surveyUpdateText + questionNumber).find(".selectOptions");
    else
        element = $("#" + surveyMainText + questionNumber).find(".selectOptions");

    if (element != undefined && element.length > 0) {
        for (i = 0; i < element.length; i++) {
            if ($(element[i]).val() == undefined || $(element[i]).val().trim() == "") {
                flag = false;
                return;
            }
        }
    }
    else
        flag = true;

    return flag;
}

/*method to find the duplicated option*/
function isDuplicateQuestionOptions(questionNumber, isUpdate) {
    let element = [];
    if (isUpdate != undefined && isUpdate)
        element = $("#" + surveyUpdateText + questionNumber).find(".selectOptions");
    else
        element = $("#" + surveyMainText + questionNumber).find(".selectOptions");

    let optionValues = [];
    element.each(function () {
        optionValues.push($(this).val().toLowerCase());
    });

    const isDuplicate = optionValues.some((item, index) => optionValues.indexOf(item) !== index);
    return isDuplicate;
}

function getQuestionPreview(data, isReadonly, currentQuestNumber) {
    list = "";
    if (data != undefined && data.QuestionType != undefined) {
        if (data.QuestionType == "1")
            list = getShortAnswerPreview(data, isReadonly, currentQuestNumber);
        else if (data.QuestionType == "2")
            list = getOptionQuestionPreview(data, "toggleQuestionPreview", isReadonly, currentQuestNumber);
        else if (data.QuestionType == "3")
            list = getOptionQuestionPreview(data, "radioQuestionPreview", isReadonly, currentQuestNumber);
        else if (data.QuestionType == "4")
            list = getOptionQuestionPreview(data, "checkBoxQuestionPreview", isReadonly, currentQuestNumber);
        else if (data.QuestionType == "5")
            list = getOptionQuestionPreview(data, "starQuestionPreview", isReadonly, currentQuestNumber);
    }
    return list;
}

function getShortAnswerPreview(data, isReadonly, currentQuestNumber) {
    currentQuestNumber = (currentQuestNumber != undefined && currentQuestNumber != "") ? ((currentQuestNumber)) : "";
    let questionNumber = data.QuestionNumber;
    let questionText = data.Question;
    let placeHolder = (data.ShortAnswerPlaceHolder != undefined && data.ShortAnswerPlaceHolder != "") ? data.ShortAnswerPlaceHolder : "Write Here";
    let shortAnswerLength = (data.ShortAnswerLength != undefined && data.ShortAnswerLength != "") ? data.ShortAnswerLength : "10";
    list = "";
    list += "<div class='SurveyShortAnswer classPreviewContent survey_card shadow fadeInUp' id='" + (surveyMainPreviewText + questionNumber) + "' questionnumber='" + questionNumber + "'>";

    list += "<div class='row'>";
    const requiredViewData = data.IsRequired ? { checked: 'checked', required: 'required' } : '';


    list += "<div class='col-md-6'>";
    list += "<div class='form-group mb-0 d-flex align-items-center'>";
    list += "<span class='classQuestionNumber'>" + currentQuestNumber + "</span>";
    list += `<label class='classQuestionText text-break ${requiredViewData.required}'>${escapeHtml(questionText)}</label></div>`;
    list += `<div class='col-md-12'><textarea class='form-control' rows='3' placeholder='${escapeHtml(placeHolder)}' maxlength='0'></textarea></div>`;
    //list += "</div>";//form-group
    list += "</div>";//col-md-6


    list += "</div>";//row

    if (!isReadonly) {
        list += "<div class='col-md-12 topSurveyActions'><div class='row'><div class='col-md-3'>";
        list += "<div class='checkbox'><input id='idQuesRequired" + questionNumber + "' type='checkbox' value='' " + requiredViewData.checked + " disabled><label for='idQuesRequired" + questionNumber + "'>Required</label></div></div>";
        list += "<div class='col-md-3'><label class='classshortAnswerLength mt-1'>" + "Minimum Length : " + shortAnswerLength + "</label></div>";
        list += "<div class='col-md-6 text-right'><button type='button' class='btn--a edit_svy_question' questionnumber='" + questionNumber + "'><img src='/was/img/survey_images/Survey_Question_Edit.svg' alt='' width='20'></button>";
        list += "<button type='button' class='btn--a delete_svy_question' questionnumber='" + questionNumber + "'><img src='/was/img/survey_images/Survey_question_Delete.svg' alt='' width='20'></button>";
        list += "<button type='button' class='btn--a clone_svy_question' title='Clone Question' questionnumber='" + questionNumber + "'><i class='far fa-clone' aria-hidden='true'></i></button>";
        list += "</div></div></div>";//col-md-6
    }


    list += "</div>";//SurveyShortAnswer
    return list;
}

function getOptionQuestionPreview(data, className, isReadonly, currentQuestNumber) {
    currentQuestNumber = (currentQuestNumber != undefined && currentQuestNumber != "") ? ((currentQuestNumber)) : "";
    let questionNumber = data.QuestionNumber;
    let questionText = data.Question;
    list = "";
    list += "<div class='" + className + " classPreviewContent survey_card shadow fadeInUp' id='" + (surveyMainPreviewText + questionNumber) + "' questionnumber='" + questionNumber + "'>";
    const requiredViewData = data.IsRequired ? { checked: 'checked', required: 'required' } : '';
    const comntConfigData = { disabled: "disabled", ...data };
    const showCommentsContent = data.IsCommentsEnabled ? "display:block" : "display:none";



    list += "<div class='row option-parent-row'>";
    list += "<div class='col-md-12'>";
    list += "<div class='form-group mb-0 d-flex align-items-center'>";
    list += "<span class='classQuestionNumber'>" + currentQuestNumber + "</span>";
    list += `<label class='classQuestionText text-break ${requiredViewData.required}'>${escapeHtml(questionText)}</label>`;
    list += "</div></div>";//col-md-12,form-group

    if (data.QuestionType == "2") {
        list += "<div class='col-md-12'><div class='d-flex'>";
        data.Options.forEach(function (v, i) {
            optionName = "optionName_" + questionNumber;
            currentOptionId = optionIdGlobal + questionNumber + i;
            list += `<div class='radio mr-3'><label class='custom_radio' for=${currentOptionId}> ${escapeHtml(v.Text)}`;
            list += `<input type='radio' id=${currentOptionId} name = ${optionName} value= ${escapeHtml(v.Text)} /><span class='checkmark'></span>`;
            list += "</label></div>";
        });
        list += "</div></div>";
    }
    else if (data.QuestionType == "5") {
        list += getRatingMainPreview(data);
        list += getCommentInputElementContent({ style: showCommentsContent, width: 'w-100' });
    }
    else if (data.QuestionType == "3" || data.QuestionType == "4") {
        let optionName = "", currentOptionId = "";
        let otherOptionInputId = otherOptionChoiceInpt + questionNumber;
        let otherOptionInputConfig = { otherOptionInputId: otherOptionInputId, show: false };

        data.Options.forEach(function (v, i) {
            optionName = "optionName_" + questionNumber;
            currentOptionId = optionIdGlobal + questionNumber + i;
            otherOptionInputConfig.otherOptionId = currentOptionId;

            if (data.QuestionType == "4") {
                list += "<div class='col-md-12'><div class='checkbox'>";
                list += `<input type='checkbox' id='${currentOptionId}' name='${optionName}' value='${escapeHtml(v.Text)}' onclick='showOrHideOtherOptionMultiselectInput(${JSON.stringify(otherOptionInputConfig)})'/><label for='${currentOptionId}'>${escapeHtml(v.Text)}</label>`;
                list += "</div></div>";//checkbox,col-md-12
            }
            else {
                list += `<div class='col-md-12'><div class='radio'><label class='custom_radio' for='${currentOptionId}'>${escapeHtml(v.Text)}`;
                list += `<input type='radio' id='${currentOptionId}' name='${optionName}'  value='${escapeHtml(v.Text)}' onclick='showOrHideOtherOptionChoiceInput(${JSON.stringify(otherOptionInputConfig)})'/><span class='checkmark'></span>`;
                list += "</label></div></div>";//radio,col-md-12
            }
        });

        if (data.QuestionType == "3" && data.IsOtherOptionEnabled) {

            otherOptionInputConfig.show = true;
            list += `<div class='col-md-12'>\
	                    <div class='col-md-12 row'>\
		                    <div class='radio'>\
			                    <label class='custom_radio' for='${otherOptionChoice + questionNumber}'>Others\
			                    <input type='radio' id='${otherOptionChoice + questionNumber}' name='${optionName}' value='Others' onclick='showOrHideOtherOptionChoiceInput(${JSON.stringify(otherOptionInputConfig)})'>\
			                    <span class='checkmark'></span>\
			                    </label>\
		                    </div>\
		                    <div class='col-md-8'>\
			                    <input type='text' id='${otherOptionInputId}' class='form-control' maxlength='0' placeholder='Write Here' style='display: none;'>\
		                    </div>\
	                    </div>\
                    </div>`;
        }
        else if (data.QuestionType == "4" && data.IsOtherOptionEnabled) {
            otherOptionInputConfig.IsOtherOption = true;
            otherOptionInputConfig.otherOptionId = otherOptionChoice + questionNumber;

            list += `<div class='col-md-12'>\
	                    <div class='row'>\
		                    <div class='checkbox col-md-4'>\
			                    <input type='checkbox' id='${otherOptionChoice + questionNumber}' name='${optionName}' class='cls-other-option-chk' value='Others' onclick='showOrHideOtherOptionMultiselectInput(${JSON.stringify(otherOptionInputConfig)})'>\
                                <label for='${otherOptionChoice + questionNumber}'>Others</label>\
		                    </div>\
		                    <div class='col-md-8'>\
			                    <input type='text' id='${otherOptionInputId}' class='form-control' maxlength='0' placeholder='Write Here' style='display: none;'>\
		                    </div>\
	                    </div>\
                    </div>`;

        }

        list += getCommentInputElementContent({ style: showCommentsContent, width: 'w-100' });
    }


    list += "</div>";//row


    if (!isReadonly) {
        list += "<div class='col-md-12 topSurveyActions'>";
        list += "<div class='row'><div class='col-md-8'>";
        list += "<div class='checkbox'><input id='idQuesRequired" + questionNumber + "' type='checkbox' value='' " + requiredViewData.checked + " disabled><label for='idQuesRequired" + questionNumber + "'>Required</label></div>";
        if (data.QuestionType != 2) {
            list += "<div class='comment-section'>" + getCommentsChkBoxContent(questionNumber, comntConfigData) + "</div>";
        }
        if (data.QuestionType == "3" || data.QuestionType == "4") {
            list += "<div class='other-option-section'>" + getOtherOptionChkBoxContent(questionNumber, comntConfigData) + "</div>";
        }

        list += "</div><div class='col-md-4 text-right'>";
        list += "<button type='button' class='btn--a edit_svy_question' questionnumber='" + questionNumber + "'><img src='/was/img/survey_images/Survey_Question_Edit.svg' alt='' width='20'></button>";
        list += "<button type='button' class='btn--a delete_svy_question' questionnumber='" + questionNumber + "'><img src='/was/img/survey_images/Survey_question_Delete.svg' alt='' width='20'></button>";
        list += "<button type='button' class='btn--a clone_svy_question' title='Clone Question' questionnumber='" + questionNumber + "'><i class='far fa-clone' aria-hidden='true'></i></button>";
        list += "</div>";//col-md-12
        list += "</div></div>";//row
    }


    list += "</div>";//SurveyShortAnswer
    return list;
}

function updateQuestion(questionNumber) {
    list = "";
    if (questionNumber != undefined) {
        //finding question from the list
        var data = createdSurvey.filter((d) => d.QuestionNumber == questionNumber);
        if (data != undefined && data.length > 0) {
            if (data[0].QuestionType == "1")
                list = getShortAnswerUpdate(data[0]);
            else
                list = getOptionAnswerUpdate(data[0]);
        }
    }
    return list;
}

function getShortAnswerUpdate(data) {
    let questionNumber = data.QuestionNumber;
    list = "";
    list += "<div class='SurveyShortAnswerUpdate classUpdateContent survey_card shadow fadeInUp' id='" + (surveyUpdateText + questionNumber) + "' questionnumber='" + questionNumber + "'>";
    list += getShortAnswerContent(questionNumber, data);
    list += getActionButtonContent(questionNumber);
    list += "</div>";//SurveyShortAnswerUpdate
    return list;
}

function getOptionAnswerUpdate(data) {
    let questionNumber = data.QuestionNumber;
    list = "";
    var existingData = data
    list += "<div class='SurveyOptionUpdate classUpdateContent survey_card shadow fadeInUp' id='" + (surveyUpdateText + questionNumber) + "' questionnumber='" + questionNumber + "'>";
    list += getQuestionAndType(questionNumber, data);

    list += "<div class='classQuestionBody' id='" + (questionBodyText + questionNumber) + "'>";
    if (data.QuestionType == "2")
        list += createSingleSelect(questionNumber, data);
    else if (data.QuestionType == "5")
        list += createRatingQuestionContent(questionNumber, data);
    else
        list += createMultiOptionContent(questionNumber, data);
    list += "</div>";//classQuestionBody

    if (existingData != undefined) {
        const checked = existingData.IsRequired ? 'checked' : '';

        list += "<div class='commentTop d-flex'>\
                    <div class='checkbox'>\
                    <input id='idQuesRequired" + questionNumber + "' type='checkbox' value='' " + checked + ">\
                    <label for='idQuesRequired" + questionNumber + "'>Required</label>\
                </div>";

        if (existingData.QuestionType == "3" || existingData.QuestionType == "4" || existingData.QuestionType == "5") {
            list += "<div class='comment-section'>" + getCommentsChkBoxContent(questionNumber, existingData) + "</div>";
        }
        else {
            list += "<div class='comment-section'></div>\
                    <div class='other-option-section'></div>";
        }

        if (existingData.QuestionType == "3" || existingData.QuestionType == "4") {
            list += "<div class='other-option-section'>" + getOtherOptionChkBoxContent(questionNumber, existingData) + "</div>";
        }

        list += "</div>";/*comment top end*/
    }


    list += getActionButtonContent(questionNumber);
    list += "</div>";//SurveyOptionUpdate
    return list;
}

function getActionButtonContent(questionNumber) {
    let list = "";
    list += "<div class='row'>";
    list += "<div class='col-md-12 text-right'>";
    list += "<button type='button' id='" + (updateButtonText + questionNumber) + "' questionnumber='" + questionNumber + "' class='btn btn-outline-success classUpdateButton'><i class='fas fa-plus mr-2' aria-hidden='true'></i>Update</button>";
    list += "<button type='button' id='" + (cancelButtonText + questionNumber) + "' questionnumber='" + questionNumber + "' class='btn btn-outline-success classUpdateCancelButton btn-outline-danger'><i class='fas fa-times mr-2' aria-hidden='true'></i>Cancel</button>";
    list += "</div>";//col-md-12
    list += "</div>";//row
    return list;
}

function getQuestionAndType(questionNumber, existingData) {

    let list = "";
    list += "<div class='row'>";
    list += "<div class='col-md-6 position-static'>";
    //for update purpose
    if (existingData != undefined) {
        list += `<div class='form-group'><label class='required'>Question</label><input type='text' class='form-control questionContent' value='${escapeHtml(existingData.Question)}' placeholder='Write Here' maxlength=${shortAnswerMaxLength}></div>`;
    }
    else
        list += "<div class='form-group'><label class='required'>Question</label><input type='text' class='form-control questionContent' placeholder='Write Here' maxlength='" + shortAnswerMaxLength+ "'></div>";
    list += "</div>";//col-md-6
    list += "<div class='col-md-6'>";
    list += "<div class='form-group'>";
    list += "<label class='required'>Question Type</label>";
    list += createQuestionTypeElement(questionNumber);
    list += "</div>";//form-group
    list += "</div>";//col-md-6
    list += "</div>";//row

    return list;

}

function createMultiOptionContent(questionNumber, data) {
    let list = "";
    let selectCount = 1;
    let showCmntSection = data.IsCommentsEnabled ? "display:block" : "display:none";
    const showOtherOptionContent = data.IsOtherOptionEnabled ? "display:block" : "display:none";

    let parentText = "", selectDeleteText = "", multiSelectOptions = "", addOption = "", deleteOption = "";
    if (data.QuestionType == "3") {
        parentText = multiChoiceParentText;
        selectDeleteText = multiChoiceDeleteText;
        multiSelectOptions = "classMultiChoiceOptions";
        addOption = "add_option_Multichoice";
        deleteOption = "remove_option_Multichoice";
    }
    else if (data.QuestionType == "4") {
        parentText = multiSelectParentText;
        selectDeleteText = multiSelectDeleteText;
        multiSelectOptions = "classMultiSelectOptions";
        addOption = "add_option_MultiSelect";
        deleteOption = "remove_option_MultiSelect";
    }
    else if (data.QuestionType == "5") {
        parentText = RatingParentText;
        selectDeleteText = RatingDeleteText;
        multiSelectOptions = "classRatingOptions";
    }



    list += "<div class='row'>";
    list += "<div class='col-md-6' id='" + (parentText + questionNumber) + "'>";

    data.Options.forEach(function (v, i) {
        if (data.QuestionType == "5") {
            if (i > 2)
                list += "<div class='form-group OptionRating rating_Delete_btn' id='" + (selectDeleteText + questionNumber + selectCount) + "'>";
            else
                list += "<div class='form-group OptionRating' id='" + (selectDeleteText + questionNumber + selectCount) + "'>";
            list += "<label>" + (i + 1) + "</label>";

        }
        else {
            list += "<div class='form-group' id='" + (selectDeleteText + questionNumber + selectCount) + "'>";
            list += "<label>Option " + (i + 1) + "</label>";
        }
        list += `<input type='text' class='form-control selectOptions ${multiSelectOptions}' value='${escapeHtml(v.Text)}' placeholder='Write Here' maxlength=${maxLength} />`;

        if (data.QuestionType == "5" && i > 2)
            list += "<img class='remove_option remove_option_Rating' src='/was/img/survey_images/Option_Remove.svg' questionnumber='" + questionNumber + "' optionnumber='" + selectCount + "' alt=''  width='30' />";
        else if ((data.QuestionType == "3" || data.QuestionType == "4") && (i > minOptionCount - 1))
            list += "<img class='remove_option " + deleteOption + "' src='/was/img/survey_images/Option_Remove.svg' questionnumber='" + questionNumber + "' optionnumber='" + selectCount + "' alt=''  width='30' />";

        list += "</div>";//form-group
        ++selectCount;
    });

    list += "</div></div>";//col-md-6,row

    if ((data.QuestionType == "3" || data.QuestionType == "4") && (data.Options.length < maxOptionCount)) {
        list += "<div class='col-md-12 addOptionParent'>";
        list += "<a class='add_option " + addOption + "' questionnumber='" + questionNumber + "'><i class='fas fa-plus' ></i> Add Option</a>";
        list += "</div>";//col-md-12,
    }
    else if ((data.QuestionType == "5") && (data.Options.length < 5)) {
        list += "<div class='col-md-12 addOptionParent'>";
        list += "<a class='add_option add_option_Rating' questionnumber='" + questionNumber + "'><i class='fas fa-plus' ></i> Add Option</a>";
        list += "</div>";//col-md-12,
    }

    if ((data.QuestionType == "5")) {
        list += "<div class='rating-add-option-section'></div>";
    }
    if (data.QuestionType == "3" || data.QuestionType == "4")
        list += getotherOptionElementContent({ rowClass: true, style: showOtherOptionContent });

    if (data.QuestionType == "3" || data.QuestionType == "4" || data.QuestionType == "5")
        list += getCommentInputElementContent({ rowClass: true, style: showCmntSection });

    return list;
}

function checkSurveySubject() {
    if ($("#idSurveySubject").val() == undefined || $("#idSurveySubject").val().trim() == "") {
        showEmptyTextAlert("Please provide survey title");
        return false;
    }
    return true;
}

function checkSurveyBeforeSave(event) {
    if (!checkSurveySubject())
        return false;

    if ($(".classPreviewContent ").length == 0 && event.currentTarget.id == "idSaveSurvey" || createdSurvey.length == 0 && event.currentTarget.id== "idSaveSurvey") {
        showEmptyTextAlert("Create atleast one question.");
        return false;
    }
    if ($(".classPreviewContent ").length == 0 && event.currentTarget.id == "idUpdateSurvey" || createdSurvey.length == 0 && event.currentTarget.id == "idUpdateSurvey") {
        showEmptyTextAlert("Please complete all the questions.");
        return false;
    }
    if ($(".SurveyQuestionCreate").length > 0 || $(".classUpdateContent").length > 0) {
        showEmptyTextAlert("Please complete all the questions.");
        return false;
    }

    return true;
}

function updatePreviewQuestionNumber() {
    let value = 0;
    $(".classQuestionNumber").each(function (index, element) {
        if ($(element) != undefined && $(element) != null) {
            value = index + 1;
            $(element).text(value);
        }
    });
}

function commentsOnClick(id) {

    let checked = $(id).prop('checked')
    if (checked) {
        $(id).parents('.SurveyMainParent').find('.div-comment-input').show();
    }
    else {
        $(id).parents('.SurveyMainParent').find('.div-comment-input').hide();
    }
}

function showOrHideOtherOptionChoiceInput(otherOptionInputConfig) {
    if (otherOptionInputConfig != undefined && otherOptionInputConfig.show) {
        $('#' + otherOptionInputConfig.otherOptionInputId).show();
    }
    else if (otherOptionInputConfig != undefined && !otherOptionInputConfig.show) {
        $('#' + otherOptionInputConfig.otherOptionInputId).hide();
    }
}
function showOrHideOtherOptionMultiselectInput(otherOptionInputConfig) {
    let isOtherOptionChecked = false;

    if (otherOptionInputConfig != undefined && otherOptionInputConfig.IsOtherOption) {
        isOtherOptionChecked = $('#' + otherOptionInputConfig.otherOptionId).prop('checked');
    }
    else {
        isOtherOptionChecked = $('#' + otherOptionInputConfig.otherOptionId).parents('.option-parent-row').find('.cls-other-option-chk').prop('checked');
    }

    if (isOtherOptionChecked) {
        $('#' + otherOptionInputConfig.otherOptionInputId).show();
    } else {
        $('#' + otherOptionInputConfig.otherOptionInputId).hide();
    }
}

function getCommentsChkBoxContent(questionNum, comntConfig) {
    let checked = (comntConfig != undefined && comntConfig.IsCommentsEnabled) ? "checked" : "";
    let disabled = comntConfig != undefined && comntConfig.disabled ? "disabled" : "";

    return "<div class='checkbox'>\
            <input id = '" + otherCommentChk + questionNum + "' type = 'checkbox' value = '' onclick='commentsOnClick(" + otherCommentChk + questionNum + ")' " + checked + " " + disabled + ">\
            <label for='" + otherCommentChk + questionNum + "'>Enable Comments</label>\
            </div > ";
}

function getCommentInputElementContent(config) {

    let row = (config && config.rowClass) ? 'row' : '';
    let style = (config && config.style) ? config.style : 'display:none';
    let width = (config && config.width) ? 'w-100' : '';

    //row section for other comments
    let content = "";
    content += "<div class='" + row + " div-comment-input mt-1 " + width + "' style='" + style + "'>";
    content += "<div class='col-md-6 form-group'>";
    content += "<label>Comments</label>";
    content += "<input type='text' class='form-control' placeholder='Write Here' maxlength='0' />";
    content += "</div>";
    content += "</div>";

    return content;
}

function getOtherOptionChkBoxContent(questionNum, config) {

    let checked = (config != undefined && config.IsOtherOptionEnabled) ? "checked" : "";
    let disabled = config != undefined && config.disabled ? "disabled" : "";

    return "<div class='checkbox'>\
            <input id = '" + otherOptionChk + questionNum + "' type = 'checkbox' value = '' onclick='enableOrDisableOtherOptionChkBox(" + otherOptionChk + questionNum + ")' " + checked + " " + disabled + ">\
            <label for='" + otherOptionChk + questionNum + "'>Enable other option</label>\
            </div > ";
}

function getotherOptionElementContent(config) {

    let row = (config && config.rowClass) ? 'row' : '';
    let style = (config && config.style) ? config.style : 'display:none';
    let width = (config && config.width) ? 'w-100' : '';

    //row section for other comments
    let content = "";
    content += "<div class='" + row + " div-other-option mt-1 " + width + "' style='" + style + "'>";
    content += "<div class='col-md-6 form-group'>";
    content += "<label>Others</label>";
    content += "<input type='text' class='form-control' placeholder='Write Here' maxlength='0' />";
    content += "</div>";
    content += "</div>";

    return content;
}

function enableOrDisableOtherOptionChkBox(id) {
    let checked = $(id).prop('checked');

    if (checked) {
        $(id).parents('.SurveyMainParent').find('.div-other-option').show();
    }
    else {
        $(id).parents('.SurveyMainParent').find('.div-other-option').hide();
    }
}

/*checking for the unsaved question and create if exists*/
function checkAndAddOrUpdateUnsavedQuestion() {
 
    $('.SurveyQuestionCreate').find('.classCreateButton').click();

    $('.classUpdateContent').find('.classUpdateButton').click();
    if (isValid) {
        return true;
    }
    else {
        return false;
    }
}

/*survey view and update*/
function getSurveyDetails() {

    showLoader();
    $.ajax({
        url: "/was/Survey/GetSurveyById",
        type: "POST",
        data: {
            Id: "" + ($("#idSurveyId").val())
        },
        success: function (d) {

            let isReadonly = true;
            if (d != undefined && d != null) {
                let data = JSON.parse(d);
                let surveyOwner = (data.CreatedBy != undefined && data.CreatedBy != "") ? data.CreatedBy.trim().toLowerCase() : "";
                $("#idCreatedBy").val(data.CreatedBy);
                $("#idSurveySubjectParent").append("<label class='required'>Subject</label>");
                $("#idDescriptionLabel").text("Description");
                if ($("#idCurrentUser").val() != undefined && $("#idCurrentUser").val() != "" && $("#idCurrentUser").val() == surveyOwner) {
                    //subject and Description
                    $("#idSurveyHead").append("Edit Survey");
                    $("#idSurveySubjectParent").append(`<input type='text' class='form-control' value='${escapeHtml(data.Subject)}' placeholder='Subject' id='idSurveySubject' maxlength='100'>`);
                    $("#idSurveyDescriptionParent").append("<textarea class='form-control' rows='3' id='idSurveyDescription' maxlength='1000'>" + data.Description + "</textarea>");

                    $("#idSurveyActionParent").append("<button type='button' class='btn--a add_new_post' id='idSurveyAddQuestion'><img src='/was/img/survey_images/Survey_Add_Question.svg' alt='' width='30'> Add Question</button>");
                    $("#idSurveyActionParent").append("<button type='button' class='btn--a updateSurey' id='idUpdateSurvey'>Update Survey</button>");
                    isReadonly = false;
                }
                else {
                    $("#idSurveyHead").append(data.Subject);
                    $("#idSurveySubjectParent").append("<span>" + data.Subject + "</span>");
                    $("#idSurveyDescriptionParent").append("<span>" + data.Description + "</span>");

                }
                if (data.Questions != undefined && data.Questions.length > 0)
                    createdSurvey = data.Questions;
                bindExistingQuestion(isReadonly);
            }
            hideLoader();
        },
        error: function (errorMessage) {
            hideLoader();
        }
    });
}
function bindExistingQuestion(isReadonly) {
    let list = "";
    let isUpdate = (isReadonly) ? "PaddingAdjust" : "";
    createdSurvey.forEach(function (v, i) {
        list += "<div class='SurveyMainParent " + isUpdate + "' id='" + (surveyParentText + v.QuestionNumber) + "' questionnumber='" + v.QuestionNumber + "'>";
        list += getQuestionPreview(v, isReadonly, i + 1);
        list += "</div>";//SurveyMainParent
    });
    $("#idSurveyContent").append(list);
    questionNumber = findMaximumFromList(createdSurvey);
}

function findMaximumFromList(data) {
    if (data == undefined || data.length == 0)
        return 0;
    let maximum = data[0].QuestionNumber;
    data.forEach(function (v, i) {
        if (parseInt(v.QuestionNumber) >= parseInt(maximum))
            maximum = v.QuestionNumber;
    });
    return maximum;
}