var type;
var flag;
$(document).ready(function () {
    //ReOpenBuildereNCPSurveyResponse.AssignControl();   
});


$("#BtnReOpenBuilderNCPSurveyResponse").click(function () {
    SaveBuilderNCPSurveyResponseStatus();
});

function SendContractIdAndOtherFiltersAsParameterOrder() {
    var SelectedContractId = $("#hdnSelectedContractId").val();
    var SelectedQuarter = $("#hdnSelectedQuarter").val();
    var SelectedYear = $("#hdnSelectedYear").val();
    var SelectedMarketId = $("#hdnSelectedMarketList").val();

    return { Type: type, ContractId: SelectedContractId, QuarterName: SelectedQuarter, Year: SelectedYear, MarketId: SelectedMarketId, Flag: flag };
}

function SendDataBeforeLoadDropDown() {
    var dropdownlist = $("#ContractDDL").data("kendoDropDownList");
    // alert(dropdownlist);
    return { ContractId: dropdownlist.value() };
    // return { SurveyId: $("#hdnSurveyId").val(), QuestionId: $("#hdnSelectedQuestionId").val() };
}
var ReOpenBuildereNCPSurveyResponse = {
    onSelectContract: function (e) {
        // var dataItem = this.dataItem(e.item.index());
        //  $("#hdnSelectedContractId").val(dataItem.ContractId);
        var DropDownList = $("#ContractDDL").data("kendoDropDownList");
        $("#hdnSelectedContractId").val(DropDownList.value());
        //alert("selected contract : " + DropDownList.value());
        var BuilderDropdownlist = $("#BuilderDDL").data("kendoDropDownList");

        if (BuilderDropdownlist) {
            // re-render the items in drop-down list.
            BuilderDropdownlist.refresh();
            //alert('from quarter selection dropdown refresh   survey ' + $("#hdnSurveyId").val() + '     quarter    ' + $("#hdnSelectedQuarter").val());
        }
    },
    onDataBoundContract: function () {
        var ContractId = $("#ContractDDL").data("kendoDropDownList").value();
    },
    onSelectBuilder: function (e) {
        // var dataItem = this.dataItem(e.item.index());
        //  $("#hdnSelectedContractId").val(dataItem.ContractId);
        var DropDownList = $("#BuilderDDL").data("kendoDropDownList");
        $("#hdnSelectedBuilderId").val(DropDownList.value());        
    },
    onDataBoundBuilder: function () {
        var BuilderId = $("#BuilderDDL").data("kendoDropDownList").value()
    },
    SendDataBeforeLoadDropDown: function () {
        return { IsNcpSurvey: true };
    },
    SendDataBeforeLoadDropDownContract: function () {
        var DropDownList = $("#ContractDDL").data("kendoDropDownList");        
        return { ContractId: DropDownList.value() };
    },
    SendDataBeforeLoadDropDownEditStatus: function () {
        var DropDownList = $("#ContractDDL").data("kendoDropDownList");
        var BuilderDropDownList = $("#BuilderDDL").data("kendoDropDownList");
        return { BuilderId: BuilderDropDownList.value(), ContractId: DropDownList.value() };
    }
}


$.expr[':'].containsIgnoreCase = function (n, i, m) {
    return jQuery(n).text().toUpperCase().indexOf(m[3].toUpperCase()) >= 0;
};

function SaveBuilderNCPSurveyResponseStatus() {
    var SelectedContractId = $("#hdnSelectedContractId").val();
    var SelectedBuilderId = $("#hdnSelectedBuilderId").val();
    
    var postData = { BuilderId: SelectedBuilderId, ContractId: SelectedContractId }; //done
    // console.log(JSON.stringify(ArrayProductCategory));
    //alert('calc vol-free : ' + SelectedContractId);
    $.ajax({
        type: 'POST',
        url: AjaxCallUrl.SaveBuilderNCPSurveyStatus,
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