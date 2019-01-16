var type;
var flag;
var FilterProjectId = 0;
var PrevProjList;

$(document).ready(function () {

    ManageProjectList.AssignControl();

    var KendoListView = $("#listViewActiveProject").data("kendoListView");
    $('thead').first().css('display', 'block');
    $('thead').first().css('width', '100%');
    $('tbody').first().css('display', 'block');
    $('tbody').first().css('height', '400px');
    $('tbody').first().css('width', '100%');

    if ($("#HdnIsSurveyPublish").val() == "0") {
        var DataUpdateWindow = $("#DataUpdateMessage").data("kendoWindow");
        if (DataUpdateWindow) {
            DataUpdateWindow.title("Error Information");
            DataUpdateWindow.content("<div class='msgboxError'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i><ul><li>Ncp survey not yet configure</li></ul></div>");
            DataUpdateWindow.open();
        }
    }

    $("#prevreportedProj").on("click", function () {
        var wnd = $("#PrevRptProj").data("kendoWindow");
        wnd.refresh({
            url: AjaxCallUrl.PrevRprPrjUrl, // returns JSON, { firstName: "Alyx", lastName: "Vance" }
            // data: { ResourceId: dataValue.ResourceId },
        });
        wnd.open().center();
    });

    //var KendoListViewPrj = $("#listViewPrevRptProject").data("kendoListView");
    //$('thead').first().css('display', 'inline-block');
    //$('thead').first().css('width', '1170');
    //$('tbody').first().css('display', 'inline-block');
    //$('tbody').first().css('height', '400px');
    //$('tbody').first().css('width', '1170');

});

ManageProjectList = {

    AssignControl: function () {

        //$(".nav-list").on("click", ManageProjectList.SelectProject);
        $("#btnSave").on("click", ManageProjectList.SaveProject);
        $("#btnCancel").on("click", ManageProjectList.Cancel_Click);

        $("#addproj").on("click", ManageProjectList.redirectaddproject);

    },
    redirectaddproject: function () {
        var url = AjaxCallUrl.AddProjectRedirectUrl;
        window.location.href = url;


    },
    SaveProject: function () {
        $("#btnSave").prop("disabled", true);
        $("#btnCancel").prop("disabled", true);
        $("#changeStatus").prop("disabled", true);
        $("#addproj").prop("disabled", true);

        kendo.ui.progress($("#AddProjectStatus"), true);
        var project = [];
        //var ObjContractName = '';
        //var ObjQuaterName = '';
        //var ObjYear = '';
        //var ObjContractIcon = [];
        //var ObjManuFacturerName = '';

        var Flag = 0;
        $("input[name^='ProjectStatus_']:checked").each(function () {

            var ProjectId = parseInt($(this).parent().siblings().children("input[name='HdnProject']").val());
            var ProjetStatusId = parseInt($(this).val());

            if (ProjetStatusId == "1")  ///check if there is only one report unit for this quater select or not
            {
                Flag = 1;
            }
           // project.push({ ContractId: $("#HdnContractId").val(), BuilderId: $("#HdnBuilderId").val(), QuaterId: $("#HdnQuaterId").val(), ProjectId: ProjectId, ProjectStatusId: ProjetStatusId, ContractName: ObjContractName, QuaterName: ObjQuaterName, Year: ObjYear, ContractIcon: ObjContractIcon, ManuFacturerName: ObjManuFacturerName });

            //project.push({ ContractId: 33, BuilderId: 24161, QuaterId: 9, ProjectId: 3913, ProjectStatusId: 2, ContractName: '', QuaterName: '', Year: '', ContractIcon: [], ManuFacturerName: '' });
            //project.push({ ContractId: parseInt($("#HdnContractId").val()), BuilderId: parseInt($("#HdnBuilderId").val()), QuaterId: parseInt($("#HdnQuaterId").val()), ProjectId: ProjectId, ProjectStatusId: ProjetStatusId, ContractName: ObjContractName, QuaterName: ObjQuaterName, Year: ObjYear, ContractIcon: ObjContractIcon, ManuFacturerName: ObjManuFacturerName });
            project.push([$("#HdnContractId").val(),  $("#HdnBuilderId").val(),  $("#HdnQuaterId").val(),  ProjectId.toString(),  ProjetStatusId.toString() ]);
        });

        if (Flag == 0) {

            var UsreConfirmation = confirm("Are you sure you don’t want to report for the selected projects?");

            if (UsreConfirmation == false) {
                $("#btnSave").prop("disabled", false);
                $("#btnCancel").prop("disabled", false);
                $("#changeStatus").prop("disabled", false);
                $("#addproj").prop("disabled", false);

                kendo.ui.progress($("#AddProjectStatus"), false);

                return false;
            }
        }

        var parameters = {};
        parameters.array = project;

        var projectdata = JSON.stringify(parameters);
        //var postData = { ObjVm: project };

        $.ajax({ 
            dataType: 'json',
            type: 'POST', 
            contentType: "application/json; charset=utf-8",
            url: AjaxCallUrl.SaveProjectData,
            //data: postData, //Forms name
            data: projectdata,            
            // traditional: true,
            beforeSend: function () {
            },
            success: function (data) {
                $("#btnSave").prop("disabled", false);
                $("#btnCancel").prop("disabled", false);
                $("#changeStatus").prop("disabled", false);
                $("#addproj").prop("disabled", false);

                var DataUpdateWindow = $("#DataUpdateMessage").data("kendoWindow");
                if (data.IsSuccess) {
                    kendo.ui.progress($("#AddProjectStatus"), false);


                    //  ContractBuilder.LoadContractBuilder();
                    if (DataUpdateWindow) {
                        DataUpdateWindow.title("Update Information");
                        DataUpdateWindow.content("<div class='msgboxSuccess'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i> Data Update Successfully</div>");
                        DataUpdateWindow.open();

                        if (Flag == 0)  //select no project to report for this quater
                        {
                            setTimeout(function () {
                                window.location.href = AjaxCallUrl.CancelUrl;
                            }, 800)
                        }
                        else {
                            //redirect to listing page
                            setTimeout(function () {
                                window.location.href = AjaxCallUrl.SubmitReportUrl.replace("****ContractId****", $("#HdnContractId").val());
                            }, 800)
                        }


                    }

                }
                else {
                    kendo.ui.progress($("#AddProjectStatus"), false);
                    if (DataUpdateWindow) {
                        DataUpdateWindow.title("Error Information");
                        DataUpdateWindow.content("<div class='msgboxError'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i>" + data.ModelError + " </div>");
                        DataUpdateWindow.open();
                    }
                }
            },
            error: function (projectData) {
                $("#btnSave").prop("disabled", false);
                $("#btnCancel").prop("disabled", false);
                $("#changeStatus").prop("disabled", false);
                $("#addproj").prop("disabled", false);

                var DataUpdateWindow = $("#DataUpdateMessage").data("kendoWindow");
                kendo.ui.progress($("#AddProjectStatus"), false);
                if (DataUpdateWindow) {
                    DataUpdateWindow.title("Error Information");
                    DataUpdateWindow.content("<div class='msgboxError'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i>" + "System Error!" + " </div>");
                    DataUpdateWindow.open();
                }
            }
        });



    },
    SelectProject: function () {


        $('.nav-list li.ProStatusActive').removeClass('ProStatusActive');

        $(this).addClass('ProStatusActive');

    },
    Cancel_Click: function () {
        $("#btnSave").prop("disabled", true);
        $("#btnCancel").prop("disabled", true);
        $("#changeStatus").prop("disabled", true);
        $("#addproj").prop("disabled", true);

        window.location.href = AjaxCallUrl.CancelUrl;
    }
}


function onSelectProjectStatus(e) {

    var typead = this.dataItem(e.item.index());
    FilterProjectId = typead.ProjectId;
    PrevProjList = null;

    //if (flag == '0') {
    //    flag = null;
    //}
    //type = null;

    var KendoListView = $("#listViewActiveProject").data("kendoListView");
    KendoListView.dataSource.read();
}



function myFunction1(projectid) {

    var list = "#" + statusid
    $('.nav-list li.active').removeClass('ProStatusActive');
    $(this).addClass('ProStatusActive');
}
$("#changeStatus").on("click", function () {


    var wnd = $("#ProjectStatus").data("kendoWindow");
    wnd.refresh({
        url: AjaxCallUrl.ProjectStatusUrl, // returns JSON, { firstName: "Alyx", lastName: "Vance" }
        // data: { ResourceId: dataValue.ResourceId },
        data: { ContractResourceId: 0 }
    });
    wnd.open().center();

});

function OnOpenContactResource(e) {
    var wnd = $("#ProjectStatus").data("kendoWindow");
    kendo.ui.progress(wnd.element, true);
}
function OnRefreshContractResource(e) {
    var wnd = $("#ProjectStatus").data("kendoWindow");
    kendo.ui.progress(wnd.element, false);
    // AssignEventContractResource();
    AssignEventForStatus();

}
//$("#ckbCheckAll").click(function () {
//    alert('hi');
//    $(".checkBoxClass").prop('checked', $(this).prop('checked'));
//});
$('#ckbCheckAll').click(function () {
    if ($(this).prop("checked")) {

        $(".checkBoxClass").prop("checked", true);
    } else {

        $(".checkBoxClass").prop("checked", false);
    }
});
function active(el) {

    //alert($(el).attr("class"));
    // alert('hi');
    var ul = $(el).closest('ul');

    $(ul).find('li').each(function () {

        $(this).removeClass('ProStatusActive');
        // alert('hi');
    });
    $(el).addClass('ProStatusActive');
    //alert($(el).attr('Id'));
    var td = $(el).closest('td');
    $(td).find("input[name='HdnStatus']").val($(el).attr('Id'));


    // $("#HdnStatus").val(statusid);

}

$("#asccon").click(function () {
    $('#descicon').show();
    $('#ascicon').hide();
    var UserDetails = asccon;
    type = 'asccon';
    flag = null;
    var KendoListView = $("#listViewActiveProject").data("kendoListView");
    KendoListView.dataSource.read();
});
$("#desccon").click(function () {
    $('#descicon').hide();
    $('#ascicon').show();
    var UserDetails = asccon;
    type = 'desccon';
    flag = null
    var KendoListView = $("#listViewActiveProject").data("kendoListView");
    KendoListView.dataSource.read();
});
function SendContractIdAsParameterOrder() {

    return { Type: type, Flag: flag };

}
$("#btnClose").on("click", function () {
    // console.log('cancel');

    var wnd = $("#ProjectStatus").data("kendoWindow");
    wnd.close();

});

function SendBuilderIdAsParameter() {
    return { BuilderId: $("#HdnBuilderId").val() };
}

function SendParameterToLoadGrid() {
    var SortType = '';

    if ($("#ascicon").is(":visible")) {
        SortType = "ASC";
    }
    else {
        SortType = "DESC";
    }

    return { BuilderId: $("#HdnBuilderId").val(), ContractId: $("#HdnContractId").val(), FilterProjectId: FilterProjectId, SortType: SortType, ObjPrevProjList: PrevProjList };
}


function AssignEventForStatus() {

    $("#btnConfirm").on("click", function () {
        var status = $('#projectstatus').val();
        //  console.log(status);
        $("input[name='ChkProject']:checked").each(function () {

            $(this).parent().siblings().children("input[name^='ProjectStatus_']").removeAttr('checked');
            // console.log('ggg');
            var RadioButton = $(this).parent().siblings().children("input[name^='ProjectStatus_']").each(function () {
                var Value = $(this).val();
                if (Value == status) {
                    //  console.log('ddd');
                    //  console.log(Value);
                    $(this).prop("checked", true);
                    // $(this).attr('checked', 'checked');
                }

            });

        });


        $('.table tbody tr').find('[type="checkbox"]').attr('checked', false);
        var wnd = $("#ProjectStatus").data("kendoWindow");
        wnd.close();
        //alert($('#projectstatus').val())
    });

    $("#btnClose").on("click", function () {

        var DataUpdateWindow = $("#ProjectStatus").data("kendoWindow");

        if (DataUpdateWindow) {
            DataUpdateWindow.close();
        }

    });


}

function SendParaToLoadPrevRptPrjGrid() {
    var SortType = '';

    //if ($("#ascicon").is(":visible")) {
        SortType = "ASC";
    //}
    //else {
    //    SortType = "DESC";
    //}

        return { BuilderId: $("#hdnReopenBuilderId").val(), ContractId: $("#hdnReopenContractId").val(), SortType: SortType };
}

function OnOpenPrevRptProj(e) {
    var wnd = $("#PrevRptProj").data("kendoWindow");
    kendo.ui.progress(wnd.element, true);
}


function OnRefreshPrevRptProj(e) {
    var wnd = $("#PrevRptProj").data("kendoWindow");
    kendo.ui.progress(wnd.element, false);
    AssignEventForPrevPrj();
}

function AssignEventForPrevPrj() {

    $("#ReopenPrevProj").on("click", function () {
              
        var projects = [];

        $("input[name='ChkPrevRptProject']:checked").each(function () {
            var ProjectId = $(this).parent().children("input[name='HdnReopenProject']").val();
            console.log(ProjectId);
            projects.push({ ContractId: $("#hdnReopenContractId").val(), BuilderId: $("#hdnReopenBuilderId").val(), QuaterId: $("#hdnReopenQuaterid").val(), ProjectId: ProjectId });
        });

        $('.table tbody tr').find('[type="checkbox"]').attr('checked', false);
        var wnd = $("#PrevRptProj").data("kendoWindow");
        wnd.close();

        PrevProjList= projects;

        var KendoListView = $("#listViewActiveProject").data("kendoListView");
        KendoListView.dataSource.read();

    });

    $("#CancelPrevProj").on("click", function () {
        var DataUpdateWindow = $("#PrevRptProj").data("kendoWindow");
        if (DataUpdateWindow) {
            DataUpdateWindow.close();
        }

    });

}

//$('#ckbCheckAllPrevProj').click(function () {
//    if ($(this).prop("checked")) {

//        $(".checkBoxClass").prop("checked", true);
//    } else {

//        $(".checkBoxClass").prop("checked", false);
//    }
//});