$(document).ready(function () {
    $("#CloseDataSaveButton").bind("click", function () {

        $("#DataUpdateMessage").data("kendoWindow").close();
    });

});

/* for error use this

<div class='msgboxError'>
<i class='fa fa-exclamation-triangle' aria-hidden='true'></i> 
   <ul><li>Error reason 1</li><li>Data Update Successfully</li><liError reason 2
   </li><li>Error reason 3</li></ul></div>*/


/* for success use this

<div class='msgboxSuccess'>
<i class='fa fa-exclamation-triangle' aria-hidden='true'></i> 
 Data Update Successfully
 </div>

*/


function onCloseSaveMessage() {
    // $("#undo").show();
}

function centerWindowSaveMessage(e) {
    e.sender.center();
}