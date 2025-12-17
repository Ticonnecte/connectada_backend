var delayMessage;
var uidCurrentRow;

const headerListaId = "headerLista{0}";

var tipoFiltroEnum = [];
tipoFiltroEnum[1] = "Igual";
tipoFiltroEnum[2] = "Diferente";
tipoFiltroEnum[3] = "Maior";
tipoFiltroEnum[4] = "Menor";
tipoFiltroEnum[5] = "MaiorOuIgual";
tipoFiltroEnum[6] = "MenorOuIgual";
tipoFiltroEnum[7] = "IniciandoPor";
tipoFiltroEnum[8] = "Contendo";
tipoFiltroEnum[9] = "FinalizandoPor";
tipoFiltroEnum[10] = "NaoContendo";

function getFiltros() {
	return getPesquisaAttributesModel();
}
	
function getPesquisaAttributesModel(){
    var tipoOperadorFiltro = $('input[name="TipoOperadorFiltro"]').val();
    var executarPesquisa = $('input[name="ExecutarPesquisa"]').val();
    var useCaseId = $('input[name="UseCaseId"]').val();
    var fieldNames = $('input[name^="FieldName"]');
    var fieldValueSet = [];
    var fieldVal;

    debugger;

    for (var i = 0; i < fieldNames.length; i++) {
        fieldVal = {};
        fieldVal.FieldName = $('input[name="FieldName[{0}]"]'.format(i)) ? $('input[name="FieldName[{0}]"]'.format(i)).val() : null;
        fieldVal.FieldDescription = $('input[name="FieldDescription[{0}]"]'.format(i)) ? $('input[name="FieldDescription[{0}]"]'.format(i)).val() : null;
        fieldVal.FieldType = $('input[name="FieldType[{0}]"]'.format(i)) ? $('input[name="FieldType[{0}]"]'.format(i)).val() : null;
        fieldVal.InputType = $('input[name="InputType[{0}]"]'.format(i)) ? $('input[name="InputType[{0}]"]'.format(i)).val() : null;
        fieldVal.FieldTipoDeFiltro = $('input[name="FieldTipoDeFiltro[{0}]"]'.format(i)) ? $('input[name="FieldTipoDeFiltro[{0}]"]'.format(i)).val() : null;

        switch ($('[name="FieldValue[{0}]"]'.format(i)).length) {
            case 1:
                fieldVal.FieldValue = $('[name="FieldValue[{0}]"]'.format(i)) ? $('[name="FieldValue[{0}]"]'.format(i)).val() : null;
                break;
            default:
                if ($('[name="FieldValue[{0}]"][type="radio"]:checked'.format(i)).length == 1) {
                    fieldVal.FieldValue = $('[name="FieldValue[{0}]"][type="radio"]:checked'.format(i)).val();
                }
                break;
        }
        fieldValueSet.push(fieldVal);
    }
    return { ExecutarPesquisa: executarPesquisa, UseCaseId: useCaseId, TipoOperadorFiltro: tipoOperadorFiltro, FieldValueSet: fieldValueSet };
}

function refreshGrid(cadAttrModel) {
    refreshKendoGrid($('#' + cadAttrModel.FilterObject.GridPesqName));
}

function pesquisarCad(cadAttrModel, customMsg, completeFunction) {
    debugger;
    $('#' + cadAttrModel.FilterObject.PesqResultDivId).hide();
    var form = $('#' + cadAttrModel.FilterObject.PesqFormName);
    var result = formValid(form);
    if (result) {
        refreshGrid(cadAttrModel);
    }
    else {
        customMsg.displayMessage('A pesquisa não pode ser realizada. Verificar os motivos nos campos correspondentes, corrigir e tentar novamente.', 'warning', msgTime);
    }
    return result;
}

function dataSourceRequestEndDynamic(cadAttrModel, e, completeFunction) {
    var msg = '';
    var tipoAlerta = 'success';
    debugger;
    if (Object.prototype.toString.call(e.response) == '[object Object]') {
        if (e.response != undefined && e.response.Data.length > 0) {
            $('#' + cadAttrModel.FilterObject.PesqResultDivId).show();
        }
        else {
            tipoAlerta = 'warning';
            msg = "Nenhum registro encontrado a partir do filtro especificado.";
        }
    }
    else {
        msg = errorMessageAdm;
        tipoAlerta = 'danger';
    }
    if (completeFunction != undefined) {
        if (e.response != undefined) {
            completeFunction(e.response.Data, msg, tipoAlerta);
        }
        else {
            completeFunction([], msg, tipoAlerta);
        }
    }
    else if (msg != '') {
        CustomMessage.displayMessage(msg, tipoAlerta, undefined);
    }
    if (tipoAlerta == 'success') {
        clearDirty('#{0}'.format(cadAttrModel.FilterObject.PesqFormName));
    }
}

function getLinkEditCad(cadAttrModel, id, descricao, actionRoute, linkComplement) {
    var retorno;
    debugger;
    var routeKey = '';
    if (Object.prototype.toString.call(id) === "[object Object]") {
        for (var prop in id) {
            if (id.hasOwnProperty(prop) && typeof id[prop] != 'Object' && typeof id[prop] != 'object' && typeof id[prop] != 'function' && prop != 'uid') {
                if (routeKey == '') {
                    routeKey = 'id.' + prop + '=' + id[prop];
                }
                else {
                    routeKey = routeKey + '&id.' + prop + '=' + id[prop];
                }
            }
        }
    }
    else {
        routeKey = 'id=' + id;
    }

    if (linkComplement != undefined) {
        retorno = "<a href='" + actionRoute + "?" + routeKey + linkComplement + "' title='" + cadAttrModel.Title + " edit...''" + descricao + "'''>" + descricao + "</a>";
    }
    else {
        retorno = "<a href='" + actionRoute + "?" + routeKey + "' title='" + cadAttrModel.Title + " edit...''" + descricao + "'''>" + descricao + "</a>";
    }
    return retorno;
}

function excluirCad(cadAttrModel, id, descricao, uid) {
    bootbox.confirm("Confirmar exclusão:\n " + cadAttrModel.Title + " '" + descricao + "'?", function (result) {
        CustomMessage.init({
            messageId: cadAttrModel.GridMessageId
        });
        if (result) {
            if (Object.prototype.toString.call(id) !== "[object Object]") {
                $('input[name="Key"]').val(id);
            }
            uidCurrentRow = uid;
            debugger;
            $('#{0}'.format(cadAttrModel.FilterObject.DeleteFormName)).submit();
        }
    });
}

function excluirOnBeginCad(cadAttrModel) {
    showCover(cadAttrModel.FilterObject.AmpulhetaGridId, "Excluindo registro. Aguarde...");
}

function excluirOnCompleteCad(cadAttrModel) {
    $("#{0}".format(cadAttrModel.FilterObject.AmpulhetaGridId)).fadeOut(400);
}

function excluirOnFailure() {
    return;
}

function excluirOnSuccessCad(cadAttrModel, data, status, xhr) {
    debugger;
    switch (status) {
        case "success":
            var grid = $("#" + cadAttrModel.FilterObject.GridPesqName).data("kendoGrid");
            grid.removeRow("tr[data-uid='" + uidCurrentRow + "']");
            break;
    }
    return;
}

function dataSourceError(e) {
    CustomMessage.displayMessages(e.errors, '@MessageType.danger.ToString()', msgTime);
}

function pdfExport(e) {
    e.sender.hideColumn('IsNew');
    e.promise
    //.progress(function (e) {
    //    console.log(kendo.format("{0:P} complete", e.progress));
    //})
    .done(function () {
        e.sender.showColumn('IsNew');
    });
}

function pesquisarReset(cadAttrModel, url) {
    clearForm(cadAttrModel.FilterObject.PesqFormName);

    $.ajax({
        url: url,
        type: 'GET',
        dataType: "json",
        cache: false,
        contentType: "application/json; charset=utf-8",
        //contentType: false,
        processData: false,
        async: true,
        beforeSend: function () {
            return;
        },
        success: function (data) {
            CustomMessage.clear();
            $('#' + cadAttrModel.FilterObject.PesqResultDivId).hide();
            return;
        },
        error: function (data) {
            CustomMessage.displayMessage(errorMessageAdm, 'danger');
            return;
        },
        complete: function () {
            return;
        },
    });
}