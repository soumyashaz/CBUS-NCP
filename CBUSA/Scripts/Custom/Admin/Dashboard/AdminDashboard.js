$(document).ready(function () {

    BindControls();
    //GetQuarterReportingWindow();
    GetReportingStatistics();
    //PopulateBurndownChart();
    //-- commented on 4/4/2018 - angshuman
    //PopulateBuilderContractGridView();
    //-- commented on 4/4/2018 - angshuman
    //GetInProgressGrid();
    //GetCompletedGrid();
    //GetNotStartedGrid();

});

function BindControls() {
    $("#lnkSetQuarterEmailTemplate").on("click", ShowEmailTemplateWindowPopup);
    $("#lnkSetReportingWindow").on("click", ShowReportingWindowPopup);
    $("#lnkSetQuarterReminder").on("click", ShowQuarterReminderWindowPopup);

    PopulateCalendarControls();
    PopulateTextEditorControls();

    $(".form-control").on("change", function () {
        EnableDisableEmailSaveButton();
    });

    EnableDisableEmailSaveButton();
}

function ShowEmailTemplateWindowPopup() {
    $('#divSetQuarterEmailTemplate').modal('show');
}

function ShowReportingWindowPopup() {
    $('#divSetReportingWindow').modal('show');
}

function ShowQuarterReminderWindowPopup() {
    $('#divSetQuarterReminder').modal('show');
}

function CloseEmailTemplateWindowPopup() {
    $('#divSetQuarterEmailTemplate').modal('hide');
}

function CloseReportingWindowPopup() {
    $('#divSetReportingWindow').modal('hide');
}

function CloseQuarterReminderWindowPopup() {
    $('#divSetQuarterReminder').modal('hide');
}

function PopulateTextEditorControls() {
    $("#txtInvitationEmailTemplate").kendoEditor({
        tools: [
            "bold",
            "italic",
            "underline",
            "strikethrough",
            "justifyLeft",
            "justifyCenter",
            "justifyRight",
            "justifyFull",
            "insertUnorderedList",
            "insertOrderedList",
            "indent",
            "outdent",
            "createLink",
            "unlink",
            "subscript",
            "superscript",
            "createTable",
            "addRowAbove",
            "addRowBelow",
            "addColumnLeft",
            "addColumnRight",
            "deleteRow",
            "deleteColumn",
            "viewHtml",
            "formatting",
            "fontName",
            "fontSize",
            "foreColor",
            "backColor"
        ],
    });

    $("#txtReminderEmailTemplate").kendoEditor({
        tools: [
            "bold",
            "italic",
            "underline",
            "strikethrough",
            "justifyLeft",
            "justifyCenter",
            "justifyRight",
            "justifyFull",
            "insertUnorderedList",
            "insertOrderedList",
            "indent",
            "outdent",
            "createLink",
            "unlink",
            "subscript",
            "superscript",
            "createTable",
            "addRowAbove",
            "addRowBelow",
            "addColumnLeft",
            "addColumnRight",
            "deleteRow",
            "deleteColumn",
            "viewHtml",
            "formatting",
            "fontName",
            "fontSize",
            "foreColor",
            "backColor"
        ],
    });
}

function PopulateCalendarControls() {
    var arrHolidayList = [];

    //Ajax call to fetch holiday list
    $.ajax({
        url: AjaxCallUrl.GetHolidayList,
        type: "GET",
        //dataType: "json",
        data: '{}',
        contentType: "application/json; charset=utf-8",
        success: function (HolidayList) {
            var HolidayCount = HolidayList.Data.length;

            for (var i = 0; i < HolidayCount; i++) {
                var strHoliday = (HolidayList.Data[i].HolidayDate)
                strHoliday = strHoliday.replace("/Date(", "").replace(")/", "");

                var HolidayDate = moment(new Date(parseInt(strHoliday))).format("MM/DD/YYYY");
                arrHolidayList.push(new Date(HolidayDate));
            }

            //Ajax call to fetch reporting window
            $.ajax({
                url: AjaxCallUrl.GetReportingWindow,
                type: "GET",
                dataType: "json",
                data: {},
                contentType: "application/json; charset=utf-8",
                success: function (quarter) {
                    var tempDate = new Date();
                    var TZOffset = parseInt(tempDate.getTimezoneOffset());

                    var strFromDate = (quarter.ReportingStartDate)
                    strFromDate = strFromDate.replace("/Date(", "").replace(")/", "");

                    var strToDate = (quarter.ReportingEndDate)
                    strToDate = strToDate.replace("/Date(", "").replace(")/", "");

                    var FromDate; // = moment(new Date(parseInt(strFromDate))).format("MM-DD-YYYY");
                    var ToDate; // = moment(new Date(parseInt(strToDate))).format("MM-DD-YYYY");

                    if (TZOffset > 0) {
                        FromDate = moment(moment(new Date(parseInt(strFromDate))).add(1, 'd').format('MM-DD-YYYY'));
                        ToDate = moment(moment(new Date(parseInt(strToDate))).add(1, 'd').format('MM-DD-YYYY'));
                    } else {
                        FromDate = moment(new Date(parseInt(strFromDate))).format("MM-DD-YYYY");
                        ToDate = moment(new Date(parseInt(strToDate))).format("MM-DD-YYYY");
                    }

                    PopulateQuarterReminderDates(arrHolidayList, FromDate, ToDate);

                    $("#divClnReportingStartDate").kendoCalendar({
                        value: new Date(FromDate),
                        dates: arrHolidayList,
                        disableDates: function (date) {
                            var dates = $("#divClnReportingStartDate").data("kendoCalendar").options.dates;
                            if (date && CompareDates(date, dates, FromDate, ToDate)) {
                                return true;
                            } else {
                                return false;
                            }
                        }
                    });
                    $("#divClnReportingEndDate").kendoCalendar({
                        value: new Date(ToDate),
                        dates: arrHolidayList,
                        disableDates: function (date) {
                            var dates = $("#divClnReportingEndDate").data("kendoCalendar").options.dates;
                            if (date && CompareDates(date, dates, FromDate, ToDate)) {
                                return true;
                            } else {
                                return false;
                            }
                        }
                    });
                },
                error: function (err) {
                    //console.log(err);
                }
            });
        },
        error: function (err) {
            //alert(err);
        }
    });
}

function PopulateQuarterReminderDates(HolidayList, ReportingStartDate, ReportingEndDate) {
    var arrHolidayList = HolidayList;

    var cFromDate = new Date(ReportingStartDate);
    var cToDate = new Date(ReportingEndDate);

    $.ajax({
        url: AjaxCallUrl.GetQuarterReminderDates,
        type: "GET",
        dataType: "json",
        data: {},
        contentType: "application/json; charset=utf-8",
        success: function (QuarterReminderDates) {
            var ReminderCount = QuarterReminderDates.Data.length;

            if (ReminderCount > 0) {
                for (var i = 0; i < ReminderCount; i++) {
                    var strReminderDate = (QuarterReminderDates.Data[i].ReminderDate);
                    strReminderDate = strReminderDate.replace("/Date(", "").replace(")/", "");
                    
                    var tempDate = new Date(parseInt(strReminderDate));
                    var TZOffset = parseInt(tempDate.getTimezoneOffset());

                    var ReminderDate = moment(new Date(parseInt(strReminderDate))).format("MM-DD-YYYY");

                    if (TZOffset > 0) {
                        ReminderDate = moment(moment(new Date(parseInt(strReminderDate))).add(1, 'd').format('MM-DD-YYYY'));
                    }

                    //console.log(ReminderDate);

                    //var ReminderDate = moment(new Date(parseInt(strReminderDate))).format("MM-DD-YYYY");
                    var CurrDate = moment(new Date());
                    if (CurrDate >= moment(new Date(ReminderDate)))
                    {
                        var clnCtrlName = "#divClnReminderDate" + (i + 1);
                        $(clnCtrlName).kendoCalendar({
                            value: new Date(ReminderDate),
                            dates: arrHolidayList,
                            min: new Date(cFromDate.getFullYear(), cFromDate.getMonth(), cFromDate.getDate()),
                            max: new Date(cToDate.getFullYear(), cToDate.getMonth(), cToDate.getDate()),
                            //disableDates: function (date) {
                            //    var dates = $(clnCtrlName).data("kendoCalendar").options.dates;
                            //    if (date && CompareDatesForQuarterReminder(date, dates)) {
                            //        return true;
                            //    } else {
                            //        return false;
                            //    }
                            //}
                        });
                        $(clnCtrlName).find("*").prop("disabled", true);
                    }
                    else
                    {
                        var clnCtrlName = "#divClnReminderDate" + (i + 1);
                        $(clnCtrlName).kendoCalendar({
                            value: new Date(ReminderDate),
                            dates: arrHolidayList,
                            min: new Date(cFromDate.getFullYear(), cFromDate.getMonth(), cFromDate.getDate()),
                            max: new Date(cToDate.getFullYear(), cToDate.getMonth(), cToDate.getDate()),
                            disableDates: function (date) {
                                var dates = $(clnCtrlName).data("kendoCalendar").options.dates;
                                if (date && CompareDatesForQuarterReminder(date, dates)) {
                                    return true;
                                } else {
                                    return false;
                                }
                            }
                        });
                    }
                }
            } else {
                for (var i = 1; i <= 4; i++) {
                    var ReminderDate = moment().format("MM-DD-YYYY");
                    var clnCtrlName = "#divClnReminderDate" + i;
                    $(clnCtrlName).kendoCalendar({
                        value: new Date(ReminderDate),
                        dates: arrHolidayList,
                        min: new Date(cFromDate.getFullYear(), cFromDate.getMonth(), cFromDate.getDate()),
                        max: new Date(cToDate.getFullYear(), cToDate.getMonth(), cToDate.getDate()),
                        disableDates: function (date) {
                            var dates = $(clnCtrlName).data("kendoCalendar").options.dates;
                            if (date && CompareDatesForQuarterReminder(date, dates)) {
                                return true;
                            } else {
                                return false;
                            }
                        }
                    });
                }
            }
        },
        error: function (err) {
            //console.log(err);
        }
    });
}

function CompareDates(date, disabledates, FromDate, ToDate) {
    var cFromDate = new Date(FromDate);
    var cToDate = new Date(ToDate);
    var currDate = new Date(moment());

    if ((date.getDate() == cFromDate.getDate() && date.getMonth() == cFromDate.getMonth() && date.getYear() == cFromDate.getYear())
        || (date.getDate() == cToDate.getDate() && date.getMonth() == cToDate.getMonth() && date.getYear() == cToDate.getYear())
        || (date.getDate() == currDate.getDate() && date.getMonth() == currDate.getMonth() && date.getYear() == currDate.getYear())) {
        return false;
    }

    if (date.getDay() == 0 || date.getDay() == 6 || date < moment()) {
        return true;
    } else {
        for (var i = 0; i < disabledates.length; i++) {
            if (disabledates[i].getDate() == date.getDate() &&
                disabledates[i].getMonth() == date.getMonth() &&
                disabledates[i].getYear() == date.getYear()) {
                return true;
            }
        }
    }    
}

function CompareDatesForQuarterReminder(date, disabledates) {
    if (date.getDay() == 0 || date.getDay() == 6 || date < moment()) {
        return true;
    } else {
        for (var i = 0; i < disabledates.length; i++) {
            if (disabledates[i].getDate() == date.getDate() &&
                disabledates[i].getMonth() == date.getMonth() &&
                disabledates[i].getYear() == date.getYear()) {
                return true;
            }
        }
    }
}

function SaveReportingWindow() {
    var ReportingStartDate = $("#divClnReportingStartDate").data("kendoCalendar").value();
    var ReportingEndDate = $("#divClnReportingEndDate").data("kendoCalendar").value();

    var FromDate = moment(new Date(ReportingStartDate)).format("MM-DD-YYYY");
    var ToDate = moment(new Date(ReportingEndDate)).format("MM-DD-YYYY");

    var isBefore = moment(new Date(ReportingStartDate)).isBefore(moment(new Date(ReportingEndDate)));

    if (isBefore == false) {
        alert("Reporting Start Date cannot be greater than Reporting End Date");
        return false;
    }

    //Ajax call to save reporting window
    $.ajax({
        url: AjaxCallUrl.SetReportingWindow,
        type: "POST",
        dataType: "json",
        data: JSON.stringify({
            ReportingFromDate: FromDate,
            ReportingToDate: ToDate
        }),
        contentType: "application/json; charset=utf-8",
        success: function (msg) {
            alert("Reporting window successfully updated!");
            CloseReportingWindowPopup();
        },
        error: function (err) {
            alert("Reporting window successfully updated!");
            CloseReportingWindowPopup();
        }
    });
}

function SaveQuarterReminder() {
    var strReminderDate1 = $("#divClnReminderDate1").data("kendoCalendar").value();
    var strReminderDate2 = $("#divClnReminderDate2").data("kendoCalendar").value();
    var strReminderDate3 = $("#divClnReminderDate3").data("kendoCalendar").value();
    var strReminderDate4 = $("#divClnReminderDate4").data("kendoCalendar").value();

    var cReminderDate1 = moment(new Date(strReminderDate1)).format("MM-DD-YYYY");
    var cReminderDate2 = moment(new Date(strReminderDate2)).format("MM-DD-YYYY");
    var cReminderDate3 = moment(new Date(strReminderDate3)).format("MM-DD-YYYY");
    var cReminderDate4 = moment(new Date(strReminderDate4)).format("MM-DD-YYYY");

    var arrReminderDates = [];
    arrReminderDates.push(cReminderDate1);
    arrReminderDates.push(cReminderDate2);
    arrReminderDates.push(cReminderDate3);
    arrReminderDates.push(cReminderDate4);

    //Ajax call to save quarter reminders
    $.ajax({
        url: AjaxCallUrl.SetQuarterReminderDates,
        type: "POST",
        dataType: "json",
        data: JSON.stringify({
            ReminderDates: arrReminderDates
        }),
        contentType: "application/json; charset=utf-8",
        success: function (msg) {
            alert("Quarter Reminder dates successfully set!");
            CloseQuarterReminderWindowPopup();
        },
        error: function (err) {
            alert("Quarter Reminder dates successfully set!");
            CloseQuarterReminderWindowPopup();
        }
    });
}

function EnableDisableEmailSaveButton() {

    var InvitationSubject = $("#txtInvitationEmailSubject").val().trim();
    var InvitationEmail = $("#txtInvitationEmailTemplate").data("kendoEditor").value().trim();
    var ReminderSubject = $("#txtReminderEmailSubject").val().trim();
    var ReminderEmail = $("#txtReminderEmailTemplate").data("kendoEditor").value().trim();

    $("#txtInvitationEmailSubject").css("border", "none");
    $(document).find("iframe.k-content").first().css("border", "none");
    $("#txtReminderEmailSubject").css("border", "none");
    $(document).find("iframe.k-content").last().css("border", "none");

    if (InvitationSubject == "" || InvitationEmail == "" || ReminderSubject == "" || ReminderEmail == "") {
        $("#btnSaveEmailTemplates").prop("disabled", "");
        $("#btnSaveEmailTemplates").addClass("disabled");

        if (InvitationSubject == "") {
            $("#txtInvitationEmailSubject").css("border", "solid 1px red");
        }

        if (InvitationEmail == "") {
            $(document).find("iframe.k-content").first().css("border", "solid 1px red");
        }

        if (ReminderSubject == "") {
            $("#txtReminderEmailSubject").css("border", "solid 1px red");
        }

        if (ReminderEmail == "") {
            $(document).find("iframe.k-content").last().css("border", "solid 1px red");
        }

    } else {
        $("#btnSaveEmailTemplates").removeProp("disabled");
        $("#btnSaveEmailTemplates").removeClass("disabled");
    }

    return;
}

function SaveQuarterEmailTemplate() {
    var arrEmailTemplate = [];
    arrEmailTemplate.push($("#txtInvitationEmailSubject").val());
    arrEmailTemplate.push($("#txtInvitationEmailTemplate").data("kendoEditor").value());
    arrEmailTemplate.push($("#txtReminderEmailSubject").val());
    arrEmailTemplate.push($("#txtReminderEmailTemplate").data("kendoEditor").value());

    //Ajax call to save quarter email templates
    $.ajax({
        url: AjaxCallUrl.SetQuarterEmailTemplates,
        type: "POST",
        dataType: "json",
        data: JSON.stringify({
            EmailTemplate: arrEmailTemplate
        }),
        contentType: "application/json; charset=utf-8",
        success: function () {
            CloseEmailTemplateWindowPopup();
            alert("Quarter Email Templates successfully set!");
        },
        error: function (err) {
            CloseEmailTemplateWindowPopup();
            alert("Quarter Email Templates successfully set!");
        }
    });
}

function GetCountdownStatistics() {
    //Ajax call to fetch countdown statistics
    $.ajax({
        url: AjaxCallUrl.GetCountdownStatistics,
        type: "GET",
        //dataType: "json", 
        data: '{}',
        contentType: "application/json; charset=utf-8",
        success: function (CountdownData) {
            var TotalDays, RemainingDays;
            var arrCountdownData = CountdownData.split("/");
            TotalDays = arrCountdownData[0];
            RemainingDays = arrCountdownData[1];

            var BuildersReported = parseInt($("#spnCompletedReportingCount").text());
            var BuildersRemaining = parseInt($("#spnTotalReportingCount").text()) - BuildersReported;

            if (RemainingDays > 0) {
                PopulateCountDownChart(TotalDays, RemainingDays, BuildersReported, BuildersRemaining);
            }
            else {
                PopulateCountDownChart(TotalDays, 0, BuildersReported, BuildersRemaining);
            }
        },
        error: function (err) {
            //alert(err);
        }
    });
}

function GetReportingStatistics() {
    //Ajax call to fetch reporting statistics
    $.ajax({
        url: AjaxCallUrl.GetReportingStatistics,
        type: "GET",
        data: '{}',
        contentType: "application/json; charset=utf-8",
        //dataType: "json",
        success: function (ReportStatus) {
            var CompletedReports, InProgressReports, NotStartedReports, TotalReports;
            ReportingStatus = ReportStatus;

            var arrReportingStatus = ReportingStatus.split("/");
            CompletedReports = arrReportingStatus[0];
            InProgressReports = arrReportingStatus[1];
            NotStartedReports = arrReportingStatus[2];

            TotalReports = parseInt(CompletedReports) + parseInt(InProgressReports) + parseInt(NotStartedReports);

            $("#spnCompletedReportingCount").text(CompletedReports);
            $("#spnInProgressReportingCount").text(InProgressReports);
            $("#spnNotStartedReportingCount").text(NotStartedReports);
            $("#spnTotalReportingCount").text(TotalReports);

            createProgressCountChart(CompletedReports, InProgressReports, NotStartedReports);

            GetCountdownStatistics();
        },
        error: function (err) {
            //alert(err);
        }
    });
}

function createProgressCountChart(CompletedReports, InProgressReports, NotStartedReports) {
    var x = parseInt(CompletedReports);
    var y = parseInt(InProgressReports);
    var z = parseInt(NotStartedReports);

    $("#ProgressChart").kendoChart({
        chartArea: {
            height: 70,
            width: 375,
            margin: {
                left: 0,
                top: 0,
                bottom: 0
            }
        },
        title: {
            visible: false
        },
        legend: {
            visible: false
        },
        seriesDefaults: {
            type: "bar",
            stack: {
                type: "100%"
            }
        },
        series: [{
            name: "Completed",
            data: [x],
            color: "#E2A542"
        }, {
            name: "In Progress",
            data: [y],
            color: "#FAB251"
        }, {
            name: "Not Started",
            data: [z],
            color: "#FAED51"
        }],
        valueAxis: {
            visible: false,
            line: {
                visible: false
            },
            majorGridLines: {
                visible: false
            },
            minorGridLines: {
                visible: false
            }
        },
        categoryAxis: {
            visible: false,
            line: {
                visible: false
            },
            majorGridLines: {
                visible: false
            }
        },
        tooltip: {
            visible: true,
            template: "#= series.name #: #= value #"
        }
    });

    $("#ProgressChart").data("kendoChart").options.series[0].visible = true;
    $("#ProgressChart").data("kendoChart").options.series[1].visible = true;
    $("#ProgressChart").data("kendoChart").options.series[2].visible = true;
    $("#ProgressChart").data("kendoChart").redraw();
}

function PopulateCountDownChart(TotalDays, DaysRemaining, BuildersReported, BuildersRemaining)
{
    var DaysPassed = TotalDays - DaysRemaining;
    
    var BaseColor = "9C640C";
    var RemainingDaysSeries = [];

    for (var i = 0; i < DaysRemaining; i++) {
        var dayColor = ColorLuminance(BaseColor, parseFloat(i/10));
        var dayData = { category: "Day_".concat(i), value: 1, color: dayColor };
        RemainingDaysSeries.push(dayData);
    }

    var PassedDays = { category: "DaysPassed", value: DaysPassed, color: "transparent" };
    RemainingDaysSeries.push(PassedDays);

    $("#CountdownChart").kendoChart({
        legend: {
            visible: false
        },
        chartArea: {
            background: "",
            width: 140,
            height: 140
        },
        seriesDefaults: {
            type: "donut",
            startAngle: 90,
            visual: function (e) {
                center = e.center;
                radius = e.radius;
                
                return e.createVisual();
            }
        },
        series: [{
            name: "Builders Remaining",
            holeSize: 20,
            data: [{
                category: "Remaining",
                value: BuildersRemaining,
                color: "#2072B8"
            }, {
                category: "Completed",
                value: BuildersReported,
                color: "transparent"
            }]
        }, {
            name: "Days Remaining",
            data: RemainingDaysSeries,
        }],
        render: function (e) {
            var draw = kendo.drawing;
            var geom = kendo.geometry;
            var chart = e.sender;
            
            var circleGeometry = new geom.Circle(center, radius);
            var bbox = circleGeometry.bbox();
            
            var text = new draw.Text(DaysRemaining, [0, 0], {
                font: "14px Verdana,Arial,sans-serif"
            });
            
            draw.align([text], bbox, "center");
            draw.vAlign([text], bbox, "center");
            
            e.sender.surface.draw(text);
        }
    });
}

function ColorLuminance(hex, lum) {

    // validate hex string
    hex = String(hex).replace(/[^0-9a-f]/gi, '');
    if (hex.length < 6) {
        hex = hex[0] + hex[0] + hex[1] + hex[1] + hex[2] + hex[2];
    }
    lum = lum || 0;

    // convert to decimal and change luminosity
    var rgb = "#", c, i;
    for (i = 0; i < 3; i++) {
        c = parseInt(hex.substr(i * 2, 2), 16);
        c = Math.round(Math.min(Math.max(0, c + (c * lum)), 255)).toString(16);
        rgb += ("00" + c).substr(c.length);
    }

    return rgb;
}

function PopulateBurndownChart() {
    var arrBurndownData = [];

    //Ajax call to fetch burndown statistics
    $.ajax({
        url: AjaxCallUrl.GetBurndownStatistics,
        type: "GET",
        data: '{}',
        contentType: "application/json; charset=utf-8",
        //dataType: "json",
        success: function (BurndownData) {

            for (var i = 0; i < BurndownData.length; i++) {
                var DailyBurndownData = { value: BurndownData[i][1].Value, date: new Date(BurndownData[i][0].Value) };
                arrBurndownData.push(DailyBurndownData);
            }

            $("#divBurndownChart").kendoChart({
                chartArea: {
                    height: 300,
                    width: 300,
                    margin: {
                        left: 0,
                        top: 0,
                        bottom: 0
                    }
                },
                dataSource: {
                    data: arrBurndownData
                },
                series: [{
                    type: "line",
                    aggregate: "max",
                    field: "value",
                    categoryField: "date"
                }],
                categoryAxis: {
                    visible: false,
                    type: "date",
                    baseUnit: "days",
                    line: {
                        visible: false
                    }
                },
                valueAxis: {
                    line: {
                        visible: false
                    }
                }
            });
        },
        error: function (err) {
            alert("Error generating Burndown Chart!!");
        }
    });    
}

function PopulateBuilderContractGridView() {
    $("#GrdBuilderContractProjectHierarchy").kendoGrid({
        dataSource: {
            transport: {
                read: {
                    url: AjaxCallUrl.GetDetailsOfAllBuilderContractsForCurrentQuarter,
                    dataType: "Json",
                    type: "GET",
                },
            },
            schema: {

                data: "data",
                total: "total",
                //groups: function (response) {
                //    response.group.forEach(BindGroupData);
                //    return response.group; // groups are returned in the "groups" field of the response
                //},
                model: {
                    id: "BuilderId",
                    fields: {
                        BuilderId: { editable: false },
                        BuilderName: {
                            type: "string", editable: false
                        },
                        MarketId: {
                            type: "string", editable: false
                        },
                        MarketName: {
                            type: "string", editable: false
                        },
                        ContractId: {
                            type: "string", editable: false
                        },
                        ContractName: {
                            type: "string", editable: false
                        },
                        VendorName: {
                            type: "string", editable: false
                        },
                        TotalProjects: {
                            type: "number", editable: false
                        },
                        ReportedProjects: {
                            type: "number", editable: false
                        },
                        ContractEnrolledDate: { type: "date", format: "{0:MM/dd/yyyy}", editable: false },
                        ListReport: {type:"number"}
                    }
                }
            },
            
            //group: GroupAble,
            batch: true,
            pageSize: 50,
            serverPaging: true,
            serverFiltering: true,
            serverSorting: true,
            //serverGrouping: true,
        },
        excel: {
            fileName: "BuilderContractProjectReport" + GetTodaysDate() + ".xlsx",
                proxyURL: "https://demos.telerik.com/kendo-ui/service/export",
                filterable: true
            ///allPages: true
            },
        height: 600,
        filterable: {
            extra: false,
            operators: {
                string: {
                    startswith: "Starts with",
                    eq: "Is equal to",
                    neq: "Is not equal to",
                    contains: "Contains",
                    doesnotcontain: "Does not contain",
                    endswith: "Ends with",
                },
                number: {

                    eq: "Is equal to",
                    neq: "Is not equal to",


                    gt: "Is Greater Than",

                    lt: "Is Less Than",
                }, date: {
                    eq: "Is equal to",
                    neq: "Is not equal to",


                    gt: "Is after",

                    lt: "Is before",
                }, boolean: {
                    eq: "Is equal to",
                    neq: "Is not equal to",
                }

            },
           
        },
        filter: function (e) {

            if (e.filter) {

                e.filter.filters[0].value = e.filter.filters[0].value.trim();
            }
        },
        lockedHeader: true,
        reorderable: true,
        resizable: true,
        selectable: 'row',
        sortable: {
            mode: "multiple",
            allowUnsort: true,
            //showIndexes: true
        },
        groupable: true,
        toolbar: kendo.template($("#TemplateHierarchyGridToolBar").html()),
        pageable: {
            refresh: true,
            navigatable: true,
            pageSizes: [50, 100, 200, 250, 500],
            buttonCount: 5,
        },
        detailInit: PopulateProjectGridView,
        dataBound: function () {
            detailExportPromises = [];
            this.expandRow(this.tbody.find("tr.k-master-row").first());
        },
        //excelExport: GetAllPageExcel,
        columns: [
            {
                field: "BuilderName",
                title: "Builder",
                width: "110px"
            },
            {
                field: "ContractName",
                title: "Contract",
                width: "110px"
            },
            {
                field: "VendorName",
                title: "Vendor",
                width: "110px"
            },
            {
                field: "ReportedProjects",
                title: "Reported",
                width: "110px",
                template: "<div><span><i class='#=ProjectStatusChart(ListReport)#'></i></span><span class='pull-right'>#:ReportedProjects#" + "/" +"#:TotalProjects#</span></div> ",
                filterable: false,
                sortable: false,
                groupable: false,
            },
            {
                field: "MarketName",
                title: "Market",
                width: "110px"
            },
            {
                field: "ContractEnrolledDate",
                title: "Date Enrolled",
                width: "110px",
                format: "{0:MM/dd/yyyy}",
                filterable: {
                    ui: function (element) {
                        element.kendoDatePicker({
                            format: "MM/dd/yyyy"
                        });
                    }
                },
            },
             {
                 field: "TotalProjects",
                title: "Contract Action",
                width: "110px",
                template:"<a><i class='fa fa-calendar-times-o'></i></a><a><i class='fa fa fa-times-circle-o'></i></a><a><i class='fa fa fa-flag-o'></i></a>",
                filterable: false,
                sortable: false,
                groupable:false,
            }
        ]
    });
}

function PopulateProjectGridView(e) {
    
    $("<div/>").appendTo(e.detailCell).kendoGrid({
        dataSource: {
            transport: {
                read: {
                    url: AjaxCallUrl.GetDetailsOfAllProjectByBuilderContractsForCurrentQuarter,
                    dataType: "Json",
                    type: "GET",
                    data: { "BuilderId": e.data.BuilderId, "ContractId": e.data.ContractId}
                },
            },
            schema: {

                data: "data",
                total: "total",
                model: {
                    id: "ProjectId",
                    fields: {
                        ProjectId: { editable: false },
                        BuilderId: { editable: false },
                        ContractId: { editable: false },
                        ProjectName: {
                            type: "string", editable: false
                        },
                        LotNo: {
                            type: "string", editable: false
                        },
                        Address: {
                            type: "string", editable: false
                        },
                        City: {
                            type: "string", editable: false
                        },
                        State: {
                            type: "string", editable: false
                        },
                        Zip: {
                            type: "string", editable: false
                        },
                        ProjectStatus: {
                            type: "number", editable: false
                        },
                        
        ProjectCreatedOn: { type: "date", format: "{0:MM/dd/yyyy}", editable: false }
                    }
                }
            },
            batch: true,
            pageSize: 50,
            serverPaging: true,
            serverFiltering: true,
            serverSorting: true,
           
        },
        excelExport: function (e) {
            // prevent saving the file
            e.preventDefault();

            // resolve the deferred
            //deferred.resolve({
            //  masterRowIndex: masterRowIndex,
            //  sheet: e.workbook.sheets[0]
            //});
        },
        filterable: {
            extra: false,
            operators: {
                string: {
                    startswith: "Starts with",
                    eq: "Is equal to",
                    neq: "Is not equal to",
                    contains: "Contains",
                    doesnotcontain: "Does not contain",
                    endswith: "Ends with",
                },
                number: {

                    eq: "Is equal to",
                    neq: "Is not equal to",


                    gt: "Is Greater Than",

                    lt: "Is Less Than",
                }, date: {
                    eq: "Is equal to",
                    neq: "Is not equal to",


                    gt: "Is after",

                    lt: "Is before",
                }, boolean: {
                    eq: "Is equal to",
                    neq: "Is not equal to",
                }

            },

        },
        filter: function (e) {

            if (e.filter) {

                e.filter.filters[0].value = e.filter.filters[0].value.trim();
            }
        },
        lockedHeader: true,
        reorderable: true,
        resizable: true,
        selectable: 'row',
        sortable: {
            mode: "multiple",
            allowUnsort: true,
            //showIndexes: true
        },
        groupable: false,
        //toolbar: kendo.template($("#TemplateHierarchyGridToolBar").html()),
        pageable: {
            refresh: true,
            navigatable: true,
            pageSizes: [5, 10, 25, 50, 100],
            buttonCount: 5,
        },
        //detailInit: PopulateProjectGridView,
        //dataBound: function () {
        //    this.expandRow(this.tbody.find("tr.k-master-row").first());
        //},
        columns: [
            
            
            {
                field: "ProjectName",
                title: "Project Name",
                width: "110px"
            },
            {
                field: "LotNo",
                title: "LotNo",
                width: "110px"
            },
            {
                field: "Address",
                title: "Address",
                width: "110px"
            },
            {
                field: "City",
                title: "City",
                width: "110px"
            },
            {
                field: "State",
                title: "State",
                width: "110px"
            },
            {
                field: "Zip",
                title: "Zip",
                width: "110px"
            },
            {
                field: "ProjectCreatedOn",
                title: "Created",
                width: "110px",
                format: "{0:MM/dd/yyyy}",
                filterable: {
                    ui: function (element) {
                        element.kendoDatePicker({
                            format: "MM/dd/yyyy"
                        });
                    }
                },
                
            },
            {
                field: "ProjectStatus",
                title: "Status",
                width: "110px",
                template: "<span><i class='#=ProjectStatusChart(ProjectStatus)#'></i></span>",
                filterable: false,
                sortable: false,
                groupable: false,
            },
            {
                field: "ProjectId",
                title: "Action",
                width: "110px",
                template: "<a><i class='fa fa-pencil'></i></a><a><i class='fa fa-calendar-times-o'></i></a><a><i class='fa fa-times-circle-o'></i></a><a><i class='fa fa-flag-o'></i></a>",
                filterable: false,
                sortable: false,
                groupable: false,
            }
            
        ]
    });
}

function CreateBuilderContractChart(BuilderId, ContractId,TotalProjects, ReportedProjects) {
    var ValueA = Math.round(parseInt(ReportedProjects) / parseInt(TotalProjects) * 100);
    var ValueB = Math.round((parseInt(TotalProjects) - parseInt(ReportedProjects)) / parseInt(TotalProjects) * 100);
    $('#Pie' + BuilderId + ContractId)
        .kendoChart({
        legend: {
            visible: false,

        },
        chartArea: {
            width: 20,
            height: 20
        },
        series: [{
            type: "pie",
            startAngle: 0,
            border: {
                width: 0.5,
                color: "black"
            },
            data: [{
                category: "A",
                value: ValueA,
                color: "#ffffff"
            }, {
                category: "B",
                value: ValueB,
                color: "#000000"
            },]
        }],

    });
}

function ProjectStatusChart(ProjectStatus) {
    switch (ProjectStatus) {
        case 1:
            return "fa fa-circle ";
            break;
        case 2:
            return "fa fa-circle-thin  ";
            break;
        case 3:
            return "fa fa-circle ";            break;
        case 0:
            return "fa fa-circle ";
            break;

    }
}

function ProjectReportingChart(TotalProjects, ReportedProjects) {
    var ValueA = parseInt(ReportedProjects) / parseInt(TotalProjects) ;
    if (ValueA == 0) {
        return "fa fa-circle-thin  ";
    }
    else if (ValueA == 1) {
        return "fa fa-circle ";
    }
    else {
        return "fa fa-check-circle-o";
    }

    
}

function GetTodaysDate() {
    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1; //January is 0!

    var yyyy = today.getFullYear();
    if (dd < 10) {
        dd = '0' + dd;
    }
    if (mm < 10) {
        mm = '0' + mm;
    }
    var today = mm+ '-' + dd + '-' + yyyy;
   return today;
}

$(document).on('click', '#SearchButtonForBuilderContract', function () {

    var val = $('#SearchBoxForBuilderContract').val().trim();
    if (val.length <= 0 && val == '') {
        BuilderContractGridClearFilter();
    }
    else {
        $("#GrdBuilderContractProjectHierarchy").data("kendoGrid").dataSource.filter({
            logic: "or",
            filters: [
                {
                    field: "BuilderName",
                    operator: "contains",
                    value: val
                },
                {
                    field: "ContractName",
                    operator: "contains",
                    value: val
                },
                {
                    field: "VendorName",
                    operator: "contains",
                    value: val
                },

                {
                    field: "MarketName",
                    operator: "contains",
                    value: val
                },
               
            ]
        });
       
    }

});

function BuilderContractGridClearFilter() {
    var grid = $("#GrdBuilderContractProjectHierarchy").data("kendoGrid");
    $('#SearchBoxForBuilderContract').val('');
    grid.dataSource.filter({});
}

$(document).on('click', '#BtnResetFilter', function () { BuilderContractGridClearFilter(); });

$(document).on('click', '.AddTask', function () {
    var value = $(this).data('value');
    var FilterData = [];
    switch (value) {
        case 0:
            FilterData = [{
                field: "ListReport",
                operator: "eq",
                value: 0
            }]
            break;
        case 1:
            FilterData = [{
                field: "ListReport",
                operator: "eq",
                value: 1
            }]
            break;

        case 2:
            FilterData = [{
                field: "ListReport",
                operator: "eq",
                value: 2
            }]
            break;
    }
    $("#GrdBuilderContractProjectHierarchy").data("kendoGrid").dataSource.filter({
        logic: "and",
        filters: FilterData
    });
});

function exportChildData(BuilderID, ContractID, rowIndex) {
    var deferred = $.Deferred();

    detailExportPromises.push(deferred);

    var rows = [{
        cells: [
            // First cell
            { value: "ProjectName" },
            // Second cell
            { value: "LotNo" },
            // Third cell
            { value: "Address" },
            // Fourth cell
            { value: "City" },
            // Fifth cell
            { value: "State" },
            { value: "Zip" },
            { value: "ProjectCreatedOn" },
            { value: "ProjectStatus" }
        ]
    }];
    //dataSource.read( { "BuilderId": BuilderID, "ContractId": ContractID });
    dataSource.filter({
        logic: "and",
        filters: [{ field: "BuilderId", operator: "eq", value: BuilderID }, { field: "ContractId", operator: "eq", value: ContractID }]
    });

    var exporter = new kendo.ExcelExporter({
        columns: [{ field: "ProjectName" },
        // Second cell
            { field: "LotNo" },
        // Third cell
            { field: "Address" },
        // Fourth cell
            { field: "City" },
        // Fifth cell
            { field: "State" },
            { field: "Zip" },
            { field: "ProjectCreatedOn" },
            { field: "ProjectStatus" }],
        dataSource: dataSource
    });

    exporter.workbook().then(function (book, data) {
        deferred.resolve({
            masterRowIndex: rowIndex,
            sheet: book.sheets[0]
        });
    });
}

function GetAllPageExcel(e) {
    
        e.preventDefault();

        var workbook = e.workbook;

        detailExportPromises = [];

        var masterData = e.data;

        for (var rowIndex = 0; rowIndex < masterData.length; rowIndex++) {
            exportChildData(masterData[rowIndex].BuilderId, masterData[rowIndex].ContractId, rowIndex);
        }

        $.when.apply(null, detailExportPromises)
            .then(function () {
                // get the export results
                var detailExports = $.makeArray(arguments);

                // sort by masterRowIndex
                detailExports.sort(function (a, b) {
                    return a.masterRowIndex - b.masterRowIndex;
                });

                // add an empty column
                workbook.sheets[0].columns.unshift({
                    width: 30
                });

                // prepend an empty cell to each row
                for (var i = 0; i < workbook.sheets[0].rows.length; i++) {
                    workbook.sheets[0].rows[i].cells.unshift({});
                }

                // merge the detail export sheet rows with the master sheet rows
                // loop backwards so the masterRowIndex doesn't need to be updated
                for (var i = detailExports.length - 1; i >= 0; i--) {
                    var masterRowIndex = detailExports[i].masterRowIndex + 1; // compensate for the header row

                    var sheet = detailExports[i].sheet;

                    // prepend an empty cell to each row
                    for (var ci = 0; ci < sheet.rows.length; ci++) {
                        if (sheet.rows[ci].cells[0].value) {
                            sheet.rows[ci].cells.unshift({});
                        }
                    }

                    // insert the detail sheet rows after the master row
                    [].splice.apply(workbook.sheets[0].rows, [masterRowIndex + 1, 0].concat(sheet.rows));
                }

                // save the workbook
                kendo.saveAs({
                    dataURI: new kendo.ooxml.Workbook(workbook).toDataURL(),
                    fileName: "Export.xlsx"
                });


            });
}

var detailExportPromises = [];

//var dataSource = new kendo.data.DataSource({
    
//    transport: {
//        read: AjaxCallUrl.GetDetailsOfAllProjectByBuilderContractsForCurrentQuarterByQuater,
//        dataType: "Json",
//        type: "GET",
//    }
//});
//dataSource.read();

function GetInProgressGrid() {
    $("#GetInProgressGrid").kendoGrid({
        dataSource: {
            transport: {
                read: {
                    url: AjaxCallUrl.GetDetailsOfInProgressBuilders,
                    dataType: "Json",
                    type: "GET",
                }
            },
            schema: {
                model: {
                    id: "BuilderId",
                    fields: {
                        BuilderId: { editable: false },
                        BuilderName: {
                            type: "string", editable: false
                        },
                        MarketId: {
                            type: "string", editable: false
                        },
                        MarketName: {
                            type: "string", editable: false
                        },
                        ContractStatus: {
                            type: "string", editable: false
                        },
                        LastActivityDate: {
                            type: "string", editable: false
                        },
                        
                    }
                }
            },

        },
        pageable: false,
        excel: {
            fileName: "GetInProgressGrid" + GetTodaysDate() + ".xlsx",
            proxyURL: "https://demos.telerik.com/kendo-ui/service/export",
            filterable: true
            ///allPages: true
        },
        toolbar: ["excel"],
        lockedHeader:true,
        //rowTemplate: kendo.template($("#TemplateBuilderDetails").html()),
        columns: [
            {
                
                title: "Builder Name", 
                headerAttributes: {
                    
                    style: "display: none"
                },
                template: kendo.template($("#TemplateBuilderDetails").html())
                
            },
            //{
            //    field: "ContractStatus",
            //    title: "Contract Status", 
                

            //},
            //{
            //    field: "MarketName",
            //    title: "Market Name", 
               

            //},
            //{
            //    field: "LastActivityDate",
            //    title: "Last Activity Date",
                

            //},
        ],
        
    });
    
}

function GetCompletedGrid() {
    $("#GetCompletedGrid").kendoGrid({
        dataSource: {
            transport: {
                read: {
                    url: AjaxCallUrl.GetDetailsOfReportingCompletedBuilders,
                    dataType: "Json",
                    type: "GET",
                }
            },
            schema: {
                model: {
                    id: "BuilderId",
                    fields: {
                        BuilderId: { editable: false },
                        BuilderName: {
                            type: "string", editable: false
                        },
                        MarketId: {
                            type: "string", editable: false
                        },
                        MarketName: {
                            type: "string", editable: false
                        },
                        ContractStatus: {
                            type: "string", editable: false
                        },
                        LastActivityDate: {
                            type: "string", editable: false
                        },

                    }
                }
            },

        },
        pageable: false,
        toolbar: ["excel"],
        excel: {
            fileName: "GetCompletedGrid" + GetTodaysDate() + ".xlsx",
            proxyURL: "https://demos.telerik.com/kendo-ui/service/export",
            filterable: true
            ///allPages: true
        },
        lockedHeader: true,
        //rowTemplate: kendo.template($("#TemplateBuilderDetails").html()),
        columns: [
            {
                field: "BuilderName",
                title: "Builder Name",
                //headerAttributes: {

                //    style: "display: none"
                //}

            },
            {
                field: "ContractStatus",
                title: "Contract Status",


            },
            {
                field: "MarketName",
                title: "Market Name",


            },
            {
                field: "LastActivityDate",
                title: "Last Activity Date",


            },
        ],

    });
}

function GetNotStartedGrid() {
    $("#GetNotStartedGrid").kendoGrid({
        dataSource: {
            transport: {
                read: {
                    url: AjaxCallUrl.GetDetailsOfNotStartedBuilders,
                    dataType: "Json",
                    type: "GET",
                }
            },
            schema: {
                model: {
                    id: "BuilderId",
                    fields: {
                        BuilderId: { editable: false },
                        BuilderName: {
                            type: "string", editable: false
                        },
                        MarketId: {
                            type: "string", editable: false
                        },
                        MarketName: {
                            type: "string", editable: false
                        },
                        ContractStatus: {
                            type: "string", editable: false
                        },
                        LastActivityDate: {
                            type: "string", editable: false
                        },

                    }
                }
            },

        },
        pageable: false,
        toolbar: ["excel"],
        excel: {
            fileName: "GetNotStartedGrid" + GetTodaysDate() + ".xlsx",
            proxyURL: "https://demos.telerik.com/kendo-ui/service/export",
            filterable: true
            ///allPages: true
        },
        lockedHeader: true,
        //rowTemplate: kendo.template($("#TemplateBuilderDetails").html()),
        columns: [
            {
                field: "BuilderName",
                title: "Builder Name",
                //headerAttributes: {

                //    style: "display: none"
                //}

            },
            {
                field: "ContractStatus",
                title: "Contract Status",


            },
            {
                field: "MarketName",
                title: "Market Name",


            },
            {
                field: "LastActivityDate",
                title: "Last Activity Date",


            },
        ],

    });
}

function getReportName() {
    //var Rad = $("input[name='RadSurvey']:checked");
   // var name = Rad.siblings().filter("input[name='HdnExcelReportName']").val();
    return 'Excel';
}
//End