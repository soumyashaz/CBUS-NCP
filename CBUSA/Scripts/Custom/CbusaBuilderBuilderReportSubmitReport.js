$(document).ready(function () {    
    BuilderReport.AssignControl();
    BuilderReport.LoadAdminProjectReport();

});


var BuilderReport = {

    AssignControl: function () {
        $("#DivBuilderContractProjectDropdown").on("click", BuilderReport.LoadProjectCustomDropDown)
        $("#BtnSave").on("click", BuilderReport.SaveAdminReport)
        $("#BtnCancel").on("click", BuilderReport.BackBtn_Cancel)
        $("#BtnBack").on("click", BuilderReport.BackBtn_Click)
        $("#addproj").on("click", BuilderReport.redirectaddproject)
    },
    redirectaddproject: function () {
        var url = AjaxCallUrl.AddProjectRedirectUrl;
        window.location.href = url;


    },
    LoadProjectCustomDropDown: function () {
        CustomControlProject($(this).parent().parent(), "CustomControlPoupProject");
    },

    LoadAdminProjectReport: function () {        
        kendo.ui.progress($("#DivAdminReportGenarate"), true);
        var postData = { ContractId: $("#HdnContractId").val(), BuilderId: $("#HdnBuilderId").val(), QuaterId: $("#HdnQuaterId").val() };        
        $.ajax({ //Process the form using $.ajax()
            type: 'POST', //Method type
            url: AjaxCallUrl.LoadBuilderReportViewUrl,
            data: postData, //Forms name
            dataType: 'json',
            // traditional: true,
            //beforeSend: function () {
            //},
            success: function (data) {
                if (data.IsSuccess) {

                    $("#DivAdminReportGenarate").append(data.ProjectCustomControl);
                    kendo.ui.progress($("#DivAdminReportGenarate"), false);
                    AddEventForQuestion();
                }
                else {
                    var DataUpdateWindow = $("#DataUpdateMessage").data("kendoWindow");
                    if (DataUpdateWindow) {
                        DataUpdateWindow.title("Error Information");
                        DataUpdateWindow.content("<div class='msgboxError'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i>" + data.ModelError + " </div>");
                        DataUpdateWindow.open();
                    }
                    kendo.ui.progress($("#DivAdminReportGenarate"), false);
                }
            },
            error: function (request, error) {
                kendo.ui.progress($("#DivAdminReportGenarate"), false);
            }
        });
        //alert('called ajax finished');
    },

    IsAnswareValidate: function (ShownDiv) {
        // var ShownDiv = $("div[id^='Div_']:visible");
        var MaxMin = { Max: 0, Min: 0 };
        var AllowedCharecter = { Alphabet: 0, Number: 0, SpecialCharecter: 0 };
        var Validator = $(ShownDiv).kendoValidator({
            messages: {
                required: "*",
                email: function (input) {
                    return "Please enter correct email"
                },
                numberrange: function (input) {
                    return 'Please enter number between ' + MaxMin.Min + ' and ' + MaxMin.Max + ' '
                },
                allowedcharecter: function (input) {
                    if (AllowedCharecter.Alphabet == 1) {
                        return 'Alphabet is not allowed '
                    }
                    else if (AllowedCharecter.Number == 1) {
                        return 'Number is not allowed '
                    }
                    else if (AllowedCharecter.SpecialCharecter == 1) {
                        return 'Special charecter is not allowed '
                    }
                },
            },
            rules: {
                required: function (input) {
                    if (!input.is(":hidden")) {
                        var validate = input.data('check');
                        //console.log(validate);
                        if (typeof validate !== 'undefined' && validate !== false) {
                            var QuestionType = ShownDiv.children("input[name='HdnQuestionType']").val();
                            // console.log(QuestionType);
                            if (QuestionType == "1") //Text Box type
                            {
                                // var TextBoxType = ShownDiv.children("input[name='HdnTextBoxTypeId']").val();
                                var IsMandatory = ShownDiv.children("input[name='HdnIsMandatory']");
                                //  console.log(IsMandatory);
                                if (IsMandatory.length) {
                                    //  console.log(IsMandatory.val());
                                    if (IsMandatory.val().toLowerCase() == "true") {
                                        return $.trim(input.val()).length > 0;
                                    }
                                }
                            }
                        }
                        return true;
                    }
                    return true;
                },
                numberrange: function (input) {
                    if (!input.is(":hidden")) {
                        var validate = input.data('check');
                        //console.log(validate);
                        if (typeof validate !== 'undefined' && validate !== false) {
                            //  console.log('1');
                            // console.log('hh3');
                            var QuestionType = ShownDiv.children("input[name='HdnQuestionType']").val();
                            if (QuestionType == "1") //Text Box type
                            {
                                //  console.log('2');
                                var TextBoxType = ShownDiv.children("input[name='HdnTextBoxTypeId']");
                                // console.log(TextBoxType.val() );
                                if (TextBoxType.length) {

                                    //  console.log('3');
                                    if (TextBoxType.val() == "2" && $.trim(input.val()).length > 0) {   //for number
                                        var NumberRange = ShownDiv.children("input[name='HdnIsNumberLimit']").val().split(',');
                                        //  console.log('ffff');
                                        MaxMin.Min = NumberRange[0];
                                        MaxMin.Max = parseInt(NumberRange[1]);

                                        if (parseInt(NumberRange[0]) == 0 && parseInt(NumberRange[1]) == 0) {
                                            return true;
                                        }
                                        else {
                                            return parseInt(input.val()) > parseInt(NumberRange[0]) && parseInt(input.val()) < parseInt(NumberRange[1]);
                                        }

                                        //return parseInt(input.val()) > parseInt(NumberRange[0]) && parseInt(input.val()) < parseInt(NumberRange[1]);
                                    }
                                    return true;
                                }
                            }
                            return true;
                        }
                        return true;
                    }
                    return true;
                },
                allowedcharecter: function (input) {
                    if (!input.is(":hidden")) {
                        var validate = input.data('check');
                        //console.log(validate);
                        if (typeof validate !== 'undefined' && validate !== false) {
                            // console.log('hh3');
                            var QuestionType = ShownDiv.children("input[name='HdnQuestionType']").val();
                            if (QuestionType == "1") //Text Box type
                            {
                                var TextBoxType = ShownDiv.children("input[name='HdnTextBoxTypeId']");

                                if (TextBoxType.length) {

                                    if (TextBoxType.val() == "1" && $.trim(input.val()).length > 0) {   //for Text
                                        var AllowedCharecterList = ShownDiv.children("input[name='HdnAllowOnly']").val().split(',');
                                        if (AllowedCharecterList.length == 3) {
                                            var IsCharecterValid = true;
                                            if (AllowedCharecterList[0].toLowerCase() == "false") {

                                                var regex = new RegExp("[a-zA-Z]");
                                                if (regex.test(input.val())) {
                                                    //  IsCharecterValid = false;
                                                    AllowedCharecter.Alphabet = 1;
                                                    return false;
                                                }
                                            }
                                            if (AllowedCharecterList[1].toLowerCase() == "false") {

                                                var regex = new RegExp("[1-9]");
                                                if (regex.test(input.val())) {
                                                    //  IsCharecterValid = false;
                                                    console.log('kk');
                                                    AllowedCharecter.Number = 1;
                                                    return false;
                                                }
                                            }
                                            if (AllowedCharecterList[2].toLowerCase() == "false") {
                                                // console.log('3');
                                                var specialChars = "<>@!#$%^&*()_+[]{}?:;|'\"\\,./~`-="
                                                for (i = 0; i < specialChars.length; i++) {
                                                    if (input.val().indexOf(specialChars[i]) > -1) {
                                                        AllowedCharecter.SpecialCharecter = 1;
                                                        return false
                                                    }
                                                }
                                            }
                                        }
                                        return true;
                                    }
                                    return true;
                                }
                            }
                            return true;
                        }
                        return true;
                    }
                    return true;

                }
            }
        }).data("kendoValidator");
        var IsValid = Validator.validate();

        if (IsValid) {

            return { IsValid: true };
        }
        //else {
        //    var Div = Validator.element[0];
        //    return { IsValid: false, ParentDiv: $(Div).attr('id') };
        //}

    }
    ,
    SaveAdminReport: function () {
        //   var v = $("#TblAdminReport").children().find("td[data-role='TdAdminReport']").eq;
        //   console.log(v.length);
        $("#BtnSave").attr('disabled','disabled');
        kendo.ui.progress($("#BuilderContractQuaterReport"), true);
       
        $("#BtnSave").text('Processing ...');
        var IsValidationSuccess = true;
        $("#TblAdminReport").children().find("td[data-role='TdAdminReport']").each(function () {
            var Valid = BuilderReport.IsAnswareValidate($(this));
            if (!Valid) {
                IsValidationSuccess = false;
            }
            // console.log('rr');
        })
       
        if (IsValidationSuccess) {
            BuilderReport.SaveAdminReportResult();
        }
        else
        {

            $("#BtnSave").removeAttr('disabled');
            $("#BtnSave").text('SAVE');
            kendo.ui.progress($("#BuilderContractQuaterReport"), false);
        }
    },

    AddProjectToReport: function (ProjectId) {
        //console.log(ProjectId);
        kendo.ui.progress($("#DivAdminReportGenarate"), true);
        var postData = { ContractId: $("#HdnContractId").val(), BuilderId: $("#HdnBuilderId").val(), QuaterId: $("#HdnQuaterId").val(), ProjectId: ProjectId };
        $.ajax({ //Process the form using $.ajax()
            type: 'POST', //Method type
            url: AjaxCallUrl.AddProjectAdminReportUrl,
            data: postData, //Forms name
            dataType: 'json',
            // traditional: true,
            beforeSend: function () {
            },
            success: function (data) {
                $("#TblAdminReport").children('tbody').append(data.ProjectCustomControl);
                kendo.ui.progress($("#DivAdminReportGenarate"), false);
                AddEventForQuestion();
            },
            error: function (request, error) {
                kendo.ui.progress($("#DivAdminReportGenarate"), false);
            }
        });

    },

     AddMultipleProjectToReport: function (ListProjectId) {
     //   console.log(ListProjectId);
      //  return;

        kendo.ui.progress($("#DivAdminReportGenarate"), true);
        var postData = { ContractId: $("#HdnContractId").val(), BuilderId: $("#HdnBuilderId").val(), QuaterId: $("#HdnQuaterId").val(), ListProjectId: ListProjectId };
        $.ajax({ //Process the form using $.ajax()
            type: 'POST', //Method type
            url: AjaxCallUrl.AddMultipleProjectAdminReportUrl,
            data: postData, //Forms name
            dataType: 'json',
            // traditional: true,
            beforeSend: function () {
            },
            success: function (data) {
                $("#TblAdminReport").children('tbody').append(data.ProjectCustomControl);
                kendo.ui.progress($("#DivAdminReportGenarate"), false);
                AddEventForQuestion();
            },
            error: function (request, error) {
                kendo.ui.progress($("#DivAdminReportGenarate"), false);
            }
        });

    },
     SaveAdminReportResult: function () {

      //  kendo.ui.progress($("#TblAdminReport"), true);
        //  var DivAll = $("div[id^='Div_']");
        //  var CurrentDiv = $("div[id^='Div_']:visible");
        //  var CurrentDivNo = CurrentDiv.children("input[name='HdnCurrentQuestionOrder']").val();
        //  var Flag = 1;
      //  $("#BtnSave").attr('disabled', 'disabled');
      //  $("#BtnSave").text('Processing ...');
        var SurveyAdminReport = [];
        var SurveyAdminReportProject = [];
        $("#TblAdminReport").children().find("td[data-role='TdAdminReport']").each(function () {
            var QuestionType = $(this).children("input[name='HdnQuestionType']").val();
            if (QuestionType == "1") //Text Box
            {
                var Answare = $.trim($(this).children("input[name^='txt_']").val());
                var FileName = $(this).children("input[name='HdnFileName']").val();
                var Result = { ProjectId: $(this).children("input[name='HdnProjectId']").val(), QuestionId: $(this).children("input[name='HdnQuestionId']").val(), Answer: Answare, QuestionTypeId: QuestionType, FileName: FileName }; //We use Question type as BuilderQuaterContractProjectReportId Id to send data to server
                SurveyAdminReport.push(Result);
            }
            else if (QuestionType == "2") {
                var Answare = $.trim($(this).children("select[name='ddlquestion']").find('option:selected').text());
                var FileName = $(this).children("input[name='HdnFileName']").val();
                var Result = { ProjectId: $(this).children("input[name='HdnProjectId']").val(), QuestionId: $(this).children("input[name='HdnQuestionId']").val(), Answer: Answare, QuestionTypeId: QuestionType, FileName: FileName };
                SurveyAdminReport.push(Result);
            }
            else {  //grid type question
                var ProjectId = $(this).children("input[name='HdnProjectId']").val();
                var QuestionId = $(this).children("input[name='HdnQuestionId']").val();
                var Table = $(this).children().find("table[data-table='true']");
                var Tr = $(this).children().find("tr[data-tr='true']");
                var Row = 0;
                var Coloumn = 0;
                //  var QuestionId = $(this).children("input[name='HdnQuestionId']").val();
                $(Tr).each(function () {
                    var Td = $(this).find("td[data-td='true']");
                    Coloumn = 0;
                    $(Td).each(function () {
                        var Control = $(this).children().eq(0);
                        var Answare = ''
                        if ($(Control).is('input')) {
                            Answare = $.trim($(Control).val());
                        }
                        else {
                            Answare = $.trim($(Control).find('option:selected').val());
                        }
                        var Result = { ProjectId: ProjectId, QuestionId: QuestionId, Answer: Answare, RowNumber: Row, ColumnNumber: Coloumn, QuestionTypeId: QuestionType, FileName: FileName };
                        SurveyAdminReport.push(Result);
                        Coloumn = Coloumn + 1;
                    });
                    Row = Row + 1;
                });
            }

            // console.log(SurveyResult);
            // Flag = Flag + 1;
        });

        //   console.log(SurveyAdminReport);

        var PostData = { ContractId: $("#HdnContractId").val(), BuilderId: $("#HdnBuilderId").val(), QuaterId: $("#HdnQuaterId").val(), SubmitReport: SurveyAdminReport };

       //  console.log(PostData);
        // return;

        $.ajax({ //Process the form using $.ajax()
            type: 'POST', //Method type
            url: AjaxCallUrl.SaveBuilderReportUrl,
            data: { ObjVm: PostData }, //Forms name
            dataType: 'json',
            beforeSend: function () {
            },
            success: function (data) {
                //alert(data);
                // console.log('fff1');
                $("#BtnSave").removeAttr('disabled');
                $("#BtnSave").text('SAVE');
                var DataUpdateWindow = $("#DataUpdateMessage").data("kendoWindow");
                if (data.IsSuccess) {
                    kendo.ui.progress($("#BuilderContractQuaterReport"), false);
                    // window.location.href = AjaxCallUrl.ThankYouSurveyUrl.replace("****SurveyId****", $("#HdnSurveyId").val()).replace("****BuilderId****", $("#HdnBuilderId").val()).replace("****IsSurveyCompleted****", IsSurveyComplete)
                    if (DataUpdateWindow) {

                        DataUpdateWindow.title("Update Information");
                        DataUpdateWindow.content("<div class='msgboxSuccess'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i> Data Update Successfully</div>");
                        DataUpdateWindow.open();
                    }

                    setTimeout(function () {

                        if ($("#HdnEditReportBuilder").length)
                        {
                            ///doesn't need to redirect to another page - when admin edit response
                            window.location.href = AjaxCallUrl.AdminNcpSurveyRebateReport.replace("****SurveyId****", $("#HdnSurveyId").val());
                        }
                        else
                        {
                            window.location.href = AjaxCallUrl.RegularReportingUrl;
                        }
                        
                    }, 1000);
                    

                   

                }
                else {
                    kendo.ui.progress($("#BuilderContractQuaterReport"), false);
                    if (DataUpdateWindow) {
                        DataUpdateWindow.title("Error Information");
                        DataUpdateWindow.content("<div class='msgboxError'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i>" + data.ModelError + " </div>");
                        DataUpdateWindow.open();
                    }
                }
            },
            error: function (request, error) {
                //alert(error);
                kendo.ui.progress($("#BuilderContractQuaterReport"), false);
                $("#BtnSave").removeAttr('disabled');
                $("#BtnSave").text('SAVE');
            }
        });

    }
    ,

    DownlaodBuilderDetails: function () {

        var ContractId = $("#HdnContractId").val();
        var src = AjaxCallUrl.DownloadContractMarketDetailsUrl + '?ContractId=' + ContractId;


        var iframe = $("<iframe/>").load(function () {

        }).attr({
            src: src
        }).appendTo($("#DivIframeContainer"));


    }
    ,
    BackBtn_Click: function () {
        window.location.href = AjaxCallUrl.BackUrl.replace("****ContractId****", $("#HdnContractId").val());
    },
    BackBtn_Cancel: function () {
        window.location.href = AjaxCallUrl.CancelUrl;
    },

}

function AddEventForQuestion() {
    $("#TblAdminReport").children().find("input[data-phonenumber='true']").kendoMaskedTextBox({
        mask: "(999) 000-0000"
    });

    //var FileUpload = $("#Div_" + Page).children().find("input[data-role='upload']");
    //console.log(FileUpload.length);
    //if (FileUpload.length) {
    //    $(FileUpload).attr('name', 'files');
    //}

}

function numericFilter(txb) {
    txb.value = txb.value.replace(/[^\0-9]/ig, "");
}


function onImageUpload(e) {
    e.data = { uploadID: 1 };
}


//onImageSuccess = function (result) {
//    // console.log(result);
//    var data = result.response;
//    if (data.IsUpload) {
//        var CurrentDiv = $("div[id^='Div_']:visible");
//        $(CurrentDiv).children().find("input[name='HdnFileName']").val(data.UploadFileName);
//    }
//    else {
//        var CurrentDiv = $("div[id^='Div_']:visible");
//        $(CurrentDiv).children().find("input[name='HdnFileName']").val('');
//    }

//};

function onSelectContractReourceUpload(e) {

    var files = e.files;

    $.each(files, function () {
        //   console.log(this.extension.toLowerCase());
        if (this.extension.toLowerCase() != ".png" && this.extension.toLowerCase() != ".jpg" && this.extension.toLowerCase() != ".jpeg"
            && this.extension.toLowerCase() != ".doc" && this.extension.toLowerCase() != ".docx" && this.extension.toLowerCase() != ".rtf"
            && this.extension.toLowerCase() != ".pdf"
            ) {
            alert("Only .png/.jpg/.jpeg/.doc/.pdf/.docx files can be uploaded!")
            e.preventDefault();
        }
        if (this.size / 1024 / 1024 > 20) {
            alert("Max 5Mb file size is allowed!")
            e.preventDefault();
        }
    });
}


function ViewNcpRebateResultFile(FileName) {

    if (FileName != "") {
        //******** CONDITION ADDED TO SUPPORT MULTIPLE FILE UPLOAD (APALA - 18th Dec 2018) ********
        var arrFileNames = FileName.split(";");

        for (var i = 0; i < arrFileNames.length; i++) {
            var src = AjaxCallUrl.DownLoadFileUrl + '?FileName=' + arrFileNames[i];

            var iframe = $("<iframe/>").load(function () {
                // $.unblockUI();
            }).attr({
                src: src
            }).appendTo($("#DivIframeContainer"));
        }
    }
}

function onImageUpload(e) {

    var Hdn = $("input[name='" + $(this)[0].name + "']").closest('.k-upload');
    var hdnFileName = $(Hdn).siblings("input[name='HdnPrevFileName']").val();

    e.data = { uploadID: 1, PreviousUploadedFiles: hdnFileName };

    //clear out previous file name
    $(Hdn).siblings("input[name='HdnPrevFileName']").val("");
}

onImageSuccess = function (result) {
   // console.log($(this)[0].name);

    var data = result.response;
    if (data.IsUpload) {
        var Hdn = $("input[name='" + $(this)[0].name + "']").closest('.k-upload');
        
        var hdnFileName = $(Hdn).siblings("input[name='HdnFileName']").val();
        var hdnActualFileName = $(Hdn).siblings("input[name='HdnActualFileName']").val();

        if (hdnFileName != "") {
            $(Hdn).siblings("input[name='HdnFileName']").val(hdnFileName + ";" + data.UploadFileName);
            $(Hdn).siblings("input[name='HdnActualFileName']").val(hdnActualFileName + ";" + data.ActualFileNames);
        } else {
            $(Hdn).siblings("input[name='HdnFileName']").val(data.UploadFileName);
            $(Hdn).siblings("input[name='HdnActualFileName']").val(data.ActualFileNames);
        }
    }
    else {
        var Hdn = $("input[name='" + $(this)[0].name + "']").closest('.k-upload');
        $(Hdn).siblings("input[name='HdnFileName']").val(data.UploadFileName);
        $(Hdn).siblings("input[name='HdnActualFileName']").val(data.ActualFileNames);
    }
};

function onSelectContractReourceUpload(e) {

    var Hdn = $("input[name='" + $(this)[0].name + "']").closest('.k-upload');
    var hdnPrevFileName = $(Hdn).siblings("input[name='HdnPrevFileName']").val();

    if (hdnPrevFileName != "") {
        var result = confirm("Uploading new files will replace the previously uploaded ones. Are you sure you want to continue?");

        if (result == false) {
            e.preventDefault();
            return;
        } else {
            //$(Hdn).siblings("input[name='HdnPrevFileName']").val("");       //Clear out previous file names
        }
    }

    var files = e.files;

    $.each(files, function () {
        //   console.log(this.extension.toLowerCase());
        if (this.extension.toLowerCase() != ".png" && this.extension.toLowerCase() != ".jpg" && this.extension.toLowerCase() != ".jpeg"
            && this.extension.toLowerCase() != ".doc" && this.extension.toLowerCase() != ".docx" && this.extension.toLowerCase() != ".rtf"
            && this.extension.toLowerCase() != ".pdf"
            ) {
            alert("Only .png/.jpg/.jpeg/.doc/.pdf/.docx files can be uploaded!")
            e.preventDefault();
        }
        if (this.size / 1024 / 1024 > 20) {
            alert("Max 5Mb file size is allowed!")
            e.preventDefault();
        }
    });
}
