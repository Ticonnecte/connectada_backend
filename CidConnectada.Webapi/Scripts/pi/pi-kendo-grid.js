//const kendoMessageTmpl = '<div class="k-widget k-tooltip k-tooltip-validation k-invalid-msg field-validation-error" style="margin: 0.5em; display: block; "><span class="k-icon k-warning"> </span>#=message#<div class="k-callout k-callout-nw"></div></div>';

var validationMessageTmplConst = '<div class="k-widget k-tooltip k-tooltip-validation k-invalid-msg field-validation-error" style="margin:0.5em; display: block;" role="alert"><span class="k-icon k-warning"> </span>#=message#<div class="k-callout k-callout-n"></div></div>';

function error_handler(e) {
	if (e.errors) {
		var message = "Errors:\n";
		$.each(e.errors, function (key, value) {
			if ('errors' in value) {
				$.each(value.errors, function () {
					message += this + "\n";
				});
			}
		});
		bootbox.alert(message);
	}
}

function clearGrid(gridName) {
    var grid = $('#' + gridName).data('kendoGrid');
    if (grid) {
        var dataGrid = grid.dataSource.data();
        while (dataGrid.length > 0) {
            grid.removeRow("tr[data-uid='" + dataGrid[0].uid + "']");
        }
    }
}

function getColumnName(values) {
    var result = "";
    for (var propName in values) {
        if (typeof (values[propName]) != "undefined") {
            result = propName;
            break;
        }
    }
    return result;
}

function getColumnValue(values) {
    var result = "";
    for (var propName in values) {
        if (typeof (values[propName]) != "undefined") {
            result = values[propName];
            break;
        }
    }
    return result;
}

function getIndexRowKendoGrid(gridName, dataItem) {
    var data = $('#' + gridName).data("kendoGrid").dataSource.data();
    return data.indexOf(dataItem);
}

function refreshKendoGrid(grid) {
    if (grid != undefined) {
        var kendoGrid = grid.data('kendoGrid');
        if (kendoGrid != undefined) {
            var dataSource = kendoGrid.dataSource;
            dataSource.read();
        }
    }
}

function refreshToolTipValidation(sender, outFocus) {
    sender.element.siblings().first().focus();
    if (outFocus != undefined) {
        outFocus.focus();
    }
    else {
        sender.element.siblings().first().click();
    }
    //toolTipVal.refresh();
}

function gridValidation(gridName, outFocus) {
    var result = true;
    var delay = 0;
    $('#{0} .k-tooltip.k-tooltip-validation.k-invalid-msg'.format(gridName)).each(function () {
        var toolTipVal = $(this).kendoTooltip().data("kendoTooltip");
        toolTipVal.autoHide = false;
        toolTipVal.showOn = 'click';
        setTimeout(refreshToolTipValidation(toolTipVal, outFocus), 120 + delay);
        delay = delay + 20;
        result = false;
    });
    
    return result;

    //var result = true;
    //var container;
    //var message;
    //var gridIndex = gridsNames.indexOf(gridName);
    //var validationMessageTmpl = kendo.template(validationMessageTmplConst);

    //$('#{0} .k-tooltip-validation.k-tooltip-validation'.format(gridName)).each(function (index, element) {
    //    container = $(element).parent();
    //    message = "The '{0}' field is required.".format(element.dataset.for);
    //    $(validationMessageTmpl({ message: message })).appendTo(container);
    //    result = false;
    //});

    //if (gridIndex >= 0 && detailRequiredCtrl.length > gridIndex) {
    //    var grid = $('#{0}'.format(gridName)).data('kendoGrid');
    //    if (grid != undefined) {
    //        var requiredCtrl = detailRequiredCtrl[gridIndex];
    //        var data = grid.dataSource.data();
    //        var row;
    //        var column;
    //        for (var i = 0; i < requiredCtrl.length; i++) {
    //            row = grid.table.find('tr[data-uid="{0}"]'.format(data[i].uid));
    //            for (var j = 0; j < requiredCtrl[i].length; j++) {
    //                column = $.grep(grid.columns, function (e) { return e.field === requiredCtrl[i][j] });
    //                if (column.length == 1) {
    //                    column = column[0];
    //                    container = row.find('td:eq({0})'.format(grid.columns.indexOf(column)));   //Then find the column - includes hidden fields (starting at 0)
    //                    //container.css("border", "6px solid red");
    //                    switch (messageType) {
    //                        case 'required':
    //                            message = "The '{0}' field is required.".format(column.title);
    //                            break;
    //                    }
    //                    if (container.find('.k-tooltip-validation').length == 0) {
    //                        $(validationMessageTmpl({ message: message })).appendTo(container);
    //                    }
    //                    result = false;
    //                }
    //            }
    //        }
    //    }
    //}
    //return result;
}

function setInputsFromGrid(formID, gridName, searchDataClass, indexAttr, columnNameAttr) {
    var data = $('#' + gridName).data('kendoGrid').dataSource.data();
    if (searchDataClass != undefined && indexAttr != undefined && columnNameAttr != undefined) {
        setDataFromInputs(formID, data, searchDataClass, indexAttr, columnNameAttr);
    }
    for (var i = 0; i < data.length; i++) {
        setInputHidden(formID, data[i], gridName + '[' + i + ']');
    }
}

function setInputHidden(formID, data, prefixoNome) {
    var input, numberAtt, numberSep;
    for (var prop in data) {
        if (data.hasOwnProperty(prop)) {
            if (prop == undefined || prop == null || prop == 'uid' || prop == 'parent' || prop == 'dirty' || prop == '__proto__' || prop == '_events') {
                continue;
            }
            //debugger;
            switch (Object.prototype.toString.call(data[prop])) {
                case "[object Object]":
                    setInputHidden(formID, data[prop], prefixoNome + '.' + prop);
                    break;
                case "[object Array]":
                    for (var i = 0; i < data[prop].length; i++) {
                        setInputHidden(formID, data[prop][i], prefixoNome + '.' + prop + '[' + i.toString() + ']');
                    }
                    break;
                default:
                    input = $('#' + formID + ' input[name="' + prefixoNome + '.' + prop + '"]');
                    if (input.length == 0) {
                        input = $('<input type="hidden" name="' + prefixoNome + '.' + prop + '" />');
                        input.appendTo('#' + formID);
                    }
                    try {
                        if (data[prop] != undefined && data[prop] != null && data[prop].toString() != '') {
                            if (isNaN(data[prop]) || isNaN(parseFloat(data[prop]))) {
                                if (!isNaN(Date.parse(data[prop]))) {
                                    debugger;
                                    input.val(data[prop].toLocaleDateString(Globalize.culture().name));
                                    //input.val(data[prop].toLocaleDateString(Globalize.cultures["default"].name));
                                }
                                else {
                                    input.val(data[prop]);
                                }
                            }
                            else {
                                numberAttr = getScaleAndPrecision(data[prop]);
                                numberSep = Globalize.cultures["default"].numberFormat.currency.pattern.toString().replaceAll('$n', '').replaceAll('(', '').replaceAll(')', '').toString();
                                input.val(parseFloat(data[prop]).formatMoney(numberAttr.scale, numberSep, ''));
                            }
                        }
                    } catch (e) {
                        input.val(data[prop]);
                    }
                    break;
            }
        }
    }
}

function setDataFromInputs(formName, dataGrid, searchDataClass, indexAttr, columnNameAttr) {
    $('#' + formName + ' .' + searchDataClass).each(function () {
        if ($(this).hasAttr(indexAttr) && $(this).hasAttr(columnNameAttr)) {
            var index = $(this).attr(indexAttr);
            var columnName = $(this).attr(columnNameAttr);
            if (dataGrid[index].hasOwnProperty(columnName)) {
                dataGrid[index][columnName] = $(this).val();
            }
        }
    });
}

function clearDirtyCells(gridName) {
    $('#' + gridName).find('span.k-dirty').remove();
    $('#' + gridName).find('td[role="gridcell"]').removeClass().removeAttr('data-role');
}

//$('input[type="checkbox"][data-grid-column-name="IndFiltro"], input[type="checkbox"][data-grid-column-name="IndOrdenacao"]').click(function () {
//    debugger;
//    var index = $(this).attr('data-grid-index');
//    var fieldName = $(this).attr('data-grid-column-name');
//    var grid = $('#@gridName1').data('kendoGrid');
//    data = grid.dataSource.data();
//    data[index][fieldName] = $(this).prop('checked');
//});
