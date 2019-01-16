var ProductCategoryOld = [];

$(document).ready(function () {    
    if ($("#BuilderContractQuaterReport").length) {
        $('html').on('click', function () {
            // do your stuff here
            var ChildDiv = $("#DivBuilderContractProject").children(".CustomControlPoupProject");

            if ($(ChildDiv).is(':visible')) {
                $(ChildDiv).hide();
            }
        }).find('.classproject').on('click', function (e) {
            e.stopPropagation();
        });
    }

});


var CustomControlProject = function (MainDiv, ClassName) {

    if ($(MainDiv).children("." + ClassName).length == 0) {
        $(MainDiv).append("<div  class='" + ClassName + "'><div id='ProjectList'></div></div>");
        var ChildDiv = $(MainDiv).children("." + ClassName);

        $(ChildDiv).css({
            "top": parseInt($(MainDiv).children('.CustomControl-Div').height()) + 5,
            "width": parseInt(($(MainDiv).children('.CustomControl-Div').width()))
        });


        FetchProject();
    }
    else {
        var ChildDiv = $(MainDiv).children("." + ClassName);
        if ($(ChildDiv).is(':visible')) {
            $(ChildDiv).hide();
        }
        else {
            $(ChildDiv).show();
        }
        //    var Validator = $("#DivProduct").kendoValidator({
        //        messages: {
        //            contractproduct: function (input) {
        //                return "Atleast one Product selection is required";
        //            }
        //        },
        //        rules: {
        //            contractproduct: function (input) {
        //                if (input.is("[data-role=contracproduct]")) {
        //                    var CheckedCheckBox = $("#ProductList").children().find("input[name='ChkProductSelect']:checked");
        //                    if (CheckedCheckBox.length == 0) {
        //                        return false;
        //                    }
        //                    else {
        //                        return true;
        //                    }

        //                    return false

        //                }
        //                return true;


        //            }

        //        }

        //    }).data("kendoValidator");

        //    Validator.validateInput($("#DivProduct"));


        //}
        //else {
        // $(ChildDiv).show();
        //  FetchProduct();
        //  }
        // $()


    }
};


var FetchProject = function () {

    kendo.ui.progress($("#ProjectList"), false);
    var postData = { ContractId: $("#HdnContractId").val(), BuilderId: $("#HdnBuilderId").val(), QuaterId: $("#HdnQuaterId").val() }; //done
    // console.log(JSON.stringify(ArrayProductCategory));
    $.ajax({ //Process the form using $.ajax()
        type: 'POST', //Method type
        url: AjaxCallUrl.CustomProjectControlUrl,
        data: postData, //Forms name
        dataType: 'json',
        // traditional: true,
        beforeSend: function () {
        },
        success: function (data) {
            $("#ProjectList").append(data.ProjectCustomControl);
            kendo.ui.progress($("#ProjectList"), false);
            AssignControlEvent();
        },
        error: function (request, error) {
            kendo.ui.progress($("#ProjectList"), false);
        }
    });
}




var AssignControlEvent = function () {
    $('#ChkSelectAll').on("click", function () {

        var ListProjectId = [];


        var AllCheckBox = $("input[name='ChkProjectSelect']");

        AllCheckBox.each(function () {

            if (!$(this).is(':checked')) {
                ListProjectId.push($(this).siblings().eq(0).val());
                $(this).prop('checked', true);
                $(this).attr("disabled", "disabled");
                $(this).parent().siblings().css("color", "green");
            }

        });

        BuilderReport.AddMultipleProjectToReport(ListProjectId);
       // console.log(ListProjectId);
       // return false;

       // $("input[name='ChkProjectSelect']").prop('checked', true);
        //Add all the project in NCP Rebate report here





    });

    $('#ChkClearAll').on("click", function () {
        //   if ($(this).is(":checked")) {
        $("input[name='ChkProjectSelect']").prop('checked', false);
        // }

    });

    $("#TxtProjectSearch").keyup(function () {

        var Searchtext = $(this).val().toUpperCase();

        //$("#ShowProductTable td span").filter(function (index) {
        //    //return $("strong", this).length === 1;
        //    if ($(this).contains(Searchtext)) {
        //        console.log('gg');
        //        return true;
        //    }
        //});

        $(".ShowProductTable td span:containsIgnoreCase(" + Searchtext + ")").parent().parent().show();

        $(".ShowProductTable td span:not(:containsIgnoreCase(" + Searchtext + "))").parent().parent().hide();
        // var SerachSpan = $("#ShowProductTable td span:contains(" + Searchtext + ")").parent().parent().show();
        //console.log(SerachSpan.length);
    });


    $("input[name='ChkProjectSelect']").on("click", function () {
        var ProjectId = $(this).siblings().eq(0).val();

        $(this).attr("disabled", "disabled");
        $(this).parent().siblings().css("color", "green");
        //  return false;

        BuilderReport.AddProjectToReport(ProjectId);
    });




}


$.expr[':'].containsIgnoreCase = function (n, i, m) {
    return jQuery(n).text().toUpperCase().indexOf(m[3].toUpperCase()) >= 0;
};

