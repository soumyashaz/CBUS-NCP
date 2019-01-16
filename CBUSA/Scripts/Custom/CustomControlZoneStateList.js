var ProductCategoryOld = [];


$(document).ready(function () {



});



var CustomControlZoneStateList = function (MainDiv, ClassName) {

    if ($(MainDiv).children("." + ClassName).length == 0) {        
        $(MainDiv).append("<div  class='" + ClassName + "'><div id ='ZoneStateList'></div></div>");
        var ChildDiv = $(MainDiv).children("." + ClassName);
        // console.log(parseInt($(MainDiv).height()));
        $(ChildDiv).css({
            "top": parseInt($(MainDiv).children('.CustomControl-Div').height()) + 5,
            "width": parseInt(($(MainDiv).children('.CustomControl-Div').width()) + 285)
        });

        // RenderProductCategoryTreeViewControl(MainDiv);
        // FetchProduct();
        FetchZoneState();
    }
    else {
        
        var ChildDiv = $(MainDiv).children("." + ClassName);
        if ($(ChildDiv).is(':visible')) {
            $(ChildDiv).hide();

            //check about any of the state and zone selected or not
            //var CheckedCheckBox = $("input[name='ChkStateSelect']").prop('checked');
            //if (CheckedCheckBox.length == 0) {
            //    $("#ValidationMsgZoneState").text("Atleast one market selection is required");
            //}


            var Validator = $("#DivZoneState").kendoValidator({
                messages: {
                    ZoneSate: function (input) {
                        return "Atleast one market selection is required";
                    }
                },
                rules: {
                    ZoneSate: function (input) {
                        if (input.is("[data-role=ZoneSate]")) {
                            var CheckedCheckBox = $("#ZoneStateList").children().find("input[name='ChkStateSelect']:checked");
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

            var v = Validator.validateInput($("#DivZoneState"));
            // console.log(v);
            if (v) {

                if ($("#HdnZoneSetting").length) {
                    if ($("#HdnZoneSetting").val() == "1") {
                        var DdlBuilder = $("#DdlBuilder").data("kendoDropDownList");
                        DdlBuilder.dataSource.read();
                    }
                }
            }
        }
        else {
            $(ChildDiv).show();
            //  FetchZoneState();
            // FetchZoneState();
        }
    }
};


var FetchZoneState = function () {



    kendo.ui.progress($("#ZoneStateList"), true);
    //var ProductCategory = $('.CustomControl-Div').find("[id ^='HdnCategorySelect_']");
    //var ArrayProductCategory = [];
    //$(ProductCategory).each(function () {
    //    ArrayProductCategory.push(parseInt($(this).val()));
    //});

    //var ProductCategoryData = new FormData();
    //ProductCategoryData.append('Values', ArrayProductCategory);

    var postData = {}; //done

    // console.log(JSON.stringify(ArrayProductCategory));


    if ($("#DivConfigureSurveyInvite").length) {
        postData = { Flag: "SurveyMarket", Id: $("#HdnSurveyId").val() };
    }

    if ($("#DivContractResource").length) {
        postData = { Flag: "ResourceMarket", Id: $("#HdnResourceId").val() };
    }
    

    $.ajax({ //Process the form using $.ajax()
        type: 'POST', //Method type
        url: ZoneControlUrl,
        data: postData, //Forms name
        dataType: 'json',
        // traditional: true,
        beforeSend: function () {
        },
        success: function (data) {
            $("#ZoneStateList").html(data.ProductCustomControl);
            //   AssignControlEvent();
            AssignControlZoneStateEvent();
            kendo.ui.progress($("#ZoneStateList"), false);
        },
        error: function (request, error) {
            kendo.ui.progress($("#ZoneStateList"), false);
        }
    });


}


var AssignControlZoneStateEvent = function () {

    if ($("#HdnSelectedMarketControl").length) {

       // console.log($("#HdnSelectedMarketControl").val());
        if ($("#HdnSelectedMarketControl").val() !== undefined && $("#HdnSelectedMarketControl").val() != "") {
            var MarketLis = $("#HdnSelectedMarketControl").val().split(",");
         //   console.log(MarketLis.length);
            $("input[name='ChkStateSelect']")
              .filter(function (index) {
                  // return $("strong", this).length === 1;
                  var Market = $(this).siblings().eq(0).val();

                  return MarketLis.indexOf(Market) > -1;
              }).prop('checked', true);


            // console.log(MarketLis);
        }
        else {
            $("input[name='ChkStateSelect']").prop('checked', true);
        }
    }
    //console.log($("#SelectedMarket").val());
    // $("input[name='ChkStateSelect']").prop('checked', true);



    $("#StateChkSelectAll").click(function () {
        $("input[name='ChkStateSelect']").prop('checked', true);
    });
    $("#StateChkClearAll").click(function () {
        $("input[name='ChkStateSelect']").prop('checked', false);
    });

    //$('#StateChkSelectAll').change(function () {
    //    if ($(this).is(":checked")) {
    //        $("input[name='ChkStateSelect']").prop('checked', true);
    //    }
    //});

    //$('#StateChkClearAll').change(function () {
    //    if ($(this).is(":checked")) {
    //        $("input[name='ChkStateSelect']").prop('checked', false);
    //    }
    //});

    $("#TxtStateSearch").keyup(function () {

        var Searchtext = $(this).val().toUpperCase();

        //$("#ShowProductTable td span").filter(function (index) {
        //    //return $("strong", this).length === 1;
        //    if ($(this).contains(Searchtext)) {
        //        console.log('gg');
        //        return true;
        //    }
        //});

        $(".ShowZoneStateTable td span:containsIgnoreCase(" + Searchtext + ")").parent().parent().show();

        $(".ShowZoneStateTable td span:not(:containsIgnoreCase(" + Searchtext + "))").parent().parent().hide();
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

