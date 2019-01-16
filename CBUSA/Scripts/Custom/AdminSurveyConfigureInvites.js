$(document).ready(function () {
    // SurveyAddQuestion.AssignControl();
    SurveyInvites.AssignControl();
    
    $('thead').first().css('display', 'inline-block');
    $('thead').first().css('width', '800');
    $('tbody').first().css('display', 'inline-block');
    $('tbody').first().css('height', '300px');
    $('tbody').first().css('width', '800');
});



function SendSurveyAsParameter() {

    return { SurveyId: $("#HdnSurveyId").val() };
}

function SurveyMarketBuilderListDataBound() {
    $
}


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

        if ($("#DivConfigureSurveyInvite").length) {
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

    },
    OpenMarketCustomControl: function () {
        CustomControlZoneStateList($(this).parent().parent(), "CustomControlPoupZoneState");



    },
    SaveSurveyMarket: function () {

        //console.log('sdsad');
        var Isvalid = SurveyInvites.ValidateSurveyMarketSave();
        //  console.log(Isvalid);

        if (Isvalid) {

            kendo.ui.progress($("#DivConfigureSurveyInvite"), true);

            var SelectedMarket = $("#ZoneStateList").children().find("input[name='ChkStateSelect']:checked");
            var MarketList = [];
            $(SelectedMarket).each(function () {
                MarketList.push($(this).siblings().eq(0).val());
            });
            var PostData = { MarketList: MarketList, SurveyId: $("#HdnSurveyId").val() };

            $.ajax({ //Process the form using $.ajax()
                type: 'POST', //Method type
                url: AjaxCallUrl.SaveSurveyMarket,
                data: PostData, //Forms name
                dataType: 'json',
                // traditional: true,
                //   contentType: false, // Not to set any content header  
                //  processData: false,
                beforeSend: function () {
                },
                success: function (data) {

                    var DataUpdateWindow = $("#DataUpdateMessage").data("kendoWindow");
                    if (data.IsSuccess) {

                        kendo.ui.progress($("#DivConfigureSurveyInvite"), false);
                        if (DataUpdateWindow) {

                            DataUpdateWindow.title("Update Information");
                            DataUpdateWindow.content("<div class='msgboxSuccess'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i> Data Update Successfully</div>");
                            DataUpdateWindow.open();

                            var KendoListView = $("#SurveyMarketBuilderList").data("kendoListView");
                            KendoListView.dataSource.read();
                        }
                    }
                    else {
                        kendo.ui.progress($("#DivConfigureSurveyInvite"), false);
                        if (DataUpdateWindow) {
                            DataUpdateWindow.title("Error Information");
                            DataUpdateWindow.content("<div class='msgboxError'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i>" + data.ModelError + " </div>");
                            DataUpdateWindow.open();
                        }
                    }
                },
                error: function (request, error) {
                    kendo.ui.progress($("#DivConfigureSurveyInvite"), false);
                }
            });




        }
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

        return Validator.validateInput($("#DivZoneState"));
    },
    CancelSurveyMarket: function () {
        var DataUpdateWindow = $("#WndSurveyInvitesPopup").data("kendoWindow");
        if (DataUpdateWindow) {
            DataUpdateWindow.close();
        }
    },
    RemoveSurveyMarket: function (MarketId) {

        var Confirm = confirm("Are you sure you want to delete this item?")
        if (Confirm) {

            kendo.ui.progress($("#SurveyMarketBuilderList"), true);
            var PostData = { MarketId: MarketId, SurveyId: $("#HdnSurveyId").val() };

            $.ajax({ //Process the form using $.ajax()
                type: 'POST', //Method type
                url: AjaxCallUrl.RemoveSurveyMarket,
                data: PostData, //Forms name
                dataType: 'json',
                // traditional: true,
                //   contentType: false, // Not to set any content header  
                //  processData: false,
                beforeSend: function () {
                },
                success: function (data) {

                    var DataUpdateWindow = $("#DataUpdateMessage").data("kendoWindow");
                    if (data.IsSuccess) {

                        kendo.ui.progress($("#SurveyMarketBuilderList"), false);
                        if (DataUpdateWindow) {

                            DataUpdateWindow.title("Update Information");
                            DataUpdateWindow.content("<div class='msgboxSuccess'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i> Data Update Successfully</div>");
                            DataUpdateWindow.open();

                            var KendoListView = $("#SurveyMarketBuilderList").data("kendoListView");
                            KendoListView.dataSource.read();
                        }
                    }
                    else {
                        kendo.ui.progress($("#SurveyMarketBuilderList"), false);
                        if (DataUpdateWindow) {
                            DataUpdateWindow.title("Error Information");
                            DataUpdateWindow.content("<div class='msgboxError'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i>" + data.ModelError + " </div>");
                            DataUpdateWindow.open();
                        }
                    }
                },
                error: function (request, error) {
                    kendo.ui.progress($("#SurveyMarketBuilderList"), false);
                }
            });
        }






    }


}