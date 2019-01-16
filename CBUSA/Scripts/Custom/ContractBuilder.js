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
$("#ascpcon").click(function () {
    $('#descpicon').show();
    $('#ascpicon').hide();
    var UserDetails = asccon;
    type = 'ascpcon';
    var KendoListView = $("#listViewContractPendingAsc").data("kendoListView");
    KendoListView.dataSource.read();
});
$("#descpcon").click(function () {
    $('#descpicon').hide();
    $('#ascpicon').show();
    var UserDetails = asccon;
    type = 'descpcon';
    var KendoListView = $("#listViewContractPendingAsc").data("kendoListView");
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
$("#ascpncp").click(function () {
    $('#descpincp').show();
    $('#ascpincp').hide();

    type = 'ascpncp';
    var KendoListView = $("#listViewContractPendingAsc").data("kendoListView");
    KendoListView.dataSource.read();
});

$("#descpncp").click(function () {
    $('#descpincp').hide();
    $('#ascpincp').show();

    type = 'descpncp';
    var KendoListView = $("#listViewContractPendingAsc").data("kendoListView");
    KendoListView.dataSource.read();
});
$("#ascpct").click(function () {
    $('#descpict').show();
    $('#ascpict').hide();

    type = 'ascpct';
    var KendoListView = $("#listViewContractPendingAsc").data("kendoListView");
    KendoListView.dataSource.read();
});
$("#descpct").click(function () {
    $('#descpict').hide();
    $('#ascpict').show();

    type = 'descpct';
    var KendoListView = $("#listViewContractPendingAsc").data("kendoListView");
    KendoListView.dataSource.read();
});
$("#ascpst").click(function () {
    $('#descpist').show();
    $('#ascpist').hide();

    type = 'ascpst';
    var KendoListView = $("#listViewContractPendingAsc").data("kendoListView");
    KendoListView.dataSource.read();
});
$("#descpst").click(function () {
    $('#descpist').hide();
    $('#ascpist').show();

    type = 'descpst';
    var KendoListView = $("#listViewContractPendingAsc").data("kendoListView");
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
$("#asceed").click(function () {
    
    $('#descieed').show();
    $('#ascieed').hide();

    type = 'asceed';
    var KendoListView = $("#listViewContractPendingAsc").data("kendoListView");
    KendoListView.dataSource.read();
});
$("#desceed").click(function () {
    $('#descieed').hide();
    $('#ascieed').show();

    type = 'desceed';
    var KendoListView = $("#listViewContractPendingAsc").data("kendoListView");
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
    //$('#descincp').show();
    //$('#ascincp').hide();

    //type = 'ascncp';
    //flag = null;
    //var KendoListView = $("#listViewContractActiveAsc").data("kendoListView");
    //KendoListView.dataSource.read();
});
$("#descser").click(function () {
    //$('#descincp').hide();
    //$('#ascincp').show();

    //type = 'descncp';
    //flag = null;
    //var KendoListView = $("#listViewContractActiveAsc").data("kendoListView");
    //KendoListView.dataSource.read();
});
$("#ascbj").click(function () {
   
    //$('#descibj').show();
    //$('#ascibj').hide();

    //type = 'ascbj';
    //flag = null;
    //var KendoListView = $("#listViewContractActiveAsc").data("kendoListView");
    //KendoListView.dataSource.read();
});
$("#descbj").click(function () {
    //$('#descibj').hide();
    //$('#ascibj').show();

    //type = 'descbj';
    //flag = null;
    //var KendoListView = $("#listViewContractActiveAsc").data("kendoListView");
    //KendoListView.dataSource.read();
});
$("#ascbj1").click(function () {

    $('#descibj1').show();
    $('#ascibj1').hide();

    type = 'ascbj';
    flag = null;
    var KendoListView = $("#listViewContractPendingAsc").data("kendoListView");
    KendoListView.dataSource.read();
});
$("#descbj1").click(function () {
    $('#descibj1').hide();
    $('#ascibj1').show();

    type = 'descbj';
    flag = null;
    var KendoListView = $("#listViewContractPendingAsc").data("kendoListView");
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
$("#ascppc").click(function () {

    $('#descippc').show();
    $('#ascippc').hide();

    type = 'ascpc';
    var KendoListView = $("#listViewContractPendingAsc").data("kendoListView");
    KendoListView.dataSource.read();
});
$("#descppc").click(function () {

    $('#descippc').hide();
    $('#ascippc').show();

    type = 'descpc';
    var KendoListView = $("#listViewContractPendingAsc").data("kendoListView");
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
   

    return { Type: type, PageValue: $("#HdnResourcePageValue").val() };

}
function SendContractIdAsParameterPending() {
  
    return { Type: type, PageValue: $("#HdnResourcePageValue3").val() };

}

function onSelectContractStatus(e) {

    var typead = this.dataItem(e.item.index());
    flag = typead.ContractId;
    type = null;

    var KendoListView = $("#listViewContractActiveAsc").data("kendoListView");
    KendoListView.dataSource.read();
}