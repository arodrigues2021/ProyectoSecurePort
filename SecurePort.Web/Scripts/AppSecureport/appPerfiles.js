var mensaje = "Datos guardados correctamente";
$(function () {
   
	$('#IdPerfiles').addClass('active open');

	$('#IdGestionUsuarios').attr('style', 'display:block');

	$('#IdGestionUsuarios').addClass('open');

	$('#IdSubMenuMaestros').attr('style', 'display:none');

	$('#confirm').attr('style', 'display:none');

	if (history.forward(1)) {
		location.replace(history.forward(1));
	}
	
	Ladda.bind('.button-ladda button', { timeout: 20000 });
	
	$('a.clip-chevron-right').attr('style', 'display:none');

	$('i.clip-chevron-left').attr('style', 'display:block;font-size: 25px;');
	
	$('a.clip-chevron-right').click(function () {
		$.ajax({
			type: "POST",
			url: '/Perfiles/RefrescarPerfiles',
			success: function (data) {
				if (data) {
					$('#ListPerfil').html(data);
				}
			}
		});
		$('body').addClass('navigation-small');
		$('a.clip-chevron-right').attr('style', 'display:none');
		$('i.clip-chevron-left').attr('style', 'display:block;font-size: 25px;');
	});

	$('i.clip-chevron-left').click(function () {
		$.ajax({
			type: "POST",
			url: '/Perfiles/RefrescarPerfiles',
			success: function (data) {
				if (data) {
					$('#ListPerfil').html(data);
				}
			}
		});
		$('body').removeClass('navigation-small');
		$('a.clip-chevron-right').attr('style', 'display:block;font-size: 25px;');
		$('i.clip-chevron-left').attr('style', 'display:none');
	});

	$('button#IdPerfilesEditar.btn.btn-blue').click(function() {

		var PerfilesJson = {
			"Id": $('input#id002.form-control').val(),
			"Nombre": $('input#Descripcion002.form-control').val(),
			"Activo": $('input#Activo01').val()
		};

		$.ajax({
			url: '/Usuarios/UsuarioLogueado',
			type: "POST",
			dataType: "json",
			contentType: "application/json; charset=utf-8",
			success: function(data) {
				if (data.result) {
					$.ajax({
						url: '/Perfiles/EditPerfiles',
						type: "POST",
						data: JSON.stringify({ PerfilesJson: PerfilesJson }),
						dataType: "json",
						contentType: "application/json; charset=utf-8",
						success: function(data) {
							if (data.result) {
								showLoading();
								$('#myModalEditar').modal('hide');
								alert(mensaje);
							} else {
								showLoading();
								$('#myModalEditar').modal('hide');
								alert(data.Message);
							}
						}
					});
				}
			}
		});

	});
	
});


function RegistraPerfiles() {
    var PerfilesJson = {
        "Nombre": $('input#IdPerfilNombre01').val(),
        "Descripcion": $('input#IdPerfilDescrip01').val()
    };
    var o = eval(PerfilesJson);
    if (o.Nombre !== "") {
        $.ajax({
            url: '/Usuarios/UsuarioLogueado',
            type: "POST",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.result) {
                    $.ajax({
                        url: '/Perfiles/RegisterPerfiles',
                        type: "POST",
                        data: JSON.stringify({ PerfilesJson: PerfilesJson }),
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            if (data.result) {
                                showLoading();
                                $('#myModalAgregar').modal('hide');
                                alert(mensaje);
                            } else {
                                $('#myModalAgregar').modal('hide');
                                showLoading();
                                alert(data.Message);
                            }
                        }
                    });
                }
            }
        });
    } else {
        showLoading();
        $('#myModalAgregar').modal('hide');
        alert("El Nombre Perfil es un campo requerido.", "Grupo");
    }
    if (o.Descripcion == '') {
        o.Descripcion = '';
    }
}

function RegisterPerfiles() {
	var PerfilesJson = {
		"Nombre": $('input#IdPerfilNombre01').val(),
		"Descripcion": $('input#IdPerfilDescrip01').val(),
		"Activo": $('input#Activo01').val()
	};
	var o = eval(PerfilesJson);
	if (o.Nombre !== "") {
		$.ajax({
			url: '/Usuarios/UsuarioLogueado',
			type: "POST",
			dataType: "json",
			contentType: "application/json; charset=utf-8",
			success: function (data) {
				if (data.result) {
					$.ajax({
						url: '/Perfiles/AltaPerfiles',
						type: "POST",
						data: JSON.stringify({ PerfilesJson: PerfilesJson }),
						dataType: "json",
						contentType: "application/json; charset=utf-8",
						success: function (data) {
							if (data.result) {
								showLoading();
								$('#myModalAgregar').modal('hide');
								alert(data.Message);
							} else {
								$('#myModalAgregar').modal('hide');
								showLoading();
								alert(data.Message, 'Confirm', 1, 9);
							}
						}
					});
				}
			}
		});
	} else {
		showLoading();
		$('#myModalAgregar').modal('hide');
		alert("El Nombre Perfil es un campo requerido.", "Grupo");
	}
	if (o.Descripcion == '') {
		o.Descripcion = '';
	}
}

function VolverPerfil() {
	
	$('input#IdPerfilNombre.form-control').val('');
	$('input#IdPerfilDescrip.form-control').val('');
	window.location.reload();
}

function RegistrarPerfiles() {
	showLoading();
	if ($('input#Descripcion001.form-control').val() == '') {
		alert("El Nombre es un campo requerido", "PerfilAlta");
	} else {
	    if ($('input#ValidarAllCombos').val())
	        alert("¿Esta Seguro de Guardar los cambios?", 'Confirm', 0, 9);
	    else
	        hideLoading();
	}
}

function AltaPerfilesAcciones() {
	//ALTA DE PERFIL ACCIONES
	showLoading();
	var PerfilesJson = {
		"Descripcion": $('input#Descripcion002.form-control').val(),
		"Nombre": $('input#Descripcion001.form-control').val(),
		"Activo": $('input#Activo01').val()
	};
	$.ajax({
		url: '/Perfiles/GuardarAcciones',
		type: "POST",
		data: JSON.stringify({ PerfilesJson: PerfilesJson }),
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (data) {
			if (data.result) {
				$.ajax({
					type: "POST",
					url: '/Perfiles/VisualizarPerfil',
					data: { id: null },
					success: function (data) {
						if (data) {
							hideLoading();
							$('#ListPerfil').html(data);
							$('#MostrarMensaje').html("Visualizar Perfil");
						}
					}
				});
			} else {
				setTimeout(function () {
					showLoading();
					alert(data.Message);
				}, 2000);
			}
		}
	});
}

function DeshacerAcciones() {
    showLoading();
    $('input#URL').val("/Perfiles/DeshacerAcciones");
    $('input#URL1').val("#ListPerfil");
	alert("¿Esta Seguro de Deshacer los cambios?", 'Confirm',0,6);
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
