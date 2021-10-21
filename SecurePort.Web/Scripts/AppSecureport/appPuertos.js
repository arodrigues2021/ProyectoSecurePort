var mensaje = "Datos guardados correctamente";

var FormValidar = function () {
    var runValidator1 = function () {
        var form1 = $('#Puertoform');
        var errorHandler1 = $('.errorHandler', form1);
        var successHandler1 = $('.successHandler', form1);
        $('#Puertoform').validate({
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
                Nombre: {
                    minlength: 2,
                    required: true
                },
                Responsable: {
                    minlength: 2,
                    required: true
                },
            },
            messages: {
                Nombre: 'Este campo es requerido.',
                Responsable: 'Este campo es requerido.',
                
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


function AltaEditarPuertos(action) {
	var puertosJson = {
	    "Id": $('input#Id01').val(),
	    "Nombre": $('input#Nombre01').val(),
	    "id_Organismo": $('input#Organismo01').val(),
	    "Responsable": $('input#Responsable01').val(),
	    "Direccion": $('input#Direccion01').val(),
	    "Id_Ciudad": $('input#Ciudad01').val(),
	    "Cod_Postal": $('input#codPostal01').val(),
	    "Id_Provincia": $('input#Provincia01').val(),
	    "Observaciones": $('textarea#Observaciones01').val(),
	    "Id_CapMarit": $('input#Capitania01').val(),
	    "Latitud": $('input#Latitud01').val(),
	    "Longitud": $('input#Longitud01').val(),
	    "Locode": $('input#Locode01').val(),
		"action": action,
		"activo": $('input#Activo01').val()

	};
	var o = eval(puertosJson);	
	if (o.Nombre && o.id_Organismo && o.Responsable && $('input#ValidarAllCombos').val()) {
		$.ajax({
			url: '/Puertos/AltaEditarPuertos',
			type: "POST",
			data: JSON.stringify({ PuertosJson: puertosJson }),
			dataType: "json",
			contentType: "application/json; charset=utf-8",
			success: function (data) {
				if (data.result == 0) {
				    showLoading();
				    $.ajax({
				        type: "POST",
				        url: '/Puertos/VisualizarPuertos',
				        data: { id: null, Accion: "Ver" },
				        success: function (data) {
				            if (data) {
				                hideLoading();
				                $('#ListPuerto').html(data);
				            }
				        }
				    });
				} else if (data.result == 1) {
				    showLoading();
				    alert(data.Message, 'Puertos');
				} else if (data.result == 2) {
				    showLoading();
				    alert(data.Message, 'Confirm', 0 , 43);
				} else {
				    showLoading();
				   alert(data.Message, 'Confirm', 0, 44);
				}
			}

		});
	}
}



function CambiarProvincia() {
    showLoading();
    $.ajax({
        type: "POST",
        url: '/Puertos/CambiarProvincia',
        data: { id: $('input#Provincia01').val() },
        success: function (data) {
            if (data) {
                ValidacionCombos(document.getElementById('Provincia01'), 'errorProvincia');
                hideLoading();
                $('#ComboCiudad').html(data);                
            }
        }
    });
}

function CambiarCiudad() {
    showLoading();
    $.ajax({
        type: "POST",
        url: '/Puertos/CambiarCiudad',
        data: { id: $('input#Ciudad01').val() },
        success: function (data) {
            if (data) {
                ValidacionCombos(document.getElementById('Ciudad01'), 'errorCiudad');
                hideLoading();
                $('#ComboIsla').html(data);
            }
        }
    });
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

function ValidarAllCombos(estadoActivo) {
    var comboValida = true;
    if (estadoActivo != "display:none")
        if (!ValidacionCombos(document.getElementById('Activo01'), 'errorActivo'))
            comboValida = false;
    if (!ValidacionCombos(document.getElementById('Organismo01'), 'errorOrganismo'))
        comboValida = false;
    if (!ValidacionCombos(document.getElementById('Provincia01'), 'errorProvincia'))
        comboValida = false;
    if (!ValidacionCombos(document.getElementById('Ciudad01'), 'errorCiudad'))
        comboValida = false;
    if (!ValidacionCombos(document.getElementById('Capitania01'), 'errorCapitania'))
        comboValida = false;


    return comboValida;
}


function AltaEditarOperadoresPuerto(actionOperador) {
    var operadoresPuertoJson = {
        "Id": $('input#IdOperador01').val(),
        "Id_Puerto": $('input#Puerto01').val(),
        "Nombre": $('input#NombreOperador01').val(),
        "Es_Suplente": ($('input#Suplente01').val()=="1" ? "true" : "false"),
        "Telefono": $('input#Telefono01').val(),
        "Fax": $('input#Fax01').val(),
        "Email": $('input#Correo01').val(),
        "Es_Activo": ($('input#Activo01').val()=="1" ? "true" : "false"),
        "Observaciones": $('textarea#Observacion01').val(),
        "Direccion": $('input#DireccionOperador01').val(),
        "Id_Ciudad": $('input#CiudadOperador01').val(),
        "Id_provincia": $('input#ProvinciaOperador01').val(),
        "Cod_Postal": $('input#PostalOperador01').val(),        
        "ValidadoTipo": $('input#IdValor').val(),
        "action": actionOperador
    };
    var o = eval(operadoresPuertoJson);
    if (o.Id_Puerto && o.Nombre && o.Es_Suplente && o.ValidadoTipo && $('input#ValidarAllCombosOP').val()) {
        jsShowWindowLoad("Ejecutando datos.....");
        $.ajax({
            url: '/Puertos/AltaEditarOperador',
            type: "POST",
            data: JSON.stringify({ OperadoresPuertoJson: operadoresPuertoJson }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.result == 0) {
                    $.ajax({
                        url: '/Puertos/OperadoresEditar',
                        type: "POST",
                        data: { id: $('input#IdOperador01').val() },
                        success: function (data) {
                            if (data) {
                                $('#MostrarMensaje').html('Visualizar Puerto');
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
                    alert(data.Message, 'Confirm', 0, 46);
                } else {
                    showLoading();
                    alert(data.Message, 'Confirm', 0, 47);
                }
            }

        });
    }
}

function onChangeEditOperadorPuerto(e) {
    //Seleccionamos el registro
    var code = this.dataItem(this.select()).Id;
    var documento = this.dataItem(this.select()).Nombre;
    $('input#IdOperador').val(code);
    $('input#NombreOperador').val(documento);
}

function VisualizarOperador(Id) {
    $.ajax({

        url: '/Puertos/OperadoresEditar',
        type: "POST",
        data: { id: Id },
        success: function (data) {
            if (data) {
                $('#MostrarMensaje').html('Visualizar Puerto');
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
        url: '/Puertos/CambiarProvinciaOperador',
        data: { id: $('input#ProvinciaOperador01').val() },
        success: function (data) {
            if (data) {
                ValidacionCombos(document.getElementById('ProvinciaOperador01'), 'errorProvinciaOP');
                hideLoading();
                $('#OperadorComboCiudad').html(data);
            }
        }
    });
}



function validarCorreo(email) {
    if ($('input#Correo01').val() != "" ) {
        var expr = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
        if (!expr.test($('input#Correo01').val())) {
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

function ValidarTexto(Texto) {
    if (Texto == "")
        return false;
    else
        return true;

}

function ValidarCombo(Combo) {
    if (Combo == null || Combo == "")
        return false;
    else
        return true;

}

function ValidarResponsable(Usuar) {
    if (Usuar.value == "") {
        datosReponsable.style.display = "";
    }
    else
        datosReponsable.style.display = "none";
}

function ValidarAllCombosOP(estadoActivo) {
    var comboValida = true;
    if (estadoActivo != "display:none")
        if (!ValidacionCombos(document.getElementById('ActivoOperador01'), 'errorActivoOP'))
            comboValida = false;
    if (!ValidacionCombos(document.getElementById('ProvinciaOperador01'), 'errorProvinciaOP'))
        comboValida = false;
    if (!ValidacionCombos(document.getElementById('CiudadOperador01'), 'errorCiudadOP'))
        comboValida = false;
    if (!ValidacionCombos(document.getElementById('Suplente01'), 'errorSuplenteOP'))
        comboValida = false;


    return comboValida;
}
