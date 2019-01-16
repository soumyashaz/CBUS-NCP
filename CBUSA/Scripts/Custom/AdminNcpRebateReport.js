var type;
var flag;
$("#asccon").click(function () {
    $('#descicon').show();
    $('#ascicon').hide();
    var UserDetails = asccon;
    type = 'asccon';
    var KendoListView = $("#listViewContractActiveAsc").data("kendoListView");
    KendoListView.dataSource.read();
});
$("#desccon").click(function () {
    $('#descicon').hide();
    $('#ascicon').show();
    var UserDetails = asccon;
    type = 'desccon';
    var KendoListView = $("#listViewContractActiveAsc").data("kendoListView");
    KendoListView.dataSource.read();
});


$("#ascncp").click(function () {
    $('#descincp').show();
    $('#ascincp').hide();

    type = 'ascncp';
    var KendoListView = $("#listViewContractActiveAsc").data("kendoListView");
    KendoListView.dataSource.read();
});

$("#descncp").click(function () {
    $('#descincp').hide();
    $('#ascincp').show();

    type = 'descncp';
    var KendoListView = $("#listViewContractActiveAsc").data("kendoListView");
    KendoListView.dataSource.read();
});
$("#ascbj").click(function () {
    $('#descibj').show();
    $('#ascibj').hide();

    type = 'ascbj';
    var KendoListView = $("#listViewContractActiveAsc").data("kendoListView");
    KendoListView.dataSource.read();
});
$("#descbj").click(function () {
    $('#descibj').hide();
    $('#ascibj').show();

    type = 'descbj';
    var KendoListView = $("#listViewContractActiveAsc").data("kendoListView");
    KendoListView.dataSource.read();
});
$("#ascpc").click(function () {
    $('#descipc').show();
    $('#ascipc').hide();

    type = 'ascpc';
    var KendoListView = $("#listViewContractActiveAsc").data("kendoListView");
    KendoListView.dataSource.read();
});
$("#descpc").click(function () {
    $('#descipc').hide();
    $('#ascipc').show();

    type = 'descpc';
    var KendoListView = $("#listViewContractActiveAsc").data("kendoListView");
    KendoListView.dataSource.read();
});
$("#ascst").click(function () {
    $('#descist').show();
    $('#ascist').hide();

    type = 'ascst';
    var KendoListView = $("#listViewContractActiveAsc").data("kendoListView");
    KendoListView.dataSource.read();
});
$("#descst").click(function () {
    $('#descist').hide();
    $('#ascist').show();

    type = 'descst';
    var KendoListView = $("#listViewContractActiveAsc").data("kendoListView");
    KendoListView.dataSource.read();
});
$("#ascct").click(function () {
    $('#descict').show();
    $('#ascict').hide();

    type = 'ascct';
    var KendoListView = $("#listViewContractActiveAsc").data("kendoListView");
    KendoListView.dataSource.read();
});
$("#descct").click(function () {
    $('#descict').hide();
    $('#ascict').show();

    type = 'descct';
    var KendoListView = $("#listViewContractActiveAsc").data("kendoListView");
    KendoListView.dataSource.read();
});
$("#arch").click(function () {

    var url = AjaxCallUrl.archievedcontracts;
    var win = window.open(url, '_blank');
    win.focus();


    //$('#descicon').hide();
    //$('#ascicon').show();
    //var UserDetails = asccon;
    //type = 'arch';

    //$("#ulTask #acli").removeClass("active-mang-cont");
    //$("#ulTask #archli").show();

    //var KendoListView = $("#listViewContractActiveAsc").data("kendoListView");
    //KendoListView.dataSource.read();
});

$("#asccont").click(function () {

    $('#descicon').show();
    $('#ascicon').hide();

    type = 'asccon';
    flag = null;
    var KendoListView = $("#listViewContractActiveAsc").data("kendoListView");
    KendoListView.dataSource.read();
});
$("#desccont").click(function () {
    $('#descicon').hide();
    $('#ascicon').show();

    type = 'desccon';
    flag = null;
    var KendoListView = $("#listViewContractActiveAsc").data("kendoListView");
    KendoListView.dataSource.read();
});


$("#ascser").click(function () {
    $('#descincp').show();
    $('#ascincp').hide();

    type = 'ascncp';
    flag = null;
    var KendoListView = $("#listViewContractActiveAsc").data("kendoListView");
    KendoListView.dataSource.read();
});
$("#descser").click(function () {
    $('#descincp').hide();
    $('#ascincp').show();

    type = 'descncp';
    flag = null;
    var KendoListView = $("#listViewContractActiveAsc").data("kendoListView");
    KendoListView.dataSource.read();
});
$("#ascyr").click(function () {
    $('#desciyr').show();
    $('#asciyr').hide();

    type = 'ascyr';
    flag = null;
    var KendoListView = $("#listViewContractActiveAsc").data("kendoListView");
    KendoListView.dataSource.read();
});
$("#descyr").click(function () {
    $('#desciyr').hide();
    $('#asciyr').show();

    type = 'descyr';
    flag = null;
    var KendoListView = $("#listViewContractActiveAsc").data("kendoListView");
    KendoListView.dataSource.read();
});
$("#ascdue").click(function () {

    $('#descidue').show();
    $('#ascidue').hide();

    type = 'ascdue';
    flag = null;
    var KendoListView = $("#listViewContractActiveAsc").data("kendoListView");
    KendoListView.dataSource.read();
});
$("#descdue").click(function () {
    $('#descidue').hide();
    $('#ascidue').show();

    type = 'descdue';
    flag = null;
    var KendoListView = $("#listViewContractActiveAsc").data("kendoListView");
    KendoListView.dataSource.read();
});

function RedirecttoContract(contractid) {
    var url = AjaxCallUrl.RedirectContractUrl.replace("****ContractId****", contractid);
    window.location.href = url;
}
function SendContractIdAsParameterOrder() {

    return { Type: type, Flag: flag, PageValue: $("#HdnResourcePageValue").val() };

}


function onSelectContractStatus(e) {

    var typead = this.dataItem(e.item.index());
    flag = typead.ContractId;
    type = null;

    var KendoListView = $("#listViewContractActiveAsc").data("kendoListView");
    KendoListView.dataSource.read();
}



$(document).ready(function () {


    // SurveyAddQuestion.AssignControl();
    ManageNcpSurveyList.AssignControl();

    $('thead').first().css('display', 'block');
    $('thead').first().css('width', '100%');
    $('tbody').first().css('display', 'block');
    $('tbody').first().css('height', '500px');
    $('tbody').first().css('width', '100%');


});

ManageNcpSurveyList = {

    AssignControl: function () {

        $("#BtnEdit").on("click", ManageNcpSurveyList.EditNcpSurvey);
        $("#BtnCopyNCP").on("click", ManageNcpSurveyList.CopyNcpSurvey);
        $("#BtnPublish").on("click", ManageNcpSurveyList.publishNcpSurvey);
        $("#BtnPreview").on("click", ManageNcpSurveyList.ViewActiveNcpSurvey);
        $("#BtnArchivedSurvey").on("click", ManageNcpSurveyList.ArchivedNcpSurvey);
    },
    EditNcpSurvey: function () {

        var Rad = $("input[name='RadSurvey']:checked");
        var SurveyId = Rad.siblings().filter("input[name='HdnSurvey']").val();
        //  console.log(SurveyId)
        if (parseInt(SurveyId) > 0) {
            var url = AjaxCallUrl.RedirectSurveyEditUrl.replace("****SurveyId****", SurveyId);
            window.location.href = url;
        }

    },
    CopyNcpSurvey: function () {

        var Rad = $("input[name='RadSurvey']:checked");
        var SurveyId = Rad.siblings().filter("input[name='HdnSurvey']").val();
        var ContractId = Rad.siblings().filter("input[name='HdnContract']").val();

        if (parseInt(SurveyId) > 0 && parseInt(ContractId) > 0) {



            var Confirm = confirm("Are you sure you want to copy the selected NCP Quarterly Rebate Report?");

            if (!Confirm) {
                return;
            }
            var DataUpdateWindow = $("#DataUpdateMessage").data("kendoWindow");
            // kendo.ui.progress(".main-body-wrapper", false);
            var postData = { ContractId: ContractId, SurveyId: SurveyId, IsNcp: 'true' }; //done
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
                       
                        if (DataUpdateWindow) {
                            DataUpdateWindow.title("Update Information");
                            DataUpdateWindow.content("<div class='msgboxSuccess'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i> Data Update Successfully</div>");
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
    ViewActiveNcpSurvey: function () {


        //Note : This function is for only Active NCP Survey
        var Rad = $("input[name='RadSurvey']:checked");
        var SurveyId = Rad.siblings().filter("input[name='HdnSurvey']").val();
        if (parseInt(SurveyId) > 0) {
            var url = AjaxCallUrl.PreviewSurveyUrl.replace("****id****", SurveyId);
            var win = window.open(url, '_blank');
            win.focus();
        }
    },


    publishNcpSurvey: function () {
        var Rad = $("input[name='RadSurvey']:checked");

        var SurveyId = Rad.siblings().filter("input[name='HdnSurvey']").val();
        if (parseInt(SurveyId) > 0) {
            var url = AjaxCallUrl.RedirectSurveyDeployUrl.replace("****SurveyId****", SurveyId);
            window.location.href = url;
        }
    },
    ArchivedNcpSurvey: function () {

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
        else {
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
            else {
                alert('Live & Published surveys cannot be Archived.');
            }

        }
    }
}
function surveyresponse(surveyid, IsCompleted) {

    var url = AjaxCallUrl.RedirectContractUrl.replace("****SurveyId****", surveyid);
    url = url.replace("****IsCompleted****", IsCompleted);
    //var win = window.open(url, '_blank');
    //win.focus();
    window.location.href = url;

}