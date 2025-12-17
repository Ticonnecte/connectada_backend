var CustomReportObject = null;

var CustomReport = (function () {
    "use strict";

    var that = {};

    //that.init = function (optionsGrid) {
    //    that.reset();
    //    CustomReportObject = {};

    //    // Grid...
    //    if (optionsGrid.id == undefined || optionsGrid.id == null) {
    //        CustomReportObject.Grid = undefined;
    //    }
    //    else {
    //        CustomReportObject.GridId = optionsGrid.id;
    //    }

    //    if (optionsGrid.linkLoad == undefined || optionsGrid.linkLoad == null) {
    //        CustomReportObject.LinkLoad = undefined;
    //    }
    //    else {
    //        CustomReportObject.LinkLoad = $('#' + optionsGrid.linkLoad);
    //    }

    //    if (optionsGrid.urlSalvar == undefined || optionsGrid.urlSalvar == null) {
    //        CustomReportObject.UrlSalvar = undefined;
    //    }
    //    else {
    //        CustomReportObject.UrlSalvar = optionsGrid.urlSalvar;
    //    }

    //    if (optionsGrid.targetId == undefined || optionsGrid.targetId == null) {
    //        CustomReportObject.Target = undefined;
    //    }
    //    else {
    //        CustomReportObject.Target = $('#' + optionsGrid.targetId);
    //        CustomReportObject.TargetId = optionsGrid.targetId;
    //    }

    //};

    that.new = function (reportName, titleDialog) {
        CustomReportObject = {};
        CustomMessage.init({
            messageId: 'messageDialog'
        });
        CustomReportObject.ReportName = reportName;
        CustomReportObject.Url = '/Home/GetCrystalParameters?reportName=' + reportName;
        mostrarDialog('crystalDialog', CustomReportObject.Url, "Relatório - " + titleDialog);
    };

    return that;
}());
