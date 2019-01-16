$(document).ready(function () {

    $("input[data-phonenumber='true']").kendoMaskedTextBox({
        mask: "(999) 000-0000"
    });

    EditProjectResponse.AssignControl();



});





var EditProjectResponse = {

    AssignControl: function () {

        $("#BtnEditReport").on("click", EditProjectResponse.SaveAdminReport);
        $("#BtnCancelReport").on("click", EditProjectResponse.Cancel);

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
                                        return input.val().length > 0;
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
                                    if (TextBoxType.val() == "2" && input.val().length > 0) {   //for number
                                        var NumberRange = ShownDiv.children("input[name='HdnIsNumberLimit']").val().split(',');
                                        //  console.log('ffff');
                                        MaxMin.Min = NumberRange[0];
                                        MaxMin.Max = parseInt(NumberRange[1]);
                                        return parseInt(input.val()) > parseInt(NumberRange[0]) && parseInt(input.val()) < parseInt(NumberRange[1]);
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

                                    if (TextBoxType.val() == "1" && input.val().length > 0) {   //for Text
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
    Cancel: function () {
        alert('cancel called');
        var url = AjaxCallUrl.RecallSurveyUrl.replace("****SurveyId****", $("#HdnSurveyId").val());
        window.location.href = url;
    },
    SaveAdminReport: function () {

        var IsValidationSuccess = true;

        $("Div[id^='Div_']").each(function () {
            var Isvalid = EditProjectResponse.IsAnswareValidate($(this));
            if (!Isvalid) {
                IsValidationSuccess = false;
            }
        });

        if (IsValidationSuccess) {
            EditProjectResponse.SaveAdminReportResult();
        }


    },



    SaveAdminReportResult: function () {
        kendo.ui.progress($("#DivContainerReport"), true);

        var SurveyAdminReport = [];
        var SurveyAdminReportProject = [];
        $("Div[id^='Div_']").each(function () {
            var QuestionType = $(this).children("input[name='HdnQuestionType']").val();
            if (QuestionType == "1") //Text Box
            {
                var Answare = $(this).children().find("input[name^='txt_']").val();
                var FileName = $(this).children().find("input[name='HdnFileName']").val();
                var Result = { ProjectId: $(this).children("input[name='HdnProjectId']").val(), QuestionId: $(this).children("input[name='HdnQuestionId']").val(), Answer: Answare, QuestionTypeId: QuestionType, FileName: FileName }; //We use Question type as BuilderQuaterContractProjectReportId Id to send data to server
                SurveyAdminReport.push(Result);
            }
            else if (QuestionType == "2") {
                var Answare = $(this).children().find("select[name='ddlquestion'] option:selected").text();
                var FileName = $(this).children().find("input[name='HdnFileName']").val();
                var Result = { ProjectId: $(this).children("input[name='HdnProjectId']").val(), QuestionId: $(this).children("input[name='HdnQuestionId']").val(), Answer: Answare, QuestionTypeId: QuestionType, FileName: FileName };
                SurveyAdminReport.push(Result);
            }
            else {  //grid type question
                var ProjectId = $(this).children().find("input[name='HdnProjectId']").val();
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
                            Answare = $(Control).val();
                        }
                        else {
                            Answare = $(Control).find('option:selected').val();
                        }
                        var Result = { ProjectId: ProjectId, QuestionId: QuestionId, Answer: Answare, RowNumber: Row, ColumnNumber: Coloumn, QuestionTypeId: QuestionType, FileName: FileName };
                        SurveyAdminReport.push(Result);
                        Coloumn = Coloumn + 1;
                    });
                    Row = Row + 1;
                });
            }


        });

        var PostData = { ContractId: $("#HdnContractId").val(), BuilderId: $("#HdnBuilderId").val(), QuaterId: $("#HdnQuaterId").val(), ProjectId: $("#HdnProjectId").val(), ObjSubmitReportResult: SurveyAdminReport };

      //  console.log(PostData);
        // return false;

        $.ajax({ //Process the form using $.ajax()
            type: 'POST', //Method type
            url: AjaxCallUrl.EditBuilderReortUrl,
            data: { ObjVm: PostData }, //Forms name
            dataType: 'json',
            beforeSend: function () {
            },
            success: function (data) {

                var DataUpdateWindow = $("#DataUpdateMessage").data("kendoWindow");
                if (data.IsSuccess) {
                    kendo.ui.progress($("#DivContainerReport"), false);
                    // window.location.href = AjaxCallUrl.ThankYouSurveyUrl.replace("****SurveyId****", $("#HdnSurveyId").val()).replace("****BuilderId****", $("#HdnBuilderId").val()).replace("****IsSurveyCompleted****", IsSurveyComplete)
                    if (DataUpdateWindow) {

                        DataUpdateWindow.title("Update Information");
                        DataUpdateWindow.content("<div class='msgboxSuccess'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i> Data Update Successfully</div>");
                        DataUpdateWindow.open();
                    }

                    setTimeout(function () {
                       // window.location.href = AjaxCallUrl.RegularReportingUrl;
                    }, 1000);

                    var url = AjaxCallUrl.RecallSurveyUrl.replace("****SurveyId****", $("#HdnSurveyId").val());
                    window.location.href = url;

                }
                else {
                    kendo.ui.progress($("#TblAdminReport"), false);
                    if (DataUpdateWindow) {
                        DataUpdateWindow.title("Error Information");
                        DataUpdateWindow.content("<div class='msgboxError'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i>" + data.ModelError + " </div>");
                        DataUpdateWindow.open();
                    }
                }


            },
            error: function (request, error) {

                kendo.ui.progress($("#DivContainerReport"), false);
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
}

function numericFilter(txb) {
    txb.value = txb.value.replace(/[^\0-9]/ig, "");
}


function onImageUpload(e) {
    e.data = { uploadID: 1 };
}


onImageSuccess = function (result) {
    // console.log(result);
    var data = result.response;
    if (data.IsUpload) {
        var Hdn = $("input[name='" + $(this)[0].name + "']").closest('.k-upload');
        $(Hdn).siblings("input[name='HdnFileName']").val(data.UploadFileName);
    }
    else {
        var Hdn = $("input[name='" + $(this)[0].name + "']").closest('.k-upload');
        $(Hdn).siblings("input[name='HdnFileName']").val('');
    }

};

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
        if (this.size / 1024 / 1024 > 5) {
            alert("Max 5Mb file size is allowed!")
            e.preventDefault();
        }
    });
}


function ViewNcpRebateResultFile(FileName) {

    var src = AjaxCallUrl.DownLoadFileUrl + '?FileName=' + FileName;

    var iframe = $("<iframe/>").load(function () {
        // $.unblockUI();
    }).attr({
        src: src
    }).appendTo($("#DivIframeContainer"));

}



function onImageUpload(e) {

    //console.log(e);

    e.data = { uploadID: 1 };
}


