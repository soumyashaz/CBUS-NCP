$(document).ready(function () {


    // SurveyAddQuestion.AssignControl();

    PublishSurvey.AssignControl();

});

var PublishSurvey = {

    AssignControl: function () {
        $("#BtnDone").on("click", PublishSurvey.CompleteSurvey)

    },
    CompleteSurvey: function () {
        var RasdioAns = $("input[name='SurveyAction']:checked");
        //   console.log(RasdioAns.val());
        if (RasdioAns.length > 0) {

            if (RasdioAns.val() == "CloseExist") {

              //  window.location.href = AjaxCallUrl.RedirectToSurveyListingUrl;
                if ($("#HdnIsNcpSurvey").val().toLowerCase() === "true") {
                    window.location.href = AjaxCallUrl.RedirectToNcpSurveyListingUrl;
                }
                else {
                    window.location.href = AjaxCallUrl.RedirectToSurveyListingUrl;
                }


            }
            else  //send email to 
            {
                kendo.ui.progress($("#DivPublishSurvey"), true);
                var postData = {
                    SurveyId: $("#HdnSurveyId").val(),
                }; //done


                $.ajax({ //Process the form using $.ajax()
                    type: 'POST', //Method type
                    url: AjaxCallUrl.SendSurveyUrl,
                    data: postData, //Forms name
                    dataType: 'json',
                    // traditional: true,
                    beforeSend: function () {
                    },

                    success: function (data) {
                        var DataUpdateWindow = $("#DataUpdateMessage").data("kendoWindow");
                        if (data.IsSuccess) {


                            kendo.ui.progress($("#DivPublishSurvey"), false);


                            var DataUpdateWindow = $("#DataSuccessMessage").data("kendoWindow");
                            if (DataUpdateWindow) {
                                DataUpdateWindow.title("Update Information");
                                DataUpdateWindow.content("<div class='msgboxSuccess'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i>Survey Published Successfully</div>");
                                DataUpdateWindow.open();
                                //redirect to listing page
                            }
                            setTimeout(function () {
                                if ($("#HdnIsNcpSurvey").val().toLowerCase() === "true") {
                                    window.location.href = AjaxCallUrl.RedirectToNcpSurveyListingUrl;
                                }
                                else {
                                    window.location.href = AjaxCallUrl.EmailPublishSurvey;
                                }
                            }, 1500);
                            //$('#btnok').click(function () {
                            //    window.location.href = AjaxCallUrl.EmailPublishSurvey;
                            //});
                        }
                        else {
                            if (DataUpdateWindow) {
                                DataUpdateWindow.title("Error Information");
                                DataUpdateWindow.content("<div class='msgboxError'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i>" + data.ModelError + " </div>");
                                DataUpdateWindow.open();
                            }
                            kendo.ui.progress($("#DivPublishSurvey"), false);

                        }
                    },
                    error: function (request, error) {

                        kendo.ui.progress($("#DivPublishSurvey"), false);

                    }
                });
            }
        }
    }
};