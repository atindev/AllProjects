var SelectedSubscription = [];
var flag = false;
$(document).ready(function () {

    //Initialize Select2 Elements
    $('.select2').select2();

    //Initialize Select2 Elements
    $('.select2bs4').select2({
        theme: 'bootstrap4',
        allowClear: false
    });

    $('.select2bs4').on('change', function () {
        setTimeout(function () {
            $('.select2 .select2-selection__rendered li:last-child input').focus();
        })
    })
    $('#idExistingGroups').change(function (event) {
        let selectedElements = getGroupsSelected();
        appendExistingGroupCount();
    });
    function getGroupsSelected() {
        let selectedGroups = [];
        $("#idExistingGroups").find("option:selected").each(function (index, element) {
            let value = $(element).attr("value");
            if (value != undefined && value != null && value != "")
                selectedGroups.push(value);
            document.getElementById("demo").innerHTML = "";
        });
        return selectedGroups;
    }

    function createAddSubscriptionRequest() {
        let request = {}, SubscriptionIds = [];
        if ($("#idOperationType").attr("value") == "existing") {
            request["Id"] = [];
            request["Id"] = getGroupsSelected();
        }
        else {
            request["Id"] = [];
            request["Id"].push(($('#idSubscriptionGroupID').val() == undefined || $('#idSubscriptionGroupID').val() == "") ? 0 : parseInt($('#idSubscriptionGroupID').val()));
            request["Name"] = $('#idNewSubscriptionGroupName').val();
            request["Description"] = $('#idNewSubscriptionGroupDescription').val();
            request["IsPrivate"] = $("#idAccessOnlyToOwner").prop("checked");
            request["IsAccessToAdmins"] = $("#idAccessToMembers").prop("checked");
        }

        request["CreatedBy"] = request["ModifiedBy"] = $('#idSubscriptionGroupCreatedBy').val();
        var selectedsubscribeId = $("#hdnSubscriptionsId").val();
        var selectedSubscriptionId = [];
        if (selectedsubscribeId == undefined || selectedsubscribeId == null || selectedsubscribeId == "") {
            var gridObj = document.getElementById('GridFilterdSubscriptions').ej2_instances[0];
            if (SelectedSubscription.length>0) {
                SubscriptionIds = SelectedSubscription;
                request["SubscriptionId"] = SubscriptionIds;
            }
        } else {
            selectedSubscriptionId.push(selectedsubscribeId);
            request["SubscriptionId"] = selectedSubscriptionId;
        }

        return request;
    }

    function getgroupdetails() {
        showLoader();
        var selectedsubscribeId = $("#hdnSubscriptionsId").val();
        if (selectedsubscribeId == "" || selectedsubscribeId == null || selectedsubscribeId == undefined) {
        }
        else {
            let url = "", data = {};
            url = "/was/Subscription/GetMaskedById";
            data = { id: selectedsubscribeId };
            
            $.ajax({
                url: url,
                type: "POST",
                data: data,
                success: function (d) {
                    if (d != undefined && d != null) {
                        $("#idSubscriptionDetailView").html(d);
                        $('#idShowSubscriptionDetails').click();
                        getUserPictures($(".img-circle"));
                    }
                    $(".modal-backdrop .fade").hide();
                    hideLoader();
                },
                error: function (errorMessage) {
                    hideLoader();
                }
            });
        }
    }
    function getUserPictures(imgElement) {
        showLoader();
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
        });
    }
    $('#idbuttonSubscriptionCreateGroup').click(function () {
        if ($("#idOperationType").attr("value") == "new" && $('#idNewSubscriptionGroupName').val() == "") {
            return false;
        }
        if ($("#idOperationType").attr("value") == "existing" && getGroupsSelected().length == 0) {
            document.getElementById("demo").innerHTML = "Please select an item in the list";
            return false;
        } else { document.getElementById("demo").innerHTML = ""; }
        showLoader();
        let subscriptionList = createAddSubscriptionRequest();

        if (subscriptionList.SubscriptionId.length == 0) {
            hideLoader();
            return false;
        }
        
        $.ajax({
            url: "/was/group/AddSubscription",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(subscriptionList),
            success: function (d) {
                if (d.IsNameExist != undefined && d.IsNameExist != null && d.IsNameExist) {
                    hideLoader();
                    return false;
                }
                if ($("#GridFilterdSubscriptions .e-row").length > 0 || $("#GridGroups .e-row").length > 0)
                    location.reload();
                else {
                    $('#modal-sg').hide();
                    getgroupdetails();
                    hideLoader();
                    $('.modal').modal('hide');
                }
            },
            error: function (errorMessage) { // error callback 
                window.location.href = '/was/group/List';
                hideLoader();
            }
        });
        return false;
    });
});

function appendExistingGroupCount() {
    $('#idExistingPeopleCount').html("");
    let currentText = "";
    $("#idExistingGroups").find("option:selected").each(function (index, element) {
        currentText = element.text + ": " + $(element).attr("count");
        $('#idExistingPeopleCount').append("<small class='badge badge-danger classExistingCountChild'>" + currentText + "</small>");

    });
}

function addToExistingGroup() {
    if ($("#modal-SubscriptionDetails:visible").length == 0)
        $("#hdnSubscriptionsId").val("");

    $("#modal-sg .modal-title").text("Add to Group");
    $("#idOperationType").attr("value", "existing");
    $('.newGroupPopup').hide();
    $("#idExistingGroupPopup").show();
    var selectedsubscribeId = $("#hdnSubscriptionsId").val();
    if (selectedsubscribeId == "" || selectedsubscribeId == null || selectedsubscribeId == undefined) {
        $("#idSelectedSubscriptions").show();
        let subscriptionCount = "Selected People: " + SelectedSubscription.length;
        $('#idSelectedSubscriptions').html("<i class='fas fa-user-friends' aria-hidden='true'></i> " + subscriptionCount);
    } else {
        $("#idSelectedSubscriptions").hide();
        $('#idShowSubscriptionDetails').click();
    }
    clearExistingGroupSelection();
}

function clearExistingGroupSelection() {
    $("#idExistingGroups").val("");

    $('#idExistingGroupPopup .select2-selection__choice__remove').each(function (index, element) {
        element.click();
    });
    appendExistingGroupCount();
}
function createNewGroup() {
    if ($("#modal-SubscriptionDetails:visible").length == 0)
        $("#hdnSubscriptionsId").val("");

    $("#modal-sg .modal-title").text("Create Group");
    $("#idOperationType").attr("value", "new");
    $('.newGroupPopup').hide();
    $("#idNewGroupPopup").show();
    $('#idNewSubscriptionGroupName').val("");
    $('#idNewSubscriptionGroupDescription').val("");
    $('#idSubscriptionGroupID').val("");
    enableDisablePrivacyCheckbox();
}

function Showmodelpopup() {
    if ($('#modal-sg').css("display") == "block") {
        $('#modal-SubscriptionDetails').hide();
    } else {
        $('#idShowSubscriptionDetails').click();
    }
    $('#idShowmodellg').click();
    addToExistingGroup();
}

function closemodel() {
    $('#modal-sg').hide();
    var selectedsubscribeId = $("#hdnSubscriptionsId").val();
    if (selectedsubscribeId == "" || selectedsubscribeId == null || selectedsubscribeId == undefined) {
        $('#modal-SubscriptionDetails').hide();
    } else {
        $('#idShowSubscriptionDetails').click();
    }
}

