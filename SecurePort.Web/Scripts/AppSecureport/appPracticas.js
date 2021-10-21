var mensaje = "Datos guardados correctamente";


function AltaEditarPractica(action) {
    var practicasJson = {
        "Id": $('input#ID01').val(),
        "Id_Puerto": $('input#Puerto01').val(),
        "Id_IIPP": $('input#Instalacion01').val(),
        "Descripcion": $('input#Descripcion01').val(),
        "Tipo": $('input#Tipo01').val(),
        "Estado": $('input#Estado01').val(),
        "Ini_Programa" : $('input#IniPrograma01').val(),
        "Fin_Programa": $('input#FinPrograma01').val(),
        "Ini_Planifica": $('input#IniPlanifica01').val(),
        "Fin_Planifica": $('input#FinPlanifica01').val(),
        "Ini_Real": $('input#IniReal01').val(),
        "Fin_Real": $('input#FinReal01').val(),
        "Responsable": $('input#Responsable01').val(),
        "Cuerpos" : $('input#Cuerpos01').val(),
        "Valoracion": $('input#Valoracion01').val(),
        "Ratifico": ($('input#Ratifico01').val() == '1' ? 'true' : 'false'),   // $('input#Ratifico01').val(),
        "Conclusiones": $('textarea#Conclusiones01').val(),
        "Propuesta": $('textarea#Propuesta01').val(),
        "Observaciones": $('textarea#Observaciones01').val(),
        "Validar" :  $('input#Validar').val(),
        "action": action
    };
    var o = eval(practicasJson);
    if (o.Id_Puerto && o.Descripcion && o.Tipo && o.Estado && o.Validar && $('input#ValidarAllCombos').val()) {
        $.ajax({
            url: '/Practicas/AltaEditarPractica',
            type: "POST",
            data: JSON.stringify({ PracticasJson: practicasJson }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function(data) {
                if (data.result == 0) {
                    //$('#myModalCentros').modal('hide');
                    window.location.reload();
                } else if (data.result == 1) {
                    showLoading();
                    $.ajax({
                        type: "POST",
                        url: '/Practicas/VisualizarPracticas',
                        data: { id: $('input#IdValor').val(), Accion: "Ver" },
                        success: function (data) {
                            if (data) {
                                hideLoading();
                                $('#ListPracticas').html(data);
                            }
                        }
                    });
                } else {
                    //$('#myModalCentros').modal('hide');
                    showLoading();
                    alert(data.Message, 'Confirm', 0, 25);
                }

            }
        });
    }
}


function VolverPracticas() {

    $('input#Puerto01').val('');
    $('input#Instalacion01').val('');
    $('div#myModalPracticas.modal.fade.in').attr('style', 'display:none');
    hideLoading();
    $('#DatosPracticas').html('');
    window.location.reload();

}

function CambiarPuerto() {
    showLoading();
    $.ajax({
        type: "POST",
        url: '/Practicas/CambiarCentro',
        data: { id: $('input#Puerto01').val() },
        success: function (data) {
            if (data) {
                ValidarPuerto();
                ValidacionCombos(document.getElementById('Puerto01'), 'errorPuerto');
                hideLoading();
                $('#ComboCentros').html(data);
            }
        }
    });
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

function ValidarDescripcion() {
    if (!ValidarTexto($('input#Descripcion01').val())) {
        datosDescripcion.style.display = "";
        return false;
    }
    else {
        datosDescripcion.style.display = "none";
        return true;
    }
}

function ValidarTipo() {
    if (!ValidarTexto($('input#Tipo01').val())) {
        datosTipo.style.display = "";
        return false;
    }
    else {
        datosTipo.style.display = "none";
        return true;
    }
}

function ValidarEstado() {
    if (!ValidarTexto($('input#Estado01').val())) {
        datosEstado.style.display = "";
        return false;
    }
    else {
        datosEstado.style.display = "none";
        ValidarFecha();
        return true;
    }
}

function ValidarPuerto()
{
    if (!ValidarCombo($('input#Puerto01').val())) {
        datosPuerto.style.display = "";
        return false;
    }
    else {
        datosPuerto.style.display = "none";
        return true;
    }
}

function ValidarCentro()
{
    if (!ValidarCombo($('input#Instalacion01').val())) {
        datosCentro.style.display = "";
        return false;
    }
    else {
        datosCentro.style.display = "none";
        return true;
    }

}

function ValidarFecha()
{
    var estado = $('input#Estado01').val();
    if (estado == 1) {
        if (ValidarMayor($('input#IniPrograma01').val(), $('input#FinPrograma01').val()))
            datosFinValidaPro.style.display = "none";
        else
            datosFinValidaPro.style.display = "";
    } else if(estado == 2) {
        if (ValidarMayor($('input#IniPlanifica01').val(), $('input#FinPlanifica01').val()))
            datosFinValidaPla.style.display = "none";
        else
            datosFinValidaPla.style.display = "";
    } else if (estado == 3) {
        if (ValidarMayor($('input#IniReal01').val(), $('input#FinReal01').val()))
            datosFinValidaRea.style.display = "none";
        else
            datosFinValidaRea.style.display = "";
    }    
}

function CambiarEstado(estado)
{
    if (ValidacionCombos(document.getElementById('Estado01'), 'errorEstado')) {
        showLoading();
        $.ajax({
            type: "POST",
            url: '/Practicas/CambiarEstadoPlan',
            data: { fechaIni: $('input#IniPlanifica01').val(), fechaFin: $('input#FinPlanifica01').val(), Estado: estado.value },
            success: function (data) {
                if (data) {
                    hideLoading();
                    $('#EstadoPlanifica').html(data);
                    $.ajax({
                        type: "POST",
                        url: '/Practicas/CambiarEstadoReal',
                        data: { fechaIni: $('input#IniReal01').val(), fechaFin: $('input#FinReal01').val(), Estado: estado.value },
                        success: function (data) {
                            if (data) {
                                hideLoading();
                                $('#EstadoReal').html(data);
                                $.ajax({
                                    type: "POST",
                                    url: '/Practicas/CambiarEstadoProm',
                                    data: { fechaIni: $('input#IniPrograma01').val(), fechaFin: $('input#FinPrograma01').val(), Estado: estado.value },
                                    success: function (data) {
                                        if (data) {
                                            hideLoading();
                                            $('#EstadoProm').html(data);
                                        }
                                    }
                                });

                            }
                        }
                    });
                }
            }
        });
    }
}


function ValidarMayor(Inicial, final) {
    var fechaInicio = Inicial;
    var fechaFin = final;
    if (fechaFin != "") {
        if (fechaInicio != "") {
            fechaInicio = fechaInicio.split('/');
            fechaFin = fechaFin.split('/');

            fechaInicio = fechaInicio[1].toString() + "/" + fechaInicio[0].toString() + "/" + fechaInicio[2].toString();
            fechaFin = fechaFin[1].toString() + "/" + fechaFin[0].toString() + "/" + fechaFin[2].toString();

            if ((Date.parse(fechaInicio)) > (Date.parse(fechaFin))) {
                $('input#Validar').val("");
                return false;
            }
            else {
                $('input#Validar').val("1");
                return true;
            }
        } else {
            $('input#Validar').val("");
            return false;
        }
    } else {
        $('input#Validar').val("1");
        return true;
    }
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
    if (!ValidacionCombos(document.getElementById('Valoracion01'), 'errorValoracion'))
        comboValida = false;
    if (!ValidacionCombos(document.getElementById('Tipo01'), 'errorTipo'))
        comboValida = false;
    if (!ValidacionCombos(document.getElementById('Estado01'), 'errorEstado'))
        comboValida = false;
    if (!ValidacionCombos(document.getElementById('Ratifico01'), 'errorRatifico'))
        comboValida = false;

    return comboValida;
}




//Documentos
function VolverDocPractica() {
    $.ajax({

        url: '/Practicas/AsociadosEditPracticas',
        type: "POST",
        data: { id: 1 },
        success: function (data) {
            if (data) {
                $('#MostrarMensaje').html('Visualizar Práctica');
                $('#myModalUpload').modal('hide');
                $('#DocumentosRefrescar').html(data);
                $('#Edit-page').html('');
                hideLoading();
            }
        }
    });
}

function onChangeEditDocPractica(e) {
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
        url: '/Practicas/GuardarDocumento',
        type: "POST",
        data: { DocumentoJson: DocumentoJson },
        success: function (data) {
            if (data.result) {
                $.ajax({
                    type: "POST",
                    url: '/Practicas/AsociadosEditPracticas',
                    data: { id: 1 },
                    success: function (data) {
                        if (data) {
                            $('#MostrarMensaje').html('Visualizar Prácticas');
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
        url: '/Practicas/DeshacerCambios',
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

function VisualizarDocPractica(Id) {
    $.ajax({
        type: "POST",
        url: '/Practicas/AsociadosEditPracticas',
        data: { id: Id },
        success: function (data) {
            if (data) {
                $('#MostrarMensaje').html('Visualizar Práctica');
                $('#myModalUpload').modal('hide');
                $('#DocumentosRefrescar').html(data);
                $('#Edit-page').html('');
                hideLoading();
            }
        }
    });

}

// fin documentos

// intalaciones
function InstalacionesVisualizarAccionesChange() {
    var value = this.value();
    $.ajax({
        type: "POST",
        url: '/Practicas/AsignarInstalacion',
        success: function (data) {
            if (data) {
                hideLoading();
                $('#ListPracticas').html(data);

            }
        }
    });
}


function onClickInstalacionPractica() {
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
                    url: '/Practicas/ActualizarInstalacion',
                    data: { id: id },
                    success: function (data) {
                        if (data.result == false) {
                            showLoading();
                            alert(data.Message, 'Eliminar');
                        } else {
                            $('#InstaDisponibles').html(data);
                            $.ajax({
                                type: "POST",
                                url: '/Practicas/ActualizarInstalacionAsignada',
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

function onClickInstalacionQuitarPractica() {
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
                    url: '/Practicas/QuitarInstalacion',
                    data: { id: id },
                    success: function (data) {
                        if (data.result == false) {
                            showLoading();
                            alert(data.Message, 'Eliminar');
                        } else {
                            $('#InstaDisponibles').html(data);
                            $.ajax({
                                type: "POST",
                                url: '/Practicas/ActualizarInstalacionAsignada',
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

function GuardarPracticaInst() {
    $.ajax({
        url: '/Practicas/GuardarPracticaInstalaciones',
        type: "POST",
        data: { id: 1 },
        success: function (data) {
            showLoading();
            $.ajax({
                type: "POST",
                url: '/Practicas/VisualizarPracticas',
                data: { id: $('input#IdValor').val(), Accion: "Ver" },
                success: function (data) {
                    if (data) {
                        hideLoading();
                        $('#ListPracticas').html(data);

                    }
                }
            });
        }

    });
}

function DeshacerPracticaInst() {
    $.ajax({
        url: '/Practicas/DeshacerPracticaInstalaciones',
        type: "POST",
        data: { id: 1 },
        success: function (data) {
            if (data) {
                $('#InstaDisponibles').html(data);
                $.ajax({
                    type: "POST",
                    url: '/Practicas/ActualizarInstalacionAsignada',
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

function VolverPracticaInst() {
    showLoading();
    $.ajax({
        type: "POST",
        url: '/Practicas/VisualizarPracticas',
        data: { id: $('input#IdValor').val(), Accion: "Ver" },
        success: function (data) {
            if (data) {
                hideLoading();
                $('#ListPracticas').html(data);

            }
        }
    });
}



