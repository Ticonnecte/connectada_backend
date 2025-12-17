/*!
 * Pi Script - Scripts da API Pi.
 */

const pathNameSystemFolder = 'SystemFolder';

var errorMessageAdm = "Houve um problema ao tentar realizar a opera&#231;&#227;o. Tente novamente ou entre em contato com o administrador.";

var CustomMessageObject = null;

var CustomMessage = (function () {
    "use strict";

    var CustomMessageObject = null;

    var that = {};

    that.init = function (options) {
        // antes era clear...ficou reset, mas voltou a ser clear...
        that.clear();
        CustomMessageObject = {};

        if (options != undefined && options != null && options.messageId != undefined && options.messageId != null) {
            CustomMessageObject.messageId = options.messageId;
            CustomMessageObject.messageWrapper = $("#messagewrapper" + options.messageId);
            CustomMessageObject.messagesContainer = $("#messagesContainer" + options.messageId);
        }
        else {
            CustomMessageObject.messageId = '';
            CustomMessageObject.messageWrapper = $("#messagewrapper");
            CustomMessageObject.messagesContainer = $("#messagesContainer");
        }

        if (options != undefined && options != null) {
            switch (options.type) {
                case 'danger':
                case 'success':
                case 'warning':
                    CustomMessageObject.type = options.type;
                    break;
                default:
                    CustomMessageObject.type = 'warning';
                    break;
            }

            if (options.message == undefined || options.message == null) {
                CustomMessageObject.message = '';
            }
            else {
                CustomMessageObject.message = options.message;
            }

            if (options.messages == undefined || options.messages == null) {
                CustomMessageObject.messages = '';
            }
            else {
                CustomMessageObject.messages = options.messages;
            }

            if (options.duration == undefined || options.duration == null) {
                CustomMessageObject.duration = 0;
            }
            else {
                CustomMessageObject.duration = options.duration;
            }
        }
    };

    that.clear = function () {
        apagarMensagensValidation();
        clearMessages();
        if (that.isInit()) {
            if (CustomMessageObject != null) {
                CustomMessageObject.messagesContainer.hide();
                CustomMessageObject.messageWrapper.empty();
            }
        }
    };

    that.isVisible = function () {
        return that.isInit() && CustomMessageObject.visible;
    }

    that.reset = function () {
        that.clear();
        //CustomMessageObject = null;
        that.init();
    };

    that.isInit = function () {
        return CustomMessageObject != null && CustomMessageObject != {};
    };

    that.messageId = function () {
        return CustomMessageObject.messageId;
    };

    that.type = function () {
        return CustomMessageObject.type;
    };

    that.type = function (type) {
        if (type != undefined) {
            CustomMessageObject.type = type;
        }
        else {
            CustomMessageObject.type = 'warning';
        }

    };

    that.message = function () {
        return CustomMessageObject.message;
    };

    that.message = function (messageDesc) {
        CustomMessageObject.message = messageDesc;
    };

    that.messages = function () {
        return CustomMessageObject.messages;
    };

    that.messages = function (messagesDesc) {
        CustomMessageObject.messages = messagesDesc;
    };

    that.duration = function () {
        return CustomMessageObject.duration;
    };

    that.displayMessage = function (message, type, duration) {
        if (that.isInit()) {
            that.type(type);
            if (duration == undefined || duration == null) {
                duration = CustomMessageObject.duration;
            }
            var messageShow = message == undefined || message == '' ? CustomMessageObject.message : message;
            displayMessage(messageShow, CustomMessageObject.type, CustomMessageObject.messageId, duration);
            CustomMessageObject.visible = true;
        }
        else {
            displayMessage(message, type, undefined, duration);
        }
    }

    that.displayMessages = function (messages, type, duration) {
        if (that.isInit()) {
            that.type(type);
            if (duration == undefined || duration == null) {
                duration = CustomMessageObject.duration;
            }
            var messagesShow = messages == undefined || messages.length == 0 ? CustomMessageObject.messages : messages;
            displayMessages(messagesShow, CustomMessageObject.type, CustomMessageObject.messageId, duration);
            CustomMessageObject.visible = true;
        }
        else {
            displayMessages(messages, type, undefined, duration);
        }
    }

    return that;
}());

$(document).ready(function () {
    $.ajaxSetup({ cache: false });
    $(document).ajaxComplete(function (event, xhr, settings) {
        var redirectLocation = xhr.getResponseHeader("REDIRECT_LOCATION");
        if (redirectLocation) {
            window.location.href = redirectLocation;
        }
    });
    handleAjaxMessages();
    //displayServerMessages();
    putMask();
    //criarValidadoresCustomizados();
    initDialog();
    //$('input').bind('paste', function (e) {
    //    e.preventDefault();
    //});
    //formatTableScroll();
    formSerialize();
    //detectChanges();
    configurarButtonsConfirmacao();

    $(".modal").modal({
        show: false,
        "backdrop": "static"
    });
});

// functions incluídas por AMM:

function ajaxHelper(uri, method, data, coverId, beforeSendFunc) {
    return $.ajax({
        type: method,
        url: uri,
        dataType: 'json',
        cache: false,
        contentType: "application/json; charset=utf-8",
        async: true,
        data: data ? JSON.stringify(data) : null,
        beforeSend: function (jqXHR, settings) {
            var result = true;
            $("html").addClass("wait");
            if (coverId != undefined) {
                showCover(coverId);
            }
            if (beforeSendFunc != undefined) {
                result = beforeSendFunc(jqXHR, settings);
            }
            return result;
        },
        error: function (jqXHR, textStatus, errorThrown) {
            debugger;
            switch (textStatus) {
                case 'timeout':
                    CustomMessage.displayMessage('Timeout ocorrued!', 'warning');
                    break;
                case 'error':
                case 'abort':
                case 'parsererror':
                    HandleAjaxError(jqXHR, errorThrown);
                    break;
                default:
                    HandleAjaxError(jqXHR, errorThrown);
                    break;
            }
        },
        complete: function (jqXHR, textStatus) {
            switch (textStatus) {
                case 'success':
                case 'notmodified':
                case 'nocontent':
                case 'error':
                case 'timeout':
                case 'abort':
                case 'parsererror':
                    $('html').removeClass("wait");
                    $('#' + coverId).fadeOut(100);
                    break;
                default:
                    $('html').removeClass("wait");
                    $('#' + coverId).fadeOut(100);
                    break;
            }

        },
    });
}

// parametros: um form e um jquery selector.
function formIgnore(form, ignore) {
    if (form != undefined && form != null) {
        var validator = form.data('validator');
        if (validator != null && validator != undefined)
            validator.settings.ignore = ignore;    // default is ":hidden".
    }
}

function setarObrigatoriedadeDocumento() {
    // INPUTS OBRIGATÓRIOS...
    $('input[type!="checkbox"]').each(function () {
        setarObrigatoriedade($(this));
    });

    // SELECTS OBRIGATÓRIOS...
    $('select').each(function () {
        setarObrigatoriedade($(this));
    });
}

function setarObrigatoriedade(elem) {
    var req = $(elem).attr('data-val-required');
    if (undefined != req) {
        var label = $('label[for="' + $(elem).attr('id') + '"]');
        var text = label.text();
        if (text.length > 0) {
            label.children('span.marca-campo-obrigatorio').remove();
            label.append('<span class="marca-campo-obrigatorio" style="color:red"> *</span>');
        }
    }
}

// Format
if (!String.prototype.format) {
    String.prototype.format = function () {
        var args = arguments;
        return this.replace(/{(\d+)}/g, function (match, number) {
            return typeof args[number] != 'undefined'
              ? args[number]
              : match
            ;
        });
    };
}

// Date Functions..

Date.isLeapYear = function (year) {
    return (((year % 4 === 0) && (year % 100 !== 0)) || (year % 400 === 0));
};

Date.getDaysInMonth = function (year, month) {
    return [31, (Date.isLeapYear(year) ? 29 : 28), 31, 30, 31, 30, 31, 31, 30, 31, 30, 31][month];
};

Date.prototype.isLeapYear = function () {
    var y = this.getFullYear();
    return (((y % 4 === 0) && (y % 100 !== 0)) || (y % 400 === 0));
};

Date.prototype.getDaysInMonth = function () {
    return Date.getDaysInMonth(this.getFullYear(), this.getMonth());
};

Date.prototype.addMonths = function (value) {
    var n = this.getDate();
    this.setDate(1);
    this.setMonth(this.getMonth() + value);
    this.setDate(Math.min(n, this.getDaysInMonth()));
    return this;
};

// Fim Date Functions...

// FIM FUNCTIONS AMM...

function formSerialize() {
    originalFormFields = [];
    $("form[detectChanges!=false]").each(function () {
        originalFormFields.push($(this).serialize());
    });
}

function formatTableScroll() {
    $(".tableScroll").chromatable({
        width: "100%",
        height: "300px",
        scrolling: "yes"
    });
}

/*Adicionar novos validadors*/
function criarValidadoresCustomizados() {
    $.extend($.validator.methods, {
        date: function (value, element) {
            return this.optional(element) || /^\d\d?\/\d\d?\/\d\d\d?\d?$/.test(value);
        },
        number: function (value, element) {
            return this.optional(element) || /^-?(?:\d+|\d{1,3}(?:\.\d{3})+)(?:,\d+)?$/.test(value);
        },
        range: function (value, element, param) {
            var val = value.replace(",", "#").replace(".", ",").replace("#", ".");
            return this.optional(element) || (val >= param[0] && val <= param[1]);
        }
    });
}

/*Detecta mudanças nos campos do formulário*/
//var originalFormFields = [];
//function detectChanges() {
//    function canGo(evt) {
//        var changed = false;
//        var destination = this;
//        var formsToDetect = $("form[detectChanges!=false]");

//        $(originalFormFields).each(function (index) {
//            if ($(formsToDetect[index]).serialize() != this) {
//                changed = true;
//                evt.stopImmediatePropagation();
//                return false;
//            }
//        });

//        // DESCOMENTADO POR AMM EM 28.10.2014...
//        var changed = formsToDetect.length && formsToDetect.serialize() != originalFormFields;
//        if (changed) {
//            var opts = {
//                buttons: {
//                    "Sim": function () {
//                        $(this).dialog("close");
//                        $(destination).unbind("click", canGo);
//                        destination.click();
//                        return false;
//                    },
//                    "Não": function () {
//                        $(this).dialog("close");
//                        return false;
//                    }
//                }
//            };

//            ShowDialog("Existem modificações que não foram salvas. <br/>Deseja realmente continuar?", opts);
//            evt.preventDefault();
//            return false;
//        }

//        return true;
//    }

//    $("a[data-ajax!=true][detectChanges!=false]").bind("click", canGo);
//    $("input[type=checkbox][detectChanges=true]").bind("click", canGo);
//}

/* Dialogs --------------------------------------------------------------*/

function initDialog() {
    $("#dialog").dialog({
        autoOpen: false,
        modal: true,
        width: "auto",
        close: function () {
            clearMessages();
        }
    });
}

function ShowDialog(contentToShow, dialogOptions) {

    if (contentToShow) {
        $("#conteudoDialog").html(contentToShow);
    }
    //restaurando options originais
    if ($("#dialog").data().origOptions) {
        $("#dialog").dialog($("#dialog").data().origOptions);
        $("#dialog").data().origOptions = null;
    }
    if (dialogOptions && typeof (dialogOptions) === "object") {
        $("#dialog").data({ origOptions: $("#dialog").dialog("option") });
        $("#dialog").dialog(dialogOptions);
    }
    var popupTitle = $("#conteudoDialog").find("span.popupTitle");
    popupTitle.hide();
    $("#dialog").dialog({ title: popupTitle.html() });

    $("#dialog").dialog("open");
    putMask();
    handleAjaxMessages();
}

/* Buttons com confirmação ---------------------------------------------------*/
function configurarButtonsConfirmacao() {
    $('input[type=submit][confirmation]').bind("click", function (e) {
        e.stopImmediatePropagation();
        e.preventDefault();
        var button = $(this);
        var action = button.attr('confirmation');

        $.get(action, null, function (data) {
            var opts = {
                buttons: {
                    "Sim": function () {
                        $(this).dialog("close");
                        button.unbind("click");
                        button.click();
                        configurarButtonsConfirmacao();
                        return false;
                    },
                    "Não": function () {
                        $(this).dialog("close");
                        return false;
                    }
                }
            };
            ShowDialog(data, opts);
        });
    });
}

/* Masks ----------------------------------------------------------------*/

function putMask() {
    putMaskButton();
    putMaskNumeric();
    putMaskDecimal();
    putMaskDate();
}

function putMaskButton() {
    $("input[type='submit'], input[type='reset'], input[type='button'], a.button").button();
}

function putMaskNumeric() {
    $("input[type*='numerico']").keypress(function (e) {
        var valor = String.fromCharCode(e.which);
        return $.isNumeric(valor) ||
            (e.keyCode == 46 || e.keyCode == 9 ||
            e.keyCode == 8 || e.keyCode == 37 || e.keyCode == 39);
    });
}

function putMaskDecimal(campo) {
    if (campo) {
        putFormat(campo);
    }
    else {
        $("input[type*='decimal']").each(function (i, e) {
            putFormat($(e));
            //$(this).attr('align', 'right');
        });
    }

    function putFormat(element) {
        var value = element.val().toString();
        if (value.indexOf('.') >= 0) {
            switch (value.length - value.indexOf('.')) {
                case 1:
                    element.val(value + '00');
                    break;
                case 2:
                    element.val(value + '0');
                    break;
            }
        }
        else {
            if (value.indexOf(',') < 0) {
                element.val(value + '.00');
            }
        }

        var limit = (isNaN(limit = parseInt($(element).attr("limit"))) ? 25 : limit);
        var scale = (isNaN(scale = parseInt($(element).attr("scale"))) ? 2 : scale);

        element.priceFormat({
            allowNegative: true,
            prefix: '',
            centsSeparator: ',',
            thousandsSeparator: '.',
            limit: limit,

            centsLimit: scale
        });
        element.css('text-align', 'right');
    }
}

function putMaskDate() {
    var virtualPath = $('#VirtualPath').html();

    $("input[type*='dataHora']").mask("99/99/9999 99:99:99");
    $("input[type*='competencia']").mask("99/9999");
    $("input[type*='data']")
        .mask("99/99/9999")
        .datepicker({
            //showOn: "button",
            //buttonImage: virtualPath + "Images/calendar.png",
            //buttonImageOnly: true,
            changeMonth: true,
            changeYear: true
        });

    jQuery(function ($) {
        $.datepicker.regional['pt-BR'] = {
            closeText: 'Fechar',
            prevText: '&#x3c;Anterior',
            nextText: 'Pr&oacute;ximo&#x3e;',
            currentText: 'Hoje',
            monthNames: ['Janeiro', 'Fevereiro', 'Mar&ccedil;o', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'],
            monthNamesShort: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'],
            dayNames: ['Domingo', 'Segunda-feira', 'Ter&ccedil;a-feira', 'Quarta-feira', 'Quinta-feira', 'Sexta-feira', 'S&aacute;bado'],
            dayNamesShort: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'S&aacute;b'],
            dayNamesMin: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'S&aacute;b'],
            weekHeader: 'Sm',
            dateFormat: 'dd/mm/yy',
            firstDay: 0,
            isRTL: false,
            showMonthAfterYear: false,
            yearSuffix: ''
        };
        $.datepicker.setDefaults($.datepicker.regional['pt-BR']);
    });
}
/* Mensagens --------------------------------------------------------------*/

function displayMessages(results, messageType, messageId, duration) {
    debugger;
    var message = "";
    if (results != undefined) {
        switch (Object.prototype.toString.call(results)) {
            case "[object Object]":
                if (results['ExceptionMessage'] != undefined) {
                    message += '<p>' + results['ExceptionMessage'] + '</p>';
                }
                if (results['Message'] != undefined) {
                    message += '<p>' + results['Message'] + '</p>';
                }
                if (results['MessageDetail'] != undefined) {
                    message += '<p>' + results['MessageDetail'] + '</p>';
                }
                break;
            case "[object Array]":
                for (var i = 0; i < results.length; i++) {
                    message += '<p>' + results[i] + '</p>';
                }
                break;
        }

    }
    else {
        message = errorMessageAdm;
    }
    displayMessage(message, messageType, messageId, duration);
}


///
// ******  Versão original, mas já com parâmetros para customizar as msg...
//
//function displayMessage(message, messageType, messageWrapper, messagesContainer, duration) {
//    if (messageWrapper == undefined) {
//        messageWrapper = $("#messagewrapper");
//    }
    
//    if (messagesContainer == undefined) {
//        messagesContainer = $("#messagesContainer");
//    }
//    var messageContainer = '<div class="' + messageType.toLowerCase() + '" ><span></span>';
//    messageContainer += message;
//    messageContainer += "</div>";

//    messageWrapper.html(messageContainer);

//    if ($("#dialog").dialog("isOpen")) {
//        messageWrapper.prependTo($("#dialog"));
//    } else {
//        messageWrapper.prependTo(messagesContainer);
//        }

//    showAndPrepareToHideMessages(messageWrapper, duration);
//}

function displayMessage(message, messageType, messageId, duration) {
    //CustomMessage.clear();
    if (messageId == undefined || messageId == null || messageId == '') {
        messageWrapper = $("#messagewrapper");
        messagesContainer = $("#messagesContainer");
    }
    else {
        messageWrapper = $("#messagewrapper" + messageId);
        messagesContainer = $("#messagesContainer" + messageId);
    }

    if (messageWrapper && messagesContainer) {
        var messageContainer = '<br /><div class="alert alert-' + messageType + '">';
        messageContainer += '<button type="button" class="close" data-dismiss="alert"> x </button>';
        messageContainer += message;
        messageContainer += "</div>";

        messageWrapper.html(messageContainer);

        if ($("#dialog").dialog("isOpen")) {
            messageWrapper.prependTo($("#dialog"));
        } else {
            messageWrapper.prependTo(messagesContainer);
        }

        //showAndPrepareToHideMessages(messageWrapper, duration);
        showAndPrepareToHideMessages(messagesContainer, duration);
    }
}

function displayServerMessages(duration) {
    showAndPrepareToHideMessages($("#messagesContainer"), duration);
}

function showAndPrepareToHideMessages(messageContainer, duration) {
    $('.cover').fadeOut(400);
    if (messageContainer.find('div.alert').length > 0) {
        if (duration != undefined && duration > 0) {
            messageContainer.show()
                .delay(duration)
                .hide(200);
        }
        else {
            messageContainer.show();
        }

        // Rever esse método...

        //$("form").each(function () {
        //    messageContainer.show();
        //    $(this).submit(function () {
        //        messageContainer.hide();
        //    });
        //});

        //if (messageContainer.parents('.box-content').length > 0) {
        //    scroll(messageContainer.parents('.box-content').first().offset().left, messageContainer.parents('.box-content').first().offset().top);
        //} else {
        //    if (messageContainer.parents('#content').length > 0) {
        //        scroll(messageContainer.parents('#content').first().offset().left, messageContainer.parents('#content').first().offset().top);
        //    }
        //    else {
        //        scroll(0, 0);
        //    }
        //}
    }
}

// parametros: um form e um jquery selector.
function formValid(form, ignore) {
    var isValid = false;
    if (form != undefined && form != null) {
        form.removeData("validator") /* added by the raw jquery.validate plugin */
            .removeData("unobtrusiveValidation");  /* added by the jquery unobtrusive plugin*/

        if (ignore == undefined)
            ignore = '';
        formIgnore(form, ignore);
        jQuery.validator.unobtrusive.parse(form);
        isValid = form.valid();
    }
    return isValid;
}

function clearDirty(nodeStartSelector) {
    $(nodeStartSelector).find('span.k-dirty').remove();
}

function clearForm(formId) {
    var controlSelector = '#{0} .form-control'.format(formId);
    $(controlSelector).val('');
    $('[type="radio"]').prop('checked', false);
    $('[type="checkbox"]').val(false);
    if ($(controlSelector).length > 0) {
        $(controlSelector).eq(0).focus();
    }
}

function clearMessages(partialId, tipoMsg, delay) {
    var messageWrapper;
    var messagesContainer;
    if (partialId == undefined) {
        messageWrapper = $("#messagewrapper");
        messagesContainer = $("#messagesContainer");
    }
    else {
        messageWrapper = $("#messagewrapper{0}".format(partialId));
        messagesContainer = $("#messagesContainer{0}".format(partialId));
    }

    var alertClass = 'alert-0';
    if (tipoMsg != undefined)   {
        alertClass = 'alert-{0}'.format(tipoMsg);
    }
     
    var alertDiv = $("#{0} > .{1}".format(messageWrapper.attr('id'), alertClass));
    if (messageWrapper != undefined && (tipoMsg == undefined || (alertDiv.length > 0))) {
        if (delay != undefined) {
            messagesContainer
                .delay(delay)
                .hide(400, function () {
                    messageWrapper.empty();
                });
        }
        else {
            messagesContainer.hide();
            messageWrapper.empty();
        }
    }
}

function apagarMensagensValidation() {
    $('.validation-summary-errors').each(function()   {
        $(this).addClass('validation-summary-valid');
        $(this).removeClass('validation-summary-errors');
    });

    $(".input-validation-error").each(function () {
        $(this).addClass("input-validation-valid").removeClass("input-validation-error");
    });

    $(".field-validation-error").each(function () {
        $(this).removeClass("field-validation-error").addClass("field-validation-valid");
    });
}


function handleAjaxMessages() {
    $(document).ajaxSuccess(function (event, request) {
        apagarMensagensValidation();
        debugger;
        var msgType = request.getResponseHeader('X-Message-Type');
        if (msgType != null && msgType.indexOf(',') > 1) {
            msgType = msgType.substring(0, msgType.indexOf(','));
        }
        switch (msgType) {
            case null:
            case 'success':
                if (isJson(request.statusText)) {
                    //var errors = JSON.parse(request.statusText);
                    //if (CustomMessage.isInit()) {
                    //    CustomMessage.displayMessages(errors.Errors, "success", CustomMessage.duration());
                    //}
                    //else if (event.currentTarget.location.pathname.indexOf(pathNameSystemFolder) == -1) {
                    //    displayMessages(errors.Errors, "success");
                    //}
                }
                else if (request.statusText !== '' && request.statusText.toUpperCase() !== 'OK' && request.status === 200) {
                    if (CustomMessage.isInit()) {
                        CustomMessage.displayMessage(request.statusText, "success", CustomMessage.duration());
                    }
                    else if (event.currentTarget.location.pathname.indexOf(pathNameSystemFolder) == -1) {
                        displayMessage(request.statusText, "success");
                    }
                }
                break;
            case 'warning':

                break;

            case 'danger':
                HandleAjaxError(request);
                break;

        }


        //else if (msgType == "success") {
        //    var redirectLocation = request.getResponseHeader("REDIRECT_LOCATION");
        //    if (!redirectLocation) {
        //        var chrome = /chrome/.test(navigator.userAgent.toLowerCase());
        //        if (chrome) {
        //            setTimeout(window.location.reload.bind(window.location), 250);
        //        } else {
        //            window.location.reload(true);
        //        }
        //    }
        //}
    }).ajaxError(function (event, request, settings, thrownError) {
        debugger;
        HandleAjaxError(request, thrownError);
    });
}

function HandleAjaxError(request, thrownError) {
    debugger;

    // Preferência pelo request...
    var msgJson = request.getResponseHeader('X-Error-Json');

    if (msgJson != null && isJson(msgJson)) {
        var errors = JSON.parse(msgJson);
        if (errors.length > 0) {
            if (typeof (errors) == "string") {
                CustomMessage.displayMessage(errors, 'danger');
            }
            else {
                CustomMessage.displayMessages(errors, 'danger');
            }
        }
    }
    else if (thrownError != undefined || request.responseText != undefined) {
        var result = thrownError != undefined ? thrownError : '';
        if (request.responseText != undefined && request.responseText.length > result.length) {
            result = request.responseText;
        }
        if (isJson(result)) {
            var errors = JSON.parse(result);
            CustomMessage.displayMessages(errors, "danger");
        }
        else if (result !== '' && request.status != 200) {
            var msg = result == '' ? errorMessageAdm : result;
            CustomMessage.displayMessage(msg, "danger");
        }
    }
}

function isJson(str) {
    try {
        JSON.parse(str);
    } catch (e) {
        return false;
    }
    return true;
}

//function getDate() {
//    var miliseconds = parseFloat(RegExp.ma long.Parse(Regex.Match(attemptedValue, JsonDatePattern, RegexOptions.IgnoreCase).Groups[1].Value);

//    DateTime epoc = new DateTime(1970, 1, 1);
//    return epoc.AddMilliseconds(miliseconds);
//}

function formatDecimal(val) {
    var result;
    if (val != null && val != undefined) {
        result = kendo.toString(val, 'n').replace('.', '');
    }
    else {
        result = '';
    }
    return result;
}

function formatDecimalFull(val) {
    var result;
    if (val != null && val != undefined) {
        result = kendo.toString(val, 'n');
    }
    else {
        result = '';
    }
    return result;
}

$.fn.hasAttr = function (nameAttr) {
    var attr = $(this).attr(nameAttr);
    return typeof attr !== typeof undefined && attr !== false;
};

////** Navegação do Web Api **////

function navigationResolver(ucId) {
    if (location.pathname.indexOf(pathNameSystemFolder) > 0) {
        openUseCase(ucId);
    }
    else {
        location.href = '/' + pathNameSystemFolder + '?ucId=' + ucId;
    }
}

//  - FIM -

//Globalize.culture("pt-BR");