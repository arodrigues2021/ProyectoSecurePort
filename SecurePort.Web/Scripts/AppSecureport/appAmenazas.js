var mensaje = "Datos guardados correctamente";
function AltaEditarAmenazas(action) {
	var AmenazasJson = {
		"Descripcion": $('input#Nombre01').val(),
		"Id": $('input#Id01').val(),
		"action": action,
		"activo": $('input#Activo01').val()
	};
	var o = eval(AmenazasJson);
	if (o.Descripcion && $('input#ValidarAllCombos').val()) {
		$.ajax({
			url: '/Amenazas/AltaEditarAmenazas',
			type: "POST",
			data: JSON.stringify({ AmenazasJson: AmenazasJson }),
			dataType: "json",
			contentType: "application/json; charset=utf-8",
			success: function (data) {
				if (data.result == 0) {
					$('#myModalAmenazas').modal('hide');
					window.location.reload();
				} else if (data.result == 1) {
				    $('#myModalAmenazas').modal('hide');
				    showLoading();
				    alert(data.Message, 'Amenazas');
				} else if (data.result == 2) {
				    $('#myModalAmenazas').modal('hide');
				    showLoading();
				    alert(data.Message, 'Confirm', 0 , 28);
				} else {
				    $('#myModalAmenazas').modal('hide');
				    showLoading();
				    alert(data.Message, 'Confirm', 0, 30);
				}
			}

		});
	}
}

function VolverAmenazas() {
	$('input#Nombre01').val('');
	$('div#myModalAmenazas.modal.fade.in').attr('style', 'display:none');
	hideLoading();
	$('#DatosAmenazas').html('');
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

