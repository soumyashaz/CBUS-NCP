$(document).ready(function () {
    DownloadVolumeFeeReport.AssignControl();
    $("#BtnDownload").on("click", function () {
        DownloadVolumeFeeReport.DownloadFormulaData();        
    });
});

var DownloadVolumeFeeReport = {
    AssignControl: function () {
        $("#BtnDownload").on("click", function () {
            DownloadVolumeFeeReport.DownloadFormulaData();
        });
    },
    onSelectSurvey: function (e) {
        var dataItem = this.dataItem(e.item.index());
        //var dropdownlist = $("#Contract").data("kendoDropDownList");
        //console.log(dropdownlist.text())
        //$("#hdnSelectedQuestionId").val(dataItem.QuestionId);
        //$("#hdnSelectedQuestionText").val(dataItem.QuestionValue);
        //$("#hdnSurveyId").val(dropdownlist.value());
        

        $("#hdnSurveyId").val(dataItem.SurveyId);
        //alert('selected survey -> ' + $("#hdnSurveyId").val());
    },
    onSelectContract: function (e) {
        var dataItem = this.dataItem(e.item.index());
        $("#hdnSelectedContractId").val(dataItem.ContractId);
    },
    onDataBoundContract: function () {
        
        var ContractId = $("#ContractId").data("kendoDropDownList").value()
    },
    onSelectQuarter: function (e) {
        var dataItem = this.dataItem(e.item.index());
        var QuarterNameYear = dataItem.Value.split('-');
        //alert('Quarter ' + QuarterNameYear);
        if (QuarterNameYear!=0)
        {
            $("#hdnSelectedQuarter").val(QuarterNameYear[0]);
            $("#hdnSelectedYear").val(QuarterNameYear[1]);
            var FormData = {
                'ContractId': $("#hdnSelectedContractId").val(),
                'Quarter': QuarterNameYear[0],
                'Year': QuarterNameYear[1]
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
        }
        else
        {
            $("#hdnSelectedQuarter").val('');
            $("#hdnSelectedYear").val('');
            $('#hdnSurveyId').val('');
        }

    },
    onSelectMarket: function (e) {
        var dataItem = this.dataItem(e.item.index());
        $("#hdnSelectedMarketList").val(dataItem.Value);
        //alert('selected market : ' + dataItem.Value);
    },
    DownloadFormulaData: function()
    {
        var QuarterName = $("#hdnSelectedQuarter").val();
        var Year = $("#hdnSelectedYear").val();
        //alert('Quarter ' + QuarterNameYear);
        if (QuarterName != undefined && QuarterName !='' && QuarterName != null) {            
            var FormData = {
                'ContractId': $("#hdnSelectedContractId").val(),
                'Quarter': QuarterName,
                'Year': Year
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
        }
        else {
            $("#hdnSelectedQuarter").val('');
            $("#hdnSelectedYear").val('');
            $('#hdnSurveyId').val('');
        }

        var SelectedSurveyId = $("#hdnSurveyId").val();
        var SelectedMarketId = $("#hdnSelectedMarketList").val();
        if (SelectedMarketId == undefined || SelectedMarketId == '' || SelectedMarketId == null || SelectedMarketId =='undefined')
        {
            SelectedMarketId = 0;            
        }

        if (SelectedSurveyId == undefined || SelectedSurveyId == '' || SelectedSurveyId == null)
        {            
            alert("Please Select Contract, Quarter First");
        }
        else
        {
            if (SelectedMarketId == 0)
            {
                alert("Please Select Market");
            }
            else
            {
                var RedirectToUrl = AjaxCallUrl.DownloadUrl;
                
                var UrlReplace = RedirectToUrl.replace("SurveyIdParam", SelectedSurveyId);
                UrlReplace = UrlReplace.replace("MarketIdParam", SelectedMarketId);

                var div = document.createElement('div');
                div.innerHTML = UrlReplace
                var decoded = div.firstChild.nodeValue;

                window.location.href = decoded;
            }            
        }
    },
}

function SendContractIdAndOtherFiltersAsParameterOrder() {
    var SelectedContractId = $("#hdnSelectedContractId").val();
    var SelectedQuarter = $("#hdnSelectedQuarter").val();
    var SelectedYear = $("#hdnSelectedYear").val();
    var SelectedMarketId = $("#hdnSelectedMarketList").val();

    return { Type: type, ContractId: SelectedContractId, QuarterName: SelectedQuarter, Year: SelectedYear, MarketId: SelectedMarketId, Flag: flag };
}
