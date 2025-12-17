function clearMultiSelect(selector) {
    var selection = $(selector);
    var multiSelect;

    if (selection.length == 1) {
        multiSelect = selection.data('kendoMultiSelect');
        multiSelect.value([]);
    } else if (selection.length > 0) {
        selection.each(function () {
            multiSelect = $(this).data('kendoMultiSelect');
            multiSelect.value([]);
        });
    }
}
