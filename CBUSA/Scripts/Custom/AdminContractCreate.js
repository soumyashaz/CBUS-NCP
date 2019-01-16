/* contract Portion*/
var MainContract = function () {

}

function onSelectContractStatus(e) {
    // console.log(e);
    //  console.log($(this).val());
    // var dataItem = this.dataItem(e.item);
    // kendoConsole.log("event :: select (" + dataItem.text + " : " + dataItem.value + ")");
    //  var dropdownlist = $("#ContractStatus").data("kendoDropDownList");

    // console.log(dropdownlist.value());

    var dataItem = this.dataItem(e.item.index());
    // CCCOnline.changeAccount(dataItem.AccountNumberId);
    // console.log(dataItem.ContractStatusId);

    if (dataItem.ContractStatusId == "1") {
        $("div[data-role='Active']").show();
        $("div[data-role='Pending']").hide();
        $("label[data-role='Pending']").hide();
        $("label[data-role='Active']").show();
    }
    else {
        $("div[data-role='Active']").hide();
        $("div[data-role='Pending']").show();
        $("label[data-role='Pending']").show();
        $("label[data-role='Active']").hide();
    }
}




/* Contract Validation */
$(document).ready(function () {




    var MessageCache = {};
    var SpecialValiadateElement = ["EstimatedStartDate", "EntryDeadline", "ContractFrom", "ContractTo", "ContractDeliverables"];

    $("#SaveContract").bind("click", function () {

        var Validator = $("#CreateContract").kendoValidator({
            messages: {
                validproduct: "Please enter product in correct format",
                mvcdate: "please enter correct date",
                custom: "Please enter valid value for my custom rule",
                required: "*",
                email: function (input) {
                    return getMessage(input);
                },
                date: "Not a valid date",
                futuredate: "Cannot be a past date",
                lesserdate: "Invalid start Date",
                greaterdate: "Invalid End Date",
                lesserdateBirdEntryDeadline: "Invalid estimated start date",
                contractproduct: function (input) {
                    return "Atleast one product selection is required";
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
                required: function (input) {
                    if (input.is("[required=required]")) {
                        var dropdownlist = $("#ContractStatus").data("kendoDropDownList");
                        var id = input.attr('id');
                        //  console.log(id);
                        if (dropdownlist.value() != "1" && (id == "EstimatedStartDate" || id == "EntryDeadline")) {
                            //  console.log(id);
                            return input.val().length > 0;
                        }
                        else if (dropdownlist.value() == "1" && (id == "ContractFrom" || id == "ContractTo" || id == "ContractDeliverables")) {
                            // console.log(id);
                            return input.val().length > 0;
                        }
                        else if (id == "Manufacturer") {
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

                        return true;
                    }
                    return true;
                },
                mvcdate: function (input) {

                    if (input.is("[data-role=datepicker]")) {

                        var dropdownlist = $("#ContractStatus").data("kendoDropDownList");
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

                        var dropdownlist = $("#ContractStatus").data("kendoDropDownList");
                        var id = input.attr('id');
                        if (dropdownlist.value() != "1" && (id == "EstimatedStartDate" || id == "EntryDeadline")) {
                            var date = kendo.parseDate(input.val());
                            var CurrentDate = new Date();
                            var TodayDate = kendo.parseDate(new Date(CurrentDate.getFullYear(), CurrentDate.getMonth(), CurrentDate.getDate()));
                            return date >= TodayDate
                        }
                        else if (dropdownlist.value() == "1" && (id == "ContractTo")) {
                            var date = kendo.parseDate(input.val());
                            var CurrentDate = new Date();
                            var TodayDate = kendo.parseDate(new Date(CurrentDate.getFullYear(), CurrentDate.getMonth(), CurrentDate.getDate()));
                            //  var dd = new Date(2011, 8, 20)
                            return date >= TodayDate
                        }
                    }
                    return true;
                },
                lesserdate: function (input) {
                    var validate = input.data('lesserdate');

                    var id = input.attr('id');

                    // if the input has a `data-available` attribute...
                    if (typeof validate !== 'undefined' && validate !== false && input.val() != "") {
                        var dropdownlist = $("#ContractStatus").data("kendoDropDownList");
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
                        var dropdownlist = $("#ContractStatus").data("kendoDropDownList");
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
                        //   console.log('hhh');
                        var dropdownlist = $("#ContractStatus").data("kendoDropDownList");
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
                checkcontractname: function (input) {
                    var validate = input.data('contractnameavailable');
                    // if the input has a `data-available` attribute...
                    if (typeof validate !== 'undefined' && validate !== false) {
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
                },
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
                },
                validurl: function (input) {
                    var validate = input.data('validurl');
                    // if the input has a `data-available` attribute...
                    if (typeof validate !== 'undefined' && validate !== false) {

                        var UrlList = [];
                        UrlList = input.val().split(",");
                        var dropdownlist = $("#ContractStatus").data("kendoDropDownList");

                        if (dropdownlist.value() == "1") { ///Active Contract
                            //console.log("jjj");
                            if (UrlList.length != 1) {
                                MessageCache.IsWebsiteCount = true;
                                return false;
                            }

                        }



                        MessageCache.IsWebsiteCount = false;
                        var Flag = 0;



                        // var regex = new RegExp("(https?:\/\/(?:www\.|(?!www))[^\s\.]+\.[^\s]{2,}|www\.[^\s]+\.[^\s]{2,})");
                        //  var regex = new RegExp("/(http(s)?:\/\/.)?(www\.)?[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)/g");           //it accepts with http without http, with www and without www
                        //  var regex = new RegExp(https?:\/\/.(?:www\.|(?!www))[^\s\.]+\.[^\s]{2,}|www\.[^\s]+\.[^\s]{2,});
                        var regex = /https?:\/\/(?:www\.|(?!www))[^\s\.]+\.[^\s]{2,}|www\.[^\s]+\.[^\s]{2,}/
                        for (var i = 0; i < UrlList.length; i++) {
                            //  console.log(UrlList[i]);
                            if (regex.test(UrlList[i])) {
                                // alert("Successful match");
                                //  console.log('dd');
                            } else {
                                //alert("No match");
                                //  console.log(UrlList[i]);
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
                }
            }
        }).data("kendoValidator");

        //console.log(Validator.validate());
        // console.log(Validator.validateInput($("#DivProduct")));
        var IsValid = Validator.validate();
        // var IsValid1 = Validator.validateInput($("#DivProduct"));

        //   console.log(IsValid);
        //  console.log(IsValid1);

        if (IsValid) {
            //if (IsProductListvalid($("#ProductList").val())) {
            PostContractDataToServer();
            //}
        }
        else {
            //  console.log('not valid');
        }

    });




    /* Custm control product actegory*/

    //$("#DivProductCategoryDropdown").on("click", function () {
    //    CustomControl($(this).parent().parent(), "CustomControlPoup", $(this).attr('id'));
    //    // event.stopPropagation();
    //});

    /*Close by Rabi on 29 feb

    $("#DivProductDropdown").on("click", function () {
        if ($('.CustomControl-Div').children('div[id ^= "divCategorySelected_"]').length == 0) {
        }
        else {
            //  $("#DivProduct").attr('disabled', false);
            CustomControlProduct($(this).parent().parent(), "CustomControlPoupProduct");
        }
        // event.stopPropagation();
    });*/

});

function getMessage(input) {
    return "Not a valid email";
}

var IsProductListvalid = function (ProductLis) {
    var ProductListArray = ProductLis.split(",");

    for (var i = 0; i < ProductListArray.length; i++) {
        if (ProductListArray[i] == "" || ProductListArray[i] == null || ProductListArray[i] == undefined) {
            return false;
        }
    }

    return true;

}

var PostContractDataToServer = function () {

    kendo.ui.progress($("#CreateContract"), true);

    // var Markets = [];
    var ProductList = '';
    /* var SelectedProduct = $("#ProductList").children().find("input[name='ChkProductSelect']:checked");  //close by Rab
     var Products = [];
   
     $(SelectedProduct).each(function () {
         Products.push($(this).siblings().eq(0).val());
     });
      ProductList = Products.join();
     */

    ProductList = $("#ProductList").val();





    var fileUpload = $("input[name*='ContractLogo']").get(0);
    var files = fileUpload.files;
    var fileData = new FormData();

    // Looping over all files and add it to FormData object  
    for (var i = 0; i < files.length; i++) {
        fileData.append(files[i].name, files[i]);
    }

    // console.log($("#EstimatedStartDate").kendoDatePicker({ parseFormats: ["MM/dd/yyyy"], format: "MM/dd/yyyy" }).data("kendoDatePicker").value());

    fileData.append('ContractStatusId', $("#ContractStatus").data("kendoDropDownList").value());
    fileData.append('ContractName', $("#ContractName").val());
    fileData.append('Label', $("#ContractLabel").val());
    fileData.append('EstimatedStartDate', $("#EstimatedStartDate").val());
    fileData.append('EntryDeadline', $("#EntryDeadline").val());
    fileData.append('ContractFrom', $("#ContractFrom").val());
    fileData.append('ContractTo', $("#ContractTo").val());
    fileData.append('ContractDeliverables', $("#ContractDeliverables").val());
    fileData.append('PrimaryManufacturer', $("#Manufacturer").val());
    fileData.append('ManufacturerId', $("#ContractManufacturer").data("kendoDropDownList").value());
    fileData.append('Website', $("#Website").val());
    fileData.append('Products', ProductList);

    if ($("#HdnDumpId").length) {
        fileData.append('DumpId', $("#HdnDumpId").val());
    }
    else {
        fileData.append('DumpId', "0");
    }






    // console.log(JSON.stringify(ArrayProductCategory));
    $.ajax({ //Process the form using $.ajax()
        type: 'POST', //Method type
        url: AjaxCallUrl.SaveContractlUrl,
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
                kendo.ui.progress($("#CreateContract"), false);

                if (DataUpdateWindow) {

                    DataUpdateWindow.title("Update Information");
                    DataUpdateWindow.content("<div class='msgboxSuccess'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i> Data Update Successfully</div>");
                    DataUpdateWindow.open();
                    //redirect to listing page
                    setTimeout(function () {
                        if ($("#ContractStatus").data("kendoDropDownList").value() == "1")  //Active Contrcat
                        {
                            window.location.href = AjaxCallUrl.RedirectActiveContractUrl;
                        }
                        else {
                            window.location.href = AjaxCallUrl.RedirectPendingontractUrl;
                        }
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
        },
        error: function (request, error) {
            kendo.ui.progress($("#CreateContract"), false);
        }
    });
};

$(document).ready(function () {

    $("#a_manage_status").on("click", function () {
        var wnd = $("#ManageStatus").data("kendoWindow");
        wnd.refresh({
            url: AjaxCallUrl.ManageStatusUrl, // returns JSON, { firstName: "Alyx", lastName: "Vance" }
            // data: { ResourceId: dataValue.ResourceId },
        });
        wnd.open().center();

    });


    $("div[data-role='Active']").hide();
    $("label[data-role='Active']").hide();

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
                        beforeSend: function () {

                            // console.log('before send');
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

    $("#ContractLabel").on("focusout", function () {


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
                        beforeSend: function () {

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

});







function OnOpenManageStatus(e) {
    var wnd = $("#ManageStatus").data("kendoWindow");
    kendo.ui.progress(wnd.element, true);
}


function OnRefreshManageStatus(e) {
    var wnd = $("#ManageStatus").data("kendoWindow");
    kendo.ui.progress(wnd.element, false);
    AssignEvent();
}

var AssignEvent = function () {

    $("#SaveContractStatus").on("click", function () {
        // console.log('save');

        var Validator = $("#DivContractStatus").kendoValidator().data("kendoValidator");
        if (Validator.validate()) {

            // SaveProfileDetails();
            // console.log('valid');
            SaveContractStatus();

        }
        else {
            //  console.log('not valid');
        }

    });

    $("#CancelContractStatus").on("click", function () {
        // console.log('cancel');

        var wnd = $("#ManageStatus").data("kendoWindow");
        wnd.close();

    });

};


var SaveContractStatus = function () {
    kendo.ui.progress($("#DivContractStatus"), false);
    var postData = { ContractStatusName: $("#ContractStatusName").val() }; //done
    // console.log(JSON.stringify(ArrayProductCategory));
    $.ajax({ //Process the form using $.ajax()
        type: 'POST', //Method type
        url: AjaxCallUrl.SaveContractStatusUrl,
        data: postData, //Forms name
        dataType: 'json',
        // traditional: true,
        beforeSend: function () {
        },
        success: function (data) {
            // $("#ProductList").html(data.ProductCustomControl);
            //  AssignControlEvent();
            if (data.IsSuccess) {

                $("#ContractStatusName").val('');
                kendo.ui.progress($("#DivContractStatus"), false);
                $("#ContractStatusGrid").data("kendoGrid").dataSource.read();


                var ContractStatusDrp = $("#ContractStatus").data("kendoDropDownList");
                ContractStatusDrp.dataSource.read();
            }
        },
        error: function (request, error) {
            kendo.ui.progress($("#DivContractStatus"), false);
        }
    });
};


///Contract Manage status
function onRequestEndContractStatus(e) {


    if (e.type == "update") {
        $("#ContractStatusGrid").data("kendoGrid").dataSource.read();
        var ContractStatusDrp = $("#ContractStatus").data("kendoDropDownList");
        ContractStatusDrp.dataSource.read();
    }
    else if (e.type == "destroy") {
        $("#ContractStatusGrid").data("kendoGrid").dataSource.read();
        var ContractStatusDrp = $("#ContractStatus").data("kendoDropDownList");
        ContractStatusDrp.dataSource.read();
    }

    //  $("#ContractStatusGrid").data("kendoGrid").dataSource.read();

}

function createRow() {
    grid = $("#ContractStatusGrid").data("kendoGrid");
    grid.addRow();
}

function EditRow(element) {
    grid = $("#ContractStatusGrid").data("kendoGrid");
    grid.editRow($(element).closest("tr"));
}

function DeleteRow(element) {
    grid = $("#ContractStatusGrid").data("kendoGrid");
    grid.removeRow($(element).closest("tr"));
}


function cancelRow() {
    grid = $("#ContractStatusGrid").data("kendoGrid");
    grid.cancelRow();
}



function updateRow() {
    grid = $("#ContractStatusGrid").data("kendoGrid");
    grid.saveRow();
}

function onEdit(e) {
    //on row edit replace the Delete and Edit buttons with Update and Cancel
    $(e.container).find("td:last").html("<a href='javascript: void(0)'  onclick='updateRow()'>Save</a> &nbsp;" +
        "<a href='javascript: void(0)' class='abuttonCancelSymbol' onclick='cancelRow()'>Cancel</a>");
}



function dataBound(e) {
    var data = this.dataSource.view();
    for (var i = 0; i < data.length; i++) {
        var uid = data[i].uid;
        var row = this.table.find("tr[data-uid='" + uid + "']");
        if (data[i].IsNonEditable === true) {
            row.find("td").eq(1).html('Non - Editable');
        }
        if (data[i].IsNonDeletable === true) {
            row.find("td").eq(1).children().eq(1).hide();
        }
    }
}
//End


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



/*Contract Resource*/







