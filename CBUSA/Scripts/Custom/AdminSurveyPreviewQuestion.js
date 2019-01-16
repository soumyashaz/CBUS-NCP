$(document).ready(function () {
    PreviewQuestion.AssignControl();

    $('thead').first().css('display', 'block');
    $('thead').first().css('width', '100%');
    $('tbody').first().css('display', 'block');
    $('tbody').first().css('height', '300px');
    $('tbody').first().css('width', '100%');
});

function SendSurveyIdAsParameter() {
    return { SurveyId: $("#HdnSurveyId").val() };
}

function PreviewQuestionListDataBound() {
    $("input[name^='TxtQuestionValue']").on("focusout", PreviewQuestion.ModifyQuestionText);
    $("a[id^='a_question_up_']").on("click", PreviewQuestion.UpQuestionOrder);
    $("a[id^='a_question_down_']").on("click", PreviewQuestion.DownQuestionOrder);
}

var PreviewQuestion = {
    AssignControl: function () {
        $("#btn_delete").on("click", PreviewQuestion.DeleteQuestion);
        $("#btn_Edit").on("click", PreviewQuestion.EditQuestion);
        $("#btn_Copy").on("click", PreviewQuestion.CopyQuestion);
        $("#btn_preview").on("click", PreviewQuestion.PreviewQuestion);
        // $("#a_ConfigureRemainderEmail").on("click", SurveySettings.ConfigureRemainderEmailPopup);
        //  $("#a_ConfigureSaveContinueEmail").on("click", SurveySettings.ConfigureSaveContinueEmailPopup);
        // $("#BtnSurveyEmailStiing").on("click", SurveySettings.SaveSurveyEmailSettings);

    },
    ModifyQuestionText: function () {

        var InputName = $(this).attr('name');
        var Validator = $("#PreviewQuestionList").kendoValidator({
            messages: {
                ZoneSate: function (input) {
                    return "Atleast one market selection is required";
                },
            },
            rules: {
                reuired: function (input) {
                    return input.val().length > 0;
                }
            }
        }).data("kendoValidator");

        var IsValid = Validator.validateInput($("input[name='" + InputName + "']"));
        if (IsValid) {

            //   console.log($(this).siblings("input[name='HdnQuestionHistory']").val());
           // return;
            if ($(this).val().length > 0) {

                if ($(this).siblings("input[name='HdnQuestionHistory']").val().trim() == $(this).val()) {
                    return;
                }

                kendo.ui.progress($("#PreviewQuestionList"), true);
                var postData = {
                    SurveyId: $("#HdnSurveyId").val(),
                    QuestionId: $(this).parent().siblings().find("input[name='HdnListQuestionId']").val(),
                    Question: $(this).val()
                }; //done

                $.ajax({ //Process the form using $.ajax()
                    type: 'POST', //Method type
                    url: AjaxCallUrl.UpdateQuestionTextUrl,
                    data: postData, //Forms name
                    dataType: 'json',
                    // traditional: true,
                    beforeSend: function () {
                    },
                    success: function (data) {
                        if (data.IsSuccess) {
                            kendo.ui.progress($("#PreviewQuestionList"), false);
                            var DataUpdateWindow = $("#DataUpdateMessage").data("kendoWindow");

                            //  ContractBuilder.LoadContractBuilder();
                            if (DataUpdateWindow) {
                                DataUpdateWindow.title("Update Information");
                                DataUpdateWindow.content("<div class='msgboxSuccess'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i> Data Update Successfully</div>");
                                DataUpdateWindow.open();
                                //redirect to listing page

                                var KendoListView = $("#PreviewQuestionList").data("kendoListView");
                                KendoListView.dataSource.read();
                            }

                        }
                        else {
                            kendo.ui.progress($("#PreviewQuestionList"), false);
                            if (DataUpdateWindow) {
                                DataUpdateWindow.title("Error Information");
                                DataUpdateWindow.content("<div class='msgboxError'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i>" + data.ModelError + " </div>");
                                DataUpdateWindow.open();
                            }
                        }
                    },
                    error: function (request, error) {
                        kendo.ui.progress($("#PreviewQuestionList"), false);
                        if (DataUpdateWindow) {
                            DataUpdateWindow.title("Error Information");
                            DataUpdateWindow.content("<div class='msgboxError'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i>" + data.ModelError + " </div>");
                            DataUpdateWindow.open();
                        }
                    }
                });

            }
        }
    },
    DeleteQuestion: function () {
        // alert("hhe");


        var Confirm = confirm("Are you sure you want to delete the selected question?")
        if (!Confirm) {
            return;
        }

        var RadioButton = $("input[name='RadSurbeyQuestion']:checked");
        //  console.log(RadioButton.length);

        kendo.ui.progress($("#PreviewQuestionList"), true);

        var postData = {
            SurveyId: $("#HdnSurveyId").val(),
            QuestionId: RadioButton.siblings().eq(0).val()
        }; //done



        $.ajax({ //Process the form using $.ajax()
            type: 'POST', //Method type
            url: AjaxCallUrl.DeleteSurveyQuestionUrl,
            data: postData, //Forms name
            dataType: 'json',
            // traditional: true,
            beforeSend: function () {
            },
            success: function (data) {
                if (data.IsSuccess) {
                    kendo.ui.progress($("#PreviewQuestionList"), false);
                    var DataUpdateWindow = $("#DataUpdateMessage").data("kendoWindow");

                    //  ContractBuilder.LoadContractBuilder();
                    if (DataUpdateWindow) {
                        DataUpdateWindow.title("Update Information");
                        DataUpdateWindow.content("<div class='msgboxSuccess'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i> Data Update Successfully</div>");
                        DataUpdateWindow.open();
                        //redirect to listing page

                        var KendoListView = $("#PreviewQuestionList").data("kendoListView");
                        KendoListView.dataSource.read();
                    }

                }
                else {
                    kendo.ui.progress($("#PreviewQuestionList"), false);
                    if (DataUpdateWindow) {
                        DataUpdateWindow.title("Error Information");
                        DataUpdateWindow.content("<div class='msgboxError'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i>" + data.ModelError + " </div>");
                        DataUpdateWindow.open();
                    }
                }
            },
            error: function (request, error) {
                kendo.ui.progress($("#PreviewQuestionList"), false);
                if (DataUpdateWindow) {
                    DataUpdateWindow.title("Error Information");
                    DataUpdateWindow.content("<div class='msgboxError'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i>" + data.ModelError + " </div>");
                    DataUpdateWindow.open();
                }
            }
        });


    },
    PreviewQuestion: function () {

        var url = AjaxCallUrl.PreviewSurveyQuestionUrl.replace("****id****", $("#HdnSurveyId").val());
        // console.log(url);

        var win = window.open(url, '_blank');
        win.focus();
    },
    EditQuestion: function () {


        var RadioButton = $("input[name='RadSurbeyQuestion']:checked");
        var QuestionId = RadioButton.siblings().eq(0).val();
        //console.log(RadioButton.val());
        if (parseInt(QuestionId) > 0) {
            var url = AjaxCallUrl.EditQuestionUrl.replace("****SurveyId****", $("#HdnSurveyId").val()).replace("****QuestionId****", QuestionId);
            window.location.href = url;
        }

    },
    CopyQuestion: function () {

        var RadioButton = $("input[name='RadSurbeyQuestion']:checked");
        var QuestionId = RadioButton.siblings().eq(0).val();

        if (parseInt(RadioButton.siblings().eq(0).val()) > 0) {
            var url = AjaxCallUrl.CopyQuestionUrl.replace("****SurveyId****", $("#HdnSurveyId").val()).replace("****QuestionId****", QuestionId).replace("****IsCopy****", 1);
            window.location.href = url;
        }
    },
    UpQuestionOrder: function () {

        kendo.ui.progress($("#PreviewQuestionList"), true);
        var postData = {
            SurveyId: $("#HdnSurveyId").val(),
            QuestionId: $(this).find("input[type='hidden']").val()

        }; //done


        $.ajax({ //Process the form using $.ajax()
            type: 'POST', //Method type
            url: AjaxCallUrl.OrderingUpQuestionUrl,
            data: postData, //Forms name
            dataType: 'json',
            // traditional: true,
            beforeSend: function () {
            },
            success: function (data) {
                if (data.IsSuccess) {
                    kendo.ui.progress($("#PreviewQuestionList"), false);
                    //  var DataUpdateWindow = $("#DataUpdateMessage").data("kendoWindow");

                    //  ContractBuilder.LoadContractBuilder();
                    //if (DataUpdateWindow) {
                    //    DataUpdateWindow.title("Update Information");
                    //    DataUpdateWindow.content("<div class='msgboxSuccess'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i> Data Update Successfully</div>");
                    //    DataUpdateWindow.open();
                    //    //redirect to listing page


                    //}
                    var KendoListView = $("#PreviewQuestionList").data("kendoListView");
                    KendoListView.dataSource.read();
                }
                else {
                    kendo.ui.progress($("#PreviewQuestionList"), false);
                    //if (DataUpdateWindow) {
                    //    DataUpdateWindow.title("Error Information");
                    //    DataUpdateWindow.content("<div class='msgboxError'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i>" + data.ModelError + " </div>");
                    //    DataUpdateWindow.open();
                    //}
                }
            },
            error: function (request, error) {
                kendo.ui.progress($("#PreviewQuestionList"), false);
                //if (DataUpdateWindow) {
                //    DataUpdateWindow.title("Error Information");
                //    DataUpdateWindow.content("<div class='msgboxError'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i>" + data.ModelError + " </div>");
                //    DataUpdateWindow.open();
                //}
            }
        });

    },
    DownQuestionOrder: function () {


        kendo.ui.progress($("#PreviewQuestionList"), true);
        var postData = {
            SurveyId: $("#HdnSurveyId").val(),
            QuestionId: $(this).find("input[type='hidden']").val(),

        }; //done

        $.ajax({ //Process the form using $.ajax()
            type: 'POST', //Method type
            url: AjaxCallUrl.OrderingDownQuestionUrl,
            data: postData, //Forms name
            dataType: 'json',
            // traditional: true,
            beforeSend: function () {
            },
            success: function (data) {
                if (data.IsSuccess) {
                    kendo.ui.progress($("#PreviewQuestionList"), false);
                    //   var DataUpdateWindow = $("#DataUpdateMessage").data("kendoWindow");

                    //  ContractBuilder.LoadContractBuilder();
                    //if (DataUpdateWindow) {
                    //    DataUpdateWindow.title("Update Information");
                    //    DataUpdateWindow.content("<div class='msgboxSuccess'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i> Data Update Successfully</div>");
                    //    DataUpdateWindow.open();
                    //    //redirect to listing page

                    //    var KendoListView = $("#PreviewQuestionList").data("kendoListView");
                    //    KendoListView.dataSource.read();
                    //}
                    var KendoListView = $("#PreviewQuestionList").data("kendoListView");
                    KendoListView.dataSource.read();
                }
                else {
                    kendo.ui.progress($("#PreviewQuestionList"), false);
                    //if (DataUpdateWindow) {
                    //    DataUpdateWindow.title("Error Information");
                    //    DataUpdateWindow.content("<div class='msgboxError'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i>" + data.ModelError + " </div>");
                    //    DataUpdateWindow.open();
                    //}
                }
            },
            error: function (request, error) {
                kendo.ui.progress($("#PreviewQuestionList"), false);
                //if (DataUpdateWindow) {
                //    DataUpdateWindow.title("Error Information");
                //    DataUpdateWindow.content("<div class='msgboxError'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i>" + data.ModelError + " </div>");
                //    DataUpdateWindow.open();
                //}
            }
        });

    }


}