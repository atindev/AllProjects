function getLocations() {
    var filterValue = document.getElementById('locations').selectedOptions[0].value;
    showLoader();
    $.ajax({
        type: "GET",
        url: "/WAS/Reports/location",
        data: {
            "locationId": filterValue
        },
        success: function (response) {
            $("#locationFilter").html(response);
            hideLoader();
        },
        error: function () {
            hideLoader();
        }
    });
}