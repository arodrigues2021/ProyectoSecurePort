var mensaje = "Datos guardados correctamente";
$(function () {

	$('#confirm').attr('style', 'display:none');

	$('#IdUsuario').addClass('active open');

	$('#IdGestionUsuarios').attr('style', 'display:block');
	
	$('#IdSubMenuMaestros').attr('style', 'display:none');

	$('#IdGestionUsuarios').addClass('open');

	if (history.forward(1)) {
		location.replace(history.forward(1));
	}
	
	$('a#myLink').click(function() {
		$.post("/Login/validarlogin", { email: $('input#login.form-control').val(), password: $('input#password.form-control.password').val(), parametro: 1 }, function(data) {
			$.each(data, function(i, item) {
				if (item.nombre != "" && item.id != "4") {
					$('input#login.form-control').blur();
					$('input#password.form-control.password').blur();
					$('#Idloading').attr('style', 'display:none');
					setTimeout(function() {
						showLoading();
						alert(item.nombre, "Login");
					}, 1000);
				} else {
					if (item.id == "1") {
						$('button.btn.btn-primary').attr('style', 'display:none');
						$('div#myModalCondiciones.modal.container.fade.in').attr('style', 'margin-top:-50%');
						$('div#myModalCondiciones.modal.container.fade.in').attr('style', 'display:block');

					} else if (item.id == "4") {
						$('button.btn.btn-primary').attr('style', 'display:block');
						$('div#myModalCondiciones.modal.container.fade.in').attr('style', 'margin-top:-50%');
						$('div#myModalCondiciones.modal.container.fade.in').attr('style', 'display:block');
					}
				}
			});
		});
	});
	
	$('input#checkbox0012_.k-radio').click(function () {
		//OPIP
		$('div#IdFecha').attr('style', 'display:block');
		$('#IdOpp').val("OPIP");
	});

	$('input#checkbox0011_.k-radio').click(function () {
		//OPP
		$('div#IdFecha').attr('style', 'display:none');
		$('#IdOpp').val("OPP");
	});

	$('input#checkbox0013_.k-radio').click(function () {
		$('div#IdFecha').attr('style', 'display:none');
		$('#IdOpp').val("OTROS");
	});
	
	$('button#RegisterUsuario.btn.btn-primary.hidden-xs').click(function () {
		if (ValidarAllCombos())
			$('input#ValidarAllCombos').val("1");
		else
			$('input#ValidarAllCombos').val("");
		FormValidator.init();
		var UsuariosJson = {
			"Nombre": $('input#firstname.form-control').val(),
			"Apellidos": $('input#lastname.form-control').val(),
			"Categoria": $('input#Categoria01').val(),
			"Grupo": $('input#Gruposdropdown01').val(),
			"Login": $('input#login.form-control').val(),
			"Email": $('input#email.form-control').val(),
			"password_": $('input#password.form-control').val(),
			"password_again_": $('input#password_again.form-control').val(),
			"Observaciones": $('textarea#Observaciones').val(),
			"fech_exp_OPIP": $('input#datetimepicker').val(),
			"opp": $('#IdOpp').val(),
			"empresa": $('input#empresa.form-control').val()
		};
		var o = eval(UsuariosJson);
		if (o.Nombre && o.Apellidos && o.Email && o.Login && o.Grupo && o.Categoria && $('input#ValidarAllCombos').val()) {
			jsShowWindowLoad("Ejecutando datos.....");
			$('#myModalAgregar').modal('hide');
			$('input#IdUsuario').val("");
			$.ajax({
				url: '/Usuarios/UsuarioLogueado',
				type: "POST",
				dataType: "json",
				contentType: "application/json; charset=utf-8",
				success: function(data) {
					if (data.result) {
						$.ajax({
							url: '/Usuarios/AltaUsuarios',
							type: "POST",
							data: JSON.stringify({ UsuariosJson: UsuariosJson }),
							dataType: "json",
							contentType: "application/json; charset=utf-8",
							success: function(data) {
								if (data.result == true) {
									showLoading();
									if (o.Categoria != 1 || o.Categoria != 2) {								      
										$.ajax({
											type: "POST",
											url: '/Usuarios/AsignarDependencia',
											data: JSON.stringify({ UsuariosJson: UsuariosJson }),
											success: function(data) {
												if (data) {
													hideLoading();
													$('#ListUsers').html(data);
													$('#Edit-page').html('');
													$('#MostrarMensaje').html('Mantenimiento Dependencia');
													jsRemoveWindowLoad();

												}
											}
										});
									} else {								        
										$.ajax({
											type: "POST",
											url: '/Usuarios/AsignarDependenciaAdmin',
											data: JSON.stringify({ UsuariosJson: UsuariosJson }),
											success: function(data) {
											  if (data) {
												 hideLoading();
												 $('#ListUsers').html(data);
												 $('#Edit-page').html('');
												 $('#MostrarMensaje').html('Mantenimiento Dependencia');
												 jsRemoveWindowLoad();

											 }
										 }
									    });
									    
									}
							} else {
									showLoading();
									alert(data.Message, 'Correo');
								}
							}

						});
					}
				}
			});
		}
	});

	$("#send").click(function () {
		var LoginJson = {
			"Email": $('input#login.form-control').val(),
			"Password": $('input#password.form-control.password').val(),
		};
		var o = eval(LoginJson);
		if (o.Password == "") {
			showLoading();
			alert('Contraseña no puede estar vacio.', "Login");
		}
		if (o.Email == "") {
			showLoading();
			alert('Usuario no puede estar vacio.', "Login");
		}
		if (o.Email !== "" && o.Password !== "" && o.Password !== undefined) {
			Loading();
			$.post("/Login/validarlogin", { email: o.Email, password: o.Password, parametro: '0' }, function(data) {
				$.each(data, function(i, item) {
					if (item.nombre != "" && item.id != "4") {
						$('input#login.form-control').blur();
						$('input#password.form-control.password').blur();
						$('#Idloading').attr('style', 'display:none');
						setTimeout(function() {
							showLoading();
							alert(item.nombre, "Login");
						}, 2000);
					} else if (item.id == "4") {
						showLoading();
						$('div#myModalCondiciones.modal.fade.in').attr('style', 'display:block');
					} else {
						location.reload();
					}

				});

			});
		}

	});

	$('a.clip-chevron-right').attr('style', 'display:none');

	$('i.clip-chevron-left').attr('style', 'display:block;font-size: 25px;');
	
	$('a.clip-chevron-right').click(function() {
		$('body').addClass('navigation-small');
		$('a.clip-chevron-right').attr('style', 'display:none');
		$('i.clip-chevron-left').attr('style', 'display:block;font-size: 25px;');
	});

	$('i.clip-chevron-left').click(function() {
		$('body').removeClass('navigation-small');
		$('a.clip-chevron-right').attr('style', 'display:block;font-size: 25px;');
		$('i.clip-chevron-left').attr('style', 'display:none');
	});
	
});

function GuardarUsuario(item) {
	showLoading();
	if ($('input#Categoria01').val() != $('input#IdCategoria').val()) {
		if ($('input#IdDependencias').val() == "True") {
			ValidarCategoria();
		} else {
			if (ValidarAllCombos())
				alert("¿Esta Seguro de Guardar los cambios?", 'Confirm', 0, item);
			else
				hideLoading();
		}
		
	} else {
		alert("¿Esta Seguro de Guardar los cambios?", 'Confirm', 0, item);
	}
 }

function GuardarDependencia(item) {
	showLoading();
	$.ajax({
		url: '/Usuarios/ValidarDependencias',
		data: { categoria: $('input#Categoria01').val() },
		type: "POST",
		success: function (data) {
			if (data.result == false) {
				alert(data.Message,"Eliminar");
			} else {
				alert("¿Esta Seguro de Guardar las Dependencias asociadas?", 'Confirm', 0, item);
			}
		}
	});
	
}


function Reiniciar() {
	showLoading();
	ReiniciarContrasena("¿Esta Seguro de Reiniciar la contraseña?");
}

function AsignarDependencia() {
   showLoading();
   var expresion = $('input#Categoria01').val();
	switch (expresion) {
		case "1":
			alert('Para este tipo de categoria no existen dependencias', "Visualizar");
			break;
		case "2":
			alert('Para este tipo de categoria no existen dependencias', "Visualizar");
			break;
		case "6":
			alert('Para este tipo de categoria no existen dependencias', "Visualizar");
			break;
		default:
			$.ajax({
				type: "POST",
				url: '/Usuarios/AsignarDependencia',
				success: function (data) {
					if (data) {
						hideLoading();
						$('#ListUsers').html(data);
						$('#Edit-page').html('');
						$('#MostrarMensaje').html('Mantenimiento Dependencia');

					}
				}
			});
	}
}

function UsuarioEditar() {
	 $.ajax({
			type: "POST",
			url: '/Usuarios/Edit',
			success: function (data) {
				if (data) {
					$('#MostrarMensaje').html('Modificar Usuarios');
					$('#ListUsers').html(data);
					$('#Edit-page').html('');
				}
			}
		});
	}

function GuardarUsuarioEditar() {
	var validator = $("#form").kendoValidator().data("kendoValidator");
	var status = $(".status");
	if (validator.validate()) {
		status.removeClass("invalid").addClass("valid");
	} else {
		status.removeClass("valid").addClass("invalid");
	}	
	var UsuariosEditJson = {
		"Id":            $('input#id01.k-textbox.form-control').val(),
		"Categoria":     $('input#Categoria01').val(),
		"Id_Grupo":      $('input#Gruposdropdown01').val(),
		"Nombre":        $('input#firstname01.form-control').val(),
		"Login":         $('input#email01.form-control').val(),
		"Apellidos":     $('input#lastname01.form-control').val(),
		"Email":         $('input#email01.form-control').val(),
		"password_":     $('input#password01.form-control').val(),
		"Observaciones": $('textarea#Observaciones01.form-control').val(),
		"fech_exp_OPIP": $('input#datetimepicker').val(),
		"Empresa":       $('input#empresas01.form-control').val()
	};
	var o = eval(UsuariosEditJson);
	if (o.Nombre && o.Apellidos && o.Email && o.Login && o.Id_Grupo && o.Categoria) {
		showLoading();
		$.ajax({
			url: '/Usuarios/EditUsuarios',
			type: "POST",
			data: JSON.stringify({ UsuariosEditJson: UsuariosEditJson }),
			dataType: "json",
			contentType: "application/json; charset=utf-8",
			success: function (data) {
				if (data.result) {
					$.ajax({
						type: "POST",
						url: '/Usuarios/Edit',
						success: function (data) {
							if (data) {
								VisualizarUsuario();
							}
						}
					});
				} else {
					showLoading();
					alert(data.Message,"Mensaje");
				}
			}
		});
	}
	hideLoading();
}

function GuardarCambios() {
	showLoading();
	var DocumentoJson = {
		"Id": $('input#idDocumento.form-control').val(),
		"tipodocumento": $('select#TiposDocumento01.form-control').val(),
		"descripcion": $('input#descripcion01.form-control').val()
	};
	$.ajax({
		url: '/Usuarios/GuardarCambios',
		type: "POST",
		data: { DocumentoJson: DocumentoJson },
		success: function(data) {
			if (data.result) {
				$.ajax({
					type: "POST",
					url: '/Usuarios/Visualizar',
					data: { id: $('input#IdUsuario').val() },
					success: function(data) {
						if (data) {
							$('#MostrarMensaje').html('Visualizar Usuario');
							$('#myModalUpload').modal('hide');
							$('#ListUsers').html(data);
							$('#Edit-page').html('');
							hideLoading();
						}
					}
				});
			}
		}
	});
}

function OrganismoCancelar() {
	showLoading();
	$.ajax({
		type: "POST",
		url: '/Usuarios/DesahacerOrganismos',
		success: function (data) {
			if (data) {
				hideLoading();
				$('#ListUsers').html(data);
				$('#Edit-page').html('');
				$('#MostrarMensaje').html('Mantenimiento Dependencia');

			}
		}
	});
}

function CancelarInstalacion() {
	showLoading();
	$.ajax({
		type: "POST",
		url: '/Usuarios/DesahacerInstalacion',
		success: function (data) {
			if (data) {
				hideLoading();
				$('#ListUsers').html(data);
				$('#Edit-page').html('');
				$('#MostrarMensaje').html('Mantenimiento Dependencia');

			}
		}
	});
}

function CancelarEdicion() {
	showLoading();
	$.ajax({
		type: "POST",
		url: '/Usuarios/AsignarDependencia',
		success: function (data) {
			if (data) {
				hideLoading();
				$('#ListUsers').html(data);
				$('#Edit-page').html('');
				$('#MostrarMensaje').html('Mantenimiento Dependencia');

			}
		}
	});
}

function DeshacerEdicion() {
	showLoading();
	$.ajax({
		url: '/Usuarios/DeshacerCambios',
		type: "POST",
		data: { id: $('input#idDocumento.form-control').val() },
		success: function(data) {
			if (data) {
				hideLoading();
				$('#myModalUpload').modal('show');
				$('#AsignarDocumentos').html(data);
				$('#Edit-page').html('');
			}
		}
	});
}

function AsignarUsuario() {
  
	var UsuariosEditJson = {
		"Id": $('input#id01.form-control').val(),
		"Categoria": $('select#Categoria01.form-control').val(),
		"Id_Grupo": $('select#Gruposdropdown01.form-control').val(),
		"Nombre": $('input#firstname01.form-control').val(),
		"Login": $('input#login01.form-control').val(),
		"Apellidos": $('input#lastname01.form-control').val(),
		"Email": $('input#email01.form-control').val(),
		"password_": $('input#password01.form-control').val(),
		"Observaciones": $('textarea#Observaciones01.form-control').val(),
		"fech_exp_OPIP":$('input.form-control-b.date-picker').val()
	};
	
  switch ($('select#Categoria01.form-control').val()) {
	
	   case '1':
			break;
	   case '2':
			break;
	   case '3':
			//Organismo Gestión Portuaria
		   $.ajax({
			   url: '/Usuarios/DatosUsuarios',
			   type: "POST",
			   data: JSON.stringify({ UsuariosEditJson: UsuariosEditJson }),
			   dataType: "json",
			   contentType: "application/json; charset=utf-8",
			   success: function (data) {
				   if (data.result) {
					   $.ajax({
						   type: "POST",
						   url: '/Usuarios/SeleccionarOrganismo',
						   success: function (data) {
							   if (data) {
								   $('#Edit-page-Usuario').html(data);
							   }
						   }
					   });
				   }
			   }
		   });
			break;
	   case '4':
			//Puerto
			$.ajax({
			   url: '/Usuarios/DatosUsuarios',
			   type: "POST",
			   data: JSON.stringify({ UsuariosEditJson: UsuariosEditJson }),
			   dataType: "json",
			   contentType: "application/json; charset=utf-8",
			   success: function (data) {
				   if (data.result) {
					   $.ajax({
							 type: "POST",
							 url: '/Usuarios/SeleccionarPuertos',
							 success: function (data) {
							 if (data) {
								 $('#Edit-page-Usuario').html(data);
							 }
						   }
					   });
				   }
				}
			});
			break;
	   case '5':
			//Instalación
		   $.ajax({
			   url: '/Usuarios/DatosUsuarios',
			   type: "POST",
			   data: JSON.stringify({ UsuariosEditJson: UsuariosEditJson }),
			   dataType: "json",
			   contentType: "application/json; charset=utf-8",
			   success: function (data) {
				   if (data.result) {
					   $.ajax({
						   type: "POST",
						   url: '/Usuarios/SeleccionarInstalacion',
						   success: function (data) {
							   if (data) {
								   $('#Edit-page-Usuario').html(data);
							   }
						   }
					   });
				   }
			   }
		   });
			break;
	   case '6':
			break;
	   case '7':
			break;
	}
	
}

function VisualizarUsuario() {
	if ($('input#IdUsuario').val() == "") {
		alert('Debe Seleccionar un registro');
	} else {
		$.ajax({
			type: "POST",
			url: '/Usuarios/Visualizar',
			data: { id: $('input#IdUsuario').val() },
			success: function(data) {
				if (data) {
					$('#MostrarMensaje').html('Visualizar Usuario');
					$('#myModalUpload').modal('hide');
					$('#ListUsers').html(data);
					$('#Edit-page').html('');
					hideLoading();
				}
			}
		});
	}
}
function ValidarCategoria() {
	showLoading();
	var item = $('input#Categoria01').val();
	mensajeOK("Se va a proceder a modificar la Categoría asignada al usuario esta acción llevará asociado el borrado de todas las dependencias del usuario ¿desea continuar con los cambios?", $('input#id01.form-control').val(), item);
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

	if (!ValidacionCombos(document.getElementById('Categoria01'), 'errorCategoria'))
		comboValida = false;
	if (!ValidacionCombos(document.getElementById('Gruposdropdown01'), 'errorGrupo'))
		comboValida = false;

	return comboValida;
}

  