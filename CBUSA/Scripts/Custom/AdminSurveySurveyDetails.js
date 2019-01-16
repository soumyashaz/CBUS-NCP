$(document).ready(function () {
    SurveyDetails.AssignControl();
    SurveyDetails.ShowValidationSummaryMsg();

    //To disallow editing Start Date
    var SurveyId = $('#SurveyId').val();
    if (SurveyId != '0') {  //edit mode
        var datepicker = $("#StartDate").data("kendoDatePicker");

        var Today = new Date();
        var StartDate = new Date(datepicker.value());

        if (StartDate < Today) { //already published survey
            datepicker.min(datepicker.value());
            datepicker.max(datepicker.value());
        } else {
            datepicker.min(Today);
            datepicker.max($("#EndDate").data("kendoDatePicker").value());
        }
    }
});

var SurveyDetails = {
    AssignControl: function () {
        $("#SaveSurvey").on("click", SurveyDetails.SaveSurveyDetails);

        $("input[name='IsEnrolment']").change(function () {
            // $('#textbox1').val($(this).is(':checked'));
            $('#IsEnrollmentChange').val(1);
        });

    },
    ShowValidationSummaryMsg: function () {

        if ($("#HdnValidationSummaryFlag").val() == "1") {

            var DataUpdateWindow = $("#DataUpdateMessage").data("kendoWindow");
            if (DataUpdateWindow) {
                DataUpdateWindow.title("Error Information");
                var v = "<div class='msgboxError'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i>" + $("#HdnValidationSummaryMsg").html() + " </div>";
                $(v).append($("#HdnValidationSummaryMsg").html());
                //    console.log(v);
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
                // futuredate: "Cannot be a past date or current date",
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

                //futuredate: function (input) {
                //    var validate = input.data('futuredate');
                //    // if the input has a `data-available` attribute...
                //    if (typeof validate !== 'undefined' && validate !== false && input.val() != "") {

                //        if (input.val() != "") {
                //            var date = kendo.parseDate(input.val());
                //            var TodayDate = kendo.parseDate(new Date());
                //            return date > TodayDate
                //        }

                //        return true;
                //    }
                //    return true;
                //},
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
        if ($("#IsNcpSurvey").val() === "True") {
            return;
        }
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

            },
            success: function (data) {
                if (data.IsSuccess) {

                    $("#IsEnrolment").removeAttr("disabled");
                }
                else {
                    $("#IsEnrolment").attr("disabled", true);
                }

            }
        });

    },
    SendDataBeforeLoadDropDown: function () {
        // console.log($("#IsNcpSurvey").val());
        if ($("#IsNcpSurvey").val().toLowerCase() === "true") {
            return { IsNcpSurvey: $("#IsNcpSurvey").val() };
        }
    }



};


function OnchangeFromDate() {
    var Validator = $("#FormSaveSurveyDetails").kendoValidator({
        messages: {
            mvcdate: "please enter correct date",
            custom: "Please enter a valid date from the given range",
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
            custom: function (input) {
                var fromdate = new Date(input.data('min'));
                var todate = new Date(input.data('max'));
                var currdate = new Date();

                if ((currdate.getDate() == fromdate.getDate()) && (currdate.getDate() == todate.getDate()) && 
                    (currdate.getMonth() == fromdate.getMonth()) && (currdate.getMonth() == todate.getMonth()) && 
                    (currdate.getFullYear() == fromdate.getFullYear()) && (currdate.getFullYear() == todate.getFullYear())) {

                    return true;
                }

                var SurveyId = $('#SurveyId').val();
                if (SurveyId != '0') {  //edit mode
                    if (typeof fromdate !== 'undefined' && fromdate !== false && input.val() != "") {                        
                        if (input.val() != "") {
                            var date = kendo.parseDate(input.val());
                            if (date < fromdate || date > todate) {
                                return false;
                            }
                        }
                        return true;
                    }
                }
                return true;
            },

            futuredate: function (input) {
                var validate = input.data('futuredate');
                // if the input has a `data-available` attribute...
                if (typeof validate !== 'undefined' && validate !== false && input.val() != "") {

                    if (input.val() != "") {
                        var date = kendo.parseDate(input.val());
                      //  var TodayDate = kendo.parseDate(input.data('prevdate'));

                        /* var TodayDate = kendo.parseDate(input.data('prevdate'));*/

                        /*Add by Rabi on 12 April*/
                        var DateToday = new Date();
                        //  console.log((date.getMonth() + 1) + '/' + date.getDate() + '/' + date.getFullYear());
                        var TodayDate = kendo.parseDate((DateToday.getMonth() + 1) + '/' + DateToday.getDate() + '/' + DateToday.getFullYear());


                        return date >= TodayDate
                    }

                    return true;
                }
                return true;
            }

        }
    }).data("kendoValidator");
    var IsValid = Validator.validateInput($("#StartDate"));
    if (!IsValid) {
        var datepicker = $("#StartDate").data("kendoDatePicker");
        //datepicker.value('');
    }
    // console.log(IsValid);
}


function OnchangeToDate() {
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


            futuredate: function (input) {
                var validate = input.data('futuredate');
                // if the input has a `data-available` attribute...
                if (typeof validate !== 'undefined' && validate !== false && input.val() != "") {

                    if (input.val() != "") {
                        var date = kendo.parseDate(input.val());

                        /* var TodayDate = kendo.parseDate(input.data('prevdate'));*/

                        /*Add by Rabi on 12 April*/
                        var DateToday = new Date();
                        //  console.log((date.getMonth() + 1) + '/' + date.getDate() + '/' + date.getFullYear());
                        var TodayDate = kendo.parseDate((DateToday.getMonth() + 1) + '/' + DateToday.getDate() + '/' + DateToday.getFullYear());




                        // var todayDate = new Date();
                        //console.log(todayDate);

                        return date >= TodayDate
                    }

                    return true;
                }
                return true;
            }

        }
    }).data("kendoValidator");
    var IsValid = Validator.validateInput($("#EndDate"));
    if (!IsValid) {
        var datepicker = $("#EndDate").data("kendoDatePicker");
        datepicker.value('');
    }
    // console.log(IsValid);
}