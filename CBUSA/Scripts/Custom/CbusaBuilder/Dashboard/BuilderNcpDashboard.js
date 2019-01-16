var slideIndex = 1;

$(document).ready(function () {

    var ifrmHeight = $("#hdnIFrmHeight").val();

    window.parent.postMessage({
        'height': ifrmHeight,
        'viz': 'Dis',
        'location': window.location.href
    }, "*");

    setTimeout(ShowFirstSlide, 500);

    if (parseInt(ifrmHeight) > 240) {
        setTimeout(PopulateReportingCompletionGauge, 500);
        setTimeout(PopulateReportingStatusPieChart, 500);
        //PopulateReportingCompletionGauge();
        //PopulateReportingStatusPieChart();
    }    
});

function ShowFirstSlide() {
    slideIndex = 1;
    showSlides(slideIndex);
}

function plusSlides(n) {
    showSlides(slideIndex += n);
}

function showSlides(n) {
    var i;
    var slides = document.getElementsByClassName("mySlides");

    if (n > slides.length) { slideIndex = 1 }
    if (n < 1) { slideIndex = slides.length }
    for (i = 0; i < slides.length; i++) {
        slides[i].style.display = "none";
    }

    var divSlideId = "#divSlides_" + slideIndex;
    $(divSlideId).fadeIn(500);
}

function PopulateReportingCompletionGauge() {
    var ReportingCompletedCount = parseInt($("#hdnContractReportCompleted").val());
    var ReportingInProgressCount = parseInt($("#hdnContractReportInProgress").val());
    var ReportingNotStartedCount = parseInt($("#hdnContractReportNotStarted").val());

    var Total = (ReportingCompletedCount + ReportingInProgressCount + ReportingNotStartedCount);

    var CompletionPercent = 0;
    if (Total > 0) {
        CompletionPercent = parseInt((ReportingCompletedCount / Total) * 100);
    }

    $("#divGauge").kendoRadialGauge({
        pointer: {
            value: CompletionPercent,
            color: "#0e2d50"
        },
        scale: {
            minorUnit: 5,
            majorUnit: 20,
            startAngle: -30,
            endAngle: 210,
            max: 100,
            labels: {
                font: "8px Arial, Helvetica,sans-serif",
                color: "#0e2d50"
            }
        }
    });

    $("#divGauge").data("kendoRadialGauge").redraw();

    if (Total == 0) {
        $("#divGauge").css("margin-top", "-163px");
    } else {
        $("#divGauge").css("margin-top", "-56px");
    }
}

function PopulateReportingStatusPieChart()
{
    var ReportingCompletedCount = parseInt($("#hdnContractReportCompleted").val());
    var ReportingInProgressCount = parseInt($("#hdnContractReportInProgress").val());
    var ReportingNotStartedCount = parseInt($("#hdnContractReportNotStarted").val());
    var ReportSubmittedCount = 0;

    var TotalContracts = (ReportingCompletedCount + ReportingInProgressCount + ReportingNotStartedCount);

    var ReportSubmitted = false;

    if ($("#hdnIsReportSubmitted").val() === "True") {
        ReportSubmitted = true;
        ReportSubmittedCount = 100;

        ReportingCompletedCount = 0;
        ReportingInProgressCount = 0;
        ReportingNotStartedCount = 0;

        $("#btnNTRTQ").prop('disabled', 'disabled');
        $("#btnNTRTQ").addClass('disabled');

        $("#lnkReportPurchases").prop('disabled', 'disabled');
        $("#lnkReportPurchases").addClass('disabled');
        $("#lnkReportPurchases").css("cursor", "not-allowed");
        $("#lnkReportPurchases").css("pointer-events", "auto");
    }

    if (TotalContracts === 0) {
        $("#divNoReportableContract").show();
        $("#divReportingStatusLegend").hide();
    } else {
        $("#divNoReportableContract").hide();
        $("#divReportingStatusLegend").show();

        var lgReportSubmitted = "Report Submitted";
        var lgCompleted = "Completed (" + $("#hdnContractReportCompleted").val() + ")";
        var lgInProgress = "In Progress (" + $("#hdnContractReportInProgress").val() + ")";
        var lgNotStarted = "Not Started (" + $("#hdnContractReportNotStarted").val() + ")";

        $("#spnCompletedCount").text($("#hdnContractReportCompleted").val());
        $("#spnInProgressCount").text($("#hdnContractReportInProgress").val());
        $("#spnNotStartedCount").text($("#hdnContractReportNotStarted").val());

        if (ReportSubmitted == true) {
            $("#divReportingStatus").color
        }

        $("#divReportingStatus").kendoChart({
            legend: {
                visible: false,
                position: "right"
            },
            plotArea: {
                width: 150,
                height: 150
            },
            chartArea: {
                width: 150,
                height: 150
            },
            series: [{
                type: "pie",
                startAngle: 0,
                width: 150,
                border: {
                    width: 0.5,
                    color: "black"
                },
                data: [{
                    category: lgReportSubmitted,
                    value: ReportSubmittedCount,
                    color: "#3d9968"
                },
                {
                    category: lgCompleted,
                    value: ReportingCompletedCount,
                    color: "#0e2d50"
                },
                {
                    category: lgInProgress,
                    value: ReportingInProgressCount,
                    color: "#58cff5"
                },
                {
                    category: lgNotStarted,
                    value: ReportingNotStartedCount,
                    color: "#d3d3d3"
                }]
            }],
        });

        $("#divReportingStatus").data("kendoChart").redraw();

        if (ReportSubmitted == false && TotalContracts == ReportingCompletedCount) {
            setTimeout(PromptBuilderToSubmitReport, 2000);
        }
    }
}

function PromptBuilderToSubmitReport()
{
    $("#btnNTRTQ").prop('disabled', 'disabled');
    $("#btnNTRTQ").addClass('disabled');

    alert("You have reported your purchases for all the enrolled Contracts but haven't submitted your Rebate Report yet. Please click on the REPORT PURCHASES button to go to your Reporting Dashboard and submit the Rebate Report.");
}

function UpdateNTRTQForAllContract(qtrid) {
    var Confirm = confirm("You are about to mark all of your available projects as having no purchases for any contract this quarter. Are you sure you want to do this?");
    if (!Confirm) {
        return;
    }
    Confirm = confirm("Are you positive you made no eligible contract purchases at all this quarter?");
    if (!Confirm) {
        return;
    }

    $('#btnNTRTQ').addClass('disabled');
    kendo.ui.progress($("#ncpQuartlyRebateReport"), true);
    $.ajax({
        type: "POST",
        url: AjaxCallUrl.UpdateNoReportAllContract,
        data: { QuaterId: qtrid },
        cache: false,
        success: function (result) {
            kendo.ui.progress($("#ncpQuartlyRebateReport"), false);
            $('#btnNTRTQ').removeClass('disabled');
            if (result.Success) {
                alert("Project(s) for all contract(s) are updated successfully marked as 'Nothing To Report This Quarter'");
                window.location.href = window.location.href;
            }
            else {
                kendo.ui.progress($("#ncpQuartlyRebateReport"), false);
                $('#btnNTRTQ').removeClass('disabled');
                alert(result.DataMessage);
            }
        },
        error: function (request, error) {
            kendo.ui.progress($("#ncpQuartlyRebateReport"), false);
            $('#btnNTRTQ').removeClass('disabled');
            alert("Error Occured");
        }
    });
}