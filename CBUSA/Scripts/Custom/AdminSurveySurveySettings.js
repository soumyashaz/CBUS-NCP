$(document).ready(function () {


    // SurveyAddQuestion.AssignControl();
    SurveySettings.AssignControl();


});

var SurveySettings = {
    AssignControl: function () {
        $("#a_ConfigureInviteEmail").on("click", SurveySettings.ConfigureInviteEmailPopup);
        $("#a_ConfigureRemainderEmail").on("click", SurveySettings.ConfigureRemainderEmailPopup);
        $("#a_ConfigureSaveContinueEmail").on("click", SurveySettings.ConfigureSaveContinueEmailPopup);
        $("#BtnSurveyEmailStiing").on("click", SurveySettings.SaveSurveyEmailSettings);

    },


    SaveSurveyEmailSettings: function () {
        var ValidationFrom = SurveySettings.VaidateSurveyEmailsetting();
        if (!ValidationFrom) {
            return;
        }


        kendo.ui.progress($("#DivSurveymainContainer"), true);
        var TakeSurvey = $("input[name='RemainderForTakeSurvey']:checked").length > 0;
        var TakeSurveySecond = $("input[name='RemainderForTakeSurveySecond']:checked").length > 0;
        var TakeSurveyThird = $("input[name='RemainderForTakeSurveyThird']:checked").length > 0;
        var ContinueSurvey = $("input[name='RemainderForContinueSurvey']:checked").length > 0;
        var ObjVm = {
            SurveyId: $("#HdnSurveyId").val(), SenderEmail: $("#SenderEmail").val(), RemainderForTakeSurvey: TakeSurvey,
            DayBeforeSurveyEnd: $("#DayBeforeSurveyEnd").val(), RemainderForContinueSurvey: ContinueSurvey, DayAfterSurveyEnd: $("#DayAfterSurveyEnd").val(),
            InviteEmailDumpId: $("#HdnInviteEmailDumpId").val(), RemainderEmailDumpId: $("#HdnRemainderEmailDumpId").val(),
            ContinueEmailDumpId: $("#HdnContinueEmailDumpId").val(),
            RemainderForTakeSurveySecond: TakeSurveySecond,
            DayBeforeSurveyEndSecond: $("#DayBeforeSurveyEndSecond").val(),
            RemainderForTakeSurveyThird: TakeSurveyThird,
            DayBeforeSurveyEndThird: $("#DayBeforeSurveyEndThird").val()
        };
        var postData = {
            ObjVm: ObjVm
        }; //done
        $.ajax({ //Process the form using $.ajax()
            type: 'POST', //Method type
            url: AjaxCallUrl.SaveSurveyEmailSettingsUrl,
            data: postData, //Forms name
            dataType: 'json',
            // traditional: true,
            beforeSend: function () {
            },
            success: function (data) {
                if (data.IsSuccess) {
                    kendo.ui.progress($("#DivSurveymainContainer"), false);
                    var DataUpdateWindow = $("#DataUpdateMessage").data("kendoWindow");

                    //  ContractBuilder.LoadContractBuilder();
                    if (DataUpdateWindow) {
                        DataUpdateWindow.title("Update Information");
                        DataUpdateWindow.content("<div class='msgboxSuccess'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i> Data Update Successfully</div>");
                        DataUpdateWindow.open();
                        //redirect to listing page
                        setTimeout(function () {
                            window.location.href = AjaxCallUrl.PreviewQustionUrl.replace("****SurveyId****", $("#HdnSurveyId").val());
                        }, 800)
                    }

                }
                else {
                    kendo.ui.progress($("#DivSurveymainContainer"), false);
                    if (DataUpdateWindow) {
                        DataUpdateWindow.title("Error Information");
                        DataUpdateWindow.content("<div class='msgboxError'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i>" + data.ModelError + " </div>");
                        DataUpdateWindow.open();
                    }
                }
            },
            error: function (request, error) {
                kendo.ui.progress($("#DivSurveymainContainer"), false);
                if (DataUpdateWindow) {
                    DataUpdateWindow.title("Error Information");
                    DataUpdateWindow.content("<div class='msgboxError'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i>" + data.ModelError + " </div>");
                    DataUpdateWindow.open();
                }
            }
        });
    },

    VaidateSurveyEmailsetting: function () {
        var Validator = $("#DivSurveymainContainer").kendoValidator({
            messages: {
                required: "*",
                email: function (input) {
                    "Please enter valid email"
                },
            },
            rules: {
                required: function (textarea) {

                    if (textarea.val() == "") {

                        return false;
                    }

                    return true;
                }

            }



        }).data("kendoValidator");

        return Validator.validate();
    },

    ConfigureInviteEmailPopup: function () {
        //    console.log('hhh');

        var wnd = $("#WndConfigureInviteEmailPopup").data("kendoWindow");
        wnd.refresh({
            url: AjaxCallUrl.LoadSurveyInviteEmailUrl, // returns JSON, { firstName: "Alyx", lastName: "Vance" }
            data: { SurveyId: $("#HdnSurveyId").val(), DumpId: $("#HdnInviteEmailDumpId").val() },
        });
        wnd.open().center();
    },

    OnOpenConfigureInviteEmailPopup: function (e) {
        var wnd = $("#WndConfigureInviteEmailPopup").data("kendoWindow");
        kendo.ui.progress(wnd.element, true);
        SurveySettings.AddEventInviteEmail();
    },


    OnRefreshConfigureInviteEmailPopup: function (e) {
        var wnd = $("#WndConfigureInviteEmailPopup").data("kendoWindow");
        kendo.ui.progress(wnd.element, false);
        // AssignEventContractResource();
        SurveySettings.AddEventInviteEmail();

    },
    AddEventInviteEmail: function () {
        //var fullUrl = window.location.pathname;

        //alert($("#hdnUrlRoot").val());
        $("#InviteEmailBody").kendoEditor({            
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
                //"insertFile",
                "subscript",
                "superscript",
                "createTable",
                "addRowAbove",
                "addRowBelow",
                "addColumnLeft",
                "addColumnRight",
                "deleteRow",
                "deleteColumn",
                "viewHtml",
                "formatting",
                //"cleanFormatting",
               "fontName",
                "fontSize",
                "foreColor",
                "backColor",
                "imageBrowser"              
            ],            
            imageBrowser: {
                transport: {
                    read: AjaxCallUrl.EditorImageRead,
                    create: AjaxCallUrl.EditorImageCreate ,
                    uploadUrl: AjaxCallUrl.EditorImageUpload,
                    thumbnailUrl: AjaxCallUrl.EditorImageThumbnail,
                    imageUrl: GetBaseUrl() + "ApplicationDocument/EditorImage/{0}"
                }
            },
        });
        $("#BtnSaveInviteEmail").on("click", SurveySettings.SaveInviteEmailBody);

        $("#BtncancelInviteEmail").on("click", SurveySettings.CancelPopUp);
    },
    ///Remainder email poup portion

    SaveInviteEmailBody: function () {
        
        var Validator = $("#DivConfigureInviteEmail").kendoValidator({
            messages: {
                required: "*"
            },
            rules: {
                required: function (textarea) {

                    if (textarea.val() == "") {

                        return false;
                    }

                    return true;
                }
                //maxTextLength: function (textarea) {
                //    if (textarea.val() == "") {
                //        var maxlength = textarea.attr("data-maxtextlength");
                //        var value = textarea.data("kendoEditor").value();
                //        //return value.replace(/<[^>]+>/g, "").length <= maxlength;
                //        return false;
                //    }

                //    return true;
                //},
                //maxHtmlLength: function (textarea) {
                //    if (textarea.val() == "") {
                //        var maxlength = textarea.attr("data-maxhtmllength");
                //        var value = textarea.data("kendoEditor").value();
                //        //  return value.length <= maxlength;
                //        return false;
                //    }

                //    return true;
                //}
            }



        }).data("kendoValidator");

        if (Validator.validate()) {

            kendo.ui.progress($("#DivConfigureInviteEmail"), true);
            var editor = $("#InviteEmailBody").data("kendoEditor");

            // var PostData = { EmailSubject: $("#EmailSubject").val(), EmailBody: editor.value(), DumpId: $("#HdnInviteEmailDumpId").val() };
            var PostData = new FormData();
            PostData.append('InviteEmailSubject', $("#InviteEmailSubject").val());
            PostData.append('InviteEmailBody', editor.value());
            PostData.append('DumpId', $("#HdnInviteEmailDumpId").val());
            PostData.append('SurveyId', $("#HdnSurveyId").val());

            $.ajax({ //Process the form using $.ajax()
                type: 'POST', //Method type
                url: AjaxCallUrl.SaveSurveyInviteEmailUrl,
                data: PostData, //Forms name
                dataType: 'json',
                // traditional: true,
                contentType: false, // Not to set any content header  
                processData: false,
                beforeSend: function () {
                },
                success: function (data) {

                    var DataUpdateWindow = $("#DataUpdateMessage").data("kendoWindow");
                    if (data.IsSuccess) {
                        if ($("#HdnInviteEmailDumpId").val() == "0") {
                            $("#HdnInviteEmailDumpId").val(data.DumpId);
                        }


                        kendo.ui.progress($("#DivConfigureInviteEmail"), false);
                        if (DataUpdateWindow) {

                            DataUpdateWindow.title("Update Information");
                            DataUpdateWindow.content("<div class='msgboxSuccess'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i> Data Update Successfully</div>");
                            DataUpdateWindow.open();


                        }
                    }
                    else {
                        kendo.ui.progress($("#DivConfigureInviteEmail"), false);
                        if (DataUpdateWindow) {
                            DataUpdateWindow.title("Error Information");
                            DataUpdateWindow.content("<div class='msgboxError'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i>" + data.ModelError + " </div>");
                            DataUpdateWindow.open();
                        }
                    }
                },
                error: function (request, error) {
                    kendo.ui.progress($("#DivConfigureInviteEmail"), false);
                }
            });

        }
    },


    ConfigureRemainderEmailPopup: function () {
        var wnd = $("#WndConfigureRemainderEmailPopup").data("kendoWindow");
        wnd.refresh({
            url: AjaxCallUrl.LoadSurveyRemainderEmailUrl, // returns JSON, { firstName: "Alyx", lastName: "Vance" }
            data: { SurveyId: $("#HdnSurveyId").val(), DumpId: $("#HdnRemainderEmailDumpId").val() }
        });
        wnd.open().center();
    },

    OnOpenConfigureRemainderEmailPopup: function (e) {
        var wnd = $("#WndConfigureRemainderEmailPopup").data("kendoWindow");
        kendo.ui.progress(wnd.element, true);
    },


    OnRefreshConfigureRemainderEmailPopup: function (e) {
        var wnd = $("#WndConfigureRemainderEmailPopup").data("kendoWindow");
        kendo.ui.progress(wnd.element, false);
        // AssignEventContractResource();
        SurveySettings.AddEventRemainderEmail();

    },
    AddEventRemainderEmail: function () {
        $("#RemainderEmailBody").kendoEditor({
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
                //"insertFile",
                "subscript",
                "superscript",
                "createTable",
                "addRowAbove",
                "addRowBelow",
                "addColumnLeft",
                "addColumnRight",
                "deleteRow",
                "deleteColumn",
                "viewHtml",
                "formatting",
                //"cleanFormatting",
               "fontName",
                "fontSize",
                "foreColor",
                "backColor",
                //"print"
            ],            
            imageBrowser: {
                transport: {
                    read: AjaxCallUrl.EditorImageRead,
                    create: AjaxCallUrl.EditorImageCreate ,
                    uploadUrl: AjaxCallUrl.EditorImageUpload,
                    thumbnailUrl: AjaxCallUrl.EditorImageThumbnail,
                    imageUrl: GetBaseUrl() + "ApplicationDocument/EditorImage/{0}"
                }
            },
        });

        $("#BtnSaveRemainderEmail").on("click", SurveySettings.SaveRemainderEmailBody);
        $("#BtncancelRemainder").on("click", SurveySettings.CancelPopUp);
    },

    SaveRemainderEmailBody: function () {

        var Validator = $("#DivConfigureRemainderEmail").kendoValidator({
            messages: {
                required: "*"
            },
            rules: {
                required: function (textarea) {

                    if (textarea.val() == "") {

                        return false;
                    }

                    return true;
                }

            }



        }).data("kendoValidator");

        if (Validator.validate()) {

            kendo.ui.progress($("#DivConfigureRemainderEmail"), true);
            var editor = $("#RemainderEmailBody").data("kendoEditor");

            // var PostData = { EmailSubject: $("#EmailSubject").val(), EmailBody: editor.value(), DumpId: $("#HdnInviteEmailDumpId").val() };
            var PostData = new FormData();
            PostData.append('RemainderEmailSubject', $("#RemainderEmailSubject").val());
            PostData.append('RemainderEmailBody', editor.value());
            PostData.append('DumpId', $("#HdnRemainderEmailDumpId").val());
            PostData.append('SurveyId', $("#HdnSurveyId").val());
            $.ajax({ //Process the form using $.ajax()
                type: 'POST', //Method type
                url: AjaxCallUrl.SaveSurveyRemainderEmailUrl,
                data: PostData, //Forms name
                dataType: 'json',
                // traditional: true,
                contentType: false, // Not to set any content header  
                processData: false,
                beforeSend: function () {
                },
                success: function (data) {

                    var DataUpdateWindow = $("#DataUpdateMessage").data("kendoWindow");
                    if (data.IsSuccess) {
                        if ($("#HdnRemainderEmailDumpId").val() == "0") {
                            $("#HdnRemainderEmailDumpId").val(data.DumpId);
                        }


                        kendo.ui.progress($("#DivConfigureRemainderEmail"), false);
                        if (DataUpdateWindow) {

                            DataUpdateWindow.title("Update Information");
                            DataUpdateWindow.content("<div class='msgboxSuccess'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i> Data Update Successfully</div>");
                            DataUpdateWindow.open();


                        }
                    }
                    else {
                        kendo.ui.progress($("#DivConfigureRemainderEmail"), false);
                        if (DataUpdateWindow) {
                            DataUpdateWindow.title("Error Information");
                            DataUpdateWindow.content("<div class='msgboxError'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i>" + data.ModelError + " </div>");
                            DataUpdateWindow.open();
                        }
                    }
                },
                error: function (request, error) {
                    kendo.ui.progress($("#DivConfigureRemainderEmail"), false);
                }
            });

        }

    },



    ///Remainder Save and continue poup portion

    ConfigureSaveContinueEmailPopup: function () {
        var wnd = $("#WndConfigureSaveContinueEmailPopup").data("kendoWindow");
        wnd.refresh({
            url: AjaxCallUrl.LoadSurveyInviteSaveandContinuelUrl, // returns JSON, { firstName: "Alyx", lastName: "Vance" }
            data: { SurveyId: $("#HdnSurveyId").val(), DumpId: $("#HdnContinueEmailDumpId").val() }
        });
        wnd.open().center();
    },

    OnOpenConfigureSaveContinueEmailPopup: function (e) {
        var wnd = $("#WndConfigureSaveContinueEmailPopup").data("kendoWindow");
        kendo.ui.progress(wnd.element, true);
    },


    OnRefreshConfigureSaveContinueEmailPopup: function (e) {
        var wnd = $("#WndConfigureSaveContinueEmailPopup").data("kendoWindow");
        kendo.ui.progress(wnd.element, false);
        // AssignEventContractResource();
        SurveySettings.AddEventSaveContinueEmail();

    },

    AddEventSaveContinueEmail: function () {
        $("#SaveContinueEmailBody").kendoEditor({
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
                //"insertFile",
                "subscript",
                "superscript",
                "createTable",
                "addRowAbove",
                "addRowBelow",
                "addColumnLeft",
                "addColumnRight",
                "deleteRow",
                "deleteColumn",
                "viewHtml",
                "formatting",
                //"cleanFormatting",
               "fontName",
                "fontSize",
                "foreColor",
                "backColor",
                //"print"
            ],            
            imageBrowser: {
                transport: {
                    read: AjaxCallUrl.EditorImageRead,
                    create: AjaxCallUrl.EditorImageCreate ,
                    uploadUrl: AjaxCallUrl.EditorImageUpload,
                    thumbnailUrl: AjaxCallUrl.EditorImageThumbnail,
                    imageUrl: GetBaseUrl() + "ApplicationDocument/EditorImage/{0}"
                }
            },
        });

        $("#BtnSaveContinueEmail").on("click", SurveySettings.SaveContinueEmailBody);
        $("#BtncancelSaveContinue").on("click", SurveySettings.CancelPopUp);
    },
    SaveContinueEmailBody: function () {

        var Validator = $("#DivConfigureSaveContinueEmail").kendoValidator({
            messages: {
                required: "*"
            },
            rules: {
                required: function (textarea) {

                    if (textarea.val() == "") {

                        return false;
                    }

                    return true;
                }

            }



        }).data("kendoValidator");

        if (Validator.validate()) {

            kendo.ui.progress($("#DivConfigureSaveContinueEmail"), true);
            var editor = $("#SaveContinueEmailBody").data("kendoEditor");

            // var PostData = { EmailSubject: $("#EmailSubject").val(), EmailBody: editor.value(), DumpId: $("#HdnInviteEmailDumpId").val() };
            var PostData = new FormData();
            PostData.append('SaveContinueEmailSubject', $("#SaveContinueEmailSubject").val());
            PostData.append('SaveContinueEmailBody', editor.value());
            PostData.append('DumpId', $("#HdnContinueEmailDumpId").val());
            PostData.append('SurveyId', $("#HdnSurveyId").val());
            $.ajax({ //Process the form using $.ajax()
                type: 'POST', //Method type
                url: AjaxCallUrl.SaveSurveySaveContinueEmailUrl,
                data: PostData, //Forms name
                dataType: 'json',
                // traditional: true,
                contentType: false, // Not to set any content header  
                processData: false,
                beforeSend: function () {
                },
                success: function (data) {

                    var DataUpdateWindow = $("#DataUpdateMessage").data("kendoWindow");
                    if (data.IsSuccess) {
                        if ($("#HdnContinueEmailDumpId").val() == "0") {
                            $("#HdnContinueEmailDumpId").val(data.DumpId);
                        }


                        kendo.ui.progress($("#DivConfigureSaveContinueEmail"), false);
                        if (DataUpdateWindow) {

                            DataUpdateWindow.title("Update Information");
                            DataUpdateWindow.content("<div class='msgboxSuccess'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i> Data Update Successfully</div>");
                            DataUpdateWindow.open();


                        }
                    }
                    else {
                        kendo.ui.progress($("#DivConfigureSaveContinueEmail"), false);
                        if (DataUpdateWindow) {
                            DataUpdateWindow.title("Error Information");
                            DataUpdateWindow.content("<div class='msgboxError'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i>" + data.ModelError + " </div>");
                            DataUpdateWindow.open();
                        }
                    }
                },
                error: function (request, error) {
                    kendo.ui.progress($("#DivConfigureSaveContinueEmail"), false);
                }
            });

        }
    },

    CancelPopUp: function () {

        var id = $(this).attr('id');
        if (id == "BtncancelInviteEmail") {

            var DataUpdateWindow = $("#WndConfigureInviteEmailPopup").data("kendoWindow");
            if (DataUpdateWindow) {
                DataUpdateWindow.close();
            }

        }
        else if (id == "BtncancelRemainder") {

            var DataUpdateWindow = $("#WndConfigureRemainderEmailPopup").data("kendoWindow");
            if (DataUpdateWindow) {
                DataUpdateWindow.close();
            }
        }
        else if (id == "BtncancelSaveContinue") {

            var DataUpdateWindow = $("#WndConfigureSaveContinueEmailPopup").data("kendoWindow");
            if (DataUpdateWindow) {
                DataUpdateWindow.close();
            }
        }



    }


}