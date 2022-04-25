var iti;
$(document).ready(function () {
   
    //browser check
    let browserCheck = sessionStorage.getItem("browserCheck");
    if (browserCheck != "true") {
        sessionStorage.setItem("browserCheck", "true");
        if (checkBrowser()) {
            sessionStorage.setItem("browserCheck", "false");
            window.location.href = '/was/Account/UnsupportedBrowser';

        }
    }
    showLoader();
    populateUserPictures($(".img-circle"));
    populateUserDetails($(".ad-user"));
    populateUserDesignation($(".ad-user-designation"));
    hideLoader();
});

function populateUserPictures(imgElement) {
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
                    if (result != undefined && result != null && result.PictureBase64 != undefined && result.PictureBase64!=null)
                    $(imageElement).attr('src', result.PictureBase64)
                },
                error: function (err) {
                },
                complete: function (data) {
                }
            });
        }
    });
}

function populateUserDetails(element) {
    $(element).each(function () {
        var emailId = $(this).data("useremail");
        var spanElement = $(this);
        if (emailId != undefined && emailId != '') {
            url = '/was/graph/getuserdetails';
            $.ajax({
                url: url,
                type: 'GET',
                data: { emailId: emailId },
                success: function (result) {
                    if (result!=undefined && result!=null && result.FullName!=undefined)
                        $(spanElement).text(result.FullName);
                    else
                        $(spanElement).text(emailId);
                },
                error: function (err) {
                },
                complete: function (data) {
                }
            });
        }
    });
}

function populateUserDesignation(element) {
    $(element).each(function () {
        var emailId = $(this).data("useremail");
        var divElement = $(this);
        if (emailId != undefined && emailId != '') {
            url = '/was/graph/getuserdetails';
            $.ajax({
                url: url,
                type: 'GET',
                data: { emailId: emailId },
                success: function (result) {
                    $(divElement).text(result.Designation);
                },
                error: function (err) {
                },
                complete: function (data) {
                }
            });
        }
    });
}

function populateUserDetailsforGrid(element) {
 
    let userEmailNameMapping = [], distinctUserName = [];
    userEmailNameMapping = element;

    //finding unique email
    if (userEmailNameMapping.length > 0) {
        $(userEmailNameMapping).each(function () {
            if (distinctUserName.indexOf($(this).data("useremail")) == -1)
                distinctUserName.push($(this).data("useremail"));
        });
    }

    if (distinctUserName.length > 0) {
        let url = '/was/graph/getuserdetails', currentText = "";
        $(distinctUserName).each(function (i, v) {
            if (v != undefined && v != '') {
                $.ajax({
                    url: url,
                    type: 'GET',
                    data: { emailId: v },
                    success: function (result) {
                        if (userEmailNameMapping.length > 0 && result!=undefined) {
                            $(userEmailNameMapping).each(function () {
                                currentText = "";
                                if ($(this).data("useremail") == v && result.FullName != undefined && result.FullName != "") {
                                    currentText = $(this).text().replace(v, result.FullName);
                                    $(this).text(currentText);
                                    $(this).attr("title", currentText);
                                }
                            });
                        }
                    },
                    error: function (err) {
                    },
                    complete: function (data) {
                    }
                });
            }
        });
    }

}

function populateUserRoleforGrid(user) {
    let userEmailNameMapping = [], distinctUserName = [];
    userEmailNameMapping = user;

    //finding unique email
    if (userEmailNameMapping.length > 0) {
        $(userEmailNameMapping).each(function () {
            if (distinctUserName.indexOf($(this).data("useremail")) == -1)
                distinctUserName.push($(this).data("useremail"));
        });
    }

    if (distinctUserName.length > 0) {
        let url = '/was/graph/getuserrole';
        $(distinctUserName).each(function (i, v) {
            if (v != undefined && v != '') {
                $.ajax({
                    url: url,
                    type: 'GET',
                    data: { emailId: v },
                    success: function (result) {
                        if (userEmailNameMapping.length > 0 && result != undefined && result != null) {
                            $(userEmailNameMapping).each(function () {
                                if ($(this).data("useremail") == v) {
                                    if (result == "GlobalAdministrator") {
                                        $(this).html('<i class="fas fa-crown CheckGlobalAdmin shadow-sm" title="Global Administrator" aria-hidden="true"></i>');
                                    }
                                    else if (result == "WASAdmin") {
                                        $(this).html('<i class="fas fa-crown CheckWASadmin shadow-sm" title="Administrator" aria-hidden="true"></i>');
                                    }
                                    else if (result == "Approver") {
                                        $(this).html('<i class="fas fa-crown CheckApprover shadow-sm" title="Approver" aria-hidden="true"></i>');
                                    }
                                    else {
                                        $(this).html('<i class="fas fa-crown CheckCommApprover shadow-sm" title="Communication Approver" aria-hidden="true"></i>');
                                    }
                                }
                            });
                        }
                    },
                    error: function (err) {
                    },
                    complete: function (data) {
                    }
                });
            }
        });
    }
}

function populateUserPicturesforGrid(imgElement) {

    let userEmailPicMapping = [], distinctUserName = [];
    userEmailPicMapping = imgElement;

    //finding unique email
    if (userEmailPicMapping.length > 0) {
        $(userEmailPicMapping).each(function () {
            if (distinctUserName.indexOf($(this).data("useremail")) == -1)
                distinctUserName.push($(this).data("useremail"));
        });
    }

    if (distinctUserName.length > 0) {
        let url = '/was/graph/getuserpicture';
        $(distinctUserName).each(function (i, v) {
            if (v != undefined && v != '') {
                $.ajax({
                    url: url,
                    type: 'GET',
                    data: { emailId: v },
                    success: function (result) {
                        if (userEmailPicMapping.length > 0 && result != undefined) {
                            $(userEmailPicMapping).each(function () {
                                if ($(this).data("useremail") == v && result.PictureBase64 != undefined && result.PictureBase64 != "" && result.PictureBase64 != null)
                                    $(this).attr('src', result.PictureBase64);
                            });
                        }
                    },
                    error: function (err) {
                    },
                    complete: function (data) {
                    }
                });
            }
        });
    }


}

//for time zone updation
function updateTimezone(element) {
    element.each(function (index, element) {
        let convertedDate = "", finalText = "";
        let currentDate = $(element).attr("utctime");
        var timeZone = $(element).attr("timezone") != undefined ? $(element).attr("timezone") : moment.tz.guess();
        let currentText = element.innerHTML;
        if (currentDate != undefined && timeZone != undefined && currentDate != "") {
            var momentVariable = moment.utc(currentDate, "");
            convertedDate = momentVariable.clone().tz(timeZone).format("MMM DD, YYYY hh:mm:ss A");
            finalText = currentText + "  " + convertedDate + " (" + timeZone + ")";
            $(element).html(finalText);
        }
    });
}

function populateDialCodeWithFlag(elements) {
    $(elements).each(function () {
        var element = this;

        //clearing existing binding
        if ($(element).parent().find("input[type='hidden']").length > 0)
            $(element).parent().find("input[type='hidden']").remove();
        if ($(element).parent().find(".iti__flag-container").length>0)
            $(element).parent().find(".iti__flag-container").remove();

        iti = window.intlTelInput(element, {
            autoPlaceholder: "off",
            dropdownContainer: document.body,
            hiddenInput: this.id,
            localizedCountries: { 'de': 'Deutschland' },
            placeholderNumberType: "MOBILE",
            separateDialCode: true,
            utilsScript: "/was/js/utils.js",
        });
    });
}

function showLoader() {
    $('.divLoader').show();
}

function hideLoader() {
    $('.divLoader').hide();
}
 
function bindSubscriptionDetails(element) {
 
    showLoader();  
    let url = "", data = {};
    if (element.getAttribute("data-userid") != undefined && element.getAttribute("data-userid") != null) {
        url = "/was/Subscription/GetMaskedById";
        data = { id: element.getAttribute("data-userid") };
    }
    else {
        url = "/was/Subscription/GetMaskedByMail";
        data = { officialEmail: element.getAttribute("data-useremail") };

    }
    $("#hdnSubscriptionId").val(element.getAttribute("data-userid"));

    $("#hdnviewSubscriptionId").val(element.getAttribute("data-userid"));

    $.ajax({
        url: url,
        type: "POST",
        data: data,
        success: function (d) {
            if (d != undefined && d != null) {
                
                $("#idSubscriptionDetailView").html(d);

                $('#idShowSubscriptionDetails').click();

                populateUserPictures($(".img-circle"));
               
            }
            hideLoader();
        },
        error: function (errorMessage) {
            hideLoader();
        }
    });

}

function checkBrowser() {
    var ua = window.navigator.userAgent;
    var msie = ua.indexOf("MSIE ");

    if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./))  // If Internet Explorer 
    {
        return true;
    }
    else  // If another browser, return 0
    {
        return false;
    }
}
 