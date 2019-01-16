$(document).ready(function () {
    // SurveyAddQuestion.AssignControl();
    ManageSurveyList.AssignControl();

   $('thead').first().css('display', 'block');
   $('thead').first().css('width', '100%');
   $('tbody').first().css('display', 'block');
   $('tbody').first().css('height', '500px');
   $('tbody').first().css('width', '100%');
});

ManageSurveyList = {

    AssignControl: function () {

        $("#BtnEdit").on("click", ManageSurveyList.EditSurvey);
        $("#BtnCopy").on("click", ManageSurveyList.copySurvey);
        $("#BtnPublish").on("click", ManageSurveyList.publishSurvey);
        $("#BtnPreview").on("click", ManageSurveyList.ViewSurvey);
        $("#BtnArchivedSurvey").on("click", ManageSurveyList.ArchivedSurvey);
    },
    EditSurvey: function () {
        var SurveyStatus = $("input[name='HdnSurveyStatus']").val();
        var PublishStatus = $("input[name='HdnSurveyPublished']").val();        
        var Rad = $("input[name='RadSurvey']:checked");
        var SurveyId = Rad.siblings().filter("input[name='HdnSurvey']").val();
        var Pub = Rad.siblings().filter("input[name='HdnPub']").val();
        var SurvStat = Rad.siblings().filter("input[name='Hdnst']").val();
        //  console.log(SurveyId)
        //alert(SurvStat + '       ' + Pub);
        if ((SurvStat == 'Live' && Pub == 'true') || (SurvStat == '' && Pub == 'true'))
        {
            var Confirm = confirm("You are about to Edit a Live & Published survey. Are you sure you want to proceed?");

            if (!Confirm) {
                return;
            }
            else
            {
                if (parseInt(SurveyId) > 0) {
                    var url = AjaxCallUrl.RedirectSurveyEditUrl.replace("****SurveyId****", SurveyId);
                    window.location.href = url;
                }
            }
        }
        else
        {
            if (parseInt(SurveyId) > 0) {
                var url = AjaxCallUrl.RedirectSurveyEditUrl.replace("****SurveyId****", SurveyId);
                window.location.href = url;
            }
        }
        

    },
    copySurvey: function () {

        var Rad = $("input[name='RadSurvey']:checked");
        var SurveyId = Rad.siblings().filter("input[name='HdnSurvey']").val();
        var ContractId = Rad.siblings().filter("input[name='HdnContract']").val();

        if (parseInt(SurveyId) > 0 && parseInt(ContractId) > 0) {



            var Confirm = confirm("Are you sure you want to copy the survey?");

            if (!Confirm) {
                return;
            }

            // kendo.ui.progress(".main-body-wrapper", false);
            var postData = { ContractId: ContractId, SurveyId: SurveyId }; //done
            $.ajax({ //Process the form using $.ajax()
                type: 'POST', //Method type
                url: AjaxCallUrl.CopySurvey,
                data: postData, //Forms name
                dataType: 'json',
                // traditional: true,
                beforeSend: function () {
                },
                success: function (data) {
                    if (data.IsSuccess) {
                        //  kendo.ui.progress($(".main-body-wrapper"), false);
                        var DataUpdateWindow = $("#DataUpdateMessage").data("kendoWindow");
                        if (DataUpdateWindow) {
                            DataUpdateWindow.title("Update Information");
                            DataUpdateWindow.content("<div class='msgboxSuccess'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i> Survey copy Successfully</div>");
                            DataUpdateWindow.open();
                            //redirect to listing page

                            var KendoListView = $("#listViewContractActiveAsc").data("kendoListView");
                            KendoListView.dataSource.read();
                        }
                    }
                },
                error: function (request, error) {
                    //  kendo.ui.progress($(".main-body-wrapper"), false);
                    if (DataUpdateWindow) {
                        DataUpdateWindow.title("Error Information");
                        DataUpdateWindow.content("<div class='msgboxError'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i>" + data.ModelError + " </div>");
                        DataUpdateWindow.open();
                    }
                }
            });
        }
    },
    ViewSurvey: function () {

        var Rad = $("input[name='RadSurvey']:checked");
        var SurveyId = Rad.siblings().filter("input[name='HdnSurvey']").val();
        if (parseInt(SurveyId) > 0) {
            var url = AjaxCallUrl.PreviewSurveyUrl.replace("****id****", SurveyId);
            var win = window.open(url, '_blank');
            win.focus();
        }
    },
    publishSurvey: function () {
        var Rad = $("input[name='RadSurvey']:checked");
       
        var SurveyId = Rad.siblings().filter("input[name='HdnSurvey']").val();
        if (parseInt(SurveyId) > 0) {
            var url = AjaxCallUrl.RedirectSurveyDeployUrl.replace("****SurveyId****", SurveyId);
            window.location.href = url;
        }
    },
    ArchivedSurvey: function () {

        var Rad = $("input[name='RadSurvey']:checked");
        var SurveyId = Rad.siblings().filter("input[name='HdnSurvey']").val();
        var Archive = Rad.siblings().filter("input[name='HdnArchive']").val();
        var Pub = Rad.siblings().filter("input[name='HdnPub']").val();
       
        if (Archive == 'n') {
       
        if (parseInt(SurveyId) > 0) {
            var urldata = AjaxCallUrl.ArchievedStatusUrl;
              $.ajax({ //Process the form using $.ajax()
            type: 'POST', //Method type
            url: urldata,
            data: { surveyid: SurveyId }, //Forms name
            dataType: 'json',
            beforeSend: function () {
            },
            error: function (request, error) {
                // $(document).ajaxStop($.unblockUI);
                // console.log('gggg');
            },
            success: function (data) {
                if (data.IsSuccess) {
                    var KendoListView = $("#listViewContractActiveAsc").data("kendoListView");
                    KendoListView.dataSource.read();
                }
                else {
                }

            }
        });

       
        }
    }
        else
        {
            if (Pub == 'false') {
               
                if (parseInt(SurveyId) > 0) {
                    var urldata = AjaxCallUrl.ArchievedStatusUrl;
                    $.ajax({ //Process the form using $.ajax()
                        type: 'POST', //Method type
                        url: urldata,
                        data: { surveyid: SurveyId }, //Forms name
                        dataType: 'json',
                        beforeSend: function () {
                        },
                        error: function (request, error) {
                            // $(document).ajaxStop($.unblockUI);
                            // console.log('gggg');
                        },
                        success: function (data) {
                            if (data.IsSuccess) {
                                var KendoListView = $("#listViewContractActiveAsc").data("kendoListView");
                                KendoListView.dataSource.read();
                            }
                            else {
                            }

                        }
                    });


                }
            }
            else
            {
                alert('Live & Published surveys cannot be Archived.');
            }
           
        }
    }

}
//function SendContractIdAsParameterOrder() {

//    return { Type: type, Flag: flag, PageValue: $("#HdnResourcePageValue").val() };

//}
function surveyresponse(surveyid, IsCompleted) {
    var url = AjaxCallUrl.RedirectToSurveyResponseUrl.replace("****SurveyId****", surveyid).replace("****IsCompleted****", parseInt(IsCompleted));
    //url = url.replace("****IsCompleted****", parseInt(IsCompleted));
    //var win = window.open(url, '_blank');
    //win.focus();    
    window.location.href = url;
}

function RedirectToSurveyResponse(surveyid, IsCompleted) {
    var url = AjaxCallUrl.RedirectContractUrl.replace("****SurveyId****", surveyid).replace("****IsCompleted****", parseInt(IsCompleted));
    //url = url.replace("****IsCompleted****", parseInt(IsCompleted));
    //var win = window.open(url, '_blank');
    //win.focus();    
    window.location.href = url;
}