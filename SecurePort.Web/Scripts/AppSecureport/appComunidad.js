var mensaje = "Datos guardados correctamente";
//$(function () {
   
	
//	$('#confirm').attr('style', 'display:none');

//	if (history.forward(1)) {
//		location.replace(history.forward(1));
//	}
	
//	$('#IdGestionUsuarios').attr('style', 'display:none');

//	$('#IdSubMenuMaestros').attr('style', 'display:block');

//	$('#Id012').addClass('active open');

//	$('a.clip-chevron-right').attr('style', 'display:none');

//	$('i.clip-chevron-left').attr('style', 'display:block;font-size: 25px;');

//	$('a.clip-chevron-right').click(function () {
//	    $('body').addClass('navigation-small');
//	    $('a.clip-chevron-right').attr('style', 'display:none');
//	    $('i.clip-chevron-left').attr('style', 'display:block;font-size: 25px;');
//	});

//	$('i.clip-chevron-left').click(function () {
//	    $('body').removeClass('navigation-small');
//	    $('a.clip-chevron-right').attr('style', 'display:block;font-size: 25px;');
//	    $('i.clip-chevron-left').attr('style', 'display:none');
//	});
	
//});

function AltaComunidad() {
	var comunidadJson = {
		"Nombre": $('input#Nombre01').val(),
		"Pais": $('input#Pais01').val()
	};
	var o = eval(comunidadJson);
	if (o.Nombre && o.Pais && $('input#ValidarAllCombos').val()) {
		$.ajax({
			url: '/Comunidades/AltaComunidad',
			type: "POST",
			data: JSON.stringify({ ComunidadJson: comunidadJson }),
			dataType: "json",
			contentType: "application/json; charset=utf-8",
			success: function (data) {
			    if (data.result == 0) {
			        $('#myModalComunidad').modal('hide');
			        window.location.reload();
			    } else if (data.result == 1) {
			        $('#myModalComunidad').modal('hide');
			        showLoading();
			        alert(data.Message, 'Comunidades');
			    } else if (data.result == 2) {
			        $('#myModalComunidad').modal('hide');
			        showLoading();
			        alert(data.Message, 'Confirm', 0, 39);
			    } else {
			        $('#myModalComunidad').modal('hide');
			        showLoading();
			        alert(data.Message, 'Confirm', 0, 40);
			    }
			}
		});
	} 
}

function EditarComunidad() {
	var comunidadJson = {
		"Id": $('input#ID01').val(),
		"Nombre": $('input#Comunidad01').val(),
		"Pais": $('input#Pais01').val(),
		"activo": $('input#Activo01').val()
	};
	var o = eval(comunidadJson);
	if (o.Nombre && o.Pais && $('input#ValidarAllCombos').val()) {
		$.ajax({
			url: '/Comunidades/EditarComunidad',
			type: "POST",
			data: JSON.stringify({ ComunidadJson: comunidadJson }),
			dataType: "json",
			contentType: "application/json; charset=utf-8",
			success: function (data) {
			    if (data.result == 0) {
			        $('#myModalComunidad').modal('hide');
			        window.location.reload();
			    } else if (data.result == 1) {
			        $('#myModalComunidad').modal('hide');
			        showLoading();
			        alert(data.Message, 'Comunidades');
			    } else if (data.result == 2) {
			        $('#myModalComunidad').modal('hide');
			        showLoading();
			        alert(data.Message, 'Confirm', 0, 39);
			    } else {
			        $('#myModalComunidad').modal('hide');
			        showLoading();
			        alert(data.Message, 'Confirm', 0, 40);
			    }
			}
		});
	}
}


function VolverComunidad() {

	$('input#Nombre01').val('');
	$('input#Pais01').val('');
	$('div#myModalComunidad.modal.fade.in').attr('style', 'display:none');
	hideLoading();
	$('#DatosComunidad').html('');
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
    if (estado == "1")
        if (!ValidacionCombos(document.getElementById('Activo01'), 'errorActivo'))
            comboValida = false;
    if (!ValidacionCombos(document.getElementById('Pais01'), 'errorPais'))
        comboValida = false;

    return comboValida;
}