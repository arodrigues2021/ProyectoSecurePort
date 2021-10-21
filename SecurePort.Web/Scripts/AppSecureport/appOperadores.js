var mensaje = "Datos guardados correctamente";

function AltaEditarOperadores(actionOperador) {
		var operadoresJson = {
			"Id": $('input#IdOperador01').val(),
			"Id_Instalacion": $('input#Instalacion01').val(),
			"Nombre": $('input#Nombre01').val(),			
			"Es_Suplente": ($('input#Suplente01').val() == "1"? true: false),
			"Direccion": $('input#Direccion01').val(),
			"Id_Ciudad": $('input#CiudadOperador01').val(),
			"Cod_postal": $('input#Postal01').val(),
			"Id_provincia": $('input#ProvinciaOperador01').val(),
			"Telefono1": $('input#Telefono101').val(),
			"Fax": $('input#Fax01').val(),
			"Email": $('input#EmailOperador01').val(),
			"Es_Activo": ($('input#ActivoOperador01').val() == "1" ? true : false),
			"Observaciones": $('textarea#ObservacionOperador01').val(),
			"ValidadoTipo": $('input#IdValor').val(),
			"action": actionOperador,
			"Cargo": $('input#Cargo01').val()
		};
		
	var o = eval(operadoresJson);
	if (o.Id_Instalacion && (o.Id_Usuario || o.Nombre) && $('input#Suplente01').val() && o.ValidadoTipo && $('input#ValidarAllCombosOP').val()) {
	    jsShowWindowLoad("Ejecutando datos.....");
		$.ajax({
			url: '/Operadores/AltaEditarOperador',
			type: "POST",
			data: JSON.stringify({ OperadoresJson: operadoresJson }),
			dataType: "json",
			contentType: "application/json; charset=utf-8",
			success: function (data) {
				if (data.result == 0) {
					$.ajax({
						url: '/Operadores/OperadoresEditar',
						type: "POST",
						data: { id: $('input#IdOperador01').val() },
						success: function (data) {
							if (data) {
								//$('#OperadoresRefrescar').html('');
								$('#MostrarMensaje').html('Visualizar Instalacion');
								$('#myModalOperadores').modal('hide');
								$('#OperadoresRefrescar').html(data);
								$('#Edit-page').html('');
								hideLoading();
								jsRemoveWindowLoad();
							}
						}
					});
				} else if (data.result == 1) {
					showLoading();
					alert(data.Message, 'Operadores');
				} else if (data.result == 2) {
					showLoading();
					alert(data.Message, 'Confirm', 0 , 46);
				} else {
					showLoading();
				   alert(data.Message, 'Confirm', 0, 47);
				}
			}

		});
	}
}

function VisualizarOperador(Id) {
	$.ajax({
		
		url: '/Operadores/OperadoresEditar',
		type: "POST",        
		data: { id: Id },      
		success: function (data) {
			if (data) {             
				$('#MostrarMensaje').html('Visualizar Instalación');
				$('#myModalOperadores').modal('hide');                
				$('#OperadoresRefrescar').html(data);
				$('#Edit-page').html('');
				hideLoading();
			}
			}
		});    
}

function CambiarProvinciaOperador() {
	showLoading();
	$.ajax({
		type: "POST",
		url: '/Operadores/CambiarProvincia',
		data: { id: $('input#ProvinciaOperador01').val() },
		success: function (data) {
			if (data) {
			    hideLoading();
			    ValidacionCombosOP(document.getElementById('ProvinciaOperador01'), 'errorProvinciaOP');
				$('#OperadorComboCiudad').html(data);
			}
		}
	});
}

function ValidacionCombosOP(datos, campoerror) {
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

function ValidarAllCombosOP(estadoActivo) {
    var comboValida = true;
    if (estadoActivo != "display:none")
        if (!ValidacionCombosOP(document.getElementById('ActivoOperador01'), 'errorActivoOP'))
            comboValida = false;
    if (!ValidacionCombosOP(document.getElementById('ProvinciaOperador01'), 'errorProvinciaOP'))
        comboValida = false;
    if (!ValidacionCombosOP(document.getElementById('CiudadOperador01'), 'errorCiudadOP'))
        comboValida = false;
    if (!ValidacionCombosOP(document.getElementById('Suplente01'), 'errorNotificaOP'))
        comboValida = false;
    
    return comboValida;
}


// documentos
function VolverOperadorDoc() {
	$.ajax({

		url: '/Operadores/AsociadosEditOperadores',
		type: "POST",
		data: { id: 1 },
		success: function (data) {
			if (data) {
				$('#MostrarMensaje').html('Visualizar Instalacion');
				$('#myModalUpload').modal('hide');
				$('#DocumentosRefrescar').html(data);
				$('#Edit-page').html('');
				hideLoading();
			}
		}
	});   
}

function onChangeEditOperador(e) {
	//Seleccionamos el registro
	var code = this.dataItem(this.select()).Id;
	var documento = this.dataItem(this.select()).Nombre;
	$('input#IdOperador').val(code);
	$('input#NombreOperador').val(documento);
}

function onChangeEditOperadorDoc(e) {
	//Seleccionamos el registro
	var code = this.dataItem(this.select()).id;
	var documento = this.dataItem(this.select()).documento;
	$('input#IdDoc').val(code);
	$('input#NombreDocumento').val(documento);
}

function GuardarCambios() {
	showLoading();
	var DocumentoJson = {
		"Id": $('input#idDocumento.form-control').val(),
		"tipodocumento": $('select#TiposDocumento01.form-control').val(),
		"descripcion": $('input#descripcion01.form-control').val()
	};
	$.ajax({
		url: '/Operadores/GuardarCambios',
		type: "POST",
		data: { DocumentoJson: DocumentoJson },
		success: function (data) {
			if (data.result) {
				$.ajax({
					type: "POST",
					url: '/Operadores/AsociadosEditOperadores',
					data: { id: 1},
					success: function (data) {
						if (data) {
							$('#MostrarMensaje').html('Visualizar Instalacion');
							$('#myModalUpload').modal('hide');
							$('#DocumentosRefrescar').html(data);
							$('#Edit-page').html('');
							hideLoading();
						}
					}
				});
			}
		}
	});
}

function DeshacerEdicion() {
	showLoading();
	$.ajax({
		url: '/Operadores/DeshacerCambios',
		type: "POST",
		data: { id: $('input#idDocumento.form-control').val() },
		success: function (data) {
			if (data) {
				hideLoading();
				$('#myModalUpload').modal('show');
				$('#AsignarDocumentos').html(data);
				$('#Edit-page').html('');
			}
		}
	});
}

function VisualizarDocOperadores(Id) {
		$.ajax({
			type: "POST",
			url: '/Operadores/AsociadosEditOperadores',
			data: { id: Id },
			success: function (data) {
				if (data) {
					$('#MostrarMensaje').html('Visualizar Instalación');
					$('#myModalUpload').modal('hide');
					$('#DocumentosRefrescar').html(data);
					$('#Edit-page').html('');
					hideLoading();
				}
			}
		});

}

// validaciones
function validarCorreo(email) {
	if ($('input#EmailOperador01').val() != "") {
		var expr = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
		if (!expr.test($('input#EmailOperador01').val())) {
			$('input#IdValor').val("");
			Contenido.style.display = ""
			return false;
		}
		else {
			$('input#IdValor').val(1);
			Contenido.style.display = "none";
			return true;
		}
	} else {
		$('input#IdValor').val(1);
		Contenido.style.display = "none";
		return true;
	}	    
}

function ValidarTexto(Texto)
{
	if (Texto =="")        
		return false;
	else
		return true;

}

function ValidarCombo(Combo)
{
	if (Combo == null || Combo == "")
		return false;
	else
		return true;
}

function ValidarResponsable(Usuar)
{
	if (Usuar.value =="") {
		datosReponsable.style.display = "";        
	}
	else
		datosReponsable.style.display = "none";
}
