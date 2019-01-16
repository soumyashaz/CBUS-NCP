var type;
var flag;

function onSelectContractStatus(e) {
   
   var typead = this.dataItem(e.item.index());
   flag = typead.ContractId;
   type = null;
    
    var KendoListView = $("#listViewContractActiveAsc").data("kendoListView");
    KendoListView.dataSource.read();
}