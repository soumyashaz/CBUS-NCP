var popupNotification = $("#popupNotification").kendoNotification({
    stacking: "down",
    show: onShow,
    button: true
}).data("kendoNotification");

$(document).ready(function () {
    kendo.ui.progress($(document.body), true);

    ContractContent.ContentSectionDeleteClicked = false;

    ContractContent.AssignControls();
    ContractContent.InitEditorControl();
    ContractContent.ShowHideContractList();
    ContractContent.PopulateAttachmentLibrary(1);

    $("li.li-content-tab").first().click();
    $("ul.below-bar-section").children("li:visible").first().click();

    $("#spnSelectedContract").text($("li.list-contract-name[data-current-contract='1']").find("a").text());

    //Keep "Embedded Items" tab disabled - temporarily
    $("#lnkEmbeddedItems").click(function (e) {
        e.stopPropagation();
        e.preventDefault();
    });
});

var ContractContent = {
    ContentSectionDeleteClicked: false,

    AssignControls: function () {
        $("#ddlVariationList").change(function () {
            $("#hdnContentId").val($("#ddlVariationList :selected").val());

            ContractContent.PopulateContent($("#hdnContentId").val());
            ContractContent.PopulateContentMarket($("#hdnContentId").val());
            ContractContent.PopulateContentAttachmentCount($("#hdnContentId").val());
            ContractContent.PopulateContentAttachmentList($("#hdnContentId").val());

            $("#ddlCopyFromVariation").children("[data-content-id='" + $("#hdnContentId").val() + "']").hide();
        });

        $("#lnkAddSection").click(function () {
            $("#divSection").modal({ backdrop: 'static', keyboard: false });
            setTimeout(function () { $("#ddlSection").focus(); }, 1000);

            return false;
        });

        $("#btnAddSection").click(function () {
            ContractContent.AddSection();
        });

        $("#btnCloseSectionDiv").click(function () {
            $("#divSection").modal("hide");
        });

        $("#lnkAddVariation").click(function () {
            $("#h4VariationModalHeading").text("Add New Variation");
            $("#txtVariationName").val("");
            $("#hdnContentId").val("0");
            
            $("#divVariation").modal({ backdrop: 'static', keyboard: false });
            setTimeout(function () { $("#txtVariationName").focus(); }, 1000);
        });

        $("#btnCloseVariationDiv").click(function () {
            $("#divVariation").modal("hide");
        });

        $("#btnSaveVariation").click(function () {
            ContractContent.SaveVariation(false);
        });

        $("#lnkEditVariation").click(function () {
            $("#h4VariationModalHeading").text("Edit Variation");
            $("#txtVariationName").val($("#ddlVariationList :selected").text());
            $("#hdnContentId").val($("#ddlVariationList :selected").val());
            
            $("#divVariation").modal({ backdrop: 'static', keyboard: false });
            setTimeout(function () { $("#txtVariationName").focus(); } , 1000);
        });

        $("#lnkDeleteVariation").click(function () {
            if (confirm("Are you sure you want to delete this variation?")) {
                ContractContent.DeleteVariation();
            }
        });

        $("#lnkCopyContent").click(function () {
            $("#divCopyContent").modal({ backdrop: 'static', keyboard: false });
        });

        $("#btnCloseCopyVariation").click(function () {
            $("#divCopyContent").modal("hide");
        });

        $("#btnCopyVariation").click(function () {
            var ConfirmationMessage = "Copying from another variation will replace/remove all existing attachments, embedded items and images. Are you sure you want to proceed?";
            if (confirm(ConfirmationMessage) == true) {
                ContractContent.CopyVariation();
            }
        });

        $("#lnkSaveContent").click(function () {
            ContractContent.SaveContent();
        });

        $("#lnkSaveContentText").click(function () {
            ContractContent.SaveContent();
        });

        $("#lnkSelectAllMarket").click(function () {
            $("input.chk-market:enabled").prop('checked', true);
        });

        $("#lnkDeselectAllMarket").click(function () {
            $("input.chk-market").prop('checked', false);
        });

        $("li.li-content-section").mouseover(function () {
            $(this).find("a.delete-icon").first().show();
        });

        $("li.li-content-section").mouseout(function () {
            $(this).find("a.delete-icon").first().hide();
        });

        $("li.li-content-section").click(function () {
            if (!ContractContent.ContentSectionDeleteClicked) {
                kendo.ui.progress($(document.body), true);

                $("li.li-content-section").each(function () {
                    $(this).removeClass("selected");
                    $(this).css("background-color", "");
                });
                $(this).addClass("selected");
                $(this).css("background-color", "#ddd");

                $("#lblSelectedSection").text($(this).data("section"));
                $("#iSelectedSection").removeClass().addClass($(this).data("icon"));

                var ContractId = $("#hdnContractId").val();
                var SectionId = $(this).data("section-id");

                $("#hdnSectiontId").val(SectionId);

                ContractContent.PopulateVariationList(ContractId, SectionId);
            }
        });

        $("a.delete-icon").click(function () {
            ContractContent.ContentSectionDeleteClicked = true;
            var SectionName = $(this).parent().first().data("section");
            var ConfirmationMessage = "Deleting the Section will delete all the associated Content Variations for this Contract. Are you sure you want to delete the " + SectionName + " section for this Contract?";
            if (confirm(ConfirmationMessage) == true) {
                ContractContent.DeleteSection($(this).data("section-id"));
            }
        });

        $("li.li-content-tab").click(function () {
            $("li.li-content-tab").each(function () {
                $(this).removeClass("selected");
                $(this).css("background-color", "");
            });
            $(this).addClass("selected");
            $(this).css("background-color", "aliceblue");

            $("div.tab-panel").hide();
            $("div.tab-panel[data-tab='" + $(this).data("tab") + "']").show();
        });

        $("input.chk-market").click(function () {
            var ContentId = parseInt($(this).data("content-id"));

            if (ContentId > 0) {
                var result = confirm("This market is associated with another variation for this Contract/Section. Do you want to disassociate it? This action cannot be undone.");

                if (result == true) {
                    ContractContent.DisassociateContentMarket(ContentId, $(this).data("market-id"));
                } else {
                    return false;
                }
            }
        });

        //=================== ATTACHMENT SECTION =================
        $("#btnAddNewAttachment").click(function () {
            $("#hdnContentAttachmentId").val("0");
            $("#txtContentAttachmentTitle").val("");
            $("#txtContentAttachmentDesc").val("");
            $("#txtContentAttachmentVersion").val("");
            $("#txtExternalURL").val("");

            $("#rbtnExternalLink").removeAttr("disabled").removeClass("disabled");
            $("#rbtnLibraryItem").removeAttr("disabled").removeClass("disabled");
            $("#rbtnNewFile").removeAttr("disabled").removeClass("disabled");

            $("#divNewAttachment").show();
            $("#btnAddNewAttachment").addClass("disabled");
            $("#btnAddNewAttachment").prop("disabled", "");
        });

        $("#btnCancelAttachment").click(function () {
            $("#divNewAttachment").hide();
            $("#btnAddNewAttachment").removeClass("disabled");
            $("#btnAddNewAttachment").removeAttr("disabled");
        });

        $("#btnSaveAttachment").click(function () {
            if ($("#txtContentAttachmentTitle").val().trim() == "") {
                $("#txtContentAttachmentTitle").css("border", "2px solid red");
                setTimeout(function () { $("#txtContentAttachmentTitle").css("border", "") }, 5000);

                return false;
            }

            if ($("#rbtnExternalLink").prop("checked")) {
                if ($("#txtExternalURL").val().trim() == "") {
                    $("#txtExternalURL").css("border", "2px solid red");
                    setTimeout(function () { $("#txtExternalURL").css("border", "") }, 5000);

                    return false;
                }
            }

            ContractContent.SaveContentAttachment();
        });

        $("#txtContentAttachmentTitle").change(function () {
            if ($(this).val().trim() == "") {
                $(this).css("border", "2px solid red");
                setTimeout(function () { $("#txtContentAttachmentTitle").css("border", "") }, 5000);
            } else {
                $(this).css("border", "");
            }
        });

        $("input[name='optAttachmentType']").click(function () {
            $("input[name='optAttachmentType']").each(function () {
                if ($(this).prop("checked")) {
                    if ($(this).val() == "1") {
                        $("#txtExternalURL").show();
                        $("#btnSaveAttachment").removeAttr("disabled").removeClass("disabled");

                        $("#btnSelectLibraryAttachment").prop("disabled", "disabled");
                        $("#btnSelectLibraryAttachment").addClass("disabled");
                        $("#btnUploadLibraryAttachment").prop("disabled", "disabled");
                        $("#btnUploadLibraryAttachment").addClass("disabled");
                    }
                    if ($(this).val() == "2") {
                        $("#txtExternalURL").hide();

                        $("#btnSelectLibraryAttachment").removeAttr("disabled");
                        $("#btnSelectLibraryAttachment").removeClass("disabled");
                        $("#btnUploadLibraryAttachment").prop("disabled", "disabled");
                        $("#btnUploadLibraryAttachment").addClass("disabled");
                    }
                    if ($(this).val() == "3") {
                        $("#txtExternalURL").hide();

                        $("#btnUploadLibraryAttachment").removeAttr("disabled");
                        $("#btnUploadLibraryAttachment").removeClass("disabled");
                        $("#btnSelectLibraryAttachment").prop("disabled", "disabled");
                        $("#btnSelectLibraryAttachment").addClass("disabled");
                    }
                }
            });
        });

        $("#btnSelectLibraryAttachment").click(function () {
            $("#divAttachmentLibrary").modal({ backdrop: 'static', keyboard: false });
        });

        $("#btnUploadLibraryAttachment").click(function () {
            $("#divAttachmentLibrary").modal({ backdrop: 'static', keyboard: false });
            $("#UploadAttachment").click();
        });
        //=================== ATTACHMENT SECTION =================

        //=================== ATTACHMENT LIBRARY =================
        $("#UploadAttachment").kendoUpload({
            async: {
                saveUrl: AjaxCallUrl.UploadLibraryFileUrl,
                removeUrl: "",
                autoUpload: true
            },
            validation: {
                allowedExtensions: [".doc", ".docx", ".pdf", ".xlsx", ".xls", ".ppt", ".ppx"]
            },
            localization: {
                select: 'UPLOAD'
            },
            showFileList: false,
            //dropZone: ".dropZoneElement",
            multiple: true,
            select: onFileSelect,
            progress: onProgress,
            success: onSuccess,
            complete: onAttachmentUploadComplete
        });

        $("#UploadAttachment").kendoUpload().attr("accept", ".doc,.docx,.pdf,.xlsx,.xls,.ppt,.ppx");

        $("#ReplaceAttachment").kendoUpload({
            async: {
                saveUrl: AjaxCallUrl.ReplaceLibraryFileUrl,
                removeUrl: "",
                autoUpload: true
            },
            upload: function (e) {
                e.data = { AttachmentId: $("#hdnSelectedAttachmentId").val() };
            },
            validation: {
                allowedExtensions: [".doc", ".docx", ".pdf", ".xlsx", ".xls", ".ppt", ".ppx"]
            },
            localization: {
                select: 'UPLOAD'
            },
            showFileList: false,
            //dropZone: ".dropZoneElement",
            multiple: false,
            select: onFileSelect,
            progress: onProgress,
            success: onSuccess,
            complete: onAttachmentUploadComplete
        });

        $("#ReplaceAttachment").kendoUpload().attr("accept", ".doc,.docx,.pdf,.xlsx,.xls,.ppt,.ppx");
        $("#ReplaceAttachment").removeAttr("multiple");

        $("#btnCancelAttachmentLibraryPopup").click(function () {
            $("#divAttachmentLibrary").modal('hide');
        });

        $("#btnSelectAttachmentLibraryPopup").click(function () {
            $("#txtContentAttachmentTitle").val($("#h3FileTitle").text());
            $("#txtContentAttachmentDesc").val($("#pAttachmentDescription").text());
            $("#txtContentAttachmentVersion").val($("#pAttachmentVersion").text());

            $("#btnSaveAttachment").removeAttr("disabled").removeClass("disabled");

            $("#divAttachmentLibrary").modal('hide');
        });

        $("#lnksFileDetailView").click(function () {
            $("i.fa-list-ul").css("font-weight", "bold");
            $("i.fa-compress").css("font-weight", "normal");

            $("#divAttachmentListView").hide();
            $("#divAttachmentDetailsView").show();
        });

        $("#lnkFileListView").click(function () {
            $("i.fa-list-ul").css("font-weight", "normal");
            $("i.fa-compress").css("font-weight", "bold");

            $("#divAttachmentDetailsView").hide();
            $("#divAttachmentListView").show();
        });

        $("#lnkFilterSort").click(function () {
            $("i.fa-sliders").css("font-weight", "bold");
            ContractContent.PopulateAttachmentLibrary(3);
        });

        $("#lnkAttachmentInfo").click(function () {
            $("#divEditAttachmentDetails").hide();
            $("#divLibAttachmentDetails").show();
        });

        $("#lnkAttachmentReplace").click(function () {
            var result = confirm("Are you sure you want to replace the existing copy of this file? This action cannot be undone.");

            if (result == true) {
                $("#ReplaceAttachment").click();
            }
        });

        $("#lnkAttachmentEdit").click(function () {
            $("#divLibAttachmentDetails").hide();
            $("#divEditAttachmentDetails").show();
        });

        $("#lnkAttachmentEdit").click(function () {
            $("#divLibAttachmentDetails").hide();
            $("#divEditAttachmentDetails").show();
        });

        $("#btnCancelEditAttachment").click(function () {
            $("#divEditAttachmentDetails").hide();
            $("#divLibAttachmentDetails").show();
        });

        $("#lnkAttachmentDelete").click(function () {
            var result = confirm("Are you sure you want to delete this file from the library? The physical file will be deleted and it will be disassociated from any content sections to which it is currently linked. This action cannot be undone.");

            if (result == true) {
                ContractContent.DeleteAttachment();
            }
        });

        $("#btnSaveAttachmentDetails").click(function () {
            ContractContent.SaveAttachmentDetails();
        });

        $("#btnSearchAttachment").click(function () {
            ContractContent.PopulateAttachmentLibrary(1);
        });

        //=================== ATTACHMENT LIBRARY =================
    },

    AssignAttachmentListControls: function () {
        $("div.div-attachment-info").click(function () {
            $("div.div-attachment-info").each(function () {
                $(this).removeClass("selected");
                $(this).css("background-color", "");
            });

            $(this).css("background-color", "aliceblue");
            $(this).addClass("selected");

            if ($("#divAttachmentDetailsView").css("display") == "block") {         //DETAIL VIEW ON -- SELECT THE SAME ITEM IN LIST VIEW
                $("div.div-attachment-info.list-view[data-attachment-id='" + $(this).data("attachment-id") + "']").css("background-color", "aliceblue").addClass("selected");
            } else {                                                                //LIST VIEW ON -- SELECT THE SAME ITEM IN DETAIL VIEW
                $("div.div-attachment-info.detail-view[data-attachment-id='" + $(this).data("attachment-id") + "']").css("background-color", "aliceblue").addClass("selected");
            }

            $("#h3FileTitle").text($(this).data("attachment-title"));
            $("#pAttachmentDescription").text($(this).data("attachment-desc"));
            $("#lnkAttachmentPath").text($(this).data("attachment-file-path"));
            $("#pAttachmentVersion").text($(this).data("attachment-version"));
            $("#pAttachmentSize").text($(this).data("attachment-size") + " bytes");

            $("#txtAttachmentTitle").val($(this).data("attachment-title"));
            $("#txtAttacmentDesc").val($(this).data("attachment-desc"));
            $("#txtAttachmentFilePath").val($(this).data("attachment-file-path"));
            $("#txtAttachmentVersion").val($(this).data("attachment-version"));

            $("#divEditAttachmentDetails").hide();
            $("#divLibAttachmentToolbar").show();
            $("#divLibAttachmentDetails").show();

            $("#hdnSelectedAttachmentId").val($(this).data("attachment-id"));

            $("#btnSelectAttachmentLibraryPopup").removeAttr("disabled");
            $("#btnSelectAttachmentLibraryPopup").removeClass("disabled");

            var downloadUrl = AjaxCallUrl.DownloadAttachmentFileUrl;
            downloadUrl = downloadUrl.replace('aid', $(this).data("attachment-id"));
            $("#lnkAttachmentDownload").attr('href', downloadUrl);
        });

        if ($("#hdnLastSavedAttachmentId").val() != "0") {
            if ($("#divAttachmentDetailsView").css("display") == "block") {
                $("div.div-attachment-info.detail-view[data-attachment-id='" + $("#hdnLastSavedAttachmentId").val() + "']").click();
            } else {
                $("div.div-attachment-info.list-view[data-attachment-id='" + $("#hdnLastSavedAttachmentId").val() + "']").click();
            }
        } else {
            $("div.div-attachment-info").first().click();
        }
    },

    AssignContentAttachmentListControls: function () {

        $("a.con-att-edit").click(function () {
            $("#hdnContentAttachmentId").val($(this).data("content-attachment-id"));
            $("#txtContentAttachmentTitle").val($(this).data("content-attachment-title"));
            $("#txtContentAttachmentDesc").val($(this).data("content-attachment-desc"));
            $("#txtContentAttachmentVersion").val($(this).data("content-attachment-version"));
            $("#txtExternalURL").val($(this).data("content-attachment-url"));

            var ExternalURL = $(this).data("content-attachment-url");
            if (ExternalURL == "") {        //LIBRARY ITEM
                $("#rbtnLibraryItem").attr("checked", "");
                $("#rbtnLibraryItem").click();

                $("#rbtnExternalLink").attr("disabled", "");
                $("#rbtnExternalLink").addClass("disabled");
            } else {                        //EXTERNAL URL
                $("#rbtnExternalLink").attr("checked", "");
                $("#rbtnExternalLink").click();

                $("#rbtnLibraryItem").attr("disabled", "");
                $("#rbtnLibraryItem").addClass("disabled");
            }

            $("#btnSaveAttachment").removeAttr("disabled").removeClass("disabled");

            $("#divNewAttachment").show();

            $("#btnAddNewAttachment").addClass("disabled");
            $("#btnAddNewAttachment").prop("disabled", "");
        });

        $("a.con-att-disassociate").click(function () {
            ContractContent.DisassociateContentAttachment($(this).data("content-attachment-id"));
        });
    },

    InitEditorControl: function () {
        $("#txtEdContractContent").kendoEditor({
            messages: {
                fontNameInherit: "Arial",
                tableWizard: "Table Wizard"
            },
            tools: [
                "bold",
                "italic",
                "underline",
                "strikethrough",
                "justifyLeft",
                "justifyCenter",
                "justifyRight",
                "justifyFull",
                "insertUnorderedList",
                "insertOrderedList",
                "indent",
                "outdent",
                "createLink",
                "unlink",
                "insertImage",
                "createTable",
                "tableWizard",
                "addRowAbove",
                "addRowBelow",
                "addColumnLeft",
                "addColumnRight",
                "deleteRow",
                "deleteColumn",
                "viewHtml",
                //"foreColor",
                {
                    name: "customundo",
                    tooltip: "Undo",
                    exec: function (e) {
                        var editor = $(this).data("kendoEditor");
                        editor.exec("undo");
                    }
                },
                {
                    name: "customredo",
                    tooltip: "Redo",
                    exec: function (e) {
                        var editor = $(this).data("kendoEditor");
                        editor.exec("redo");
                    }
                },
                //"backColor",
                //{
                //    name: "fontName",
                //    items: [].concat(
                //        kendo.ui.Editor.prototype.options.fontName[2],
                //        [{ text: "Arial Black", value: "Arial Black" },
                //        { text: "Comic Sans MS", value: "Comic Sans MS" },
                //        { text: "Courier New", value: "Courier New" },
                //        { text: "Georgia", value: "Georgia" },
                //        { text: "Impact", value: "Impact" },
                //        { text: "Times New Roman", value: "Times New Roman" },
                //        { text: "Verdana", value: "Verdana" },
                //        { text: "Webdings", value: "Webdings" }
                //        ])
                //},
                //"fontSize",
                {
                    name: "formatting",
                    items: [].concat([
                        { text: "Paragraph", value: "p" },
                        //{ text: "Quotation", value: "q" },
                        { text: "Heading 2", value: "h2" },
                        { text: "Heading 3", value: "h3" },
                        { text: "Heading 4", value: "h4" },
                        { text: "Heading 5", value: "h5" },
                        { text: "Heading 6", value: "h6" }
                        ])
                }
            ],
        });
    },

    ShowHideContractList: function () {
        $(document.body).click(function () {
            $("#ulContractList").hide();
        });

        $("#lnkSelectAContract").click(function (e) {
            e.stopPropagation();
            //$("#ulContractList").toggle();
            $("#divContractLogoList").modal({ backdrop: 'static', keyboard: false });
        });
    },

    SelectDefaultOptions: function () {

        if ($("#ddlVariationList").children().length > 0) {
            $("#hdnContentId").val($("#ddlVariationList").val());

            ContractContent.PopulateContent($("#hdnContentId").val());
            ContractContent.PopulateContentMarket($("#hdnContentId").val());
            ContractContent.PopulateContentAttachmentCount($("#hdnContentId").val());
            ContractContent.PopulateContentAttachmentList($("#hdnContentId").val());
        } else {
            //Clear Editor text and market list
            $("#txtEdContractContent").data("kendoEditor").value("");

            $("input.chk-market").each(function () {
                $(this).data("content-id", "0");
                $(this).parent().css("color", "");
                $(this).removeAttr("disabled");
                $(this).prop("checked", false);
            });
        }
    },

    PopulateVariationList: function (ContractId, SectionId) {

        var postData = {
            ContractId: ContractId,
            SectionId: SectionId,
        };

        //Ajax call to fetch variation list
        $.ajax({
            url: AjaxCallUrl.GetVariationList,
            type: "GET",
            dataType: "json",
            data: postData,
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var ddlVariation = $('#ddlVariationList');
                ddlVariation.empty();

                if (data.length == 0) {             //------------ NO VARIATION EXISTS - ADD DEFAULT VARIATION
                    $("#hdnContentId").val("0");

                    var DefaultVariation = "Default " + $("#lblSelectedSection").text();
                    $("#txtVariationName").val(DefaultVariation);

                    $("input.chk-market:enabled").prop('checked', true); 

                    ContractContent.SaveVariation(true);
                } else {
                    var ddlCopyFromVariation = $('#ddlCopyFromVariation');
                    ddlCopyFromVariation.empty();

                    var CurrentContentId = $("#ddlVariationList :selected").val();

                    $.each(data, function (i, obj) {
                        ddlVariation.append($('<option></option>').val(obj.ContentId).html(obj.DisplayValue));

                        if (obj.ContentId != CurrentContentId) {
                            ddlCopyFromVariation.append($('<option></option>').val(obj.ContentId).html(obj.DisplayValue).attr("data-content-id", obj.ContentId));
                        }
                    });
                    ContractContent.SelectDefaultOptions();
                }

                if (data.length > 1) {
                    $("#lnkCopyContent").on('click', function () { $("#divCopyContent").modal({ backdrop: 'static', keyboard: false }); });
                    $("#lnkCopyContent").children("i").css("color", "slategrey").css("cursor", "none");
                } else {
                    $("#lnkCopyContent").children("i").css("color", "slategrey").css("cursor", "none");
                    $("#lnkCopyContent").off('click');
                }

                kendo.ui.progress($(document.body), false);
            },
            error: function (err) {
                popupNotification.show("Error in PopulateVariationList!", "error");
            }
        });
    },

    PopulateContent: function (ContentId) {
        //Ajax call to get content variation
        $.ajax({
            url: AjaxCallUrl.PopulateContentUrl.replace("pContentId", ContentId),
            type: "GET",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                $("#txtEdContractContent").data("kendoEditor").value(data);
            },
            error: function (err) {
                //console.log(err);
            }
        });
    },

    PopulateContentMarket: function (ContentId) {
        //Ajax call to get Content Market
        $.ajax({
            url: AjaxCallUrl.GetContentMarketUrl.replace("pContentId", ContentId),
            type: "GET",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                $("input.chk-market").each(function () {
                    $(this).data("content-id", "0");
                    $(this).parent().css("color", "");
                    $(this).removeAttr("disabled");
                    $(this).prop("checked", false);
                });

                for (var i = 0; i < data.length; i++) {
                    $("input.chk-market[data-market-id='" + data[i].MarketId + "']").prop("checked", true);
                }

                $.ajax({
                    url: AjaxCallUrl.GetMarketListForOtherVariationsUrl.replace("pContentId", ContentId),
                    type: "GET",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        for (var i = 0; i < data.length; i++) {
                            //$("input.chk-market[data-market-id='" + data[i].MarketId + "']").attr("disabled", "disabled");

                            $("input.chk-market[data-market-id='" + data[i].MarketId + "']").data("content-id", data[i].ContentId);
                            $("input.chk-market[data-market-id='" + data[i].MarketId + "']").parent().css("color", "#ccc");
                        }
                    },
                    error: function (err) {
                        //console.log(err);
                    }
                });

                kendo.ui.progress($(document.body), false);
            },
            error: function (err) {
                //console.log(err);
            }
        });
    },

    SaveVariation: function (IsDefaultVariation) {
        //Ajax call to Save variation
        $.ajax({
            url: AjaxCallUrl.SaveVariationUrl,
            type: "POST",
            //dataType: "json",
            data: JSON.stringify({
                ContractId: $("#hdnContractId").val(),
                SectionId: $("li.li-content-section.selected").first().data("section-id"),
                ContentId: $("#hdnContentId").val(),
                DisplayValue: $("#txtVariationName").val()
            }),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if ($("#hdnContentId").val() !== "0") {
                    $("#ddlVariationList :selected").text($("#txtVariationName").val());
                } else {
                    $("#ddlVariationList :selected").removeAttr("selected");

                    var newVariation = '<option value="' + data + '" selected="selected">' + $("#txtVariationName").val() + '</option>';
                    $("#ddlVariationList").append(newVariation);
                    $("#hdnContentId").val(data);

                    //Clear Editor text and market list
                    $("#txtEdContractContent").data("kendoEditor").value("");

                    if (IsDefaultVariation == false) {
                        ContractContent.PopulateContentMarket(data);
                    }

                    ContractContent.PopulateContentAttachmentCount(data);
                    ContractContent.PopulateContentAttachmentList(data);

                    if ($('#ddlVariationList').children('option').length > 1) {
                        $("#lnkCopyContent").on('click', function () { $("#divCopyContent").modal({ backdrop: 'static', keyboard: false }); });
                        $("#lnkCopyContent").children("i").css("color", "").css("cursor", "");
                    } else {
                        $("#lnkCopyContent").children("i").css("color", "slategrey").css("cursor", "none");
                        $("#lnkCopyContent").off('click');
                    }
                }
            },
            error: function (err) {
                //console.log(err);
            }
        });

        $("#divVariation").modal("hide");
    },

    DeleteVariation: function () {
        kendo.ui.progress($(document.body), true);

        //Ajax call to Delete variation
        $.ajax({
            url: AjaxCallUrl.DeleteVariationUrl,
            type: "POST",
            data: JSON.stringify({
                ContentId: $("#hdnContentId").val(),
            }),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                popupNotification.show("Variation deleted successfully!", "info");

                var ContractId = $("#hdnContractId").val();
                var SectionId = $("li.li-content-section.selected").first().data("section-id");

                ContractContent.PopulateVariationList(ContractId, SectionId);

                kendo.ui.progress($(document.body), false);
            },
            error: function (err) {
                //console.log(err);
            }
        });
    },

    SaveContent: function () {
        //Ajax call to Save variation
        $.ajax({
            url: AjaxCallUrl.SaveContentUrl,
            type: "POST",
            //dataType: "json",
            data: JSON.stringify({
                ContentId: $("#hdnContentId").val(),
                ContentText: $("#txtEdContractContent").data("kendoEditor").value()
            }),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                ContractContent.SaveContentMarket();
            },
            error: function (err) {
                //console.log(err);
            }
        });
    },

    SaveContentMarket: function () {
        var arrMarketIdList = [];

        $("input.chk-market").each(function () {
            if ($(this).prop("checked")) {
                arrMarketIdList.push($(this).data("market-id"));
            }
        });

        if (arrMarketIdList.length > 0) {
            $.ajax({
                url: AjaxCallUrl.SaveContentMarketUrl,
                type: "POST",
                //dataType: "json",
                data: JSON.stringify({
                    ContentId: $("#hdnContentId").val(),
                    MarketIdList: arrMarketIdList
                }),
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    popupNotification.show("Variation Content saved successfully!", "info");
                },
                error: function (err) {
                    //console.log(err);
                }
            });
        } else {
            popupNotification.show("Variation Content saved successfully!", "info");
        }
    },

    DisassociateContentMarket: function (ContentId, MarketId) {
        //Ajax call to disassociate content market
        $.ajax({
            url: AjaxCallUrl.DisassociateContentMarketUrl,
            type: "POST",
            data: JSON.stringify({
                ContentId: ContentId,
                MarketId: MarketId
            }),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                $("input.chk-market[data-market-id='" + MarketId + "']").data("content-id", "0");
                $("input.chk-market[data-market-id='" + MarketId + "']").parent().css("color", "");

                popupNotification.show("Market disassociated!", "info");
            },
            error: function (err) {
                //console.log(err);
            }
        });
    },

    AddSection: function () {
        var SectionToAdd = $("#ddlSection :selected").text();
        var SectionToAddId = $("#ddlSection").val();
        var AboveBar = $("#ddlSection :selected").data("position");

        var SelectedSectionExists = $("li.li-content-section[data-section-id='" + SectionToAddId + "']").length;

        if (SelectedSectionExists > 0) {    //Selected Section exists in the UL list - set focus
            var SelectedSection = $("ul.ul-content-section").find("li.li-content-section[data-section-id='" + SectionToAddId + "']");
            
            if (SelectedSection.css("display") === "none") {
                SelectedSection.css("display", "block");
            } else {
                popupNotification.show("Section " + SectionToAdd + " already exists!", "info");
            }

            SelectedSection.click();
        }

        $("#divSection").modal("hide");
    },

    DeleteSection: function (pSectionId) {
        kendo.ui.progress($(document.body), true);

        var pContractId = $("#hdnContractId").val();
        //var pSectionId = $("li.li-content-section.selected").first().data("section-id");

        //Ajax call to Delete variation
        $.ajax({
            url: AjaxCallUrl.DeleteSectionUrl,
            type: "POST",
            data: JSON.stringify({
                ContractId: pContractId,
                SectionId: pSectionId
            }),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                popupNotification.show("Section deleted successfully!", "info");

                $("li.li-content-section[data-section-id='" + pSectionId + "']").first().hide();        //DO NOT REMOVE - JUST HIDE!
                $("ul.below-bar-section").children("li:visible").first().click();

                ContractContent.ContentSectionDeleteClicked = false;

                kendo.ui.progress($(document.body), false);
            },
            error: function (err) {
                //console.log(err);
            }
        });
    },

    PopulateContentAttachmentCount: function (ContentId) {

        //Ajax call to fetch content attachment count
        $.ajax({
            url: AjaxCallUrl.GetContentAttachmentCountUrl,
            type: "GET",
            data: { ContentId: ContentId },
            success: function (data) {
                if (parseInt(data) == 0) {
                    $("#lnkAttachmentsTab").html("Attachments");
                } else {
                    $("#lnkAttachmentsTab").html("Attachments (" + data + ")");
                }
            },
            error: function (err) {
                popupNotification.show("Error in PopulateContentAttachmentCount!", "error");
            }
        });

    },

    PopulateContentAttachmentList: function (ContentId) {

        //Ajax call to fetch content attachment list
        $.ajax({
            url: AjaxCallUrl.GetContentAttachmentListHTMLUrl,
            type: "GET",
            data: { ContentId: ContentId },
            success: function (data) {
                kendo.ui.progress($(document.body), false);
                $("#divContentAttachmentList").html(data);

                ContractContent.AssignContentAttachmentListControls();
            },
            error: function (err) {
                popupNotification.show("Error in PopulateContentAttachmentList!", "error");
            }
        });
    },

    PopulateAttachmentLibrary: function (ViewAs) {
        kendo.ui.progress($("#divAttachmentLibrary"), true);

        var URL = AjaxCallUrl.GetAttachmentLibraryHTMLUrl;

        if (parseInt(ViewAs) == 3) {
            URL = AjaxCallUrl.GetAttachmentLibraryHTMLFilterFileTypeUrl;
        }

        //Ajax call to fetch attachment list
        $.ajax({
            url: URL,
            type: "GET",
            data: {
                "SearchText": $("#txtAttachmentSearch").val()
            },
            success: function (data) {
                kendo.ui.progress($("#divAttachmentLibrary"), false);

                $("#divAttachmentListArea").html(data);
                ContractContent.AssignAttachmentListControls();
            },
            error: function (err) {
                popupNotification.show("Error in GetAttachmentList!", "error");
            }
        });
    },

    SaveAttachmentDetails: function () {
        kendo.ui.progress($("#divAttachmentLibrary"), true);

        //Ajax call to Save attachment library details
        $.ajax({
            url: AjaxCallUrl.SaveAttachmentDetailsUrl,
            type: "POST",
            data: JSON.stringify({
                AttachmentId: $("#hdnSelectedAttachmentId").val(),
                AttachmentTitle: $("#txtAttachmentTitle").val(),
                AttachmentDescription: $("#txtAttacmentDesc").val(),
                AttachmentVersion: $("#txtAttachmentVersion").val()
            }),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                kendo.ui.progress($("#divAttachmentLibrary"), false);

                $("#h3FileTitle").text($("#txtAttachmentTitle").val());
                $("#pAttachmentDescription").text($("#txtAttacmentDesc").val());
                $("#pAttachmentVersion").text($("#txtAttachmentVersion").val());

                $("#divEditAttachmentDetails").hide();
                $("#divLibAttachmentDetails").show();

                $("#hdnLastSavedAttachmentId").val($("#hdnSelectedAttachmentId").val());

                ContractContent.PopulateAttachmentLibrary(1);
                popupNotification.show("Attachment details saved successfully!", "info");
            },
            error: function (err) {
                //console.log(err);
            }
        });
    },

    DeleteAttachment: function () {
        kendo.ui.progress($("#divAttachmentLibrary"), true);

        //Ajax call to delete attachment library item
        $.ajax({
            url: AjaxCallUrl.DeleteAttachmentUrl,
            type: "POST",
            data: JSON.stringify({
                AttachmentId: $("#hdnSelectedAttachmentId").val()
            }),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                kendo.ui.progress($("#divAttachmentLibrary"), false);

                ContractContent.PopulateAttachmentLibrary(1);
                popupNotification.show("Attachment deleted successfully!", "info");
            },
            error: function (err) {
                popupNotification.show("Error in DeleteAttachment!", "error");
                //console.log(err);
            }
        });
    },

    SaveContentAttachment: function () {
        kendo.ui.progress($(document.body), true);

        var ExternalURL = $("#txtExternalURL").val();
        var AttachmentId = $("#hdnSelectedAttachmentId").val();

        if (ExternalURL != "") {
            AttachmentId = 0;
        }

        //Ajax call to Save content attachment details
        $.ajax({
            url: AjaxCallUrl.SaveContentAttachmentUrl,
            type: "POST",
            data: JSON.stringify({
                ContentAttachmentId: $("#hdnContentAttachmentId").val(),
                ContentId: $("#hdnContentId").val(),
                AttachmentId: AttachmentId,
                ContentAttachmentTitle: $("#txtContentAttachmentTitle").val(),
                ContentAttachmentDescription: $("#txtContentAttachmentDesc").val(),
                ContentAttachmentVersion: $("#txtContentAttachmentVersion").val(),
                ExternalURL: $("#txtExternalURL").val()
            }),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                kendo.ui.progress($(document.body), false);

                $("#divNewAttachment").hide();
                $("#btnAddNewAttachment").removeAttr("disabled").removeClass("disabled");

                ContractContent.PopulateContentAttachmentCount($("#hdnContentId").val());
                ContractContent.PopulateContentAttachmentList($("#hdnContentId").val());
                popupNotification.show("Content Attachment saved successfully!", "info");
            },
            error: function (err) {
                //console.log(err);
            }
        });
    },

    DisassociateContentAttachment: function (ContentAttachmentId) {
        kendo.ui.progress($(document.body), true);

        //Ajax call to delete content attachment item
        $.ajax({
            url: AjaxCallUrl.DeleteContentAttachmentUrl,
            type: "POST",
            data: JSON.stringify({
                ContentAttachmentId: ContentAttachmentId
            }),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                kendo.ui.progress($(document.body), false);

                ContractContent.PopulateContentAttachmentCount($("#hdnContentId").val());
                ContractContent.PopulateContentAttachmentList($("#hdnContentId").val());
                popupNotification.show("Attachment deleted successfully!", "info");
            },
            error: function (err) {
                popupNotification.show("Error in DisassociateContentAttachment!", "error");
                //console.log(err);
            }
        });
    },

    CopyVariation: function () {
        //Ajax call to Copy Variation
        $.ajax({
            url: AjaxCallUrl.CopyVariationUrl,
            type: "POST",
            //dataType: "json",
            data: JSON.stringify({
                CopyFromContentId: $("#ddlCopyFromVariation :selected").val(),
                CopyToContentId: $("#hdnContentId").val()
            }),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                ContractContent.PopulateContent($("#hdnContentId").val());
                ContractContent.PopulateContentMarket($("#hdnContentId").val());
                ContractContent.PopulateContentAttachmentCount($("#hdnContentId").val());
                ContractContent.PopulateContentAttachmentList($("#hdnContentId").val());

                popupNotification.show("Content copied successfully!", "info");
            },
            error: function (err) {
                popupNotification.show("Error in CopyVariation!", "error");
                //console.log(err);
            }
        });

        $("#divCopyContent").modal("hide");
    }
};

function onFileSelect(e) {
    var fileName = e.files[0].name;
    var fileExtension = fileName.substring(fileName.length - 4, fileName.length);

    if (fileExtension != '.doc' && fileExtension != 'docx' && fileExtension != '.pdf' && fileExtension != '.xls' && fileExtension != 'xlsx' && fileExtension != '.ppt') {
        alert("Files of type .doc/.docx/.pdf/.xlsx/.xls/.ppt/.ppx are allowed");
        e.preventDefault();
    }
}

function onProgress(e) {
    $("strong.k-upload-status-total").css("display", "none");
}

function onSuccess(e) {
    var LastSavedAttachmentId = e.response;
    $("#hdnLastSavedAttachmentId").val(LastSavedAttachmentId);    
}

function onAttachmentUploadComplete(e) {
    popupNotification.show("Attachment file uploaded successfully!", "info");
    ContractContent.PopulateAttachmentLibrary(1);
}

function onShow(e) {
    if (e.sender.getNotifications().length == 1) {
        var element = e.element.parent(),
            eWidth = element.width(),
            eHeight = element.height(),
            wWidth = $(window).width(),
            wHeight = $(window).height(),
            newTop, newLeft, newWidth;

        newLeft = Math.floor(wWidth - eWidth) - 100;
        newTop = 100;

        e.element.parent().css({ top: newTop, left: newLeft });
    }
}
