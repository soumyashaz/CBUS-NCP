$(document).ready(function () {

    $("input[data-phonenumber='true']").kendoMaskedTextBox({
        mask: "(999) 000-0000"
    });

    EditSurveyResponse.AssignControl();
    // SurveyAddQuestion.AssignControl();
    //  SurveyInvites.AssignControl();


});





var EditSurveyResponse = {

    AssignControl: function () {
        $("#BtnSaveResponse").on("click", EditSurveyResponse.Save);
        $("#BtnCancelResponse").on("click", EditSurveyResponse.Cancel);
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
                            if (QuestionType == "1") //Text Box type
                            {
                                // var TextBoxType = ShownDiv.children("input[name='HdnTextBoxTypeId']").val();
                                var IsMandatory = ShownDiv.children("input[name='HdnIsMandatory']");
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
                            // console.log('hh3');
                            var QuestionType = ShownDiv.children("input[name='HdnQuestionType']").val();
                            if (QuestionType == "1") //Text Box type
                            {

                                var TextBoxType = ShownDiv.children().find("input[name='HdnTextBoxTypeId']");
                                if (TextBoxType.length) {

                                    if (TextBoxType.val() == "2" && input.val().length > 0) {   //for number
                                        var NumberRange = ShownDiv.children().find("input[name='HdnIsNumberLimit']").val().split(',');
                                        // console.log(NumberRange);
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
                                var TextBoxType = ShownDiv.children().find("input[name='HdnTextBoxTypeId']");

                                if (TextBoxType.length) {

                                    if (TextBoxType.val() == "1" && input.val().length > 0) {   //for Text
                                        var AllowedCharecterList = ShownDiv.children().find("input[name='HdnAllowOnly']").val().split(',');
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
        else {
            var Div = Validator.element[0];
            return { IsValid: false, ParentDiv: $(Div).attr('id') };
        }
    }
    ,
    Cancel: function(){
        //alert('cancel called');
        var url = AjaxCallUrl.RecallSurveyUrl.replace("****SurveyId****", $("#HdnSurveyId").val());
        window.location.href = url;
    },
    Save: function () {
        kendo.ui.progress($("#DivEditBuiderSurveyReponse"), true);
        var IsFromValid = true;
        var DivAll = $("div[id^='Div_']");
        var CurrentDiv = $("div[id^='Div_']:visible");
        var CurrentDivNo = CurrentDiv.children("input[name='HdnCurrentQuestionOrder']").val();
        var IsValid = true;
        var DivNeedtoValidId = '';
        var Flag = 1;
        $(DivAll).each(function () {

            var Valid = EditSurveyResponse.IsAnswareValidate($(this));
            IsValid = Valid.IsValid;
            if (!IsValid) {
                // DivNeedtoValidId = Valid.ParentDiv;
                // return false;
                IsFromValid = false;
                kendo.ui.progress($("#DivEditBuiderSurveyReponse"), false);
            }
        });
        // console.log(IsFromValid);

        if (IsFromValid) {
            //Save data in server
            kendo.ui.progress($("#DivEditBuiderSurveyReponse"), false);
            EditSurveyResponse.SaveSurveyResult(true);
            
        }        
    },

    SaveSurveyResult: function (IsSurveyComplete) {
        kendo.ui.progress($("#DivEditBuiderSurveyReponse"), true);
        var DivAll = $("div[id^='Div_']");
        //    var CurrentDiv = $("div[id^='Div_']:visible");
        //  var CurrentDivNo = CurrentDiv.children("input[name='HdnCurrentQuestionOrder']").val();
        var Flag = 1;
        var SurveyResult = [];
        $(DivAll).each(function () {

            var QuestionType = $(this).children("input[name='HdnQuestionType']").val();
            if (QuestionType == "1") //Text Box
            {
                var Answare = $(this).children().find("input[name^='txt_']").val();
                var FileName = $(this).children().find("input[name='HdnFileName']").val();
                var Result = { QuestionId: $(this).children("input[name='HdnQuestionId']").val(), Answer: Answare, SurveyId: QuestionType,FileName: FileName }; //We use Question type as survey Id to send data to server
                SurveyResult.push(Result);
            }
            else if (QuestionType == "2") {
                var Answare = $(this).children().find("select[name='ddlquestion'] option:selected").text();
                var FileName = $(this).children().find("input[name='HdnFileName']").val();
                var Result = { QuestionId: $(this).children("input[name='HdnQuestionId']").val(), Answer: Answare, SurveyId: QuestionType, FileName: FileName };
                SurveyResult.push(Result);
            }
            else {  //grid type question
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
                        var Result = { QuestionId: QuestionId, Answer: Answare, RowNumber: Row, ColumnNumber: Coloumn, SurveyId: QuestionType };
                        SurveyResult.push(Result);
                        Coloumn = Coloumn + 1;
                    });
                    Row = Row + 1;
                });


            }

            // console.log(SurveyResult);

            Flag = Flag + 1;
        });


        var PostData = { BuilderId: $("#HdnBuilderId").val(), SurveyId: $("#HdnSurveyId").val(), ObjSurveyResult: SurveyResult };

        //    console.log(PostData);
        
        $.ajax({ //Process the form using $.ajax()
            type: 'POST', //Method type
            url: AjaxCallUrl.EditSurveyResponseByAdminUrl,
            data: { ObjVm: PostData }, //Forms name
            dataType: 'json',
            beforeSend: function () {
            },
            success: function (data) {

              //  console.log('fff1');

                var DataUpdateWindow = $("#DataUpdateMessage").data("kendoWindow");
                if (data.IsSuccess) {
                    kendo.ui.progress($("#DivEditBuiderSurveyReponse"), false);
                    if (DataUpdateWindow) {

                        DataUpdateWindow.title("Update Information");
                        DataUpdateWindow.content("<div class='msgboxSuccess'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i> Data Update Successfully</div>");
                        DataUpdateWindow.open();
                    }
                    var url = AjaxCallUrl.RecallSurveyUrl.replace("****SurveyId****", $("#HdnSurveyId").val());
                    window.location.href = url;
                    //location.reload();
                }
                else {
                    kendo.ui.progress($("#DivEditBuiderSurveyReponse"), false);
                    if (DataUpdateWindow) {
                        DataUpdateWindow.title("Error Information");
                        DataUpdateWindow.content("<div class='msgboxError'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i>" + data.ModelError + " </div>");
                        DataUpdateWindow.open();
                    }
                }
            },
            error: function (request, error) {
               // console.log(error);
            //    console.log('ggg');
                kendo.ui.progress($("#DivEditBuiderSurveyReponse"), false);
            }
        });

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
        var CurrentDiv = $("div[id^='Div_']:visible");
        $(CurrentDiv).children().find("input[name='HdnFileName']").val(data.UploadFileName);
    }
    else {
        var CurrentDiv = $("div[id^='Div_']:visible");
        $(CurrentDiv).children().find("input[name='HdnFileName']").val('');
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


function ViewSurveyResultFile(FileName) {

    var src = AjaxCallUrl.DownLoadFileUrl + '?FileName=' + FileName;

    var iframe = $("<iframe/>").load(function () {
        // $.unblockUI();
    }).attr({
        src: src
    }).appendTo($("#DivIframeContainer"));

}