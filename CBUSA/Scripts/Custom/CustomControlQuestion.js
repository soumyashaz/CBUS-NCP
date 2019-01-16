var ProductCategoryOld = [];

$(document).ready(function () {
    if ($("#DivShowResponse").length) {
        $('html').on('click', function () {
            // do your stuff here
            var ChildDiv = $("#DivFilterQuestion").children(".CustomControlFilterQuestion");
            if ($(ChildDiv).is(':visible')) {
                $(ChildDiv).hide();
            }
        }).find('.classQuestionCustomContrrol').on('click', function (e) {
            e.stopPropagation();
        });
    };    
});


var CustomControlQuestion = function (MainDiv, ClassName) {

    if ($(MainDiv).children("." + ClassName).length == 0) {
        $(MainDiv).append("<div  class='" + ClassName + "'><div id='QuestionList'></div></div>");
        var ChildDiv = $(MainDiv).children("." + ClassName);

        $(ChildDiv).css({
            "top": parseInt($(MainDiv).children('.CustomControl-Div').height()) + 5,
            "width": parseInt(($(MainDiv).children('.CustomControl-Div').width()))
        });


        FetchQuestion();
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


var FetchQuestion = function () {

    kendo.ui.progress($("#QuestionList"), true);
    var postData = { SurveyId: $("#HdnSurveyId").val() }; //done

    $.ajax({ //Process the form using $.ajax()
        type: 'POST', //Method type
        url: AjaxCallUrl.CustomQuestionControlUrl,
        data: postData, //Forms name
        dataType: 'json',
        // traditional: true,
        beforeSend: function () {
        },
        success: function (data) {
            $("#QuestionList").append(data.QuestionFilterCustomControl);
            kendo.ui.progress($("#QuestionList"), false);
            AssignControlEvent();
        },
        error: function (request, error) {
            kendo.ui.progress($("#QuestionList"), false);
        }
    });
}




var AssignControlEvent = function () {
    $('#ChkSelectAll').on("click", function () {
        $("input[name='ChkProjectSelect']").prop('checked', true);
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


    //$("input[name='ChkProjectSelect']").change(function () {

    //    if ($(this).is(":checked")) {

    //        var ProjectId = $(this).siblings("input[name='HdnProjectId']").val();
    //        BuilderReport.AddProjectToReport(ProjectId);

    //    }

    //});


    //$("#TxtProductSearch").change(function () {
    //    // Check input( $( this ).val() ) for validity here
    //    console.log($(this).val());

    //});

}


$.expr[':'].containsIgnoreCase = function (n, i, m) {
    return jQuery(n).text().toUpperCase().indexOf(m[3].toUpperCase()) >= 0;
};

