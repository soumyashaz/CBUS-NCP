$(document).ready(function () {
    kendo.ui.progress($(document.body), true);

    ContractList.LoadContractLogoPartialView();  

    $("#liNationalContracts").mouseover(function () {
        $("#divNCPMenu").show();
    });

    $("#liNationalContracts").mouseout(function () {
        $("#divNCPMenu").hide();
    });
});

var ContractList = {
    ShowHideContractDetailButton: function () {
        $("li.contract-logo").mouseover(function () {
            $(this).find("input.more-info-button").show();
        });

        $("li.contract-logo").mouseout(function () {
            $(this).find("input.more-info-button").hide();
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

                ContractList.ShowHideContractDetailButton();
            }
        });
    }
};