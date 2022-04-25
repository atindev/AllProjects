$(document).ready(function () {

    $(document).on("change", ".ratingTypeSelection", function (event) {
        let currentVal = $(event.currentTarget).val();
        let currentQuestion = $(event.currentTarget).attr("questionnumber");
        let content = getRatingQuestionContent(currentVal, currentQuestion);
        $("#" + (RatingParentText + currentQuestion)).html(content);
    });
   
    $(document).on("blur", ".classSliderContent", function (event) {
        if ($(event.currentTarget).val() == "")
            return;
        if (parseInt($(event.currentTarget).val()) > 10)
            $(event.currentTarget).val("10");
        if (parseInt($(event.currentTarget).val()) < 5)
            $(event.currentTarget).val("5");
    });

});

function getRatingQuestionContent(type, questionNumber) {
    if (type == "1" || type == "2")
        return getStarRatingContent(type, questionNumber);
    else if (type == "3")
        return getSmilyRatingContent(questionNumber);
    else if (type == "4")
        return getSliderContent(questionNumber);
    else
        return "";
}

function getStarRatingContent(type, questionNumber) {
    let selectCount = 1;
    let contentArray = (type == "1") ? worstToExcellent : disagreeToAgree;
    let list = "";

    list += "<div class='form-group OptionRating' id='" + (RatingDeleteText + questionNumber + selectCount) + "'>";
    list += "<label>1</label>";
    list += "<input type='text' class='form-control selectOptions classRatingOptions' value='" + contentArray[selectCount - 1] + "' maxlength='" + maxLength + "' />";
    list += "</div>";//form-group

    selectCount++;
    list += "<div class='form-group OptionRating' id='" + (RatingDeleteText + questionNumber + selectCount) + "'>";
    list += "<label>2</label>";
    list += "<input type='text' class='form-control selectOptions classRatingOptions' value='" + contentArray[selectCount - 1] + "' maxlength='" + maxLength + "' />";
    list += "</div>";//form-group

    selectCount++;
    list += "<div class='form-group OptionRating' id='" + (RatingDeleteText + questionNumber + selectCount) + "'>";
    list += "<label>3</label>";
    list += "<input type='text' class='form-control selectOptions classRatingOptions' value='" + contentArray[selectCount - 1] + "' maxlength='" + maxLength + "'  />";
    list += "</div>";//form-group

    selectCount++;
    list += "<div class='form-group OptionRating rating_Delete_btn' id='" + (RatingDeleteText + questionNumber + selectCount) + "'>";
    list += "<label class='optionLabelText'>4</label>";
    list += "<input type='text' class='form-control selectOptions classRatingOptions' value='" + contentArray[selectCount - 1] + "' maxlength='" + maxLength + "' />";
    list += "<img class='remove_option remove_option_Rating' src='/was/img/survey_images/Option_Remove.svg' questionnumber='" + questionNumber + "' optionnumber='" + selectCount + "' alt=''  width='30' />";
    list += "</div>";//form-group

    selectCount++;
    list += "<div class='form-group OptionRating rating_Delete_btn' id='" + (RatingDeleteText + questionNumber + selectCount) + "'>";
    list += "<label class='optionLabelText'>5</label>";
    list += "<input type='text' class='form-control selectOptions classRatingOptions' value='" + contentArray[selectCount - 1] + "' maxlength='" + maxLength + "' />";
    list += "<img class='remove_option remove_option_Rating' src='/was/img/survey_images/Option_Remove.svg' questionnumber='" + questionNumber + "' optionnumber='" + selectCount + "' alt=''  width='30' />";
    list += "</div>";//form-group

    return list;
}

function getSmilyRatingContent(questionNumber) {
    let selectCount = 1;
    let list = "";

    list += "<div class='form-group optionRatingSmily " + smileyClass[selectCount - 1] + "' id='" + (RatingDeleteText + questionNumber + selectCount) + "'>";
    list += "<label>1</label>";
    list += "<input type='text' class='form-control selectOptions classRatingOptions' value='" + smileyContent[selectCount - 1] + "' maxlength='" + maxLength + "' />";
    list += "</div>";//form-group

    selectCount++;
    list += "<div class='form-group optionRatingSmily " + smileyClass[selectCount - 1] + "' id='" + (RatingDeleteText + questionNumber + selectCount) + "'>";
    list += "<label>2</label>";
    list += "<input type='text' class='form-control selectOptions classRatingOptions' value='" + smileyContent[selectCount - 1] + "' maxlength='" + maxLength + "' />";
    list += "</div>";//form-group

    selectCount++;
    list += "<div class='form-group optionRatingSmily " + smileyClass[selectCount - 1] + "' id='" + (RatingDeleteText + questionNumber + selectCount) + "'>";
    list += "<label>3</label>";
    list += "<input type='text' class='form-control selectOptions classRatingOptions' value='" + smileyContent[selectCount - 1] + "' maxlength='" + maxLength + "'  />";
    list += "</div>";//form-group

    selectCount++;
    list += "<div class='form-group optionRatingSmily " + smileyClass[selectCount - 1] + "' id='" + (RatingDeleteText + questionNumber + selectCount) + "'>";
    list += "<label>4</label>";
    list += "<input type='text' class='form-control selectOptions classRatingOptions' value='" + smileyContent[selectCount - 1] + "' maxlength='" + maxLength + "'  />";
    list += "</div>";//form-group

    selectCount++;
    list += "<div class='form-group optionRatingSmily " + smileyClass[selectCount - 1] + "' id='" + (RatingDeleteText + questionNumber + selectCount) + "'>";
    list += "<label>5</label>";
    list += "<input type='text' class='form-control selectOptions classRatingOptions' value='" + smileyContent[selectCount - 1] + "' maxlength='" + maxLength + "'  />";
    list += "</div>";//form-group

    return list;
}

function getSliderContent(questionNumber) {
    let list = "";
    list += "<div class='form-group'><label class='required'>Maximum Value</label>";
    list += "<select class='msg_q_select selectedSliderValue'>";
    sliderRatingValues.forEach(function (value, index) {
        list += "<option value='" + value.Id + "'>" + value.Name + "</option>";
    });
    return list;
}

//preview

function getRatingMainPreview(data) {
    if (data.RatingType == 1 || data.RatingType == 2)
        return getStarRatingView(data);
    else if (data.RatingType == 3)
        return getSmileyRatingView(data);
    else if (data.RatingType == 4)
        return getSliderRatingView(data);
    else
        return "";
}

function getStarRatingView(data) {
    let list = "";
    list += "<div class='col-md-12'><div class='row star_option_preview'>";
    data.Options.forEach(function (v, i) {
        list += "<div class='col'><div class='text-center'>";
        list += `<img src='/was/img/survey_images/Rating_Scale_big.svg' alt='' /><span>${escapeHtml(v.Text)}</span>`;
        list += "</div></div>";//text-center,col
    });
    list += "</div></div>";//row,col-md-12
    return list;
}

function getSmileyRatingView(data) {
    let list = "";
    list += "<div class='col-md-12'><div class='row star_option_preview'>";
    data.Options.forEach(function (v, i) {
        list += "<div class='col'><div class='text-center'>";
        list += `<img src='/was/img/${smileyImages[i]}' alt='' /><span>${escapeHtml(v.Text)}</span>`;
        list += "</div></div>";//text-center,col
    });
    list += "</div></div>";//row,col-md-12
    return list;
}

function getSliderRatingView(data) {
    let list = "";
    list += "<div class='col-md-12'><div class='row star_option_preview sliderPreviewMain'>";
    list += "<div class='classSliderParent'>";
    let calculatedWidth = 100 / parseInt(data.Options[0].Text);
    for (var i = 0; i < parseInt(data.Options[0].Text); i++) {
        list += "<div class='classSliderPart' style='width:" + calculatedWidth + "%'>" + (i + 1) + "";
        if (i < (parseInt(data.Options[0].Text)-1))
          list += "<div class='sliderBar'></div>";
        list += "</div>";//classSliderPart
    }
    list += "</div>";//classSliderParent

    list += "<div class='classSliderParentText'>";
    list += "<div class='classSliderTextContent'>Not at all likely</div>";
    list += "<div class='classSliderTextContent classSliderRightText'>Extremely likely</div>";
    list += "</div>";//classSliderParentText

    list += "</div></div>";//row,col-md-12
    return list;
}

function getRatingTypeSelection(questionNumber) {

    let list = "";
    list += "<div class='col-md-6 mt-2'><div class='form-group'>";
    list += "<label class='required'>Rating Type</label>";
    list += "<select class='msg_q_select ratingTypeSelection' questionnumber='" + questionNumber + "'>";
    typesOfRatingQuestions.forEach(function (value, index) {
        list += "<option value='" + value.Id + "'>" + value.Name + "</option>";
    });
    list += "</select>";//msg_q_select
    list += "</div></div>";//form-group,col-md-6

    return list;
}

function createRatingQuestionContent(questionNumber, data) {
    let list = "";
    let selectCount = 1;
    let showCmntSection = data.IsCommentsEnabled ? "display:block" : "display:none";
    let parentText = "", selectDeleteText = "", multiSelectOptions = "", addOption = "", deleteOption = "";
    parentText = RatingParentText;
    selectDeleteText = RatingDeleteText;
    multiSelectOptions = "classRatingOptions";

    //row section for other comments
    list += "<div class='row div-comment-input w-100' style='" + showCmntSection + "'>";
    list += "<div class='col-md-6 form-group'>";
    list += "<label>Comments</label>";
    list += "<input type='text' class='form-control' maxlength='" + maxLength + "' />";
    list += "</div>";
    list += "</div>";

    list += "<div class='row'>";

    //for rating type selection
    list += getRatingTypeSelection(questionNumber);

    list += "<div class='col-md-6' id='" + (parentText + questionNumber) + "'>";

    //slider
    if (data.RatingType == 4) {
        list += "<div class='form-group'><label class='required'>Maximum Value</label>";
        list += "<select class='msg_q_select selectedSliderValue'>";
        sliderRatingValues.forEach(function (value, index) {
            list += "<option value='" + value.Id + "'>" + value.Name + "</option>";
        });
        list += "</select>";//msg_q_select
        list += "</div>";//form-group
    }
    else {
        data.Options.forEach(function (v, i) {
            if (data.RatingType == 1 || data.RatingType == 2) {
                if (i > 2)
                    list += "<div class='form-group OptionRating rating_Delete_btn' id='" + (selectDeleteText + questionNumber + selectCount) + "'>";
                else
                    list += "<div class='form-group OptionRating' id='" + (selectDeleteText + questionNumber + selectCount) + "'>";

                list += "<label>" + (i + 1) + "</label>";

                list += `<input type='text' class='form-control selectOptions ${multiSelectOptions}' value='${escapeHtml(v.Text)}' placeholder='Write Here' maxlength=${maxLength} />`;

                if (i > 2)
                    list += "<img class='remove_option remove_option_Rating' src='/was/img/survey_images/Option_Remove.svg' questionnumber='" + questionNumber + "' optionnumber='" + selectCount + "' alt=''  width='30' />";

                list += "</div>";//form-group
            }
            //for smiley
            else {
                list += "<div class='form-group optionRatingSmily " + smileyClass[selectCount - 1] + "' id='" + (selectDeleteText + questionNumber + selectCount) + "'>";
                list += "<label>" + (i + 1) + "</label>";
                list += `<input type='text' class='form-control selectOptions ${multiSelectOptions}' value='${escapeHtml(v.Text)}' placeholder='Write Here' maxlength=${maxLength} />`;
                list += "</div>";//form-group
            }
            ++selectCount;
        });
    }

    list += "</div></div>";//col-md-6,row

    //for star type
    if ((data.RatingType == 1 || data.RatingType == 2) && (data.Options.length < 5)) {
        list += "<div class='col-md-12 addOptionParent'>";
        list += "<a class='add_option add_option_Rating' questionnumber='" + questionNumber + "'><i class='fas fa-plus' ></i> Add Option</a>";
        list += "</div>";//col-md-12,
    }

    list += getCommentInputElementContent({ style: showCmntSection, width: 'w-100'});

    return list;
}