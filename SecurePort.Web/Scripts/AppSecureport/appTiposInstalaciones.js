var mensaje = "Datos guardados correctamente";
//$(function () {
   
//	$('#confirm').attr('style', 'display:none');

//	if (history.forward(1)) {
//		location.replace(history.forward(1));
//	}
	
//	$('#IdGestionUsuarios').attr('style', 'display:none');

//	$('#IdSubMenuMaestros').attr('style', 'display:block');

//	$('#Id026').addClass('active open');

//	$('a.clip-chevron-right').attr('style', 'display:none');

//	$('i.clip-chevron-left').attr('style', 'display:block;font-size: 25px;');

//	$('a.clip-chevron-right').click(function () {
//		$('body').addClass('navigation-small');
//		$('a.clip-chevron-right').attr('style', 'display:none');
//		$('i.clip-chevron-left').attr('style', 'display:block;font-size: 25px;');
//	});

//	$('i.clip-chevron-left').click(function () {
//		$('body').removeClass('navigation-small');
//		$('a.clip-chevron-right').attr('style', 'display:block;font-size: 25px;');
//		$('i.clip-chevron-left').attr('style', 'display:none');
//	});
	
//});

function AltaInstalacion() {
	var tipoInstalacionJson = {
		"Nombre": $('input#Descripcion01').val(),
		"Clasificacion": $('input#clasificacion01').val()
	};
	var o = eval(tipoInstalacionJson);

	if (o.Nombre && $('input#ValidarAllCombos').val()) {
				$.ajax({
					url: '/TipoInstalaciones/AltaInstalacion',
					type: "POST",
					data: JSON.stringify({ TipoInstalacionJson: tipoInstalacionJson }),
					dataType: "json",
					contentType: "application/json; charset=utf-8",
					success: function (data) {
						if (data.result == 0) {
							$('#myModalInstalaciones').modal('hide');
							window.location.reload();
						}
						else if (data.result == 1) {
							$('#myModalInstalaciones').modal('hide');
							showLoading();
							alert(data.Message, 'TipoInstalaciones');
						} else if (data.result == 2) {
							$('#myModalInstalaciones').modal('hide');
							showLoading();
							alert(data.Message, 'Confirm', 0, 37);
						} else {
							$('#myModalInstalaciones').modal('hide');
							showLoading();
							alert(data.Message, 'Confirm', 0, 38);
						}

					}
				});
			}
	
}

function EditarInstalacion() {
	var tipoInstalacionJson = {
		"ID":$('input#ID01').val(),
		"Nombre": $('input#Descripcion01').val(),
		"Clasificacion": $('input#clasificacion01').val(),
		"activo": $('input#Activo01').val()
	};
	var o = eval(tipoInstalacionJson);
	
	if (o.ID && o.Nombre && $('input#ValidarAllCombos').val()) {
			$.ajax({
				url: '/TipoInstalaciones/EditarInstalacion',
				type: "POST",
				data: JSON.stringify({ TipoInstalacionJson: tipoInstalacionJson }),
				dataType: "json",
				contentType: "application/json; charset=utf-8",
				success: function (data) {
					if (data.result == 0) {
						$('#myModalInstalaciones').modal('hide');
						window.location.reload();
					}
					else if (data.result == 1) {
						$('#myModalInstalaciones').modal('hide');
						showLoading();
						alert(data.Message, 'TipoInstalaciones');
					} else if (data.result == 2) {
						$('#myModalInstalaciones').modal('hide');
						showLoading();
						alert(data.Message, 'Confirm', 0, 37);
					} else {
						$('#myModalInstalaciones').modal('hide');
						showLoading();
						alert(data.Message, 'Confirm', 0, 38);
					}
				}
			});
		}
}


function VolverInstalacion() {

	$('input#Descripcion01').val('');
	$('div#myModalInstalaciones.modal.fade.in').attr('style', 'display:none');
	hideLoading();
	$('#DatosInstalaciones').html('');
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

function ValidarAllCombos(estado ) {
    var comboValida = true;

    if (!ValidacionCombos(document.getElementById('clasificacion01'), 'errorClasifica'))
        comboValida = false;
    if(estado== "1")
        if (!ValidacionCombos(document.getElementById('Activo01'), 'errorActivo'))
            comboValida = false;
    
    return comboValida;
}


