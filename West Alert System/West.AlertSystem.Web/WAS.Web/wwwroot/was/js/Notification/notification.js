
//for hide and show the group count element
let groupElementArrayMapping = [{ id: "#TextCheckbox", element: ".classSMSSubscribers" },
    { id: "#VoiceCheckbox", element: ".classVoiceSubscribers" },
    { id: "#EmailCheckbox", element: ".classEmailSubscribers" },
    { id: "#WhatsappCheckbox", element: ".classWhatsAppSubscribers" }];

var notificationContentElement = ["MessageText", "VoiceMessage", "EmailBody", "WhatsAppMessage"];

var isTemplateSelected = false;
var selectedTemplateContent = {};

var $whatsappremainingchars = $('#whatsappremainingchars');
var $remaining = $('#remaining'),
    $messages = $('#messages')

var alertboxevent="";
function SMSModified() {
    var Message = $('textarea#MessageText').val();
    
    if ($("#VoiceCheckbox").prop("checked") && isTemplateSelected == true || $("#EmailCheckbox").prop("checked") && isTemplateSelected == true || $("#WhatsappCheckbox").prop("checked") && isTemplateSelected == true) {
        
        $('#modalselection').modal('show');
        var r = "";
        $("#okbutton").click(function () {
            r = $(this).data('id');//get the id of the selected button
            
            if (r) {
                $("#VoiceMessage").val(Message);
                $('#EmailBody').val(Message);
                $('#WhatsAppMessage').val(Message);
                $("#MessageText").val(Message);
            }
        });
       
    }
}
function VoiceModified() {
    var Message = $("textarea#VoiceMessage").val();
   
    if ($("#TextCheckbox").prop("checked") && isTemplateSelected == true || $("#EmailCheckbox").prop("checked") && isTemplateSelected == true || $("#WhatsappCheckbox").prop("checked") && isTemplateSelected == true) {
        $('#modalselection').modal('show');
        var r = "";
        $("#okbutton").click(function () {
            r = $(this).data('id');//get the id of the selected button
        if (r) {
            $("#MessageText").val(Message);
            $('#EmailBody').val(Message);
            $('#WhatsAppMessage').val(Message);
            $("#VoiceMessage").val(Message);
           }
        });
    }
}
function EmailModified() {
    var Message = $("textarea#EmailBody").val();
    
    if ($("#TextCheckbox").prop("checked") && isTemplateSelected == true || $("#VoiceCheckbox").prop("checked") && isTemplateSelected == true || $("#WhatsappCheckbox").prop("checked") && isTemplateSelected == true) {
        $('#modalselection').modal('show');
        var r = "";
        $("#okbutton").click(function () {
            r = $(this).data('id');//get the id of the selected button
        if (r) {
            $("#MessageText").val(Message);
            $('#VoiceMessage').val(Message);
            $('#WhatsAppMessage').val(Message);
            $('#EmailBody').val(Message);
           }
        });
    }
}
function WhatsappModified() {
    var Message = $("textarea#WhatsAppMessage").val();
    
    if ($("#TextCheckbox").prop("checked") && isTemplateSelected == true || $("#VoiceCheckbox").prop("checked") && isTemplateSelected == true || $("#EmailCheckbox").prop("checked") && isTemplateSelected == true) {
        $('#modalselection').modal('show');
        var r = "";
        $("#okbutton").click(function () {
            r = $(this).data('id');
            if (r) {
                $("#MessageText").val(Message);
                $('#VoiceMessage').val(Message);
                $('#EmailBody').val(Message);
                $('#WhatsAppMessage').val(Message);
            }
        });
    }
}
function optionselect() {
    var groupselected = $("#groupselected").val();
    if(groupselected!=null && groupselected!="")
        $('#idSelectGroups option[value="' + groupselected + '"]').prop('selected', true).trigger('change');
}

$(document).ready(function () {
    showLoader();
    sessionStorage.setItem("currentModule", "");
    isTemplateSelected = false;
    selectedTemplateContent = {};
    $('.classGroupCount').hide();
    clearGroupsDropdown();
    $("#idSelectedAttachmentText").hide();
    $("#idselectForPrivateApprover").hide();
    $("#CombineSaveMenu").hide();
    //Initialize Select2 Elements
    $('.select2').select2();

    //Initialize Select2 Elements
    $('.select2bs4').select2({
        theme: 'bootstrap4'
    })
   
    //Bootstrap Duallistbox
    $('.duallistbox').bootstrapDualListbox()

    $('.my-colorpicker2').on('colorpickerChange', function (event) {
        $('.my-colorpicker2 .fa-square').css('color', event.color.toString());
    });

    $("input[data-bootstrap-switch]").each(function () {
        $(this).bootstrapSwitch('state', $(this).prop('checked'));
    });
    
});

$(document).ready(function () {
    $.fn.extend({
        treed: function (o) {

            var openedClass = 'glyphicon-minus-sign';
            var closedClass = 'glyphicon-plus-sign';

            if (typeof o != 'undefined') {
                if (typeof o.openedClass != 'undefined') {
                    openedClass = o.openedClass;
                }
                if (typeof o.closedClass != 'undefined') {
                    closedClass = o.closedClass;
                }
            };

            //initialize each of the top levels
            var tree = $(this);
            tree.addClass("tree");
            tree.find('li').has("ul").each(function () {
                var branch = $(this); //li with children ul
                branch.prepend("<i class='indicator glyphicon " + closedClass + "'></i>");
                branch.addClass('branch');
                branch.on('click', function (e) {
                    if (this == e.target) {
                        var icon = $(this).children('i:first');
                        icon.toggleClass(openedClass + " " + closedClass);
                        $(this).children().children().toggle();
                    }
                })
                branch.children().children().toggle();
            });
            //fire event from the dynamically added icon
            tree.find('.branch .indicator').each(function () {
                $(this).on('click', function () {
                    $(this).closest('li').click();
                });
            });
            //fire event to open branch if the li contains an anchor instead of text
            tree.find('.branch>a').each(function () {
                $(this).on('click', function (e) {
                    $(this).closest('li').click();
                    e.preventDefault();
                });
            });
            //fire event to open branch if the li contains a button instead of text
            tree.find('.branch>button').each(function () {
                $(this).on('click', function (e) {
                    $(this).closest('li').click();
                    e.preventDefault();
                });
            });
        }
    });

    //Initialization of treeviews

    $('#tree1').treed();

    $('#tree2').treed({ openedClass: 'glyphicon-folder-open', closedClass: 'glyphicon-folder-close' });

    $('#tree3').treed({ openedClass: 'glyphicon-chevron-right', closedClass: 'glyphicon-chevron-down' });
});

$(document).ready(function () {

    $('.custom-file-input').on('change', function () {
        var fileName = $(this).val().split("\\").pop();
        $(this).siblings('.custom-file-label').addClass('selected').html(fileName);
    });

    $('#MessageText').keyup(function () {
        if (checkNotificationTypeSelection()) {
            showEmptyTextAlert("Select any Notification Type");
            return false;
        }
        //checking for atleast one publish selection
        if (!checkPublishSelection()) {
            showEmptyTextAlert("Select atleast one publishing Type");
            $('#MessageText').val("");
            return false;
        }

        setTextMessageCharacterCount(this.value);
    });

    $('#VoiceMessage').keyup(function () {
        $('#voiceEnteredChar').text(500-this.value.length);
    });

    $('#EmailBody').keyup(function () {
        $('#emailEnteredChar').text(this.value.length);
    });

    $('#WhatsAppMessage').keypress(function (event) {

        var keycode = (event.keyCode ? event.keyCode : event.which);
        if (keycode == '13') {
            return false;
        }
        event.stopPropagation();
    });

    $('#WhatsAppMessage').keyup(function () {
        //checking for atleast one publish selection
        if (!checkPublishSelection()) {
            showEmptyTextAlert("Select atleast one publishing Type");
            $('#WhatsAppMessage').val("");
            return false;
        }

        setWhatsAppCharacterCount(this.value);
        setWhatsappFinalMessage();
    });

    $('#WhatsAppTemplate').change(function () {
        GetWhatsAppTemplate();
    });

    $('#WhatsAppTab').click(function () {
        $('#WhatsAppMessage').focus();
    });

    $(document).on("change", ".classAttachmentsParent", function () {
        updateSelectedEmailAttachments();
    });

    //$(document).on("click", ".template_select > li.has-children > a", function (e) {
    //    $(".contentParent").removeClass("open");
    //    $(e.currentTarget).parent().addClass("open");
    //    var current_dropdown = $(this).parent().children('ul.sub-menu');
    //    if ($(this).parent().children('ul.sub-menu').is(':visible') != true) {
    //        $("ul.sub-menu").not(current_dropdown).slideUp();
    //        current_dropdown.slideToggle();
    //        e.preventDefault();
    //        return false;
    //    }
    //});
    $(document).on('click', '.template_select > li.has-children > a', function (e) {
        e.preventDefault();

        let $this = $(this);

        if ($this.next().hasClass('show')) {
            $this.next().removeClass('show');
            $this.parent().removeClass("open");
            $this.next().slideUp(350);
        } else {
            $this.parent().parent().find('li .sub-menu').removeClass('show');
            $this.parent().parent().find('li .sub-menu').slideUp(350);
            $this.parent().parent().find('li').removeClass("open");
            $this.next().toggleClass('show');
            $this.parent().addClass("open");
            $this.next().slideToggle(350);
        }
    });
    $(document).on("change", ".notificationContentClass", function (event) {

        let currentId = $(event.currentTarget).attr("id");
        let currentvalue = $(event.currentTarget).val();
        setMessageContentByDefault(currentId, currentvalue);

    });

});

$(document).ready(function () {

    alertboxevent = "";
    //getting current browser timezone
    $('#idCreatedTimeZone').val(moment.tz.guess());
    setTextBasedOnApproval();

    $('#idSendWithoutApproval').change(function (event) {
        setTextBasedOnApproval();
    });

    $('#idIsPrvateNotifiation').change(function (event) {
        setPrivateNotificationText();
    });

    $('#idIsSignatureRequired').change(function (event) {
        setNotificationSignature();
    });

    $('#TextTab').hide();
    $('#VoiceTab').hide();
    $('#EmailTab').hide();
    $('#WhatsAppTab').hide();

    $("#TextCheckbox").click(function () {
        if ($("#TextCheckbox").prop('checked')) {
            $("#TextTab").show().click();
        }
        if (!$("#TextCheckbox").prop('checked')) {
            $("#TextTab").hide();
            if (!$("#VoiceCheckbox").prop('checked') && !$("#EmailCheckbox").prop('checked') && !$("#WhatsappCheckbox").prop('checked')) {
                $("#TextTab").hide();
            }
            else if ($("#EmailCheckbox").prop('checked') && !$("#VoiceCheckbox").prop('checked') && !$("#WhatsappCheckbox").prop('checked')) {
                $("#EmailTab").click();
            }
            else if ($("#EmailCheckbox").prop('checked') && $("#EmailTab").prop('ariaSelected') === "true") {
                $("#EmailTab").click();
            }
            else if ($("#WhatsappCheckbox").prop('checked') && $("#WhatsAppTab").prop('ariaSelected') === "true") {
                $("#WhatsAppTab").click();
            }
            else {
                $("#VoiceTab").click();
            }
        }
    });
    $("#VoiceCheckbox").click(function () {

        if ($("#VoiceCheckbox").prop('checked')) {
            $("#VoiceTab").show().click();
        }
        if (!$("#VoiceCheckbox").prop('checked')) {
            $("#VoiceTab").hide();
            if (!$("#TextCheckbox").prop('checked') && !$("#EmailCheckbox").prop('checked') && !$("#WhatsappCheckbox").prop('checked')) {
                $("#TextTab").hide().click();
            }
            else if ($("#TextCheckbox").prop('checked') && !$("#EmailCheckbox").prop('checked') && !$("#WhatsappCheckbox").prop('checked')) {
                $("#TextTab").click();
            }
            else if ($("#TextCheckbox").prop('checked') && $("#TextTab").prop('ariaSelected') === "true") {
                $("#TextTab").click();
            }
            else if ($("#WhatsappCheckbox").prop('checked') && $("#WhatsAppTab").prop('ariaSelected') === "true") {
                $("#WhatsAppTab").click();
            }
            else {
                $("#EmailTab").click();
            }
        }
    });
    $("#EmailCheckbox").click(function () {

        if ($("#EmailCheckbox").prop('checked')) {
            $("#EmailTab").show().click();
        }
        if (!$("#EmailCheckbox").prop('checked')) {
            $("#EmailTab").hide();
            if (!$("#TextCheckbox").prop('checked') && !$("#VoiceCheckbox").prop('checked') && !$("#WhatsappCheckbox").prop('checked')) {
                $("#TextTab").hide().click();
            }
            else if ($("#TextCheckbox").prop('checked') && !$("#VoiceCheckbox").prop('checked') && !$("#WhatsappCheckbox").prop('checked')) {
                $("#TextTab").click();
            }
            else if ($("#TextCheckbox").prop('checked') && $("#TextTab").prop('ariaSelected') === "true") {
                $("#TextTab").click();
            }
            else if ($("#WhatsappCheckbox").prop('checked') && $("#WhatsAppTab").prop('ariaSelected') === "true") {
                $("#WhatsAppTab").click();
            }
            else {
                $("#VoiceTab").click();
            }
        }
    });

    $("#WhatsappCheckbox").click(function () {
        if ($("#WhatsappCheckbox").prop('checked')) {
            $("#WhatsAppTab").show().click();
        }
        if (!$("#WhatsappCheckbox").prop('checked')) {
            $("#WhatsAppTab").hide();
            if (!$("#TextCheckbox").prop('checked') && !$("#VoiceCheckbox").prop('checked') && !$("#EmailCheckbox").prop('checked')) {
                $("#TextTab").hide().click();
            }
            else if ($("#TextCheckbox").prop('checked') && !$("#VoiceCheckbox").prop('checked') && !$("#EmailCheckbox").prop('checked')) {
                $("#TextTab").click();
            }
            else if ($("#TextCheckbox").prop('checked') && $("#TextTab").prop('ariaSelected') === "true") {
                $("#TextTab").click();
            }
            else {
                $("#EmailTab").click();
            }
        }
    });

    $('.classNotificationType').click(function () {
        hideGroupCountElements();
    });

    

    $('#idSelectPeople').change(function (event, source) {
        if (source != undefined && source == "fromTemplate")
            return false;

        let totalCount = 0, emailCount = 0, smsCount = 0, voiceCount = 0, whatsappCount = 0;

        setGroupCount(totalCount, smsCount, emailCount, voiceCount, whatsappCount);

        if ($("#idSelectPeople").find("option:selected").length == 0) {
            $('.classGroupCount').hide();
            return false;
        }
        let peopleList = {};
        peopleList["Ids"] = $("#idSelectGroups").val();
        showLoader();
        peopleList["SubscriptionIds"] = $("#idSelectPeople").val();

        GetPeopleAndGroupCount(peopleList);
           
    });

    $('#idSelectGroups').change(function (event, source) {

        if (source != undefined && source == "fromTemplate")
            return false;

        let totalCount = 0, emailCount = 0, smsCount = 0, voiceCount = 0, whatsappCount = 0;

        setGroupCount(totalCount, smsCount, emailCount, voiceCount, whatsappCount);

        if ($(this).find("option:selected").length == 0) {
            $('.classGroupCount').hide();
            return false;
        }
        let peopleList = {};
        peopleList["Ids"] = $("#idSelectGroups").val();
        peopleList["IgnoreQueryFilters"] = true;
        showLoader();
        peopleList["SubscriptionIds"] = $("#idSelectPeople").val();

        GetPeopleAndGroupCount(peopleList);
    });

    $('#idselectPrivateApprover').change(function (event) {
        if ($('#idselectPrivateApprover').val().length > 1 && $('#idselectForPrivateApprover .select2-selection__choice__remove').length > 1) {
            let prevValue = $("#idselectPrivateApprover").attr("previousvalue");
            let completeSelection = $('#idselectPrivateApprover').val();
            let index = completeSelection.indexOf(prevValue);
            if (index > -1) {
                completeSelection.splice(index, 1);
            }
            let currentValue = completeSelection[0];
            $("#idselectPrivateApprover").attr("previousvalue", currentValue);
            let prevContent = $("#idselectPrivateApprover option[value='" + prevValue + "']").attr("content");
            $('#idselectForPrivateApprover .select2-selection__choice:contains("' + prevContent + '") .select2-selection__choice__remove').click();

        }
        else
            $("#idselectPrivateApprover").attr("previousvalue", event.currentTarget.value);
    });

    


    function ReviewNotification() {
        $('#modal-send').modal('show');
    }

    $('#SendResponse').click(function (e) {
        $(".classforApproval").trigger('click', 'showpopup');
    });

    $('.classPublishselectCheck').click(function (e,source) {
        
        if (source != undefined && source == "showpopup") {
            return true;
        }
        else {
        setEvetNameForNewEvents();
        if (checkNotificationTypeSelection()) {
            showEmptyTextAlert("Select any Notification Type");
            return false;
        }

        if (!checkPublishSelection()) {
            showEmptyTextAlert("Select atleast one publishing Type");
            return false;
        }
        removeSpaceFromWhatsAppMessage();
        let alertContent = checkNotificationContent();
        if (alertContent != "") {
            showEmptyTextAlert(alertContent);
            return false;
        }

        if (!isNotificationContentLengthValid()) {
            return false;
        }
        if (!checkWhatsappTemplateSelection()) {
            return false;
        }
        if (!checkUserGroupSelection()) {
            return false;
        }
        if (checkApproverSelection()) {
            showEmptyTextAlert("Select an approver for private notification");
            return false;
        }
        if (e.currentTarget.id == "schedule") {
            $('#modal-lg').modal('show');
            return false;
        }
                var groupdropdown = document.getElementById("idSelectGroups");
                var peopledropdown = document.getElementById("idSelectPeople");
                var selectedGroups = "";
                var selectedPeoples = "";
                for (var i = 0; i < groupdropdown.length; i++) {
                    if (groupdropdown[i].selected) {
                        selectedGroups += "<span class='chipdesign'><i class='fas fa-users'></i>" + groupdropdown[i].text+ "</span>";
                    }
                }
                for (var i = 0; i < peopledropdown.length; i++) {
                    if (peopledropdown[i].selected) {
                        selectedPeoples += "<span class='chipdesign ChipPeopleColor'><i class='fas fa-user'></i>" + peopledropdown[i].text + "</span>";
                    }
                }
                var groups = selectedGroups.slice(0, -2);
                var peoples = selectedPeoples.slice(0, -2);
                var y = document.getElementById("Modal-PeopleName");
                var x = document.getElementById("Modal-GroupName");
                x.innerHTML = groups;
                y.innerHTML = peoples;
                ReviewNotification();
        }
        return false;
    });

    $('.saveAsTemplate').click(function (e) {
        if (checkNotificationTypeSelection()) {
            showEmptyTextAlert("Select any Notification Type");
            return false;
        }

        if (!checkPublishSelection()) {
            showEmptyTextAlert("Select atleast one publishing Type");
            return false;
        }
        clearTemplatepopup("new");
    });

    function createTemplateRequest() {
        
        let request = {};

        if ($("#modal-mg").attr("value") == "existing")
            request["Id"] = $(".selectedTemplate").attr("value");

        request["Name"] = $("#idTemplateName").val();
        request["CreatedBy"] = $("#idTemplateCreatedBy").val();
        request["ModifiedBy"] = $("#idTemplateCreatedBy").val();
        request["Description"] = $("#idTemplateDescription").val();

        if ($("#idCreateCategoryNewly").attr("isselected") == "true" && $("#idNewCategoryName").val() != "") {
            request["CategoryId"] = 0;
            request["CategoryName"] = $("#idNewCategoryName").val();
        }
        else
            request["CategoryId"] = parseInt($("#idTemplatesCatList").val());

        //Notification selection
        if ($("#NotificationRadio").prop("checked")) {
            request["IsNotification"] = true;
        }
        else {
            request["IsNotification"] = false;
        }

        //publishing Types and message content
        if ($("#TextCheckbox").prop("checked")) {
            request["IsText"] = true;
            if ($("#MessageText").val() != "")
                request["MessageText"] = $("#MessageText").val();
        }

        if ($("#EmailCheckbox").prop("checked")) {
            request["IsEmail"] = true;
            if ($("#idEmailSubject").val() != "")
                request["EmailSubject"] = $("#idEmailSubject").val();
            if ($("#EmailBody").val() != "")
                request["EmailBody"] = $("#EmailBody").val();

            //selected attachment from existing template
            request["ExistingEmailAttachments"] = getSelectedAttachmentFromTemplate();
        }
       
        if ($("#VoiceCheckbox").prop("checked")) {
            request["IsVoice"] = true;
            if ($("#VoiceMessage").val() != "")
                request["VoiceMessage"] = $("#VoiceMessage").val();
            if ($("#VoiceLanguage").val() != "")
                request["VoiceLanguage"] = $("#VoiceLanguage").val();
            if ($(".form-control[name='VoiceRepeatCount']").val() != "")
                request["VoiceRepeatCount"] = parseInt($(".form-control[name='VoiceRepeatCount']").val());
        }

        if ($("#WhatsappCheckbox").prop("checked")) {
            request["IsWhatsApp"] = true;
            if ($("#WhatsAppTemplate").val() != "")
                request["WhatsAppTemplateId"] = parseInt($("#WhatsAppTemplate").val());
            if ($("#WhatsAppMessage").val() != "")
                request["WhatsAppMessage"] = $("#WhatsAppMessage").val();
        }
        //Read Confirmation
        if ($("#ReadRadio").prop("checked")) {
            request["IsReadConfirmation"] = true;
        }
        else
        {
            request["IsReadConfirmation"] = false;
        }

        //group selection
        if ($("#idSelectGroups").val() != "" && $("#idSelectGroups").val().length > 0)
            request["GroupId"] = $("#idSelectGroups").val().map(Number);

        //People selection
        if ($("#idSelectPeople").val() != "" && $("#idSelectPeople").val().length > 0)
            request["SubscriberId"] = $("#idSelectPeople").val();

        //events selection
        request["IsNewEvent"] = false;
        request["EventId"] = $("#idExistingEvents").val();
        
        //approval section
        if ($("#idSendWithoutApproval").prop("checked"))
            request["IsApprovalRequired"] = true;
        else
            request["IsApprovalRequired"] = false;

        //Signature section
        if ($("#idIsSignatureRequired").prop("checked"))
            request["IsSignatureRequired"] = true;
        else
            request["IsSignatureRequired"] = false;

        //private notification 
        request["ApproverForPrivate"] = "";
        if ($('#idIsPrvateNotifiation').prop('checked')) {
            request["IsPrivateNotification"] = true;
            if ($('#idSendWithoutApproval').prop('checked'))
                request["ApproverForPrivate"] = ($("#idselectPrivateApprover").val().length > 0) ? $("#idselectPrivateApprover").val()[0]:"";
        }
        else {
            request["IsPrivateNotification"] = false;
        }

        return request;
    }

    $('#idbuttonCreateTemplate').click(function (e) {

        let validation = validateTemplateCreation();
        if (validation != "") {
            showEmptyTextAlert(validation);
            return false;
        }

        showLoader();
        var request = createTemplateRequest();

        var uploadObj = document.getElementById("EmailAttachment").ej2_instances[0];

        var formData = new FormData();

        uploadObj.filesData.forEach(function (v, i) {
            formData.append("files", uploadObj.filesData[i].rawFile);
        });

        formData.append("data", JSON.stringify(request));
        
        $.ajax({
            url: "/was/Notification/CreateTemplate",
            data: formData,
            type: 'POST',
            cache: false,
            contentType: false,
            processData: false,
            success: function (result) {
                $("#modal-mg button.close").click();

                let isCategoryCreated = false;
                let categoryId = request["CategoryId"];
                if (request["CategoryId"] == 0) {
                    isCategoryCreated = true;
                    if (result != undefined && result.CategoryId != undefined)
                        categoryId = result.CategoryId;
                }
                //loading templates data again
                bindAllTemplates(request["Name"], categoryId, isCategoryCreated);

                isTemplateSelected = true;

                //saving template basic data
                selectedTemplateContent = {};
                selectedTemplateContent["Name"] = request.Name;
                selectedTemplateContent["Description"] = request.Description;
                selectedTemplateContent["CategoryId"] = categoryId;
                //$("#idSaveAsExistingTemplate").show();
                $("#CombineSaveMenu").show();
                $(".hideOnTemplateSelect").hide();
                setTimeout(function () { scrollTop(); hideLoader(); }, 1000);
            },
            error: function (err) {
                $('#loading').hide();
                $("#modal-mg button.close").click();
                hideLoader();
            }
        });
    });

    $(document).on("click", ".templateListItem", function (event) {

        $(".templateListItem").removeClass("selectedTemplate");
        $(event.currentTarget).addClass("selectedTemplate");

        let currentVal = $(event.currentTarget).attr("value");

        if (currentVal == undefined || currentVal == "") {
            isTemplateSelected = false;
            selectedTemplateContent = {};
            clearAllSelections();
            scrollTop();
            return false;
        }

        showLoader();
        
        $.ajax({
            url: "/was/Notification/GetTemplateById",
            type: "POST",
            data: {
                Id: "" + currentVal
            },
            success: function (d) {
                
                if (d != undefined && d != null) {
                    let data = JSON.parse(d);
                    enableTemplateSelection(data);
                    isTemplateSelected = true;

                    //saving template basic data
                    selectedTemplateContent = {};
                    selectedTemplateContent["Name"] = data.Name;
                    selectedTemplateContent["Description"] = data.Description;
                    selectedTemplateContent["CategoryId"] = data.CategoryId;
                    //$("#idSaveAsExistingTemplate").show();
                    $("#CombineSaveMenu").show();
                    $(".hideOnTemplateSelect").hide();
                }
                hideLoader();
                if ($(".select2-selection__choice").length > 0 && $(".select2-results").length > 0)
                    $(".select2-selection__choice")[0].click();
                scrollTop();
            },
            error: function (errorMessage) {
                hideLoader();
            }
        });
    });

    function enableTemplateSelection(d) {

        clearAllSelections();

        if (d != undefined && d != null) {

            let firstTab = "", firstTabContent = "";

            // notification 
            if (d.IsNotification)
                $("#NotificationRadio").prop("checked", true);
            else
                $("#NotificationRadio").prop("checked", false);

            //ReadConfirmation
            if (d.IsReadConfirmation)
                $("#ReadRadio").prop("checked", true);
            else
                $("#ReadRadio").prop("checked", false);

            //group selection
            if (d.GroupId != null && d.GroupId.length > 0) {
                d.GroupId.forEach(function (v, i) {
                    $('#idSelectGroups option[value="' + v + '"]').prop('selected', true).trigger('change', 'fromTemplate');
                });
                $("#idSelectGroups").trigger("change");
            }

            //people selection
            if (d.SubscriberId != null && d.SubscriberId.length > 0) {
                d.SubscriberId.forEach(function (v, i) {
                    $('#idSelectPeople option[value="' + v + '"]').prop('selected', true).trigger('change', 'fromTemplate');
                });
                $("#idSelectPeople").trigger("change");
            }
 
            //publishing Types and message content
            if (d.IsText) {
                //$("#TextCheckbox").click();
                $(".publishTabContent").removeClass("active");
                $(".publishTabContent").removeClass("show");
                $("#TextCheckbox").prop('checked', true);
                $("#TextTab").show();
                $("#TextTabContent").addClass("active show");
                $("#MessageText").val(d.MessageText);
                setMessageContentByDefault("MessageText", d.MessageText);
                firstTab = "#TextTab";
                firstTabContent = "#TextTabContent";
            }
            if (d.IsVoice) {
                //$("#VoiceCheckbox").click();
                $(".publishTabContent").removeClass("active");
                $(".publishTabContent").removeClass("show");
                $("#VoiceCheckbox").prop('checked', true);
                $("#VoiceTab").show();
                $("#VoiceTabContent").addClass("active show");
                $("#VoiceMessage").val(d.VoiceMessage);
                if (d.VoiceLanguage != "")
                    $("#VoiceLanguage").val(d.VoiceLanguage);
                if (d.VoiceRepeatCount > 0) {
                    document.getElementById("idVoiceRepeatCount").value = d.VoiceRepeatCount;
                    document.getElementById("idVoiceRepeatCount").setAttribute("value", d.VoiceRepeatCount);
                }
                if (firstTab == "") {
                    setMessageContentByDefault("VoiceMessage", d.VoiceMessage);
                    firstTab = "#VoiceTab";
                    firstTabContent = "#VoiceTabContent";
                }
            }
            if (d.IsEmail) {
                //$("#EmailCheckbox").click();
                $(".publishTabContent").removeClass("active");
                $(".publishTabContent").removeClass("show");
                $("#EmailCheckbox").prop('checked', true);
                $('#EmailTab').show();
                $("#EmailTabContent").addClass("active show");
                $("#idEmailSubject").val(d.EmailSubject);
                $("#EmailBody").val(d.EmailBody);

                //attachments
                if (d.EmailAttachmentsURL.length > 0) {
                    $("#idAdditionalAttachments").html(getAdditionalAttachmentsList(d.EmailAttachmentsURL));
                    selectAllEmailAttachments();
                    $("#idSelectedAttachmentText").show();
                }
                else
                    $("#idSelectedAttachmentText").hide();

                if (firstTab == "") {
                    setMessageContentByDefault("EmailBody", d.EmailBody);
                    firstTab = "#EmailTab";
                    firstTabContent = "#EmailTabContent";
                }
            }
            if (d.IsWhatsApp) {
                //$("#WhatsappCheckbox").click();
                $(".publishTabContent").removeClass("active");
                $(".publishTabContent").removeClass("show");
                $("#WhatsappCheckbox").prop('checked', true);
                $('#WhatsAppTab').show();
                $("#WhatsAppTabContent").addClass("active show");
                $("#WhatsAppMessage").val(d.WhatsAppMessage);
                if (d.WhatsAppTemplateId != "")
                    $('#WhatsAppTemplate option[value="' + d.WhatsAppTemplateId + '"]').prop('selected', true).trigger('change');

                if (firstTab == "") {
                    setMessageContentByDefault("WhatsAppMessage", d.WhatsAppMessage);
                    firstTab = "#WhatsAppTab";
                    firstTabContent = "#WhatsAppTabContent";
                }
            }

            //event
            if (!d.IsNewEvent && d.EventId != null) {
                $('#idExistingEvents option[value="' + d.EventId + '"]').prop('selected', true).trigger('change');
            }

            //private notification
            if (d.IsPrivateNotification)
                $("#idIsPrvateNotifiation").prop("checked", true);
            else
                $("#idIsPrvateNotifiation").prop("checked", false);

            //approval
            if (d.IsApprovalRequired)
                $("#idSendWithoutApproval").prop("checked", true);
            else
                $("#idSendWithoutApproval").prop("checked", false);
            setTextBasedOnApproval();

            //Signature section
            if (d.IsSignatureRequired)
                $("#idIsSignatureRequired").prop("checked", true);
            else
                $("#idIsSignatureRequired").prop("checked", false);


            //private notification approver
            if (d.IsPrivateNotification && d.IsApprovalRequired && d.ApproverForPrivate != "") {
                $('#idselectPrivateApprover option[value="' + d.ApproverForPrivate + '"]').prop('selected', true).trigger('change');
            }

            //showing first tab in peference
            if (firstTab != "") {
                $(".publishTabContent").removeClass("active");
                $(".publishTabContent").removeClass("show");
                $(firstTab).show();
                $(firstTab).click();
                $(firstTabContent).addClass("active show");
            }

            //enabling save as existing template
            $("#CombineSaveMenu").show();
            $(".hideOnTemplateSelect").hide();
        }
    }

    function clearAllSelections() {
        //Read Confirmation
        $("#ReadRadio").prop("checked", false);


        //main notification tile
        $("#NotificationRadio").prop("checked", true);

        //group selection
        clearGroupsDropdown();
        $('.classGroupCount').hide();

        //people selection
        clearPeopleDropdown();

        //publishing Types
        if ($("#TextCheckbox").prop("checked")) {
            $("#TextCheckbox").prop('checked', false);
            $('#TextTab').hide();

        }
        if ($("#VoiceCheckbox").prop("checked")) {
            $("#VoiceCheckbox").prop('checked', false);
            $('#VoiceTab').hide();
        }
        if ($("#EmailCheckbox").prop("checked")) {
            $("#EmailCheckbox").prop('checked', false);
            $('#EmailTab').hide();
        }
        if ($("#WhatsappCheckbox").prop("checked")) {
            $("#WhatsappCheckbox").prop('checked', false);
            $('#WhatsAppTab').hide();
        }
        $(".publishTabContent").removeClass("active");
        $(".publishTabContent").removeClass("show");
        $("#TextTabContent").addClass("active show");

        // message content
        $("#MessageText").val("");
        $("#VoiceMessage").val("");
        $("#VoiceLanguage").val("en-US");
        $(".form-control[name='VoiceRepeatCount']").val("1");
        $(".form-control[name='VoiceRepeatCount']").attr("value", "1");
        $("#idEmailSubject").val("Notification from West Alert System");
        $("#EmailBody").val("");
        $("#WhatsAppMessage").val("");
        $('#WhatsAppTemplate>option:eq(0)').prop('selected', true).trigger('change');

        var uploadObj = document.getElementById("EmailAttachment").ej2_instances[0];
        if (uploadObj.selectedFiles.length > 0)
            uploadObj.clearAll();
        $("#idAdditionalAttachments").html("");
        $("#idselectedEmailAttachments").html("");
        $("#idSelectedAttachmentText").hide();

        //for private notification
        $("#idIsPrvateNotifiation").prop("checked", false);
        clearPrivateApproverDropdown();

        //approval
        $("#idSendWithoutApproval").prop("checked", false);
        setTextBasedOnApproval();

        //Signature
        $("#idIsSignatureRequired").prop("checked", false);

        //enabling save as existing template
        $("#CombineSaveMenu").hide();
        $(".hideOnTemplateSelect").show();
         
        //just for hiding opend groups and people tab
        if ($("#idselectForGroups .select2-selection").attr("aria-expanded")=="true")
            $("#idselectForGroups .select2-search__field").click()
        if ($("#idselectForPeople .select2-selection").attr("aria-expanded") == "true")
            $("#idselectForPeople .select2-search__field").click();
        scrollTop();
    }

    $('#idCreateEventNewly').click(function (e) {
        if ($("#idNewEventType option").length == 0) {
            showLoader();
            getEventTypeAndUrgency();
        }
    });

    $('#idbutnCreateEvent').click(function (e) {

        if ($("#idNewEventName").val() == "") {
            return false;
        }
        let eventViewModel = {};
        $("#idOperationType").val("Create");
        eventViewModel["Name"] = $("#idNewEventName").val();
        eventViewModel["CreatedBy"] = $("#idEventCreatedBy").val();
        eventViewModel["ModifiedBy"] = $("#idEventCreatedBy").val();
        eventViewModel["TypeId"] = ($("#idNewEventType").val() == "") ? 0 : parseInt($("#idNewEventType").val());
        eventViewModel["UrgencyId"] = ($("#idNewEventUrgency").val() == "") ? 0 : parseInt($("#idNewEventUrgency").val());
        eventViewModel["Status"] = $("#idNewEventStatus").val();
        eventViewModel["Description"] = $("#idNewEventDescription").val();
        showLoader();

        $.ajax({
            url: "/was/Event/AddEvent",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(eventViewModel),
            success: function (d) {
                if (d.IsNameExist != undefined && d.IsNameExist != null && d.IsNameExist) {
                    hideLoader();
                    return false;
                }
                if (d.Success && d.Id != undefined && d.Name != undefined) {
                    let list = "<option value='" + d.Id + "'>" + d.Name + "</option>";
                    $("#idExistingEvents").prepend(list);
                    $("#idExistingEvents").val(d.Id);
                    $("#idEventCreateDismiss").click();
                }

                hideLoader();
            },
            error: function (errorMessage) { // error callback 
                hideLoader();

            }
        });

    });

    $('#idAlertForGroupnavigation').click(function (e) {
        $("#idAlertPopupContent").text("Notification data added above will be discarded");
    });

    $('#idAlertConfirm').click(function (e) {
        window.location.href = '/was/group/List?defaultTab=People';
    });

    $('#idCreateCategoryNewly').click(function (e) {

        $(e.currentTarget).attr("isselected", "true");
        $("#idNewCategoryDiv").show();
        if ($("#idTemplatesCatList").val() != "")
            $("#idTemplatesCatList").val("");
    });

    $('#idNewCategoryName').keyup(function (e) {
        if ($(e.currentTarget).val() != "" && $("#idTemplatesCatList").val() != "")
            $("#idTemplatesCatList").val("");
    });

    $('#idTemplatesCatList').change(function (event) {
        if ($("#idTemplatesCatList").val() != "") {
            $('#idNewCategoryName').val("");
            $("#idCreateCategoryNewly").attr("isselected", "false");
            $("#idNewCategoryDiv").hide();
        }
    });

    $('#idSaveAsExistingTemplate').click(function (e) {
        if (!checkPublishSelection()) {
            showEmptyTextAlert("Select atleast one publishing Type");
            return false;
        }
        clearTemplatepopup("existing");

        //setting the existing value
        $("#idTemplateName").val((selectedTemplateContent.Name != undefined && selectedTemplateContent.Name != "") ? selectedTemplateContent.Name : "");
        $("#idTemplateDescription").val((selectedTemplateContent.Description != undefined && selectedTemplateContent.Description != "") ? selectedTemplateContent.Description:"");
        if (selectedTemplateContent.CategoryId != undefined && selectedTemplateContent.CategoryId != "")
            $("#idTemplatesCatList").val(selectedTemplateContent.CategoryId);
    });

    $('#idRefreshNotification').click(function (e) {
        showLoader();
        clearAllSelections();
        $(".templateListItem").removeClass("selectedTemplate");
        isTemplateSelected = false;
        selectedTemplateContent = {};
        hideLoader();
    });
   
    $(document).on("click", ".classMenuItem", function (e, temp) {
        alertboxevent = e;
        if (temp != undefined && temp == "confirm") {
            location = alertboxevent.currentTarget.childNodes[1].childNodes[0].parentElement.attributes[0].ownerElement.href;
            return location;
        }
        else {
            if (document.getElementById("ReadRadio").checked) {
                var readRadio = true;
            }
            if (document.getElementById("SurveyRadio").checked) {
                var surveyRadio = true;
            }
            if (document.getElementById("ConferenceRadio").checked) {
                var conferenceRadio = true;
            }
            if (document.getElementById("TextCheckbox").checked) {
                var textCheckbox = true;
            }
            if (document.getElementById("VoiceCheckbox").checked) {
                var voiceCheckbox = true;
            }
            if (document.getElementById("EmailCheckbox").checked) {
                var emailCheckbox = true;
            }
            if (document.getElementById("WhatsappCheckbox").checked) {
                var whatsappCheckbox = true;
            }
            if (document.getElementById("idIsSignatureRequired").checked) {
                var idIsSignatureRequired = true;
            }
            var MessageText = document.getElementById("MessageText").value;
            if (MessageText.length != 0) {
                var message = true;
            }
            var VoiceMessage = document.getElementById("VoiceMessage").value;
            if (VoiceMessage.length != 0) {
                var voice = true;
            }
            var EmailBody = document.getElementById("EmailBody").value;
            if (EmailBody.length != 0) {
                var email = true;
            }
            var WhatsAppMessage = document.getElementById("WhatsAppMessage").value;
            if (WhatsAppMessage.length != 0) {
                var whatsApp = true;
            }
            var idSelectGroups = document.getElementById("idSelectGroups");
            if (idSelectGroups.selectedOptions.length != 0) {
                var selectGroups = true;
            }
            var idSelectPeople = document.getElementById("idSelectPeople");
            if (idSelectPeople.selectedOptions.length != 0) {
                var selectPeople = true;
            }
            if (document.getElementById("idIsPrvateNotifiation").checked) {
                var idIsPrvateNotifiation = true;
            }
            if (readRadio || idIsPrvateNotifiation || selectPeople || selectGroups || whatsApp || email || voice || message || idIsSignatureRequired || whatsappCheckbox || emailCheckbox
                || voiceCheckbox || textCheckbox || conferenceRadio || surveyRadio) {
                $('#modal-alertPopupdesign').modal('show');
                return false;
            }
        }
    });

    $("#idAlertConfirmleave").click(function () {
        $(alertboxevent.currentTarget).trigger('click', 'confirm');
    });
   
    
});

function GetWhatsAppTemplate(setPreview) {
    if ($("#WhatsAppTemplate").val() == "" || $("#WhatsAppTemplate").val() < 1) {
        $("#templateDescription").val("");
        $("#previewMessage").hide();
        $("#templateDescription").val("");
        return false;
    }

    showLoader();
    $.ajax({
        url: "/was/Notification/getwhatsapptemplate",
        type: "GET",
        contentType: "application/json",
        data: { id: $("#WhatsAppTemplate option:selected").val() },
        success: function (d) {
            $("#templateDescription").val(d.Description);
            $whatsappremainingchars.text(555 - d.Description.length);
            setWhatsAppPreview();
            hideLoader();
        }
    });
}

function onFileRemove(args) {
    args.postRawFile = false;
}

function LoadTimeZones() {
    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1;
    var yyyy = today.getFullYear();
    var h = today.getHours();
    var m = today.getMinutes();
    if (dd < 10) {
        dd = '0' + dd
    }
    if (mm < 10) {
        mm = '0' + mm
    }
    if (h < 10) {
        h = '0' + h
    }
    if (m < 10) {
        m = '0' + m
    }
    today = yyyy + '-' + mm + '-' + dd + 'T' + h + ':' + m;
    $('#ScheduledTime').attr({
        'value': today,
        'min': today
    });

    var timeZones = moment.tz.names();
    let offsetTmz = [];

    for (let i in timeZones) {
        offsetTmz.push(`(GMT${moment.tz(timeZones[i]).format('Z')}) ${timeZones[i]}`);
    }
    timeZones = offsetTmz.sort();

    for (var i in timeZones) {
        $('#SelectTimeZone').append('<option value="' + timeZones[i].substring(12, timeZones[i].length) + '">' + timeZones[i] + '</option>');
    }
    $('#SelectTimeZone').val(moment.tz.guess());
    $('#SelectTimeZone').selectpicker();

}

function GetTimeZone() {
    $('#TimeZoneOffset').val(moment.tz.zone($('#SelectTimeZone').val()).parse(Date.parse($('#ScheduledTime').val())));
    $('#TimeZone').val($('#SelectTimeZone').val());
}

function checkPublishSelection() {
    //checking atleast one publishing type is selected
    let flag = false;
    groupElementArrayMapping.forEach(function (v, i) {
        if ($(v.id).prop('checked')) {
            flag = true;
        }
    });
    return flag;
}

function checkUserGroupSelection() {
    if ($('#idSelectGroups').val() == '' && $('#idSelectPeople').val() == '') {
        showEmptyTextAlert('Select at least one group/people');
        return false;
    }
    else { return true; }
}

$(window).on('load', function () {
    //making langage default selection
    $('#VoiceLanguage').val("en-US");
    
    setTimeout(function () { scrollTop(); hideLoader(); optionselect(); }, 500);
    
});

function setTextBasedOnApproval() {
    if ($('#idSendWithoutApproval').prop('checked')) {
        $("#idTextSendWithApproval").val("true");
        $("#modal-lg .btn-success").text("Send for Approval");
        $(".classPublishselectCheck.btn-success").text("Send for Approval");
        if ($('#idIsPrvateNotifiation').prop('checked')) {
            clearPrivateApproverDropdown();
            $("#idselectForPrivateApprover").show();
        }
        else
            $("#idselectForPrivateApprover").hide();

    }
    else {
        $("#idTextSendWithApproval").val("false");
        $("#modal-lg .btn-success").text("Schedule");
        $(".classPublishselectCheck.btn-success").text("Send Now");
        $("#idselectForPrivateApprover").hide();
    }
}

function clearGroupsDropdown() {
    $('#idSelectGroups').val("");
    $('#idselectForGroups .select2-selection__choice__remove').each(function (index, element) {
        element.click();
    })
}

function clearPeopleDropdown() {
    $('#idSelectPeople').val("");
    $('#idselectForPeople .select2-selection__choice__remove').each(function (index, element) {
        element.click();
    })
}

function getAdditionalAttachmentsList(data) {

    let list = "";

    let attachementId = "";
    data.forEach(function (v, i) {
        attachementId = "emailAttachment" + i;
        list = list + "<div class='icheck-success'>";
        list = list + "<input type='checkbox' id='" + attachementId + "' class='classAttachmentsParent' value='" + (v.URL + "|" + v.FileName + "|" + v.ContentType) + "'>";
        list = list + "<label class='sub-label' for='" + attachementId + "'>" + v.FileName + "</label>";

        list = list + "</div>";

    });

    return list;
}

function selectAllEmailAttachments() {
    let list = "", value = "";
    $("#idselectedEmailAttachments").html("");
    $(".classAttachmentsParent").each(function (i, v) {
        $(v).prop("checked", true);
        value = $(v).attr("value");
        list = list + '<input type="hidden" value="' + value + '" name="ExistingEmailAttachments" class="clsSelectedtemplateAttachments" />';
    });
    $("#idselectedEmailAttachments").html(list);

}

function updateSelectedEmailAttachments() {
    let list = "", value = "";
    $("#idselectedEmailAttachments").html("");
    $(".classAttachmentsParent").each(function (i, v) {
        if ($(v).prop("checked")) {
            value = $(v).attr("value");
            list = list + '<input type="hidden" value="' + value + '" name="ExistingEmailAttachments" class="clsSelectedtemplateAttachments" />';
        }
    });
    $("#idselectedEmailAttachments").html(list);
}

function getSelectedAttachmentFromTemplate() {
    let data = [], value = "";
    $(".clsSelectedtemplateAttachments").each(function (i, v) {
        value = $(v).attr("value");
        if (value != undefined && value != "") {
            data.push(value);
        }
    });

    return data;
}

function bindAllTemplates(templateId, categoryId, isCategoryCreated) {
    showLoader();
    $.ajax({
        url: "/was/Notification/GetAllTemplates",
        type: "POST",
        success: function (d) {
            if (d != undefined && d != null) {

                $("#idTemplateParent").html(d);

                if (isCategoryCreated)
                    bindCategoryListinpopup();

                if (templateId != undefined && templateId != "") {
                    $(".templateListItem").removeClass("selectedTemplate");
                    $('.templateListItem:contains("' + templateId + '")').addClass("selectedTemplate");
                }

                if (categoryId != undefined && categoryId != "") {
                    $('.categoryMainparent[value="' + categoryId + '"]').click();
                }
            }
            hideLoader();
        },
        error: function (errorMessage) {
            hideLoader();
        }
    });
}

function bindCategoryListinpopup() {
    showLoader();
    $.ajax({
        url: "/was/Notification/GetTemplateCategories",
        type: "POST",
        success: function (d) {
            if (d != undefined && d != null && d.TemplateCategories.length > 0) {

                $("#idTemplatesCatList").html("");
                let list = "";
                list += "<option value=''>Select Category</option>";
                d.TemplateCategories.forEach(function (v, i) {
                    list += "<option value='" + v.Id + "'>" + v.Name + "</option>";
                });
                $("#idTemplatesCatList").html(list);

            }
            hideLoader();
        },
        error: function (errorMessage) {
            hideLoader();
        }
    });
}

function setWhatsAppPreview() {

    $("#previewMessage").show();
    if ($("#templateDescription").val().length == 0)
        GetWhatsAppTemplate();
    else
        setWhatsappFinalMessage();
}

function setEvetNameForNewEvents() {

    //updating the event new event name
    var momentVariable = moment.utc(new Date(), "");
    convertedDate = momentVariable.clone().tz(moment.tz.guess()).format("MMM DD, YYYY hh:mm:ss A");
    let finalText = convertedDate + " (" + moment.tz.guess() + ")";
    $("#idCreatedEventName").val(finalText);
}

function scrollTop() {
    window.scrollTo(0, 50);
}

function getEventTypeAndUrgency() {
    $.ajax({
        url: "/was/Event/GetTypeAndUrgency",
        type: "POST",
        success: function (d) {
            if (d != undefined && d != null) {
                let list = "";

                if (d.EventTypes != undefined && d.EventTypes != null && d.EventTypes.length > 0) {
                    list = "";
                    $("#idNewEventType").html("");
                    d.EventTypes.forEach(function (v, i) {
                        list += "<option value='" + v.Id + "'>" + v.Name + "</option>";
                    });
                    $("#idNewEventType").html(list);
                }

                if (d.EventUrgencies != undefined && d.EventUrgencies != null && d.EventUrgencies.length > 0) {
                    list = "";
                    $("#idNewEventUrgency").html("");
                    d.EventUrgencies.forEach(function (v, i) {
                        list += "<option value='" + v.Id + "'>" + v.Name + "</option>";
                    });
                    $("#idNewEventUrgency").html(list);
                }

            }
            hideLoader();
        },
        error: function (errorMessage) {
            hideLoader();
        }
    });
}

function setTextMessageCharacterCount(value) {

    var chars = value.length,
        messages = Math.ceil(chars / 160),
        remaining = messages * 160 - (chars % (messages * 160) || messages * 160);
    const totalCharRemain = 500 - chars;

    if (chars == 0) {
        remaining = 160;
        messages = 1;
    }

    if (totalCharRemain == 0) {
        $remaining.empty();
        $("#smsValidTxt").text('');
    }
    else {
        $remaining.text(remaining);
        $("#smsValidTxt").text('Characters remaining');
    }

    
    $messages.text(messages);
    $("#totalSmsRemaining").text(totalCharRemain);
}

function setWhatsAppCharacterCount(value) {
    var chars = value.length,
        whatsappremainingchars = 500 - chars;

    $whatsappremainingchars.text(whatsappremainingchars);
}

function setMessageContentByDefault(currentId, currentvalue) {

    if (currentvalue != undefined && currentvalue != "") {
       
        notificationContentElement.forEach(function (v, i) {

            if (v != currentId && ($("#" + v).val() == "")) {

                $("#" + v).val(currentvalue);

                if (v == "MessageText") {
                    setTextMessageCharacterCount(currentvalue);
                }
                else if (v == "VoiceMessage") {
                    $('#voiceEnteredChar').text(500-currentvalue.length);
                }
                else if (v == "EmailBody") {
                    $('#emailEnteredChar').text(currentvalue.length);
                }
                else if (v == "WhatsAppMessage") {
                    setWhatsAppCharacterCount(currentvalue);
                }

            }
        });

    }

}

function showEmptyTextAlert(text) {
    $("#idmodalsmText").text(text);
    $('#idAlertToggle').click();
}

function validateTemplateCreation() {
    let text = "";

    if ($("#idTemplateName").val() == undefined || $("#idTemplateName").val() == "")
        text = "Kindly provide a template Name";

    else if ($("#idCreateCategoryNewly").attr("isselected") == "true" && $("#idNewCategoryName").val() == "")
        text = "Kindly provide a category Name";

    else if ($("#idCreateCategoryNewly").attr("isselected") == "false" && $("#idTemplatesCatList").val() == "")
        text = "Select atleast one category";

    return text
}

function setWhatsappFinalMessage() {
    var finalmessage;
    $("#whatsappfinalmessage").html("");
    if ($("#templateDescription").val() != "") {
        finalmessage = $("#templateDescription").val().replace('{{1}}', '{{Name}}');
        finalmessage = finalmessage.replace('{{2}}', '<b>' + $("#WhatsAppMessage").val() + '</b>');
        finalmessage = $('#idIsSignatureRequired').prop('checked') ? finalmessage.replace('{{3}}', '{{"Sender Name" from West Alert System}}') : finalmessage.replace('{{3}}', '{{Admin from West Alert System}}');
        $("#whatsappfinalmessage").html(finalmessage);
    }
}

function checkWhatsappTemplateSelection() {   
    if ($("#WhatsappCheckbox").prop("checked")) {
        if ($('#WhatsAppTemplate').val() == '' && $('#WhatsAppTemplate').val() == '') {
            showEmptyTextAlert('Select at least one Whatsapp template');
            return false;
        }
        else { return true;}
    } else { return true; }
}

function clearTemplatepopup(type) {
    $("#modal-mg").attr("value", type);
    $("#idTemplateDescription,#idTemplateName").val("");
    $("#idTemplatesCatList").val("");
    $("#idNewCategoryName").val("");
    $("#idNewCategoryDiv").hide();
    $("#idCreateCategoryNewly").attr("isselected", "false");
    if (type == "new")
        $("#modal-mg .modal-title").text("Create Template");
    else
        $("#modal-mg .modal-title").text("Update Template");
}

function checkNotificationContent() {
    let text = "";

    if ($("#TextCheckbox").prop("checked") && $("#MessageText").val().trim() == "")
        text = "SMS Message content is required";
    else if ($("#VoiceCheckbox").prop("checked") && $("#VoiceMessage").val().trim() == "")
        text = "Voice Message content is required";
    else if ($("#EmailCheckbox").prop("checked") && $("#EmailBody").val().trim() == "")
        text = "Email content is required";
    else if ($("#WhatsappCheckbox").prop("checked") && $("#WhatsAppMessage").val().trim() == "")
        text = "WhatsApp Message content is required";

    return text;
}

function isNotificationContentLengthValid() {

    let text = "";

    if ($("#TextCheckbox").prop("checked") && $("#MessageText").val().length > 500)
        text = "Please enter SMS content in 500 characters";
    else if ($("#EmailCheckbox").prop("checked") && $("#idEmailSubject").val().length > 100)
        text = "Please enter email subject in 100 characters";
    else if ($("#WhatsappCheckbox").prop("checked") && $("#WhatsAppMessage").val().trim() == "")
        text = "Please enter Whatsapp content in 500 characters";

    if (text != "") {
        showEmptyTextAlert(text);
        return false;
    }
    return true;
}
function setPrivateNotificationText() {
    if ($('#idIsPrvateNotifiation').prop('checked')) {
        $("#idTextIsPrvateNotifiation").val("true");
        $("#idSendWithoutApproval").prop("checked", false);
        clearPrivateApproverDropdown();
    }
    else {
        $("#idTextIsPrvateNotifiation").val("false");
        $("#idSendWithoutApproval").prop("checked", true);
        $("#idselectForPrivateApprover").hide();
    }
    setTextBasedOnApproval();
}

function setNotificationSignature() {
    if ($('#idIsSignatureRequired').prop('checked')) {
        $("#idNotificationWithSignature").val(true);
        setWhatsappFinalMessage();
    }
    else {
        $("#idNotificationWithSignature").val(false);
        setWhatsappFinalMessage();
    }
}

function clearPrivateApproverDropdown() {
    $('#idselectPrivateApprover').val("");
    $('#idselectForPrivateApprover .select2-selection__choice__remove').each(function (index, element) {
        element.click();
    })
}

function checkApproverSelection() {
    let text = false;
    if ($('#idIsPrvateNotifiation').prop('checked') && $('#idSendWithoutApproval').prop('checked') && $("#idselectPrivateApprover").val().length == 0)
        text = true;
    return text;
}

function checkNotificationTypeSelection() {
    let text = false;
    if (!$("#ReadRadio").prop("checked") && !$("#NotificationRadio").prop("checked"))
        text = true;
    return text;
}

function removeSpaceFromWhatsAppMessage() {
    let text = $("#WhatsAppMessage").val();
    if (text == undefined || text == "" || text.trim()=="")
        $("#WhatsAppMessage").val("");
    else
        $("#WhatsAppMessage").val(text.replace(/\s\s+/g, ' '));
}

function WhatsappCheckboxClicked() {
    var whatsappTemplate = document.getElementById("WhatsAppTemplate");
    for (var i = 0; i < whatsappTemplate.options.length; i++) {
        if (whatsappTemplate.options[i].text == "Notification Template") {
            whatsappTemplate.options[i].selected = true;
        }
    }
    GetWhatsAppTemplate();
}

function hideGroupCountElements() {
    //hiding the count according to voice,text,email selection
    $('.classGroupCount').show();
    groupElementArrayMapping.forEach(function (v, i) {
        if (!$(v.id).prop('checked'))
            $(v.element).hide();
    });

}
function setGroupCount(totalCount, smsCount, emailCount, voiceCount, whatsappCount) {
    $('#idTotalSubscribers span').text(totalCount);
    $('.classSMSSubscribers span').text(smsCount);
    $('.classEmailSubscribers span').text(emailCount);
    $('.classVoiceSubscribers span').text(voiceCount);
    $('.classWhatsAppSubscribers span').text(whatsappCount);
}