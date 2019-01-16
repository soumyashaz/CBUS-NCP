



$(document).ready(function () {
    // console.log($("div").length);
    //  console.log($("div").not("#DivProductCategory").not("#DivProductCategoryDropdown"));


    //console.log(v.length);

    $("#DivProductCategoryDropdown").on("click", function () {

        CustomControl($(this).parent().parent(), "CustomControlPoup", $(this).attr('id'));
        // event.stopPropagation();

        if ($("#CreateContract").length) {
            $('html').on('click', function () {
                // do your stuff here
                var ChildDiv = $("#DivProductCategory").children(".CustomControlPoup");

                if ($(ChildDiv).is(':visible')) {
                    $(ChildDiv).hide();
                }
            }).find('.classpdtcategory').on('click', function (e) {
                e.stopPropagation();
            });
        }


    });

    //console.log($("#CreateContract").length);

    if ($("#CreateContract").length) {

        $('#CreateContract').on('click', function () {
            // do your stuff here
            var ChildDiv = $("#DivProductCategory").children(".CustomControlPoup");
            if ($(ChildDiv).is(':visible')) {
                $(ChildDiv).hide();
            }
        }).find('.classpdtcategory').on('click', function (e) {
            var ChildDiv = $("#DivProduct").children(".CustomControlPoupProduct");

            if ($(ChildDiv).is(':visible')) {
                $(ChildDiv).hide();
            }
            e.stopPropagation();
        });
    }

    

    

});

var CustomControl = function (MainDiv, ClassName, ProductcategoryHeader) {
    // console.log('ProductcategoryHeader');
    if ($(MainDiv).children("." + ClassName).length == 0) {
        $(MainDiv).append("<div  class='" + ClassName + "'><div id='CategoryTreeview'></div></div>");
        var ChildDiv = $(MainDiv).children("." + ClassName);
        // console.log(parseInt($(MainDiv).height()));
        $(ChildDiv).css({
            "top": parseInt($(MainDiv).children('.CustomControl-Div').height()) + 5,
            "width": parseInt(($(MainDiv).children('.CustomControl-Div').width()))
        });

        RenderProductCategoryTreeViewControl(MainDiv);
    }
    else {
        var ChildDiv = $(MainDiv).children("." + ClassName);
        if ($(ChildDiv).is(':visible')) {
            $(ChildDiv).hide();
        }
        else {
            $(ChildDiv).show();
        }
        // $()
        if ($('.CustomControl-Div').children('div[id ^= "divCategorySelected_"]').length == 0) {
            // console.log('ffffffffkk');
            $("#DivProduct").attr('disabled', true);
        }
        else {
            $("#DivProduct").attr('disabled', false);
        }

    }
};




var RenderProductCategoryTreeViewControl = function (MainDiv) {
    $("#CategoryTreeview").kendoTreeView({
        //  autoBind: false,
        dataSource: CategoryTreeviewDataSource,
        dataTextField: "CategoryName",
        animation: {
            expand: {
                effects: "fadeIn expandVertical"
            }
        },
        select: function (e) {
            var treeview = $("#CategoryTreeview").data("kendoTreeView");
            var item = treeview.dataItem(e.node);
            if (item) {
                //  console.log(item.CategoryId);
                var selectitem = treeview.findByUid(item.CategoryId);
                // Mark the node as selected
                treeview.select(selectitem);

                // console.log(e.node);
                if ($(MainDiv).children('.CustomControl-Div').find("input[type='hidden'][id='HdnCategorySelect_" + item.CategoryId + "']").length == 0) {
                    var SelectCategoryHtml = "<div class='divCategorySelected' id='divCategorySelected_" + item.CategoryId + "'>" + item.CategoryName;
                    SelectCategoryHtml += "<div class='DivCrossIcon' onclick='ProductCrossIcon(" + item.CategoryId + ")'></div>";
                    SelectCategoryHtml += "<input type='hidden' id='HdnCategorySelect_" + item.CategoryId + "' name='CategorySelected' value='" + item.CategoryId + "'/>";
                    SelectCategoryHtml += "</div>";
                    $(MainDiv).children('.CustomControl-Div').append(SelectCategoryHtml);
                }

                // var someVariable = item.whatever;
            } else {
                //  console.log('nothing selected');
            }
        }

    });

};


var ProductCrossIcon = function (Productcategoryid) {

    // var ProductCategoryValue = $(this).siblings().find("input[type='hidden']").val();
    // console.log(ProductCategoryValue);
    if ($('.CustomControl-Div').children('#divCategorySelected_' + Productcategoryid).length > 0) {
        //  console.log('ggg1111');
        $('.CustomControl-Div').children().remove('#divCategorySelected_' + Productcategoryid);
    }


    if ($('.CustomControl-Div').children('div[id ^= "divCategorySelected_"]').length == 0) {
        // console.log('ffffffffkk');
        $("#DivProduct").attr('disabled', true);
    }
    else {
        $("#DivProduct").attr('disabled', false);
    }

}


var CategoryTreeviewDataSource = new kendo.data.HierarchicalDataSource({
    transport: {
        read: {
            // url: '/Home/ParseProductTreeViewControl',
            url: TreeControlUrl,
            dataType: "json"
        }
    },
    schema: {
        model: {
            id: "CategoryId",
            hasChildren: "HasSubCategory"
        }
    }
});