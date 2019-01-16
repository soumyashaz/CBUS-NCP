var type;
var flag;
var flgSaveClicked = false;
var ExistingProjectsCount = 0;
var ReportSubmitted;
var TotalContracts;
var ReportedContracts;
var CopySave = 1;

$(document).ready(function () {

    $("#divReportedContractList").html($("#hdnReportedContracts").val());

    GetReportingStatusForBuilder();

    // SurveyAddQuestion.AssignControl();
    Project.AssignControl();

    var KendoListView = $("#listViewProject").data("kendoListView");
    $('thead').first().css('display', 'block');
    $('thead').first().css('width', '100%');
    $('tbody').first().css('display', 'block');

    if ($('#hdnPageName').val() == "OpenProjects") {
        $('tbody').first().css('height', '400px');
    }

    $('tbody').first().css('width', '100%');
});
var Project = {
    AssignControl: function () {
        $("#SaveTable").on("click", Project.PopulateTable);
        $("#BtnCopy").on("click", Project.InitCopyProject);
        $("#BtnClose").on("click", Project.closeProject);
        $("#BtnReopen").on("click", Project.reopenProject);
        $("#BtnCancel").on("click", Project.CancelProject);
    },

    closeProject: function () {
        var Rad = $("input[name='RadProject']:checked");
        var ProjectId = Rad.siblings().filter("input[name='HdnProject']").val();

        if (parseInt(ProjectId) > 0) {
            var Confirm = confirm("Are you sure you want to close the selected project?");

            if (!Confirm) {
                return;
            }

            // kendo.ui.progress(".main-body-wrapper", false);
            var postData = { ProjectId: ProjectId }; //done
            $.ajax({ //Process the form using $.ajax()
                type: 'POST', //Method type
                url: AjaxCallUrl.CloseProject,
                data: postData, //Forms name
                dataType: 'json',
                // traditional: true,
                beforeSend: function () {
                },
                success: function (data) {
                    var DataUpdateWindow = $("#DataUpdateMessage").data("kendoWindow");
                    if (data.IsSuccess) {
                        //  kendo.ui.progress($(".main-body-wrapper"), false);

                        if (DataUpdateWindow) {
                            DataUpdateWindow.title("Update Information");
                            DataUpdateWindow.content("<div class='msgboxSuccess'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i> Data Update Successfully</div>");
                            DataUpdateWindow.open();
                            //redirect to listing page

                            var KendoListView = $("#listViewProject").data("kendoListView");
                            KendoListView.dataSource.read();
                        }
                    }
                    else {
                        if (DataUpdateWindow) {
                            DataUpdateWindow.title("Update Information");
                            DataUpdateWindow.content("<div class='msgboxSuccess'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i>This project is associated with previous quarterly reports that have not been submitted. Contact CBUSA for assistance resolving this issue.</div>");
                            DataUpdateWindow.open();
                            //redirect to listing page

                            var KendoListView = $("#listViewProject").data("kendoListView");
                            KendoListView.dataSource.read();
                        }
                    }
                },
                error: function (request, error) {
                    //  kendo.ui.progress($(".main-body-wrapper"), false);
                    if (DataUpdateWindow) {
                        DataUpdateWindow.title("Error Information");
                        DataUpdateWindow.content("<div class='msgboxError'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i> Error Occured </div>");
                        DataUpdateWindow.open();
                    }
                }
            });
        }
    },

    reopenProject: function () {

        var Rad = $("input[name='RadProject']:checked");
        var ProjectId = Rad.siblings().filter("input[name='HdnProject']").val();


        if (parseInt(ProjectId) > 0) {



            var Confirm = confirm("Are you sure?");

            if (!Confirm) {
                return;
            }

            // kendo.ui.progress(".main-body-wrapper", false);
            var postData = { ProjectId: ProjectId }; //done
            $.ajax({ //Process the form using $.ajax()
                type: 'POST', //Method type
                url: AjaxCallUrl.ReopenProject,
                data: postData, //Forms name
                dataType: 'json',
                // traditional: true,
                beforeSend: function () {
                },
                success: function (data) {
                    if (data.IsSuccess) {
                        //  kendo.ui.progress($(".main-body-wrapper"), false);
                        var DataUpdateWindow = $("#DataUpdateMessage").data("kendoWindow");
                        if (DataUpdateWindow) {
                            DataUpdateWindow.title("Update Information");
                            DataUpdateWindow.content("<div class='msgboxSuccess'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i> Data Update Successfully</div>");
                            DataUpdateWindow.open();
                            //redirect to listing page

                            var KendoListView = $("#listViewProject").data("kendoListView");
                            KendoListView.dataSource.read();
                        }
                    }
                },
                error: function (request, error) {
                    //  kendo.ui.progress($(".main-body-wrapper"), false);
                    if (DataUpdateWindow) {
                        DataUpdateWindow.title("Error Information");
                        DataUpdateWindow.content("<div class='msgboxError'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i>" + data.ModelError + " </div>");
                        DataUpdateWindow.open();
                    }
                }
            });
        }
    },

    InitCopyProject: function () {
        var Confirm = confirm("Are you sure you want to copy the selected project?");

        if (!Confirm) {
            return;
        }
        CopySave = 2;
        ShowContractReportPrompt();
    },
    
    copyProject: function (optMarkNTRForReportedContracts) {

        var Rad = $("input[name='RadProject']:checked");
        var ProjectId = Rad.siblings().filter("input[name='HdnProject']").val();
        var ContractId = ContractId = $("#redcon").val();

        if (parseInt(ProjectId) > 0) {
            // kendo.ui.progress(".main-body-wrapper", false);
            var postData = { ProjectId: ProjectId, ContractId: ContractId, MarkNTRForReportedContracts: optMarkNTRForReportedContracts }; //done
            $.ajax({ //Process the form using $.ajax()
                type: 'POST', //Method type
                url: AjaxCallUrl.CopyProject,
                data: postData, //Forms name
                dataType: 'json',
                // traditional: true,
                beforeSend: function () {
                },
                success: function (data) {
                    if (data.IsSuccess) {
                        //  kendo.ui.progress($(".main-body-wrapper"), false);
                        var DataUpdateWindow = $("#DataUpdateMessage").data("kendoWindow");
                        if (DataUpdateWindow) {
                            DataUpdateWindow.title("Update Information");
                            DataUpdateWindow.content("<div class='msgboxSuccess'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i> Data Update Successfully</div>");
                            DataUpdateWindow.open();
                            //redirect to listing page

                            var KendoListView = $("#listViewProject").data("kendoListView");
                            KendoListView.dataSource.read();
                        }
                    }
                },
                error: function (request, error) {
                    //  kendo.ui.progress($(".main-body-wrapper"), false);
                    var DataUpdateWindow = $("#DataUpdateMessage").data("kendoWindow");
                    if (DataUpdateWindow) {
                        DataUpdateWindow.title("Error Information");
                        DataUpdateWindow.content("<div class='msgboxError'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i>" + data.ModelError + " </div>");
                        DataUpdateWindow.open();
                    }
                }
            });
        }
    },

    PopulateTable: function () {
        $("#project_table tbody tr.member").hide();

        var tbl = $('#project_table').children('tbody');

        //Then if no tbody just select your table 
        // var tbl = tbody.length ? tbody : $('#project_table');
        $('#addMoreMangPro').modal('hide');

        var tbl = document.getElementById('project_table');
        var lastRow = tbl.rows.length;
        var LastHmtl = tbl.rows[lastRow - 1].cells[0].innerText;
        var Lengthcount = TryParseInt(LastHmtl, 0);
        if (Lengthcount == 0) {
            LastHmtl = tbl.rows[lastRow - 2].cells[0].innerText;
            Lengthcount = TryParseInt(LastHmtl, 0);
        }
        //var id = $('#project_table').find('tr:last td:first').text();
         var i = Lengthcount +1;
        var count = $("#rowcount").val();
        // added on 16-april-2018 - angshuman
        if ($(tbl.rows[1]).hasClass("member") && Lengthcount > 0) {
            i = i - 1;
            Lengthcount = Lengthcount - 1;
        }
        // added on 16-april-2018 - angshuman
        
        var RowCounter = ((i - 1) + parseInt(count));
        if (RowCounter <= 0)
        {
            RowCounter = 1;
        }
        for (j = 1; j <= count; j++) {
            var iteration = Lengthcount - 1;
            var row = tbl.insertRow(lastRow);

            var attCounterRow = document.createAttribute("data-counter");
            attCounterRow.value = i;
            row.setAttributeNode(attCounterRow);

            var CounterCell = row.insertCell(0);
            CounterCell.innerText = RowCounter;

            var secondCell = row.insertCell(1);

            var img = document.createElement('img');
            img.src = ImagePath;

            secondCell.appendChild(img);

            //================= Contract Logo ============
            //var secondCellLogo = row.insertCell(2);

            //var img = document.createElement('img');
            //img.src = ContractLogoPath;

            //secondCellLogo.appendChild(img);
            //============================================

            var ThirdCell = row.insertCell(2);
            var e2 = document.createElement('input');

            e2.type = 'text';
            e2.name = 'projectname_' + i;
            e2.id = 'projectname_' + i;
            e2.size = 20;
            e2.maxlength = 20;

            var attCounter = document.createAttribute("data-counter");
            attCounter.value = i;
            e2.setAttributeNode(attCounter);

            if (j == count) {
                e2.className = "first-proj projectname new-project form-control";
            } else {
                e2.className = "projectname new-project form-control";
            }

            
            ThirdCell.appendChild(e2);

            var spane2 = document.createElement('span');
            var att = document.createAttribute("data-for");        // Create a "href" attribute
            att.value = "projectname_" + i;
            spane2.setAttributeNode(att);
            // spane2.data_for = 'projectname_' + i;
            spane2.className = "k-invalid-msg";
            ThirdCell.appendChild(spane2);

            var ForthCell = row.insertCell(3);
            var e3 = document.createElement('input');

            e3.type = 'text';
            e3.name = 'lotno_' + i;
            e3.id = 'lotno_' + i;
            e3.size = 20;
            e3.maxlength = 20;
            e3.className = "lotno form-control";

            var attCounterLotNo = document.createAttribute("data-counter");
            attCounterLotNo.value = i;
            e3.setAttributeNode(attCounterLotNo);
            ForthCell.appendChild(e3);

            var spane3 = document.createElement('span');
            var att = document.createAttribute("data-for");        // Create a "href" attribute
            att.value = "lotno_" + i;
            spane3.setAttributeNode(att);
            // spane2.data_for = 'projectname_' + i;
            spane3.className = "k-invalid-msg";
            ForthCell.appendChild(spane3);

            //====================== ADDRESS ===========================
            var AddressCell = row.insertCell(4);
            var e6 = document.createElement('input');

            e6.type = 'text';
            e6.name = 'address_' + i;
            e6.id = 'address_' + i;
            e6.size = 50;
            e6.maxlength = 80;
            e6.className = "address form-control";

            var attCounterAddr = document.createAttribute("data-counter");
            attCounterAddr.value = i;
            e6.setAttributeNode(attCounterAddr);

            AddressCell.appendChild(e6);

            var spane6 = document.createElement('span');
            var att = document.createAttribute("data-for");        // Create a "href" attribute
            att.value = "address_" + i;
            spane6.setAttributeNode(att);
            // spane2.data_for = 'projectname_' + i;
            spane6.className = "k-invalid-msg";
            AddressCell.appendChild(spane6);
            //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

            //====================== CITY ===========================
            var CityCell = row.insertCell(5);
            var e5 = document.createElement('input');

            e5.type = 'text';
            e5.name = 'Txt_City' + i;
            e5.id = 'Txt_City' + i;
            e5.className = "city form-control";

            var attCounterCity = document.createAttribute("data-counter");
            attCounterCity.value = i;
            e5.setAttributeNode(attCounterCity);

            CityCell.appendChild(e5);
            //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

            //====================== STATE ===========================
            var StateCell = row.insertCell(6);
            var e4 = document.createElement('select');

            e4.type = 'select-one';
            e4.name = 'state_' + i;
            e4.id = 'state_' + i;
            var v = e4.id;

            e4.className = "State form-control";

            StateCell.appendChild(e4);
            e4.onchange = function () {
                FillCity($(this).val(), this.id);
            };
            //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

            //====================== ZIP ===========================
            var ZipCell = row.insertCell(7);
            var e7 = document.createElement('input');

            e7.type = 'text';
            e7.name = 'Txt_Zip' + i;
            e7.id = 'Txt_Zip' + i;
            e7.className = "zip form-control";

            var attCounterZip = document.createAttribute("data-counter");
            attCounterZip.value = i;
            e7.setAttributeNode(attCounterZip);

            ZipCell.appendChild(e7);
            //++++++++++++++++++++++++++++++++++++++++++++++++++++++

            var LastCell = row.insertCell(8);

            var elHidden = document.createElement('input');

            elHidden.type = 'hidden';
            elHidden.name = 'HdnProject';
            elHidden.value = 0;

            LastCell.appendChild(elHidden);

            var el = document.createElement('input');
            el.type = 'radio';
            el.name = 'checkcoun_' + i;
            el.id = 'checkcoun_' + i;

            LastCell.appendChild(el);
            i++;

            RowCounter--;
        }

        $("#project_table tr").each(function (e) {
            if (typeof ($(this).find('.State').val()) != "undefined") {
                GetState($(this).find('.State'));
            }
            if (typeof ($(this).find('.City').val()) != "undefined") {
                GetCity($(this).find('.City'));
            }
        });

        $(".first-proj").focus();
    },

    CancelProject: function () {
        //console.log('fff');
        var redirectstatus = $('#redirect').val();
        if (redirectstatus == "addstatus")  //Active Contrcat
        {
            window.location.href = AjaxCallUrl.RedirectAddprojectStatus.replace("****ContractId****", $("#redcon").val());
            // window.location.href = AjaxCallUrl.RedirectAddprojectStatus;
        }
        else if (redirectstatus == "report") {
            window.location.href = AjaxCallUrl.RedirectSubmitReport.replace("****ContractId****", $("#redcon").val());
            // window.location.href = AjaxCallUrl.RedirectSubmitReport;
        }
        else {
            window.location.href = AjaxCallUrl.RegularReportingUrl;
        }
    }
    
}

function GetState(a) {
    $.ajax({
        type: "POST",
        url: AjaxCallUrl.GetStateUrl,
        data: '{}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (Statelist) {
            for (var k = 0; k < Statelist.length; k++) {
                $(a).append($("<option></option>").val(Statelist[k].statename).html(Statelist[k].statename));
            }
        }
    });
}

function GetCity(a) {
    $.ajax({
        type: "POST",
        url: AjaxCallUrl.GetCityUrl,
        data: '{}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (Statelist) {
            for (var k = 0; k < Statelist.length; k++) {
                $(a).append($("<option></option>").val(Statelist[k].cityname).html(Statelist[k].cityname));
            }
        }
    });
}

$(document).on('click', '#btnSave', function () {
    CopySave = 1;
    ShowContractReportPrompt();
});

function ShowContractReportPrompt() {
    if (CopySave == 1) {
        var CurrentTableRows = 0;
        var ExistingRows = ExistingProjectsCount;

        $("#project_table tr").each(function (e) {
            if ($(this).hasClass('tbody-row')) {
                if ($(this).find('#spnRowNumber123').length == 1) {
                    CurrentTableRows++;
                }
            } else {
                if ($(this).find('input.new-project').length == 1) {
                    CurrentTableRows++;
                }
            }
        });

        if (CurrentTableRows > ExistingRows) {
            if ($("#hdnReportedContracts").val() != "") {
                $('#divConfirmReportedProjectStatus').modal('show');
            } else {
                SaveProject('false');
            }
        } else {
            SaveProject('false');
        }
    } else {
        //----------- For Copy Project ------------------------
        if ($("#hdnReportedContracts").val() != "") {
            $('#divConfirmReportedProjectStatus').modal('show');
        } else {
            Project.copyProject('false');
        }
    }
}

function SaveProject(optMarkNTRForReportedContracts) {

    $('#divConfirmReportedProjectStatus').modal('hide');
    if (CopySave == 2) {
        Project.copyProject('true');
        return;
    }

    $('#btnSave').addClass('disabled');
    if (flgSaveClicked == true) {
        kendo.ui.progress($("#addproject"), false);
        return;
    }

    flgSaveClicked = true;
    kendo.ui.progress($("#addproject"), true);

    var IsValid = IsAddProjectValid();

    if (!IsValid) {
        kendo.ui.progress($("#addproject"), false);
        $('#btnSave').removeClass('disabled');
        flgSaveClicked = false;
        return;
    }

    if ($('.member').length) {
        $("#project_table tbody tr.member").remove();
    }

    var redirectstatus = $('#redirect').val();

    var HTMLtbl =
        {
            getData: function (table) {
                var data = [];
                table.find('tr').not(':first').each(function (rowIndex, r) {
                    var ProjectCounter = $(this).data("counter");

                    var cols = [];
                    var flgAddColsToData = true;

                    var ctrlProjectName = '#projectname_' + ProjectCounter;
                    if ($(ctrlProjectName).val() == "") {
                        flgAddColsToData = false;
                    }

                    if (flgAddColsToData) {
                        $(this).find('td').each(function (colIndex, c) {
                            if ($(this).children(':text,:hidden,textarea').length > 0) {
                                cols.push($(this).children('input,select,textarea,radio').val());
                            } else if ($(this).find('select').length > 0) {
                                cols.push($(this).find(':selected').text());
                            } else if ($(this).children(':checkbox').length > 0) {
                                cols.push($(this).children(':checkbox').is(':checked') ? 1 : 0);
                            } else {
                                cols.push($(this).text().trim());
                            }
                        });
                        data.push(cols);
                    }
                });

                return data;
            }
        }
    kendo.ui.progress($("#addproject"), true);
    var emptiproject = $('.projectname').filter(function () {
        return $.trim($(this).val()) == '';
    });
    var emptilotno = $('.lotno').filter(function () {
        return $.trim($(this).val()) == '';
    });
    var emptiaddress = $('.address').filter(function () {
        return $.trim($(this).val()) == '';
    });

    var data = HTMLtbl.getData($('#project_table'));  // passing that table's ID //
    var parameters = {};
    parameters.array = data;
    parameters.ContractId = $("#redcon").val();
    parameters.MarkNTRForReportedContracts = optMarkNTRForReportedContracts;
    //console.log(data.length);

    if (data.length == 0) {
        kendo.ui.progress($("#addproject"), false);
        $('#btnSave').removeClass('disabled');
        return;
    }

    var projectdata = JSON.stringify(parameters);

    var request = $.ajax({
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: AjaxCallUrl.SaveProjectData,
        data: projectdata,
        success: function (data) {
            $('#btnSave').removeClass('disabled');
            //flgSaveClicked = false;
            //alert(JSON.stringify(parameters));
            var DataUpdateWindow = $("#DataUpdateMessage").data("kendoWindow");
            if (data == true) {
                kendo.ui.progress($("#addproject"), false);

                if (DataUpdateWindow) {

                    DataUpdateWindow.title("Insert Information");
                    DataUpdateWindow.content("<div class='msgboxSuccess'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i> Project added Successfully</div>");
                    DataUpdateWindow.open();
                    //redirect to listing page
                    setTimeout(function () {
                        if (redirectstatus == "addstatus")  //Active Contrcat
                        {
                            window.location.href = AjaxCallUrl.RedirectAddprojectStatus.replace("****ContractId****", $("#redcon").val());
                            // window.location.href = AjaxCallUrl.RedirectAddprojectStatus;
                        }
                        else if (redirectstatus == "report") {
                            window.location.href = AjaxCallUrl.RedirectSubmitReport.replace("****ContractId****", $("#redcon").val());
                            // window.location.href = AjaxCallUrl.RedirectSubmitReport;
                        }
                        else {
                            window.location.href = AjaxCallUrl.RegularReportingUrl
                        }
                    }, 1500);

                }
            }
            else {
                kendo.ui.progress($("#addproject"), false);
                if (DataUpdateWindow) {
                    DataUpdateWindow.title("Error Information");
                    DataUpdateWindow.content("<div class='msgboxError'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i>You did not add any new project </div>");
                    DataUpdateWindow.open();
                    $('#btnSave').removeClass('disabled');
                }
            }
        },
        error: function (err) {
            $('#btnSave').removeClass('disabled');
        }
    });
}

function IsAddProjectValid() {

    var Validator = $("#project_table").kendoValidator({
        messages: {
            required: "*",
            email: function (input) {
                return "Please enter correct email"
            },
            numberrange: function (input) {
                return 'Please enter number between ' + MaxMin.Min + ' and ' + MaxMin.Max + ' '
            },
            allowedcharecter: function (input) {
                if (AllowedCharecter.Alphabet == 1) {
                    return 'Alphabet is not allowed '
                }
                else if (AllowedCharecter.Number == 1) {
                    return 'Number is not allowed '
                }
                else if (AllowedCharecter.SpecialCharecter == 1) {
                    return 'Special charecter is not allowed '
                }
            },
        },
        rules: {
            required: function (input) {

                if (!input.is(":hidden")) {
                    if (input.hasClass("new-project")) {
                        return true;
                    } else {
                        var dataCounter = $(input).data("counter");
                        var ctrlProjectName = "#projectname_" + dataCounter;

                        if ($(ctrlProjectName).hasClass("new-project")) {
                            if ($(ctrlProjectName).val().trim() == "") {
                                return true;
                            } else {
                                if ($(input).val().trim() == "")
                                    return false;
                                else
                                    return true;
                            }
                        } else {
                            if ($(input).val().trim() == "")
                                return false;
                            else
                                return true;
                        }
                    }
                }
                return true;
            }
        }
    }).data("kendoValidator");
    var IsValid = Validator.validate();
    return IsValid;
}

function PopulateCityByState(id) {

    var tr = $(id).closest('tr');
    var city = $(tr).find('.City');

    $.ajax({
        url: AjaxCallUrl.GetCityBindUrl,
        type: "GET",
        dataType: "JSON",
        data: { State: id },
        success: function (cities) {

            $(city).empty();//(""); // clear before appending new list 
            $.each(cities, function (i, city) {
                $(city).append($('<option></option>').val(city.cityid).html(city.cityname));
            });
        }
    });
}
function FillCityWithState(stateid, cityid) {
    var val = '#' + stateid;
    var State = $(val).val();

    var id = '#' + cityid;

    $.ajax({
        url: AjaxCallUrl.GetCityBindUrlByState,
        type: "GET",
        dataType: "JSON",
        data: { StateId: State },
        success: function (cities) {

            $(id).html(""); // clear before appending new list 
            $.each(cities, function (i, city) {
                $(id).append(
                    $('<option></option>').val(city.cityid).html(city.cityname));
            });
        }
    });
}
function renderDDLCities(stateid, cityid, citydropdown) {

    var id = '#' + citydropdown;

    $.ajax({
        url: AjaxCallUrl.GetCityBindUrl,
        type: "GET",
        dataType: "JSON",
        data: { State: stateid },
        success: function (cities) {

            $(id).html(""); // clear before appending new list 
            $.each(cities, function (i, city) {
                $(id).append(
                    $('<option></option>').val(city.cityid).html(city.cityname));
                $(id).val(cityid);
            });
        }
    });
}
function FillCity(State, citydrop) {



    citydrop1 = citydrop.replace("state", "city");
    var id = '#' + citydrop1;

    $.ajax({
        url: AjaxCallUrl.GetCityBindUrl,
        type: "GET",
        dataType: "JSON",
        data: { State: State },
        success: function (cities) {

            $(id).html(""); // clear before appending new list 
            $.each(cities, function (i, city) {
                $(id).append(
                    //$('<option></option>').val(city.cityid).html(city.cityname));
                    $('<option></option>').val(city.cityname).html(city.cityname));
            });
        }
    });
}
function SendContractIdAsParameterOrder() {

    return { Type: type, Flag: flag, PageValue: $("#HdnResourcePageValue").val() };

}

function renderDDLStates(data, stateid) {

    $.ajax({
        type: "POST",
        url: AjaxCallUrl.GetStateUrl,
        data: '{}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (Statelist) {
            var id = '#' + stateid;
            for (var k = 0; k < Statelist.length; k++) {
                $(id).append($("<option></option>").val(Statelist[k].stateid).html(Statelist[k].statename));
                //$(id).val(data);
                $(id).val(data);
                //$('#stateid option[value=' + data + ']').attr("selected", "selected");
            }
        }
    });
}
function SubmitReport() {

    $.ajax({
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        url: AjaxCallUrl.ChangeQuaterAdminReport,
        //data: "{ qauterid: " + qtrid + " }",
        success: function (res) {
            var DataUpdateWindow = $("#DataUpdateMessage").data("kendoWindow");
            if (res == true) {


                kendo.ui.progress($("#CreateContract"), false);

                if (DataUpdateWindow) {

                    DataUpdateWindow.title("Update Information");
                    DataUpdateWindow.content("<div class='msgboxSuccess'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i> Data Update Successfully</div>");
                    DataUpdateWindow.open();
                    //redirect to listing page
                    setTimeout(function () {
                        window.location.href = AjaxCallUrl.SuccessReport;

                    }, 1500);

                }
            }
            else {
                kendo.ui.progress($("#CreateContract"), false);
                if (DataUpdateWindow) {
                    DataUpdateWindow.title("Error Information");
                    DataUpdateWindow.content("<div class='msgboxError'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i>" + data.ModelError + " </div>");
                    DataUpdateWindow.open();
                }
            }
        }




    });
}

$("#submitreport").on("click", function () {
    var wnd = $("#ManageReport").data("kendoWindow");
    wnd.refresh({
        url: AjaxCallUrl.LoadManageReport,
    });
    wnd.open().center();
});

$("#cancelall").on("click", function () {
    var wnd = $("#ManageProjectPopUp").data("kendoWindow");
    wnd.close();
});

function OnOpenManageStatus(e) {
    var wnd = $("#ManageReport").data("kendoWindow");
    kendo.ui.progress(wnd.element, true);
}

function OnDataBound(e) {
    setTimeout(ShowReportSubmitPrompt, 1000);
}

function ShowReportSubmitPrompt() {
    if (parseInt(ReportSubmitted) != 1 && parseInt(ReportedContracts) > 0 && ReportedContracts == TotalContracts) {
        alert("Congratulations! You have reported for all of your enrolled contracts. If you’re finished, you must click the SUBMIT NCP QUARTERLY REBATE REPORT button at the bottom of the page. Until you do that, you can still make changes if necessary.");
    }
}

function OnRefreshManageStatus(e) {
    var wnd = $("#ManageReport").data("kendoWindow");
    kendo.ui.progress(wnd.element, false);
    AssignEvent();
}
var AssignEvent = function () {
    $("#confirmreport").on("click", function () {
        SubmitReport();
    });

    $("#cancelreport").on("click", function () {
        var wnd = $("#ManageReport").data("kendoWindow");
        wnd.close();
    });
};
function tablerowexist() {
    if ($('.table > tbody > tr').length == 0) {
        var newRow = $("<tr class='member'><td colspan='5'><b>There are no projects yet. Please click the ‘Add Project’ link to Add new projects</b></td></tr>");
        $(".table").append(newRow);
    }
    else {
        var RowCounter = 1;
        $("#project_table tr").each(function (e) {
            if ($(this).hasClass('tbody-row')) {
                if ($(this).find('#spnRowNumber123').length == 1) {
                    ExistingProjectsCount++;
                }
                $(this).find('#spnRowNumber').text(RowCounter);
                RowCounter++;
            }
        });
    }
}

function tablerowexistforcloseprj() {
    if ($('.table > tbody > tr').length == 0) {
        var newRow = $("<tr class='member'><td colspan='5'><b>There are no Closed Projects as of now</b></td></tr>");
        $(".table").append(newRow);
    }
    else {
        var RowCounter = 1;
        $("#project_table tr").each(function (e) {
            if ($(this).hasClass('tbody-row')) {
                var spnRowNumCount = $(this).find('#spnRowNumber').length;
                if (spnRowNumCount == 1) {
                    $(this).find('#spnRowNumber').text(RowCounter);
                    RowCounter++;
                }                
            }
        });
    }
}

function CancelModalPopup() {

    $('#addMoreMangPro').modal('hide');
}

function GetReportingStatusForBuilder() {
    //Ajax call to fetch reporting status
    $.ajax({
        url: AjaxCallUrl.GetNCPReportStatusCount,
        type: "GET",
        data: '{}',
        contentType: "application/json; charset=utf-8",
        //dataType: "json",
        success: function (ReportStatus) {
            var ReportingStatus, IncompleteReports;
            ReportingStatus = ReportStatus;

            var arrReportingStatus = ReportingStatus.split("/");
            ReportSubmitted = arrReportingStatus[0];
            ReportedContracts = arrReportingStatus[1];
            TotalContracts = arrReportingStatus[2];
            IncompleteReports = arrReportingStatus[3];

            PopulateRebateReportProgressBar(ReportSubmitted, TotalContracts, ReportedContracts, IncompleteReports);
        },
        error: function (err) {
            //alert(err);
        }
    });
}

function PopulateRebateReportProgressBar(ReportSubmitted, TotalContracts, ReportedContracts, IncompleteReports) {

    var ReportPercentage = Math.ceil((parseInt(ReportedContracts) / parseInt(TotalContracts)) * 100);
    var ColorCode, BorderColor;
    var ProgressStatus;

    ProgressStatus = ReportedContracts + " of " + TotalContracts;
    if (parseInt(ReportSubmitted) == 1) {
        BorderColor = "#7AE48D"; //GREEN FILL
        ColorCode = "#7AE48D";
        ProgressStatus = "Report Submitted";
    } else if (parseInt(ReportedContracts) > 0 && ReportedContracts == TotalContracts) {
        ColorCode = "#8793F9";  //BLUE FILL
        BorderColor = "#8793F9";
        ProgressStatus = "Please Submit Your Report";
    } else if (parseInt(ReportedContracts) == 0) {
        if (IncompleteReports > 0)
            ReportPercentage = 5;
        else
            ReportPercentage = 0.25;
        BorderColor = "#FA5E51"; //RED BORDER, NO FILL
        ColorCode = "#FA5E51";
    } else if (parseInt(ReportedContracts) == 0 && parseInt(IncompleteReports) > 0) {
        ColorCode = "#FA5E51";  //RED FILL
        BorderColor = "#FA5E51";
    } else if (ReportPercentage > 0 && ReportPercentage <= 50) {
        ColorCode = "#F78B09";  //ORANGE FILL
        BorderColor = "#F78B09";
    } else if (ReportPercentage > 50) {
        ColorCode = "#FDFB68";  //YELLOW FILL
        BorderColor = "#FDFB68";
    } else {
        //alert("else");
    }

    //Populate Progress Bar
    passProgress = $("#prgbarReportStatus").kendoProgressBar({
        type: "value",
        max: 100,
        animation: false,
        value: ReportPercentage
    }).data("kendoProgressBar");
    if (passProgress != null)
    {
        passProgress.progressWrapper.css({
            "background-color": ColorCode,
            "border-color": BorderColor
        });
        passProgress.progressWrapper.parent().css({
            "border-color": BorderColor
        });
        passProgress.progressStatus.text(ProgressStatus);
    }    
}

function TryParseInt(str, defaultValue) {
    var retValue = defaultValue;
    if (str !== null) {
        if (str.length > 0) {
            if (!isNaN(str)) {
                retValue = parseInt(str);
            }
        }
    }
    return retValue;
}