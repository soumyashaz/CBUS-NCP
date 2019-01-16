var FormulaoldVal = '';
var FormulaTypingallowedValues = ['(){}+-*/%', '0123456789'];
var MarketListForEdit = '';
$(document).ready(function () {
    ConstructFormula.AssignControl();
    var SelectedQuarter = $('#hdnSelectedQuarter').val();
    var SelectedYear = $('#hdnSelectedYear').val();
    if (SelectedQuarter != undefined && SelectedQuarter != '' && SelectedQuarter != null) {
        var dropdownlist = $("#QuarterDDL").data("kendoDropDownList");
        dropdownlist.value(SelectedQuarter + '-' + SelectedYear);
        dropdownlist.refresh();
    }

    if ($("#DivConfigureSurveyInvite").length) {
        var SelectedData = $("#DivConfigureSurveyInvite").val();
        //alert('DivConfigureSurveyInvite SelectedData -> ' + SelectedData);
        $('html').on('click', function () {

            // do your stuff here
            var ChildDiv = $("#DivZoneState").children(".CustomControlPoupZoneState");

            if ($(ChildDiv).is(':visible')) {
                $(ChildDiv).hide();
            }
        }).find('.classZoneMarket').on('click', function (e) {
            e.stopPropagation();
        });
    }
    $("#DivZoneStateDropdown").on("click", function () {

        var SelectedMarket = $("#ZoneStateList").children().find("input[name='ChkStateSelect']:checked");
        var Markets = '';
        $(SelectedMarket).each(function () {
            // Markets.push($(this).siblings().eq(0).val());
            Markets += $(this).siblings().eq(0).val() + ','
        });
        if (Markets.length > 0) {
            $('#hdnSelectedMarketList').val(Markets);
            MarketListForEdit = Markets;
        }
        else {
            $("#HdnSelectedMarketControl").val(MarketListForEdit);
            //alert('MarketListForEdit ---- ' + MarketListForEdit);
        }
        CustomControlZoneStateList($(this).parent().parent(), "CustomControlPoupZoneState");
    });
    if ($('#hdnSelectedMarketList').val() != '') {
        MarketListForEdit = $('#hdnSelectedMarketList').val();
    }

});
$(document).ready(function () {

    $("#DivZoneStateDropdown").trigger("click");
    $("html").trigger("click");

});
$("#BtnInsertFormula").on("click", function () {
    if ($("#hdnSelectedQuestionId").val() > 0) {
        var DdlQuestionColumn = $("#QuestionColumnDDL").data("kendoDropDownList");
        var SplitedDataQuestionColumn = DdlQuestionColumn.value().split('_');
        $("#hdnSelectedQuestionSettingId").val(SplitedDataQuestionColumn[0]);
        $("#hdnSelectedQuestionColumnIndex").val(SplitedDataQuestionColumn[1]);
        $("#hdnSelectedQuestionColumnText").val(DdlQuestionColumn.text());

        var DdlQuestionColumnValue = $("#QuestionColumnValueDDL").data("kendoDropDownList");
        var SplitedDataQuestionColumnValue = DdlQuestionColumnValue.value().split('_');
        //var SplitedData = dataItem.QuestionColumnValueId.split('_');
        $("#hdnSelectedQuestionColumnValueId").val(SplitedDataQuestionColumnValue[0]);
        $("#hdnSelectedQuestionRowIndex").val(SplitedDataQuestionColumnValue[1]);
        $("#hdnSelectedQuestionColumnValueText").val(DdlQuestionColumnValue.text());

        var CreateFormulaString = "~~~~" + $("#hdnSelectedQuestionText").val();
        //var CreateFormulaString = "[";
        var QuestionTypeId = $("#hdnSelectedQuestionTypeId").val();
        if (QuestionTypeId == 3) {
            //alert('col id ' + $("#hdnSelectedQuestionSettingId").val() + '   col value id ' + $("#hdnSelectedQuestionColumnValueId").val());
            if ($("#hdnSelectedQuestionColumnText").val().length > 0 && $("#hdnSelectedQuestionSettingId").val() >= 0 && $("#hdnSelectedQuestionColumnText").val() != "N/A") {
                CreateFormulaString += "~@$" + $("#hdnSelectedQuestionColumnText").val();
            }
            if ($("#hdnSelectedQuestionColumnValueText").val().length > 0 && $("#hdnSelectedQuestionColumnValueId").val() >= 0) {
                CreateFormulaString += "~@$" + $("#hdnSelectedQuestionColumnValueText").val();
            }
        }
        if (QuestionTypeId == 2) {
            if ($("#hdnSelectedQuestionColumnValueText").val().length > 0 && $("#hdnSelectedQuestionColumnValueId").val() >= 0) {
                CreateFormulaString += "~@$" + $("#hdnSelectedQuestionColumnValueText").val();
            }
        }
        CreateFormulaString += "~~##";

        //var CreateFormulaDefinition = "~" + $("#hdnSelectedQuestionId").val();
        //if ($("#hdnSelectedQuestionColumnValueId").val() > 0 && $("#hdnSelectedQuestionColumnText").val() != "N/A") {
        //    CreateFormulaDefinition += "_" + $("#hdnSelectedQuestionColumnValueId").val();
        //}
        //if ($("#hdnSelectedQuestionRowIndex").val() > 0) {
        //    CreateFormulaDefinition += "_" + parseInt($("#hdnSelectedQuestionRowIndex").val() - 1) + "_" + parseInt($("#hdnSelectedQuestionColumnIndex").val() - 1);
        //}
        //CreateFormulaDefinition += "#";
        ////var PreviousData = $("#TxtFormula").val() + CreateFormulaString.replace(/ /g, "_").replace(/<br\s?\/?>/g, "\n");;
        var PreviousData = $("#TxtFormula").val() + CreateFormulaString;
        //var PreviousDataActual = $("#TxtFormulaActual").val() + CreateFormulaDefinition;
        //if ($('#TxtFormula').val != '') {
        //    SetCaretAtEnd(TxtFormula);
        //} // --- to insert string at end of text box
        insertAtCaret('TxtFormula', PreviousData);
        $("#TxtFormula").val(PreviousData);
        //$("#TxtFormulaActual").val(PreviousDataActual);
        ////SetCaretAtEnd(TxtFormula); // --- to insert string at end of text box
    }

});

$("#BtnSaveConstructFormula").on("click", function () {
    ConstructFormula.SaveConstructFormulaData();
});

$("#BtnCancelConstructFormula").on("click", function () {
    // $("#TxtFormula").val('');
    //  $("#TxtFormulaActual").val('');
    window.location = AjaxCallUrl.FormulaListingPageUrl;

});

$("#TxtFormula").keyup(function (e) {
    var Textval = $(this).val();
    var MatchedCharFound = false;
    var TypingValue = e.key;
    var typedValue = $(this).val(),
        valLength = typedValue.length;
    for (i = 0; i < FormulaTypingallowedValues.length; i++) {
        if (typedValue.toLowerCase() === FormulaTypingallowedValues[i].substr(0, valLength)) {
            MatchedCharFound = true;
            return;
        }
    }
    //if (MatchedCharFound)
    //{
    var PreviousText = $("#TxtFormulaActual").val();
    //val = val.replace(/[^\w]+/g, "");
    if (TypingValue != 'Shift' && TypingValue != 'Control' && TypingValue != 'Alt' && TypingValue != 'Backspace' && TypingValue != 'ArrowLeft' && TypingValue != 'ArrowRight' && TypingValue != 'ArrowUp' && TypingValue != 'ArrowDown' && TypingValue != 'Delete' && TypingValue != 'Meta' && TypingValue != 'Unidentified') {
        $("#TxtFormulaActual").val(PreviousText + TypingValue);
    }
    if (TypingValue == 'Backspace' && $("#TxtFormulaActual").val().slice(0, -1) != "]" && $("#TxtFormulaActual").val().slice(0, -1) != "[") {
        $("#TxtFormulaActual").val($("#TxtFormulaActual").val().slice(0, -1));
    }
    //}
    //else
    //{
    //    $("#TxtFormula").val($("#TxtFormula").val().slice(0, -1));
    //}
    //$("#TxtFormula").empty().val(typedValue.substr(0, valLength - 1));        
    //FormulaoldVal = TypingValue;
    //alert('TypingValue -> ' + TypingValue);
});


function TextCurrPosition(el, FormulaData) {
    var val = el.value;
    //alert(val.slice(0, el.selectionStart).length + '   ->  ' + FormulaData);
    $("#TxtFormula").val(CreateFormulaString);
}

var ConstructFormula = {
    AssignControl: function () {
        $("#a_openaddinvitespopup").on("click", ConstructFormula.PopUpMarkets);
        //  $("#Btn_AddQuestion").on("click", SurveyAddQuestion.SaveNewQuestion);
    },

    onSelectContract: function (e) {
        var dataItem = this.dataItem(e.item.index());

        $("#hdnSelectedContractId").val(dataItem.ContractId);
        // var QuestionDropDownList = $("#QuestionDDL").data("kendoDropDownList");
        //QuestionDropDownList.dataSource.read();
        //console.log($("#hdnSelectedQuarter").val());
        if ($("#hdnSelectedQuarter").val() != "" || $("#hdnSelectedQuarter").val() != undefined) {
            var QuestionDropDownList = $("#QuarterDDL").data("kendoDropDownList");
            QuestionDropDownList.value("0");
            $("#hdnSelectedQuarter").val('0');
        }
    },
    onDataBoundContract: function () {
        var Dropdownlist = $("#ContractList").data("kendoDropDownList");
        if ($("#ContractList").data("kendoDropDownList") != null) {
            var ContractId = $("#ContractList").data("kendoDropDownList").value();
            //Dropdownlist.select(0);
            $("#hdnSelectedContractId").val(ContractId);
        }
    },
    onSelectQuarter: function (e) {
        var dataItem = this.dataItem(e.item.index());
        var QuarterName = dataItem.Value.substring(0, 2);
        var QuarterYear = dataItem.Value.substring(5, 9);
        $("#hdnSelectedQuarter").val(QuarterName);
        $("#hdnSelectedYear").val(QuarterYear);
        var FormData = {
            'ContractId': $("#hdnSelectedContractId").val(),
            'Quarter': QuarterName,
            'Year': QuarterYear
        };
        $.ajax({ //Process the form using $.ajax()
            type: 'GET', //Method type
            url: AjaxCallUrl.GetSurveyUrl,
            data: FormData, //Forms name
            dataType: 'json',
            async: false,
            beforeSend: function () {
            },
            error: function (request, error) {

            },
            success: function (data) {
                $('#hdnSurveyId').val(data);
            }
        });
        var SurveyId = $('#hdnSurveyId').val();
        var FormData = {
            'SurveyId': SurveyId
        };
        var dropdownlist = $("#QuestionDDL").data("kendoDropDownList");

        if (dropdownlist) {
            // re-render the items in drop-down list.
            dropdownlist.refresh();
            //alert('from quarter selection dropdown refresh   survey ' + $("#hdnSurveyId").val() + '     quarter    ' + $("#hdnSelectedQuarter").val());
        }
        if (SurveyId == null || SurveyId == undefined || SurveyId == '' || SurveyId == '0' || SurveyId == 0) {
            var DataUpdateWindow = $("#DataUpdateMessage").data("kendoWindow");
            kendo.ui.progress($("#ConstructFormula"), false);
            if (DataUpdateWindow) {
                DataUpdateWindow.title("Information");
                DataUpdateWindow.content("<div class='msgboxError'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i>Volume Fee Formula cannot be constructed because NCP Rebate Report Survey is not available for the selected Contract and Quarter. </div>");
                DataUpdateWindow.open();
            }
        }
    },
    onSelectSameAsQuarter: function (e) {
        var dataItem = this.dataItem(e.item.index());
        var QuarterName = dataItem.Value.substring(0, 2);
        var QuarterYear = dataItem.Value.substring(5, 9);
        var SelectedQuarter = $('#hdnSelectedQuarter').val();
        var SelectedYear = $('#hdnSelectedYear').val();
        var FormData = {
            'ContractId': $("#hdnSelectedContractId").val(),
            'Quarter': QuarterName,
            'Year': QuarterYear,
            'CopyToQuarter': SelectedQuarter,
            'CopyToYear': SelectedYear
        };
        $.ajax({
            type: 'GET',
            url: AjaxCallUrl.GetSameAsQuarterUrl,
            data: FormData,
            dataType: 'json',
            async: false,
            beforeSend: function () {
            },
            error: function (request, error) {

            },
            success: function (data) {
                var SplitedValue = data.split('~');
                //alert(' same quarter formula -> ' + SplitedValue[0]);
                if (SplitedValue.length > 1) {
                    $('#TxtFormula').val(SplitedValue[0]);
                    $('#TxtFormulaActual').val(SplitedValue[1]);
                }
                else {
                    $('#TxtFormula').val('');
                    $('#TxtFormulaActual').val('');
                }
            }
        });
        var SurveyId = $('#hdnSurveyId').val();
        var FormData = {
            'SurveyId': SurveyId
        };
        var dropdownlist = $("#QuestionDDL").data("kendoDropDownList");

        if (dropdownlist) {
            // re-render the items in drop-down list.
            dropdownlist.refresh();
            //alert('from quarter selection dropdown refresh   survey ' + $("#hdnSurveyId").val() + '     quarter    ' + $("#hdnSelectedQuarter").val());
        }
    },
    SendDataBeforeLoadDropDown: function () {
        // console.log($("#IsNcpSurvey").val());
        if ($("#IsNcpSurvey").val().toLowerCase() === "true") {
            return { IsNcpSurvey: $("#IsNcpSurvey").val() };
        }
    },
    // ********************

    onSelectQuestion: function (e) {
        //  var dataItem = this.dataItem(e.item.index());
        var dropdownlist = $("#QuestionDDL").data("kendoDropDownList");

        //console.log(dropdownlist.text())
        //$("#hdnSelectedQuestionId").val(dataItem.QuestionId);
        //$("#hdnSelectedQuestionText").val(dataItem.QuestionValue);

        $("#hdnSelectedQuestionId").val(dropdownlist.value());
        $("#hdnSelectedQuestionText").val(dropdownlist.text());
        var FormData = {
            'SurveyId': $("#hdnSurveyId").val(),
            'QuestionId': $("#hdnSelectedQuestionId").val()
        };
        $.ajax({
            type: 'GET',
            url: AjaxCallUrl.GetQuestionTypeIdUrl,
            data: FormData,
            dataType: 'json',
            async: false,
            beforeSend: function () {
            },
            error: function (request, error) {
                $("#hdnSelectedQuestionTypeId").val('');
            },
            success: function (data) {
                $("#hdnSelectedQuestionTypeId").val(data);
            }
        });





    },
    onDataBoundQuestion: function (e) {

        //console.log(e);
        //alert('QuestionId bata bound -> ' + $("#QuestionId").data("kendoDropDownList"));
        var Dropdownlist = $("#QuestionDDL").data("kendoDropDownList");

        if ($("#QuestionDDL").data("kendoDropDownList") != null) {
            //Dropdownlist.select(1);
            var QuestionValue = $("#QuestionDDL").data("kendoDropDownList").value();
            var QuestionText = $("#QuestionDDL").data("kendoDropDownList").text();
            $("#hdnSelectedQuestionId").val(QuestionValue);
            $("#hdnSelectedQuestionText").val(QuestionText);

        }
    },
    onOpenQuestionList: function () {
        if ($("#QuestionDDL").data("kendoDropDownList") != null) {
            var QuestionId = $("#QuestionId").data("kendoDropDownList").value();
        }
    },
    onSelectQuestionColumn: function (e) {
        //var dataItem = this.dataItem(e.item.index());
        var dropdownlist = $("#QuestionColumnDDL").data("kendoDropDownList");
        var SplitedData = dropdownlist.value().split('_');
        $("#hdnSelectedQuestionSettingId").val(SplitedData[0]);
        $("#hdnSelectedQuestionColumnIndex").val(SplitedData[1]);
        $("#hdnSelectedQuestionColumnText").val(dropdownlist.text());

        //console.log(dropdownlist.text())
        //$("#hdnSelectedQuestionId").val(dataItem.QuestionId);
        //$("#hdnSelectedQuestionText").val(dataItem.QuestionValue);

    },
    onDataBoundQuestionColumn: function () {
        var Dropdownlist = $("#QuestionColumnDDL").data("kendoDropDownList");
        if ($("#QuestionColumnDDL").data("kendoDropDownList") != null) {
            var QuestionColumn = $("#QuestionColumnDDL").data("kendoDropDownList").value();
            var QuestionColumnText = $("#QuestionColumnDDL").data("kendoDropDownList").text();
            //Dropdownlist.select(0);            
            var SplitedData = QuestionColumn.split('_');
            $("#hdnSelectedQuestionSettingId").val(SplitedData[0]);
            $("#hdnSelectedQuestionColumnIndex").val(SplitedData[1]);
            $("#hdnSelectedQuestionColumnText").val(QuestionColumnText);

        }
    },
    onSelectQuestionColumnValue: function (e) {
        //var dataItem = this.dataItem(e.item.index());
        var dropdownlist = $("#QuestionColumnValueDDL").data("kendoDropDownList");
        var SplitedData = dropdownlist.value().split('_');
        //var SplitedData = dataItem.QuestionColumnValueId.split('_');
        $("#hdnSelectedQuestionColumnValueId").val(SplitedData[0]);
        $("#hdnSelectedQuestionRowIndex").val(SplitedData[1]);
        $("#hdnSelectedQuestionColumnValueText").val(dropdownlist.text());
    },
    onDataBoundQuestionColumnValue: function () {
        var Dropdownlist = $("#QuestionColumnValueDDL").data("kendoDropDownList");

        if ($("#QuestionColumnValueDDL").data("kendoDropDownList") != null) {
            var QuestionColumnValue = $("#QuestionColumnValueDDL").data("kendoDropDownList").value();
            var QuestionColumnText = $("#QuestionColumnValueDDL").data("kendoDropDownList").text();
            //Dropdownlist.select(0);

            var SplitedData = QuestionColumnValue.split('_');
            $("#hdnSelectedQuestionColumnValueId").val(SplitedData[0]);
            $("#hdnSelectedQuestionRowIndex").val(SplitedData[1]);
            $("#hdnSelectedQuestionColumnValueText").val(QuestionColumnText);

        }
    },
    SaveConstructFormulaData: function () {
        //var Validator = $("#DivComplianceMainContainer").kendoValidator({
        //    messages: {
        //        required: "*",
        //    },
        //    rules: {
        //        required: function (input) {
        //            if (input.is("[required=required]")) {
        //                // var dropdownlist = $("#ContractStatus").data("kendoDropDownList");
        //                var id = input.attr('id');
        //                if ($("#HdnTab").val() == "1") {     /// Estimated 
        //                    if (id == "EstimatedSurveyId" || id == "EstimatedQuestionId") {
        //                        return input.val().length > 0;
        //                    }
        //                }
        //                else if ($("#HdnTab").val() == "2") {   //Actual
        //                    if (id == "ActualsSurveyId" || id == "ActualQuestionId") {
        //                        return input.val().length > 0;
        //                    }
        //                    if (id == "Year" || id == "Quater") {
        //                        return input.val() != "0";
        //                    }
        //                }
        //                return true;
        //            }
        //            return true;
        //        }
        //    }
        //}).data("kendoValidator");
        //var v = Validator.validate();
        // console.log(v);
        var v = true;
        var SelectedMarket = $("#ZoneStateList").children().find("input[name='ChkStateSelect']:checked");
        var Markets = '';
        $(SelectedMarket).each(function () {
            // Markets.push($(this).siblings().eq(0).val());
            Markets += $(this).siblings().eq(0).val() + ','
        });

        if (Markets.length == 0) {
            if ($("#hdnSelectedMarketList").length == 0) {
                v = false;
            }
        }
        else {
            $("#hdnSelectedMarketList").val(Markets);
        }

        if (v) {
            var postData = {
                ConstructFormulaId: $("#hdnContractFormulaId").val(),
                ContractId: $("#hdnSelectedContractId").val(),
                SurveyId: $("#hdnSurveyId").val(),
                MarketList: $("#hdnSelectedMarketList").val(),
                Quarter: $("#hdnSelectedQuarter").val(),
                Year: $("#hdnSelectedYear").val(),

                QuestionId: $("#hdnSelectedQuestionId").val(),
                QuestionColumn: $("#hdnSelectedQuestionSettingId").val(),
                QuestionColumnValue: $("#hdnSelectedQuestionColumnValueId").val(),
                QuestionTypeId: 0,
                Formula: $("#TxtFormula").val(),
                FormulaBuild: $("#TxtFormulaActual").val(),
                IsNcpSurvey: $("#hdnIsNcpSurvey").val()
            };

            $.ajax({
                type: 'POST',
                url: AjaxCallUrl.SaveConstructFormulaUrl,
                data: postData,
                dataType: 'json',
                // traditional: true,
                beforeSend: function () {
                },
                success: function (data) {
                    var DataUpdateWindow = $("#DataUpdateMessage").data("kendoWindow");
                    if (data.IsSuccess) {
                        kendo.ui.progress($("#ConstructFormula"), false);
                        if (DataUpdateWindow) {
                            DataUpdateWindow.title("Update Information");
                            DataUpdateWindow.content("<div class='msgboxSuccess'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i> Data Update Successfully</div>");
                            DataUpdateWindow.open();

                            window.location.href = AjaxCallUrl.RedirectToThisPageUrl;
                        }
                    }
                    else {
                        kendo.ui.progress($("#ConstructFormula"), false);
                        if (DataUpdateWindow) {
                            DataUpdateWindow.title("Error Information");
                            DataUpdateWindow.content("<div class='msgboxError'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i>" + data.ModelError + " </div>");
                            DataUpdateWindow.open();
                        }
                    }
                    //alert('Success');
                },
                error: function (request, error) {
                    kendo.ui.progress($("#ConstructFormula"), false);
                    if (DataUpdateWindow) {
                        DataUpdateWindow.title("Error Information");
                        DataUpdateWindow.content("<div class='msgboxError'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i>" + data.ModelError + " </div>");
                        DataUpdateWindow.open();
                    }
                    //alert('failure');
                }
            });
        }
    },
}

function SendSurveyIdAsParameter() {
    return { SurveyId: $("#hdnSurveyId").val() };
}

function SendSurveyIdQuarterAsParameter() {
    //alert('from dropdown call survey ' + $("#hdnSurveyId").val() + '     quarter    ' + $("#hdnSelectedQuarter").val());
    return { SurveyId: $("#hdnSurveyId").val(), QuarterName: $("#hdnSelectedQuarter").val() };
}

function SendSurveyIdQuestionIdAsParameter() {
    var dropdownlist = $("#QuestionDDL").data("kendoDropDownList");
    // alert(dropdownlist);
    return { SurveyId: $("#hdnSurveyId").val(), QuestionId: dropdownlist.value() };
    // return { SurveyId: $("#hdnSurveyId").val(), QuestionId: $("#hdnSelectedQuestionId").val() };
}

function SendSurveyIdQuestionSettingIdAsParameter() {
    var dropdownlist = $("#QuestionDDL").data("kendoDropDownList");
    var dropdownlistColumn = $("#QuestionColumnDDL").data("kendoDropDownList");
    var SplitedQuestionColumnValue = dropdownlistColumn.value().split('_');
    var CheckEditDocument = $('#hdnIsEditDocument').val();
    if (CheckEditDocument == 'Yes') {
        setTimeout(GetSelectedQuestionColumnIdValue(), 3500);
        SelectedQuestionColumnId = $("#hdnSelectedQuestionSettingId").val();

        return {
            SurveyId: $("#hdnSurveyId").val(),
            QuestionId: dropdownlist.value(),
            QuestionSettingId: SelectedQuestionColumnId
        };

    }
    else {
        return {
            SurveyId: $("#hdnSurveyId").val(),
            QuestionId: dropdownlist.value(),
            QuestionSettingId: SplitedQuestionColumnValue[0]
        };

    }
}
function GetSelectedQuestionColumnIdValue() {
    ConstructFormula.onDataBoundQuestionColumn();
    var QuestionColumn = $("#QuestionColumnDDL").data("kendoDropDownList").value();
    var QuestionColumnText = $("#QuestionColumnDDL").data("kendoDropDownList").text();
    //Dropdownlist.select(0);            
    var SplitedData = QuestionColumn.split('_');
    $("#hdnSelectedQuestionSettingId").val(SplitedData[0]);
    $("#hdnSelectedQuestionColumnIndex").val(SplitedData[1]);
    $("#hdnSelectedQuestionColumnText").val(QuestionColumnText);

    //return QuestionColumn;
}
var CustomControlMarket = function (MainDiv, ClassName) {
    //alert('called market populate section');
    if ($(MainDiv).children("." + ClassName).length == 0) {
        $(MainDiv).append("<div  class='" + ClassName + "'><div id='MarketList'></div></div>");
        var ChildDiv = $(MainDiv).children("." + ClassName);
        // console.log(parseInt($(MainDiv).height()));
        $(ChildDiv).css({
            "top": parseInt($(MainDiv).children('.CustomControl-Div').height()) + 5,
            "width": parseInt(($(MainDiv).children('.CustomControl-Div').width()))
        });
        // RenderMarketTreeViewControl(MainDiv);
        FetchMarket();
    }
    else {
        var ChildDiv = $(MainDiv).children("." + ClassName);
        if ($(ChildDiv).is(':visible')) {
            $(ChildDiv).hide();
            var Validator = $("#DivMarket").kendoValidator({
                messages: {
                    CustomControlMarket: function (input) {
                        return "Atleast one Market selection is required";
                    }
                },
                rules: {
                    CustomControlMarket: function (input) {
                        if (input.is("[data-role=constructformulamarket]")) {
                            var CheckedCheckBox = $("#MarketList").children().find("input[name='ChkMarketSelect']:checked");
                            if (CheckedCheckBox.length == 0) {
                                return false;
                            }
                            else {
                                return true;
                            }
                            return false;
                        }
                        return true;
                    }
                }
            }).data("kendoValidator");
            Validator.validateInput($("#DivMarket"));
        }
        else {
            $(ChildDiv).show();
            FetchMarket();
        }
    }
};

var FetchMarket = function () {
    //alert('CustomControlMarket -> ProductUrl ' + AjaxCallUrl.MarketUrl);
    kendo.ui.progress($("#MarketList"), true);
    var ProductCategory = $('.CustomControl-Div').find("[id ^='HdnCategorySelect_']");
    var ArrayProductCategory = [];
    $(ProductCategory).each(function () {
        ArrayProductCategory.push(parseInt($(this).val()));
    });

    var SelectedMarket = $("#MarketList").children().find("input[name='ChkMarketSelect']:checked");
    var MarketList = [];
    $(SelectedMarket).each(function () {
        MarketList.push($(this).siblings().eq(0).val());
    });

    var ProductCategoryHistory = [];
    MarketListOld = $("#MarketIdHistory").val().split(",");
    //var ProductCategoryData = new FormData();
    //ProductCategoryData.append('Values', ArrayProductCategory);
    //alert('MarketListOld -> ' + MarketListOld);
    var Flag = 0;

    if ($("#MarketIdHistory").val() != "") {
        Flag = 1;
    }

    var postData = { MarketHistory: MarketListOld, Flag: Flag }; //done
    // console.log(JSON.stringify(ArrayProductCategory));

    $.ajax({ //Process the form using $.ajax()
        type: 'POST', //Method type
        url: AjaxCallUrl.MarketUrl,
        data: postData, //Forms name
        dataType: 'json',
        // traditional: true,
        beforeSend: function () {
        },
        success: function (data) {
            if (data.IsAppendRequired) {
                $("#MarketList").append(data.MarketCustomControl);

                //   console.log
                // (data.RemoveList);
                if (data.RemoveList != null) {
                    for (var i = 0; i < data.RemoveList.length; i++) {
                        var Item = data.RemoveList[i];
                        $("#MarketList").find("#divMarketContainer").remove();
                    }
                }

                AssignControlEvent();
                kendo.ui.progress($("#MarketList"), false);
                $("#MarketIdHistory").val(MarketList.join());
            }
            else {
                if (data.RemoveList != null) {
                    for (var i = 0; i < data.RemoveList.length; i++) {
                        var Item = data.RemoveList[i];
                        $("#MarketList").find("#divMarketContainer").remove();
                    }
                }
                kendo.ui.progress($("#MarketList"), false);
            }
        },
        error: function (request, error) {
            kendo.ui.progress($("#MarketList"), false);
        }
    });
}


var AssignControlEvent = function () {

    if ($("#MarketIdHistory").length) {

        // console.log($("#HdnSelectedMarketControl").val());
        if ($("#MarketIdHistory").val() !== undefined && $("#MarketIdHistory").val() != "") {
            var MarketLis = $("#MarketIdHistory").val().split(",");
            //   console.log(MarketLis.length);
            $("input[name='ChkStateSelect']")
              .filter(function (index) {
                  // return $("strong", this).length === 1;
                  var Market = $(this).siblings().eq(0).val();

                  return MarketLis.indexOf(Market) > -1;
              }).prop('checked', true);


            // console.log(MarketLis);
        }
        else {
            $("input[name='ChkStateSelect']").prop('checked', true);
        }
    }
    //console.log($("#SelectedMarket").val());
    // $("input[name='ChkStateSelect']").prop('checked', true);



    $("#StateChkSelectAll").click(function () {
        $("input[name='ChkStateSelect']").prop('checked', true);
    });
    $("#StateChkClearAll").click(function () {
        $("input[name='ChkStateSelect']").prop('checked', false);
    });

    //$('#StateChkSelectAll').change(function () {
    //    if ($(this).is(":checked")) {
    //        $("input[name='ChkStateSelect']").prop('checked', true);
    //    }
    //});

    //$('#StateChkClearAll').change(function () {
    //    if ($(this).is(":checked")) {
    //        $("input[name='ChkStateSelect']").prop('checked', false);
    //    }
    //});

    $("#TxtStateSearch").keyup(function () {

        var Searchtext = $(this).val().toUpperCase();

        //$("#ShowProductTable td span").filter(function (index) {
        //    //return $("strong", this).length === 1;
        //    if ($(this).contains(Searchtext)) {
        //        console.log('gg');
        //        return true;
        //    }
        //});

        $(".ShowZoneStateTable td span:containsIgnoreCase(" + Searchtext + ")").parent().parent().show();

        $(".ShowZoneStateTable td span:not(:containsIgnoreCase(" + Searchtext + "))").parent().parent().hide();
        // var SerachSpan = $("#ShowProductTable td span:contains(" + Searchtext + ")").parent().parent().show();
        //console.log(SerachSpan.length);
    });




    //$("#TxtProductSearch").change(function () {
    //    // Check input( $( this ).val() ) for validity here
    //    console.log($(this).val());

    //});

}

$.expr[':'].containsIgnoreCase = function (n, i, m) {
    return jQuery(n).text().toUpperCase().indexOf(m[3].toUpperCase()) >= 0;
};

function insertAtCaret(areaId, text) {
    var txtarea = document.getElementById(areaId);
    if (!txtarea) { return; }

    var scrollPos = txtarea.scrollTop;
    var strPos = 0;
    var br = ((txtarea.selectionStart || txtarea.selectionStart == '0') ?
        "ff" : (document.selection ? "ie" : false));
    if (br == "ie") {
        txtarea.focus();
        var range = document.selection.createRange();
        range.moveStart('character', -txtarea.value.length);
        strPos = range.text.length;
    } else if (br == "ff") {
        strPos = txtarea.selectionStart;
    }
    //alert('strPos -> ' + strPos);
    //var position = txtarea.value.indexOf("]", txtarea.value.indexOf("[") + 1);
    //alert('position -> ' + position);
    var front = (txtarea.value).substring(0, strPos);
    var back = (txtarea.value).substring(strPos, txtarea.value.length);
    txtarea.value = front + text + back;
    strPos = strPos + text.length;
    if (strPos != txtarea.value + length) {
        strPos = txtarea.value + length;
    }
    if (br == "ie") {
        txtarea.focus();
        var ieRange = document.selection.createRange();
        ieRange.moveStart('character', -txtarea.value.length);
        ieRange.moveStart('character', strPos);
        ieRange.moveEnd('character', 0);
        ieRange.select();
    } else if (br == "ff") {
        txtarea.selectionStart = strPos;
        txtarea.selectionEnd = strPos;
        txtarea.focus();
    }

    txtarea.scrollTop = scrollPos;
}

function SetCaretAtEnd(elem) {
    var elemLen = elem.value.length;
    // For IE Only
    if (document.selection) {
        // Set focus
        elem.focus();
        // Use IE Ranges
        var oSel = document.selection.createRange();
        // Reset position to 0 & then set at end
        oSel.moveStart('character', -elemLen);
        oSel.moveStart('character', elemLen);
        oSel.moveEnd('character', 0);
        oSel.select();
    }
    else if (elem.selectionStart || elem.selectionStart == '0') {
        // Firefox/Chrome
        elem.selectionStart = elemLen;
        elem.selectionEnd = elemLen;
        elem.focus();
    } // if
} // SetCaretAtEnd()


// ******************************  market drop down related part start from here

var CustomControlZoneStateList = function (MainDiv, ClassName) {

    if ($(MainDiv).children("." + ClassName).length == 0) {
        $(MainDiv).append("<div  class='" + ClassName + "'><div id ='ZoneStateList'></div></div>");
        var ChildDiv = $(MainDiv).children("." + ClassName);
        // console.log(parseInt($(MainDiv).height()));
        $(ChildDiv).css({
            "top": parseInt($(MainDiv).children('.CustomControl-Div').height()) + 5,
            "width": parseInt(($(MainDiv).children('.CustomControl-Div').width()) + 285)
        });

        // RenderProductCategoryTreeViewControl(MainDiv);
        // FetchProduct();
        FetchZoneState();
    }
    else {

        var ChildDiv = $(MainDiv).children("." + ClassName);
        if ($(ChildDiv).is(':visible')) {
            $(ChildDiv).hide();

            //check about any of the state and zone selected or not
            //var CheckedCheckBox = $("input[name='ChkStateSelect']").prop('checked');
            //if (CheckedCheckBox.length == 0) {
            //    $("#ValidationMsgZoneState").text("Atleast one market selection is required");
            //}


            var Validator = $("#DivZoneState").kendoValidator({
                messages: {
                    ZoneSate: function (input) {
                        return "Atleast one market selection is required";
                    }
                },
                rules: {
                    ZoneSate: function (input) {
                        if (input.is("[data-role=ZoneSate]")) {
                            var CheckedCheckBox = $("#ZoneStateList").children().find("input[name='ChkStateSelect']:checked");
                            if (CheckedCheckBox.length == 0) {
                                return false;
                            }
                            else {
                                return true;
                            }
                            return false
                        }
                        return true;
                    }

                }

            }).data("kendoValidator");

            var v = Validator.validateInput($("#DivZoneState"));
            // console.log(v);
            if (v) {

                if ($("#HdnZoneSetting").length) {
                    if ($("#HdnZoneSetting").val() == "1") {
                        var DdlBuilder = $("#DdlBuilder").data("kendoDropDownList");
                        DdlBuilder.dataSource.read();
                    }
                }
            }
        }
        else {
            $(ChildDiv).show();
            //  FetchZoneState();
            // FetchZoneState();
        }
    }
};


var FetchZoneState = function () {



    kendo.ui.progress($("#ZoneStateList"), true);
    //var ProductCategory = $('.CustomControl-Div').find("[id ^='HdnCategorySelect_']");
    //var ArrayProductCategory = [];
    //$(ProductCategory).each(function () {
    //    ArrayProductCategory.push(parseInt($(this).val()));
    //});

    //var ProductCategoryData = new FormData();
    //ProductCategoryData.append('Values', ArrayProductCategory);

    var postData = {}; //done

    // console.log(JSON.stringify(ArrayProductCategory));


    if ($("#DivConfigureSurveyInvite").length) {
        postData = { Flag: "SurveyMarket", Id: $("#HdnSurveyId").val() };
    }

    if ($("#DivContractResource").length) {
        postData = { Flag: "ResourceMarket", Id: $("#HdnResourceId").val() };
    }
    $.ajax({ //Process the form using $.ajax()
        type: 'POST', //Method type
        url: ZoneControlUrl,
        data: postData, //Forms name
        dataType: 'json',
        // traditional: true,
        beforeSend: function () {
        },
        success: function (data) {
            $("#ZoneStateList").html(data.ProductCustomControl);
            //   AssignControlEvent();

            AssignControlZoneStateEvent();
            kendo.ui.progress($("#ZoneStateList"), false);
        },
        error: function (request, error) {
            kendo.ui.progress($("#ZoneStateList"), false);
        }
    });


}


var AssignControlZoneStateEvent = function () {
    if (MarketListForEdit.length > 0) {
        $("#HdnSelectedMarketControl").val(MarketListForEdit);
    }
    if ($("#HdnSelectedMarketControl").length) {
        var SelectedMarketLis = $("#HdnSelectedMarketControl").val();
        // console.log($("#HdnSelectedMarketControl").val());
        if ($("#HdnSelectedMarketControl").val() !== undefined && $("#HdnSelectedMarketControl").val() != "") {
            var MarketLis = $("#HdnSelectedMarketControl").val().split(",");
            //   console.log(MarketLis.length);
            $("input[name='ChkStateSelect']")
              .filter(function (index) {
                  // return $("strong", this).length === 1;
                  var Market = $(this).siblings().eq(0).val();

                  return MarketLis.indexOf(Market) > -1;
              }).prop('checked', true);


            // console.log(MarketLis);
        }
        else {
            $("input[name='ChkStateSelect']").prop('checked', true);
        }
    }
    //console.log($("#SelectedMarket").val());
    // $("input[name='ChkStateSelect']").prop('checked', true);



    $("#StateChkSelectAll").click(function () {
        $("input[name='ChkStateSelect']").prop('checked', true);
    });
    $("#StateChkClearAll").click(function () {
        $("input[name='ChkStateSelect']").prop('checked', false);
    });

    //$('#StateChkSelectAll').change(function () {
    //    if ($(this).is(":checked")) {
    //        $("input[name='ChkStateSelect']").prop('checked', true);
    //    }
    //});

    //$('#StateChkClearAll').change(function () {
    //    if ($(this).is(":checked")) {
    //        $("input[name='ChkStateSelect']").prop('checked', false);
    //    }
    //});

    $("#TxtStateSearch").keyup(function () {

        var Searchtext = $(this).val().toUpperCase();

        //$("#ShowProductTable td span").filter(function (index) {
        //    //return $("strong", this).length === 1;
        //    if ($(this).contains(Searchtext)) {
        //        console.log('gg');
        //        return true;
        //    }
        //});

        $(".ShowZoneStateTable td span:containsIgnoreCase(" + Searchtext + ")").parent().parent().show();

        $(".ShowZoneStateTable td span:not(:containsIgnoreCase(" + Searchtext + "))").parent().parent().hide();
        // var SerachSpan = $("#ShowProductTable td span:contains(" + Searchtext + ")").parent().parent().show();
        //console.log(SerachSpan.length);
    });




    //$("#TxtProductSearch").change(function () {
    //    // Check input( $( this ).val() ) for validity here
    //    console.log($(this).val());

    //});

}


$.expr[':'].containsIgnoreCase = function (n, i, m) {
    return jQuery(n).text().toUpperCase().indexOf(m[3].toUpperCase()) >= 0;
};

