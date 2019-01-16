$(document).ready(function () {
    $('thead').first().css('display', 'inline-block');
    $('thead').first().css('width', '1140');
    $('tbody').first().css('display', 'inline-block');
    $('tbody').first().css('height', '300px');
    $('tbody').first().css('width', '1140');

    //Change CSS to show the pager at the bottom
    $("#listViewBuilderContractList_pager").css('order', '2');
});

//Select the current builder by default in the dropdown
function onBuilderDataBound(e) {
    var currBuilderId = $("#hdnSelectedBuilderId").val();
    this.value(currBuilderId);
}

//Re-populate listview on change of Builder selection
function onSelectBuilder(e) {
    var dataItem = this.dataItem(e.item.index());
    var BuilderId = dataItem.BuilderId;

    $('#hdnSelectedBuilderId').val(BuilderId);

    var KendoListView = $("#listViewBuilderContractList").data("kendoListView");
    KendoListView.dataSource.read();
}

//Send BuilderId as parameter for listview data-bind
function SendBuilderIdAsParameter() {
    var SelectedBuilderId = $("#hdnSelectedBuilderId").val();

    return { BuilderId: SelectedBuilderId };
}