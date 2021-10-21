var ventana;
var MenuActivo            = false;
var datosGrupos           = new Array();
var datosGruposQuitar     = new Array();
var datosPuertos          = new Array();
var datosPuertosQuitar    = new Array();
var datosOrganismos       = new Array();
var datosOrganismosQuitar = new Array();
var datosInstalaciones    = new Array();
var datosInstalacionesQuitar = new Array();
var datosPerfil           = new Array();
var datosPerfilQuitar = new Array();
$(function () {

	//Eliminar el Scroll del Navegador
		document.getElementsByTagName("html")[0].style.overflow = "hidden";

		$('a.clip-chevron-right').attr('style', 'display:none');

		$('i.clip-chevron-left').attr('style', 'display:block;font-size: 25px;');
 
		if ($('input#login.form-control').val() == '' && $('input#password.form-control.password').val() == '') {
			$('a#myLink').attr('style', 'display:none');
		} else {
			$('a#myLink').attr('style', 'display:block');
		}
	
});
function onDataBoundHijo(arg) {
	if (code.uid != undefined && code.uid > 0) {
			var posicion = 0;
			var dataItem = 0;
			var gridHijo = $('#grid_' + code.uid).data("kendoGrid");
			if (gridHijo._data.length>0) {
				for (posicion; posicion < gridHijo._data.length; posicion++) {
				if (gridHijo._data[posicion].id_Bien_Padre == code.uid) {
					dataItem = posicion;
					var uid = gridHijo.dataSource.at(dataItem).uid;
					var fila = gridHijo.tbody.find("tr[data-uid='" + uid + "']");
					fila.removeClass("k-state-selected");
				}
			}
		}
		
	}
}

function Expand(arg) {
	var grid = $('#grid_00').data("kendoGrid");
	var data = grid.dataSource.data();
	var len = data.length;
	var posicion = 0;
	var dataItem = 0;
	if (data.length > 0) {
		if (code.uid != undefined && code.uid > 0) {
			posicion = 0;
			dataItem = 0;
			var gridHijo = $('#grid_' + code.uid).data('kendoGrid');
			var dato = gridHijo._data;
			if (dato != undefined) {
				for (posicion; posicion < dato.length; posicion++) {
					if (gridHijo._data[posicion].id_Bien_Padre == code.uid) {
						dataItem = posicion;
						var _uid = gridHijo.dataSource.at(dataItem).uid;
						var reg = gridHijo.tbody.find("tr[data-uid='" + _uid + "']");
						reg.removeClass("k-state-selected");
					}
				}
			}
			for (var j = 0; j < len; j++) {
				var fila = data[j];
				if (fila.Id == code.uid) { // checks for the value of the Comment property
					grid.collapseRow("tr[data-uid='" + fila.uid + "']"); // expands the row with the specific uid
				}
			}
		} else {
			console.log("code uid es nulo");
		}
		var _grid = arg.detailRow.find("[data-role=grid]").data("kendoGrid");
		var secondGrid = $("#grid_00").data("kendoGrid");
		var padre = _grid._cellId.split('_');
		posicion = 0;
		dataItem = 0;
		for (posicion; posicion < secondGrid.dataSource._data.length; posicion++) {
			if (secondGrid.dataSource._data[posicion].Id.toString() == padre[1]) {
				dataItem = posicion;
			}
		}
		var uid = secondGrid.dataSource.at(dataItem).uid;
		secondGrid.select("tr[data-uid='" + uid + "']");
	}
 }

function onDataBound(arg) {
	if (code.uid != undefined && code.uid > 0) {
		var grid = $('#grid_00').data("kendoGrid");
		var data = grid.dataSource.data();
		var len = data.length;
		if (data.length > 0) {
			for (var i = 0; i < len; i++) {
			var row = data[i];
			if (row.Id == code.uid) { // checks for the value of the Comment property
				grid.expandRow("tr[data-uid='" + row.uid + "']"); // expands the row with the specific uid
				}
			}
		}
	}
}

function onChangeListPadre() {
	var grid = $('#grid_00').data("kendoGrid");
	var data = grid.dataSource.data();
	var len = data.length;
	//if (data.length > 0) {
	//	if (code.uid != undefined && code.uid > 0) {
	//		var posicion = 0;
	//		var dataItem = 0;
	//		var gridHijo = $('#grid_' + code.uid).data('kendoGrid');
	//		var dato = gridHijo._data;
	//		if (dato != undefined) {
	//			for (posicion; posicion < dato.length; posicion++) {
	//				if (gridHijo._data[posicion].id_Bien_Padre == code.uid) {
	//					dataItem = posicion;
	//					var uid = gridHijo.dataSource.at(dataItem).uid;
	//					var reg = gridHijo.tbody.find("tr[data-uid='" + uid + "']");
	//					reg.removeClass("k-state-selected");
	//				}
	//			}

	//			for (var j = 0; j < len; j++) {
	//				var fila = data[j];
	//				if (fila.Id == code.uid) { // checks for the value of the Comment property
	//					grid.collapseRow("tr[data-uid='" + fila.uid + "']"); // expands the row with the specific uid
	//				}
	//			}

	//		} else {
	//			console.log("grid hijo es nulo");
	//			return false;
	//		}
			
	//	}
	if (data.length > 0) {
	    code.codePadre = this.dataItem(this.select()).Id;
	    code.nombre = this.dataItem(this.select()).Descripcion;
	    $('input#SelBienPadre').val(this.dataItem(this.select()).activado);
	    $('input#SelBienhijo').val('No');
	    code.uid = this.dataItem(this.select()).Id;
	    var BienesPadre = this.dataItem(this.select()).Id;
	    //for (var i = 0; i < len; i++) {
	    //	var row = data[i];
	    //	if (row.Id == code.uid) { 
	    //		grid.expandRow("tr[data-uid='" + row.uid + "']"); 
	    //	}
	    //}
	    $.ajax({
	        url: '/Evaluaciones/AsignarEncabezado',
	        type: "POST",
	        data: { Nombre: code.nombre },
	        success: function (datos) {
	            if (datos) {
	                $('#ListEncabezado').html('');
	                $('#ListEncabezado').html(datos);
	                $.ajax({
	                    url: '/Amenazas/ListPadreHijo',
	                    type: "POST",
	                    data: { BienesPadreHijo: BienesPadre, Nombre: code.nombre, SelBienPadre:  $('input#SelBienPadre').val() },
	                    success: function (_data) {
	                        if (_data) {
	                            $('#ListAmenazas').html('');
	                            $('#ListAmenazas').html(_data);
	                            $('#ListAmenazas').attr('style', 'display:block');
	                        }
	                    }
	                });
	            }
	        }
	    });
	}
		
	//}
	return true;
}

function VolverBienPadreHijo() {
	$('#MostrarMensaje').html('Instalaciones');
	window.location.href = "/Evaluaciones/ListadoInstalaciones";
}


function RevertPadreHijo() {
	$.ajax({
		type: "POST",
		url: '/Evaluaciones/AsignarBienAmenaza',
		data: { id: null },
		success: function (data) {
			if (data) {
				hideLoading();
				$('#Edit-Instalacion').html('');
				$('#Edit-Instalacion').html(data);

			}
		}
	});
}

function SaveBienPadreHijo() {
	showLoading();
	var BienPadre = $('input#BienPadre').val();
	var QuitarPadre = $('input#QuitarPadre').val();
	$.ajax({
		type: "POST",
		url: '/Instalaciones/SaveBienPadreHijo',
		data: { BienPadre: BienPadre, QuitarPadre: QuitarPadre },
		success: function (dato) {
			if (dato.result) {
				$.ajax({
					type: "POST",
					url: '/Evaluaciones/ListaBienesAmenazas',
					data: { id: null, origen: '2' },
					success: function (data) {
						if (data) {
							hideLoading();
							$('#Edit-Instalacion').html('');
							$('#Edit-Instalacion').html(data);

						}
					}
				});
			}
		}
	});
}

function onChangeEdit(e) {
	//Seleccionamos el registro
	if (ValidacionCombos(document.getElementById('Gruposdropdown01'), 'errorGrupo')) {
		var code = this.dataItem(this.select()).id;
		var documento = this.dataItem(this.select()).documento;
		$('input#IdValor').val(code);
		$('input#NombreDocumento').val(documento);
	}
}

function onClickInstalacion() {
	datosInstalaciones = new Array();
	var y = 0;
	var a;
	var selectedItem;
	var grid = $("#GridInstalaciones").data("kendoGrid");
	var rows = grid.select();
	rows.each(function (index, row) {
		selectedItem = grid.dataItem(row);
		for (var i in selectedItem) {
			if (i == 'Id') {
				datosInstalaciones[y] = selectedItem.Id;
				y++;
			}
		}
	});
	if (datosInstalaciones.length == 0) {
		showLoading();
		alert('Debe Seleccionar la Instalación', "Mensaje");
	} else {
		console.log("Asignar==>" + datosInstalaciones);
		showLoading();
		for (a = 0; a < datosInstalaciones.length; a++) {
			var id = datosInstalaciones[a];
			if (id != null) {
				$.ajax({
					type: "POST",
					url: '/Usuarios/ActualizarInstalacion',
					data: { id: id },
					success: function (data) {
						if (data.result == false) {
							showLoading();
							alert(data.Message, 'Eliminar');
						} else {
							$('#InstaDisponibles').html(data);
							$.ajax({
								type: "POST",
								url: '/Usuarios/ActualizarInstalacionAsignada',
								success: function (data) {
									if (data.result == false) {
										showLoading();
										alert(data.Message, 'Eliminar');
									} else {
										$('#InstaAsignadas').html(data);
										hideLoading();
									}
								}
							});
						}
					}
				});
			}
		};
		console.log("Resultado OK datosInstalaciones");
		hideLoading();
	}
}

function onClickInstalacionQuitar() {
	datosInstalacionesQuitar = new Array();
	var y = 0;
	var c;
	var selectedItem;
	var grid = $("#GridInstalacionesAsociados").data("kendoGrid");
	var rows = grid.select();
	rows.each(function (index, row) {
		selectedItem = grid.dataItem(row);
		for (var i in selectedItem) {
			if (i == 'Id') {
				datosInstalacionesQuitar[y] = selectedItem.Id;
				y++;
			}
		}
	});
	if (datosInstalacionesQuitar.length == 0) {
		showLoading();
		alert('Debe Seleccionar la Instalación', "Mensaje");
	} else {
		showLoading();
		for (c = 0; c < datosInstalacionesQuitar.length; c++) {
			var id = datosInstalacionesQuitar[c];
			if (id != null) {
				$.ajax({
					type: "POST",
					url: '/Usuarios/QuitarInstalacion',
					data: { id: id },
					success: function (data) {
						if (data.result == false) {
							showLoading();
							alert(data.Message, 'Eliminar');
						} else {
							$('#InstaDisponibles').html(data);
							$.ajax({
								type: "POST",
								url: '/Usuarios/ActualizarInstalacionAsignada',
								success: function (data) {
									if (data.result == false) {
										showLoading();
										alert(data.Message, 'Eliminar');
									} else {
										$('#InstaAsignadas').html(data);
										hideLoading();
									}
								}
							});
						}
					}
				});
			}
		};
		console.log("Resultado OK datosInstalacionesQuitar");
		hideLoading();
	}
	
}

function onClickPerfil() {
	datosPerfil = new Array();
	var y = 0;
	var e;
	var selectedItem;
	var grid = $("#GridAcciones").data("kendoGrid");
	var rows = grid.select();
	rows.each(function (index, row) {
		selectedItem = grid.dataItem(row);
		for (var i in selectedItem) {
			if (i == 'Id') {
				datosPerfil[y] = selectedItem.Id;
				y++;
			}
		}
	});
	if (datosPerfil.length == 0) {
		showLoading();
		alert('Debe Seleccionar la Acción', "Mensaje");
	} else {
		console.log("Asignar==>" + datosPerfil);
		showLoading();
		var idconjunto = $('input#IdConjunto').val();
		for (e = 0; e < datosPerfil.length; e++) {
			var id = datosPerfil[e];
			if (id != null) {
				$.ajax({
					type: "POST",
					url: '/Perfiles/AgregarAccion',
					data: { id: id, idconjunto: idconjunto },
					success: function (data) {
						if (data) {
							$.ajax({
								type: "POST",
								url: '/Perfiles/RefrescarAccionAlta',
								data: { id: id },
								success: function (data) {
									if (data) {
										$('#ListAccion').html(data);
										$.ajax({
											type: "POST",
											url: '/Perfiles/RefrescarAccion',
											success: function (data) {
												if (data) {
													$('#ListAccionDisponible').html(data);
												} else {
													setTimeout(function () {
														alert(data.Message, "Mensaje");
													}, 1000);
												}
											}
										});
									}
								}
							});
						} else {
							setTimeout(function () {
								alert(data.Message, "Mensaje");
							}, 1000);
						}
					}
				});
			}
		};
		console.log("Resultado OK Acciones");
		hideLoading();
	}
}

function onClickPerfilQuitar() {
	datosPerfilQuitar = new Array();
	var y = 0;
	var h;
	var selectedItem;
	var grid = $("#GridPermisosAsignados").data("kendoGrid");
	var rows = grid.select();
	rows.each(function (index, row) {
		selectedItem = grid.dataItem(row);
		for (var i in selectedItem) {
			if (i == 'Id') {
				datosPerfilQuitar[y] = selectedItem.Id;
				y++;
			}
		}
	});
	if (datosPerfilQuitar.length == 0) {
		showLoading();
		alert('Debe Seleccionar la Acción', "Mensaje");
	} else {
		console.log("Asignar==>" + datosPerfilQuitar);
		showLoading();
		var idconjunto = $('input#IdConjunto').val();
		for (h = 0; h < datosPerfilQuitar.length; h++) {
			var id = datosPerfilQuitar[h];
			if (id != null) {
				$.ajax({
					type: "POST",
					url: '/Perfiles/RemoverAccion',
					data: { id: id, idconjunto: idconjunto },
					success: function (data) {
						if (data) {
							$.ajax({
								type: "POST",
								url: '/Perfiles/RefrescarAccionAlta',
								data: { id: id },
								success: function (data) {
									if (data) {
										$('#ListAccion').html(data);
										$.ajax({
											type: "POST",
											url: '/Perfiles/RefrescarAccion',
											success: function (data) {
												if (data) {
													$('#ListAccionDisponible').html(data);
												} else {
													setTimeout(function () {
														alert(data.Message, "Mensaje");
													}, 1000);
												}
											}
										});
									}
								}
							});
						} else {
							setTimeout(function () {
								alert(data.Message);
							}, 1000);
						}
					}
				});
			}
		};
		console.log("Resultado OK Acciones");
		hideLoading();
	}
}

function onClickOrganismosQuitar() {
	datosOrganismosQuitar = new Array();
	var b;
	var y = 0;
	var selectedItem;
	var grid = $("#GridOrganismosAsociados").data("kendoGrid");
	var rows = grid.select();
	rows.each(function (index, row) {
		selectedItem = grid.dataItem(row);
		for (var i in selectedItem) {
			if (i == 'Id') {
				datosOrganismosQuitar[y] = selectedItem.Id;
				y++;
			}
		}
	});
	if (datosOrganismosQuitar.length == 0) {
		showLoading();
		alert('Debe Seleccionar el Organismo', "Mensaje");
	} else {
		console.log("Desasignar==>" + datosOrganismosQuitar);
		showLoading();
		for (b = 0; b < datosOrganismosQuitar.length; b++) {
			var id = datosOrganismosQuitar[b];
			if (id != null) {
				$.ajax({
					type: "POST",
					url: '/Usuarios/DesasignarOrganismos',
					data: { id: id },
					success: function (data) {
						if (data) {
							$('#ListPuertos').html(data);
							$.ajax({
								type: "POST",
								url: '/Usuarios/RefrescarOrganismosAsignados',
								success: function (data) {
									if (data) {
										$('#PuertosAsignados').html(data);
									} else {
										console.log("onClickPuertoQuitarError01");
										setTimeout(function () {
											alert(data.Message);
										}, 100);
									}
								}
							});
						} else {
							console.log("onClickPuertoQuitarError02");
							setTimeout(function () {
								alert(data.Message);
							}, 1000);
						}
					}
				});
			}
		};
		console.log("Resultado OK datosOrganismosQuitar");
		hideLoading();
	}
}

function onClickOrganismo() {
	datosOrganismos = new Array();
	var y = 0;
	var a;
	var selectedItem;
	var grid = $("#GridOrganismos").data("kendoGrid");
	var rows = grid.select();
	rows.each(function (index, row) {
		selectedItem = grid.dataItem(row);
		for (var i in selectedItem) {
			if (i == 'Id') {
				datosOrganismos[y] = selectedItem.Id;
				y++;
			}
		}
	});
	if (datosOrganismos.length == 0) {
		showLoading();
		alert('Debe Seleccionar el Organismo', "Mensaje");
	} else {
		console.log("Asignar==>" + datosOrganismos);
		showLoading();
		for (a = 0; a < datosOrganismos.length; a++) {
			var id = datosOrganismos[a];
			if (id != null) {
				$.ajax({
					type: "POST",
					url: '/Usuarios/AsignarOrganismos',
					data: { id: id },
					success: function (data) {
						if (data) {
							$('#ListPuertos').html(data);
							$.ajax({
								type: "POST",
								url: '/Usuarios/RefrescarOrganismosAsignados',
								success: function (data) {
									if (data) {
										$('#ListOrganismosDisponible').html(data);
									} else {
										console.log("onClickOrganismosError01");
										setTimeout(function () {
											alert(data.Message);
										}, 100);
									}
								}
							});
						} else {
							console.log("onClickOrganismosError02");
							setTimeout(function () {
								alert(data.Message);
							}, 1000);
						}
					}
				});
			}
		};
		console.log("Resultado OK Organismos");
		hideLoading();
	}
}

function onClickPuerto()
{
	datosPuertos = new Array();
	var y = 0;
	var a;
	var selectedItem;
	var grid = $("#GridPuertos").data("kendoGrid");
	var rows = grid.select();
	rows.each(function (index, row) {
		selectedItem = grid.dataItem(row);
		for (var i in selectedItem) {
			if (i == 'Id') {
				datosPuertos[y] = selectedItem.Id;
				y++;
			}
		}
	});
	if (datosPuertos.length == 0) {
		showLoading();
		alert('Debe Seleccionar el Puerto', "Mensaje");
	} else {
		console.log("Asignar==>" + datosPuertos);
		showLoading();
		for (a = 0; a < datosPuertos.length; a++) {
			var id = datosPuertos[a];
			if (id != null) {
				$.ajax({
					type: "POST",
					url: '/Usuarios/AsignarPuertos',
					data: { id: id },
					success: function (data) {
						if (data) {
							$('#ListPuertos').html(data);
							$.ajax({
								type: "POST",
								url: '/Usuarios/RefrescarPuertosAsignados',
								success: function (data) {
									if (data) {
										$('#PuertosAsignados').html(data);
									} else {
										console.log("onClickPuertoError01");
										setTimeout(function () {
											alert(data.Message);
										}, 100);
									}
								}
							});
						} else {
							console.log("onClickPuertoError02");
							setTimeout(function () {
								alert(data.Message);
							}, 1000);
						}
					}
				});
			}
		};
		console.log("Resultado OK Puerto");
		hideLoading();
	}
}

function onClickPuertoQuitar() {
	datosPuertosQuitar = new Array();
	var b;
	var y = 0;
	var selectedItem;
	var grid = $("#GridPuertosAsociados").data("kendoGrid");
	var rows = grid.select();
	rows.each(function (index, row) {
		selectedItem = grid.dataItem(row);
		for (var i in selectedItem) {
			if (i == 'Id') {
				datosPuertosQuitar[y] = selectedItem.Id;
				y++;
			}
		}
	});
	if (datosPuertosQuitar.length == 0) {
		showLoading();
		alert('Debe Seleccionar el Puerto', "Mensaje");
	} else {
		console.log("Desasignar==>" + datosPuertosQuitar);
		showLoading();
		for (b = 0; b < datosPuertosQuitar.length; b++) {
			var id = datosPuertosQuitar[b];
			if (id != null) {
				$.ajax({
					type: "POST",
					url: '/Usuarios/DesasignarPuertos',
					data: { id: id },
					success: function (data) {
						if (data) {
							$('#ListPuertos').html(data);
							$.ajax({
								type: "POST",
								url: '/Usuarios/RefrescarPuertosAsignados',
								success: function (data) {
									if (data) {
										$('#PuertosAsignados').html(data);
									} else {
										console.log("onClickPuertoQuitarError01");
										setTimeout(function () {
											alert(data.Message);
										}, 100);
									}
								}
							});
						} else {
							console.log("onClickPuertoQuitarError02");
							setTimeout(function () {
								alert(data.Message);
							}, 1000);
						}
					}
				});
			}
		};
		console.log("Resultado OK PuertoQuitar");
		hideLoading();
	}
}

function onClickGrupos() {
	datosGrupos = new Array();
	var y = 0;
	var a ;
	var selectedItem;
	var grid = $("#GridAltaPerfiles").data("kendoGrid");
	var rows = grid.select();
	rows.each(function(index, row) {
		 selectedItem = grid.dataItem(row);
		 for (var i in selectedItem) {
			 if (i == 'Id') {
				 datosGrupos[y] = selectedItem.Id;
				 y++;
			 }
		 }
	});
	if (datosGrupos.length == 0) {
		showLoading();
		alert('Debe Seleccionar el Perfil', "Mensaje");
	} else {
		console.log("Asignar==>" + datosGrupos);
		showLoading();
		for (a = 0; a < datosGrupos.length; a++) {
			var id = datosGrupos[a];
			if (id != null) {
				$.ajax({
					type: "POST",
					url: '/Grupos/AgregarPerfil',
					data: { id: id },
					success: function (data) {
						if (data) {
							$('#ListPerfilesRefrescar').html(data);
							$.ajax({
								type: "POST",
								url: '/Grupos/RefrescarPerfilDisponible',
								success: function (data) {
									if (data) {
										$('#ListPerfilesDisponible').html(data);
										$.ajax({
											type: "POST",
											url: '/Grupos/RefrescarPerfilDisponibles',
											success: function (data) {
												if (data) {
													$('#ListPerfilesRefrescar').html(data);
												} else {
													console.log("onClickGruposError03");
													setTimeout(function () {
														alert(data.Message, "Mensaje");
													}, 100);
												}
											}
										});
									} else {
										console.log("onClickGruposError02");
										setTimeout(function () {
											alert(data.Message, "Mensaje");
										}, 100);
									}
								}
							});
						} else {
							console.log("onClickGruposError01");
							setTimeout(function () {
								alert(data.Message, "Mensaje");
							}, 1000);
						}
					}
				});
			}
		};
		console.log("Resultado OK");
		hideLoading();
	}
}

function onClickGruposQuitar() {
	datosGruposQuitar = new Array();
	var b;
	var y = 0;
	var selectedItem;
	var grid = $("#GridQuitarPerfiles").data("kendoGrid");
	var rows = grid.select();
	rows.each(function(index, row) {
		selectedItem = grid.dataItem(row);
		for (var i in selectedItem) {
			if (i == 'Id') {
				datosGruposQuitar[y] = selectedItem.Id;
				y++;
			}
		}
	});
	if (datosGruposQuitar.length == 0) {
		showLoading();
		alert('Debe Seleccionar el Perfil', "Mensaje");
	} else {
		console.log("Desasignar==>" + datosGruposQuitar);
		for (b = 0; b < datosGruposQuitar.length; b++) {
			var id = datosGruposQuitar[b];
			if (id != null) {
				showLoading();
				$.ajax({
					type: "POST",
					url: '/Grupos/RemoverPerfil',
					data: { id: id },
					success: function (data) {
						if (data) {
							$('#ListPerfilesRefrescar').html(data);
							$.ajax({
								type: "POST",
								url: '/Grupos/RefrescarPerfilDisponible',
								success: function (data) {
									if (data) {
										$('#ListPerfilesDisponible').html(data);
										$.ajax({
											type: "POST",
											url: '/Grupos/RefrescarPerfilDisponibles',
											success: function (data) {
												if (data) {
													$('#ListPerfilesRefrescar').html(data);
												} else {
													console.log("onClickGruposQuitarError02");
													setTimeout(function () {
														alert(data.Message, "Mensaje");
													}, 100);
												}
											}
										});
									} else {
										console.log("onClickGruposQuitarError03");
										setTimeout(function () {
											alert(data.Message, "Mensaje");
										}, 100);
									}
								}
							});
						} else {
							console.log("onClickGruposQuitarError01");
							setTimeout(function () {
								alert(data.Message, "Mensaje");
							}, 1000);
						}
					}
				});
			}
		}
		console.log("Resultado OK");
		hideLoading();
	}
	
}

function onSelectGrupo(e) {
	if(ValidacionCombos(document.getElementById('Gruposdropdown01'), 'errorGrupo'))
		var dataItem = this.dataItem(e.item.index());
		//alert("event :: select (" + dataItem.Text + " : " + dataItem.Value + ")");
	}

function onChangeOrganismo() {
	var id = this.dataItem(this.select()).id;
	var Nombre = this.dataItem(this.select()).Organismo;
	$('input#IdValor').val(id);
	$('input#IdNombre').val(Nombre);
}

function onChangeContacto() {
	var id = this.dataItem(this.select()).Id;
	var Nombre = this.dataItem(this.select()).Nombre;
	$('input#IdValor').val(id);
	$('input#IdNombre').val(Nombre);
}

function onChangePermisos(arg) {

		var code = this.dataItem(this.select()).Id;
		$('input#IdAcciones').val(code);
		var conjunto = this.dataItem(this.select()).idConjunto;
		$('input#IdConjunto').val(conjunto);
	}

function RefrescarAccionDisponible(id, grupo) {
		$.ajax({
			type: "POST",
			url: '/Perfiles/RefrescarAccionDisponible',
			data: { id: id, grupo: grupo },
			success: function(data) {
				if (data) {
					$('#ListAccionDisponible').html('');
					$('#ListAccionDisponible').html(data);
					$.ajax({
						type: "POST",
						url: '/Perfiles/RefrescarAsignada',
						success: function(data) {
							if (data) {
								$('#ListAccion').html('');
								$('#ListAccion').html(data);
							} else {
								setTimeout(function() {
									alert(data.Message);
								}, 1000);
							}
						}
					});
				} else {
					setTimeout(function() {
						alert(data.Message);
					}, 1000);
				}
			}
		});

	}

function onChangeIdPermisos(arg) {
		var id = this.dataItem(this.select()).Id;
		var grupo = this.dataItem(this.select()).Nombre;
		$('input#IdPermisos').val(id);
		RefrescarAccionDisponible(id, grupo);
	}

function onChangeIdOrganismo(arg) {
		var code = this.dataItem(this.select()).Id;
		$('input#IdOrganismo').val(code);
	}

function onChangePuerto(arg) {
		var code = this.dataItem(this.select()).Id;
		$('input#IdPuerto').val(code);
		$.ajax({
			type: "POST",
			url: '/Usuarios/ActualizarPuertos',
			data: { id: $('input#IdPuerto').val() },
			success: function(data) {
				if (data.result == false) {
					showLoading();
					alert(data.Message, 'Eliminar');
				} else {
					$('#InstaDisponibles').html(data);
					hideLoading();
					$.ajax({
						type: "POST",
						url: '/Usuarios/ActualizarInstalacionAsignada',
						success: function (data) {
							if (data.result == false) {
								showLoading();
								alert(data.Message, 'Eliminar');
							} else {
								$('#InstaAsignadas').html(data);
								hideLoading();
							}
						}
					});
				}
			}
		});
	}

function onChangeIdInstalacion(arg) {
		var code = this.dataItem(this.select()).Id;
		$('input#IdInstalacion').val(code);
	}

function PlantillaVisualizarAccionesChange() {
		var value = this.value();
		if (value == '1') {
			AsignarDependencia();
		} else if (value == '2') {
			Reiniciar();
		}
	}

function onClick(e) {
		if ($(e.event.target).closest(".k-button").attr("id") == "ButtonVolver") {
			VisualizarUsuario();
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonGuardar") {
			GuardarCambios();
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonDeshacer") {
			DeshacerEdicion();
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonDeshacerDoc") {
			UsuarioEditar();
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonGuardarDoc") {
			if (ValidarAllCombos("1"))
				$('input#ValidarAllCombos').val("1");
			else
				$('input#ValidarAllCombos').val("");
			GuardarUsuario(12);
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonVolverDoc") {
			window.location.href = "/Usuarios/ListadoUsuarios";
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonDeshacerInst") {
			CancelarInstalacion();
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonDeshacerDep") {
			CancelarEdicion();
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonDeshacerOrg") {
			OrganismoCancelar();
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonGuardarDep") {
			GuardarDependencia(14);
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonGuardarInst") {
			GuardarDependencia(17);
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonGuardarOrg") {
			GuardarDependencia(16);
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonGuardarPerfil") {
			RegisterPerfiles();
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonVolverPerfil") {
			VolverPerfil();
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonGuardarGrupo") {
			RegisterGrupos();
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonVolverGrupo") {
			VolverGrupo();
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonDeshacerGrupo") {
			DeshacerGrupos();
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonVolverGrupo") {
			window.location.href = "/Grupos/ListadoGrupos";
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonEditGrupo") {
			if (ValidarAllCombos("1"))
				$('input#ValidarAllCombos').val("1");
			else
				$('input#ValidarAllCombos').val("");
			RegistrarGrupos();
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonVolverPerfilEdit") {
			window.location.href = "/Perfiles/ListadoPerfiles";
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonDeshacerPerfiles") {
			DeshacerAcciones();
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonEditPerfil") {
			if (ValidarAllCombos())
				$('input#ValidarAllCombos').val("1");
			else
				$('input#ValidarAllCombos').val("");
			RegistrarPerfiles();
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonGuardarDocComite") {
			GuardarCambios();
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonDeshacerDocComite") {
			DeshacerEdicion();
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonVolverDocComite") {
			VolverComiteDoc();
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonGuardarDocOperador") {
			GuardarCambios();
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonDeshacerDocOperador") {
			DeshacerEdicion();
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonVolverDocOperador") {
			VolverOperadorDoc();
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonGuardarDocFormacion") {
			GuardarCambios();
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonDeshacerDocFormacion") {
			DeshacerEdicion();
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonGuardarDocDeclara") {
			GuardarCambios();
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonVolverDocDeclara") {
			VolverDocDeclaraMaritima();
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonVolverDocFormacion") {
			VolverDocFormacion();
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonGuardarDocPractica") {
			GuardarCambios();
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonDeshacerDocPractica") {
			DeshacerEdicion();
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonVolverDocPractica") {
			VolverDocPractica();
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonGuardarDocIncidente") {
			GuardarCambios();
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonDeshacerDocIncidente") {
			DeshacerEdicion();
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonVolverDocIncidente") {
			VolverDocIncidente();
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonGuardarDocAuditoria") {
			GuardarCambios();
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonDeshacerDocAuditoria") {
			DeshacerEdicion();
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonVolverDocAuditoria") {
			VolverDocAuditoria();
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonGuardarDocNivel") {
			GuardarCambios();
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonDeshacerDocNivel") {
			DeshacerEdicion();
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonVolverDocNivel") {
			VolverDocNivel();
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonGuardarNivelInst") {
			GuardarNivelInst();
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonDeshacerNivelInst") {
			DeshacerNivelInst();
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonVolverNivelInst") {
			VolverNivelInst();
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonGuardarDocMantenimiento") {
			GuardarCambios();
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonDeshacerDocMantenimiento") {
			DeshacerEdicion();
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonVolverDocMantenimiento") {
			VolverDocMantenimiento();
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonGuardarDocComunicacion") {
			GuardarCambios();
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonDeshacerDocComunicacion") {
			DeshacerEdicion();
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonVolverDocComunicacion") {
			VolverDocComunicacion();
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonGuardarIncidenteInst") {
			GuardarIncidenteInst();
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonDeshacerIncidenteInst") {
			DeshacerIncidenteInst();
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonVolverIncidenteInst") {
			VolverIncidenteInst();
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonGuardarPracticaInst") {
			GuardarPracticaInst();
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonDeshacerPracticaInst") {
			DeshacerPracticaInst();
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonVolverPracticaInst") {
			VolverPracticaInst();
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonGuardarDocProcedimiento") {
			GuardarCambios();
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonDeshacerDocProcedimiento") {
			DeshacerEdicion();
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonVolverDocProcedimiento") {
			VolverDocProcedimiento();
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonSaveBienPadreHijo") {
			SaveBienPadreHijo();
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonVolverBienPadreHijo") {
			VolverBienPadreHijo();
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonDeshacerPadreHijo") {
			RevertPadreHijo();
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonGuardarDocEvaluacion") {
			GuardarCambios();
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonDeshacerDocEvaluacion") {
			DeshacerEdicion();
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonVolverDocEvaluacion") {
			VolverDocEvaluacion();
		}
		else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonSaveAmenazasIns") {
			GuardarmatrizEvaluacion();
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonSaveAmenazasBajoRiesgo") {
			GuardarmatrizEvaluacionBajoRiesgo();
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonDeshacerAmenazasIns") {
			DeshacerAmenazas();
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonVolverIns") {
			volverAmenazasIns();
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonCalcularRiesgos") {
			CalcularRiesgosPanel();
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonCalcularRiesgosBajo") {
			CalcularRiesgosBajo();
		} else if ($(e.event.target).closest(".k-button").attr("id") == "ButtonDeshacerAmenazasBajoRiesgo") {
		    DeshacerAmenazas();
		}


}

function validarEmail(email) {
		var expr = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
		if (!expr.test(email)) {
			$('#myModalCambiarPassword').modal('hide');
			alert("Error: La dirección de correo: " + email + " es incorrecta.", "LoginPerfil");
		}
	}

function AvisoLegal() {
		$("#IdContainer").html('');
		$("#AvisoLegal").html('');
		$.ajax({
			type: "POST",
			url: "/Menu/Legal",
			success: function(data) {
				if (data) {
					$("#AvisoLegal").html('');
					$("#AvisoLegal").html(data);
					$("div.k-overlay").removeAttr("style");
				}
			}
		});
	}

function AvisoLegalInicio() {
	$("#AvisoLegalInicio").html('');
	$.ajax({
		type: "POST",
		url: "/Menu/LegalInicio",
		success: function (data) {
			if (data) {
				$("#AvisoLegalInicio").html('');
				$("#AvisoLegalInicio").html(data);
				$("div.k-overlay").removeAttr("style");
			}
		}
	});
	hideLoading();
}

function GuardarCorreo() {
		var CorreoJson = {
			"correoactual": '',
			"correo": $('input#correo03.form-control').val(),
			"correoagain": $('input#correoagain03.form-control').val(),
		};
		var o = eval(CorreoJson);
		$.ajax({
			type: "POST",
			url: "/Login/CambiarCorreo",
			data: { correo: o.correo },
			success: function(data) {
				if (data.result) {
					$('input#correoagain03.form-control').blur();
					$('input#correo03.form-control').blur();
					$('#Idloading').attr('style', 'display:none');
					Guardarpassword();
				} else {
					$('#myModalCambiarPassword').modal('hide');
					$('#myModalValidarPassword').modal('hide');
					setTimeout(function() {
						showLoading();
						alert(data.Message, "LoginPerfil");
					}, 3000);

				}
			}
		});

	}

function GuardarCambio() {
		$.ajax({
			type: "POST",
			url: "/Login/ValidarCambiologin",
			data: { password: $('input#contrasena').val() },
			success: function(data) {
				if (data.result) {
					if (GuardarCorreo() == false) {
						$('#myModalCambiarPassword').modal('hide');
						$('#myModalValidarCorreoPassword').modal('hide');
						$('#myModalValidarPassword').modal('hide');
						alert("Error Guardando datos.", "LoginPerfil");
					} else {
						showLoading();
						$('#myModalCambiarPassword').modal('hide');
						$('#myModalValidarCorreoPassword').modal('hide');
						$('#myModalValidarPassword').modal('hide');
						Guardarpassword();
						location.reload();
					}
				} else {
					showLoading();
					$('#myModalCambiarPassword').modal('hide');
					$('#myModalValidarCorreoPassword').modal('hide');
					$('#myModalValidarPassword').modal('hide');
					alert(data.Message, "LoginPerfil");
				}
			}
		});
	}

function Guardarpassword() {
		var CambiarLoginJson = {
			"Passwordactual": $('input#password04.form-control').val(),
			"Password": $('input#password03.form-control').val(),
			"Passwordagain": $('input#passwordagain03.form-control').val(),
		};
		var o = eval(CambiarLoginJson);
		showLoading();
		$.ajax({
			type: "POST",
			url: "/Login/Cambiarlogin",
			data: { password: o.Password },
			success: function(data) {
				if (data.result) {
					return true;
				} else {
					alert(data.Message, "LoginPerfil");
				}
			}
		});

	}

function CorreoAtras() {
		showLoading();
		$('#myModalValidarCorreoPassword').modal('hide');
		$('#myModalCambiarPassword').modal('show');
	}

function LimpiarControles() {
		$('div#password03.form-group').removeClass('has-error').addClass('has-success');
		$('div#Passwordagain033.form-group').removeClass('has-error').addClass('has-success');
		$('div#Password08.form-group').removeClass('has-error').addClass('has-success');
		$('div#correo06.form-group').removeClass('has-error').addClass('has-success');
		$('div#correoagain06.form-group').removeClass('has-error').addClass('has-success');
		$('span#Spancorreo06.help-block').attr('style', 'display:none');
		$('span#Spancorreoagain05.help-block').attr('style', 'display:none');
		$('span#Spancorreoagain06.help-block').attr('style', 'display:none');
		$('span#SpanPasswordagain033.help-block').attr('style', 'display:none');
		$('span#SpanPasswordagain05.help-block').attr('style', 'display:none');
		$('span#Spanpassword03.help-block').attr('style', 'display:none');
		$('span#Spanpassword09.help-block').attr('style', 'display:none');
		$('span#Spanpassword10.help-block').attr('style', 'display:none');
		$('span#Spanpassword11.help-block').attr('style', 'display:none');
	}

function VolverAtras() {

		LimpiarControles();
		location.reload();
	}

function Volver() {
		LimpiarControles();
		showLoading();
		$('#myModalValidarPassword').modal('hide');
		$('#myModalValidarCorreoPassword').modal('hide');
		$('#myModalCambiarPassword').modal('show');
	}

function ValidarAviso(item) {
		if (rtrim($(item).val()) == "") {
			$('a#myLink').attr('style', 'display:none');
		} else {
			$('a#myLink').attr('style', 'display:block');
		}
	}

function closeWindows() {
		if (window.ventana) {
			ventana.close();
		}
	}

function AceptarCondiciones() {
		$('div#myModalCondiciones.modal.container.fade.in').attr('style', 'display:none');
		$.ajax({
			type: "POST",
			url: "/Login/AceptarAvisoLegal",
			data: { email: $('input#login.form-control').val() },
			success: function(data) {
				if (data.result) {
					location.reload();
				} else {
					window.location.href = "/";
				}
			}
		});
	}

function CerrarCondiciones() {
		$('div#myModal.modal.container.fade.in').attr('style', 'display:none');
		$('div#myDocumento.modal.container.fade.in').attr('style', 'display:none');
		hideLoading();
	}

function CerrarModalCondiciones() {
		$('div#myModalCondiciones.modal.container.fade.in').attr('style', 'display:none');
		hideLoading();
		$.ajax({
			url: '/Login/LogOff',
			type: "POST",
			dataType: "json",
			contentType: "application/json; charset=utf-8",
			success: function(data) {
				if (data.result) {
					window.location.href = "/";
				}
			}
		});
	}

function ltrim(str, filter) {
		filter || (filter = '\\s|\\&nbsp;');
		var pattern = new RegExp('^(' + filter + ')*', 'g');
		return str.replace(pattern, "");
	}

function rtrim(str, filter) {
		filter || (filter = '\\s|\\&nbsp;');
		var pattern = new RegExp('(' + filter + ')*$', 'g');
		return str.replace(pattern, "");
	}

function trim(str, filter) {
		filter || (filter = '\\s|\\&nbsp;');
		return ltrim(rtrim(str, filter), filter);
	}

function showLoading() {
		$('#loading-page').show();
		$('.lightbox.white').fadeIn();
		$('.lightbox.white').css('position', 'fixed');

	}

function Loading() {
		$('#loading').show();
		$('.lightbox.white').fadeIn();
		$('.lightbox.white').css('position', 'fixed');

	}

function hideLoading() {
		$('.lightbox.white').fadeOut();
		$('#loading-page').hide();
	}

function MenuPrincipal() {
		window.location.href = "/Menu";
	}

function LogOff() {
		showLoading();
		alert('¿Esta seguro de cerrar sesión?', "Sesion");
	}

function LogOn() {
		window.location.href = "/SecurePort";
	}

function Alta() {
		$('#myModalAgregar').modal('show');
	}

function CambiarPassword() {
		//CAMBIO DE CONTRASEÑA
		$('#myModalCambiarPassword.modal.fade.in').attr('style', "width: 500px;margin-left: -249px;margin-top: -186px;height: 50%;");
		$('#myModalCambiarPassword').modal("show");

	}

function ValidarCorreo() {
		if ($('input#correo03.form-control').val() != "" && $('input#correoagain03.form-control').val() != "") {
			if ($('input#correo03.form-control').val() != $('input#correoagain03.form-control').val()) {
				$('div#correoagain06.form-group').removeClass('has-success').addClass('has-error');
				$('span#Spancorreoagain05.help-block').attr('style', 'display:block');
			} else {
				$('div#correoagain06.form-group').removeClass('has-error').addClass('has-success');
				$('span#Spancorreoagain05.help-block').attr('style', 'display:none');
			}
			if ($('span#Spancorreoagain05.help-block').attr('style') == 'display:none') {
				if ($('span#SpanPasswordagain05.help-block').attr('style') == 'display:none') {
					$('#myModalValidarPassword').modal('show');
				}
			}
		}
	}

function ValidarPassword() {
		$('div#Password08.form-group').removeClass('has-error').addClass('has-success');
		$('span#Spanpassword11.help-block').attr('style', 'display:none');

		if ($('input#password03.form-control').val() != "" && $('input#passwordagain033.form-control').val() != "") {
			ValidarDistintasPassword();
		}

	}

function ValidarDistintasPassword() {
		// Si las password nueva y password again son distintas  
		if ($('input#password03.form-control').val() != $('input#passwordagain033.form-control').val()) {
			$('div#Passwordagain033.form-group').removeClass('has-success').addClass('has-error');
			$('span#SpanPasswordagain05.help-block').attr('style', 'display:block');

		} else {
			ValidarLongitud();
		}

		return true;
	}

function IntroducirNuevaPassword() {

		if ($('span#Spancorreoagain05.help-block').attr('style') == 'display:none') {
			$('#myModalValidarPassword').modal('show');

		}
		return true;
	}

function ValidarLongitud() {
		// La longitud de la password debe tener al menos 8 caracteres
		$('div#Passwordagain033.form-group').removeClass('has-error').addClass('has-success');
		$('span#SpanPasswordagain05.help-block').attr('style', 'display:none');

		if ($('input#password03.form-control').val().length < 8) {
			$('div#password03.form-group').removeClass('has-success').addClass('has-error');
			$('span#Spanpassword09.help-block').attr('style', 'display:block');
		} else {
			ValidarCaracteresNumericos();
		}
		return true;
	}

function ValidarCaracteresNumericos() {

		$('div#password03.form-group').removeClass('has-error').addClass('has-success');
		$('span#Spanpassword09.help-block').attr('style', 'display:none');

		$permitidos = "0123456789";
		$cadena = ($('input#password03.form-control').val());
		$longitud = ($('input#password03.form-control').val().length);
		$encontrado = "0";

		for ($i = 0; $i < $longitud; $i++) {
			$data = $cadena.substr($i, 1);
			if ($encontrado == "0") {
				for ($j = 0; $j < 10; $j++) {
					if ($permitidos.substr($j, 1) == $data) {
						$encontrado = "1";
						IntroducirNuevaPassword();
						return false;
					}
				}
			}
		}
		if ($encontrado == "0") {
			$('div#password03.form-group').removeClass('has-success').addClass('has-error');
			$('span#Spanpassword10.help-block').attr('style', 'display:block');
		}
		return true;
	}

function ValidarContrasena() {
		if ($('input#contrasena').val() == '') {
			$('div#Password08.form-group').removeClass('has-success').addClass('has-error');
			$('span#SpanPassword08.help-block').attr('style', 'display:block');
		} else {
			ValidarIgualNuevaAntigua();
		}
	}

function ValidarIgualNuevaAntigua() {
		var contraseñanueva = ($('input#contrasena').val());
		var contraseñaantigua = ($('input#password03.form-control').val());

		if (contraseñanueva == contraseñaantigua) {
			$('div#Password08.form-group').removeClass('has-success').addClass('has-error');
			$('span#SpanPassword11.help-block').attr('style', 'display:block');
		} else {
			CambiarPassword_Nueva();
		}
		return true;
	}

function CambiarPassword_Nueva() {
		if ($('input#contrasena').val() != '') {
			var CorreoJson = {
				"correoactual": '',
				"correo": $('input#correo03.form-control').val(),
				"correoagain": $('input#correoagain03.form-control').val(),
			};
			var o = eval(CorreoJson);
			$.ajax({
				type: "POST",
				url: "/Login/ExisteCorreo",
				data: { correo: o.correo },
				success: function(data) {
					if (data.result) {
						GuardarCambio();
					} else {
						$('#myModalCambiarPassword').modal('hide');
						$('#myModalValidarPassword').modal('hide');
						showLoading();
						setTimeout(function() {
							alert(data.Message, "LoginPerfil");
						}, 1000);

					}
				}
			});
		}

	}

function onClose() {
	$("#AvisoLegal").html('');
}

function onToggleContacto(e) {
	if (e.id == "toggle51") {
		//ALTA CONTACTO
		showLoading();
		$.ajax({
			type: "POST",
			url: '/Organismo/CreateContactos',
			success: function (data) {
				if (data) {
					showLoading();
					$('#DatosContactos').html('');
					$('#DatosContactos').html(data);
					$('div#myModalContactos').modal('show');
					//hideLoading();
					//$('div#myModalContactos.modal.fade.in').attr('style', 'display:block;top:3%');
				}
			}
		});
	} else if (e.id == "toggle52") {
		//EDITAR CONTACTO
		if ($('input#IdValor').val() == "" || $('input#IdValor').val() == undefined) {
			alert('Debe Seleccionar un registro');
		} else {
			showLoading();
			$.ajax({
				type: "POST",
				url: '/Organismo/Edit',
				data: { id: $('input#IdValor').val(), Accion:"Edit" },
				success: function(data) {
					if (data) {
						showLoading();
						$('#EditContactos').html('');
						$('#EditContactos').html(data);
						$('div#myEditModalContactos').modal('show');
						//hideLoading();
						//$('div#myEditModalContactos.modal.fade.in').attr('style', 'display:block;top:3%');
					}
				}
			});
		}
	} else if (e.id == "toggle53") {
		//BORRAR CONTACTO
		if ($('input#IdValor').val() == "" || $('input#IdValor').val() == undefined) {
			alert('Debe Seleccionar un registro');
		} else {
			showLoading();
			AvisoOKContacto("¿Esta Seguro de Borrar el Contacto: " + $('input#IdNombre').val() + "?", $('input#IdValor').val());
		}
	} else if (e.id == "toggle54") {
		//VISUALIZAR CONTACTO
		if ($('input#IdValor').val() == "" || $('input#IdValor').val() == undefined) {
			alert('Debe Seleccionar un registro');
		} else {
			showLoading();
			$.ajax({
				type: "POST",
				url: '/Organismo/Edit',
				data: { id: $('input#IdValor').val(), Accion: "Ver" },
				success: function (data) {
					if (data) {
						showLoading();
						$('#EditContactos').html('');
						$('#EditContactos').html(data);
						$('div#myEditModalContactos').modal('show');
						//hideLoading();
						//$('div#myEditModalContactos.modal.fade.in').attr('style', 'display:block;top:3%');
					}
				}
			});
		}
	}
}

// Bloqueo de Ventanas (por ahora solo las de envio de correo)
function jsShowWindowLoad(mensaje) {
	
	jsRemoveWindowLoad();

	mensaje = "Enviando notificación... ";
	if (mensaje === undefined) mensaje = "Procesando la información<br>Espere por favor";

	height = 20;
	var ancho = 0;
	var alto = 0;

	//obtenemos el ancho y alto de la ventana de nuestro navegador, compatible con todos los navegadores
	if (window.innerWidth == undefined) ancho = window.screen.width;
	else ancho = window.innerWidth;
	if (window.innerHeight == undefined) alto = window.screen.height;
	else alto = window.innerHeight;

	var heightdivsito = alto / 2 - parseInt(height) / 2;

	imgCentro = "<div style='text-align:center;height:" + alto + "px;'><div  style='color:#000;margin-top:" + heightdivsito + "px; font-size:14px;font-weight:bold;color:#428bca; font-family:sans-serif'>" + mensaje + "</div><img  src='../images/Load.gif'  height='80' width='80' ></div>";

	div = document.createElement("div");
	div.id = "WindowLoad";
	div.style.width = ancho + "px";
	div.style.height = alto + "px";
	$("body").append(div);

	input = document.createElement("input");
	input.id = "focusInput";
	input.type = "text";
	$("#WindowLoad").append(input);

	$("#focusInput").focus();
	$("#focusInput").hide();

	$("#WindowLoad").html(imgCentro);

}

// se desbloquea la ventana 
function jsRemoveWindowLoad() {
	$("#WindowLoad").remove();
}

