////** Constantes e variáveis globais **////
const aTagTabStripSelector = 'a[role="tab"][data-toggle="tab"][href="{0}"]';
var Models = [];

const actions = {
    CLOSE_MODAL: 'closeModal',
    NEW: 'new',
    NOTHING: 'nothing'
}

function getViewModel(ucId) {
    debugger;
    var obj = $.grep(Models, function (e) { return e.UseCase === ucId });
    if (obj.length == 1) {
        return obj[0].ViewModel;
    }
    else {
        return null;
    }
}

function getNewModel(ucId) {
    var obj = $.grep(Models, function (e) { return e.UseCase === ucId });
    if (obj.length == 1) {
        return obj[0].NewModel;
    }
    else {
        return null;
    }
}

function removeViewModel(ucId) {
    var obj = $.grep(Models, function (e) { return e.UseCase === ucId });
    var index;
    for (var i = 0; i < obj.length; i++) {
        index = Models.indexOf(obj[i]);
        Models.splice(index, 1);
    }
}

function initUseCase(ucId) {
    debugger;
    var viewModel = getViewModel(ucId);
    viewModel.getNew(false);
    ko.applyBindings(viewModel, document.getElementById(viewModel.metadata().BoxContentId));
    $('#{0} .form-control'.format(viewModel.metadata().TabPanePesqDivId)).eq(0).focus();
}

//  - FIM -

////** Função ViewModel genérica... **////

var viewModelDefault = function (cadAttrModel, complementaryFunction, submitOnStartFunction) {
    var self = this;
    self.isMasterDetail = cadAttrModel.GridsNames != null && cadAttrModel.GridsNames != undefined && cadAttrModel.GridsNames.length > 0;
    self.metadata = ko.observable(cadAttrModel);
    self.entitySet = ko.observableArray();
    self.entity = ko.observable(null);
    self.entity.extend({ notify: 'always' });
    self.initialEntity = {};

    for (var i = 0; i < self.metadata().FilterObject.FieldValueSet.length; i++) {
       //self.metadata().FilterObject.FieldValueSet[i].FieldValue = ko.observable();
    }

    self.pesqMessage = new CustomMessageApp(self.metadata().PesqMessageId);

    self.gridMessage = new CustomMessageApp(self.metadata().GridMessageId);

    self.cadMessage = new CustomMessageApp(self.metadata().CadMessageId);

    self.getNew = function (showTab) {
        var newEntity = {};
        debugger;
        if (self.custom != null && self.custom['loadFkOptions'] != undefined) {
            self.custom.loadFkOptions(self);
        }
        $.extend(true, newEntity, getNewModel(self.metadata().UseCaseId));
        if (self.custom != null && self.custom['init'] != undefined) {
            self.custom.init(newEntity);
        }

        // tirar isso daqui...
        $(aTagTabStripSelector.format('\\\\#' + self.metadata().TabPaneCadDivId) + '>span').html('Insert new');

        if (self.isMasterDetail) {
            loadGridsFromViewModel(self.metadata(), newEntity);
        }
        self.entity(newEntity);

        if (showTab) {
            activeEditTabStrip(self.metadata(), false);
        }
    }

    self.loadFksOptionsEnd = function () {
        clearDirty('div#{0}'.format(self.metadata().TabPaneCadDivId));
    }

    self.getAll = function () {
        ajaxHelper(self.metadata().ActionGetAll, 'GET').done(function (data) {
            self.entitySet(data);
        });
    }

    self.getOne = function (key) {
        if (self.entity() && key == self.entity().Key) {
            activeEditTabStrip(self.metadata(), true);
        }
        else {
            notificarGetOneStart(self.metadata());
            if (self.entity() && self.custom != null && self.custom['formClear'] != undefined) {
                self.custom.formClear(self.entity());
            }
            ajaxHelper(self.metadata().ActionGetOne + key, 'GET').done(function (data) {
                //self.entity = ko.mapping.fromJS(data);
                $.extend(true, self.initialEntity, data);
                if (self.isMasterDetail) {
                    loadGridsFromViewModel(self.metadata(), data);
                }

                self.entity(data);
                if (self.custom != null && self.custom['getOneEnd'] != undefined) {
                    self.custom.getOneEnd(data);
                }

                notificarGetOneFinish(self.metadata(), 'success', self.entity()[self.metadata().DefaultDescriptiveField]);
            });
        }
    }

    self.getFiltro = function () {
        for (var i = 0; i < self.metadata().FilterObject.FieldValueSet.length; i++) {
            var filtroAtual = self.metadata().FilterObject.FieldValueSet[i].FieldTipoDeFiltro;
            switch(filtroAtual){
                case "Igual":
                    filtroAtual = 1;
                    break;
                case "Diferente":
                    filtroAtual = 2;
                    break;
                case "Maior":
                    filtroAtual = 3;
                    break;
                case "Menor":
                    filtroAtual = 4;
                    break;
                case "Maior ou igual":
                    filtroAtual = 5;
                    break;
                case "Menor ou igual":
                    filtroAtual = 6;
                    break;
                case "Iniciando por":
                    filtroAtual = 7;
                    break;
                case "Contendo":
                    filtroAtual = 8;
                    break;
                case "Finalizando por":
                    filtroAtual = 9;
                    break;
                case "Nao contendo":
                    filtroAtual = 10;
                    break;
                default:
                    if (!(filtroAtual > 0 && filtroAtual < 11)) {
                        filtroAtual = 8;
                    }
                    break;

            }
            self.metadata().FilterObject.FieldValueSet[i].FieldTipoDeFiltro = filtroAtual;

        }
        //self.metadata().FilterObject.FieldValueSet[0].FieldTipoDeFiltro = 8;
        //self.metadata().FilterObject.FieldValueSet[1].FieldTipoDeFiltro = 8;

        
        return self.metadata().FilterObject;
    }

    self.pesquisar = function (item) {
        notificarPesqStart(self);
        pesquisarCad(self.metadata(), self.pesqMessage);
    }

    self.pesquisarFinish = function (data, msg, tipoAlerta) {
        debugger;
        if (msg == '' && tipoAlerta == 'success') {
            self.entitySet(data);
        }
        else if ($('#{0}:visible'.format(self.metadata().GridMessageId)).length === 1) {
            self.gridMessage.displayMessage(msg, tipoAlerta, -1);
        }
        else {
            self.pesqMessage.displayMessage(msg, tipoAlerta, -1);
        }
        notificarPesqFinish(self.metadata(), tipoAlerta);
    }

    self.clearFilter =  function (item) {
        // Para funcionar self.metadata().FilterObject.FieldValueSet deve ser observable...
        //for (var i = 0; i < self.metadata().FilterObject.FieldValueSet.length; i++) {
        //    self.metadata().FilterObject.FieldValueSet[i].FieldValue(null);
        //}


        clearDirty('#{0}'.format(self.metadata().TabPanePesqDivId));

        self.pesqMessage.clear();
        self.gridMessage.clear();
        $('#' + self.metadata().FilterObject.PesqResultDivId).hide();

        clearForm(self.metadata().FilterObject.PesqFormName);
    }

    self.save = function (data, event) {
        debugger;
        try {
            notificarSaveStart(self.metadata());
            var result = true;
            if (self.custom != undefined && self.custom['submitOnStart'] != undefined) {
                result = self.custom.submitOnStart();
            }
            result = formValid($('#' + self.metadata().FormName)) && result;
            if (result) {
                if (self.isMasterDetail) {
                    var kendoGrid;
                    var gridData;
                    for (var i = 0; i < self.metadata().GridsNames.length; i++) {
                        kendoGrid = $('#{0}'.format(self.metadata().GridsNames[i])).data('kendoGrid');
                        gridData = kendoGrid.dataSource.data();
                        if (typeof gridData == 'object') {
                            self.entity()[self.metadata().GridsNames[i]] = [];
                        }
                        else {
                            self.entity()[self.metadata().GridsNames[i]] = gridData;
                        }
                    }
                }

                if (self.entity().IsNew) {
                    ajaxHelper(self.metadata().ActionPost, 'POST', self.entity()).done(function (data) {
                        if (data) {
                            switch (self.custom.actionAfterPost) {
                                case actions.CLOSE_MODAL:
                                    self.closeModalDialog();
                                    break;
                                case actions.NOTHING:
                                default:
                                    if (self.isMasterDetail) {
                                        for (var i = 0; i < self.metadata().GridsNames.length; i++) {
                                            clearDirtyCells(self.metadata().GridsNames[i]);
                                        }
                                    }
                                    clearDirty('#{0}'.format(self.metadata().BodyCadId));
                                    self.cadMessage.displayMessage(data, 'success');
                                    alertCadInit(self.metadata(), 'success');
                                    notificarPesqStart(self);
                                    refreshKendoGrid($('#' + self.metadata().FilterObject.GridPesqName));
                                    break;
                            }
                        }
                    }).always(function (data, textStatus, jqXHR) {
                        notificarSaveFinish(self.metadata(), 'success');
                    }).always(function (jqXHR, textStatus, errorThrown) {
                        notificarSaveFinish(self.metadata(), 'danger');
                    });
                }
                else {
                    ajaxHelper(self.metadata().ActionPut, 'PUT', self.entity()).done(function (data, textStatus, jqXHR) {
                        if (data) {
                            switch (self.custom.actionAfterPut) {
                                case actions.CLOSE_MODAL:
                                    self.closeModalDialog();
                                    break;
                                case actions.NEW:
                                default:
                                    refreshKendoGrid($('#' + self.metadata().FilterObject.GridPesqName));
                                    self.getNew(true);
                                    self.cadMessage.displayMessage(data, 'success');
                                    break;
                            }
                        }
                        if (textStatus == 'success') {
                            notificarSaveFinish(self.metadata(), 'success');
                        }
                        else {
                            notificarSaveFinish(self.metadata(), 'danger');
                        }
                    });
                }
            }
            else {
                $('#{0}'.format(cadAttrModel.AmpUseCaseId)).fadeOut(120);
                $('#{0}'.format(cadAttrModel.AmpulhetaCadId)).fadeOut(120);
                self.cadMessage.displayMessage('The operation cannot be completed. Check the reasons in the corresponding fields, correct them and try again.', 'warning');
            }

        } catch (exc) {
            $('#{0}'.format(cadAttrModel.AmpUseCaseId)).fadeOut(120);
            $('#{0}'.format(cadAttrModel.AmpulhetaCadId)).fadeOut(120);
            self.cadMessage.displayMessage('A error ocorred: ' + exc.message, 'danger');
            return false;
        }
        finally {
            event.preventDefault();
            event.stopPropagation();
        }
    }

    self.closeModalDialog = function () {
        let boxContent = $('#' + self.metadata().BoxContentId);
        let modalDialog = boxContent.parents('.modal.fade');
        if (modalDialog) {
            modalDialog.modal('toggle');;
        }
    }

    self.cancel = function (data, event) {
        if (self.entity() != null && self.entity().IsNew) {
            self.getNew(true);
        }
        else {
            var newData = {};
            $.extend(true, newData, self.initialEntity);
            //cancelChangesGrid(self);
            if (self.isMasterDetail) {
                loadGridsFromViewModel(self.metadata(), newData);
            }
            self.entity(newData);
            clearDirty('div#{0}'.format(self.metadata().TabPaneCadDivId));
        }
        event.preventDefault();
        event.stopPropagation();
    }

    self.delete = function (id, desc, uid) {
        var index;
        debugger;
        var entityToDel = $.grep(self.entitySet(), function (e, i) {
            var result = e.Key == id;
            if (result) {
                index = i;
            }
            return result;
        });
        if (entityToDel.length == 1) {
            bootbox.confirm("Confirmar exclusão:\n " + self.metadata().Title + " '" + desc + "'?", function (result) {
                if (result) {
                    notificarGridOprStart(self.metadata());
                    ajaxHelper(self.metadata().ActionDelete + id, 'DELETE').done(function (data) {
                        debugger;
                        if (self.entity() != null && self.entity().Key == id) {
                            self.getNew(false);
                        }
                        self.entitySet().splice(index, 1);
                        var grid = $("#" + self.metadata().FilterObject.GridPesqName).data("kendoGrid");
                        grid.removeRow("tr[data-uid='" + uid + "']");
                        notificarGridOprFinish(self.metadata());
                    });
                }
            });
        }
        else {
            self.gridMessage.displayMessage("{0} '{1}' não encontrado(a).".format(self.metadata().Title, desc), 'danger');
        }




    }

    self.validOp = function (id, tipo, nome) {
        
        $menu = $('.context-menu-list.'+id+'-'+nome+'-title');
        if ($menu.length != 1) {            
            validOperations(id, nome, tipo);            
        } else if ($menu.hasClass('context-menu-disabled')) {
            validOperations(id, nome, tipo, $menu);
        }
    }

    debugger;
    if (complementaryFunction != undefined) {
        self.custom = new complementaryFunction(self);
    }
    else {
        self.custom = null;
    }

}

//var EntityViewModel = function (metadata, data) {

//}

//  - FIM -

// Alertas e notificações...
function piscar(elem, frequency) {
    if (elem.is('.alert-on')) {
        elem.fadeToggle(frequency, function () {
            piscar(elem, frequency);
        });
    }
    else {
        elem.fadeOut();
    }
}

function alertInit(selector, nodeStart, frequency, deep) {
    var alerts;
    if (deep) {
        alerts = $(nodeStart).find(selector);
    }
    else {
        alerts = $(nodeStart).children(selector);
    }

    alerts.each(function () {
        $(this).addClass('alert-on');
        piscar($(this), frequency);
    });
}

function alertEnd(selector, nodeStart, deep) {
    if (deep) {
        $(nodeStart).find(selector).removeClass('alert-on');
        $(nodeStart).find(selector).fadeOut();
    }
    else {
        $(nodeStart).children(selector).removeClass('alert-on');
        $(nodeStart).children(selector).fadeOut();
    }
}

function alertCadInit(cadAttrModel, tipo) {
    if (!$('#{0}'.format(cadAttrModel.LiCadId)).hasClass('active')) {
        alertInit('.alert-{0}'.format(tipo), aTagTabStripSelector.format('\\\\#' + cadAttrModel.TabPaneCadDivId), 'slow', false);
    }
    if (!$('#{0}'.format(cadAttrModel.LiUseCaseId)).hasClass('active')) {
        alertInit('.alert-{0}'.format(tipo), aTagTabStripSelector.format('\\\\#' + cadAttrModel.TabPaneDivId), 'slow', false);
    }
}

function alertPesqInit(cadAttrModel, tipo) {
    if (!$('#{0}'.format(cadAttrModel.LiPesqId)).hasClass('active')) {
        alertInit('.alert-{0}'.format(tipo), aTagTabStripSelector.format('\\\\#' + cadAttrModel.TabPanePesqDivId), 'slow', false);
    }
    if (!$('#{0}'.format(cadAttrModel.LiUseCaseId)).hasClass('active')) {
        alertInit('.alert-{0}'.format(tipo), aTagTabStripSelector.format('\\\\#' + cadAttrModel.TabPaneDivId), 'slow', false);
    }
}

function notificarPesqStart(viewModel) {
    $('#{0}'.format(viewModel.metadata().AmpUseCaseId)).fadeIn(400);
    $('#{0}'.format(viewModel.metadata().FilterObject.AmpulhetaPesqId)).fadeIn(400);

    viewModel.pesqMessage.clear();
    viewModel.gridMessage.clear();
}

function notificarPesqFinish(cadAttrModel, tipo) {
    $('#{0}'.format(cadAttrModel.FilterObject.AmpulhetaPesqId)).fadeOut(400);
    $('#{0}'.format(cadAttrModel.AmpUseCaseId)).fadeOut(400);
    
    clearDirty('#{0}'.format(cadAttrModel.FilterObject.PesqFormName));

    if (!$('#{0}'.format(cadAttrModel.LiPesqId)).hasClass('active')) {
        alertInit('.alert-{0}'.format(tipo), aTagTabStripSelector.format('\\\\#' + cadAttrModel.TabPanePesqDivId), 'slow', false);
    }
    if (!$('#{0}'.format(cadAttrModel.LiUseCaseId)).hasClass('active')) {
        alertInit('.alert-{0}'.format(tipo), aTagTabStripSelector.format('\\\\#' + cadAttrModel.TabPaneDivId), 'slow', false);
    }
}

function notificarSaveStart(cadAttrModel) {
    showCover(cadAttrModel.AmpulhetaCadId, 'Saving datas...')
    $('#{0}'.format(cadAttrModel.AmpulhetaCadId)).fadeIn(400);

    clearMessages(cadAttrModel.CadMessageId);
}

function notificarSaveFinish(cadAttrModel, tipo) {
    $('#{0}'.format(cadAttrModel.AmpUseCaseId)).fadeOut(400);
    $('#{0}'.format(cadAttrModel.AmpulhetaCadId)).fadeOut(400);

    alertCadInit(cadAttrModel, tipo);
}

function notificarGetOneStart(cadAttrModel) {
    $('#{0}'.format(cadAttrModel.AmpUseCaseId)).fadeIn(400);
    $('#{0}'.format(cadAttrModel.AmpulhetaCadId)).fadeIn(400);
}

function notificarGetOneFinish(cadAttrModel, tipo, tabStripLabel) {
    $('#{0}'.format(cadAttrModel.AmpUseCaseId)).fadeOut(400);
    $('#{0}'.format(cadAttrModel.AmpulhetaCadId)).fadeOut(400);
    $(aTagTabStripSelector.format('\\\\#' + cadAttrModel.TabPaneCadDivId) + '>span').html('Edit -> {0}'.format(tabStripLabel));
    clearDirty('div#{0}'.format(cadAttrModel.TabPaneCadDivId));
    activeEditTabStrip(cadAttrModel, true);
}

function notificarGridOprStart(cadAttrModel) {
    $('#{0}'.format(cadAttrModel.AmpUseCaseId)).fadeIn(400);
    $('#{0}'.format(cadAttrModel.FilterObject.AmpulhetaPesqId)).fadeIn(400);
    clearMessages(cadAttrModel.PesqMessageId);
    clearMessages(cadAttrModel.GridMessageId);
}

function notificarGridOprFinish(cadAttrModel) {
    $('#{0}'.format(cadAttrModel.AmpUseCaseId)).fadeOut(400);
    $('#{0}'.format(cadAttrModel.FilterObject.AmpulhetaPesqId)).fadeOut(400);
}

// - Fim Alertas e notificações -

function loadGridsFromViewModel(metadata, entity) {
    var grid;
    //var dataSource;
    var dataGrid;
    var data;
    debugger;
    for (var i = 0; i < metadata.GridsNames.length; i++) {
        clearGrid(metadata.GridsNames[i]);
        grid = $('#{0}'.format(metadata.GridsNames[i])).data("kendoGrid");
        if (grid) {
            dataGrid = grid.dataSource.data();
            data = entity[metadata.GridsNames[i]];
            for (var j = 0; j < data.length; j++) {
                dataGrid.push(data[j]);
            }
        }

        //dataSource = new kendo.data.DataSource({
        //    data: entity[metadata.GridsNames[i]]
        //});
        //jGrid.kendoGrid({ dataSource: dataSource, editable: true });
        //jGrid.data("kendoGrid").refresh();
    }
}

function cancelChangesGrid(viewModel) {
    var jGrid;
    var dataSource;
    debugger;
    for (var i = 0; i < viewModel.metadata().GridsNames.length; i++) {
        jGrid = $('#{0}'.format(viewModel.metadata().GridsNames[i]));
        jGrid.data("kendoGrid").cancelChanges();
    }
}

////** Navegação, rotas e manipulação das Tabstrips dos casos de uso **////

function openUseCase(ucId) {
    debugger;
    var useCaseUrl = pathNameSystemFolder + '/GetUseCaseContentIni?ucId={0}'.format(ucId);
    var tabPaneUseCaseSelector = "tab-pane-{0}".format(ucId);
    var tabMenuId = 'systemFolderTab';
    var useCaseTabMenuHtml = '<li id="li-uc-{3}"><a href="{0}" role="tab" data-toggle="tab"><span>{1}</span>{2}<i class="fa fa-times" onclick="javascript: closeUseCase(event, this, {3});" title="Close {1}"></i></a></li>';
    var useCaseTabDivContentHtml = '<div class="tab-pane" id="{0}">';

    var alertHtml = "<i style='display:none;' class='fa fa-check alert-{0}'></i>";
    var alerts = alertHtml.format('danger');
    alerts += alertHtml.format('warning');
    alerts += alertHtml.format('success');

    var ul = $('#' + tabMenuId + '.tab-menu');
    var a = ul.find('a[href="\\\\#{0}"]'.format(tabPaneUseCaseSelector)).first();

    if (a.length > 0) {
        a.tab('show');
    }
    else {
        var li = $(useCaseTabMenuHtml.format('\\#' + tabPaneUseCaseSelector, '', alerts, ucId));
        ul.append(li);

        var divTabContent = ul.next('div.tab-content');
        var divTabPane = $(useCaseTabDivContentHtml.format(tabPaneUseCaseSelector));

        divTabContent.append(divTabPane);
        divTabPane.load(useCaseUrl, function (response, status) {
            if (status == 'success') {
                initUseCase(ucId);
                li.children('a').tab('show');
            }
            else {
                var viewModel = getViewModel(ucId);
                viewModel.pesqMessage.displayMessage(errorMessageAdm, "danger");
            }
        });
    }
}

function closeUseCase(e, iElement, useCaseId) {
    var $elem = $(iElement);
    var ul = $elem.closest('ul');
    var cadAttrModel = getViewModel(useCaseId).metadata();
    var node = $('#{0}'.format(cadAttrModel.BoxContentId));
    if (ul.children('li').length == 1) {
        location.href = '/Home';
    }
    else {
        var li = $elem.closest('li').first();
        if (li.hasClass('active')) {
            if (li.next('li').length == 1) {
                li.next('li').first().children('a').first().tab('show');
            }
            else {
                li.prev('li').first().children('a').first().tab('show');
            }

        }
        ko.cleanNode(node);
        li.remove();
        $tr = $("ul[class *= '" + useCaseId + "-']");
        $tr.contextMenu(false);
        ul.next('div.tab-content').children('.tab-pane#' + cadAttrModel.TabPaneDivId).remove();
    }

    removeViewModel(useCaseId);
    e.preventDefault();
}

function activeEditTabStrip(cadAttrModel, isEdit) {
    var aTag;
    aTag = $(aTagTabStripSelector.format('\\\\#' + cadAttrModel.TabPaneDivId));
    if (aTag) {
        aTag.tab('show');
    }
    aTag = $(aTagTabStripSelector.format('\\\\#' + cadAttrModel.TabPaneCadDivId));
    if (aTag) {
        aTag.tab('show');
        $('#{0}'.format(cadAttrModel.FormName)).find('.form-control').eq(0).focus();
    }
}

function getOne(useCaseId, key) {
    debugger;
    getViewModel(useCaseId).getOne(key);
}

function deleteOne(useCaseId, id, desc, uid) {
    debugger;
    getViewModel(useCaseId).delete(id, desc, uid);
}

function validOperations(id,nome,tipo,menu) {
    var icon = $('#' + id + '-' + nome + '-filter-icon').find(">:first-child");
    var options = [];

    if (tipo === "int16" || tipo === "int" || tipo === "single" || tipo === "double" || tipo === "decimal" || tipo === "datetime" || tipo === "byte" || tipo === "string") {
        options.push("Igual");
        options.push("Diferente");
        options.push("Maior");
        options.push("Menor");
        options.push("Maior ou igual");
        options.push("Menor ou igual");
        icon.attr('src', 'Images/icons/Igual.png');
        icon.attr('title', 'Igual');
    }

    if (tipo === "string") {
        options.push("Iniciando por");
        options.push("Finalizando por");
        options.push("Contendo");
        options.push("Nao contendo");
        icon.attr('src', 'Images/icons/Contendo.png');
        icon.attr('title', 'Contendo');
    }

    var item = {};
    var key;
    for (var i = 0; i < options.length; i++) {
        key = "op{0}".format(i);
        item[key] = { name: options[i], icon: "dif" };

    }
  
    options.forEach(function (item2) {
        $('select[name=' + id + '-' + nome + ']').append('<option>' + item2+'</option>');
    })
    
    if (menu != undefined) {
        menu.contextMenu(true);
    } else {
        $.contextMenu({
            selector: '#' + id + '-' + nome + '-filter-icon',
            className: id + '-' + nome + '-title',
            trigger: 'left',
            callback: function (key, options) {                
                atualizaContextMenu(id, nome);
            },
            items: item
        });

    }

    inicializaContextMenu(id, nome);
}

function inicializaContextMenu(id, nome) {
    var lista = $('.context-menu-list.' + id + '-' + nome + '-title > li.context-menu-item');
    lista.each(function () {
        var elem = $(this);
        var texto = elem.text();
        var initIco = $('#' + id + '-' + nome + '-filter-icon').find(">:first-child").attr('src');
        if ('Images/icons/' + texto + '.png'=== initIco){
            if (elem.children().length > 1) {
                elem.find('img').attr('src', 'Images/icons/check.png');
            } else {
                elem.prepend("<img style = 'padding-right:4%;' src='Images/icons/check.png' />");
            }            
        } else {
            if (elem.children().length > 1) {
                elem.find('img').attr('src', 'Images/icons/' + texto + '.png');
            } else {
                elem.prepend("<img style = 'padding-right:4%;' src='Images/icons/" + texto + ".png' />");
            }
        }

    })
    
}

function atualizaContextMenu(id, nome) {
    var itemAtual = $('.context-menu-list.' + id + '-' + nome + '-title > li.context-menu-item.context-menu-hover');
    var textoAtual = itemAtual.text();
    var lista = $('.context-menu-list.' + id + '-' + nome + '-title > li.context-menu-item');
    var icon = $('#' + id + '-' + nome + '-filter-icon').find(">:first-child");

    //recarrega icones da lista
    lista.each(function () {
        var texto = $(this).text();
        $(this).find(">:first-child").attr('src', 'Images/icons/' + texto + '.png');             
    })
    itemAtual.find(">:first-child").attr('src', 'Images/icons/check.png');
    //atualia o icone ao lado do input

    icon.attr('src', 'Images/icons/' + textoAtual + '.png');
    icon.attr('title', textoAtual);

    

    $('select[name=' + id + '-' + nome + ']').val(textoAtual).change();
}

//  - FIM -

// Message

var CustomMessageApp = function (msgId, msgType, msgDuration, msgDefaultText) {

    var self = this;
    self.visible = false;
    if (msgId != undefined && $('#' + msgId) != undefined) {
        self.messageId = msgId;
        self.messageWrapper = $("#messagewrapper" + msgId);
        self.messagesContainer = $("#messagesContainer" + msgId);
    }
    else {
        self.messageId = '';
        self.messageWrapper = $("#messagewrapper");
        self.messagesContainer = $("#messagesContainer");
    }

    if (msgType != undefined) {
        switch (msgType) {
            case 'danger':
            case 'success':
            case 'warning':
                self.type = msgType;
                break;
            default:
                self.type = 'warning';
                break;
        }
    }

    if (msgDuration != undefined) {
        self.duration = msgDuration;
    }
    else {
        self.duration = -1;
    }

    if (msgDefaultText != undefined) {
        self.message = msgDefaultText;
    }
    
    self.displayMessage = function (msg, msgType, msgDuration) {
        if (msg != undefined) {
            self.message = msg;
        }

        if (msgType != undefined) {
            switch (msgType) {
                case 'danger':
                case 'success':
                case 'warning':
                    self.type = msgType;
                    break;
                default:
                    self.type = 'warning';
                    break;
            }
        }

        if (msgDuration != undefined) {
            self.duration = msgDuration;
        }
        else {
            self.duration = -1;
        }

        displayMessage(self.message, self.type, self.messageId, self.duration);
        self.visible = true;
    }

    self.displayMessages = function (msgs, msgtype, msgduration) {
        if (msgType != undefined) {
            switch (msgType) {
                case 'danger':
                case 'success':
                case 'warning':
                    self.type = msgType;
                    break;
                default:
                    self.type = 'warning';
                    break;
            }
        }

        if (msgDuration != undefined) {
            self.duration = msgDuration;
        }
        else {
            self.duration = -1;
        }

        if (msgs != undefined && msgs.length > 0) {
            displayMessages(msgs, self.type, self.messageId, self.duration);
        }
        self.visible = true;
    }

    self.clear = function () {
        self.messagesContainer.hide();
        self.messageWrapper.empty();
    };

    self.isVisible = function () {
        return false;
    }

    self.reset = function () {
        self.clear();
    };
}

