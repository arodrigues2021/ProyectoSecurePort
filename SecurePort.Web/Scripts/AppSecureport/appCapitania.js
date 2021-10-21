function AltaEditarCapitanias(action) {
	var CapitaniaJson = {
		"Id": $('input#ID01').val(),
		"Nombre": $('input#Nombre01').val(),
		"Activo": $('input#Activo01').val(),
		"Action":action
	};
	var o = eval(CapitaniaJson);
	if (o.Id && o.Nombre && $('input#ValidarAllCombos').val()) {
		$.ajax({
			url: '/Capitanias/AltaEditarCapitanias',
			type: "POST",
			data: JSON.stringify({ CapitaniaJson: CapitaniaJson }),
			dataType: "json",
			contentType: "application/json; charset=utf-8",
			success: function (data) {
				if (data.result) {
					$('#myModalCapitanias').modal('hide');
					window.location.reload();
				} else {
					$('#myModalCapitanias').modal('hide');
					showLoading();
					alert(data.Message);
				}
			}
		});
	}
}

function VolverCapitania() {

	$('input#Nombre01').val('');
	$('div#myModalCapitanias.modal.fade.in').attr('style', 'display:none');
	hideLoading();
	$('#DatosCapitanias').html('');
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

function ValidarAllCombos() {
    var comboValida = true;

    if (!ValidacionCombos(document.getElementById('Activo01'), 'errorActivo'))
        comboValida = false;

    return comboValida;
}
