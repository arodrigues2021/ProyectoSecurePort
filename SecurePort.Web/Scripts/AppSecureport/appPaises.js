var mensaje = "Datos guardados correctamente";
function AltaPais() {
		var PaisJson = {
		"Nombre": $('input#Nombre01').val(),
		"Codigo": $('input#Codigo01').val(),
		"Tipo": $('input#Tipo01').val(),
		"ID_Provincia": $('input#Provincia01').val(),
		"ID_Isla": $('input#Isla01').val()
            
	};
		var o = eval(PaisJson);

		if (o.Tipo == "Ciudad") 
		{
		    if (o.Nombre && o.Codigo && o.ID_Provincia && $('input#ValidarAllCombos').val()) {
				$.ajax({
					url: '/Paises/AltaCiudad',
					type: "POST",
					data: JSON.stringify({ PaisJson: PaisJson }),
					dataType: "json",
					contentType: "application/json; charset=utf-8",
					success: function (data) {
						if (data.result == 0) {
							$('#myModalCiudades').modal('hide');
							window.location.reload();
						} else if (data.result == 1) {
						    $('#myModalCiudades').modal('hide');
						    showLoading();
						    alert(data.Message, 'Ciudades');
						} else if (data.result == 2) {
						    $('#myModalCiudades').modal('hide');
						    showLoading();
						    alert(data.Message, 'Confirm', 0, 35);
						} else {
						    $('#myModalCiudades').modal('hide');
						    showLoading();
						    alert(data.Message, 'Confirm', 0, 36);
						}
					}
				});
			}
		}else{
		    if (o.Nombre && o.Codigo) {
				$.ajax({
					url: '/Paises/AltaPais',
					type: "POST",
					data: JSON.stringify({ PaisJson: PaisJson }),
					dataType: "json",
					contentType: "application/json; charset=utf-8",
					success: function (data) {
						if (data.result == 0) {
							$('#myModalPaises').modal('hide');
							window.location.reload();
						} else if (data.result == 1) {
						    $('#myModalPaises').modal('hide');
						    showLoading();
						    alert(data.Message, 'Paises');
						} else if (data.result == 2) {
						    $('#myModalPaises').modal('hide');
						    showLoading();						  
						    alert(data.Message, 'Confirm', 0, 32);
						} else {
						    $('#myModalPaises').modal('hide');
						    showLoading();
						    alert(data.Message, 'Confirm', 0, 33);
						}
					}
				});
			}

		}
}

function AltaEditarProvincias(action) {
	var ProvinciasJson = {
		"Id": $('input#ID01').val(),
		"Codigo": $('input#Codigo01').val(),
		"ID_ComAut": $('input#Comunidad01').val(),
		"Nombre": $('input#Nombre01').val(),
		"activo": $('input#Activo01').val(),
		"action":action
	};
	var o = eval(ProvinciasJson);
	if (o.Id && o.Codigo && o.Nombre && o.ID_ComAut && $('input#ValidarAllCombos').val()) {
		$.ajax({
			url: '/Provincias/AltaEditarProvincias',
			type: "POST",
			data: JSON.stringify({ ProvinciasJson: ProvinciasJson }),
			dataType: "json",
			contentType: "application/json; charset=utf-8",
			success: function (data) {
			    if (data.result == 0) {
			        $('#myModalProvincias').modal('hide');
			        window.location.reload();
			    } else if (data.result == 1) {
			        $('#myModalProvincias').modal('hide');
			        showLoading();
			        alert(data.Message, 'Provincias');
			    } else if (data.result == 2) {
			        $('#myModalProvincias').modal('hide');
			        showLoading();
			        alert(data.Message, 'Confirm', 0, 31);
			    } else {
			        $('#myModalProvincias').modal('hide');
			        showLoading();
			        alert(data.Message, 'Confirm', 0, 32);
			    }
			}
		});
	}
}

function EditarPais() {
	var PaisJson = {
		"ID_Pais":$('input#ID01').val(),
		"Nombre": $('input#Nombre01').val(),
		"Codigo": $('input#Codigo01').val(),
		"Tipo": $('input#Tipo01').val(),
		"ID_Provincia": $('input#Provincia01').val(),
		"ID_Isla": $('input#Isla01').val(),
		"activo": $('input#Activo01').val()
	};
	var o = eval(PaisJson);
	if (o.Tipo == "Ciudad") {
	    if (o.ID_Pais && o.Nombre && o.Codigo && o.ID_Provincia && $('input#ValidarAllCombos').val()) {
			$.ajax({
				url: '/Paises/EditarCiudad',
				type: "POST",
				data: JSON.stringify({ PaisJson: PaisJson }),
				dataType: "json",
				contentType: "application/json; charset=utf-8",
				success: function (data) {
                   if (data.result == 0) {
						$('#myModalCiudades').modal('hide');
						window.location.reload();
					} else if (data.result == 1) {
					    $('#myModalCiudades').modal('hide');
					    showLoading();
					    alert(data.Message, 'Ciudades');
					} else if (data.result == 2) {
					    $('#myModalCiudades').modal('hide');
					    showLoading();
					    alert(data.Message, 'Confirm', 0, 35);
					} else {
					    $('#myModalCiudades').modal('hide');
					    showLoading();
					    alert(data.Message, 'Confirm', 0, 36);
					}
				}
			});
		}
	} else {
	    if (o.ID_Pais && o.Nombre && o.Codigo && $('input#ValidarAllCombos').val()) {
			$.ajax({
				url: '/Paises/EditarPais',
				type: "POST",
				data: JSON.stringify({ PaisJson: PaisJson }),
				dataType: "json",
				contentType: "application/json; charset=utf-8",
				success: function (data) {
					if (data.result== 0) {
						$('#myModalPaises').modal('hide');
						window.location.reload();
					} else if (data.result == 1) {
					    $('#myModalPaises').modal('hide');
					    showLoading();
					    alert(data.Message, 'Paises');
					} else if (data.result == 2) {
					    $('#myModalPaises').modal('hide');
					    showLoading();
					    alert(data.Message, 'Confirm', 0, 33);
					} else {
					    $('#myModalPaises').modal('hide');
					    showLoading();
					    alert(data.Message, 'Confirm', 0, 34);
					}
				}
			});
		}
	}
}


function VolverProvincia() {

	$('input#Nombre01').val('');
	$('input#Codigo01').val('');
	$('div#myModalProvincias.modal.fade.in').attr('style', 'display:none');
	hideLoading();
	$('#DatosProvincias').html('');
	window.location.reload();

}

function VolverPais() {

	$('input#Nombre01').val('');
	$('input#Codigo01').val('');
	$('div#myModalPaises.modal.fade.in').attr('style', 'display:none');
	hideLoading();
	$('#DatosPaises').html('');
	window.location.reload();
	
}

function VolverCiudad() {

	$('input#Nombre01').val('');
	$('input#Codigo01').val('');
	$('div#myModalPaises.modal.fade.in').attr('style', 'display:none');
	hideLoading();
	$('#DatosCiudades').html('');
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

function ValidarAllCombosC(estado) {   
    var comboValida = true;
    if(estado == "1")
        if (!ValidacionCombos(document.getElementById('Activo01'), 'errorActivo'))
            comboValida = false;
    if (!ValidacionCombos(document.getElementById('Provincia01'), 'errorProvincia'))
        comboValida = false;
    if (!ValidacionCombos(document.getElementById('Isla01'), 'errorIsla'))
        comboValida = false;

    return comboValida;
}

function ValidarAllCombosP() {
    var comboValida = true;
        if (!ValidacionCombos(document.getElementById('Activo01'), 'errorActivo'))
            comboValida = false;    
    return comboValida;
}

function ValidarAllCombos(estado) {
    var comboValida = true;
    if (estado != "Alta")
        if (!ValidacionCombos(document.getElementById('Activo01'), 'errorActivo'))
            comboValida = false;  
    if (!ValidacionCombos(document.getElementById('Comunidad01'), 'errorComunidad'))
        comboValida = false;

    return comboValida;
}