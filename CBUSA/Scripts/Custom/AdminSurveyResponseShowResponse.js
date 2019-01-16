var ResponseFilter = '';
var MarketFilter = '0';
var BuilderFilter = '0';
var QuestionFilter = '';
$(document).ready(function () {
    SurveyResponse.AssignControl();
    SurveyResponse.DeleteSurveyResult();
    SurveyResponse.EditSurvey();
    SurveyResponse.ChangeStatusSurveyResult();    

    var KendoListView = $("#QuestionAnswareList").data("kendoListView");
    $('thead').first().css('display', 'block');
    $('thead').first().css('width', '100%');
    $('tbody').first().css('display', 'block');
    $('tbody').first().css('height', '500px');
    $('tbody').first().css('width', '100%');
});

var SurveyResponse = {
    AssignControl: function () {
        $("#DivFilterQuestionDropdown").on("click", SurveyResponse.LoadQuestionCustomDropDown);
        $("#BtnDeleteSurveyRow").on("click", SurveyResponse.DeleteSurveyResult);
        $("#BtnEditSurveyRow").on("click", SurveyResponse.EditSurvey);
        $("#BtnChangeSurveyRow").on("click", SurveyResponse.ChangeStatusSurveyResult);
    },
    LoadQuestionCustomDropDown: function () {
        //QuestionId        
        CustomControlQuestion($(this).parent().parent(), "CustomControlFilterQuestion");
        GetSelectedQuestionList();
    },
    DeleteSurveyResult : function()
    {
        var Rad = $("input[name='RadSurvey']:checked");
        var SurveyId = Rad.siblings().filter("input[name='HdnSurvey']").val();
        var Archive = Rad.siblings().filter("input[name='HdnArchive']").val();
        var BuilderId = Rad.siblings().filter("input[name='HdnBuilder']").val();
        var contractId = Rad.siblings().filter("input[name='HdnContract']").val();
        var projectId = Rad.siblings().filter("input[name='HdnProject']").val();
        var quaterId = Rad.siblings().filter("input[name='HdnQuater']").val();
        var selectedBuilderArray = new Array();
        var Confirm = 'No';
       
        //if (parseInt(surveyId) > 0 && parseInt(builderId) > 0 && parseInt(projectId) == 0) {
        //    $("input:checkbox[name='RadSurvey']:checked").each(function () {
        //        selectedBuilderArray.push($(this).siblings().filter("input[name='HdnBuilder']").val());
        //    });
        //}
        //else
        //{
        //    $("input:checkbox[name='RadSurvey']:checked").each(function () {
        //        selectedBuilderArray.push($(this).siblings().filter("input[name='HdnProject']").val());
        //    });
        //}
        $("input:checkbox[name='RadSurvey']:checked").each(function () {
            selectedBuilderArray.push($(this).siblings().filter("input[name='HdnBuilder']").val());
        });
        if (selectedBuilderArray.length > 0)
        {
            if (confirm('Are you sure you want to delete survey response?')) {
                Confirm = 'Yes';
            } else {
                Confirm = 'No';
                return;
            }
        }
        else
        {
            return;
        }
        if (parseInt(SurveyId) > 0 && Confirm == 'Yes') {
            var urldata = AjaxCallUrl.DeleteSurveyResultUrl;
            $.ajax({ //Process the form using $.ajax()
                type: 'POST', //Method type
                url: urldata,
                data: { surveyid: SurveyId, BuilderIdList: selectedBuilderArray }, //, BuilderId: BuilderId
                dataType: 'json',
                beforeSend: function () {
                },
                error: function (request, error) {
                    // $(document).ajaxStop($.unblockUI);
                    // console.log('gggg');
                },
                success: function (data) {
                    if (data.IsSuccess) {
                        var KendoListView1 = $("#QuestionListww").data("kendoListView");
                        KendoListView1.dataSource.read();

                        var KendoListView = $("#QuestionAnswareList").data("kendoListView");
                        KendoListView.dataSource.read();
                    }
                    else {
                    }

                }
            });
        }
    },
    ChangeStatusSurveyResult: function ()
    {
        var Rad = $("input[name='RadSurvey']:checked");
        var SurveyId = Rad.siblings().filter("input[name='HdnSurvey']").val();
        var Archive = Rad.siblings().filter("input[name='HdnArchive']").val();
        var BuilderId = Rad.siblings().filter("input[name='HdnBuilder']").val();
        var contractId = Rad.siblings().filter("input[name='HdnContract']").val();
        var projectId = Rad.siblings().filter("input[name='HdnProject']").val();
        var quaterId = Rad.siblings().filter("input[name='HdnQuater']").val();
        var selectedBuilderArray = new Array();
        var selectedQuarter = '';
        var Confirm = 'No';
        
        
        //if (parseInt(surveyId) > 0 && parseInt(builderId) > 0 && parseInt(projectId) == 0) {
        //    $("input:checkbox[name='RadSurvey']:checked").each(function () {
        //        selectedBuilderArray.push($(this).siblings().filter("input[name='HdnBuilder']").val());
        //    });
        //}
        //else
        //{
        //    $("input:checkbox[name='RadSurvey']:checked").each(function () {
        //        selectedBuilderArray.push($(this).siblings().filter("input[name='HdnProject']").val());
        //    });
        //}
        $("input:checkbox[name='RadSurvey']:checked").each(function () {
            selectedBuilderArray.push($(this).siblings().filter("input[name='HdnBuilder']").val() + '~' + $(this).siblings().filter("input[name='HdnSurveyStatus']").val());
            selectedQuarter = ($(this).siblings().filter("input[name='HdnQuater']").val());
        });
        if (selectedQuarter == null || selectedQuarter == undefined || selectedQuarter == 'undefined' || selectedQuarter == 'null')
        {
            selectedQuarter = '';
        }
        if (selectedBuilderArray.length > 0)
        {
            if (confirm('Are you sure you want to change status of survey response?')) {
                Confirm = 'Yes';
            } else {
                Confirm = 'No';
                return;
            }
        }
        else
        {
            return;
        }
        if (parseInt(SurveyId) > 0 && Confirm == 'Yes') {
            var urldata = AjaxCallUrl.ChangeStatusSurveyResultUrl;
            $.ajax({ 
                type: 'POST', 
                url: urldata,
                data: { surveyid: SurveyId, BuilderIdList: selectedBuilderArray, QuarterId: selectedQuarter }, //, BuilderId: BuilderId
                dataType: 'json',
                beforeSend: function () {
                },
                error: function (request, error) {
                },
                success: function (data) {
                    if (data.IsSuccess) {
                        var KendoListView1 = $("#QuestionListww").data("kendoListView");
                        KendoListView1.dataSource.read();

                        var KendoListView = $("#QuestionAnswareList").data("kendoListView");
                        KendoListView.dataSource.read();
                    }
                    else {
                    }

                }
            });
        }
    },
    ArchivedNcpSurvey: function () {
        var Rad = $("input[name='RadSurvey']:checked");
        var SurveyId = Rad.siblings().filter("input[name='HdnSurvey']").val();
        var Archive = Rad.siblings().filter("input[name='HdnArchive']").val();
        var BuilderId = Rad.siblings().filter("input[name='HdnBuilder']").val();

        if (Archive == 'n') {
            if (parseInt(SurveyId) > 0) {
                var urldata = AjaxCallUrl.ArchievedStatusUrl;
                $.ajax({ //Process the form using $.ajax()
                    type: 'POST', //Method type
                    url: urldata,
                    data: { surveyid: SurveyId }, //, BuilderId: BuilderId
                    dataType: 'json',
                    beforeSend: function () {
                    },
                    error: function (request, error) {
                        // $(document).ajaxStop($.unblockUI);
                        // console.log('gggg');
                    },
                    success: function (data) {
                        if (data.IsSuccess) {
                            var KendoListView1 = $("#QuestionListww").data("kendoListView");
                            KendoListView1.dataSource.read();

                            var KendoListView = $("#QuestionAnswareList").data("kendoListView");
                            KendoListView.dataSource.read();
                        }
                        else {
                        }

                    }
                });
            }
        }
        else {
            //if (Pub == 'false') {
            if (parseInt(SurveyId) > 0) {
                var urldata = AjaxCallUrl.ArchievedStatusUrl;
                $.ajax({ //Process the form using $.ajax()
                    type: 'POST', //Method type
                    url: urldata,
                    data: { surveyid: SurveyId }, //Forms name
                    dataType: 'json',
                    beforeSend: function () {
                    },
                    error: function (request, error) {
                        // $(document).ajaxStop($.unblockUI);
                        // console.log('gggg');
                    },
                    success: function (data) {
                        if (data.IsSuccess) {
                            var KendoListView1 = $("#QuestionListww").data("kendoListView");
                            KendoListView1.dataSource.read();

                            var KendoListView = $("#QuestionAnswareList").data("kendoListView");
                            KendoListView.dataSource.read();
                        }
                        else {
                        }

                    }
                });


            }
            //}
            //else {
            //    alert('Live & Published surveys cannot be Archived.');
            //}

        }
    },
    EditSurvey : function (){
        var urldata = AjaxCallUrl.EditSurveyUrl;
        var Rad = $("input[name='RadSurvey']:checked");
        var surveyId = Rad.siblings().filter("input[name='HdnSurvey']").val();
        var builderId = Rad.siblings().filter("input[name='HdnBuilder']").val();
        var contractId = Rad.siblings().filter("input[name='HdnContract']").val();
        var projectId = Rad.siblings().filter("input[name='HdnProject']").val();
        var quaterId = Rad.siblings().filter("input[name='HdnQuater']").val();
        var SelectedRowCount = 0;
        $("input:checkbox[name='RadSurvey']:checked").each(function () {
            SelectedRowCount += 1;
        });
        
        if (SelectedRowCount == 1)
        {
            if (parseInt(surveyId) > 0 && parseInt(builderId) > 0 && parseInt(projectId) == 0) {
                var url = urldata.replace("****SurveyId****", surveyId).replace("****BuilderId****", builderId);
                window.location.href = url;
            }
            else if (parseInt(surveyId) > 0 && parseInt(builderId) > 0 && parseInt(projectId) > 0) {
                var url = AjaxCallUrl.EditNCPSurveyUrl.replace("****ContractId****", contractId).replace("****BuilderId****", builderId).replace("****QuaterId****", quaterId);
                window.location.href = url;
            }
        }
        
    }
};

function GetSelectedQuestionList()
{    
    var Products = [];
    var ProductList = '';
    var CheckedCheckBox = $("#DivFilterQuestion").children().find("input[name='ChkProjectSelect']:checked");
    if (CheckedCheckBox.length > 0) {                
        $(CheckedCheckBox).each(function () {
            Products.push($(this).siblings().eq(0).val());
        });
        ProductList = Products.join();
        QuestionFilter = ProductList;
    }
    else { QuestionFilter = '';}
    var KendoListView1 = $("#QuestionListww").data("kendoListView");
    KendoListView1.dataSource.read();

    var KendoListView = $("#QuestionAnswareList").data("kendoListView");
    KendoListView.dataSource.read();
}

function SendSurveyIdAsParameter() {
    var Filter;
    var marketFilter;
    var builderFilter;

    if (ResponseFilter != '' && ResponseFilter != null && ResponseFilter != 'undefined')
    {
        Filter = ResponseFilter;
        if (Filter == "") {
            Filter = "1";
        }

    }
    else if ($("#HdnIsCompleted").val() != null && $("#HdnIsCompleted").val() != 'undefined' && $("#HdnIsCompleted").val() != 'xxx') {
        Filter = $("#HdnIsCompleted").val();
        //$("#HdnIsCompleted").val('xxx');

    } 

    if (MarketFilter != '' && MarketFilter != null && MarketFilter != 'undefined') {
        marketFilter = MarketFilter;
    }

    if (BuilderFilter != '' && BuilderFilter != null && BuilderFilter != 'undefined') {
        builderFilter = BuilderFilter;
    }

    return { ResponseFilter: Filter, MarketFilter: marketFilter, BuilderFilter: builderFilter, SurveyId: $("#HdnSurveyId").val(), QuestionIdListFilter: QuestionFilter };
}

function UiniqueRow(GridResultset) {
    var unique = [];
    for (var i = 0; i < GridResultset.length; i++) {
        if (unique.indexOf(GridResultset[i].RowNo) == -1) {
            unique.push(GridResultset[i].RowNo);
        }
    }
    return unique;

}

function UiniqueColoumn(GridResultset) {

    var unique = [];
    for (var i = 0; i < GridResultset.length; i++) {
        if (unique.indexOf(GridResultset[i].ColNo) == -1) {
            unique.push(GridResultset[i].ColNo);
        }
    }
    //console.log(unique);
    return unique;
}

function onFilterServerResponse(e) {
    var dataItem = this.dataItem(e.item.index());
    ResponseFilter = dataItem.Value;

    var KendoListView = $("#QuestionAnswareList").data("kendoListView");
    KendoListView.dataSource.read();
}

function onFilterMarketServerResponse(e) {
    var dataItem = this.dataItem(e.item.index());
    MarketFilter = dataItem.Value;

    var KendoListView = $("#QuestionAnswareList").data("kendoListView");
    KendoListView.dataSource.read();
}

function onFilterBuilderServerResponse(e) {
    var dataItem = this.dataItem(e.item.index());
    BuilderFilter = dataItem.Value;

    var KendoListView = $("#QuestionAnswareList").data("kendoListView");
    KendoListView.dataSource.read();
}

function checkSelectAll() {
    //alert('Select All ' + document.getElementById("SelectAll").checked);
    if (document.getElementById("SelectAll").checked == true)
    {
        document.getElementById("SelectRow").checked = true
        var isChecked = true;
        $("#QuestionAnswareList").find('input[type="checkbox"]').prop('checked', isChecked);
    }
    else
    {
        document.getElementById("SelectRow").checked = false
        var isChecked = false;
        $("#QuestionAnswareList").find('input[type="checkbox"]').prop('checked', isChecked);
    }
}

function getReportName() {    
    var Rad = $("input[name='RadSurvey']:checked");
    var name = Rad.siblings().filter("input[name='HdnExcelReportName']").val();
    return name;
}