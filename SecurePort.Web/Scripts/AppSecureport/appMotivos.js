function AltaEditarMotivos(action) {
	var MotivosJson = {
		"Motivo": $('input#Nombre01').val(),
		"Descripcion": $('input#Descripcion01').val(),
		"Id": $('input#Id01').val(),
		"action": action,
		"activo": $('input#Activo01').val()
	};
	var o = eval(MotivosJson);
	if (o.Motivo && $('input#ValidarAllCombos').val()) {
		$.ajax({
			url: '/Motivos/AltaEditarMotivos',
			type: "POST",
			data: JSON.stringify({ MotivosJson: MotivosJson }),
			dataType: "json",
			contentType: "application/json; charset=utf-8",
			success: function (data) {
				if (data.result == 0) {
					$('#myModalMotivos').modal('hide');
					window.location.reload();
				} else if (data.result == 1) {
					$('#myModalMotivos').modal('hide');
					showLoading();
					alert(data.Message, 'Motivos');
				} else if (data.result == 2) {
					$('#myModalMotivos').modal('hide');
					showLoading();
					alert(data.Message, 'Confirm', 0 , 50);
				} else {
					$('#myModalMotivos').modal('hide');
					showLoading();
					alert(data.Message, 'Confirm', 0, 51);
				}
			}

		});
	}
}

function VolverMotivos() {
	$('input#Nombre01').val('');
	$('input#Descripcion01').val('');
	$('div#myModalMotivos.modal.fade.in').attr('style', 'display:none');
	hideLoading();
	$('#DatosMotivos').html('');
	window.location.reload();
}

function ValidacionCombos(datos, campoerror) {
    if (isNaN(datos.value)) {
        $('span#' + campoerror).attr('style', 'display:block;color:#b94a48;');
        return false;
    }
    else {
        var combo = $('#' + datos.id).data("kendoComboBox");
        var existe = false;
        if (datos.value != "") {
            for (var obj in combo.dataSource._pristineData) {
                if (combo.dataSource._pristineData[obj].Value == datos.value)
                    existe = true;
            }
            if (existe) {
                $('span#' + campoerror).attr('style', 'display:none;');
                //combo.select(parseInt(datos.value));
                return true;
            }
            else {
                $('span#' + campoerror).attr('style', 'display:block;color:#b94a48;');
                return false;
            }
        }
        else {
            $('span#' + campoerror).attr('style', 'display:none;');
            return true;
        }
    }
}

function ValidarAllCombos(estado) {
    var comboValida = true;

    if (!ValidacionCombos(document.getElementById('Activo01'), 'errorActivo'))
            comboValida = false;

    return comboValida;
}


