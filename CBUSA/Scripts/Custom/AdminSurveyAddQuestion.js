$(document).ready(function () {


    SurveyAddQuestion.AssignControl();



});


var SurveyAddQuestion = {
    AssignControl: function () {
        $("#BtnGenarateGrid").on("click", SurveyAddQuestion.ShowGridHeadrerView);
        $("#Btn_AddQuestion").on("click", SurveyAddQuestion.SaveNewQuestion);
        $("#Btn_save_addnew_question").on("click", SurveyAddQuestion.SaveNewQuestion);
    },
    ClearQuestion: function () {




    },
    SaveNewQuestion: function () {

        //console.log($(this).attr('id'));
        var Buttonid = $(this).attr('id');
        //console.log(Buttonid);
        // return;

        $("#HdnButtonId").val(Buttonid);

        var IsValid = SurveyAddQuestion.ValidationAddQuestion();
        if (IsValid) {
            kendo.ui.progress($("#SectionAddQuestion"), true);
            var IsMandatory;
            var RadioMandatory = $("input[name='ObjQuestion.IsMandatory']:checked");
            if ($(RadioMandatory).val() == "Yes") {
                IsMandatory = true;
            }
            else {
                IsMandatory = false;
            }

            var UploadFileAvail = $("input[name='ObjQuestion.IsFileNeedtoUpload']");
            var IsUploadFile;
            if ($(UploadFileAvail).is(':checked')) {
                IsUploadFile = true;
            }
            else {
                IsUploadFile = false;
            }


            var AllowAlphabets = $("input[name='ObjTextBoxSetting.IsAlphabets']:checked").length > 0;
            var AllowNumber = $("input[name='ObjTextBoxSetting.IsNumber']:checked").length > 0;
            var AllowSpecialCharecter = $("input[name='ObjTextBoxSetting.IsSpecialCharecter']:checked").length > 0;





            var Question = {
                QuestionId: 0, QuestionValue: $("#ObjQuestion_QuestionValue").val(), QuestionTypeId: $("#ObjQuestion_QuestionTypeId").data("kendoDropDownList").value(),
                IsMandatory: IsMandatory, IsFileNeedtoUpload: IsUploadFile, SurveyId: $("#HdnSurveyId").val(),
                QuestionId: $("#HdnQuestionId").val()
            };

            var QuestionTextBox;
            var QuestionDropDown;
            var QuestionGrid;
            var QuestionGridSettings;
            var QuestionTypeId = $("#ObjQuestion_QuestionTypeId").data("kendoDropDownList").value();

            if (QuestionTypeId == "1") {

                QuestionTextBox = {
                    QuestionTextBoxSettingId: 0, IsAlphabets: AllowAlphabets, IsNumber: AllowNumber,
                    IsSpecialCharecter: AllowSpecialCharecter, LowerLimit: $("#ObjTextBoxSetting_LowerLimit").val(),
                    UpperLimit: $("#ObjTextBoxSetting_UpperLimit").val(), TextBoxTypeId: $("#ObjTextBoxType_TextBoxTypeId").data("kendoDropDownList").value()
                };
            }
            else if (QuestionTypeId == "2") {
                QuestionDropDown = {
                    QuestionDropdownSettingId: 0, Value: $("#ObjDropDownSetting_Value").val()
                };
            }
            else if (QuestionTypeId == "3") {

                //  if()

                QuestionGrid = {
                    QuestionGridSettingId: 0, Row: parseInt($("#HdnGridMaxHeaderRowHistory").val()) - 1, Column: parseInt($("#HdnGridMaxHeaderColHistory").val()) - 1
                };


                var HeaderRowValueArray = [];
                var HeaderRowValue = '';
                var TextBoxRow = $("input[name='TxtGridRowHeader']");
                $(TextBoxRow).each(function () {

                    if ($(this).val().length > 0) {
                        HeaderRowValueArray.push($(this).val());
                    }
                });

                HeaderRowValue = HeaderRowValueArray.join("####RowValue####");

                var HeaderColoumnValueArray = [];
                var HeaderColoumnValue = '';
                var TextBoxColoumn = $("input[name='TxtGridColumnHeader']");
                $(TextBoxColoumn).each(function () {
                    if ($(this).val().length > 0) {
                        HeaderColoumnValueArray.push($(this).val());
                    }
                });

                HeaderColoumnValue = HeaderColoumnValueArray.join("####ColValue####");


                var ColoumnDropDownArray = [];
                var ColoumnDropDownValue = '';
                var ColoumnDropDown = $("select[name='DdlGridColumnControlType']");
                var Flag = 1;
                var ControlType = 1;
                var ControlValue = '';
                $(ColoumnDropDown).each(function () {
                    var ParentColoumnheadingTextBox = $(this).parent().parent().children().find("input[name='TxtGridColumnHeader']");
                    //  console.log(ColoumnDropDownArray);

                    if (ParentColoumnheadingTextBox.val().length > 0) {



                        ControlType = 1;
                        ControlValue = '';

                        if ($(this).val() == "2") {
                            var DropDownOptionCltr = $(this).parent().parent().children().find("input[id^='HdnControl']");
                            //  //ColoumnDropDownArray.push($(DropDownOptionCltr).val());
                            //if (ColoumnDropDownValue != '') {
                            //    ColoumnDropDownValue = ColoumnDropDownValue + DropDownOptionCltr.val();
                            //}
                            //else if (Flag == ColoumnDropDown.length) {
                            //    ColoumnDropDownValue = ColoumnDropDownValue + DropDownOptionCltr.val() + "$#$"
                            //}
                            //else {
                            //    ColoumnDropDownValue = ColoumnDropDownValue + DropDownOptionCltr.val() + "$#$"
                            //}

                            ControlType = 2;
                            ControlValue = DropDownOptionCltr.val();
                        }

                        ColoumnDropDownArray.push({ ControIndex: Flag, ControlType: ControlType, ControlValue: ControlValue })

                        Flag = Flag + 1;
                    }
                });


                //   console.log(ColoumnDropDownArray);
                //   return false;

                //     ColoumnDropDownValue = ColoumnDropDownArray.join();

                QuestionGridSettings = {
                    QuestionGridSettingHeaderId: 0, RowHeaderValue: HeaderRowValue,
                    ColoumnHeaderValue: HeaderColoumnValue,
                    ColoumnControlValue: JSON.stringify(ColoumnDropDownArray)
                    // DropdownTypeOptionValue: ColoumnDropDownValue
                };
            }

            var postData = {
                ObjQuestion: Question, ObjTextBoxSetting: QuestionTextBox, ObjDropDownSetting: QuestionDropDown, ObjGridSetting: QuestionGrid,
                ObjQuestionGridSettingHeader: QuestionGridSettings
            }; //done
            $.ajax({ //Process the form using $.ajax()
                type: 'POST', //Method type
                url: AjaxCallUrl.SaveQuestionUrl,
                data: postData, //Forms name
                dataType: 'json',
                // traditional: true,
                beforeSend: function () {
                },
                success: function (data) {
                    var DataUpdateWindow = $("#DataUpdateMessage").data("kendoWindow");
                    if (data.IsSuccess) {
                        kendo.ui.progress($("#SectionAddQuestion"), false);
                        //  ContractBuilder.LoadContractBuilder();
                        if (DataUpdateWindow) {
                            DataUpdateWindow.title("Update Information");
                            DataUpdateWindow.content("<div class='msgboxSuccess'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i> Data Update Successfully</div>");
                            DataUpdateWindow.open();
                            //redirect to listing page
                            //  $(this).attr('id')
                            var Buttonid = $(this).attr('id');
                            var IsInEditQuestion = $("#HdnQuestionId").val();

                            //  console.log(Buttonid);
                            // console.log(IsInEditQuestion);
                            //  return;

                            //&& parseInt(IsInEdit) > 0
                            setTimeout(function () {

                                if ($("#HdnButtonId").val() == "Btn_AddQuestion" && parseInt(IsInEditQuestion) == 0) {
                                    if ($("#HdnIsCopy").val() == "1") {

                                        // console.log('dddd');


                                        window.location.href = AjaxCallUrl.PreviewQustionUrl.replace("****SurveyId****", $("#HdnSurveyId").val());


                                    }
                                    else {

                                        if ($("#HdnIsNcpSurvey").val().toLowerCase() === "true") {
                                            window.location.href = AjaxCallUrl.SurvaySettingUrl.replace("****SurveyId****", $("#HdnSurveyId").val());
                                        }
                                        else {
                                            window.location.href = AjaxCallUrl.ConfigureInviteeUrl.replace("****SurveyId****", $("#HdnSurveyId").val());
                                        }
                                    }
                                    //if ($("#HdnIsNcpSurvey").val().toLowerCase() === "true") {
                                    //    window.location.href = AjaxCallUrl.SurvaySettingUrl.replace("****SurveyId****", $("#HdnSurveyId").val());
                                    //}
                                    //else {
                                    //   // window.location.href = AjaxCallUrl.ConfigureInviteeUrl.replace("****SurveyId****", $("#HdnSurveyId").val());
                                    //}

                                    //window.location.href = AjaxCallUrl.ConfigureInviteeUrl.replace("****SurveyId****", $("#HdnSurveyId").val());
                                }
                                else if ($("#HdnButtonId").val() == "Btn_AddQuestion" && parseInt(IsInEditQuestion) > 0) {
                                    window.location.href = AjaxCallUrl.PreviewQustionUrl.replace("****SurveyId****", $("#HdnSurveyId").val());
                                }
                                else {
                                    window.location.href = AjaxCallUrl.OwnPageUrl.replace("****SurveyId****", $("#HdnSurveyId").val());
                                }
                            }, 800);

                        }

                    }
                    else {
                        kendo.ui.progress($("#SectionAddQuestion"), false);
                        if (DataUpdateWindow) {
                            DataUpdateWindow.title("Error Information");
                            DataUpdateWindow.content("<div class='msgboxError'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i>" + data.ModelError + " </div>");
                            DataUpdateWindow.open();
                        }
                    }
                },
                error: function (request, error) {
                    kendo.ui.progress($("#SectionAddQuestion"), false);
                    if (DataUpdateWindow) {
                        DataUpdateWindow.title("Error Information");
                        DataUpdateWindow.content("<div class='msgboxError'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i>" + data.ModelError + " </div>");
                        DataUpdateWindow.open();
                    }
                }
            });

        }

    },
    ValidationAddQuestion: function () {

        var Flag = 0;
        var FlagGridMessage = 0;

        var Validator = $("#SectionAddQuestion").kendoValidator({
            messages: {
                required: "*",
                requiredCheckBox: "Select the allowed character set",
                greaternumerictextvalue: "Invalid limit value",
                greaterthanzero: "Must be greater than zero",
                gridrowcoloumnvalidation: function () {
                    //console.log(FlagGridMessage);
                    if (FlagGridMessage == 1) {
                        return "Row value can't be empty";
                    }
                    else if (FlagGridMessage == 2) {
                        return "Coloumn value can't be empty";
                    }
                    else if (FlagGridMessage == 3) {
                        return "Please add some option value";
                    }
                }
            },
            rules: {
                required: function (input) {

                    if (input.is("[required=required]")) {
                        var dropdownlist = $("#ObjQuestion_QuestionTypeId").data("kendoDropDownList");



                        var id = input.attr('id');
                        if (dropdownlist.value() == "1") {

                            if (id == "ObjQuestion_QuestionValue") {
                                return input.val().length > 0;
                            }
                        }
                        else if (dropdownlist.value() == "2") {
                            if (id == "ObjDropDownSetting_Value" || id == "ObjQuestion_QuestionValue") {
                                return input.val().length > 0;
                            }

                        }
                        else if (dropdownlist.value() == "3") {
                            //  console.log(dropdownlist.value())
                            //   console.log(id)
                            if (id == "ObjQuestion_QuestionValue") {
                                //  console.log('dfsd');
                                return input.val().length > 0; // 
                                // return true;
                            }


                        }

                        return true;
                    }
                    return true;
                },
                requiredCheckBox: function (item) {
                    var id = item.attr('id');
                    //  console.log(id);
                    if (item.is('[data-checkrequired=required]')) {

                        var dropdownlist = $("#ObjQuestion_QuestionTypeId").data("kendoDropDownList");

                        if (dropdownlist.value() == "1") {

                            var dropdownlistSub = $("#ObjTextBoxType_TextBoxTypeId").data("kendoDropDownList");
                            if (dropdownlistSub.value() == "1") {
                                return item.children(':checked').length > 0
                            }
                        }
                    }
                    return true;
                },
                greaternumerictextvalue: function (input) {
                    var validate = input.data('greaternumerictextbox');
                    // if the input has a `data-available` attribute...
                    if (typeof validate !== 'undefined' && validate !== false && input.val() != "") {
                        var dropdownlist = $("#ObjTextBoxType_TextBoxTypeId").data("kendoDropDownList");
                        if (dropdownlist.value() == "2") {
                            var RangeTo = parseInt(input.val());
                            var RtangeFrom = parseInt($("[name='" + input.data("lesserrnumericfield") + "']").val());

                            //  console.log(RangeTo)
                            // console.log(RtangeFrom)
                            return RangeTo >= RtangeFrom;
                        }

                        return true;
                    }
                    return true;
                },
                greaterthanzero: function (input) {
                    var validate = input.data('greaterthanzero');
                    if (typeof validate !== 'undefined' && validate !== false && input.val() != "") {
                        var dropdownlist = $("#ObjQuestion_QuestionTypeId").data("kendoDropDownList");
                        if (dropdownlist.value() == "3") {
                            return parseInt(input.val()) > 0;
                        }
                    }
                    return true;
                },
                gridrowcoloumnvalidation: function (item) {
                    var IsValidateRow = true;
                    var IsValidateColoumn = true;
                    var IsValidateDropValue = true;
                    var IsValidate = true;
                    if (item.is('[data-gridrowcoloumnvalidation=required]')) {

                        var TextBoxRow = $("input[name='TxtGridRowHeader']");
                        $(TextBoxRow).each(function () {

                            if ($(this).val().length == 0) {
                                FlagGridMessage = 1;
                                // console.log('11');
                                //   IsValidateRow = false;
                                IsValidateRow = true;


                            }

                        });

                        var TextBoxColoumn = $("input[name='TxtGridColumnHeader']");
                        $(TextBoxColoumn).each(function () {

                            if ($(this).val().length == 0) {
                                FlagGridMessage = 2;
                                // IsValidateColoumn = false;
                                IsValidateColoumn = true;
                            }

                        });

                        var ColoumnDropDown = $("select[name='DdlGridColumnControlType']");
                        $(ColoumnDropDown).each(function () {

                            if ($(this).val() == "2") {

                                var ParentheadingTextBox = $(this).parent().parent().children().find("input[name='TxtGridColumnHeader']");

                                if ($(ParentheadingTextBox).val().length > 0) {

                                    var DropDownOptionValue = $(this).parent().parent().children().find("input[id^='HdnControl']");
                                    if (DropDownOptionValue.val() == "0") {
                                        FlagGridMessage = 3;
                                        //console.log('33');
                                        IsValidateDropValue = false;
                                    }

                                }

                            }
                            //if ($(this).val().length == 0) {
                            //    FlagGridMessage = 2;
                            //    return false;

                            //}

                        });

                        if (!IsValidateRow) {
                            FlagGridMessage = 1;
                            return IsValidateRow;
                        }
                        else if (!IsValidateColoumn) {
                            FlagGridMessage = 2;
                            return IsValidateColoumn;
                        }
                        else if (!IsValidateDropValue) {
                            FlagGridMessage = 3;
                            return IsValidateDropValue;
                        }

                        //FlagGridMessage

                        return true;
                    }

                    return true;
                }
            }
        }).data("kendoValidator");

        var Validate1 = Validator.validateInput('#DivAllowOnlyCheckBox');

        var Validate2 = Validator.validateInput('#DivGridTypeQuestionContainer');

        return Validator.validate() && Validate1 && Validate2;
    },
    OnSelectControlType: function (e) {
        var dataItem = this.dataItem(e.item.index());
        // SurveyDetails.IsEnrollmentSurveyAvailable(dataItem.QuestionTypeId);
        SurveyAddQuestion.ShowViewForEachQuestionType(dataItem.QuestionTypeId);
    },
    OnDataBoundControlType: function () {
        var QuestionTypeId = $("#ObjQuestion_QuestionTypeId").data("kendoDropDownList").value();

        // console.log(QuestionTypeId);
        SurveyAddQuestion.ShowViewForEachQuestionType(QuestionTypeId);
    },
    ShowViewForEachQuestionType: function (QuetionTypeId) {

        if (QuetionTypeId == "1") // TextBox Type
        {
            $("div[data-role='TextBoxType']").show();
            $("div[data-role='DropDownType']").hide();
            $("div[data-role='CommonType']").show();
            $("div[data-role='GridType']").hide();
            //ShowViewForSpecificTextBox

            var TextBoxTypeId = $("#ObjTextBoxType_TextBoxTypeId").data("kendoDropDownList").value();
            SurveyAddQuestion.ShowViewForSpecificTextBox(TextBoxTypeId, QuetionTypeId);

        }
        else if (QuetionTypeId == "2") {   // Dropdown list type
            $("div[data-role='TextBoxType']").hide();
            $("div[data-role='DropDownType']").show();
            $("div[data-role='CommonType']").show();
            $("div[data-role='GridType']").hide();

        }
        else if (QuetionTypeId == "3") {    // Grid type

            //  console.log('kkkk');

            $("div[data-role='TextBoxType']").hide();
            $("div[data-role='DropDownType']").hide();
            $("div[data-role='CommonType']").hide();
            $("div[data-role='GridType']").show();

            //   $("div[data-subrole='GridTypeGenarateRow']").hide();
            if ($("#HdnQuestionId").val() != "0" || $("#HdnIsCopy").val() == "1") {
                SurveyAddQuestion.GenarateViewGridTypeQuestionForEdit();
            }

        }

    },

    OnSelectTextBoxType: function (e) {
        var dataItem = this.dataItem(e.item.index());
        // SurveyDetails.IsEnrollmentSurveyAvailable(dataItem.TextBoxTypeId);
        var QuestionTypeId = $("#ObjQuestion_QuestionTypeId").data("kendoDropDownList").value();
        SurveyAddQuestion.ShowViewForSpecificTextBox(dataItem.TextBoxTypeId, QuestionTypeId);
    },

    OnDataBoundTextBoxType: function () {
        var TextBoxTypeId = $("#ObjTextBoxType_TextBoxTypeId").data("kendoDropDownList").value();
        var QuestionTypeId = $("#ObjQuestion_QuestionTypeId").data("kendoDropDownList").value();
        SurveyAddQuestion.ShowViewForSpecificTextBox(TextBoxTypeId, QuestionTypeId);
    },

    ShowViewForSpecificTextBox: function (TextBoxTypeId, QuestionTypeId) {
        // console.log(TextBoxTypeId);
        // var QuestionTypeId = $("#ObjQuestion_QuestionTypeId").data("kendoDropDownList").value();

        // console.log(QuestionTypeId);
        if (QuestionTypeId == "1") {
            if (TextBoxTypeId == "1") {  /// for text
                $("div[data-subrole='Text']").show();
                $("div[data-subrole='Number']").hide();
            }
            else if (TextBoxTypeId == "2") {  //for number
                $("div[data-subrole='Text']").hide();
                $("div[data-subrole='Number']").show();
            }
            else {
                $("div[data-subrole='Text']").hide();
                $("div[data-subrole='Number']").hide();
            }
        }

    },
    ShowGridHeadrerView: function () {
        var Row = parseInt($("#ObjGridSetting_Row").val());
        var Column = parseInt($("#ObjGridSetting_Column").val());
        if (Row > 0 && Column > 0) {
            $("div[data-subrole='GridTypeGenarateRow']").show();
            $("div[data-subrole='GridTypeGenarateColumn']").show();

            var RowHistory = parseInt($("#HdnGridMaxHeaderRowHistory").val());
            var ColumnHistory = parseInt($("#HdnGridMaxHeaderColHistory").val());

            Row = Row + RowHistory;
            Column = Column + ColumnHistory;


            ///sec:GridHeadreColumnTypeDropDown Column Control Type 1= Text Box 2 - drop list





            for (var i = RowHistory; i < Row; i++) {

                $("div[data-role='GridAddRowHeaderControl']").append("<input name='TxtGridRowHeader' class='form-control marginbtm10' type='text' placeholder='Row " + i + "'>");


            }
            for (var i = ColumnHistory; i < Column; i++) {

                var Control = "<div class='row'> <div class='col-md-5'><input name='TxtGridColumnHeader' class='form-control marginbtm10' type='text' placeholder='Col " + i + "'> </div> ";
                Control += "<div class='col-md-4 selectRightIcon'>";
                Control += " <select  name='DdlGridColumnControlType'class='form-control marginbtm10'>";
                Control += "<option value='1'>Text box</option>";
                Control += "<option value='2'>DropList</option> </select></div>";
                Control += "<div class='col-md-3'>";
                Control += "<a style='display:none' data-val='" + i + "' name='a_PopUpGridColoumn' title='Add Values'>Add Value</a>";
                Control += "<input type='hidden'id='HdnControl_" + i + "' value='0'/> ";
                Control += "</div> </div>";
                $("div[data-role='GridAddColumnHeaderControl']").append(Control);

            }
            $("#HdnGridMaxHeaderRowHistory").val(Row);
            $("#HdnGridMaxHeaderColHistory").val(Column);

            SurveyAddQuestion.AssignControlGridColumnControlType();

        }
    },
    AssignControlGridColumnControlType: function () {
        $("a[name='a_PopUpGridColoumn']").on("click", SurveyAddQuestion.PopUpGridColumnDropDownView);
        $("select[name='DdlGridColumnControlType']").on("change", SurveyAddQuestion.ShowGridColoumnDropDownAddValue);
    },

    PopUpGridColumnDropDownView: function () {

        //console.log($(this).data('val'));
        $("#HdnLatestAddDropdownControlAdd").val($(this).data('val'))
        var ControlValue = $("#HdnControl_" + $(this).data('val')).val();

        var wnd = $("#WndGridColoumnDropdownAddValye").data("kendoWindow");
        wnd.refresh({
            url: AjaxCallUrl.AddGridColoumnDropDownvalueUrl + "?ControlValue=" + ControlValue, // returns JSON, { firstName: "Alyx", lastName: "Vance" }
            // data: { ResourceId: dataValue.ResourceId },
        });
        wnd.open().center();
    },
    ShowGridColoumnDropDownAddValue: function () {
        var AddValue = $(this).parent().parent().children().find("a[name='a_PopUpGridColoumn']").length;
        if ($(this).val() == "2") {
            $(this).parent().parent().children().find("a[name='a_PopUpGridColoumn']").show();
        }
        else {
            $(this).parent().parent().children().find("a[name='a_PopUpGridColoumn']").hide();
        }
    },
    OnOpenAddGridColoumnDropDownvalue: function (e) {
        var wnd = $("#WndGridColoumnDropdownAddValye").data("kendoWindow");
        kendo.ui.progress(wnd.element, true);
    },


    OnRefreshAddGridColoumnDropDownvalue: function (e) {
        var wnd = $("#WndGridColoumnDropdownAddValye").data("kendoWindow");
        kendo.ui.progress(wnd.element, false);
        // AssignEventContractResource();


        SurveyAddQuestion.AssignEventAddGridColoumnDropDownvalue();

    },



    AssignEventAddGridColoumnDropDownvalue: function () {
        $("#Btn_GridColoumnDropDownValue").on("click", SurveyAddQuestion.SaveAddGridColoumnDropDownvalue);
        $("#Btn_Close_WndDropDownValue").on("click", SurveyAddQuestion.CloseGridColoumnDropDownvalue);
    },

    SaveAddGridColoumnDropDownvalue: function () {
        //  console.log('hhhh');
        var Validator = $("#DivPopUpColoumnDropDownvalue").kendoValidator({
            messages: {
                required: "*",
            },
            rules: {
                required: function (input) {
                    if (input.is("[required=required]")) {
                        return $("#Txt_GridColoumnDropDownValue").val().length > 0;
                    }
                    return true;
                }
            }
        }).data("kendoValidator");

        if (Validator.validate()) {
            var ControlId = $("#HdnLatestAddDropdownControlAdd").val();
            var ComaSepartaedValue = $("#Txt_GridColoumnDropDownValue").val();
            $("#HdnControl_" + ControlId).val(ComaSepartaedValue);
            $("#WndGridColoumnDropdownAddValye").data("kendoWindow").close();

        }
    },
    CloseGridColoumnDropDownvalue: function () {
        var Window = $("#WndGridColoumnDropdownAddValye").data("kendoWindow");
        if (Window) {
            Window.close();
        }

    },
    GenarateViewGridTypeQuestionForEdit: function () {

        if ($("#IsPageLoadComplete").val() == "1") {
            return;
        }

        var Row = parseInt($("#ObjGridSetting_Row").val());
        var Column = parseInt($("#ObjGridSetting_Column").val());
        if (Row > 0 && Column > 0) {
            $("div[data-subrole='GridTypeGenarateRow']").show();
            $("div[data-subrole='GridTypeGenarateColumn']").show();

            var RowHistory = parseInt($("#HdnGridMaxHeaderRowHistory").val());
            var ColumnHistory = parseInt($("#HdnGridMaxHeaderColHistory").val());

            Row = Row + RowHistory;
            Column = Column + ColumnHistory;

            //  console.log(Column);
            //   console.log(Row);

            ///sec:GridHeadreColumnTypeDropDown Column Control Type 1= Text Box 2 - drop list

            var IsEditMode = parseInt($("#HdnQuestionId").val()) > 0 || parseInt($("#HdnIsCopy").val()) > 0;

            var RowHeaderValueArray = [];
            var ColoumnHeaderValueArray = [];

            if (!IsEditMode) {
                return;
            }

            if (IsEditMode) {
                if ($("#HdnRowHeaderValue").val() !== undefined) {
                    RowHeaderValueArray = $("#HdnRowHeaderValue").val().split('####RowValue####'); //split by this item
                }

                if ($("#HdnColoumnHeaderValue").val() !== undefined) {
                    ColoumnHeaderValueArray = jQuery.parseJSON($("#HdnColoumnHeaderValue").val().split(',')); //for coloumn it send object
                }
            }

            for (var i = RowHistory; i < Row; i++) {
                $("div[data-role='GridAddRowHeaderControl']").append("<input name='TxtGridRowHeader' class='form-control marginbtm10' type='text' value='" + RowHeaderValueArray[i - 1] + "' placeholder='Row " + i + "'>");
            }

            for (var i = ColumnHistory; i < Column; i++) {


                var Control = "<div class='row'> <div class='col-md-5'><input name='TxtGridColumnHeader' class='form-control marginbtm10' type='text' value='" + ColoumnHeaderValueArray[i - 1].ColVal + "' placeholder='Col " + i + "'> </div> ";
                Control += "<div class='col-md-4'>";
                Control += " <select  name='DdlGridColumnControlType'class='form-control marginbtm10'>";


                Control += "<option value='1'>Text box</option>";
                if (ColoumnHeaderValueArray[i - 1].CltrType == "2") {
                    Control += "<option value='2' selected>DropList</option> </select></div>";
                    Control += "<div class='col-md-3'>";
                    Control += "<a data-val='" + i + "' name='a_PopUpGridColoumn' title='Add Values'>Add Values</a>";
                    Control += "<input type='hidden'id='HdnControl_" + i + "' value='" + ColoumnHeaderValueArray[i - 1].DropDownValue + "'/> ";
                }
                else {
                    Control += "<option value='2'>DropList</option> </select></div>";
                    Control += "<div class='col-md-3'>";
                    Control += "<a style='display:none' data-val='" + i + "' name='a_PopUpGridColoumn' title='Add Values'>Add Values</a>";
                    Control += "<input type='hidden'id='HdnControl_" + i + "' value='0'/> ";
                }


                Control += "</div> </div>";
                $("div[data-role='GridAddColumnHeaderControl']").append(Control);

            }

            $("#HdnGridMaxHeaderRowHistory").val(Row);
            $("#HdnGridMaxHeaderColHistory").val(Column);
            $("#IsPageLoadComplete").val("1");

            SurveyAddQuestion.AssignControlGridColumnControlType();

        }

    }


}
