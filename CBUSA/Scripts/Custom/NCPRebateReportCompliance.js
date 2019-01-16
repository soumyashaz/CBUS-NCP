$(document).ready(function () {
    ContractCompliance.AssignControl();

    $('thead').first().css('display', 'inline-block');
    $('thead').first().css('width', '1140');
    $('tbody').first().css('display', 'inline-block');
    $('tbody').first().css('height', '400px');
    $('tbody').first().css('width', '1140');
});

var ContractCompliance = {

    AssignControl: function () {
        // console.log($("a[name='a_ConfigureCompliance']").length);

        //  $("[data-popupconfigurecompliance='required']").on("click", ContractCompliance.LoadConfigureComplianceWindow);
        // $("[data-popupoverideconfigurecompliance='required']").on("click", ContractCompliance.LoadOvverideConfigureComplianceWindow);
    },
    LoadConfigureComplianceWindow: function () {
        // console.log($(this).attr('data-val'));

        var wnd = $("#WndConfigureCompliance").data("kendoWindow");
        wnd.refresh({
            url: AjaxCallUrl.WndConfigureComplianceUrl, // returns JSON, { firstName: "Alyx", lastName: "Vance" }
            data: { ContractId: $(this).attr('data-val') },
        });
        wnd.open().center();
    },
    LoadOvverideConfigureComplianceWindow: function () {
        //  console.log('hello1');
        var wnd = $("#WndOvverideConfigureCompliance").data("kendoWindow");
        wnd.refresh({
            url: AjaxCallUrl.WndOvverideConfigureComplianceUrl, // returns JSON, { firstName: "Alyx", lastName: "Vance" }
            data: { ContractId: $(this).attr('data-val'), Flag: 0 },
        });
        wnd.open().center();
    },

    OnOpenWndConfigureCompliance: function (e) {
        var wnd = $("#WndConfigureCompliance").data("kendoWindow");
        kendo.ui.progress(wnd.element, true);
    },
    OnRefreshWndConfigureCompliance: function (e) {
        var wnd = $("#WndConfigureCompliance").data("kendoWindow");
        kendo.ui.progress(wnd.element, false);
        // AssignEventContractResource();
        ContractCompliance.AssginControlConfigureCompliance();


    },

    AssginControlConfigureCompliance: function () {

        $("#BtnSaveCompliance").on("click", ContractCompliance.SaveContractComplianceDate);
        $("#CancelCompliance").on("click", ContractCompliance.CancelContractComplianceDate);

        $("#HdnTab").val(1);

        $("a[name='a_tab']").on("click", function () {

            $("#HdnTab").val($(this).attr('href'));
            var Tab = $(this).attr('href');

            if (Tab == "#DivConfEstimatvalue")  // 1 - estimate
            {
                $("#HdnTab").val(1);
            }
            else  // 2 - actual
            {
                $("#HdnTab").val(2);
            }

        });
    },

    OnOpenWndOvverideConfigureCompliance: function (e) {
        var wnd = $("#WndOvverideConfigureCompliance").data("kendoWindow");
        kendo.ui.progress(wnd.element, true);
    },
    OnRefreshWndOvverideConfigureCompliance: function (e) {
        var wnd = $("#WndOvverideConfigureCompliance").data("kendoWindow");
        kendo.ui.progress(wnd.element, false);
        ContractCompliance.AssginControlOverrideConfigureCompliance();

    },
    AssginControlOverrideConfigureCompliance: function () {

        $("#a_AddMoreRows").on("click", ContractCompliance.AddMoreRow);
        $("#Btn_SaveOverrideCompliance").on("click", ContractCompliance.SaveOverrideContractCompliance);
        $("#Btn_CancelOverrideCompliance").on("click", ContractCompliance.CancelOverrideContractCompliance);
    },
    SaveContractComplianceDate: function () {
        var Validator = $("#DivComplianceMainContainer").kendoValidator({
            messages: {
                required: "*",
            },
            rules: {
                required: function (input) {
                    if (input.is("[required=required]")) {
                        // var dropdownlist = $("#ContractStatus").data("kendoDropDownList");
                        var id = input.attr('id');
                        if ($("#HdnTab").val() == "1") {     /// Estimated 
                            if (id == "EstimatedSurveyId" || id == "EstimatedQuestionId") {
                                return input.val().length > 0;
                            }
                        }
                        else if ($("#HdnTab").val() == "2") {   //Actual
                            if (id == "ActualsSurveyId" || id == "ActualQuestionId") {
                                return input.val().length > 0;
                            }
                            if (id == "Year" || id == "Quater") {
                               

                                return input.val() != "0";
                            }
                        }
                        return true;
                    }
                    return true;
                }
            }
        }).data("kendoValidator");
        var v = Validator.validate();
       // console.log(v);

        if (v) {


            var IsstimateDirectQuestion = false;
            var CheckRadIsEstimate = $("input[name='IsEstimateDirectQuestion']:checked");
            if (CheckRadIsEstimate.val() == "directquestion") {
                IsstimateDirectQuestion = true;
            }

            var IsActualDirectQuestion = false;
            var CheckRadIsActual = $("input[name='IsActualDirectQuestion']:checked");
            if (CheckRadIsActual.val() == "directquestion") {
                IsActualDirectQuestion = true;
            }

            var postData = {
                ContractId: $("#HdnContractId").val(),
                EstimatedSurveyId: $("#EstimatedSurveyId").val(),
                EstimatedQuestionId: $("#EstimatedQuestionId").val(),
                EstimatedComposeFormula: $("#EstimatedComposeFormula").val(),
                IsEstimateDirectQuestion: IsstimateDirectQuestion,

                ActualsSurveyId: $("#ActualsSurveyId").val(),
                ActualQuestionId: $("#ActualQuestionId").val(),
                ActualComposeFormula: $("#ActualComposeFormula").val(),
                IsActualDirectQuestion: IsActualDirectQuestion,

                IsEstimated: parseInt($("#HdnTab").val()) == 1 ? true : false,
                Year: $("#Year").val(),
                Quater: $("#Quater").val()


            }; //done

           

            $.ajax({ //Process the form using $.ajax()
                type: 'POST', //Method type
                url: AjaxCallUrl.SaveContractComplianceUrl,
                data: postData, //Forms name
                dataType: 'json',
                // traditional: true,
                beforeSend: function () {
                },
                success: function (data) {
                    var DataUpdateWindow = $("#DataUpdateMessage").data("kendoWindow");
                    if (data.IsSuccess) {
                        kendo.ui.progress($("#SectionAddQuestion"), false);
                       

                        if (DataUpdateWindow) {
                            DataUpdateWindow.title("Update Information");
                            DataUpdateWindow.content("<div class='msgboxSuccess'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i> Data Update Successfully</div>");
                            DataUpdateWindow.open();
                        }
                    }
                    else {
                        kendo.ui.progress($("#SectionAddQuestion"), false);
                        if (DataUpdateWindow) {
                            DataUpdateWindow.title("Error Information");
                            DataUpdateWindow.content("<div class='msgboxError'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i>" + data.ModelError + " </div>");
                            DataUpdateWindow.open();
                        }
                    }
                },
                error: function (request, error) {
                    kendo.ui.progress($("#SectionAddQuestion"), false);
                    if (DataUpdateWindow) {
                        DataUpdateWindow.title("Error Information");
                        DataUpdateWindow.content("<div class='msgboxError'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i>" + data.ModelError + " </div>");
                        DataUpdateWindow.open();
                    }
                }
            });

        }
    },
    CancelContractComplianceDate: function () {
        var DataUpdateWindow = $("#WndConfigureCompliance").data("kendoWindow");
        if (DataUpdateWindow) {
            DataUpdateWindow.close();
        }
    },

    AddMoreRow: function () {
        kendo.ui.progress($("#table_complaincelist"), true);
        var postData = {
            ContractId: $("#HdnContractId").val(),
            Count: parseInt($("#HdnNoOfDropDown").val()) + 1
        };

        $.ajax({ //Process the form using $.ajax()
            type: 'POST', //Method type
            url: AjaxCallUrl.AddNewRowUrl,
            data: postData, //Forms name
            dataType: 'json',
            // traditional: true,
            beforeSend: function () {
            },
            success: function (data) {
                if (data.IsSuccess) {
                    kendo.ui.progress($("#table_complaincelist"), false);
                    $('#table_complaincelist tr:last').prev().after(data.PartialView);
                    $("#HdnNoOfDropDown").val(parseInt($("#HdnNoOfDropDown").val()) + 1);
                }
                else {
                    kendo.ui.progress($("#table_complaincelist"), false);

                }
            },
            error: function (request, error) {
                kendo.ui.progress($("#table_complaincelist"), false);

            }
        });


    },

    SaveOverrideContractCompliance: function () {
        var Validator = $("#table_complaincelist").kendoValidator({
            messages: {
                required: "*",
                greaterthanzero: "Plese enter correct value"
            },
            rules: {
                required: function (input) {
                    if (input.is("[required=required]")) {
                        return input.val().length > 0;
                    }
                    return true;
                },
                greaterthanzero: function (input) {
                    if (input.is("[required=required]")) {
                        return parseFloat(input.val()) > 0;
                    }
                    return true;
                },
            }
        }).data("kendoValidator");
        var v = Validator.validate();
        if (v) {
            kendo.ui.progress($("#DivContractComplianceBuilder"), true);
            var Tr = $('#table_complaincelist tr[data-role="Modfied"]');
            var ContractBuilderComplianceList = [];
            Tr.each(function () {
                ContractBuilderComplianceList.push({
                    BuilderId: $(this).find("input[id^='ContractBuilder_']").val(),
                    NewlEstilamteValue: $(this).find("input[name^='Txt_NewValue_Estimate']").val(),
                    NewActualValue: $(this).find("input[name^='Txt_NewValue_Actual']").val()
                });
            });

            var postData = {
                ContractBuilderComplianceList: ContractBuilderComplianceList,
                ContractId: $("#HdnContractId").val()
            }; //done
            $.ajax({ //Process the form using $.ajax()
                type: 'POST', //Method type
                url: AjaxCallUrl.SaveContractComplianceBuilderUrl,
                data: postData, //Forms name
                dataType: 'json',
                // traditional: true,
                beforeSend: function () {
                },
                success: function (data) {
                    var DataUpdateWindow = $("#DataUpdateMessage").data("kendoWindow");
                    if (data.IsSuccess) {
                        kendo.ui.progress($("#DivContractComplianceBuilder"), false);


                        if (DataUpdateWindow) {
                            DataUpdateWindow.title("Update Information");
                            DataUpdateWindow.content("<div class='msgboxSuccess'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i> Data Update Successfully</div>");
                            DataUpdateWindow.open();
                        }
                    }
                    else {
                        kendo.ui.progress($("#DivContractComplianceBuilder"), false);
                        if (DataUpdateWindow) {
                            DataUpdateWindow.title("Error Information");
                            DataUpdateWindow.content("<div class='msgboxError'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i>" + data.ModelError + " </div>");
                            DataUpdateWindow.open();
                        }
                    }
                },
                error: function (request, error) {
                    var DataUpdateWindow = $("#DataUpdateMessage").data("kendoWindow");
                    kendo.ui.progress($("#DivContractComplianceBuilder"), false);
                    if (DataUpdateWindow) {
                        DataUpdateWindow.title("Error Information");
                        DataUpdateWindow.content("<div class='msgboxError'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i>" + data.ModelError + " </div>");
                        DataUpdateWindow.open();
                    }
                }
            });

        }
    },

    CancelOverrideContractCompliance: function () {
        var DataUpdateWindow = $("#WndOvverideConfigureCompliance").data("kendoWindow");
        if (DataUpdateWindow) {
            DataUpdateWindow.close();
        }
    },



}


function listActiveContractDataBound() {



    $("a[name='a_ConfigureCompliance']").on("click", ContractCompliance.LoadConfigureComplianceWindow);
    $("a[name='a_OverrideCompliance']").on("click", ContractCompliance.LoadOvverideConfigureComplianceWindow);
}

function SendContractIdasParameter() {
    return { ContractId: $("#HdnContractId").val() };
}

function SendDataToLoadNcpSurvey() {
    return { ContractId: $("#HdnContractId").val(), Year: $("#Year").val(), Quater: $("#Quater").val() };
}




//function onSelectEstimateSurvey(e) {
//    var dataItem = this.dataItem(e.item.index());
//    // console.log('dfsd')
//    //  console.log(dataItem.SurveyId);
//    // $("#HdnSurveyIdTemp").val(dataItem.SurveyId);
//    $("#HdnSurveyIdTemp").val(dataItem.SurveyId);
//    var dropdownlist = $("#EstimatedQuestionId").data("kendoDropDownList");
//    dropdownlist.dataSource.read();
//}


//function onSelectActualSurvey(e) {
//    var dataItem = this.dataItem(e.item.index());
//    // console.log('ee')
//    //  console.log(dataItem.SurveyId);
//    $("#HdnSurveyIdTemp").val(dataItem.SurveyId);
//    var dropdownlist = $("#ActualQuestionId").data("kendoDropDownList");
//    dropdownlist.dataSource.read();
//}

function SendEstimatedSurveyIdAsParameter() {
    var SurveyId = $("#EstimatedSurveyId").val();
    return { SurveyId: SurveyId };
}

function SendActualSurveyIdAsParameter() {
    var SurveyId = $("#ActualsSurveyId").val();
    return { SurveyId: SurveyId };
}

function numericFilter(txb) {
    txb.value = txb.value.replace(/[^\0-9]/ig, "");
}

//function onDataBoundEstimateSurvey(e) {
//    var dataItem = this.dataItem(e.item);
//  //  console.log(dataItem.SurveyId);

//    $("#HdnSurveyIdTemp").val(dataItem.SurveyId);
//    var dropdownlist = $("#EstimatedQuestionId").data("kendoDropDownList");
//    dropdownlist.dataSource.read();
//}

//function onDataBoundActualsSurvey(e) {
//    var dataItem = this.dataItem(e.item);
//   // console.log(dataItem.SurveyId);

//    $("#HdnSurveyIdTemp").val(dataItem.SurveyId);
//    var dropdownlist = $("#ActualQuestionId").data("kendoDropDownList");
//    dropdownlist.dataSource.read();
//}

function onSelectContractBuilder(e) {

    var dataItem = this.dataItem(e.item);
    // console.log(dataItem.Value);
    if (parseInt(dataItem.Value) > 0) {
        kendo.ui.progress($("#table_complaincelist"), true);
        postData = { ContractId: $("#HdnContractId").val(), BuilderId: dataItem.Value };

        $.ajax({ //Process the form using $.ajax()
            type: 'POST', //Method type
            url: AjaxCallUrl.GetBuilderActualComplianceFactorUrl,
            data: postData, //Forms name
            dataType: 'json',
            // traditional: true,
            beforeSend: function () {
            },
            success: function (data) {
                if (data.IsSuccess) {
                    kendo.ui.progress($("#table_complaincelist"), false);
                    // console.log($(e.item).attr('id'));
                    var Tr = $("span[aria-activedescendant='" + $(e.item).attr('id') + "']").parent().parent();

                    Tr.find("input[name='Txt_Orginal_Estimate']").val(data.OrginalEstilamteValue);
                    Tr.find("input[name='Txt_Orginal_Actual']").val(data.OrginalActualValue);
                }
                else {
                    kendo.ui.progress($("#table_complaincelist"), false);
                }
            },
            error: function (request, error) {
                kendo.ui.progress($("#table_complaincelist"), false);

            }
        });
    }

}

