$(document).ready(function () {
    kendo.ui.progress($(document.body), true);

    AdminContractList.LoadContractLogoPartialView();

});

var AdminContractList = {
    ShowHideContractDropdownList: function () {
        $(document.body).click(function () {
            $("#ulContractList").hide();
        });

        $("#lnkSelectAContract").click(function (e) {
            e.stopPropagation();
            $("#ulContractList").toggle();
        });
    },
        
    LoadContractLogoPartialView: function () {
        var hdnBuilderId = $("#hdnBuilderId").val();

        $.ajax({
            url: AjaxCallUrl.ContractLogoListUrl,
            type: "GET",
            data: { BuilderId: hdnBuilderId },
            contentType: "application/json; charset=utf-8",
            success: function (result) {
                $("#divContractCentral").html(result);

                kendo.ui.progress($(document.body), false);

                $("li.contract-logo").each(function () {
                    $(this).find("input.more-info-button").hide();
                });

                AdminContractList.ShowHideContractDropdownList();
            }
        });
    }
};
