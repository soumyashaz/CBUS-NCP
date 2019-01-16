
var type;
var flag;
function SendContractIdAsParameterOrder() {

    return { Type: type, Flag: flag };

}
$("#asccon").click(function () {
    $('#descicon').show();
    $('#ascicon').hide();
    var UserDetails = asccon;
    type = 'asccon';
    var KendoListView = $("#listViewreporthistory").data("kendoListView");
    KendoListView.dataSource.read();
});
$("#desccon").click(function () {
    $('#descicon').hide();
    $('#ascicon').show();
    var UserDetails = asccon;
    type = 'desccon';
    var KendoListView = $("#listViewreporthistory").data("kendoListView");
    KendoListView.dataSource.read();
});


$("#ascncp").click(function () {
    $('#descincp').show();
    $('#ascincp').hide();

    type = 'ascncp';
    var KendoListView = $("#listViewreporthistory").data("kendoListView");
    KendoListView.dataSource.read();
});

$("#descncp").click(function () {
    $('#descincp').hide();
    $('#ascincp').show();

    type = 'descncp';
    var KendoListView = $("#listViewreporthistory").data("kendoListView");
    KendoListView.dataSource.read();
});
$("#ascbj").click(function () {
    $('#descibj').show();
    $('#ascibj').hide();

    type = 'ascbj';
    var KendoListView = $("#listViewreporthistory").data("kendoListView");
    KendoListView.dataSource.read();
});
$("#descbj").click(function () {
    $('#descibj').hide();
    $('#ascibj').show();

    type = 'descbj';
    var KendoListView = $("#listViewreporthistory").data("kendoListView");
    KendoListView.dataSource.read();
});
$("#ascpc").click(function () {
    $('#descipc').show();
    $('#ascipc').hide();

    type = 'ascpc';
    var KendoListView = $("#listViewreporthistory").data("kendoListView");
    KendoListView.dataSource.read();
});
$("#descpc").click(function () {
    $('#descipc').hide();
    $('#ascipc').show();

    type = 'descpc';
    var KendoListView = $("#listViewreporthistory").data("kendoListView");
    KendoListView.dataSource.read();
});
function onSelectContractStatus(e) {

    var typead = this.dataItem(e.item.index());
    flag = typead.ContractId;
    type = null;

    var KendoListView = $("#listViewreporthistory").data("kendoListView");
    KendoListView.dataSource.read();

    $('#spnContractName').text(typead.ContractName);
}
function onSelectYearStatus(e) {

    var typead = this.dataItem(e.item.index());
    flag = typead.ProjectId;
    type = "year";

    var KendoListView = $("#listViewreporthistory").data("kendoListView");
    KendoListView.dataSource.read();
}

function redirectdetail(contractid, quaterid) {
   
    var url = AjaxCallUrl.RedirectViewDetailUrl.replace("****ContractId****", contractid);
    url = url.replace("****QuaterId****", quaterid);
    var win = window.open(url, '_blank');
    win.focus();
    //window.location.href = url;

}