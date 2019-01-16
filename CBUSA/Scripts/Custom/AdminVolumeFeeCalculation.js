var type;
var flag;
$(document).ready(function () {
    ConstructFormula.AssignControl();

    $("#DivMarketDropdown").on("click", function () {
        CustomControlMarket($(this).parent().parent(), "CustomControlPoupMarket");
    });

    $('thead').first().css('display', 'inline-block');
    $('thead').first().css('width', '1140');
    $('tbody').first().css('display', 'inline-block');
    $('tbody').first().css('height', '300px');
    $('tbody').first().css('width', '1140');
});

$("#asccon").click(function () {
    $('#descicon').show();
    $('#ascicon').hide();
    var UserDetails = asccon;
    type = 'asccon';
    var KendoListView = $("#listViewConstructFormulaList").data("kendoListView");
    KendoListView.dataSource.read();
});
$("#desccon").click(function () {
    $('#descicon').hide();
    $('#ascicon').show();
    var UserDetails = asccon;
    type = 'desccon';
    var KendoListView = $("#listViewConstructFormulaList").data("kendoListView");
    KendoListView.dataSource.read();
});


$("#ascqtr").click(function () {
    $('#desciqtr').show();
    $('#asciqtr').hide();

    type = 'ascqtr';
    var KendoListView = $("#listViewConstructFormulaList").data("kendoListView");
    KendoListView.dataSource.read();
});

$("#descqtr").click(function () {
    $('#desciqtr').hide();
    $('#asciqtr').show();

    type = 'descqtr';
    var KendoListView = $("#listViewConstructFormulaList").data("kendoListView");
    KendoListView.dataSource.read();
});

$("#ascyr").click(function () {
    $('#desciyr').show();
    $('#asciyr').hide();

    type = 'ascyr';
    flag = null;
    var KendoListView = $("#listViewConstructFormulaList").data("kendoListView");
    KendoListView.dataSource.read();
});
$("#descyr").click(function () {
    $('#desciyr').hide();
    $('#asciyr').show();

    type = 'descyr';
    flag = null;
    var KendoListView = $("#listViewConstructFormulaList").data("kendoListView");
    KendoListView.dataSource.read();
});
$("#ascmkt").click(function () {

    $('#descimkt').show();
    $('#ascimkt').hide();

    type = 'ascmkt';
    flag = null;
    var KendoListView = $("#listViewConstructFormulaList").data("kendoListView");
    KendoListView.dataSource.read();
});
$("#descmkt").click(function () {
    $('#descimkt').hide();
    $('#ascimkt').show();

    type = 'descmkt';
    flag = null;
    var KendoListView = $("#listViewConstructFormulaList").data("kendoListView");
    KendoListView.dataSource.read();
});
$("#BtnCalculateVolumeFee").click(function () {
    ProcessVolumeFeeRebate();
});
function RedirecttoContract(contractid) {
    var url = AjaxCallUrl.RedirectContractUrl.replace("****ContractId****", contractid);
    window.location.href = url;
}
function SendContractIdAndOtherFiltersAsParameterOrder() {
    var SelectedContractId = $("#hdnSelectedContractId").val();
    var SelectedQuarter = $("#hdnSelectedQuarter").val();
    var SelectedYear = $("#hdnSelectedYear").val();
    var SelectedMarketId = $("#hdnSelectedMarketList").val();

    return { Type: type, ContractId: SelectedContractId, QuarterName: SelectedQuarter, Year: SelectedYear, MarketId: SelectedMarketId, Flag: flag };
}


var ConstructFormula = {
    AssignControl: function () {
        $("#a_openaddinvitespopup").on("click", ConstructFormula.PopUpMarkets);
        $("#BtnEdit").on("click", ConstructFormula.EditFormula);
    },

    EditFormula: function () {

        var Rad = $("input[name='RadSurvey']:checked");
        var SurveyId = Rad.siblings().filter("input[name='HdnSurvey']").val();
        //  console.log(SurveyId)
        if (parseInt(SurveyId) > 0) {
            var url = AjaxCallUrl.RedirectSurveyEditUrl.replace("****SurveyId****", SurveyId);
            window.location.href = url;
        }

    },
    onSelectContract: function (e) {
        // var dataItem = this.dataItem(e.item.index());
        //  $("#hdnSelectedContractId").val(dataItem.ContractId);
        var DropDownList = $("#ContractId").data("kendoDropDownList");
        $("#hdnSelectedContractId").val(DropDownList.value());
        flag = "Contract";
        var KendoListView = $("#listViewConstructFormulaList").data("kendoListView");
        KendoListView.dataSource.read();
    },
    onDataBoundContract: function () {

        var ContractId = $("#ContractId").data("kendoDropDownList").value()
    },
    onSelectQuarter: function (e) {
        var dataItem = this.dataItem(e.item.index());
        var QuarterNameYear = dataItem.Value.split('-');
        //alert('Quarter ' + QuarterNameYear);
        if (QuarterNameYear != 0) {
            $("#hdnSelectedQuarter").val(QuarterNameYear[0]);
            $("#hdnSelectedYear").val(QuarterNameYear[1]);
        }
        else {
            $("#hdnSelectedQuarter").val('');
            $("#hdnSelectedYear").val('');
        }
        var dropdownlist = $("#QuestionDDL").data("kendoDropDownList");

        if (dropdownlist) {
            // re-render the items in drop-down list.
            dropdownlist.refresh();
            //alert('from quarter selection dropdown refresh   survey ' + $("#hdnSurveyId").val() + '     quarter    ' + $("#hdnSelectedQuarter").val());
        }
        flag = "Quarter";
        var KendoListView = $("#listViewConstructFormulaList").data("kendoListView");
        KendoListView.dataSource.read();
    },
    onSelectMarket: function (e) {
        var dataItem = this.dataItem(e.item.index());
        $("#hdnSelectedMarketList").val(dataItem.Value > 0 ? dataItem.Value : '');
        flag = "Market";
        var KendoListView = $("#listViewConstructFormulaList").data("kendoListView");
        KendoListView.dataSource.read();
    },

    SendDataBeforeLoadDropDown: function () {
        return { IsNcpSurvey: "true" };
    },

    PopUpMarkets: function () {
        var wnd = $("#WndSurveyInvitesPopup").data("kendoWindow");
        wnd.refresh({
            url: AjaxCallUrl.MarketPopupUrl,
        });
        wnd.open().center();
    },

    OnOpenSurveyInvitesPopup: function (e) {
        var wnd = $("#WndSurveyInvitesPopup").data("kendoWindow");
        kendo.ui.progress(wnd.element, true);
    },


    OnRefreshSurveyInvitesPopup: function (e) {
        var wnd = $("#WndSurveyInvitesPopup").data("kendoWindow");
        kendo.ui.progress(wnd.element, false);
        // AssignEventContractResource();
        ConstructFormula.AddEventSurveyInvitesPopup();

    },
    AddEventSurveyInvitesPopup: function () {
        $("#DivZoneStateDropdown").on("click", ConstructFormula.OpenMarketCustomControl);
        $("#BtnReturnMarketPopup").on("click", ConstructFormula.ReturnSelectedMarket);
        $("#BtnCancelarketPopup").on("click", ConstructFormula.CancelSelectedMarket);

        if ($("#DivConfigureMarketList").length) {
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
    },
    OpenMarketCustomControl: function () {
        CustomControlZoneStateList($(this).parent().parent(), "CustomControlPoupZoneState");
    },
    ReturnSelectedMarket: function () {
        //console.log('sdsad');
        var Isvalid = ConstructFormula.ValidateSelectedMarketReturn();
        //  console.log(Isvalid);
        if (Isvalid) {
            kendo.ui.progress($("#DivConfigureMarketList"), true);
            var SelectedMarket = $("#ZoneStateList").children().find("input[name='ChkStateSelect']:checked");
            var MarketList = [];
            $(SelectedMarket).each(function () {
                MarketList.push($(this).siblings().eq(0).val());
            });

            $('#hdnSelectedMarketList').val(MarketList);
            //alert('MarketList -> ' + MarketList);
            kendo.ui.progress($("#DivConfigureMarketList"), false);
            //var KendoListView = $("#hdnSelectedMarketList").data("kendoListView");
            //KendoListView.dataSource.read();
            //var FormData = {
            //    'ContractId': $("#hdnSelectedContractId").val(),
            //    'Quarter': $("#hdnSelectedQuarter").val(),
            //    'Year': $("#hdnSelectedYear").val(),
            //    'Market': MarketList
            //};
            //if (MarketList.length > 0) {
            //    $.ajax({ //Process the form using $.ajax()
            //        type: 'GET', //Method type
            //        url: AjaxCallUrl.GetFormulaDetailsUrl,
            //        data: FormData, //Forms name
            //        dataType: 'json',
            //        async: false,
            //        beforeSend: function () {
            //        },
            //        error: function (request, error) {

            //        },
            //        success: function (data) {
            //            alert('called fetch grid data');
            //        }
            //    });
            //}
            flag = "Market";
            var KendoListView = $("#listViewConstructFormulaList").data("kendoListView");
            KendoListView.dataSource.read();
        }
    },
    ValidateSelectedMarketReturn: function () {
        var Validator = $("#DivConfigureMarketList").kendoValidator({
            messages: {
                ZoneSate: function (input) {
                    return "Atleast one market selection is required";
                },
            },
            rules: {
                ZoneSate: function (input) {
                    if (input.is("[data-role=ZoneSate]")) {

                        // console.log($("#ZoneStateList").children().find("input[name='ChkStateSelect']").length);
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
                },
            }
        }).data("kendoValidator");

        return Validator.validateInput($("#DivZoneState"));
    },
    CancelSelectedMarket: function () {
        var DataUpdateWindow = $("#WndSurveyInvitesPopup").data("kendoWindow");
        if (DataUpdateWindow) {
            DataUpdateWindow.close();
        }
    },
    onSelectQuestion: function (e) {
        var dataItem = this.dataItem(e.item.index());
        //  console.log(dataItem.ContractId)
        //ConstructFormula.IsEnrollmentSurveyAvailable(dataItem.QuestionId);
        $("#hdnSelectedQuestionId").val(dataItem.QuestionId);
        $("#hdnSelectedQuestionText").val(dataItem.QuestionValue);
        var SurveyUrl = AjaxCallUrl.GetSurveyUrl;
        var FormData = {
            'ContractId': ContractId
            //Store name fields value
        };
        //$.ajax({ //Process the form using $.ajax()
        //    type: 'GET', //Method type
        //    url: AjaxCallUrl.GetSurveyUrl,
        //    data: FormData, //Forms name
        //    dataType: 'json',
        //    beforeSend: function () {
        //    },
        //    error: function (request, error) {
        //    },
        //    success: function (data) {
        //        if (data.IsSuccess) {
        //            $("#IsEnrolment").removeAttr("disabled");
        //        }
        //        else {
        //            $("#IsEnrolment").attr("disabled", true);
        //        }
        //    }
        //});
    },
    onDataBoundQuestion: function () {

        var QuestionId = $("#QuestionId").data("kendoDropDownList").value()
        //ConstructFormula.IsEnrollmentSurveyAvailable(ContractId);
    },
    onSelectQuestionColumn: function (e) {
        var dataItem = this.dataItem(e.item.index());
        //  console.log(dataItem.ContractId)
        //ConstructFormula.IsEnrollmentSurveyAvailable(dataItem.QuestionId);
        $("#hdnSelectedQuestionSettingId").val(dataItem.QuestionColumnId);
        $("#hdnSelectedQuestionColumnText").val(dataItem.QuestionColumnText);
        //alert('hdnSelectedQuestionSettingId -> ' + dataItem.QuestionColumnId);
    },
    onDataBoundQuestionColumn: function () {
        var QuestionId = $("#QuestionId").data("kendoDropDownList").value()
        //ConstructFormula.IsEnrollmentSurveyAvailable(ContractId);
    },
    onSelectQuestionColumnValue: function (e) {
        var dataItem = this.dataItem(e.item.index());
        $("#hdnSelectedQuestionColumnValueId").val(dataItem.QuestionColumnValueId);
        $("#hdnSelectedQuestionColumnValueText").val(dataItem.QuestionColumnValueText);
    },
    onDataBoundQuestionColumnValue: function () {
        //alert("check selected Question Value Databound");
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
    return { SurveyId: $("#hdnSurveyId").val(), QuestionId: $("#hdnSelectedQuestionId").val() };
}

function SendSurveyIdQuestionSettingIdAsParameter() {
    //alert('SendSurveyIdQuestionSettingIdAsParameter -> ' + $("#hdnSelectedQuestionSettingId").val());
    return { SurveyId: $("#hdnSurveyId").val(), QuestionId: $("#hdnSelectedQuestionId").val(), QuestionSettingId: $("#hdnSelectedQuestionSettingId").val() };
}

var CustomControlMarket = function (MainDiv, ClassName) {
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
    if ($('.CustomControl-Div').children('div[id ^= "divCategorySelected"]').length == 0) {
        // console.log('ffffffffkk');
        //return;
    }
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
    $('#ChkSelectAll').on("click", function () {
        // if ($(this).is(":checked")) {
        $("input[name='ChkMarketSelect']").prop('checked', true);
        //}
    });

    $('#ChkClearAll').on("click", function () {
        //   if ($(this).is(":checked")) {
        $("input[name='ChkMarketSelect']").prop('checked', false);
        // }
    });

    $("#TxtMarketSearch").keyup(function () {
        var Searchtext = $(this).val().toUpperCase();

        $(".ShowProductTable td span:containsIgnoreCase(" + Searchtext + ")").parent().parent().show();
        $(".ShowProductTable td span:not(:containsIgnoreCase(" + Searchtext + "))").parent().parent().hide();
    });
}

$.expr[':'].containsIgnoreCase = function (n, i, m) {
    return jQuery(n).text().toUpperCase().indexOf(m[3].toUpperCase()) >= 0;
};


function EditConstructFormula(ParamId) {
    var url = AjaxCallUrl.RedirectConstructFormulaUrl;
    //var win = window.open(url, '_blank');
    //win.focus();
    var UrlReplaced = url.replace("ConstructFormulaIdParam", ParamId);
    window.location.href = UrlReplaced;

}

function ProcessVolumeFeeRebate()
{
    var SelectedContractId = $("#hdnSelectedContractId").val();
    if (SelectedContractId == undefined || SelectedContractId == '')
    {
        SelectedContractId = '';
    }
    var postData = { ContractId: SelectedContractId }; //done
    // console.log(JSON.stringify(ArrayProductCategory));
    //alert('calc vol-free : ' + SelectedContractId);
    $.ajax({
        type: 'POST',
        url: AjaxCallUrl.ProcessVolumeFeeRebate,
        data: postData,
        dataType: 'json',
        beforeSend: function () {
        },
        success: function (data) {
            var DataUpdateWindow = $("#DataUpdateMessage").data("kendoWindow");
            if (DataUpdateWindow) {
                DataUpdateWindow.title("Update Information");
                DataUpdateWindow.content("<div class='msgboxSuccess'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i>" + data + " </div>");
                DataUpdateWindow.open();
            }            
        },
        error: function (request, error) {
            var DataUpdateWindow = $("#DataUpdateMessage").data("kendoWindow");
            if (DataUpdateWindow) {
                DataUpdateWindow.title("Error Information");
                DataUpdateWindow.content("<div class='msgboxError'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i>" + error + " </div>");
                DataUpdateWindow.open();
            }
        }
    });
}