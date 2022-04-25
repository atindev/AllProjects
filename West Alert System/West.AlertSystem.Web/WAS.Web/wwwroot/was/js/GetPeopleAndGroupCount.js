function GetPeopleAndGroupCount(peopleList){
    $.ajax({
        url: "/was/Notification/GetSubscriptionCount",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(peopleList),
        async: false,
        success: function (d) {
            if (d != undefined && d != null) {
                hideGroupCountElements();
                var totalsubscribers = d.TotalSubscribers;
                var smscount = d.SMSCount;
                var Emailcount = d.EmailCount;
                var Voicecount = d.VoiceCount;
                var Whatsappcount = d.WhatsappCount;
                setGroupCount(totalsubscribers, smscount, Emailcount, Voicecount, Whatsappcount);
            }
            hideLoader();
        },
        error: function (errorMessage) {
            hideLoader();
        }
    });
}