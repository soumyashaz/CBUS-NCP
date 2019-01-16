$(document).ready(function () {


    // SurveyAddQuestion.AssignControl();
    SurveyInvites.AssignControl();


});

var SurveyInvites = {
    AssignControl: function () {
        $("#a_openaddinvitespopup").on("click", SurveyInvites.PopUpSurveyInvites);
        //  $("#Btn_AddQuestion").on("click", SurveyAddQuestion.SaveNewQuestion);
    },
    PopUpSurveyInvites: function () {
        var wnd = $("#WndSurveyInvitesPopup").data("kendoWindow");
        wnd.refresh({
            url: AjaxCallUrl.SurveyInvitesPopupUrl, // returns JSON, { firstName: "Alyx", lastName: "Vance" }
            // data: { ResourceId: dataValue.ResourceId },
        });
        wnd.open().center();
    },

    OnOpenSurveyInvitesPopup: function (e) {
        var wnd = $("#WndSurveyInvitesPopup").data("kendoWindow");
        kendo.ui.progress(wnd.element, true);
    },


    OnRefreshSurveyInvitesPopup: function (e) {
        var wnd = $("#WndSurveyInvitesPopup").data("kendoWindow");
        kendo.ui.progress(wnd.element, false);
        // AssignEventContractResource();
        SurveyInvites.AddEventSurveyInvitesPopup();

    },
    AddEventSurveyInvitesPopup: function () {
        $("#DivZoneStateDropdown").on("click", SurveyInvites.OpenMarketCustomControl);
        $("#BtnSaveSurveyMarket").on("click", SurveyInvites.SaveSurveyMarket);
        $("#BtnCanvelarket").on("click", SurveyInvites.CancelSurveyMarket);
    },
    OpenMarketCustomControl: function () {
        CustomControlZoneStateList($(this).parent().parent(), "CustomControlPoupZoneState");
    },
    SaveSurveyMarket: function () {
        SurveyInvites.ValidateSurveyMarketSave();
    },
    ValidateSurveyMarketSave: function () {

        var Validator = $("#DivConfigureSurveyInvite").kendoValidator({
            messages: {
                ZoneSate: function (input) {
                    return "Atleast one market selection is required";
                },
            },
            rules: {
                ZoneSate: function (input) {
                    if (input.is("[data-role=ZoneSate]")) {

                        // console.log($("#ZoneStateList").children().find("input[name='ChkStateSelect']").length);
                        var CheckedCheckBox = $("#ZoneStateList").children().find("input[name='ChkStateSelect']:checked");
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
            }

        }).data("kendoValidator");

        Validator.validateInput($("#DivZoneState"));
    },
    CancelSurveyMarket: function () {

    }


}