var mensaje = "Datos guardados correctamente";
$(function () {
   
	//$('#confirm').attr('style', 'display:none');

	//if (history.forward(1)) {
	//	location.replace(history.forward(1));
	//}

	//$('#IdGestionUsuarios').attr('style', 'display:none');

	//$('#IdSubMenuMaestros').attr('style', 'display:none');

	//$('#IdSubMenuMovimientos').attr('style', 'display:block');
	
	//$('#IdSubMenuMaestros').removeClass('open');
	
	//$('#IdSubMenuMovimientos').addClass('open');
	
	//$('#Id011').addClass('active open');

	//$('a.clip-chevron-right').attr('style', 'display:none');

	//$('i.clip-chevron-left').attr('style', 'display:block;font-size: 25px;');
	
	//$('a.clip-chevron-right').click(function () {
	//	$('body').addClass('navigation-small');
	//	$('a.clip-chevron-right').attr('style', 'display:none');
	//	$('i.clip-chevron-left').attr('style', 'display:block;font-size: 25px;');
	//});

	//$('i.clip-chevron-left').click(function () {
	//	$('body').removeClass('navigation-small');
	//	$('a.clip-chevron-right').attr('style', 'display:block;font-size: 25px;');
	//	$('i.clip-chevron-left').attr('style', 'display:none');
	//});

});

function AltaEditarBien(action) {
	var BienesJson = {
		"Id": $('input#ID01').val(),
		"Descripcion": $('input#Descripcion01').val(),
		"id_Tipo_IIPP": $('input#TipoInstalacion01').val(),
		"id_Bien_Padre": $('input#BienPadre01').val(),
		"activo": $('input#Activo01').val(),
		"action": action
	};
		var o = eval(BienesJson);
		if (o.Descripcion && o.id_Tipo_IIPP && $('input#ValidarAllCombos').val()) {
				$.ajax({
					url: '/Bienes/AltaEditarBienes',
					type: "POST",
					data: JSON.stringify({ BienesJson: BienesJson }),
					dataType: "json",
					contentType: "application/json; charset=utf-8",
					success: function (data) {
						if (data.result == 0) {
							$('#myModalBienes').modal('hide');
							window.location.reload();
						}
						else if (data.result == 1) {
							$('#myModalBienes').modal('hide');
							showLoading();
							alert(data.Message, 'Bienes');
						} else if (data.result == 2) {
							$('#myModalBienes').modal('hide');
							showLoading();
							alert(data.Message, 'Confirm', BienesJson, 25);
						} else {
							$('#myModalBienes').modal('hide');
							showLoading();
							alert(data.Message, 'Confirm', BienesJson, 29);
						}


					}
				});
			}
}


function VolverBien() {

	$('input#Nombre01').val('');
	$('input#Codigo01').val('');
	$('div#myModalBienes.modal.fade.in').attr('style', 'display:none');
	hideLoading();
	$('#DatosBienes').html('');
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
    if (!ValidacionCombos(document.getElementById('TipoInstalacion01'), 'errorTipo'))
        comboValida = false;
    if (!ValidacionCombos(document.getElementById('BienPadre01'), 'errorPadre'))
        comboValida = false;  
    if (estado != "display:none")
        if (!ValidacionCombos(document.getElementById('Activo01'), 'errorActivo'))
            comboValida = false;


    return comboValida;
}



