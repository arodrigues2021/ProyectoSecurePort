var mensaje = "Datos guardados correctamente";


function AltaEditarIncidente(action) {
    var incidentesJson = {
        "Id": $('input#ID01').val(),
        "Id_Puerto": $('input#Puerto01').val(),
        "Id_IIPP": $('input#Instalacion01').val(),
        "Descripcion": $('input#Descripcion01').val(),
        "Tipo": $('input#Tipo01').val(),
        "Fecha" : $('input#Fecha01').val(),
        "Observaciones": $('textarea#Observaciones01').val(),
        "action": action
    };
    var o = eval(incidentesJson);
    if (o.Id_Puerto && o.Descripcion && o.Tipo && o.Fecha && $('input#ValidarAllCombos').val()) {
        $.ajax({
            url: '/Incidentes/AltaEditarIncidente',
            type: "POST",
            data: JSON.stringify({ IncidentesJson: incidentesJson }),
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
                        url: '/Incidentes/Visualizarincidentes',
                        data: { id: $('input#IdValor').val(), Accion: "Ver" },
                        success: function (data) {
                            if (data) {
                                hideLoading();
                                $('#ListIncidentes').html(data);
                            }
                        }
                    });
                } else {
                    showLoading();
                    alert(data.Message, 'Confirm', 0, 25);
                }

            }
        });
    }
}


function VolverIncidentes() {

    $('input#Puerto01').val('');
    $('input#Instalacion01').val('');
    $('div#myModalPracticas.modal.fade.in').attr('style', 'display:none');
    hideLoading();
    $('#DatosIncidentes').html('');
    window.location.reload();

}

function CambiarPuerto() {
    showLoading();
    $.ajax({
        type: "POST",
        url: '/Incidentes/CambiarCentro',
        data: { id: $('input#Puerto01').val() },
        success: function (data) {
            if (data) {
                ValidarPuerto();
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

function ValidarFecha(fecha, campoerror)
{

    if (!ValidarTexto($('input#Fecha01').val())) {
        datosFecha.style.display = "";
        return false;
    }
    else {
        datosFecha.style.display = "none";
        return true;
    }
    //if (fecha.value == "")
    //    $('span#' + campoerror).attr('style', 'display:block;color:#b94a48;');       // campoerror.style.display = "";
    //else
    //    $('span#' + campoerror).attr('style', 'display:none');        //  campoerror.style.display = "none";

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
    if (!ValidacionCombos(document.getElementById('Tipo01'), 'errorTipo'))
        comboValida = false;
    
    return comboValida;
}



//Documentos
function VolverDocIncidente() {

    $.ajax({

        url: '/Incidentes/AsociadosEditIncidentes',
        type: "POST",
        data: { id: 1 },
        success: function (data) {
            if (data) {
                $('#MostrarMensaje').html('Visualizar Incidente');
                $('#myModalUpload').modal('hide');
                $('#DocumentosRefrescar').html(data);
                $('#Edit-page').html('');
                hideLoading();
            }
        }
    });
}

function onChangeEditDocIncidente(e) {
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
        url: '/Incidentes/GuardarDocumento',
        type: "POST",
        data: { DocumentoJson: DocumentoJson },
        success: function (data) {
            if (data.result) {
                $.ajax({
                    type: "POST",
                    url: '/Incidentes/AsociadosEditIncidentes',
                    data: { id: 1 },
                    success: function (data) {
                        if (data) {
                            $('#MostrarMensaje').html('Visualizar Incidentes');
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
        url: '/Incidentes/DeshacerCambios',
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

function VisualizarDocIncidente(Id) {
    $.ajax({
        type: "POST",
        url: '/Incidentes/AsociadosEditIncidentes',
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
        url: '/Incidentes/AsignarInstalacion',
        success: function (data) {
            if (data) {
                hideLoading();
                $('#ListIncidentes').html(data);

            }
        }
    });
}


function onClickInstalacionNivel() {
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
                    url: '/Incidentes/ActualizarInstalacion',
                    data: { id: id },
                    success: function (data) {
                        if (data.result == false) {
                            showLoading();
                            alert(data.Message, 'Eliminar');
                        } else {
                            $('#InstaDisponibles').html(data);
                            $.ajax({
                                type: "POST",
                                url: '/Incidentes/ActualizarInstalacionAsignada',
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

function onClickInstalacionQuitarNivel() {
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
                    url: '/Incidentes/QuitarInstalacion',
                    data: { id: id },
                    success: function (data) {
                        if (data.result == false) {
                            showLoading();
                            alert(data.Message, 'Eliminar');
                        } else {
                            $('#InstaDisponibles').html(data);
                            $.ajax({
                                type: "POST",
                                url: '/Incidentes/ActualizarInstalacionAsignada',
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

function GuardarIncidenteInst() {
    $.ajax({
        url: '/Incidentes/GuardarIncidenteInstalaciones',
        type: "POST",
        data: { id: 1 },
        success: function (data) {
            showLoading();
            $.ajax({
                type: "POST",
                url: '/Incidentes/Visualizarincidentes',
                data: { id: $('input#IdValor').val(), Accion: "Ver" },
                success: function (data) {
                    if (data) {
                        hideLoading();
                        $('#ListIncidentes').html(data);

                    }
                }
            });
        }

    });
}

function DeshacerIncidenteInst() {
    $.ajax({
        url: '/Incidentes/DeshacerIncidenteInstalaciones',
        type: "POST",
        data: { id: 1 },
        success: function (data) {
            if (data) {
                $('#InstaDisponibles').html(data);
                $.ajax({
                    type: "POST",
                    url: '/Incidentes/ActualizarInstalacionAsignada',
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

function VolverIncidenteInst() {
    showLoading();
    $.ajax({
        type: "POST",
        url: '/Incidentes/Visualizarincidentes',
        data: { id: $('input#IdValor').val(), Accion: "Ver" },
        success: function (data) {
            if (data) {
                hideLoading();
                $('#ListIncidentes').html(data);

            }
        }
    });

}


