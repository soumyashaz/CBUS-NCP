$(document).ready(function () {
    ContractDetail.LoadContractLogoPartialView();
    ContractDetail.AssignControls();
    ContractDetail.ShowHideContractList();

    $("#spnSelectedContract").text($("li.list-contract-name[data-current-contract='1']").find("a").text());
});

var ContractDetail = {

    AssignControls: function () {
        $("#lnkDownloadAttachment").click(function () {
            var DownLoad = AjaxCallUrl.GetAttachmentsZipUrl.replace("conid", $("#hdnContractId").val());
            window.location.href = DownLoad;
        });

        $("li.li-content-section").click(function () {
            $("li.li-content-section").each(function () {
                $(this).removeClass("selected");
                $(this).css("background-color", "");
            });
            $(this).addClass("selected");
            $(this).css("background-color", "#ddd");

            var SectionId = $(this).data("section-id");

            var scrollPos = $("div.variation-heading[data-section-id='" + SectionId + "']").offset().top;
            var divTop = $("div.overflow-area").offset().top + 20;

            $("div.overflow-area").animate({
                scrollTop: (scrollPos + $("div.overflow-area").scrollTop() - divTop)
            }, 1000);
        });
    },

    ShowHideContractList: function () {
        $(document.body).click(function () {
            $("#ulContractList").hide();
        });

        $("#lnkSelectAContract").click(function (e) {
            e.stopPropagation();
            $("#divContractLogoList").modal('show');
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
                $("#divBuilderContractLogos").html(result);

                kendo.ui.progress($(document.body), false);

                $("li.contract-logo").each(function () {
                    $(this).find("input.more-info-button").hide();
                });

                //ContractList.ShowHideContractDetailButton();
            }
        });
    }
};