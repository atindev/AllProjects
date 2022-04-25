
$('#idFilterByEvents').change(function (event) {
    getPagedData();
    
});
$('#idFilterByStatus').change(function (event) {
    getPagedData();
});

$('#idFilterByTopic').bind("enterKey", function (e) {
    getPagedData();
});
$('#idFilterByTopic').keyup(function (e) {
    if (e.keyCode == 13 || $(this).val() == "") {
        $(this).trigger("enterKey");
    }
});

function getPagedData() {

    let eventFilter = "", statusFilter = "", messageFilter = "";

    if ($("#idFilterByEvents").find("option:selected").val() == -1)
        eventFilter = "";
    else
        eventFilter = $("#idFilterByEvents").find("option:selected").text();

    if ($("#idFilterByStatus").find("option:selected").val() == -1)
        statusFilter = "";
    else
        statusFilter = $("#idFilterByStatus").find("option:selected").val();

    messageFilter = $('#idFilterByTopic').val();

    var grid = document.getElementById('GridNotifications').ej2_instances[0]; // Grid instance 
    grid.pageSettings.currentPage = 1;
    grid.query = new ej.data.Query()
        .addParams('EventFilter', eventFilter)
        .addParams('StatusFilter', statusFilter)
        .addParams('MessageFilter', messageFilter);
}

function updateUserDetails() {
    populateUserDetailsforGrid($(".ad-user-grid"));
    populateUserPicturesforGrid($(".img-circle-grid"));

    var grid = document.getElementById('GridNotifications').ej2_instances[0];
    if (grid == undefined || grid.dataSource == undefined || $('#GridNotifications .e-row').length == 0)
    {
        var emptyContent = '<div class="col-12 bg-white emptyTemplateWas text-center mb-5">' +
            '<img src = "/was/img/No_Notifications.svg" width = "300" alt = "" />' +
            '<h2>No Notifications Available</h2>' +
            '<p>You will see notifications when administrator compose and trigger a notification, to create a new notification click on the Create Notification button</p>'+
            '<a href="/WAS/Notification/Create" class="btn" title="Create new notification">Create Notification</a>' +
            '</div >'
        $('#custom-content-below-tabContent').html("").html(emptyContent);
    }
}

