
var SelectedSubscription = [];

$(document).ready(function () {

    //Initialize Select2 Elements
    $('.select2').select2();

    //Initialize Select2 Elements
    $('.select2bs4').select2({
        theme: 'bootstrap4'
    })

    $("#idRemovePeopleFromGroup").hide();

    $('#idsearchGroupList').bind("enterKey", function (e) {
        var searchString = $('#idsearchGroupList').val();
        searchGrid("GridSubscriptionList", searchString);
    });

    $('#idsearchGroupList').keyup(function (e) {
        if (e.keyCode == 13 || $(this).val() == "") {
            $(this).trigger("enterKey");
        }
    });

    function searchGrid(element, text) {
        var gridObj = document.getElementById(element).ej2_instances[0];
        gridObj.search(text);
    }

    $('#idRemovePeopleFromGroup').click(function (e) {
        $("#idAlertPopupContent").text("Are you sure you want to remove selected people from this group");
    });

    $('#idAlertConfirm').click(function (e) {
        var id = $("#idGroupIdForDelete").val();
        if (id == undefined || id == "" || SelectedSubscription.length == 0)
            return false;
        showLoader();
        let request = {};
        request["GroupId"] = parseInt(id);
        request["SubscriptionGroupId"] = SelectedSubscription;
        $.ajax({
            url: "/was/group/RemoveSubscription",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(request),
            success: function (d) {
                $("#idAlertDismiss").click();
                window.location.href = '/was/group/View?groupId=' + id + '';
            },
            error: function (errorMessage) { // error callback
                $("#idAlertDismiss").click();
                window.location.href = '/was/group/View?groupId=' + id + '';
            }
        });

    });

});

function updateUserDetails() {
    populateUserDetailsforGrid($(".ad-user-grid"));
    populateUserPicturesforGrid($(".img-circle-grid"));
    populateUserRoleforGrid($(".ad-user-role"));
}

function filteredRowSelected(event) {
    if (event.target == undefined || event.target == null)
        return false;

    if (event.isHeaderCheckboxClicked) //selectALl
    {
        SelectedSubscription = [];
        SelectedSubscription = event.data.map((d) => d.SubscriptionGroupId)
    }
    else {
        let selectedId = "";
        if (event.data == undefined)
            selectedId = $(event.row).find(".customcssId").text();
        else
            selectedId = event.data.SubscriptionGroupId;
        if (selectedId != "" && selectedId != undefined) {
            if (SelectedSubscription.indexOf(event.data.SubscriptionGroupId) == -1)
                SelectedSubscription.push(event.data.SubscriptionGroupId);
        }
    }
    $('#idRemovePeopleFromGroup').show();
    if (SelectedSubscription.length == 0)
        $('#idRemovePeopleFromGroup').hide();
}

function filteredRowDeSelected(event) {
    if (event.target == undefined || event.target == null)
        return false;

    if (event.isHeaderCheckboxClicked) //selectALl
    {
        SelectedSubscription = [];
    }
    else {
        let selectedId = "";
        if (event.data == undefined)
            selectedId = $(event.row).find(".customcssId").text();
        else
            selectedId = event.data.SubscriptionGroupId;
        if (selectedId != "" && selectedId != undefined) {
            const index = SelectedSubscription.indexOf(selectedId);
            if (index > -1 && SelectedSubscription.length > 0) {
                SelectedSubscription.splice(index, 1);
            }
        }
    }
    $('#idRemovePeopleFromGroup').hide();
    if (SelectedSubscription.length > 0)
        $('#idRemovePeopleFromGroup').show();
}

function createAddSubscriptionRequest() {
    let request = {}, SubscriptionIds = [];
    request["Id"] = [];
    request["Id"].push($('#idGroup').val());
    
    if ($('#idSelectPeople').val().length > 0) {

        SubscriptionIds = $('#idSelectPeople').val();
    }
    request["SubscriptionId"] = SubscriptionIds;

    return request;
}

$('#idButtonAddSubscriptionToGroup').click(function () {
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
            window.location.href = '/was/group/View?groupId=' + subscriptionList.Id;
            hideLoader();
        },
        error: function (errorMessage) { // error callback 
            window.location.href = '/was/group/View?groupId=' + subscriptionList.Id;
            hideLoader();
        }
    });
});