function mostrarDialog(windowId, urlContent, title) {
    var window = $("#" + windowId);

    var defaultContent = "Carregando. Aguarde...";
    if ($('#' + windowId + "_ConteudoIni") != undefined) {
        defaultContent = $('#' + windowId + "_ConteudoIni").val();
    }

    //if ($('#coverDialog') != undefined) {
    //    defaultContent = $('#coverDialog').html();
    //}
    CustomMessage.init({
        messageId: 'messageDialog'
    });
    window.kendoWindow({
        width: "50%",
        height: "80%",
        modal: true,
        draggable: true,
        resizable: true,
        close: function (e) {
            var dialog = $("#" + windowId).data("kendoWindow");
            dialog.content(defaultContent);
        },
        animation: {
            open: {
                effects: { fadeIn: {} },
                duration: 800,
                show: true
            }
        },
        modal: true,
        content: urlContent
    });

    if (title != undefined) {
        window.data("kendoWindow")
            .center()
            .title(title)
            .open();
    } else {
        window.data("kendoWindow")
            .center()
            .open();
    }
}

function fecharDialog(windowId) {
    CustomMessage.reset();
    var window = $('#' + windowId);
    window.data("kendoWindow").close();
}