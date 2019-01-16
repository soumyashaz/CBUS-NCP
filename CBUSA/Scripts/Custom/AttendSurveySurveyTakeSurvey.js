var IsPaginationbuttonNextClick = 0;
var IsNextButtonClick = 0;
var LastPageNo = 0;
$(document).ready(function () {

    if ($("#Div_1").length) {
        var TotalPage = parseInt($("#HdnTotalQuestion").val());
        //   console.log(TotalPage);
        if (TotalPage == 1) {
            // console.log('ssss');
            if ($("#Btn_Next").is(":enabled")) {
                $('#Btn_Next').attr("disabled", true);
                // $('#Btn_Save').attr("enabled", true);
            }
            if ($("#Btn_Save").is(":disabled")) {
                $('#Btn_Save').removeAttr("disabled");
            }
        }
        $("#Div_1").show();
        RenderQuestionStatus(1);
    }
    // AddEventForQuestion(1);
    TakeSurvey.AssignControl();
});

var TakeSurvey = {
    Page: { MaxPage: 5 },
    AssignControl: function () {
        $("#Btn_SaveNew").on("click", TakeSurvey.SaveSurvey);
        $("#Btn_Save").on("click", TakeSurvey.Save);
        $("#Btn_SaveContinue").on("click", TakeSurvey.SaveAndContinuelatter);
        $("#Btn_Next").on("click", TakeSurvey.NextQuestion);
        $("#Btn_Prev").on("click", TakeSurvey.PrevQuestion);
        $("#a_next").on("click", TakeSurvey.PaginationNext);
        $("#a_prev").on("click", TakeSurvey.PaginationPrev);
        $("a[name='a_pagination']").on("click", TakeSurvey.ParticularPage);
    },
    PaginationNext: function (Clickflag, ClickFlag) {
        // var IsPaginationbuttonNextClick = 0;
        IsNextButtonClick = 0;
        var PaginationButton = $("a[name='a_pagination']");
        var InitialNo = parseInt($("#HdnPaginationSet").val()) * TakeSurvey.Page.MaxPage;
        var TotalPage = parseInt($("#HdnTotalQuestion").val());
        if ((InitialNo + TakeSurvey.Page.MaxPage) < TotalPage) {
            var Flag = InitialNo + 1;
            $(PaginationButton).each(function () {
                $(this).text(Flag);
                Flag = Flag + 1;
            });
        }
        else {
            var Flag = InitialNo + 1;
            $(PaginationButton).each(function () {

                if (Flag > TotalPage) {
                    $(this).text('');
                    $(this).parent().hide();
                }
                else {
                    $(this).text(Flag);
                    Flag = Flag + 1;
                }
            });
            $(this).hide();
        }

        if ($("#a_prev").is(":visible") == false) {
            $("#a_prev").show();
        }

        // console.log(Clickflag);


        if (ClickFlag != "NotOnButtonClick") {
            // console.log(InitialNo);
            TakeSurvey.ShowPage(InitialNo + 1);
        }

        $("#HdnPaginationSet").val(parseInt($("#HdnPaginationSet").val()) + 1);


    },
    PaginationPrev: function () {
        var PaginationButton = $("a[name='a_pagination']");
        $("#HdnPaginationSet").val(parseInt($("#HdnPaginationSet").val()) - 1);
        var CurrentPageSet = $("#HdnPaginationSet").val();
        var CurrentInitialNo = parseInt($("#HdnPaginationSet").val()) * TakeSurvey.Page.MaxPage;
        InitialNo = CurrentInitialNo - TakeSurvey.Page.MaxPage;
        if (CurrentInitialNo > TakeSurvey.Page.MaxPage) {
            var Flag = InitialNo + 1;
            $(PaginationButton).each(function () {
                $(this).text(Flag);
                Flag = Flag + 1;
            });
        }
        else {
            var Flag = InitialNo + 1;
            $(PaginationButton).each(function () {
                $(this).text(Flag);
                $(this).parent().show();
                Flag = Flag + 1;
            });
            $(this).hide();
        }

        if ($("#a_next").is(":visible") == false) {
            $("#a_next").show();
        }
        //cons
        TakeSurvey.ShowPage(CurrentPageSet * TakeSurvey.Page.MaxPage);
    },
    ParticularPage: function () {


        var CurrentPage = $(this).text();
        //if ($(".page-item").hasClass("active")) {
        //    $(".page-item").removeClass("active");
        //}
        //$(this).parent().addClass("active");
        TakeSurvey.ShowPage(CurrentPage);

    },
    ShowPage: function (PageNo) {

        //   console.log('ddd');
        LastPageNo = PageNo;
        var TotalPage = parseInt($("#HdnTotalQuestion").val());
        var ShownPage = $("div[id^='Div_']:visible");


        var CurrentPage = $("#Div_" + PageNo);




        if (CurrentPage.length) {

            $(ShownPage).hide();
            $(CurrentPage).show();
            RenderQuestionStatus(PageNo);
        }
        else {
            //  console.log(PageNo);
            for (var i = 2; i <= PageNo; i++) {

                var DivTorender = $("#Div_" + i);
                if (!DivTorender.length) {
                    TakeSurvey.RenderNewQuestion(i);
                }
            }

        }

        if (PageNo == 1) {
            $('#Btn_Prev').attr("disabled", true);

            if ($("#Btn_Next").is(":disabled")) {
                $('#Btn_Next').removeAttr("disabled");
            }
            if (PageNo == TotalPage) {
                if ($("#Btn_Next").is(":enabled")) {
                    $('#Btn_Next').attr("disabled", true);
                }
            }
        }
        else if (PageNo == TotalPage) {
            $('#Btn_Next').attr("disabled", true);
            $('#Btn_Save').removeAttr("disabled");
            //  $('#Btn_SaveContinue').attr("disabled", true);
            if ($("#Btn_Prev").is(":disabled")) {
                $('#Btn_Prev').removeAttr("disabled");
            }

        }
        else {
            if ($("#Btn_Next").is(":disabled")) {
                $('#Btn_Next').removeAttr("disabled");
            }
            if ($("#Btn_Prev").is(":disabled")) {
                $('#Btn_Prev').removeAttr("disabled");
            }
            if ($("#Btn_Save").is(":enabled")) {
                $('#Btn_Save').attr("disabled", true);

            }

        }





        if ($(".page-item").hasClass("active")) {
            $(".page-item").removeClass("active");
        }



        var search = $(".page-link").filter(function () {
            return $(this).text() == PageNo;
        }).first().parent().addClass("active");

    },
    NextQuestion: function () {
        IsNextButtonClick = 1;
        var ShownDiv = $("div[id^='Div_']:visible");
        // var Valid = TakeSurvey.IsAnswareValidate(ShownDiv);
        //  if (Valid.IsValid) {
        var TotalPage = parseInt($("#HdnTotalQuestion").val());
        var ShownPage = $("div[id^='Div_']:visible");
        var CurrentPage = ShownPage.children("input[name='HdnCurrentQuestionOrder']").val();
        var NextPage = parseInt(CurrentPage) + 1;
        TakeSurvey.ShowPage(NextPage);
        //}
    },
    PrevQuestion: function () {
        var ShownPage = $("div[id^='Div_']:visible");
        var CurrentPage = ShownPage.children("input[name='HdnCurrentQuestionOrder']").val();
        var PrevPage = parseInt(CurrentPage) - 1;
        var IsLinkButtonAvailable = $(".page-link").filter(function () {
            // return $(this).text().toLowerCase().indexOf(PageNo.toLowerCase()) >= 0;
            return $(this).text() == PrevPage;
        });

        if (IsLinkButtonAvailable.length == 0) {

            $("#a_prev").trigger("click");
        }
        else {
            TakeSurvey.ShowPage(PrevPage);
        }

    },
    SaveAndContinuelatter: function () {
        //  console.log(Valid);
        var DivAll = $("div[id^='Div_']");
        var CurrentDiv = $("div[id^='Div_']:visible");
        var CurrentDivNo = CurrentDiv.children("input[name='HdnCurrentQuestionOrder']").val();
        var IsValid = true;
        var DivNeedtoValidId = '';
        var Flag = 1



        for (var i = 1; i <= parseInt(CurrentDivNo) ; i++) {

            var Valid = TakeSurvey.IsAnswareValidate($("#Div_" + i));
            IsValid = Valid.IsValid;

            if (!IsValid) {
                DivNeedtoValidId = Valid.ParentDiv;
                break;
            }

        }






        if (IsValid) {
            //Save data in server
            TakeSurvey.SaveSurveyResult(false, 1);
        }
        else {

            //back to question
            var QuestionOrder = $("#" + DivNeedtoValidId).children("input[name='HdnCurrentQuestionOrder']").val();
            TakeSurvey.ShowPage(QuestionOrder);
        }



        //$(DivAll).each(function () {
        //    if (Flag > parseInt(CurrentDivNo)) {
        //        return false;
        //    }
        //    var Valid = TakeSurvey.IsAnswareValidate($(this));
        //    IsValid = Valid.IsValid;
        //    if (!IsValid) {
        //        DivNeedtoValidId = Valid.ParentDiv;
        //        return false;
        //    }
        //    Flag = Flag + 1;
        //});

        //if (IsValid) {
        //    //Save data in server
        //    TakeSurvey.SaveSurveyResult(false);

        //}
        //else {
        //    //back to question
        //    var QuestionOrder = $("#" + DivNeedtoValidId).children("input[name='HdnCurrentQuestionOrder']").val();
        //    TakeSurvey.ShowPage(QuestionOrder);
        //}
    },
    IsAnswareValidate: function (ShownDiv) {

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
                    // console.log('ssss');
                    // if (!input.is(":hidden")) {
                    var validate = input.data('check');
                    //console.log(validate);
                    //  console.log('ee');
                    if (typeof validate !== 'undefined' && validate !== false) {
                        var QuestionType = ShownDiv.children("input[name='HdnQuestionType']").val();
                        // console.log(QuestionType);
                        if (QuestionType == "1") //Text Box type
                        {
                            // console.log('dddddww');
                            // var TextBoxType = ShownDiv.children("input[name='HdnTextBoxTypeId']").val();
                            var IsMandatory = ShownDiv.children("input[name='HdnIsMandatory']");
                            if (IsMandatory.length) {
                                // console.log(input.val());
                                if (IsMandatory.val().toLowerCase() == "true") {

                                    return input.val().length > 0;
                                }
                            }
                        }
                    }
                    return true;
                    // }
                    //  return true;
                },
                numberrange: function (input) {
                    //  if (!input.is(":hidden")) {
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
                                    if (parseInt(NumberRange[0]) == 0 && parseInt(NumberRange[1]) == 0) {
                                        return true;
                                    }
                                    else {
                                        return parseInt(input.val()) > parseInt(NumberRange[0]) && parseInt(input.val()) < parseInt(NumberRange[1]);
                                    }

                                }
                                return true;
                            }
                        }
                        return true;
                    }
                    return true;
                    // }
                    // return true;
                },
                allowedcharecter: function (input) {
                    //  if (!input.is(":hidden")) {
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
                    //  }
                    // return true;

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
    Save: function () {

        var DivAll = $("div[id^='Div_']");
        var CurrentDiv = $("div[id^='Div_']:visible");
        var CurrentDivNo = CurrentDiv.children("input[name='HdnCurrentQuestionOrder']").val();
        var IsValid = true;
        var DivNeedtoValidId = '';
        var Flag = 1;
        $(DivAll).each(function () {
            if (Flag > parseInt(CurrentDivNo)) {
                return false;
            }
            var Valid = TakeSurvey.IsAnswareValidate($(this));
            IsValid = Valid.IsValid;
            if (!IsValid) {
                DivNeedtoValidId = Valid.ParentDiv;
                return false;
            }
            Flag = Flag + 1;
        });
        if (IsValid) {
            //Save data in server
            TakeSurvey.SaveSurveyResult(true, 1);

        }
        else {
            //back to question
            var QuestionOrder = $("#" + DivNeedtoValidId).children("input[name='HdnCurrentQuestionOrder']").val();
            TakeSurvey.ShowPage(QuestionOrder);
        }

    },

    SaveSurveyResult: function (IsSurveyComplete, RedirectFlag) {
        var DivAll = $("div[id^='Div_']");
        var CurrentDiv = $("div[id^='Div_']:visible");
        var CurrentDivNo = CurrentDiv.children("input[name='HdnCurrentQuestionOrder']").val();
        var Flag = 1;
        var SurveyResult = [];
        $(DivAll).each(function () {



            if (Flag > CurrentDivNo) {
                return false;
            }
            console.log($(this).attr('id'));

            var QuestionType = $(this).children("input[name='HdnQuestionType']").val();
            if (QuestionType == "1") //Text Box
            {
                var Answare = $(this).children().find("input[name^='txt_']").val();
                var FileName = $(this).children().find("input[name='HdnFileName']").val();
                var Result = { QuestionId: $(this).children("input[name='HdnQuestionId']").val(), Answer: Answare, SurveyId: QuestionType, FileName: FileName }; //We use Question type as survey Id to send data to server
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


        var PostData = { BuilderId: $("#HdnBuilderId").val(), SurveyId: $("#HdnSurveyId").val(), ObjTakeSurveyQuestion: { ObjSurveyResult: SurveyResult }, IsSurveyComplete: IsSurveyComplete };

        //  console.log(PostData);
        //  return false;


        kendo.ui.progress($(".container"), true);
        $.ajax({ //Process the form using $.ajax()
            type: 'POST', //Method type
            url: AjaxCallUrl.SaveSurveyResult,
            data: { ObjVm: PostData }, //Forms name
            dataType: 'json',
            beforeSend: function () {
            },
            success: function (data) {

                // console.log('fff1');

                // var DataUpdateWindow = $("#DataUpdateMessage").data("kendoWindow");
                if (data.IsSuccess) {
                    kendo.ui.progress($(".container"), false);

                    if (RedirectFlag == 1) {
                        window.location.href = AjaxCallUrl.ThankYouSurveyUrl.replace("****SurveyId****", $("#HdnSurveyId").val()).replace("****BuilderId****", $("#HdnBuilderId").val()).replace("****IsSurveyCompleted****", IsSurveyComplete)
                    }





                }
                else {
                    kendo.ui.progress($(".container"), false);
                    if (DataUpdateWindow) {
                        DataUpdateWindow.title("Error Information");
                        DataUpdateWindow.content("<div class='msgboxError'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i>" + data.ModelError + " </div>");
                        DataUpdateWindow.open();
                    }
                }
            },
            error: function (request, error) {
                //  console.log(error);
                // console.log('ggg');
                kendo.ui.progress($(".container"), false);
            }
        });

    }
    ,
    RenderNewQuestion: function (PageNo) {
        var PostData = { SurveyId: $("#HdnSurveyId").val(), BuilderId: $("#HdnBuilderId").val(), QuestionIndex: PageNo, TotalQuestion: $("#HdnTotalQuestion").val() };

        kendo.ui.progress($(".container"), true);
        $.ajax({ //Process the form using $.ajax()
            type: 'POST', //Method type
            url: AjaxCallUrl.RenderQuestion,
            data: PostData, //Forms name
            dataType: 'json',
            beforeSend: function () {
            },
            success: function (data) {



                var DataUpdateWindow = $("#DataUpdateMessage").data("kendoWindow");
                if (data.IsSuccess) {

                    var ShownPage = $("div[id^='Div_']:visible");
                    $('#DivMainQuestionContainer').append(data.PartialView);

                    var CurrentPage = $("#Div_" + PageNo);

                    /*Automatically change the pagination set*/
                    var IsLinkButtonAvailable = $(".page-link").filter(function () {
                        // return $(this).text().toLowerCase().indexOf(PageNo.toLowerCase()) >= 0;
                        return $(this).text() == PageNo;
                    });


                    if (IsNextButtonClick == 1) {
                        if (IsLinkButtonAvailable.length == 0) {
                            var CurrentPageOrder = ShownPage.children("input[name='HdnCurrentQuestionOrder']").val();
                            var ShowPageOrder = CurrentPage.children("input[name='HdnCurrentQuestionOrder']");

                            if (!ShowPageOrder.length)  //Should Move forward
                            {
                                $("#a_next").trigger("click", ["NotOnButtonClick"]);
                                // PaginationNextPreview();
                            }
                            else {

                                if (parseInt($(ShowPageOrder).val()) > parseInt(CurrentPageOrder))  //move forward
                                {
                                    $("#a_next").trigger("click", ["NotOnButtonClick"]);
                                    //  PaginationNextPreview();
                                }
                                //else //move back
                                //{
                                //    $("#a_prev").trigger("click");
                                //}
                            }
                            var search = $(".page-link").filter(function () {
                                return $(this).text() == PageNo;
                            }).first().parent().addClass("active");
                        }
                    }

                    /*end*/




                    $(ShownPage).hide();
                    //  $("#Div_" + PageNo).show();
                    $("#Div_" + LastPageNo).show();

                    kendo.ui.progress($(".container"), false);
                    AddEventForQuestion(PageNo);
                    RenderQuestionStatus(PageNo);
                    //setTimeout(function () {

                    //    $(ShownPage).hide();
                    //    $("#Div_" + PageNo).show();
                    //    kendo.ui.progress($(".container"), false);
                    //}, 500);


                }
                else {
                    kendo.ui.progress($(".container"), false);
                }
            },
            error: function (request, error) {

                kendo.ui.progress($(".container"), false);
            }
        });

    }
    ,
    IsAnswareValidateForAll: function () {
        // console.log('dsfs');
        // var ShownDiv = $("div[id^='Div_']");

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
                        console.log('ssss');
                        var validate = input.data('check');
                        //console.log(validate);
                        if (typeof validate !== 'undefined' && validate !== false) {
                            //   console.log('ssss');
                            var QuestionType = input.closest("div[id^='Div_").children("input[name='HdnQuestionType']").val();
                            //  console.log(QuestionType);
                            if (QuestionType == "1") //Text Box type
                            {
                                //  console.log('dddddww');
                                // var TextBoxType = ShownDiv.children("input[name='HdnTextBoxTypeId']").val();
                                var IsMandatory = input.closest("div[id^='Div_").children("input[name='HdnIsMandatory']");
                                if (IsMandatory.length) {
                                    // console.log(input.val());
                                    if (IsMandatory.val().toLowerCase() == "true") {
                                        console.log('asdas');
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
                            var QuestionType = input.closest("div[id^='Div_").children("input[name='HdnQuestionType']").val();
                            if (QuestionType == "1") //Text Box type
                            {

                                var TextBoxType = input.siblings("input[name='HdnTextBoxTypeId']");
                                if (TextBoxType.length) {

                                    if (TextBoxType.val() == "2" && input.val().length > 0) {   //for number
                                        var NumberRange = input.siblings("input[name='HdnIsNumberLimit']").val().split(',');
                                        // console.log(NumberRange);
                                        MaxMin.Min = NumberRange[0];
                                        MaxMin.Max = parseInt(NumberRange[1]);
                                        if (parseInt(NumberRange[0]) == 0 && parseInt(NumberRange[1]) == 0) {
                                            return true;
                                        }
                                        else {
                                            return parseInt(input.val()) > parseInt(NumberRange[0]) && parseInt(input.val()) < parseInt(NumberRange[1]);
                                        }

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
                            var QuestionType = input.closest("div[id^='Div_").children("input[name='HdnQuestionType']").val();
                            if (QuestionType == "1") //Text Box type
                            {
                                var TextBoxType = input.siblings("input[name='HdnTextBoxTypeId']");

                                if (TextBoxType.length) {

                                    if (TextBoxType.val() == "1" && input.val().length > 0) {   //for Text
                                        var AllowedCharecterList = input.siblings("input[name='HdnAllowOnly']").val().split(',');
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

        //if (IsValid) {

        //    return { IsValid: true };
        //}
        //else {
        //    var Div = Validator.element[0];
        //    return { IsValid: false, ParentDiv: $(Div).attr('id') };
        //}
        return false;

    }
    ,
    SaveSurvey: function () {

        var DivAll = $("div[id^='Div_']");
        var CurrentDiv = $("div[id^='Div_']:visible");
        var CurrentDivNo = CurrentDiv.children("input[name='HdnCurrentQuestionOrder']").val();
        var IsValid = true;
        var DivNeedtoValidId = '';
        var Flag = 1

        for (var i = 1; i <= parseInt(CurrentDivNo) ; i++) {

            var Valid = TakeSurvey.IsAnswareValidate($("#Div_" + i));
            IsValid = Valid.IsValid;

            if (!IsValid) {
                DivNeedtoValidId = Valid.ParentDiv;
                break;
            }

        }

        if (IsValid) {
            //Save data in server
            TakeSurvey.SaveSurveyResult(false, 0);
        }
        else {

            //back to question
            var QuestionOrder = $("#" + DivNeedtoValidId).children("input[name='HdnCurrentQuestionOrder']").val();
            TakeSurvey.ShowPage(QuestionOrder);
        }
    }

}


function AddEventForQuestion(Page) {
    $("#Div_" + Page).children().find("input[data-phonenumber='true']").kendoMaskedTextBox({
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
        if (this.size / 1024 / 1024 > 20) {
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


function RenderQuestionStatus(PageNo) {
    var TotalQuestion = $("#HdnTotalQuestion").val();
    $("#LblQuestionStatus").text(PageNo + " / " + TotalQuestion);
    var Percentage = Math.floor((PageNo / TotalQuestion) * 100);
    $("#DivQuestionStatus").css("width", Percentage + '%');


    if ($("#Div_" + PageNo).find(':text,:hidden,textarea').length > 0)
        $("#Div_" + PageNo).find('input,select,textarea,radio').focus();



}

function PaginationNextPreview() {
    console.log('1111')
    var PaginationButton = $("a[name='a_pagination']");
    var InitialNo = parseInt($("#HdnPaginationSet").val()) * TakeSurvey.Page.MaxPage;
    var TotalPage = parseInt($("#HdnTotalQuestion").val());
    if ((InitialNo + TakeSurvey.Page.MaxPage) < TotalPage) {
        var Flag = InitialNo + 1;
        $(PaginationButton).each(function () {
            $(this).text(Flag);
            Flag = Flag + 1;
        });
    }
    else {
        //console.log('dddwww')
        var Flag = InitialNo + 1;

        $(PaginationButton).each(function () {
            console.log(Flag);

            if (Flag > TotalPage) {
                $(this).text('');
                $(this).parent().hide();
            }
            else {
                $(this).text(Flag);
                Flag = Flag + 1;

                // console.log($(this).attr('name'));
            }
            //  $(this).html('');
            console.log($(this).text());

        });
        $("#a_next").hide();
    }

    if ($("#a_prev").is(":visible") == false) {
        $("#a_prev").show();
    }
    $("#HdnPaginationSet").val(parseInt($("#HdnPaginationSet").val()) + 1);
}