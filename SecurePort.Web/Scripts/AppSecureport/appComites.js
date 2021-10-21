var mensaje = "Datos guardados correctamente";

var FormValidarComite = function () {
	var runValidator1 = function () {
		var form1 = $('#Comiteform');
		var errorHandler1 = $('.errorHandler', form1);
		var successHandler1 = $('.successHandler', form1);
		$('#Comiteform').validate({
			errorElement: "span", // contain the error msg in a span tag
			errorClass: 'help-block',
			errorPlacement: function (error, element) { // render error placement for each input type
				if (element.attr("type") == "radio" || element.attr("type") == "checkbox") { // for chosen elements, need to insert the error after the chosen container
					error.insertAfter($(element).closest('.form-group').children('div').children().last());
				} else if (element.attr("name") == "dd" || element.attr("name") == "mm" || element.attr("name") == "yyyy") {
					error.insertAfter($(element).closest('.form-group').children('div'));
				} else {
					error.insertAfter(element);
					// for other inputs, just perform default behavior
				}
			},
			ignore: "",
			rules: {
				Nivel: {
					minlength: 1,
					required: true,                    
				},
				Convocado: {
					minlength: 8,
					required: true
				},
			},
			messages: {
				Nivel: 'Este campo es requerido.',
				Convocado: 'Este campo es requerido.',

				gender: "Please check a gender!"
			},
			groups: {
				DateofBirth: "dd mm yyyy",
			},
			invalidHandler: function (event, validator) { //display error alert on form submit
				successHandler1.hide();
				errorHandler1.show();
			},
			highlight: function (element) {
				$(element).closest('.help-block').removeClass('valid');
				// display OK icon
				$(element).closest('.form-group').removeClass('has-success').addClass('has-error').find('.symbol').removeClass('ok').addClass('required');
				// add the Bootstrap error class to the control group
			},
			unhighlight: function (element) { // revert the change done by hightlight
				$(element).closest('.form-group').removeClass('has-error');
				// set error class to the control group
			},
			success: function (label, element) {
				label.addClass('help-block valid');
				// mark the current input as valid and display OK icon
				$(element).closest('.form-group').removeClass('has-error').addClass('has-success').find('.symbol').removeClass('required').addClass('ok');
			},
			submitHandler: function (form) {
				// successHandler1.show();
				errorHandler1.hide();
			}
		});
	};
	return {
		//main function to initiate template pages
		init: function () {
			runValidator1();

		}
	};
}();




function AltaEditarComites(action) {
	var comitesJson = {
		"Id": $('input#Id01').val(),
		"ID_Organismo": $('input#Organismo01').val(),
		"Nivel": $('input#Nivel01').val(),
		"Convocado": $('input#Convocado01').val(),
		"Observaciones": $('textarea#Observaciones01').val(),
		"ID_Puerto": $('input#Puerto01').val(),
		"action": action
	};
	var o = eval(comitesJson);
	if (o.ID_Organismo && o.ID_Puerto && o.Nivel && o.Convocado && $('input#ValidarAllCombos').val()) {
		$.ajax({
			url: '/Comites/AltaEditarComites',
			type: "POST",
			data: JSON.stringify({ ComitesJson: comitesJson }),
			dataType: "json",
			contentType: "application/json; charset=utf-8",
			success: function (data) {
				if (data.result == 0) {
					VisualizarComite(0);
				} else if (data.result == 1) {
					showLoading();
					alert(data.Message, 'Comites');
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

function VisualizarComite(Id) {
		$.ajax({
			type: "POST",
			url: '/Comites/VisualizarComites',
			data: { id: Id,  accion: "Ver" },
			success: function (data) {
				if (data) {
					$('#MostrarMensaje').html('Visualizar Comite');
					$('#myModalUpload').modal('hide');
					$('#ListComite').html(data);
					$('#Edit-page').html('');
					hideLoading();
				}
			}
		});
}

function VolverComiteDoc() {
	$.ajax({
		url: '/Comites/AsociadosEditComites',
		type: "POST",
		data: { id: 1 },
		success: function (data) {
			if (data) {
				$('#MostrarMensaje').html('Visualizar Comite');
				$('#myModalUpload').modal('hide');
				$('#DocumentosRefrescar').html(data);
				$('#Edit-page').html('');
				hideLoading();
			}
		}
	});
}

function onChangeEditComite(e) {
	//Seleccionamos el registro
	var code = this.dataItem(this.select()).id;
	var documento = this.dataItem(this.select()).documento;
	$('input#IdDoc').val(code);
	$('input#NombreDocumento').val(documento);
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

    if (!ValidacionCombos(document.getElementById('Puerto01'), 'errorPuerto'))
        comboValida = false;
    if (!ValidacionCombos(document.getElementById('Organismo01'), 'errorOrganismo'))
        comboValida = false;
    if (!ValidacionCombos(document.getElementById('Nivel01'), 'errorNivel'))
        comboValida = false;
    
    return comboValida;
}

// documentos
function GuardarCambios() {
	showLoading();
	var DocumentoJson = {
		"Id": $('input#idDocumento.form-control').val(),
		"tipodocumento": $('select#TiposDocumento01.form-control').val(),
		"descripcion": $('input#descripcion01.form-control').val()
	};
	$.ajax({
		url: '/Comites/GuardarCambios',
		type: "POST",
		data: { DocumentoJson: DocumentoJson },
		success: function (data) {
			if (data.result) {
				$.ajax({
					type: "POST",
					url: '/Comites/AsociadosEditComites',
					data: { id: 1},
					success: function (data) {
						if (data) {
							$('#MostrarMensaje').html('Visualizar Comite');
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
		url: '/Comites/DeshacerCambios',
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

function VisualizarDocComites(Id) {
		$.ajax({
			type: "POST",
			url: '/Comites/AsociadosEditComites',
			data: { id: Id },
			success: function (data) {
				if (data) {
					$('#MostrarMensaje').html('Visualizar Comite');
					$('#myModalUpload').modal('hide');
					$('#DocumentosRefrescar').html(data);
					$('#Edit-page').html('');
					hideLoading();
				}
			}
		});
}

function CambiarOrganismo() {
	showLoading();
	$.ajax({
		type: "POST",
		url: '/Comites/CambiarPuerto',
		data: { id: $('input#Organismo01').val() },
		success: function (data) {
		    if (data) {
		        ValidacionCombos(document.getElementById('Organismo01'), 'errorOrganismo');
				hideLoading();
				$('#ComboTipo').html(data);
			}
		}
	});
}