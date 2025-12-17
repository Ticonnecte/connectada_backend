////////// INÍCIO UTILITARIOS FORM E INTEGRAÇÃO MVC VIA AJAX....////////////////

//function preencherFormulario(data, habilitar) {
//    if (data[0].Column != undefined) {
//        for (var i = 0; i < data.length; i++) {
//            $('input[name="' + data[i].Column + '"]').val(data[i].Value);
//            $('select[name="' + data[i].Column + '"]').val(data[i].Value);
//        }
//        if (habilitar != undefined)
//            $('.form-control').prop('disabled', !habilitar);
//    }
//    else {
//        //displayMessages(data, 'message-error');

//        var msg = 'Verificar as inconformidades abaixo:\n';
//        for (var i = 0; i < data.length; i++) {
//            msg += data[i] + '\n';
//        }
//        alert(msg);
//    }
//}

// Varre um data Array que vem com objetos do tipo (Column, Value) - representando um único registro.
// implementar para restringir escopo a um único formulário....
function preencherFormulario(data, habilitar, form) {
    var formId = '';

    if (data[0].Column != undefined) {
        for (var i = 0; i < data.length; i++) {
            $('input[name="' + data[i].Column + '"]').val(data[i].Value);
            $('select[name="' + data[i].Column + '"]').val(data[i].Value);
            $('textarea[name="' + data[i].Column + '"]').val(data[i].Value);
            $('input[name="' + data[i].Column + '"][type="checkbox"]').prop('checked', data[i].Value);
        }
        if (habilitar != undefined)
            $('.form-control').prop('disabled', !habilitar);
    }
    else {
        //displayMessages(data, 'message-error');

        var msg = 'Verificar as inconformidades abaixo:\n';
        for (var i = 0; i < data.length; i++) {
            msg += data[i] + '\n';
        }
        alert(msg);
    }
}

function obterColumnValue(data, columnName) {
    var result;
    if (data[0].Column != undefined) {
        for (var i = 0; i < data.length; i++) {
            if (columnName == data[i].Column) {
                result = data[i].Value;
                break;
            }
        }
    }
    return result;
}

////////// FIM UTILITARIOS FORM E INTEGRAÇÃO MVC VIA AJAX....////////////////

///****////
/// UTILITÁRIOS PARA JSON   //////

function serializeJson(form) {

    var jsonData = {};
    var formData = form.serializeArray();
    $.each(formData, function() {
        if (jsonData[this.name]) {
            if (!jsonData[this.name].push) {
                jsonData[this.name] = [jsonData[this.name]];
            }
            jsonData[this.name].push(this.value || '');
        } else {
            jsonData[this.name] = this.value || '';
        }
    });
    return jsonData;
}

function formatDateJson(dateJsonFormat)     {
    var result = "";

    if (dateJsonFormat != null) {
        var value = new Date
                    (
                         parseInt(dateJsonFormat.replace(/(^.*\()|([+-].*$)/g, ''))
                    );
        result = value.getDate() +
                               "/" +
                (value.getMonth() + 1).toString() +
                                   "/" +
               value.getFullYear();

    }
    return result;
}

///****////
/// UTILITÁRIOS PARA STRING   //////

String.prototype.replaceAll = function (output, input) {
    if (this == undefined)
        return undefined;
    if (this.indexOf(output) == -1)
        return this;
    else
        return this.replace(output, input).replaceAll(output, input);
}

String.prototype.extractIndexes = function () {
    var result = [];
    var subStr = '';
    var index;
    var abreColcheteIndex = this.indexOf('[');
    var fechaColcheteIndex = this.indexOf(']');

    if (abreColcheteIndex >= 0 && fechaColcheteIndex >= 0 && abreColcheteIndex < fechaColcheteIndex) {
        index = this.substring(abreColcheteIndex + 1, fechaColcheteIndex);
        result.push(parseInt(index));
        if (fechaColcheteIndex < this.length - 1)  {
            result = result.concat(this.substring(fechaColcheteIndex + 1, this.length).extractIndexes());
        }
    }
    return result;
}


///****////
/// UTILITÁRIOS PARA NÚMEROS   //////

function checkNumber(field, rules, i, options) {
    if (isNaN(field.val())) {
        // this allows to use i18 for the error msgs
        return "* Informe um valor numérico";
    }
}

Number.prototype.formatMoney = function (c, d, t) {
    var n = this, c = isNaN(c = Math.abs(c)) ? 2 : c, d = d == undefined ? "," : d, t = t == undefined ? "." : t, s = n < 0 ? "-" : "", i = parseInt(n = Math.abs(+n || 0).toFixed(c)) + "", j = (j = i.length) > 3 ? j % 3 : 0;
    return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? d + Math.abs(n - i).toFixed(c).slice(2) : "");
};

// Na visão SQL...
function getScaleAndPrecision(num) {
    var result = null;
    var precision, scale;
    var numStr;

    if (!isNaN(num) && !isNaN(parseFloat(num))) {
        if (num.toString().indexOf('.') > 0) {
            numStr = parseFloat(num).toString();
            precision = numStr.length - 1;
            scale = numStr.length - numStr.indexOf('.') - 1;
        }
        else {
            numStr = parseInt(num).toString();
            precision = numStr.length;
            scale = 0;
        }
        result = { scale: scale, precision: precision };
    }

    return result;
}