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

function RedirecttoContract(contractid)
{
    var url = AjaxCallUrl.RedirectContractUrl.replace("****ContractId****", contractid);
    window.location.href = url;
}
function SendContractIdAsParameterOrder() {

    return { Type: type,Flag:flag, PageValue: $("#HdnResourcePageValue").val() };
   
}
function popup(contractid) {
   
    type = contractid;
  
    var wnd = $("#BuilderDetails").data("kendoWindow");
    //var wnd = $("#ContractResourceCategory").data("kendoWindow");
    wnd.refresh({
        url: AjaxCallUrl.BuilderDetailsUrl, // returns JSON, { firstName: "Alyx", lastName: "Vance" }
        // data: { ResourceId: dataValue.ResourceId },
        data: { ContractId: contractid }
       
    });
    wnd.open().center();

}
function OnOpenContactResource(e) {
    var wnd = $("#BuilderDetails").data("kendoWindow");
    kendo.ui.progress(wnd.element, true);
}
function OnRefreshContractResource(e) {
    var wnd = $("#BuilderDetails").data("kendoWindow");
    kendo.ui.progress(wnd.element, false);
    // AssignEventContractResource();
}

$(document).ready(function () {
    var PageName = $("#hdnPageName").val();

    if (PageName == 'ActiveContracts') {
        $('thead').first().css('display', 'inline-block');
        $('thead').first().css('width', '1140');
        $('tbody').first().css('display', 'inline-block');
        $('tbody').first().css('height', '500px');
        $('tbody').first().css('width', '1140');
    }

    if (PageName == 'PendingContracts') {
        //$('thead').first().css('display', 'inline-block'); <!-- Done by Rita on 11thOct,2017-->
        //$('thead').first().css('width', '1100'); <!-- Done by Rita on 11thOct,2017-->
        //$('tbody').first().css('display', 'inline-block'); <!-- Done by Rita on 11thOct,2017-->
        $('tbody').first().css('height', '500px');
        //$('tbody').first().css('width', '1100'); <!-- Done by Rita on 11thOct,2017-->
    }
});
