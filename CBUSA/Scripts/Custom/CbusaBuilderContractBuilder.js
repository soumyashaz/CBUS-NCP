var type;
var flag;

$(document).ready(function () {
    $(".a_ContractResource").on("click", function () {
        var wnd = $("#ContractResource").data("kendoWindow");
        wnd.refresh({
            url: AjaxCallUrl.ContractResourceUrl, // returns JSON, { firstName: "Alyx", lastName: "Vance" }
            // data: { ResourceId: dataValue.ResourceId },
            data: { ContractResourceId: 0 }
        });
        wnd.open().center();
    });

    $('thead').css('display', 'inline-block');
    $('thead').css('width', '1140');
    $('tbody').css('display', 'inline-block');
    $('tbody').css('height', '300px');
    $('tbody').css('width', '1140');
});

function popup(contractid, resoursecatid) {
    type = contractid;
    flag = resoursecatid;
    var wnd = $("#ContractResource").data("kendoWindow");
    //var wnd = $("#ContractResourceCategory").data("kendoWindow");
    wnd.refresh({
        url: AjaxCallUrl.ContractResourceUrl, // returns JSON, { firstName: "Alyx", lastName: "Vance" }
        // data: { ResourceId: dataValue.ResourceId },
        data: { ContractResourceId: contractid }
    });
    wnd.open().center();

    setTimeout(DisplayEmptyListMessage, 1500);
}

function DisplayEmptyListMessage() {
    var ListItemCount = $("#listViewContractActiveAsc12").data("kendoListView").dataSource.total();

    if (ListItemCount == 0) {
        $("#listViewContractActiveAsc12").append("<div><h4>No Resource Uploaded</h4></div>");
    } else {
        $("#listViewContractActiveAsc12").find('tr').first().find('a.list-group-item').first().click();
    }
}

function surveyredirect(surveyid, builderid) {

    var url = AjaxCallUrl.RedirectSurveyUrl.replace("****SurveyId****", surveyid);
    url = url.replace("****BuilderId****", builderid);
    var win = window.open(url, '_blank');
        win.focus();
    //window.location.href = url;
}

function survey(surveyid, builderid) {

    var url = AjaxCallUrl.RedirectContractUrl.replace("****SurveyId****", surveyid);
    url = url.replace("****BuilderId****", builderid);
    var win = window.open(url, '_blank');
    win.focus();
    //window.location.href = url;

}

function OnOpenContactResource(e) {
    var wnd = $("#ContractResource").data("kendoWindow");
    kendo.ui.progress(wnd.element, true);
}

function OnRefreshContractResource(e) {
    var wnd = $("#ContractResource").data("kendoWindow");
    kendo.ui.progress(wnd.element, false);
   // AssignEventContractResource();

}

function myFunction(catid, contractid) {
  
    $("a").removeClass("active");
    $('#p'+catid).addClass('active');
   
    type = contractid;
    flag = catid;
    var KendoListView = $("#listViewResource").data("kendoListView");
    KendoListView.dataSource.read();
    
}

function SendResourceContract() {

    return { ContractId: type};

}

function SendResourceData() {
    
    return { ContractId: type, CatId: flag};

}

function ViewContractResource(ResourceId) {
    
    //var wnd = $("#WndContractResourceView").data("kendoWindow");
    //wnd.refresh({
    //    url: AjaxCallUrl.ContractResourceViewUrl, // returns JSON, { firstName: "Alyx", lastName: "Vance" }
    //    data: { ResourceId: $("#HdnContactResourceListId_" + ResourceId).val() }
    //});
    //wnd.open().center();


    // var ResourceId = $(this).siblings().eq(0).val();
    var src = AjaxCallUrl.DownLaodResourceFile + '?ResourceId=' + ResourceId;
    //   console.log(src);

    var iframe = $("<iframe/>").load(function () {
        // $.unblockUI();
    }).attr({
        src: src
    }).appendTo($("#DivIframeContainer"));

}
//$(document).ready(function () {
 
//    var showChar = 1;
//    var ellipsestext = "...";
//    var moretext = "more";
//    var lesstext = "less";
//    function SurveyMarketBuilderLlistDataBound() {
//        $('.more').each(function () {
        
//            var content = $(this).html();
//            alert(content);
//            if (content.length > showChar) {

//                var c = content.substr(0, showChar);
//                var h = content.substr(showChar - 1, content.length - showChar);

//                var html = c + '<span class="moreellipses">' + ellipsestext + '&nbsp;</span><span class="morecontent"><span>' + h + '</span>&nbsp;&nbsp;<a href="" class="morelink">' + moretext + '</a></span>';

//                $(this).html(html);
//            }

//        });

//        $(".morelink").click(function () {
//            if ($(this).hasClass("less")) {
//                $(this).removeClass("less");
//                $(this).html(moretext);
//            } else {
//                $(this).addClass("less");
//                $(this).html(lesstext);
//            }
//            $(this).parent().prev().toggle();
//            $(this).prev().toggle();
//            return false;
//        });
//    }
//});

function SurveyMarketBuilderLlistDataBound() {
    var showChar = 2;
    var ellipsestext = "...";
    var moretext = "more";
    var lesstext = "less";
    $('.more').each(function () {
      
        var content = $(this).html();
       
        if (content.length > showChar) {
           
            var c = content.substr(0, showChar);
            var h = content.substr(showChar - 1, content.length - showChar);

            var html = c + '<span class="moreellipses">' + ellipsestext + '&nbsp;</span><span class="morecontent"><span>' + h + '</span>&nbsp;&nbsp;<a href="" class="morelink">' + moretext + '</a></span>';

            $(this).html(html);
        }

    });

    $(".morelink").click(function () {
        if ($(this).hasClass("less")) {
            $(this).removeClass("less");
            $(this).html(moretext);
        } else {
            $(this).addClass("less");
            $(this).html(lesstext);
        }
        $(this).parent().prev().toggle();
        $(this).prev().toggle();
        return false;
    });
}

function SurveyMarketBuilderLlistDataBoundActive() {
    var showChar = 5;
    var ellipsestext = "...";
    var moretext = "more";
    var lesstext = "less";
    $('.moreac').each(function () {

        var content = $(this).html();

        if (content.length > showChar) {
            
            var c = content.substr(0, showChar);
            var h = content.substr(showChar - 1, content.length - showChar);

            var html = c + '<span class="moreellipses">' + ellipsestext + '&nbsp;</span><span class="morecontent"><span>' + h + '</span>&nbsp;&nbsp;<a href="" class="morelinkac">' + moretext + '</a></span>';

            $(this).html(html);
        }

    });

    $(".morelinkac").click(function () {
        if ($(this).hasClass("less")) {
            $(this).removeClass("less");
            $(this).html(moretext);
        } else {
            $(this).addClass("less");
            $(this).html(lesstext);
        }
        $(this).parent().prev().toggle();
        $(this).prev().toggle();
        return false;
    });
}

function SurveyMarketBuilderLlistDataBoundActiveJoin() {
    var showChar = 22;
    var ellipsestext = "...";
    var moretext = "more";
    var lesstext = "less";
    $('.moreacjoin').each(function () {

        var content = $(this).html();

        if (content.length > showChar) {
         
            var c = content.substr(0, showChar);
            var h = content.substr(showChar - 1, content.length - showChar);

            var html = c + '<span class="moreellipses">' + ellipsestext + '&nbsp;</span><span class="morecontent"><span>' + h + '</span>&nbsp;&nbsp;<a href="" class="morelinkacjoin">' + moretext + '</a></span>';

            $(this).html(html);
        }

    });

    $(".morelinkacjoin").click(function () {
        if ($(this).hasClass("less")) {
            $(this).removeClass("less");
            $(this).html(moretext);
        } else {
            $(this).addClass("less");
            $(this).html(lesstext);
        }
        $(this).parent().prev().toggle();
        $(this).prev().toggle();
        return false;
    });
}

function SurveyMarketBuilderLlistDataBoundPendingJoin() {
    var showChar = 1;
    var ellipsestext = "...";
    var moretext = "more";
    var lesstext = "less";
    $('.s').each(function () {

        var content = $(this).html();

        if (content.length > showChar) {
         
            var c = content.substr(0, showChar);
            var h = content.substr(showChar - 1, content.length - showChar);

            var html = c + '<span class="moreellipses">' + ellipsestext + '&nbsp;</span><span class="morecontent"><span>' + h + '</span>&nbsp;&nbsp;<a href="" class="morelinkpenjoin">' + moretext + '</a></span>';

            $(this).html(html);
        }

    });

    $(".morelinkpenjoin").click(function () {
        if ($(this).hasClass("less")) {
            $(this).removeClass("less");
            $(this).html(moretext);
        } else {
            $(this).addClass("less");
            $(this).html(lesstext);
        }
        $(this).parent().prev().toggle();
        $(this).prev().toggle();
        return false;
    });
}