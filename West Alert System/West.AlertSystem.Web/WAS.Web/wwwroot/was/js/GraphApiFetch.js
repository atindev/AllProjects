$("#load").hide();
window.sessionStorage.removeItem("DataLoad");
if (window.sessionStorage.getItem('DataLoad') == null) {
    window.sessionStorage.setItem('DataLoad', 'set');
    setTimeout(function () {
        notif("BackGround Data Fetch", "Refresh Page to Get New Data", "danger",15000);
        window.sessionStorage.removeItem('DataLoad');
    }, 900000);
    
    $.ajax({
        "async": true,
        "crossDomain": true,
        "url": "https://cors-anywhere.herokuapp.com/https://login.microsoftonline.com/61a70d37-ff63-45e3-bb68-f0edbf718ffd/oauth2/v2.0/token", // Pass your tenant name instead of sharepointtechie
        "method": "POST",
        "headers": {
            "content-type": "application/x-www-form-urlencoded"
        },
       
        "data": {
            "grant_type": "client_credentials",
            "client_id ": "05256ab2-8545-4a1c-9940-84dc1756ddfb", //SeatAllocation app id
            "client_secret": "WwwH-j~l29QrfwXVsR1b-z8jVixR-l7H~p", //Provide your client secret genereated from your app
            "scope ": "https://graph.microsoft.com/.default"
        },
        success: function (response) {
            window.sessionStorage.setItem("token", response.access_token);
            
        },
        failure:
            
            function (resp) {
                notif("Graph Api Call", "Failed in getting token", "danger",200000);
            },
        error:
            
            function (resp) {
                notif("Graph Api Call", "Error in getting token", "danger",200000);
            }

    });
}
else {
    notif("BackGround Data Check", "Already Done <strong>Use Now<strong>", "primary");
}


function fetchAdUsers(inputElement, listElement) {
    var token = window.sessionStorage.getItem("token");
    $("#load").show();
    $("#createManager").hide();
    // alert("Asfd");
    
    if ($(inputElement).val().toString() != "")
        $.ajax({
            url: 'https://graph.microsoft.com/v1.0/users',
            type: 'GET',
            headers: { "Authorization": "Bearer " + token },
            data: {
                $filter: "startswith(displayName, '" + $(inputElement).val() + "') or startswith(userPrincipalName, '" + $(inputElement).val() + "') or startswith(mail, '" + $(inputElement).val() + "')"
            },
            success: function (results) {
                $(listElement).empty();
                $.each(results.value, function (index, data) {
                    if (data.userPrincipalName.toString().toLowerCase().search("westpharma") >= 0 && data.userPrincipalName.toString().toLowerCase().search("01") < 0)
                        $(listElement).append("<option id='dispn' jobTitle='" + data.jobTitle + "' value='" + data.userPrincipalName + "'>" + data.displayName + "</option>");
                });
                $("#load").hide();
                $("#createManager").hide();
                
                //alert("loaded data");
            },
            error:
                function (error) {
                    
                    notif("Data Fetching ", "error in data", "danger", 200000);
                },
            failure:
                function (error) {
                   
                    notif("Data Fetching ", "failed in calling api data", "danger", 200000);
                }
        });
};

$('#EmpId').on('input', function (e) {
    fetchAdUsers($(this), $("#EmpList"))
});
$('#EmpIdAllocation').on('input', function (e) {
    fetchAdUsers($(this), $("#EmpListAllocation"))
});
$('#officeEmpIdAllocation').on('input', function (e) {
    fetchAdUsers($(this), $("#officeEmpListAllocation"))
});
$('#globalEmpSearchInput').on('input', function (e) {
    fetchAdUsers($(this), $("#globalEmpSearchList"))
});
$('#searchBoxForValidate').on('input', function (e) {
    fetchAdUsers($(this), $("#validateCheckInEmpSearchList"))
});
$('#headOfTeamEmail').on('input', function (e) {
    fetchAdUsers($(this), $("#attendanceReportEmpSearchList"))
});
$('#shareReportEmpInput').on('input', function (e) {
    fetchAdUsers($(this), $("#shareReportEmpSearchList"))
});
