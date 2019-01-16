var ProductCategoryOld = [];

$(document).ready(function () {

    if ($("#CreateContract").length) {
        $('html').on('click', function () {
            // do your stuff here
            var ChildDiv = $("#DivProduct").children(".CustomControlPoupProduct");

            if ($(ChildDiv).is(':visible')) {
                $(ChildDiv).hide();
            }
        }).find('.classpdt').on('click', function (e) {
            e.stopPropagation();
        });
    }

});


var CustomControlProduct = function (MainDiv, ClassName) {

    if ($(MainDiv).children("." + ClassName).length == 0) {
        $(MainDiv).append("<div  class='" + ClassName + "'><div id='ProductList'></div></div>");
        var ChildDiv = $(MainDiv).children("." + ClassName);
        // console.log(parseInt($(MainDiv).height()));
        $(ChildDiv).css({
            "top": parseInt($(MainDiv).children('.CustomControl-Div').height()) + 5,
            "width": parseInt(($(MainDiv).children('.CustomControl-Div').width()))
        });

        // RenderProductCategoryTreeViewControl(MainDiv);
        FetchProduct();
    }
    else {
        var ChildDiv = $(MainDiv).children("." + ClassName);
        if ($(ChildDiv).is(':visible')) {
            $(ChildDiv).hide();
            var Validator = $("#DivProduct").kendoValidator({
                messages: {
                    contractproduct: function (input) {
                        return "Atleast one Product selection is required";
                    }
                },
                rules: {
                    contractproduct: function (input) {
                        if (input.is("[data-role=contracproduct]")) {
                            var CheckedCheckBox = $("#ProductList").children().find("input[name='ChkProductSelect']:checked");
                            if (CheckedCheckBox.length == 0) {
                                return false;
                            }
                            else {
                                return true;
                            }

                            return false

                        }
                        return true;


                    }

                }

            }).data("kendoValidator");

            Validator.validateInput($("#DivProduct"));


        }
        else {
            $(ChildDiv).show();
            FetchProduct();
        }
        // $()


    }
};


var FetchProduct = function () {

    if ($('.CustomControl-Div').children('div[id ^= "divCategorySelected_"]').length == 0) {
        // console.log('ffffffffkk');
        return;
    }
    kendo.ui.progress($("#ProductList"), true);
    var ProductCategory = $('.CustomControl-Div').find("[id ^='HdnCategorySelect_']");
    var ArrayProductCategory = [];
    $(ProductCategory).each(function () {
        ArrayProductCategory.push(parseInt($(this).val()));
    });

    var ProductCategoryHistory = [];
    ProductCategoryOld = $("#ProductIdHistory").val().split(",");
    //var ProductCategoryData = new FormData();
    //ProductCategoryData.append('Values', ArrayProductCategory);

    var Flag = 0;

    if ($("#ProductIdHistory").val() != "") {
        Flag = 1;
    }

    var postData = { SubCategories: ArrayProductCategory, SubCategoryHistory: ProductCategoryOld, Flag: Flag }; //done
    // console.log(JSON.stringify(ArrayProductCategory));
    $.ajax({ //Process the form using $.ajax()
        type: 'POST', //Method type
        url: ProductUrl,
        data: postData, //Forms name
        dataType: 'json',
        // traditional: true,
        beforeSend: function () {
        },
        success: function (data) {
            if (data.IsAppendRequired) {
                $("#ProductList").append(data.ProductCustomControl);

                //   console.log
                // (data.RemoveList);
                if (data.RemoveList.length > 0) {
                    for (var i = 0; i < data.RemoveList.length; i++) {
                        var Item = data.RemoveList[i];
                        $("#ProductList").find("#divProductCategoryContainer_" + Item + "").remove();
                    }
                }

                AssignControlEvent();
                kendo.ui.progress($("#ProductList"), false);
                $("#ProductIdHistory").val(ArrayProductCategory.join());
            }
            else {

                // console.log
                // (data.RemoveList);
                if (data.RemoveList.length > 0) {
                    for (var i = 0; i < data.RemoveList.length; i++) {
                        var Item = data.RemoveList[i];
                        $("#ProductList").find("#divProductCategoryContainer_" + Item + "").remove();
                    }
                }
                kendo.ui.progress($("#ProductList"), false);
            }
        },
        error: function (request, error) {
            kendo.ui.progress($("#ProductList"), false);
        }
    });


}


var AssignControlEvent = function () {
    $('#ChkSelectAll').on("click", function () {
        // if ($(this).is(":checked")) {
        $("input[name='ChkProductSelect']").prop('checked', true);
        //}

    });

    $('#ChkClearAll').on("click", function () {
        //   if ($(this).is(":checked")) {
        $("input[name='ChkProductSelect']").prop('checked', false);
        // }

    });

    $("#TxtProductSearch").keyup(function () {

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




    //$("#TxtProductSearch").change(function () {
    //    // Check input( $( this ).val() ) for validity here
    //    console.log($(this).val());

    //});

}


$.expr[':'].containsIgnoreCase = function (n, i, m) {
    return jQuery(n).text().toUpperCase().indexOf(m[3].toUpperCase()) >= 0;
};

