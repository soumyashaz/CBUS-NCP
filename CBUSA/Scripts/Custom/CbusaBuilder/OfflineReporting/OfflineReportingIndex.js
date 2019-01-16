$(document).ready(function () {

    $("#UploadProjectFile").kendoUpload({
        async: {
            saveUrl: AjaxCallUrl.UpdateProjectList,
            removeUrl: "",
            autoUpload: true
        },
        validation: {
            allowedExtensions: [".xlsx", ".xls"]
        },
        localization: {
            select: 'SELECT FILE'
        },
        showFileList: false,
        dropZone: ".dropZoneElement",
        multiple: false,
        select: onFileSelect,
        complete: onProjectListUploadComplete
    });

    $("#UploadProjectFile").kendoUpload().attr("accept", ".xls,.xlsx");
    $("#UploadProjectFile").removeAttr("multiple");

    $("#UploadQuarterReportFile").kendoUpload({
        async: {
            saveUrl: AjaxCallUrl.UploadQuarterReport,
            removeUrl: "",
            autoUpload: true
        },
        validation: {
            allowedExtensions: [".xlsx", ".xls"]
        },
        localization: {
            select: 'SELECT FILE'
        },
        showFileList: false,
        dropZone: ".dropZoneElement",
        multiple: false,
        select: onFileSelect,
        complete: onQuarterReportUploadComplete
    });

    $("#UploadQuarterReportFile").kendoUpload().attr("accept", ".xls,.xlsx");
    $("#UploadQuarterReportFile").removeAttr("multiple");
});

function onFileSelect (e) {
    if (e.files.length > 1) {
        alert("You are allowed to select only ONE file!");
        e.preventDefault();
    } else {
        var fileName = e.files[0].name;
        var fileExtension = fileName.substring(fileName.length - 4, fileName.length);

        if (fileExtension != '.xls' && fileExtension != 'xlsx') {
            alert("Files of type .xlsx and/or .xls are allowed");
            e.preventDefault();
        }
    }
}

function onProjectListUploadComplete(e) {
    window.location.href = AjaxCallUrl.AddProject;
}

function onQuarterReportUploadComplete(e) {
    window.location.href = AjaxCallUrl.SubmitNCPRebateReport;
}
