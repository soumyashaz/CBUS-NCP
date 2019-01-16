$(document).ready(function () {
    $("#a_ContractResource").on("click", function () {

        var wnd = $("#ContractResource").data("kendoWindow");
        wnd.title("Upload Resource");
        wnd.refresh({
            url: AjaxCallUrl.ContractResourceUrl, // returns JSON, { firstName: "Alyx", lastName: "Vance" }
            // data: { ResourceId: dataValue.ResourceId },
            data: { ContractResourceId: 0 }
        });
        wnd.open().center();

    });




});

function OnOpenContactResource(e) {
    var wnd = $("#ContractResource").data("kendoWindow");
    kendo.ui.progress(wnd.element, true);
}


function OnRefreshContractResource(e) {
    var wnd = $("#ContractResource").data("kendoWindow");
    kendo.ui.progress(wnd.element, false);
    AssignEventContractResource();

}

function OnOpenContractResourceCategory(e) {
    var wnd = $("#ContractResourceCategoryWindow").data("kendoWindow");
    kendo.ui.progress(wnd.element, true);
}


function OnRefreshContractResourceCategory(e) {
    var wnd = $("#ContractResourceCategoryWindow").data("kendoWindow");
    kendo.ui.progress(wnd.element, false);
    AssignEventContractResourceCategorty();

}


function OnOpenContractResourceView(e) {
    var wnd = $("#WndContractResourceView").data("kendoWindow");
    kendo.ui.progress(wnd.element, true);
}


function OnRefreshContractResourceView(e) {
    var wnd = $("#WndContractResourceView").data("kendoWindow");
    kendo.ui.progress(wnd.element, false);
    // AssignEventContractResource();
    DownLiadContractResourceFile();

}



var AssignEventContractResourceCategorty = function () {

    $("#SaveContractResourceCategory").on("click", function () {
        // console.log('save');

        var Validator = $("#DivContractResourceCategory").kendoValidator().data("kendoValidator");


        if (Validator.validate()) {

            // SaveProfileDetails();
            //  console.log('valid');
            SaveContractResourecategory();

        }
        else {
            // console.log('not valid');
        }

    });

    $("#CancelContractResourceCategory").on("click", function () {
        // console.log('cancel');

        var wnd = $("#ContractResourceCategoryWindow").data("kendoWindow");
        wnd.close();

    });

};


var SaveContractResourecategory = function () {


    kendo.ui.progress($("#DivContractResourceCategory"), false);
    var postData = { ResourceCategoryName: $("#CategoryName").val() }; //done
    // console.log(JSON.stringify(ArrayProductCategory));
    $.ajax({ //Process the form using $.ajax()
        type: 'POST', //Method type
        url: AjaxCallUrl.SaveContractResourceCategoryUrl,
        data: postData, //Forms name
        dataType: 'json',
        // traditional: true,
        beforeSend: function () {
        },
        success: function (data) {
            // $("#ProductList").html(data.ProductCustomControl);
            //  AssignControlEvent();
            if (data.IsSuccess) {
                kendo.ui.progress($("#DivContractResourceCategory"), false);
                $("#ContractResourceCategoryGrid").data("kendoGrid").dataSource.read();

                var ResourceCategorieDrp = $("#ResourceCategoryDropDown").data("kendoDropDownList");
                ResourceCategorieDrp.dataSource.read();
                $("#CategoryName").val('');
            }
        },
        error: function (request, error) {
            kendo.ui.progress($("#DivContractResourceCategory"), false);
        }
    });


}

function onRequestEndContractResourceCategory(e) {

    if (e.type == "update") {
        $("#ContractResourceCategoryGrid").data("kendoGrid").dataSource.read();

        var ResourceCategorieDrp = $("#ResourceCategoryDropDown").data("kendoDropDownList");
        ResourceCategorieDrp.dataSource.read();


    }
    else if (e.type == "destroy") {
        $("#ContractResourceCategoryGrid").data("kendoGrid").dataSource.read();

        var ResourceCategorieDrp = $("#ResourceCategoryDropDown").data("kendoDropDownList");
        ResourceCategorieDrp.dataSource.read();

    }

    //  $("#ContractStatusGrid").data("kendoGrid").dataSource.read();
}



function EditContractResourceCategoryRow(element) {
    grid = $("#ContractResourceCategoryGrid").data("kendoGrid");
    grid.editRow($(element).closest("tr"));
}

function DeleteContractResourceCategoryRow(element) {
    grid = $("#ContractResourceCategoryGrid").data("kendoGrid");
    grid.removeRow($(element).closest("tr"));
}


function cancelContractResourceCategoryRow() {
    grid = $("#ContractResourceCategoryGrid").data("kendoGrid");
    grid.cancelRow();
}



function updateContractResourceCategoryRow() {
    grid = $("#ContractResourceCategoryGrid").data("kendoGrid");
    grid.saveRow();
}

function onContractResourceCategoryEdit(e) {
    //on row edit replace the Delete and Edit buttons with Update and Cancel
    $(e.container).find("td:last").html("<a href='javascript: void(0)'  onclick='updateContractResourceCategoryRow()'>Save</a> &nbsp;" +
        "<a href='javascript: void(0)' class='abuttonCancelSymbol' onclick='cancelContractResourceCategoryRow()'>Cancel</a>");
}

function onContractResourceCategoryEdit(e) {
    //on row edit replace the Delete and Edit buttons with Update and Cancel
    $(e.container).find("td:last").html("<a href='javascript: void(0)'  onclick='updateContractResourceCategoryRow()'>Save</a> &nbsp;" +
        "<a href='javascript: void(0)' class='abuttonCancelSymbol' onclick='cancelContractResourceCategoryRow()'>Cancel</a>");
}

function onContractResourceDataBound(e)
{
    var data = this.dataSource.view();
    for (var i = 0; i < data.length; i++) {
        var uid = data[i].uid;
        console.log(uid);
        var row = this.table.find("tr[data-uid='" + uid + "']");
        if (data[i].IsActive === false) {
            row.find("td").eq(1).children().eq(1).hide();
        }
        
    }
}


var AssignEventContractResource = function () {

    $("#a_ContactResourceCategory").on("click", function () {
        var wnd = $("#ContractResourceCategoryWindow").data("kendoWindow");
        wnd.refresh({
            url: AjaxCallUrl.ContractResourceCategoryUrl, // returns JSON, { firstName: "Alyx", lastName: "Vance" }
            // data: { ResourceId: dataValue.ResourceId },
        });
        wnd.open().center();

    });

    $("#DivZoneStateDropdown").on("click", function () {
        CustomControlZoneStateList($(this).parent().parent(), "CustomControlPoupZoneState");
        // event.stopPropagation();
    });


    $("#SaveContractResource").on("click", function () {
        SaveContractResource();
        // event.stopPropagation();
    });

    $("#CancelContractResource").on("click", function () {
        var DataUpdateWindow = $("#ContractResource").data("kendoWindow");
        if (DataUpdateWindow) {

            DataUpdateWindow.close();
        }
        // event.stopPropagation();
    });


    $("#ContractResourceTitle").on("focusout", function () {

        //  console.log($('span[data-for="ContractResourceTitle"]').length);

        if ($(this).val().trim() == $("#HdnResourceTitleHistory").val().trim()) {
            Flag == 2;
            cache.valid = true;
            $("#ValidResourceTitle").val("1");
            // $('#SpanResourceTitleValidation').hide();

            return;

        }

        var Flag = 1;
        var availability = {
            cache: {},
            check: function (element, settings) {

                var id = element.attr('id');
                var cache = this.cache[id] = this.cache[id] || {};
                var ContractId = 0;
                if ($("#HdnContractId").length) {
                    ContractId = $("#HdnContractId").val();
                }
                // var ContractId=
                // console.log(Flag);
                if (Flag == 1) {
                    $.ajax({
                        url: AjaxCallUrl.ResourceTitleUrl,
                        dataType: 'json',
                        data: { LabelName: element.val(), ResourceDumpId: $("#HdnDumpId").val(), EditMode: $("#HdnEditMode").val(), ContractId: ContractId },
                        beforeSend: function () {
                            Flag = 2;
                            // console.log('before send');
                        },
                        success: function (data) {

                            // console.log(data);
                            // the `data` object returns true or false
                            // based on the availability of the value
                            // set the value on the cache object so 
                            // that it can be referenced in the next validation run

                            cache.valid = data.IsLabelAvailable;

                            // trigger validation again
                            cache.value = element.val();

                            if (data.IsLabelAvailable) {
                                $("#ValidResourceTitle").val("1");
                            }
                            else {
                                $("#ValidResourceTitle").val("0");
                            }
                            //  return true;

                            //  Flag = 1;
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


                        }
                    });
                }

            }

        };


        var Validator = $("#tdtitle").kendoValidator({
            messages: {
                available: function (input) {
                    //    console.log('msg');
                    var id = input.attr('id');
                    var msg = 'Atleast one market selection is required';
                    var cache = availability.cache[id];

                    if (cache.checking) {
                        return "Checking..."
                    }
                    else {
                        return "You can't use same file name";
                    }
                }

            },
            rules: {
                available: function (input) {
                    //  console.log('avail');
                    var validate = input.data('available');
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
                            //  console.log('vVValid33');
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
                        return false;
                    }
                    // this rule does not apply to the input
                    return true;
                }
            }
        }).data("kendoValidator");
        Validator.validateInput($(this));
    });



    if ($("#DivContractResource").length) {
        $('html').on('click', function () {
            // do your stuff here
            var ChildDiv = $("#DivZoneState").children(".CustomControlPoupZoneState");

            if ($(ChildDiv).is(':visible')) {
                $(ChildDiv).hide();
            }
        }).find('.classZoneMarket').on('click', function (e) {
            e.stopPropagation();
        });
    }
}

var SaveContractResource = function () {



    var Validator = $("#DivContractResource").kendoValidator({
        messages: {
            available: function (input) {




                return "You can't use same file name";

            },
            ZoneSate: function (input) {
                return "Atleast one market selection is required";
            },
            uploadFile: function (input) {
                return "*";
            }
        },
        rules: {
            available: function (input) {

                var validate = input.data('available');
                // if the input has a `data-available` attribute...
                if (typeof validate !== 'undefined' && validate !== false) {

                    if ($("#ValidResourceTitle").val() == "1") {
                        return true;
                    }
                    else {
                        return false;
                    }



                }
                // this rule does not apply to the input
                return true;
            },

            ZoneSate: function (input) {

                if (input.is("[data-role=ZoneSate]")) {

                    if ($("#HdnEditMode").val() == "0") {
                        var CheckedCheckBox = $("#ZoneStateList").children().find("input[name='ChkStateSelect']:checked");
                        if (CheckedCheckBox.length == 0) {
                            return false;
                        }
                        else {
                            return true;
                        }
                        return false
                    }
                    else {
                        if ($("#ZoneStateList").length) {
                            var CheckedCheckBox = $("#ZoneStateList").children().find("input[name='ChkStateSelect']:checked");
                            if (CheckedCheckBox.length == 0) {
                                return false;
                            }
                            else {
                                return true;
                            }
                            return false
                        }
                        else {
                            return true;
                        }
                    }
                    return false
                }
                return true;


            },
            uploadFile: function (input) {
                var validate = input.data('filerequired');
                // if the input has a `data-available` attribute...
                if (typeof validate !== 'undefined' && validate !== false) {
                    //  console.log('heloofff');

                    if ($("#HdnEditMode").val() != "1") {
                        return input.closest(".k-upload").find(".k-file").length;
                    }


                }

                //if (input[0].type == "file") {
                //  //  console.log(input.closest(".k-upload").find(".k-file").length);

                //}
                return true;
            }

        }

    }).data("kendoValidator");
    if (Validator.validate()) {
        // console.log('save/update');
        //  console.log('vaid');
        if (Validator.validateInput($("#DivZoneState"))) {
            PostResourceToServer();
        }
    }
    else {
        //  console.log('in vaid');
        // console.log(Validator);
        // console.log('save/update');
    }
}

function ClearForm() {
}

function PostResourceToServer() {
    kendo.ui.progress($("#DivContractResource"), true);

    // var Markets = [];
    var SelectedMarket = $("#ZoneStateList").children().find("input[name='ChkStateSelect']:checked");
    var Markets = '';
    $(SelectedMarket).each(function () {
        // Markets.push($(this).siblings().eq(0).val());
        Markets += $(this).siblings().eq(0).val() + ','
    });

    //var postData = {
    //    ResourceCategoryId: $("#ResourceCategoryDropDown").data("kendoDropDownList").value(),
    //    Markets: Markets,
    //    Title: $("#ContractResourceTitle").val(),
    //    Description: $("#ContractResourceDescription").val(),
    //    DumpId: $("#HdnDumpId").val()
    //}; //done

    var fileUpload = $("input[name*='ContractReourceUpload']").get(0);
    var files = fileUpload.files;
    var fileData = new FormData();

    // Looping over all files and add it to FormData object  
    for (var i = 0; i < files.length; i++) {
        fileData.append(files[i].name, files[i]);
    }


    //  console.log('jj');

    fileData.append('ResourceId', $("#HdnResourceId").val());
    fileData.append('ResourceCategoryId', $("#ResourceCategoryDropDown").data("kendoDropDownList").value());
    fileData.append('Markets', Markets);
    fileData.append('Title', $("#ContractResourceTitle").val());
    fileData.append('Description', $("#ContractResourceDescription").val());
    fileData.append('DumpId', $("#HdnDumpId").val());
    fileData.append('EditMode', $("#HdnEditMode").val());

    if ($("#HdnContractId").length) {
        fileData.append('ContractId', $("#HdnContractId").val());

    }



    // console.log(JSON.stringify(ArrayProductCategory));
    $.ajax({ //Process the form using $.ajax()
        type: 'POST', //Method type
        url: AjaxCallUrl.SaveResourceUrl,
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

                //  console.log((data));

                $("#HdnDumpId").val(data.DumpId);
                kendo.ui.progress($("#DivContractResource"), false);

                if (DataUpdateWindow) {

                    DataUpdateWindow.title("Update Information");
                    DataUpdateWindow.content("<div class='msgboxSuccess'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i> Data Update Successfully</div>");
                    DataUpdateWindow.open();

                    $(".k-upload-files.k-reset").find("li").parent().remove();
                    $("#ContractResourceTitle").val('');
                    $("#ContractResourceDescription").val('');

                    if ($("#listViewContractResource").length) {
                        var KendoListView = $("#listViewContractResource").data("kendoListView");
                        KendoListView.dataSource.read();
                    }

                }
            }
            else {
                kendo.ui.progress($("#DivContractResource"), false);
                if (DataUpdateWindow) {
                    DataUpdateWindow.title("Error Information");
                    DataUpdateWindow.content("<div class='msgboxError'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i>" + data.ModelError + " </div>");
                    DataUpdateWindow.open();
                }
            }
        },
        error: function (request, error) {
            kendo.ui.progress($("#DivContractResource"), false);
        }
    });
}



function onSelectContractReourceUpload(e) {

    var files = e.files;

    $.each(files, function () {
        //   console.log(this.extension.toLowerCase());
        if (this.extension.toLowerCase() != ".png" && this.extension.toLowerCase() != ".jpg" && this.extension.toLowerCase() != ".jpeg"
            && this.extension.toLowerCase() != ".doc" && this.extension.toLowerCase() != ".docx" && this.extension.toLowerCase() != ".rtf"
            && this.extension.toLowerCase() != ".pdf" && this.extension.toLowerCase() != ".xls" && this.extension.toLowerCase() != ".xlsx"
            ) {
            alert("Only .png/.jpg/.jpeg/.doc/.pdf/.docx files can be uploaded!")
            e.preventDefault();
        }
        if (this.size / 1024 / 1024 > 100) {
            alert("Max 5Mb file size is allowed!")
            e.preventDefault();
        }
    });
}


function EditResource(ContractID) {

    //var v1 = $(e).siblings();
    //console.log(v1.length);
    //var v = $(this).parent().parent().find("input[name='HdnContactResourceListId']").eq(0);
    //console.log(v.val());
    //console.log(e.target);
    var wnd = $("#ContractResource").data("kendoWindow");
    wnd.title('Edit Resource');
    wnd.refresh({
        url: AjaxCallUrl.ContractResourceUrl, // returns JSON, { firstName: "Alyx", lastName: "Vance" }
        data: { ContractResourceId: $("#HdnContactResourceListId_" + ContractID).val() }
    });
    wnd.open().center();
}



function ViewContractResource(ResourceId) {
    //var wnd = $("#WndContractResourceView").data("kendoWindow");
    //wnd.refresh({
    //    url: AjaxCallUrl.ContractResourceViewUrl, // returns JSON, { firstName: "Alyx", lastName: "Vance" }
    //    data: { ResourceId: $("#HdnContactResourceListId_" + ResourceId).val() }
    //});
    //wnd.open().center();


    // var ResourceId = $(this).siblings().eq(0).val();
    var src = AjaxCallUrl.DownLaodResourceFile + '?ResourceId=' + ResourceId;
    //   console.log(src);

    var iframe = $("<iframe/>").load(function () {
        // $.unblockUI();
    }).attr({
        src: src
    }).appendTo($("#DivIframeContainer"));

}







function DeleteContractResource(ResourceId) {


    var result = confirm("Are you sure ?");
    if (!result) {
        return;
    }

    kendo.ui.progress($("body"), true);




    var postData = {
        ResourceId: $("#HdnContactResourceListId_" + ResourceId).val()
    }; //done


    $.ajax({ //Process the form using $.ajax()
        type: 'POST', //Method type
        url: AjaxCallUrl.ContractResourceDeleteUrl,
        data: postData, //Forms name
        dataType: 'json',
        // traditional: true,
        // contentType: false, // Not to set any content header  
        //   processData: false,
        beforeSend: function () {
        },
        success: function (data) {
            // $("#ProductList").html(data.ProductCustomControl);
            //  AssignControlEvent();
            var DataUpdateWindow = $("#DataUpdateMessage").data("kendoWindow");
            if (data.IsSuccess) {
                kendo.ui.progress($("body"), false);
                if (DataUpdateWindow) {
                    DataUpdateWindow.title("Update Information");
                    DataUpdateWindow.content("<div class='msgboxSuccess'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i> Data Update Successfully</div>");
                    DataUpdateWindow.open();
                }
                if ($("#listViewContractResource").length) {
                    var KendoListView = $("#listViewContractResource").data("kendoListView");
                    KendoListView.dataSource.read();
                }

            }
            else {
                kendo.ui.progress($("body"), false);
                if (DataUpdateWindow) {
                    DataUpdateWindow.title("Error Information");
                    DataUpdateWindow.content("<div class='msgboxError'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i>" + data.ModelError + " </div>");
                    DataUpdateWindow.open();
                }
            }
        },
        error: function (request, error) {
            kendo.ui.progress($("body"), false);
        }
    });
}



function DownLiadContractResourceFile() {



    $("#a_ContractResourceDownload").on("click", function () {

        var ResourceId = $(this).siblings().eq(0).val();
        var src = AjaxCallUrl.DownLaodResourceFile + '?ResourceId=' + ResourceId;
        //   console.log(src);

        var iframe = $("<iframe/>").load(function () {
            // $.unblockUI();
        }).attr({
            src: src
        }).appendTo($("#DivIframeContainer"));
    });



    //$.ajax({
    //    url: AjaxCallUrl.DownLaodResourceFile,
    //    contentType: 'application/json; charset=utf-8',
    //    datatype: 'json',
    //    data: {
    //        studentId: 123
    //    },
    //    type: "GET",
    //    success: function () {
    //        window.location = '@Url.Action("DownloadAttachment", "PostDetail", new { studentId = 123 })';
    //    }
    //});
}