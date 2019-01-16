$(document).ready(function () {


    var dropdownlist = $("#ContractStatusId").data("kendoDropDownList");
    if (dropdownlist.value() == "1") {
        $("div[data-role='Active']").show();
        $("label[data-role='Active']").show();
        $("div[data-role='Pending']").hide();
        $("label[data-role='Pending']").hide();
    }
    else {
        $("div[data-role='Active']").hide();
        $("div[data-role='Pending']").show();
        $("label[data-role='Active']").hide();
        $("label[data-role='Pending']").show();
    }


    // ProductContractView.AssignControl();
    // ContractBuilder.AssignControl();
    ProductContractView.LoadContractProduct();
    ContractBuilder.LoadContractBuilder();

    //ContractProduct.View();

    //$("#ResourceAddMore").on("click", function () {
    //    ContractResourceView.AddMore();
    //   // console.log("kkk");
    //});

    $("#a_ContactResourceViewMore").on("click", function () {
        ContractResourceView.ViewMore();
    });


    $("#a_manage_rebate").on("click", function () {
        var wnd = $("#WndContractRebate").data("kendoWindow");
        wnd.refresh({
            url: AjaxCallUrl.ContractRebateUrl, // returns JSON, { firstName: "Alyx", lastName: "Vance" }
            data: { ContractId: $("#HdnContractId").val() },
        });
        wnd.open().center();
    });
});

function surveyresponse(surveyid, IsCompleted) {
    var url = AjaxCallUrl.RedirectResponsetUrl.replace("****SurveyId****", surveyid).replace("****IsCompleted****", 0);
    //alert(url);
    //url = url.replace("****IsCompleted****", parseInt(IsCompleted));
    //var win = window.open(url, '_blank');
    //win.focus();    
    window.location.href = url;
}

function SendContractIdAsParameter() {

    return { ContractId: $("#HdnContractId").val(), PageValue: $("#HdnResourcePageValue").val(), SearchText: $("#txtSearchResource").val() };
}


function ContractResourceListViewDataBound() {

    if (this.dataSource.data().length > 0) {
        var PageValue = $("#HdnResourcePageValue").val();
        $("#HdnResourcePageValue").val(parseInt(PageValue) + 1);

        var datasourcedata = $("#listViewContractResource").data("kendoListView").dataSource.data();

        for (var i = 0; i < datasourcedata.length; i++) {
            if (datasourcedata[i].IsAllRowVisible) {
                $("#a_ContactResourceViewMore").hide();
            }
        }
    }
    else {
        $("#a_ContactResourceViewMore").hide();
        $("#listViewContractResource").append("<h5>There are no resource associated with this Contract</h5>");
    }
}

function ContractSurveyListViewDataBound() {
    if (this.dataSource.data().length == 0) {
        //custom logic
        $("#listViewContractSurvey").append("<h5>There are no surveys associated with this Contract</h5>");
    }
}

function SendContractSurveyParameter() {

    return { ContractId: $("#HdnContractId").val() };
}

function OnOpenContractRebate(e) {
    var wnd = $("#WndContractRebate").data("kendoWindow");
    kendo.ui.progress(wnd.element, true);
}

function OnRefreshContractRebate(e) {
    var wnd = $("#WndContractRebate").data("kendoWindow");
    kendo.ui.progress(wnd.element, false);
    ContractRebate.AssignControl();
}

function OnOpenContractRebateOverride(e) {
    var wnd = $("#WndContractRebateOverride").data("kendoWindow");
    kendo.ui.progress(wnd.element, true);
}

function OnRefreshContractRebateOverride(e) {
    var wnd = $("#WndContractRebateOverride").data("kendoWindow");
    kendo.ui.progress(wnd.element, false);
    ContractRebateOverride.AssignControl();
}

function OnOpenModifyContractProduct(e) {
    var wnd = $("#WndAddContractProduct").data("kendoWindow");
    kendo.ui.progress(wnd.element, true);
}

function OnRefreshModifyContractProduct(e) {
    var wnd = $("#WndAddContractProduct").data("kendoWindow");
    kendo.ui.progress(wnd.element, false);
    ProductContractView.AddProductContractAssignControl();
}

function OnOpenProductViewDetails(e) {
    var wnd = $("#WndProductViewDetails").data("kendoWindow");
    kendo.ui.progress(wnd.element, true);
}

function OnRefreshProductViewDetails(e) {
    var wnd = $("#WndProductViewDetails").data("kendoWindow");
    kendo.ui.progress(wnd.element, false);
    // ProductContractView.AddProductContractAssignControl();
    ProductContractView.AssignControlViewdetailsPopup();

}

function OnOpenMarketBuilderViewDetails(e) {
    var wnd = $("#WndMarketBuilderViewDetails").data("kendoWindow");
    kendo.ui.progress(wnd.element, true);
}

function OnRefreshMarketBuilderViewDetails(e) {
    var wnd = $("#WndMarketBuilderViewDetails").data("kendoWindow");
    kendo.ui.progress(wnd.element, false);
    // ProductContractView.AddProductContractAssignControl();
    // console
    // .log('ffff');
    ContractBuilder.AssignControlViewdetailsBuilderPopup();
}

function GetMarketInformation() {

    var SelectedMarket = $("#ZoneStateList").children().find("input[name='ChkStateSelect']:checked");
    var Markets = [];
    var MarketList = '';
    $(SelectedMarket).each(function () {
        Markets.push($(this).siblings().eq(0).val());
    });

    MarketList = Markets.join();

    return {
        MarketList: MarketList,
        ContractId: $("#HdnContractId").val()
    }

}

function onSelectDdlBuilder(e) {
    var dropdownlist = $("#DdlBuilder").data("kendoDropDownList");
    //  console.log(dropdownlist.value());
    var dataItem = this.dataItem(e.item.index());
    //  console.log(dataItem);
    //  console.log(dataItem.BuilderId);
    //  console.log(dropdownlist.value());
    // console.log(dropdownlist.value());

    if (dataItem.BuilderId > 0) {
        ContractRebateOverride.GetBuilderRebateInformation(dataItem.BuilderId, $("#HdnContractId").val());
    }
}

function onDataBoundDdlBuilder(e) {
    var BuilderId = $("#DdlBuilder").data("kendoDropDownList").value();
    if (BuilderId > 0) {
        ContractRebateOverride.GetBuilderRebateInformation(BuilderId, $("#HdnContractId").val());
    }

}

//function OnOpenContactResource(e) {
//    var wnd = $("#ContractResource").data("kendoWindow");
//    kendo.ui.progress(wnd.element, true);
//}


//function OnRefreshContractResource(e) {
//    var wnd = $("#ContractResource").data("kendoWindow");
//    kendo.ui.progress(wnd.element, false);


//}


var ContractResourceView = {

    ViewMore: function () {
        var KendoListView = $("#listViewContractResource").data("kendoListView");
        KendoListView.dataSource.read();
    },
    //AddMore: function () {

    //    var wnd = $("#ContractResource").data("kendoWindow");
    //    wnd.refresh({
    //        url: AjaxCallUrl.ContractResourceUrl, // returns JSON, { firstName: "Alyx", lastName: "Vance" }
    //        // data: { ResourceId: dataValue.ResourceId },
    //    });
    //    wnd.open().center();
    //}



}


var ContractBuilder = {
    AssignControl: function () {
        $("#BtnSaveContractBuilder").on("click", ContractBuilder.UpdateContractBuilder);
        $("#BtnMarketBuildersViewDetails").on("click", ContractBuilder.OpenMarketBuilderDetailsPopup);

        //====================== Search (Apala on 26th May 2017) =======================
        $("#txtSearchBuilder").bind("focus", function () {
            $("#txtSearchBuilder").val("");
            $("#txtSearchBuilder").prop("placeholder", "");
        });

        $("#txtSearchBuilder").bind("blur", function () {
            if ($("#txtSearchBuilder").val().trim() == "") {
                $("#txtSearchBuilder").prop("placeholder", "Search Builder...");
            }
        });

        $("#txtSearchBuilder").bind("keyup keydown", function () {
            //ContractBuilder.SearchBuilder();
        });

        $("#lnkSearchBuilder").bind("click", function () {
            ContractBuilder.SearchBuilder();
        });

        $("#txtSearchResource").bind("focus", function () {
            $("#txtSearchResource").val("");
            $("#txtSearchResource").prop("placeholder", "");
        });

        $("#txtSearchResource").bind("blur", function () {
            if ($("#txtSearchResource").val().trim() == "") {
                $("#txtSearchResource").prop("placeholder", "Search Resource...");
            }
        });

        //$("#txtSearchResource").bind("keyup keydown", function () {
            //ContractBuilder.SearchResource();
        //});

        $("#lnkSearchResource").bind("click", function () {
            $("#HdnResourcePageValue").val('1');
            ContractResourceView.ViewMore();
        });

        $("#lnkClearSearch").bind("click", function () {
            $("#txtSearchResource").val("");
            $("#txtSearchResource").prop("placeholder", "Search Resource...");
            $("#a_ContactResourceViewMore").show();
            $("#HdnResourcePageValue").val('1');
            ContractResourceView.ViewMore();
        });
    },
    UpdateContractBuilder: function () {
        var SelectedBuilder = $("input[name='ChkBuilderSelect']");
        var Builders = [];

        var UnselectedBuilderCount = 0;
        $(SelectedBuilder).each(function () {
            if ($(this).is(':not(:checked)'))
            {
                UnselectedBuilderCount = UnselectedBuilderCount + 1;
                Builders.push({ ContractId: $("#HdnContractId").val(), BuilderId: $(this).siblings().eq(0).val() });
            }
        });

        var result;
        if (UnselectedBuilderCount > 0) {
            result = confirm('You have un-selected ' + UnselectedBuilderCount + ' Builders. Are you sure you want to remove them from this contract?');
        }
        if (result == false) {
            return false;
        } else {
            kendo.ui.progress($("#ContractMarketBuilder"), true);

            var postData = { ObjContractBuilder: Builders }; //done
            $.ajax({ //Process the form using $.ajax()
                type: 'POST', //Method type
                url: AjaxCallUrl.UpdateContractBuilder,
                data: postData, //Forms name
                dataType: 'json',
                // traditional: true,
                beforeSend: function () {
                },
                success: function (data) {
                    if (data.IsSuccess) {
                        kendo.ui.progress($("#ContractMarketBuilder"), false);
                        var DataUpdateWindow = $("#DataUpdateMessage").data("kendoWindow");

                        ContractBuilder.LoadContractBuilder();
                        if (DataUpdateWindow) {
                            DataUpdateWindow.title("Update Information");
                            DataUpdateWindow.content("<div class='msgboxSuccess'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i> Data Update Successfully</div>");
                            DataUpdateWindow.open();
                            //redirect to listing page
                        }
                    }
                },
                error: function (request, error) {
                    kendo.ui.progress($("#ContractMarketBuilder"), false);
                    if (DataUpdateWindow) {
                        DataUpdateWindow.title("Error Information");
                        DataUpdateWindow.content("<div class='msgboxError'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i>" + data.ModelError + " </div>");
                        DataUpdateWindow.open();
                    }
                }
            });
        }
    },

    LoadContractBuilder: function () {
        var FormData = {
            'ContractId': $("#HdnContractId").val()
        };
        kendo.ui.progress($("#ContractMarketBuilder"), true);
        $.ajax({ //Process the form using $.ajax()
            type: 'GET', //Method type
            url: AjaxCallUrl.LoadContractBuilderUrl,
            data: FormData, //Forms name
            beforeSend: function () {
            },
            error: function (request, error) {
                kendo.ui.progress($("#ContractMarketBuilder"), false);
            },
            success: function (data) {
                if (data.IsSuccess) {
                    $("#DivChildMarketBuilder").html(data.PartialView);
                    ContractBuilder.AssignControl();
                }
                kendo.ui.progress($("#ContractMarketBuilder"), false);
            }
        });
    },

    OpenMarketBuilderDetailsPopup: function () {
        var wnd = $("#WndMarketBuilderViewDetails").data("kendoWindow");
        wnd.refresh({
            url: AjaxCallUrl.OpenMarketBuilderDetailsPopupUrl, // returns JSON, { firstName: "Alyx", lastName: "Vance" }
            data: { ContractId: $("#HdnContractId").val() }
        });
        wnd.open().center();
    },

    AssignControlViewdetailsBuilderPopup: function () {
        // console.log('asda');

        $("#downloadbuilderdetials").on("click", ContractBuilder.DownlaodBuilderDetails);
    },

    DownlaodBuilderDetails: function () {


        var ContractId = $("#HdnContractId").val();
        var src = AjaxCallUrl.DownloadContractMarketDetailsUrl + '?ContractId=' + ContractId;
        console.log(src);

        var iframe = $("<iframe/>").load(function () {
            // $.unblockUI();
        }).attr({
            src: src
        }).appendTo($("#DivIframeContainer"));

        //var postData = {

        //    ContractId: $("#HdnContractId").val()
        //}; //done
        //// console.log(JSON.stringify(ArrayProductCategory));
        //$.ajax({ //Process the form using $.ajax()
        //    type: 'POST', //Method type
        //    url: AjaxCallUrl.DownloadContractMarketDetailsUrl,
        //    data: postData, //Forms name
        //    dataType: 'json',
        //    // traditional: true,
        //    beforeSend: function () {
        //    },
        //    success: function (data) {
        //        // $("#ProductList").html(data.ProductCustomControl);
        //        //  AssignControlEvent();

        //    },
        //    error: function (request, error) {

        //    }
        //});

    },
    
    //====================== Search (Apala on 26th May 2017) =======================
    SearchBuilder: function () {
        var SearchText = $("#txtSearchBuilder").val().trim().toLowerCase();

        $("div.market-builder").each(function () {
            $(this).css('background-color', '#ffffff');
        });

        $("div.market-builder").each(function () {
            var BuilderName = $(this).data('builder-name').toLowerCase().trim();

            if (BuilderName.indexOf(SearchText) >= 0)
            {
                $(this).css('background-color', 'yellow');
                $("#DivChildMarketBuilder :first-child").scrollTo($(this));
                return;
            }

            var arrBuilderName = $(this).data('builder-name').split(' ');

            for (i = 0; i < arrBuilderName.length; i++) {
                if (arrBuilderName[i].trim().toLowerCase() == SearchText) {
                    $(this).css('background-color', 'yellow');
                    $("#DivChildMarketBuilder :first-child").scrollTo($(this));
                    break;
                }
            }
        });
    },

    SearchResource: function () {
        var SearchText = $("#txtSearchResource").val().trim().toLowerCase();

        $("tbody#listViewContractResource tr").each(function () {
            $(this).find('td:eq(0)').find('div:eq(0)').css('background-color', '#ffffff');
        });

        $("tbody#listViewContractResource tr").each(function () {
            var ResourceText = $(this).find('td:eq(0)').find('div:eq(0)').html().trim();
            ResourceText = ResourceText.replace(/,/g, ' ').replace(/\//g, ' ');
            var arrResourceName = ResourceText.split(' ');

            for (i = 0; i < arrResourceName.length; i++) {
                if (arrResourceName[i].trim().toLowerCase().trim() == SearchText) {
                    $(this).find('td:eq(0)').find('div:eq(0)').css('background-color', 'yellow');
                    $(window).scrollTo($(this));
                    break;
                }
            }
        });
    }
}

var ProductContractView = {
    AssignControl: function () {
        $("#BtnSaveContractProduct").on("click", ProductContractView.UpdateContractProductToDb);
        $("#BtnModifyContractProduct").on("click", ProductContractView.OpenContractProductPopup);
        $("#BtnProductViewDetails").on("click", ProductContractView.OpenProductDetailsPopup);
        //console.log('afascccc');
    },
    UpdateContractProductToDb: function () {
        //  kendo.ui.progress($("#ContractProduct"), false);

        var SelectedProduct = $("input[name='ChkContractProductSelect']");
        var Products = [];
        var ProductList = '';

        if (SelectedProduct.length > 0) {
            $(SelectedProduct).each(function () {
                if (!$(this).is(':checked')) {
                    Products.push({ ContractId: $("#HdnContractId").val(), ProductId: $(this).siblings().eq(0).val() });
                }
            });
        }
        else {
            return;
        }


        var postData = { ObjContractProduct: Products }; //done
        $.ajax({ //Process the form using $.ajax()
            type: 'POST', //Method type
            url: AjaxCallUrl.UpdateContractProductPopupUrl,
            data: postData, //Forms name
            dataType: 'json',
            // traditional: true,
            beforeSend: function () {
            },
            success: function (data) {
                if (data.IsSuccess) {
                    kendo.ui.progress($("#ContractProduct"), false);
                    var DataUpdateWindow = $("#DataUpdateMessage").data("kendoWindow");

                    ProductContractView.LoadContractProduct();
                    if (DataUpdateWindow) {
                        DataUpdateWindow.title("Update Information");
                        DataUpdateWindow.content("<div class='msgboxSuccess'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i> Data Update Successfully</div>");
                        DataUpdateWindow.open();
                        //redirect to listing page
                    }
                }
            },
            error: function (request, error) {
                kendo.ui.progress($("#ContractProduct"), false);
                if (DataUpdateWindow) {
                    DataUpdateWindow.title("Error Information");
                    DataUpdateWindow.content("<div class='msgboxError'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i>" + data.ModelError + " </div>");
                    DataUpdateWindow.open();
                }
            }
        });


    },
    LoadContractProduct: function () {

        // console.log('fff');
        var FormData = {
            'ContractId': $("#HdnContractId").val()
            //Store name fields value
        };

        kendo.ui.progress($("#ContractProduct"), true);

        $.ajax({ //Process the form using $.ajax()
            type: 'GET', //Method type
            url: AjaxCallUrl.LoadContractProductUrl,
            data: FormData, //Forms name
            beforeSend: function () {
            },
            error: function (request, error) {
                kendo.ui.progress($("#ContractProduct"), false);
            },
            success: function (data) {
                if (data.IsSuccess) {
                    $("#DivChildProduct").html(data.PartialView);
                    ProductContractView.AssignControl();
                }
                kendo.ui.progress($("#ContractProduct"), false);
            }
        });
    },
    OpenContractProductPopup: function () {
        var wnd = $("#WndAddContractProduct").data("kendoWindow");
        wnd.refresh({
            url: AjaxCallUrl.LoadAddContractProductPopupUrl, // returns JSON, { firstName: "Alyx", lastName: "Vance" }
            data: {},
        });
        wnd.open().center();
    },
    AddProductContractAssignControl: function () {
        $("#BtnAddContractProduct").on("click", ProductContractView.AddProductContractToDb);
        $("#BtnCancelContractProduct").on("click", ProductContractView.CancelProductContractToDb);

        $("#DivProductCategoryDropdown").on("click", function () {
            CustomControl($(this).parent().parent(), "CustomControlPoup", $(this).attr('id'));
            // event.stopPropagation();
        });

        $("#DivProductDropdown").on("click", function () {
            if ($('.CustomControl-Div').children('div[id ^= "divCategorySelected_"]').length == 0) {
            }
            else {
                //  $("#DivProduct").attr('disabled', false);
                CustomControlProduct($(this).parent().parent(), "CustomControlPoupProduct");
            }
            // event.stopPropagation();
        });

        if ($("#WndAddContractProduct").length) {

            $('#WndAddContractProduct').on('click', function () {
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
    },

    AddProductContractToDb: function () {

        /*Added by Rabi on 29th march*/

        var Validator = $("#DivAddContractProduct").kendoValidator({
            messages: {
                validproduct: "Please enter product in correct format",
                required: "*",
            },

            rules: {
                validproduct: function (input) {
                    var validate = input.data('productlist');
                    // if the input has a `data-available` attribute...
                    if (typeof validate !== 'undefined' && validate !== false) {
                        var ProductListArray = input.val().split(",");
                        for (var i = 0; i < ProductListArray.length; i++) {
                            if (ProductListArray[i].trim() == "" || ProductListArray[i] == null || ProductListArray[i] == undefined) {
                                return false;
                            }
                        }
                        return true;
                    }
                    return true;
                },
                required: function (input) {
                    if (input.is("[required=required]")) {
                        return input.val().length > 0;
                    }
                    return false;
                }
            }
        }).data("kendoValidator");
        var IsValid = Validator.validate();
        if (!IsValid) {
            return;
        }

        //end

        kendo.ui.progress($("#DivAddContractProduct"), false);

        /* Added by Rabi on 29th march
        var SelectedProduct = $("#ProductList").children().find("input[name='ChkProductSelect']:checked");
        var Products = [];
        var ProductList = '';
        $(SelectedProduct).each(function () {
            Products.push({ ContractId: $("#HdnContractId").val(), ProductId: $(this).siblings().eq(0).val() });
        });
        */

        // ProductList = Products.join();

        var postData = { ProductList: $("#ProductList").val(), ContractId: $("#HdnContractId").val() }; //done
        $.ajax({ //Process the form using $.ajax()
            type: 'POST', //Method type
            url: AjaxCallUrl.AddContractProductPopupUrl,
            data: postData, //Forms name
            dataType: 'json',
            // traditional: true,
            beforeSend: function () {
            },
            success: function (data) {
                if (data.IsSuccess) {
                    kendo.ui.progress($("#DivAddContractProduct"), false);
                    var DataUpdateWindow = $("#DataUpdateMessage").data("kendoWindow");

                    $("#WndAddContractProduct").data("kendoWindow").close();
                    ProductContractView.LoadContractProduct();
                    if (DataUpdateWindow) {
                        DataUpdateWindow.title("Update Information");
                        DataUpdateWindow.content("<div class='msgboxSuccess'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i> Data Update Successfully</div>");
                        DataUpdateWindow.open();
                        //redirect to listing page
                    }
                    else {
                        DataUpdateWindow.title("Error Information");
                        DataUpdateWindow.content("<div class='msgboxSuccess'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i>" + data.ModelError + "</div>");
                        DataUpdateWindow.open();
                    }
                }
            },
            error: function (request, error) {
                kendo.ui.progress($("#DivAddContractProduct"), false);
                if (DataUpdateWindow) {
                    DataUpdateWindow.title("Error Information");
                    DataUpdateWindow.content("<div class='msgboxError'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i>" + data.ModelError + " </div>");
                    DataUpdateWindow.open();
                }
            }
        });



    },
    CancelProductContractToDb: function () {
        var DataUpdateWindow = $("#WndAddContractProduct").data("kendoWindow");
        if (DataUpdateWindow) {
            DataUpdateWindow.close();
        }
        // $("#WndAddContractProduct").data("kendoWindow").close();
    },
    OpenProductDetailsPopup: function () {

        // console.log('ggggg');

        var wnd = $("#WndProductViewDetails").data("kendoWindow");
        wnd.refresh({
            url: AjaxCallUrl.OpenProductDetailsPopupUrl, // returns JSON, { firstName: "Alyx", lastName: "Vance" }
            data: { ContractId: $("#HdnContractId").val() }
        });
        wnd.open().center();
    },
    AssignControlViewdetailsPopup: function () {
        $("#DownloadProductDetials").on("click", ProductContractView.DownlaodProductDetails);
    },
    DownlaodProductDetails: function () {


        //var postData = {

        //    ContractId: $("#HdnContractId").val()
        //}; //done
        //// console.log(JSON.stringify(ArrayProductCategory));
        //$.ajax({ //Process the form using $.ajax()
        //    type: 'POST', //Method type
        //    url: AjaxCallUrl.DownloadContractProductDetailsUrl,
        //    data: postData, //Forms name
        //    dataType: 'json',
        //    // traditional: true,
        //    beforeSend: function () {
        //    },
        //    success: function (data) {
        //        // $("#ProductList").html(data.ProductCustomControl);
        //        //  AssignControlEvent();

        //    },
        //    error: function (request, error) {

        //    }
        //});

        var ContractId = $("#HdnContractId").val();
        var src = AjaxCallUrl.DownloadContractProductDetailsUrl + '?ContractId=' + ContractId;
        //   console.log(src);

        var iframe = $("<iframe/>").load(function () {
            // $.unblockUI();
        }).attr({
            src: src
        }).appendTo($("#DivIframeContainer"));


    }
}

var ContractRebateOverride = {
    AssignControl: function () {
        $("#SaveContractRebateOverride").on("click", ContractRebateOverride.SaveReabateOverrid);
        $("#CancelContractRebateOverride").on("click", ContractRebateOverride.CancelRebateOverride);
        $("#DivZoneStateDropdown").on("click", ContractRebateOverride.OpenProductControl);
        // $("#TxtOverrideRebate").kendoNumericTextBox();

        if ($("#DivOverrideRebate").length) {
            $('html').on('click', function () {
                // do your stuff here
                var ChildDiv = $("#DivZoneState").children(".CustomControlPoupZoneState");

                if ($(ChildDiv).is(':visible')) {
                    $(ChildDiv).hide();
                    if ($("#HdnZoneSetting").length) {
                        if ($("#HdnZoneSetting").val() == "1") {




                            var Validator = $("#DivOverrideRebate").kendoValidator({
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

                                },
                            }).data("kendoValidator");
                            var Isvalid = Validator.validateInput($("#DivZoneState"));
                            if (Isvalid) {
                                var DdlBuilder = $("#DdlBuilder").data("kendoDropDownList");
                                DdlBuilder.dataSource.read();
                            }
                            else {
                                //  var DdlBuilder = $("#DdlBuilder").data("kendoDropDownList");

                                // DdlBuilder.dataSource.data({});
                            }

                        }
                    }

                }
            }).find('.classZoneMarket').on('click', function (e) {
                e.stopPropagation();
            });
        }


    },
    SaveReabateOverrid: function () {        
        var Validator = $("#DivOverrideRebate").kendoValidator({
            messages: {
                required: "*",
                greaterthanzero: "Must be greater than zero",
                ZoneSate: function (input) {
                    return "Atleast one market selection is required";
                }
            },
            rules: {
                required: function (input) {
                    if (input.is("[required=required]")) {
                        return input.val().length > 0;
                    }
                    return true;
                },
                greaterthanzero: function (input) {
                    var validate = input.data('greaterthanzero');
                    // if the input has a `data-available` attribute...
                    if (typeof validate !== 'undefined' && validate !== false && input.val() != "") {

                        return parseFloat(input.val()) > 0;
                    }
                    return true;

                },
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

            },
        }).data("kendoValidator");

        var ReturnTxtCltr = Validator.validateInput($("#TxtOverrideRebate"));
        var ReturnZneCltr = Validator.validateInput($("#DivZoneState"));
        //console.log(ReturnTxtCltr);
        // console.log(ReturnZneCltr);

        if (ReturnTxtCltr == true && ReturnZneCltr == true) {

            kendo.ui.progress($("#DivOverrideRebate"), false);
            var dropdownlist = $("#DdlBuilder").data("kendoDropDownList");
            var postData = { ContractId: $("#HdnContractId").val(), BuilderId: dropdownlist.value(), RebatePercentage: $("#TxtOverrideRebate").val() }; //done
            console.log(postData);

            $.ajax({ //Process the form using $.ajax()
                type: 'POST', //Method type
                url: AjaxCallUrl.SaveContractBulderRebateUrl,
                data: postData, //Forms name
                dataType: 'json',
                // traditional: true,
                beforeSend: function () {
                    
                },
                success: function (data) {
                    if (data.IsSuccess) {
                        kendo.ui.progress($("#DivOverrideRebate"), false);
                        var DataUpdateWindow = $("#DataUpdateMessage").data("kendoWindow");

                        $("#SpanLastUpdated").text(data.LastUpdate);
                        $("#SpanApplicableRebeatToday").text(data.ApplicableRebateToday);




                        if (DataUpdateWindow) {
                            DataUpdateWindow.title("Update Information");
                            DataUpdateWindow.content("<div class='msgboxSuccess'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i> Data Update Successfully</div>");
                            DataUpdateWindow.open();
                            //redirect to listing page
                        }
                    }
                },
                error: function (request, error) {
                    kendo.ui.progress($("#DivOverrideRebate"), false);
                    if (DataUpdateWindow) {
                        DataUpdateWindow.title("Error Information");
                        DataUpdateWindow.content("<div class='msgboxError'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i>" + data.ModelError + " </div>");
                        DataUpdateWindow.open();
                    }
                }
            });
        }
    },
    CancelRebateOverride: function () {
        var DataUpdateWindow = $("#WndContractRebateOverride").data("kendoWindow");
        if (DataUpdateWindow) {

            DataUpdateWindow.close();
        }
    },
    OpenProductControl: function () {
        CustomControlZoneStateList($(this).parent().parent(), "CustomControlPoupZoneState");
    },
    GetBuilderRebateInformation: function (BuilderId, ContractId) {

        var FormData = {
            'BuilderId': BuilderId,
            'ContractId': ContractId

            //Store name fields value
        };

        kendo.ui.progress($("#DivOverrideRebate"), true);

        $.ajax({ //Process the form using $.ajax()
            type: 'GET', //Method type
            url: AjaxCallUrl.BuilderRebateInformationUrl,
            data: FormData, //Forms name
            beforeSend: function () {
            },
            error: function (request, error) {
                kendo.ui.progress($("#DivOverrideRebate"), false);
            },
            success: function (data) {
                if (data.IsSuccess) {

                    $("#SpanLastUpdated").text(data.LastUpdate);
                    $("#SpanApplicableRebeatToday").text(data.ApplicableRebateToday);
                    $("#SpanApplicableRebeat").text(data.ApplicableRebate);
                    $("#SpanContractStatus").text(data.ContractStatusWhenJoin);
                    $("#SpanJoinOn").text(data.ContractJoinOn);



                }
                kendo.ui.progress($("#DivOverrideRebate"), false);
            }
        });
    }
}


var ContractRebate = {
    AssignControl: function () {

        $("#SaveContractRebate").on("click", ContractRebate.ContractReabteSave);
        $("#a_override_rebate").on("click", ContractRebate.ContractOverideRebate);
        $("#CancelContractRebate").on("click", ContractRebate.ContractReabtecancel);
        $("input[name='TxtRebateValue']").kendoNumericTextBox({
            min: 0.00

        });

    },

    ContractReabteSave: function () {
        kendo.ui.progress($("#DivManageContractStatus"), false);
        var postData = { ContractStatusName: $("#ContractStatusName").val() }; //done


        var HdnRebateStatus = $("input[name='HdnRebateStatus']");
        // console.log(HdnRebateStatus.length);
        var StatusRebateList = [];
        $(HdnRebateStatus).each(function () {
            if (parseFloat($(this).siblings().find("input[name='TxtRebateValue']").val()) >= 0) {
                StatusRebateList.push({ ContractId: $("#HdnContractId").val(), ContractStatusId: $(this).val(), RebatePercentage: $(this).siblings().find("input[name='TxtRebateValue']").val() });
            }
        });

        //console.log(StatusRebateList);

        // { StatusRebateList: StatusRebateList }
        $.ajax({ //Process the form using $.ajax()
            type: 'POST', //Method type
            url: AjaxCallUrl.SaveContractRebateUrl,
            data: { StatusRebateList: StatusRebateList }, //Forms name
            dataType: 'json',
            // traditional: true,
            beforeSend: function () {
            },
            success: function (data) {
                var DataUpdateWindow = $("#DataUpdateMessage").data("kendoWindow");
                if (data.IsSuccess) {
                    kendo.ui.progress($("#DivManageContractStatus"), false);
                    if (DataUpdateWindow) {

                        DataUpdateWindow.title("Update Information");
                        DataUpdateWindow.content("<div class='msgboxSuccess'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i> Data Update Successfully</div>");
                        DataUpdateWindow.open();
                    }

                }
            },
            error: function (request, error) {
                kendo.ui.progress($("#DivManageContractStatus"), false);
            }
        });
    },
    ContractReabtecancel: function () {

        // console.log('cancel');
        var DataUpdateWindow = $("#WndContractRebate").data("kendoWindow");
        if (DataUpdateWindow) {

            DataUpdateWindow.close();
        }


    },
    ContractOverideRebate: function () {
        // console.log('overriderebate');
        var wnd = $("#WndContractRebateOverride").data("kendoWindow");
        wnd.refresh({
            url: AjaxCallUrl.LoadContractRebateOverrideUrl, // returns JSON, { firstName: "Alyx", lastName: "Vance" }
            data: {},
        });
        wnd.open().center();
    }
}


var ContractInformation = {
    //View:function()
    //{

    //}
    Save: function () {

        kendo.ui.progress($("#DivEditContractGenaralInformation"), true);
        // var Markets = [];
        var fileUpload = $("input[name*='ContractLogo']").get(0);
        var files = fileUpload.files;
        var fileData = new FormData();

        // Looping over all files and add it to FormData object  
        for (var i = 0; i < files.length; i++) {
            fileData.append(files[i].name, files[i]);
        }


        fileData.append('ContractId', $("#HdnContractId").val());
        fileData.append('ContractStatusId', $("#ContractStatusId").data("kendoDropDownList").value());
        fileData.append('ContractName', $("#ContractName").val());
        fileData.append('Label', $("#Label").val());
        fileData.append('EstimatedStartDate', $("#EstimatedStartDate").val());
        fileData.append('EntryDeadline', $("#EntryDeadline").val());
        fileData.append('ContractFrom', $("#ContractFrom").val());
        fileData.append('ContractTo', $("#ContractTo").val());
        fileData.append('ContractDeliverables', $("#ContractDeliverables").val());
        fileData.append('PrimaryManufacturer', $("#PrimaryManufacturer").val());
        fileData.append('ManufacturerId', $("#ManufacturerId").data("kendoDropDownList").value());
        fileData.append('Website', $("#Website").val());
        //  fileData.append('Products', ProductList);

        /// fileData.append('DumpId', $("#HdnDumpId").val());




        // console.log(JSON.stringify(ArrayProductCategory));
        $.ajax({ //Process the form using $.ajax()
            type: 'POST', //Method type
            url: AjaxCallUrl.EditContractDetails,
            data: fileData, //Forms name
            dataType: 'json',
            // traditional: true,
            contentType: false, // Not to set any content header  
            processData: false,
            beforeSend: function () {
            },
            success: function (data) {
                // $("#ProductList").html(data.ProductCustomControl);
                //  AssignControlEvent();
                var DataUpdateWindow = $("#DataUpdateMessage").data("kendoWindow");
                if (data.IsSuccess) {

                    //  console.log(data.LogoImageBase64);

                    if (data.LogoImageBase64 != "") {
                        // console.log(data.LogoImageBase64);
                        $("#ContractLogoImage").attr("src", data.LogoImageBase64);
                    }

                    kendo.ui.progress($("#DivEditContractGenaralInformation"), false);

                    if (DataUpdateWindow) {
                        DataUpdateWindow.title("Update Information");
                        DataUpdateWindow.content("<div class='msgboxSuccess'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i> Data Update Successfully</div>");
                        DataUpdateWindow.open();
                        //redirect to listing page
                        var url = AjaxCallUrl.ManageContractPageUrl;
                        setTimeout(function () {
                            window.location.href = url;
                        }, 1000);

                    }
                }
                else {
                    kendo.ui.progress($("#DivEditContractGenaralInformation"), false);
                    if (DataUpdateWindow) {
                        DataUpdateWindow.title("Error Information");
                        DataUpdateWindow.content("<div class='msgboxError'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i>" + data.ModelError + " </div>");
                        DataUpdateWindow.open();
                    }
                }
            },
            error: function (request, error) {
                kendo.ui.progress($("#CreateContract"), false);
            }
        });
    }
}

//var GenaralContractInformation = {
//    View: function () {
//        $.ajax({ //Process the form using $.ajax()
//            type: 'GET', //Method type
//            url: AjaxCallUrl.ViewContractGenaralInformationUrl,
//            data: FormData, //Forms name
//            dataType: 'json',
//            beforeSend: function () {
//            },
//            error: function (request, error) {
//            },
//            success: function (data) {
//                if (data.IsSuccess) {
//                    $("#DivMainContractGenaralInformation").html(data.PartialViewString);
//                }
//            }
//        });
//    }
//}

var ContractMarketBuilders = {

    View: function () {
        // console.log('ffff333');
        var FormData = {
            // 'FacilityId': $("#hdn_Facility").val()        //Store name fields value
        };

        $.ajax({ //Process the form using $.ajax()
            type: 'GET', //Method type
            url: AjaxCallUrl.ViewMarketBuildersUrl,
            data: FormData, //Forms name
            dataType: 'json',
            beforeSend: function () {
            },
            error: function (request, error) {
            },
            success: function (data) {
                if (data.IsSuccess) {
                    //$("#DivMainContractGenaralInformation").html(data.PartialViewString);
                }
            }
        });
    }

}

var ContractProduct = {
    
    View: function () {
        //console.log('ffff111');
        var FormData = {
            // 'FacilityId': $("#hdn_Facility").val()        //Store name fields value
        };

        $.ajax({ //Process the form using $.ajax()
            type: 'GET', //Method type
            url: AjaxCallUrl.ViewProductnUrl,
            data: FormData, //Forms name
            dataType: 'json',
            beforeSend: function () {
            },
            error: function (request, error) {
            },
            success: function (data) {
                if (data.IsSuccess) {
                    // $("#DivMainContractGenaralInformation").html(data.PartialViewString);
                }
            }
        });
    }
};

$(document).ready(function () {
    var MessageCache = {};
    var SpecialValiadateElement = ["EstimatedStartDate", "EntryDeadline", "ContractFrom", "ContractTo", "ContractDeliverables"];

    $("#SaveContract").bind("click", function () {
        var Validator = $("#DivEditContractGenaralInformation").kendoValidator({
            messages: {
                mvcdate: "please enter correct date",
                custom: "Please enter valid value for my custom rule",
                required: "*",
                email: function (input) {
                    return getMessage(input);
                },
                checkcontractname: function (input) {
                    return input.val() + " already exist";
                },
                checkcontractlabel: function (input) {
                    return " already exist";
                },
                date: "Not a valid date",
                futuredate: "Cannot be a past date or current date",
                lesserdate: "Invalid start Date",
                greaterdate: "Invalid End Date",
                lesserdateBirdEntryDeadline: "Invalid estimated start date",
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
                //mvcdate: function (input) {

                //    var id = input.attr('id');
                //    console.log(id);
                //    return input.val() === "" || kendo.parseDate(input.val(), "MM/dd/yyyy") !== null;
                //}
                required: function (input) {
                    if (input.is("[required=required]") && !input.is(":hidden")) {
                        var dropdownlist = $("#ContractStatusId").data("kendoDropDownList");
                        var id = input.attr('id');
                        //  console.log(id);
                        if (dropdownlist.value() != "1" && (id == "EstimatedStartDate" || id == "EntryDeadline" || id == "ContractName" || id == "Label")) {
                            // console.log(input.val().length);
                            return input.val().length > 0;
                        }
                        else if (dropdownlist.value() == "1" && (id == "ContractFrom" || id == "ContractTo" || id == "ContractDeliverables" || id == "ContractName" || id == "Label")) {
                            // console.log(id);
                            return input.val().length > 0;
                        }
                        else if (id == "PrimaryManufacturer") {
                            // console.log(dropdownlist.value());
                            if (dropdownlist.value() != "1") {
                                return input.val().length > 0;
                            }
                            else {
                                return true;
                            }

                        }
                        else {
                            if (SpecialValiadateElement.indexOf(id) < 0) {
                                return input.val().length > 0;
                            }
                            else {
                                return true;
                            }


                        }

                    }
                    return true;
                },
                mvcdate: function (input) {

                    if (input.is("[data-role=datepicker]")) {

                        var dropdownlist = $("#ContractStatusId").data("kendoDropDownList");
                        var id = input.attr('id');
                        if (dropdownlist.value() != "1" && (id == "EstimatedStartDate" || id == "EntryDeadline")) {
                            var d = kendo.parseDate(input.val(), "MM/dd/yyyy");
                            return d instanceof Date;
                        }
                        else if (dropdownlist.value() == "1" && (id == "ContractFrom" || id == "ContractTo")) {
                            var d = kendo.parseDate(input.val(), "MM/dd/yyyy");
                            return d instanceof Date;
                        }


                        return true;


                    }

                    return true;
                },
                futuredate: function (input) {
                    var validate = input.data('futuredate');
                    // if the input has a `data-available` attribute...
                    if (typeof validate !== 'undefined' && validate !== false && input.val() != "") {

                        var dropdownlist = $("#ContractStatusId").data("kendoDropDownList");
                        var id = input.attr('id');
                        if (dropdownlist.value() != "1" && (id == "EstimatedStartDate" || id == "EntryDeadline")) {
                            // var date = kendo.parseDate(input.val());
                            //   var TodayDate = kendo.parseDate(new Date());
                            //   return date > TodayDate

                            var date = kendo.parseDate(input.val());
                            var CurrentDate = new Date();
                            var TodayDate = kendo.parseDate(new Date(CurrentDate.getFullYear(), CurrentDate.getMonth(), CurrentDate.getDate()));
                            return date >= TodayDate
                        }
                        else if (dropdownlist.value() == "1" && (id == "ContractTo")) {
                            //  var date = kendo.parseDate(input.val());
                            //  var TodayDate = kendo.parseDate(new Date());
                            //  return date > TodayDate
                            var date = kendo.parseDate(input.val());
                            var CurrentDate = new Date();
                            var TodayDate = kendo.parseDate(new Date(CurrentDate.getFullYear(), CurrentDate.getMonth(), CurrentDate.getDate()));
                            return date >= TodayDate
                        }
                    }
                    return true;
                },
                lesserdate: function (input) {
                    var validate = input.data('lesserdate');

                    var id = input.attr('id');

                    if (typeof validate !== 'undefined' && validate !== false && input.val() != "") {
                        var dropdownlist = $("#ContractStatusId").data("kendoDropDownList");
                        if (dropdownlist.value() == "1") {

                            var date = kendo.parseDate(input.val()),
                                otherDate = kendo.parseDate($("[name='" + input.data("greaterfield") + "']").val());
                            return otherDate > date;
                        }
                        return true;
                    }
                    return true;

                },
                greaterdate: function (input) {
                    var validate = input.data('greaterdate');

                    var id = input.attr('id');

                    // if the input has a `data-available` attribute...
                    if (typeof validate !== 'undefined' && validate !== false && input.val() != "") {
                        //  console.log('hhh');
                        var dropdownlist = $("#ContractStatusId").data("kendoDropDownList");
                        if (dropdownlist.value() == "1") {
                            var date = kendo.parseDate(input.val()),
                            otherDate = kendo.parseDate($("[name='" + input.data("lesserfield") + "']").val());
                            return date > otherDate;
                        }
                        return true;


                    }
                    return true;
                },
                lesserdateBirdEntryDeadline: function (input) {
                    var validate = input.data('lesserdatebirdentrydeadline');
                    if (typeof validate !== 'undefined' && validate !== false && input.val() != "") {
                        //  console.log('hhh');
                        var dropdownlist = $("#ContractStatusId").data("kendoDropDownList");
                        if (dropdownlist.value() != "1") {
                            var date = kendo.parseDate(input.val()),
                            otherDate = kendo.parseDate($("[name='" + input.data("greaterfield") + "']").val());
                            return date < otherDate;
                        }

                        return true;


                    }
                    return true;

                    // var id = input.attr('id');
                },
                validurl: function (input) {
                    var validate = input.data('validurl');
                    // if the input has a `data-available` attribute...
                    if (typeof validate !== 'undefined' && validate !== false) {

                        var UrlList = [];
                        UrlList = input.val().split(",");
                        var dropdownlist = $("#ContractStatusId").data("kendoDropDownList");

                        if (dropdownlist.value() == "1") { ///Active Contract
                            //console.log("jjj");
                            if (UrlList.length != 1) {
                                MessageCache.IsWebsiteCount = true;
                                return false;
                            }

                        }



                        MessageCache.IsWebsiteCount = false;
                        var Flag = 0;
                        //var regex = new RegExp("(https?:\/\/(?:www\.|(?!www))[^\s\.]+\.[^\s]{2,}|www\.[^\s]+\.[^\s]{2,})");
                        var regex = new RegExp(/(http(s)?:\/\/.)?(www\.)?[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)/g);           //it accepts with http without http, with www and without www
                        for (var i = 0; i < UrlList.length; i++) {
                            //  console.log(UrlList[i]);
                            if (regex.test(UrlList[i])) {
                                // alert("Successful match");
                            } else {
                                //alert("No match");
                                // console.log('rrrrr');
                                Flag = 1;
                            }
                        }
                        if (Flag == 1) {
                            return false;
                        }
                        else {
                            return true;
                        }
                    }
                    return true;
                },
                checkcontractname: function (input) {
                    var validate = input.data('contractnameavailable');
                    // if the input has a `data-available` attribute...
                    if (typeof validate !== 'undefined' && validate !== false) {

                        //  console.log($("#HdnValidationContractName").val());

                        if ($("#HdnValidationContractName").val() == "1") {
                            return true;
                        }
                        else {
                            return false;
                        }
                    }
                    return true;
                },

                checkcontractlabel: function (input) {
                    var validate = input.data('contractlabelavailable');
                    // if the input has a `data-available` attribute...
                    if (typeof validate !== 'undefined' && validate !== false) {
                        if ($("#HdnValidationContractLabel").val() == "1") {
                            return true;
                        }
                        else {
                            return false;
                        }
                    }
                    return true;
                }

            }
        }).data("kendoValidator");

        //  console.log(Validator.validate());

        if (Validator.validate()) {
            ContractInformation.Save();


        }
        else {

        }

    });

    AssignFoucusoutEvent();
});


function onSelectContractStatus(e) {
    
    var dataItem = this.dataItem(e.item.index());
    
    if (dataItem.ContractStatusId == "1") {
        $("div[data-role='Active']").show();
        $("div[data-role='Pending']").hide();
        $("label[data-role='Active']").show();
        $("label[data-role='Pending']").hide();
    }
    else {
        $("div[data-role='Active']").hide();
        $("div[data-role='Pending']").show();
        $("label[data-role='Active']").hide();
        $("label[data-role='Pending']").show();
    }
}

///Contract Image
function onSelect(e) {

    var files = e.files;

    $.each(files, function () {
        //   console.log(this.extension.toLowerCase());

        if (this.extension.toLowerCase() != ".png" && this.extension.toLowerCase() != ".jpg" && this.extension.toLowerCase() != ".jpeg") {
            alert("Only .png/.jpg/.jpeg files can be uploaded!")
            e.preventDefault();
        }
        if (this.size / 1024 / 1024 > 5) {
            alert("Max 5Mb file size is allowed!")
            e.preventDefault();
        }
    });
}

function ViewContractSurvey(SurveyId, ContractId) {
    var url = AjaxCallUrl.PreviewSurveyUrl.replace("****id****", SurveyId);
    var win = window.open(url, '_blank');
    win.focus();
}

function EditContractSurvey(SurveyId, ContractId, IsNcpSurvey) {
    if (parseInt(SurveyId) > 0) {

        if (IsNcpSurvey.toLowerCase() === "true") {
            var url = AjaxCallUrl.RedirectSurveyNcpEditUrl.replace("****SurveyId****", SurveyId).replace("****IsNcpId****", "1");
            window.location.href = url;
        }
        else {
            var url = AjaxCallUrl.RedirectSurveyEditUrl.replace("****SurveyId****", SurveyId);
            window.location.href = url;
        }


    }
}

function Deploycontractsurvey(SurveyId, ContractId) {
    if (parseInt(SurveyId) > 0) {
        var url = AjaxCallUrl.RedirectSurveyDeployUrl.replace("****SurveyId****", SurveyId);
        window.location.href = url;
    }
}

function CopyContractSurvey(SurveyId, ContractId, IsNcpSurvey) {

    var Confirm = confirm("Are you sure you want copy the survey?");

    if (!Confirm) {
        return;
    }
    var postData = '';
    // kendo.ui.progress(".main-body-wrapper", false);
    if (IsNcpSurvey.toLowerCase() === "true") {
        postData = { ContractId: ContractId, SurveyId: SurveyId, IsNcp: 'true' };
    }
    else {
        postData = { ContractId: ContractId, SurveyId: SurveyId };
    }

    //done
    $.ajax({ //Process the form using $.ajax()
        type: 'POST', //Method type
        url: AjaxCallUrl.CopySurvey,
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

                    var KendoListView = $("#listViewContractSurvey").data("kendoListView");
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

function numericFilter(txb) {
    txb.value = txb.value.replace(/[^\0-9]/ig, "");
}

function AssignFoucusoutEvent() {

    $("#ContractName").on("focusout", function () {



        var Flag = 1;
        var availability = {
            cache: {},
            check: function (element, settings) {

                var id = element.attr('id');
                var cache = this.cache[id] = this.cache[id] || {};
                // console.log(Flag);
                if (Flag == 1) {
                    Flag = 2;
                    $.ajax({
                        url: AjaxCallUrl.CheckContractNameUrl,
                        dataType: 'json',
                        data: { ContractName: element.val() },
                        beforeSend: function (xhr, opts) {
                            // console.log(element.data('previous'));

                            if (element.data('previous').trim().toLowerCase() == element.val().toLowerCase()) {
                                xhr.abort();
                                cache.valid = true;
                                $("#HdnValidationContractName").val("1");
                                cache.value = element.val();
                                Validator.validateInput(element);

                            }

                        },
                        success: function (data) {

                            //  console.log(data);
                            // the `data` object returns true or false
                            // based on the availability of the value
                            // set the value on the cache object so 
                            // that it can be referenced in the next validation run

                            cache.valid = data.IsContractNameAvailable;

                            // trigger validation again
                            cache.value = element.val();

                            if (data.IsContractNameAvailable) {
                                $("#HdnValidationContractName").val("1");
                            }
                            else {
                                $("#HdnValidationContractName").val("0");
                            }
                            //  return true;


                            Validator.validateInput(element);
                        },
                        failure: function () {
                            // console.log('err');
                            // the ajax call failed so just set the field
                            // as valid since we don't know for sure that it's not
                            cache.valid = true;
                            // trigger validation
                            Validator.validateInput(element);
                        },
                        complete: function () {
                            // cache the inputs value
                            //Flag = 1;

                        }
                    });
                }

            }

        };


        var Validator = $("#TdContractName").kendoValidator({
            messages: {
                available: function (input) {
                    //    console.log('msg');
                    var id = input.attr('id');
                    //  var msg = 'Atleast one market selection is required';
                    var cache = availability.cache[id];

                    if (cache.checking) {
                        return "Checking..."
                    }
                    else {
                        return input.val() + " already exist";
                    }
                },
                required: "*"

            },
            rules: {
                available: function (input) {
                    //  console.log('avail');
                    var validate = input.data('contractnameavailable');
                    // if the input has a `data-available` attribute...
                    if (typeof validate !== 'undefined' && validate !== false) {
                        //  console.log('ddd');
                        // cache the field's id
                        var id = input.attr('id');
                        // console.log(id);

                        // console.log(cache);
                        // new up a new cache object for this field if one doesn't already eist
                        var cache = availability.cache[id] = availability.cache[id] || {};
                        //var cache;

                        //if (typeof availability.cache[id] !== 'undefined') {
                        //    cache = availability.cache[id];
                        //}
                        //else {
                        //    cache = {};
                        //}

                        // set our status to checking
                        cache.checking = true;


                        // pull the url and message off of the proper data attributes
                        var settings = {
                            //url: input.data('availableUrl') || '',
                            //  message: kendo.template(input.data('availableMsg')) || ''
                        };
                        // if the value in the cache and the current input value are the same
                        // and the cached state is valid...

                        // console.log(cache.valid);

                        if (cache.value === input.val() && cache.valid) {
                            // the value is available
                            // console.log('vVValid33');
                            return true;
                        }
                        // if the value in the cache and the input value are the same 
                        // and the cached state is not valid...
                        if (cache.value === input.val() && !cache.valid) {
                            // the value is not available

                            // console.log('INNvalid');
                            cache.checking = false;
                            return false;
                        }
                        // go to the ajax check
                        availability.check(input, settings);
                        // console.log('end...');
                        // return false which goes into 'checking...' mode
                        //   console.log('end...');
                        return false;

                    }
                    // this rule does not apply to the input
                    return true;
                }
            }
        }).data("kendoValidator");

        // console.log('focus out')

        Validator.validateInput($(this));

    });

    $("#Label").on("focusout", function () {

        //  console.log('ddd');

        var Flag = 1;
        var availability = {
            cache: {},
            check: function (element, settings) {

                var id = element.attr('id');
                var cache = this.cache[id] = this.cache[id] || {};
                // console.log(Flag);
                if (Flag == 1) {
                    Flag = 2;
                    $.ajax({
                        url: AjaxCallUrl.CheckContractLabelUrl,
                        dataType: 'json',
                        data: { ContractLabelName: element.val() },
                        beforeSend: function (xhr, opts) {

                            if (element.data('previous').trim().toLowerCase() == element.val().toLowerCase()) {
                                xhr.abort();
                                cache.valid = true;
                                $("#HdnValidationContractLabel").val("1");
                                cache.value = element.val();
                                Validator.validateInput(element);

                            }

                            // console.log('before send');
                        },
                        success: function (data) {

                            //    console.log(data);
                            // the `data` object returns true or false
                            // based on the availability of the value
                            // set the value on the cache object so 
                            // that it can be referenced in the next validation run

                            cache.valid = data.IsContractLabelAvailable;

                            // trigger validation again
                            cache.value = element.val();

                            if (data.IsContractLabelAvailable) {
                                $("#HdnValidationContractLabel").val("1");
                            }
                            else {
                                $("#HdnValidationContractLabel").val("0");
                            }
                            // return true;


                            Validator.validateInput(element);
                        },
                        failure: function () {
                            //   console.log('err');
                            // the ajax call failed so just set the field
                            // as valid since we don't know for sure that it's not
                            cache.valid = true;
                            // trigger validation
                            Validator.validateInput(element);
                        },
                        complete: function () {
                            // cache the inputs value
                            //Flag = 1;

                        }
                    });
                }

            }

        };


        var Validator = $("#TdContractLabel").kendoValidator({
            messages: {
                available: function (input) {
                    //    console.log('msg');
                    var id = input.attr('id');
                    //  var msg = 'Atleast one market selection is required';
                    var cache = availability.cache[id];

                    if (cache.checking) {
                        return "Checking..."
                    }
                    else {
                        return " already exist";
                    }
                }
                ,
                required: "*"

            },
            rules: {
                available: function (input) {
                    //  console.log('avail');
                    var validate = input.data('contractlabelavailable');
                    // if the input has a `data-available` attribute...
                    if (typeof validate !== 'undefined' && validate !== false) {
                        //  console.log('ddd');
                        // cache the field's id
                        var id = input.attr('id');
                        // console.log(id);

                        // console.log(cache);
                        // new up a new cache object for this field if one doesn't already eist
                        var cache = availability.cache[id] = availability.cache[id] || {};
                        //var cache;

                        //if (typeof availability.cache[id] !== 'undefined') {
                        //    cache = availability.cache[id];
                        //}
                        //else {
                        //    cache = {};
                        //}

                        // set our status to checking
                        cache.checking = true;


                        // pull the url and message off of the proper data attributes
                        var settings = {
                            //url: input.data('availableUrl') || '',
                            //  message: kendo.template(input.data('availableMsg')) || ''
                        };
                        // if the value in the cache and the current input value are the same
                        // and the cached state is valid...

                        // console.log(cache.valid);

                        if (cache.value === input.val() && cache.valid) {
                            // the value is available
                            //console.log('vVValid33');
                            return true;
                        }
                        // if the value in the cache and the input value are the same 
                        // and the cached state is not valid...
                        if (cache.value === input.val() && !cache.valid) {
                            // the value is not available

                            // console.log('INNvalid');
                            cache.checking = false;
                            return false;
                        }
                        // go to the ajax check
                        availability.check(input, settings);
                        // console.log('end...');
                        // return false which goes into 'checking...' mode
                        //  console.log('end...');
                        return false;

                    }
                    // this rule does not apply to the input
                    return true;
                }
            }
        }).data("kendoValidator");

        // console.log('focus out')

        Validator.validateInput($(this));

    });

    //  console.log($("td[data-role='Active']").length);
}
