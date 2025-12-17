function indexRowDetail(gridName, dataItem) {
	return getIndexRowKendoGrid(gridName, dataItem);
}

function editGridDetail(e) {
    debugger;
	e.model.IsNew = e.model.isNew();
	CustomMessage.clear();
}

function dataSourceError(e) {
	CustomMessage.displayMessages(e.errors, 'warning', undefined, undefined, delayMessage);
}

function excluirDetailGo(gridName, id, descricaoVal, uid)    {
	var grid = $("#" + gridName).data("kendoGrid");
	grid.removeRow("tr[data-uid='" + uid + "']");
	if (typeof excluirDetailOnComplete === 'function') {
		excluirDetailOnComplete(gridName, id, descricaoVal, uid);
	}
}

function excluirDetail(gridName, id, descricaoVal, uid, showConfirm) {
	if (showConfirm === false)   {
		excluirDetailGo(gridName, id, descricaoVal, uid);
	}
	else {
		bootbox.confirm("Confirm exclusion:\n '" + descricaoVal + "'?", function (result) {
			if (result) {
				excluirDetailGo(gridName, id, descricaoVal, uid);
			}
		});
	}
}

