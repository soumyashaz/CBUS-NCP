$(document).ready(function () {
    var KendoListView = $("#listViewNonResponderList").data("kendoListView");
    $('thead').first().css('display', 'inline-block');
    $('thead').first().css('width', '1140');
    $('tbody').first().css('display', 'inline-block');
    $('tbody').first().css('height', '500px');
    $('tbody').first().css('width', '1140');

    //Change CSS to show the pager at the bottom
    $("#listViewNonResponderList_pager").css('order', '2');

    //Set current Quarter Id as the Download link parameter by default
    var currQtrId = $("#hdnSelectedQuarterId").val();
    var downloadUrl = AjaxCallUrl.DownloadReportUrl;
    downloadUrl = downloadUrl.replace('quarterId', currQtrId)
    $("#lnkDownload").attr('href', downloadUrl);
    //----------------------------------------------------------
});

//Select the current quarter by default in the dropdown
function onQuarterDataBound(e) {
    var currQtrId = $("#hdnSelectedQuarterId").val();
    this.value(currQtrId);
}

//Re-populate listview and set Download link parameter on change of quarter selection
function onSelectQuarter(e) {
    var dataItem = this.dataItem(e.item.index());
    var QuarterId = dataItem.QuarterId;

    $('#hdnSelectedQuarterId').val(QuarterId);

    var KendoListView = $("#listViewNonResponderList").data("kendoListView");
    KendoListView.dataSource.read();

    var downloadUrl = AjaxCallUrl.DownloadReportUrl;    
    downloadUrl = downloadUrl.replace('quarterId', QuarterId);
    $("#lnkDownload").attr('href', downloadUrl);
}

//Send QuarterId as parameter for listview data-bind
function SendQuarterIdAsParameter() {
    var SelectedQuarterId = $("#hdnSelectedQuarterId").val();

    return { QuarterId: SelectedQuarterId };
}