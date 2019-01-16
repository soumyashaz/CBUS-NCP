$(document).ready(function () {

    SurveyDetails.AssignControl();
    SurveyDetails.ShowValidationSummaryMsg();
});

var SurveyDetails = {
    AssignControl: function () {
        $("#SaveSurvey").on("click", SurveyDetails.SaveSurveyDetails);
    },
    ShowValidationSummaryMsg: function () {

        if ($("#HdnValidationSummaryFlag").val() == "1") {

            var DataUpdateWindow = $("#DataUpdateMessage").data("kendoWindow");
            if (DataUpdateWindow) {
                DataUpdateWindow.title("Error Information");
                var v = "<div class='msgboxError'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i>" + $("#HdnValidationSummaryMsg").html() + " </div>";
                $(v).append($("#HdnValidationSummaryMsg").html());
                console.log(v);
                DataUpdateWindow.content(v);
                DataUpdateWindow.open();
            }
        }

    },
    SaveSurveyDetails: function () {
        var IsValid = SurveyDetails.ValidatSaveSurveyDetails();
        if (IsValid) {
            $("#FormSaveSurveyDetails").submit();
        }
        // console.log(IsValid);
    },
    ValidatSaveSurveyDetails: function () {
        var Validator = $("#FormSaveSurveyDetails").kendoValidator({
            messages: {
                mvcdate: "please enter correct date",
                custom: "Please enter valid value for my custom rule",
                required: "*",
                email: function (input) {
                    return getMessage(input);
                },
                date: "Not a valid date",
                futuredate: "Cannot be a past date or current date",
                lesserdate: "Invalid start Date",
                greaterdate: "Invalid End Date",
                contractproduct: function (input) {
                    return "Atleast one Product selection is required";
                },
                validurl: function (input) {

                    if (MessageCache.IsWebsiteCount) {
                        return "Please enter one url address";
                    }
                    else {
                        return "Invalid website address";
                    }
                }
            },
            rules: {

                mvcdate: function (input) {

                    if (input.is("[data-role=datepicker]")) {
                        if (input.val() != "") {
                            var d = kendo.parseDate(input.val(), "MM/dd/yyyy");
                            return d instanceof Date;
                        }
                        return true;
                    }

                    return true;
                },

                futuredate: function (input) {
                    var validate = input.data('futuredate');
                    // if the input has a `data-available` attribute...
                    if (typeof validate !== 'undefined' && validate !== false && input.val() != "") {

                        if (input.val() != "") {
                            var date = kendo.parseDate(input.val());
                            var TodayDate = kendo.parseDate(new Date());
                            return date > TodayDate
                        }

                        return true;
                    }
                    return true;
                },
                greaterdate: function (input) {
                    var validate = input.data('greaterdate');

                    var id = input.attr('id');

                    // if the input has a `data-available` attribute...
                    if (typeof validate !== 'undefined' && validate !== false && input.val() != "") {
                        //  console.log('hhh');

                        if (input.val() != "" && $("[name='" + input.data("lesserfield") + "']").val() != "") {
                            var date = kendo.parseDate(input.val()),
                             otherDate = kendo.parseDate($("[name='" + input.data("lesserfield") + "']").val());
                            return date > otherDate;
                        }
                        return true;
                    }
                    return true;
                },
            }
        }).data("kendoValidator");


        return Validator.validate();


    },
    onSelectContract: function (e) {
        var dataItem = this.dataItem(e.item.index());
        //  console.log(dataItem.ContractId)
        SurveyDetails.IsEnrollmentSurveyAvailable(dataItem.ContractId);
    },
    onDataBoundContract: function () {
        var ContractId = $("#ContractId").data("kendoDropDownList").value()
        SurveyDetails.IsEnrollmentSurveyAvailable(ContractId);
    },
    IsEnrollmentSurveyAvailable: function (ContractId) {
        // var ContractId = $("#ContractId").data("kendoDropDownList").value()
        var FormData = {
            'ContractId': ContractId
            //Store name fields value
        };
        $.ajax({ //Process the form using $.ajax()
            type: 'GET', //Method type
            url: AjaxCallUrl.IsEnrollmentSurveyAvailableUrl,
            data: FormData, //Forms name
            dataType: 'json',
            beforeSend: function () {
            },
            error: function (request, error) {
                // $(document).ajaxStop($.unblockUI);
                // console.log('gggg');
            },
            success: function (data) {
                if (data.IsSuccess) {
                    $("#IsEnrolment").attr("disabled", true);
                }
                else {
                }

            }
        });

    }



};
