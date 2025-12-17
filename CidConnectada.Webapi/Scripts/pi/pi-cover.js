function showCover(coverId, message) {
    var coverDiv = $('#' + coverId);
    if (coverDiv != undefined)  {
        var coverSpan = coverDiv.find('span');
        if (coverSpan != undefined && message != undefined) {
            coverSpan.html(message);
        }
        coverDiv.fadeIn(100);
    }
}